using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UP.WebRoot
{
    /// <summary>
    /// 安全管理中间件
    /// </summary>
    public class UPSecurityMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDataProtector _dataProtector;


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="next">初始化构造传入的对象</param>
        public UPSecurityMiddleware(RequestDelegate next, IDataProtectionProvider dataProtection)
        {
            this.next = next;
            this._dataProtector = dataProtection.CreateProtector("defaultProtector"); ;
        }

        /// <summary>
        /// 执行对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //设置x-xsss协议头
            context.Response.Headers.Add("X-Xss-Protection", "1");

            //只能本站点嵌入iframe,其它站点不能嵌入
            context.Response.Headers.Add("x-frame-options", "SAMEORIGIN");

            var requestData = new StringBuilder();

            // 将请求/响应 Body 流放到局部变量
            var currentRequest = context.Request.Body;
            var currentResponse = context.Response.Body;

            using (var newRequest = new MemoryStream())
            {


                // 将新的 MemoryStream 赋给 Request/Response Body 并准备重新写入
                context.Request.Body = newRequest;

                // 过滤请求
                await FilterRequest(context, currentRequest, requestData);

                // 开始重新写入当前 Request 流
                using (var writer = new StreamWriter(newRequest))
                {
                    await writer.WriteAsync(requestData.ToString());
                    await writer.FlushAsync();

                    newRequest.Position = 0;

                    // 必须在重写 Request Stream 关闭 Writer 前 可以去掉 using 手动 Dispose
                    await next(context);
                }
            }


            await this.next(context);
        }

        private async Task FilterRequest(HttpContext context, Stream currentRequest, StringBuilder requestData)
        {
            // 过滤 Get 请求中 QueryString 的 Id 值，并解密
            if (context.Request.Method.Equals(HttpMethods.Get, StringComparison.CurrentCultureIgnoreCase))
            {
                requestData.Append(context.Request.QueryString);

                context.Request.QueryString = new QueryString(requestData.ToString());
            }
            // 过滤 Post 请求 Body Stream 中的 Id 值，并加密
            if (context.Request.Method.Equals(HttpMethods.Post, StringComparison.CurrentCultureIgnoreCase))
            {
                var keys = context.Request.Form.Keys;
                foreach (var key in keys)
                {
                    var val = context.Request.Form[key];
                }
            }
        }
    }

    /// <summary>
    /// 安全管理扩展中间件
    /// </summary>
    public static class SecurityHandlingExtensions
    {
        /// <summary>
        /// 使用异常的中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecurityHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UPSecurityMiddleware>();
        }
    }
}
