using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AspDotNetCore3.Extensions;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.MongoDB;
using Core.Redis;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
//using StackExchange.Profiling.Storage;

namespace AspDotNetCore3
{
    public class Startup
    {
        // private IServiceCollection services;
        public Startup(IConfiguration configuration, IWebHostEnvironment Env)
        {
            Configuration = configuration;
            this.Env = Env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            services.AddIdentityServer(option =>
            {
                //����ͨ����������ָ����¼·����Ĭ�ϵĵ�½·����/account/login
                option.UserInteraction.LoginUrl = "/account/login";

            });

            services.AddContext<RedisContext>(options =>
            {
                var constr = Appsettings.app("Cache", "RedisConnection");
                options.UseCache(constr);
            });
            services.AddMongoDbContext<MongoDbContext>((
              Appsettings.app("Database", "Mongodb", "Conn"),
              Appsettings.app("Database", "Mongodb", "Ssl").ToBool(),
              Appsettings.app("Database", "Mongodb", "dbNo"),
              TimeSpan.FromMinutes(1)));

            services.AddControllers()
                .AddNewtonsoftJson(option =>
                {
                    option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
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
            services.AddIpPolicyRateLimitSetup(Configuration);

            // ADD Swagger
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
        }

        // autofac container
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // ɨ��binĿ¼�����г���
            var dllFilePaths = Directory.GetFiles(AppContext.BaseDirectory, "*.dll");
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var filePath in dllFilePaths)
            {
                if (filePath.Contains("Core.dll")
                    || filePath.Contains("Services.dll")
                    || filePath.Contains("Infrastructure.dll"))
                {
                    var assembly = Assembly.LoadFrom(filePath);
                    assemblies.Add(assembly);
                }
            }

            // ȫ��ע��module
            builder.RegisterAssemblyModules(assemblies.ToArray());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer.Container = app.ApplicationServices.GetAutofacRoot();

            app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //app.MapWhen(context =>
            //{
            //    if (context.Request.Path == "/")
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}, (builder) =>
            //{
            //    builder.Run(async httpcontext
            //       =>
            //   {
            //       httpcontext.Response.StatusCode = 200;
            //       httpcontext.Response.ContentType = "application/json; charset=utf-8";
            //       await httpcontext.Response.WriteAsync("Hello World");
            //   });
            //});

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
                //  c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Blog.Core.index.html");//���������MiniProfiler�������ܼ�صģ������£���������AOP�Ľӿ����ܷ�����������㲻��Ҫ��������ʱ��ע�͵�����Ӱ���֡�
                c.RoutePrefix = ""; //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
            });

            app.UseMiniProfiler()
                   //  .UseIdentityServer()
                   .UseStaticFiles()
                   .UseRouting()
                   .UseHttpsRedirection()
                   .UseAuthorization()
                   .UseEndpoints(endpoints =>
                   {
                       endpoints.MapControllers();
                   });
        }
    }
}
