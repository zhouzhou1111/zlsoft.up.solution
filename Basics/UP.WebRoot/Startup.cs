using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Apply;
using UP.WebRoot.stack;

namespace UP.WebRoot
{
    public class Startup : BaseController
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            try
            {
                //初始化系统配置信息
                var appItems = this.Query<AppLyInfo>()
                     .Where("数据标识", 1)
                     .GetModelList();
                if (appItems != null && appItems.Any())
                {
                    var userinfo_key = "applyitems";
                    var obj = CacheManager.Create().Set(userinfo_key, appItems);
                }

                Logger.Instance.Info("初始化系统配置信息成功");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("初始化系统配置信息失败", ex);
            }
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///配置服务 This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //设置了httponly
            services.AddAuthentication().AddCookie(opts =>
            {
                opts.Cookie.HttpOnly = true;
            });

            //添加并使用缓存
            services.AddMemoryCache();

            //注入其它web的依赖项目
            services.AddWebDi();

            // json 格式化
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // 忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //添加会话
            services.AddSession();

            //使用Jwt验证方式
            services.AddJwt();

            Action<MvcOptions> filters = new Action<MvcOptions>(r =>
            {
                r.Filters.Add(typeof(UPExceptionFilterAttribute));
                r.Filters.Add(typeof(UPActionFilterAttribute));
                r.Filters.Add(typeof(UPResultFilterAttribute));
                r.Filters.Add(typeof(UPAuthorization));
                //r.EnableEndpointRouting = false;
            });

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                    .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            //配置添加跨域访问
            services.AddCors();

            //注册全局过滤器
            services.AddMvc(filters)
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //禁用默认验证行为，使用自定义验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //注册swagger的文档
            services.AddSwagger();
        }

        /// <summary>
        ///  配置中间件 This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 本地化区域设置，其目的是解决在CentOs下获取日期格式问题
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
             {
                 new CultureInfo("zh-CN"){
                    DateTimeFormat = new DateTimeFormatInfo
                    {
                        ShortDatePattern = "yyyy-MM-dd",
                        ShortTimePattern = "HH:mm:ss",
                        AMDesignator = "",
                        PMDesignator = ""
                    }
                 },
             };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //异常处理
            app.UseErrorHandling();


            //使用swagger作为接口的API
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "医共体信息平台接口 V1");
                c.RoutePrefix = "swagger";

                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    c.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                });
                c.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "无分组");
            });

            //使用session
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            //使用鉴权
            app.UseAuthentication();

            //使用跨域
            app.UseCors(RuntimeHelper.MyAllowSpecificOrigins);

            app.UseAuthorization();

            //api接口请求参数加解密
            app.DelegaMessageHandler();

            //如果项目开启了终结点路由（Endpoint），MVC不会为AllowAnonymous特性自动添加AllowAnonymousFilter
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //默认启动页面
            app.Run(ctx =>
            {
                ctx.Response.Redirect("/swagger/index.html"); //可以支持虚拟路径或者index.html这类起始页.
                return Task.FromResult(0);
            });
        }
    }
}