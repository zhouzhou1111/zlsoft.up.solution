using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UP.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.WebRoot
{
    /// <summary>
    /// 发生错误处理的中间件
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Response.OnStarting(state =>
                {
                    return Task.FromResult(0);
                }, context);

                await next(context);
            }
            catch (Exception ex)
            {
                //var httpContext = (HttpContext)state;
                context.Response.ContentType = "application/json;charset=utf-8";

                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {

                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 400)
                {
                    msg = $"错误的请求,{context.Request.Path}";
                    context.Response.ContentType = "application/json;charset=utf-8";
                }
                else if (statusCode == 401)
                {
                    msg = $"未授权,{context.Request.Path}";
                    context.Response.ContentType = "application/json;charset=utf-8";
                }
                else if (statusCode == 404)
                {
                    msg = $"未找到服务,{context.Request.Path}";
                    context.Response.ContentType = "application/json;charset=utf-8";
                }
                else if (statusCode == 502)
                {
                    msg = $"请求错误,请求路径:{context.Request.Path}";
                    context.Response.ContentType = "application/json;charset=utf-8";
                }
                else if (statusCode != 200 && statusCode!= 302)
                {
                    msg = $"未知错误,请求路径:{context.Request.Path}";
                    context.Response.ContentType = "application/json;charset=utf-8";
                }

                //如果消息不为空，则进度异常处理
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var model = new ResponseModel(statusCode, msg);
            var result = JsonConvert.SerializeObject(model);

            //修改状态码，表示连接成功
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync(result);
        }
    }

    //扩展中间件
    public static class ErrorHandlingExtensions
    {
        /// <summary>
        /// 使用异常的中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
