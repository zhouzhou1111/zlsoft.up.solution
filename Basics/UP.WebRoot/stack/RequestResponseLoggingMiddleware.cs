using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using QWPlatform.SystemLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UP.Basics;

namespace UP.WebRoot.stack
{
    /// <summary>
    /// 拦截请求与响应内容
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private Stopwatch _stopwatch;
        private DBLoggerModel _model;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _stopwatch = new Stopwatch();
            _model = new DBLoggerModel();
        }

        public async Task Invoke(HttpContext context)
        {
            _stopwatch.Restart();
            HttpRequest request = context.Request;

            //获取登录人员账户
            _model.AccountName = context.User.Identity.Name;

            //获取IP地址
            _model.RequestIP = context.Connection.RemoteIpAddress.MapToIPv4().ToString();

            //请求方法
            _model.RequestType = request.Method;

            //方法名称（这里是调用路径）
            _model.MethodName = request.Path.ToString();

            //调用时间
            _model.CallTime = DateTime.Now;

            // 获取请求body内容
            if (request.Method.ToLower().Equals("post"))
            {
                // 启用倒带功能，就可以让 Request.Body 可以再次读取
                request.EnableBuffering();
                Stream stream = request.Body;
                byte[] buffer = new byte[request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);

                //获取请求参数
                _model.RequestParameters = Encoding.UTF8.GetString(buffer);

                //重置数据流位置
                request.Body.Position = 0;
            }
            else if (request.Method.ToLower().Equals("get"))
            {
                _model.RequestParameters = request.QueryString.Value;
            }
            // 获取Response.Body内容
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                try
                {
                    context.Response.Body = responseBody;
                    await _next(context);

                    //获取输出内容
                    _model.ResponseParameters = await GetResponse(context.Response);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error("响应流发生异常", ex);
                    return;
                }
            }
            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
            {
                _stopwatch.Stop();

                //计算消耗时间
                _model.SpendTime = _stopwatch.ElapsedMilliseconds;

                var path = context.Request.Path.ToString();
                if (path == "/" ||
                    path.Contains(".") ||
                    path.ToLower().Contains("swagger") ||
                    path.ToLower().Contains("/home/login")
                    )
                {
                    return Task.CompletedTask;
                }
                else
                {
                    //写入日志库中
                    OrleansClient.Instance.CreateInstance<IDBLogger>().Result.Add(_model);
                    //输出日志
                    Logger.Instance.Debug(_model.ToJson());
                }
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body, Encoding.Default, true).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            text = DeUnicode(text);
            return text;
        }

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DeUnicode(string str)
        {
            //最直接的方法Regex.Unescape(str);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(str, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        }
    }

    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}