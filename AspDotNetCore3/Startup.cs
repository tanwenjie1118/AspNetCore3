using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AspDotNetCore3.Extensions;
using AspDotNetCore3.Filters;
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

            // ɨ��binĿ¼�����г���
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

            //services.AddIdentityServer(option =>
            //{
            //    //����ͨ����������ָ����¼·����Ĭ�ϵĵ�½·����/account/login
            //    option.UserInteraction.LoginUrl = "/account/login";

            //});

            // Add redis
            services.AddContext<RedisContext>(options =>
            {
                var constr = Appsettings.app("Cache", "RedisConnection");
                options.UseCache(constr);
            });

            // Add sqlsugar
            services.AddSqlSugar(option =>
            {
                option.ConnectionString = Appsettings.app("Database", "Sqlite", "Conn");
                option.DbType = SqlSugar.DbType.Sqlite;
                option.AutoClose = true;
            });

            // Add MongoDb
            services.AddMongoDbContext<MongoDbContext>((
              Appsettings.app("Database", "Mongodb", "Conn"),
              Appsettings.app("Database", "Mongodb", "Ssl").ToBool(),
              Appsettings.app("Database", "Mongodb", "dbNo"),
              TimeSpan.FromMinutes(1)));

            services.AddHttpContextAccessor();

            // Add authtication handler
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
                var ApiName = Appsettings.app(new string[] { "Startup", "ApiName" });

                typeof(SwaggerApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    // swagger�ĵ�����
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} �ӿ��ĵ�",
                        Description = $"{ApiName} HTTP API " + version,
                        //Contact = new OpenApiContact { Name = ApiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                        //License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                    });
                    // �ӿ�����
                    options.OrderActionsBy(o => o.RelativePath);
                });

                try
                {
                    //��������
                    var xmlPath = Path.Combine(basePath, "AspDotNetCore3.xml");//������Ǹո����õ�xml�ļ���
                    options.IncludeXmlComments(xmlPath, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�

                    //var xmlModelPath = Path.Combine(basePath, "Blog.Core.Model.xml");//�������Model���xml�ļ���
                    //options.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    // log.Error("Blog.Core.xml��Blog.Core.Model.xml ��ʧ�����鲢������\n" + ex.Message);
                }

                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                options.OperationFilter<SecurityRequirementsOperationFilter>();


                // Token�󶨵�ConfigureServices
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
            });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(Appsettings.app("Cache", "RedisConnection"),
                new Hangfire.Redis.RedisStorageOptions()
                {
                    Db = 0,
                }));

            // register profile
            services.AddAutoMapper(assemblies);

            // Add hangfire
            services.AddHangfireServer();

            // Add Task scheduler
            services.AddJobService();

            // Add Dashboard for http reports
            services.AddHttpReportsDashboard(opt =>
            {
                opt.UseHome = false;
            }).UseMySqlStorage();

            // JsonConvert.DefaultSettings = () => new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            services.AddControllersWithViews();
        }

        // autofac container
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // register module
            builder.RegisterAssemblyModules(assemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this is global container
            AutofacContainer.Container = app.ApplicationServices.GetAutofacRoot();

            // store static httpcontext for services 
            app.UseStaticHttpContext();

            // store static automapper for services 
            app.AddStaticAutoMapper();

            // ip access limit
            app.UseIpRateLimiting();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            // start http reports plugin
            app.UseHttpReports();

            // open http report dashboard service
            app.UseHttpReportsDashboard();

            // ConfigureAuthentication(app);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //���ݰ汾���Ƶ��� ����չʾ
                var ApiName = Appsettings.app(new string[] { "Startup", "ApiName" });
                typeof(SwaggerApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });
                // ��swagger��ҳ�����ó������Զ����ҳ�棬�ǵ�����ַ�����д�������������.index.html
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("AspDotNetCore3.index.html");
                c.RoutePrefix = ""; //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
            });

            app.UseMiniProfiler();
            //  .UseIdentityServer()
            app.UseStaticFiles();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
           // app.UseAuthorization();
            
            // open job services
            app.UseJob();

            // open hangfire dashboard service
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureMiddleware(IApplicationBuilder app)
        {
            // ��Use�� make current context to next middleware
            // ��Run�� make current context shutdown and return right now 

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
