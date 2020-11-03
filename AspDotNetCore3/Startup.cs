using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hal.Core.Extensions;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Hal.Core.Entityframework;
using Hal.Core.MongoDB;
using Hal.Core.Redis;
using Hangfire;
using Hal.Infrastructure.Configuration;
using Hal.Infrastructure.Domain;
using Hal.Infrastructure.Extensions;
using Hal.Infrastructure.Filters;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Hal.Infrastructure.Constant;
using Hal.Tasks;
using Hal.Core.Web;
//using StackExchange.Profiling.Storage;

namespace Hal.AspDotNetCore3
{
    public class Startup
    {
        private readonly Assembly[] assemblies;
        public Startup(IConfiguration configuration, IWebHostEnvironment Env)
        {
            Configuration = configuration;
            this.Env = Env;

            // scan all dlls in this solution
            var dllFilePaths = Directory.GetFiles(AppContext.BaseDirectory, SystemConstant.AllDll);
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var filePath in dllFilePaths)
            {
                if (filePath.Contains(SystemConstant.Name, StringComparison.OrdinalIgnoreCase))
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
            // Get config model from appsetting.{environment}.json
            var suopt = Configuration.BindConfig<StartupOption>();
            var dbopt = Configuration.BindConfig<DatabaseOption>();
            var cacheOpt = Configuration.BindConfig<CacheOption>();

            // Add Option for service
            services.AddConfig<StartupOption>(Configuration);
            services.AddConfig<DatabaseOption>(Configuration);
            services.AddConfig<CacheOption>(Configuration);

            // Add Health check
            services.AddHealthChecks();

            // Add Httpreports with mysql
            services.AddHttpReports().UseMySqlStorage();

            // Add Compression
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

            // Add redis
            services.AddContext<RedisContext>(options =>
            {
                var constr = cacheOpt.RedisConnection;
                options.UseCache(constr);
            });

            // Add SignalR
            services.AddSignalR();

            // Add Dapper
            services.AddDapper(option => option.UseMysql(dbopt.MySql.Conn));

            // Add SqlSugar
            services.AddSqlSugar(option =>
            {
                //option.ConnectionString = dbopt.Sqlite.Conn;
                //option.DbType = SqlSugar.DbType.Sqlite;
                //option.AutoClose = true;
                option.ConnectionString = dbopt.MySql.Conn;
                option.DbType = SqlSugar.DbType.MySql;
                option.AutoClose = true;
            });

            // Add EFCore 
            services.AddDbContext<MyDbContext>(builder =>
            {
                var conn = dbopt.MySql.Conn;
                builder.UseMySQL(conn).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Add MongoDb
            services.AddMongoDbContext<MongoDbContext>((
              dbopt.Mongodb.Conn,
              dbopt.Mongodb.Ssl.ToBool(),
              dbopt.Mongodb.DbNo,
              TimeSpan.FromMinutes(1)));



            services.AddCorsPolicy();

            // Add Authtication handler
            // cookie
            //services.AddAuthenticationCore(options =>
            //{
            //    options.DefaultScheme = "myScheme";
            //    options.AddScheme<MyHandler>("myScheme", "demo scheme");
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // key
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(suopt.Jwt.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = suopt.Jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = suopt.Jwt.Audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
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
            // If using Entity Framework Hal.Core, add profiling for it as well (see the end)
            // Note .AddMiniProfiler() returns a IMiniProfilerBuilder for easy Intellisense
            services.AddMiniProfiler((opt) =>
                     opt.RouteBasePath = SystemConstant.MiniProfiler
                ).AddEntityFramework();

            // Add IP Rate Limit
            services.AddIpPolicyRateLimit(Configuration);

            // Add Swagger
            services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;
                //var basePath2 = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var ApiName = suopt.ApiName;

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
                    var xmlPath = Path.Combine(basePath, SystemConstant.WebDllName);//xml doc
                    options.IncludeXmlComments(xmlPath, true);//

                    //var xmlModelPath = Path.Combine(basePath, "Model.xml");//the model xml
                    //options.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception)
                {
                }

                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.DocumentFilter<HiddenApiFilter>();

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
                .UseRedisStorage(cacheOpt.RedisConnection,
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            var suopt = app.ApplicationServices.GetService<StartupOption>();

            // health check return 200
            app.UseHealthChecks(SystemConstant.Health, new HealthCheckOptions()
            {
                ResultStatusCodes =
            {
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
            });

            // UseCors
            app.UseCors(SystemConstant.CorsPolicy);

            // Register Consul to Consul Node
            app.RegisterConsul(lifetime, suopt.ConsulService);

            // Save to global container
            AutofacContainer.Container = app.ApplicationServices.GetAutofacRoot();

            // Add static automapper for services 
            app.AddStaticAutoMapper();

            // Active ip access limit
            app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use Swagger
            app.UseSwagger();

            // Use SwaggerUi
            app.UseSwaggerUI(c =>
            {
                // according to version
                var ApiName = suopt.ApiName;
                typeof(SwaggerApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });

                // set swagger default page to user defined page £º{SolutionName}.index.html
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream(SystemConstant.SwaggerIndex);
                c.RoutePrefix = "";
            });

            // Use static files
            app.UseStaticFiles();

            // Use routing
            app.UseRouting();

            // Use Https
            app.UseHttpsRedirection();

            // using Microsoft.AspNetCore.HttpOverrides;
            // https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-3.1#configure-nginx
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticHttpContext();

            // Authentication
            app.UseAuthentication();

            // Authorization
            app.UseAuthorization();

            // Use mini profiler
            app.UseMiniProfiler();

            // Compression
            app.UseResponseCompression();

            // Open user defined job services
            app.UseJob();

            // Open hangfire dashboard service
            // if release authorization filter must return true
            app.UseHangfireDashboard(
                SystemConstant.Hangfire
                , new DashboardOptions
                {
                    //Authorization = new[] { new HangfireAuthorizationFilter() }
                });

            // Active http reports plugin
            app.UseHttpReports();

            // Active http report dashboard service
            //app.UseHttpReportsDashboard();

            app.UseEndpoints(endpoints =>
            {
                // API endpoints
                endpoints.MapControllers();

                // HUB endpoints
                endpoints.MapHub<MyHub>(SystemConstant.SignalRHub);

                // MVC endpoints
                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");

                // Hangfire endpoints
                endpoints.MapHangfireDashboard();
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
                    await context.Response.WriteAsync("Hello, ASP.NET Hal.Core!"))
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


            //app.MapWhen(context =>
            //{
            //    Console.WriteLine("==================> Current request path is " + context.Request.Path);
            //    if (context.Request.Path == "/" || context.Request.Path == "/index.html")
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //},
            // builder =>
            // builder.Run(
            //  (context) =>
            //  {
            //      context.Response.Redirect("/swagger/");
            //      return Task.CompletedTask;
            //  }));

        }
    }
}
