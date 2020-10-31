using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;

namespace UP.WebRoot
{
    /// <summary>
    /// JWT的扩展中间件
    /// </summary>
    public static class JwtMiddlewareExtensions
    {
        /// <summary>
        /// 权限检查
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void AddJwt(this IServiceCollection services)
        {
            //添加策略鉴权模式
            services.AddAuthentication(s =>
            {
                //添加JWT Scheme
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //添加jwt验证：
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromMinutes(5),

                    ValidateAudience = true,//是否验证Audience
                    ValidAudience = "Audience",//Const.GetValidudience(),//Audience
                    //这里采用动态验证的方式，在重新登陆时，刷新token，旧token就强制失效了
                    AudienceValidator = (m, n, z) =>
                    {
                        return m != null && m.FirstOrDefault().Equals(Const.ValidAudience);
                    },

                    ValidIssuer = "Issuer", //Const.Domain,//Issuer，这两项和前面签发jwt的设置一致
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey))//拿到SecurityKey
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //Jwt注入
            services.AddTransient<IJwt, JwtToken>();
        }
    }
}
