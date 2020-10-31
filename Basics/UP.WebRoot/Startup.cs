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
                //��ʼ��ϵͳ������Ϣ
                var appItems = this.Query<AppLyInfo>()
                     .Where("���ݱ�ʶ", 1)
                     .GetModelList();
                if (appItems != null && appItems.Any())
                {
                    var userinfo_key = "applyitems";
                    var obj = CacheManager.Create().Set(userinfo_key, appItems);
                }

                Logger.Instance.Info("��ʼ��ϵͳ������Ϣ�ɹ�");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("��ʼ��ϵͳ������Ϣʧ��", ex);
            }
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///���÷��� This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //������httponly
            services.AddAuthentication().AddCookie(opts =>
            {
                opts.Cookie.HttpOnly = true;
            });

            //��Ӳ�ʹ�û���
            services.AddMemoryCache();

            //ע������web��������Ŀ
            services.AddWebDi();

            // json ��ʽ��
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // ����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // ����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //��ӻỰ
            services.AddSession();

            //ʹ��Jwt��֤��ʽ
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

            //������ӿ������
            services.AddCors();

            //ע��ȫ�ֹ�����
            services.AddMvc(filters)
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //����Ĭ����֤��Ϊ��ʹ���Զ�����֤
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //ע��swagger���ĵ�
            services.AddSwagger();
        }

        /// <summary>
        ///  �����м�� This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ���ػ��������ã���Ŀ���ǽ����CentOs�»�ȡ���ڸ�ʽ����
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


            //�쳣����
            app.UseErrorHandling();


            //ʹ��swagger��Ϊ�ӿڵ�API
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "ҽ������Ϣƽ̨�ӿ� V1");
                c.RoutePrefix = "swagger";

                //����ApiGroupNames����ö��ֵ���ɽӿ��ĵ���Skip(1)����ΪEnum��һ��FieldInfo�����õ�һ��Intֵ
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //��ȡö��ֵ�ϵ�����
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    c.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                });
                c.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "�޷���");
            });

            //ʹ��session
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            //ʹ�ü�Ȩ
            app.UseAuthentication();

            //ʹ�ÿ���
            app.UseCors(RuntimeHelper.MyAllowSpecificOrigins);

            app.UseAuthorization();

            //api�ӿ���������ӽ���
            app.DelegaMessageHandler();

            //�����Ŀ�������ս��·�ɣ�Endpoint����MVC����ΪAllowAnonymous�����Զ����AllowAnonymousFilter
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Ĭ������ҳ��
            app.Run(ctx =>
            {
                ctx.Response.Redirect("/swagger/index.html"); //����֧������·������index.html������ʼҳ.
                return Task.FromResult(0);
            });
        }
    }
}