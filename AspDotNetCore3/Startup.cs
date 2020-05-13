using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AspDotNetCore3.Extensions;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Core.MongoDB;
using Core.Redis;
using Hangfire;
using Hangfire.Common;
using Infrastructure;
using Infrastructure.Domain;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Singleton;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;
//using StackExchange.Profiling.Storage;

namespace AspDotNetCore3
{
    public class Startup
    {
        private readonly Assembly[] assemblies;
        public Startup(IConfiguration configuration, IWebHostEnvironment Env)
        {
            Configuration = configuration;
            this.Env = Env;

            // scan all dlls in this solution
            var dllFilePaths = Directory.GetFiles(AppContext.BaseDirectory, "*.dll");
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var filePath in dllFilePaths)
            {
                if (filePath.Contains("Core.dll")
                    || filePath.Contains("Services.dll")
                    || filePath.Contains("Infrastructure.dll")
                    || filePath.Contains("Applications.dll"))
                {
                    var assembly = Assembly.LoadFrom(filePath);
                    assemblies.Add(assembly);
                }
            }

            this.assemblies = assemblies.ToArray();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Env.ContentRootPath));

            services.AddHttpReports().UseMySqlStorage();

            services.Configure<GzipCompressionProviderOptions>(
            options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            // compression for response
            services.AddResponseCompression(opt =>
            {
                opt.Providers.Add<GzipCompressionProvider>();
            });

            //services.AddIdentityServer(option =>
            //{
            //    // default url 
            //    option.UserInteraction.LoginUrl = "/account/login";
            //});

            // Add redis
            services.AddContext<RedisContext>(options =>
        {
            var constr = Appsettings.Get("Cache", "RedisConnection");
            options.UseCache(constr);
        });

            // Add SignalR
            services.AddSignalR();

            // Add SqlSugar
            services.AddSqlSugar(option =>
            {
                option.ConnectionString = Appsettings.Get("Database", "Sqlite", "Conn");
                option.DbType = SqlSugar.DbType.Sqlite;
                option.AutoClose = true;
            });

            // Add MongoDb
            services.AddMongoDbContext<MongoDbContext>((
              Appsettings.Get("Database", "Mongodb", "Conn"),
              Appsettings.Get("Database", "Mongodb", "Ssl").ToBool(),
              Appsettings.Get("Database", "Mongodb", "dbNo"),
              TimeSpan.FromMinutes(1)));

            // Add Http context accessor
            services.AddHttpContextAccessor();

            // Add Authtication handler
            services.AddAuthenticationCore(options =>
            {
                options.DefaultScheme = "myScheme";
                options.AddScheme<MyHandler>("myScheme", "demo scheme");
            });

            // Add Controllers for API
            services.AddControllers(c =>
            {
                //  c.Filters.Add(typeof(ExceptionFilter));
            })
            .AddNewtonsoftJson(option =>
            {
                // Configures Newtonsoft.Json specific features such as input and output formatters.
                option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Add Memory Cache
            services.AddMemoryCache();

            // Add MiniProfiler services
            // If using Entity Framework Core, add profiling for it as well (see the end)
            // Note .AddMiniProfiler() returns a IMiniProfilerBuilder for easy Intellisense
            services.AddMiniProfiler(options =>
            {
                // ALL of this is optional. You can simply call .AddMiniProfiler() for all defaults
                // Defaults: In-Memory for 30 minutes, everything profiled, every user can see

                // Path to use for profiler URLs, default is /mini-profiler-resources
                options.RouteBasePath = "/profiler";

                // Control storage - the default is 30 minutes
                // (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
                //options.Storage = new SqlServerStorage("Data Source=.;Initial Catalog=MiniProfiler;Integrated Security=True;");

                // Control which SQL formatter to use, InlineFormatter is the default
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

                // To control authorization, you can use the Func<HttpRequest, bool> options:
                options.ResultsAuthorize = request => !Program.DisableProfilingResults;
                //options.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;

                // To control which requests are profiled, use the Func<HttpRequest, bool> option:
                //options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

                // Profiles are stored under a user ID, function to get it:
                //options.UserIdProvider =  request => MyGetUserIdFunction(request);

                // Optionally swap out the entire profiler provider, if you want
                // The default handles async and works fine for almost all applications
                //options.ProfilerProvider = new MyProfilerProvider();

                // Optionally disable "Connection Open()", "Connection Close()" (and async variants).
                //options.TrackConnectionOpenClose = false;

                // Enabled sending the Server-Timing header on responses
                options.EnableServerTimingHeader = true;

                options.IgnoredPaths.Add("/lib");
                options.IgnoredPaths.Add("/css");
                options.IgnoredPaths.Add("/js");

            }).AddEntityFramework();

            // Add IP Rate Limit
            services.AddIpPolicyRateLimit(Configuration);

            // Add Swagger
            services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;
                //var basePath2 = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var ApiName = Appsettings.Get(new string[] { "Startup", "ApiName" });

                typeof(SwaggerApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    // swaggerÎÄµµÅäÖÃ
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} API Document",
                        Description = $"{ApiName} HTTP API " + version,
                    });

                    options.OrderActionsBy(o => o.RelativePath);
                });

                try
                {
                    var xmlPath = Path.Combine(basePath, "AspDotNetCore3.xml");//xml doc
                    options.IncludeXmlComments(xmlPath, true);//

                    //var xmlModelPath = Path.Combine(basePath, "Model.xml");//the model xml
                    //options.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {

                }

                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();

                // Token binding to ConfigureServices
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Bearer {token}",
                    Name = "Authorization",//jwt token name
                    In = ParameterLocation.Header,// where
                    Type = SecuritySchemeType.ApiKey // type
                });
            });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(Appsettings.Get("Cache", "RedisConnection"),
                new Hangfire.Redis.RedisStorageOptions()
                {
                    Db = 0,
                }));

            // Add Register profile
            services.AddAutoMapper(assemblies);

            // Add Hangfire
            services.AddHangfireServer();

            // Add Task scheduler
            services.AddJobService();

            // Add Dashboard for http reports
            services.AddHttpReportsDashboard(opt =>
            {
                opt.UseHome = false;
            }).UseMySqlStorage();

            // Add Route and Views for http reports
            services.AddControllersWithViews();
        }

        // Autofac container
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register module
            builder.RegisterAssemblyModules(assemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.MapWhen(context =>
            {
                Console.WriteLine("==================> Current request path is " + context.Request.Path);
                if (context.Request.Path == "/" || context.Request.Path == "/index.html")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            },
             builder =>
             builder.Run(
              (context) =>
              {
                  context.Response.Redirect("/swagger/");
                  return Task.CompletedTask;
              }));

            // Save to global container
            AutofacContainer.Container = app.ApplicationServices.GetAutofacRoot();

            // Add static httpcontext for services 
            app.UseStaticHttpContext();

            // Add static automapper for services 
            app.AddStaticAutoMapper();

            // Active ip access limit
            app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                // according to version
                var ApiName = Appsettings.Get(new string[] { "Startup", "ApiName" });
                typeof(SwaggerApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });

                // set swagger default page to user defined page £º{SolutionName}.index.html
                // c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("AspDotNetCore3.index.html");
                // c.RoutePrefix = "";
            });

            // Use mini profiler
            app.UseMiniProfiler();

            // .UseIdentityServer()

            // Use static files
            app.UseStaticFiles();

            // Use routing
            app.UseRouting();

            app.UseHttpsRedirection();

            // Authentication(
            app.UseAuthentication();

            // Authorization
            app.UseAuthorization();

            // Compression
            app.UseResponseCompression();

            // Open user defined job services
            app.UseJob();

            // Open hangfire dashboard service
            // if release authorization filter must return true
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            // Active http reports plugin
            app.UseHttpReports();

            // Active http report dashboard service
            app.UseHttpReportsDashboard();

            // UseCors
            app.UseCors("cors");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MyHub>("/myHub");
                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureMiddleware(IApplicationBuilder app)
        {
            // ¡¾Use¡¿ make current context to next middleware
            // ¡¾Run¡¿ make current context shutdown and return right now 

            app.Map("/login", builder =>
             builder.Use(next =>
             {
                 return async (context) =>
                 {
                     var claimIdentity = new ClaimsIdentity();
                     claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "Hal"));
                     await context.SignInAsync("myScheme", new ClaimsPrincipal(claimIdentity));
                     await next(context);
                 };
             }));

            app.Map("/resource",
                builder =>
                builder.Run(
                async (context) =>
                    await context.Response.WriteAsync("Hello, ASP.NET Core!"))
                );

            app.Map("/",
                builder =>
                builder.Run(
                 (context) =>
                 {
                     context.Response.Redirect("/swagger");
                     return Task.CompletedTask;
                 }));

            // use middleware
            app.Use(async (context, next) =>
            {
                var user = context.User;
                if (user?.Identity?.IsAuthenticated ?? false)
                {
                    if (user.Identity.Name != "Hal") await context.ForbidAsync("myScheme");
                    else await next();
                }
                else
                {
                    await context.ChallengeAsync("myScheme");
                }
            });
        }
    }
}
