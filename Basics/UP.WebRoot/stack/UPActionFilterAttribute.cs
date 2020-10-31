/*********************************************************
* 功能：执行方法过滤器
* 描述：方法过滤器：它会在执行Action方法前后被调用。这个可以在方法中用来处理传递参数和处理方法返回结果。
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using XC.RSAUtil;
using UP.Basics;
using UP.Basics.Security;
using UP.WebRoot.stack;

namespace UP.WebRoot
{
    /// <summary>
    /// 方法过滤器
    /// </summary>
    public class UPActionFilterAttribute : ActionFilterAttribute
    {
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        //请求参数集合
        private List<string> requestPara = new List<string>();

        /// <summary>
        /// 方法执行中判断
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch.Restart();
            stopwatch.Start();

            if (!context.ModelState.IsValid)
            {
                //验证字段的值：验证未通过
                var requeryobj = new ResponseModel(ResponseCode.Error, "验证填写项目失败");
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        requeryobj.msg += error.ErrorMessage + "|";
                    }
                }
                //返回验证失败
                context.Result = new JsonResult(requeryobj);
            }
        }
        /// <summary>
        /// 方法调用完成判断
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //处理跨域问题
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");

            stopwatch.Stop();

            var result = context.Result;
            if (result != null && result is JsonResult resulta)
            {
                context.Result = new JsonResult(resulta.Value);
            }

            //记录日志(只有在高模式下记录)
#if DEBUG
            //LogWriter(context);
#endif
        }

        /*
         * 日志内容：
         * 调用时间======调用方法及控制器========调用人=======消耗时间==========请求IP=====请求方式======请求参数
         */

        private async void LogWriter(ActionExecutedContext context)
        {
            //调用的方法
            var methodName = context.RouteData.Values["Action"]?.ToString(); //context.ActionDescriptor.DisplayName,

            //调用的控制器
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            if (methodName.ToLower() == "" || controllerName.ToLower() == "logger")
            { //查询日志不记录
                return;
            }

            //这是获取自定义参数的方法（从token中获取用户名）
            var auth = context.HttpContext.AuthenticateAsync().Result?.Principal?.Claims;
            var account = string.Empty;
            if (auth != null)
            {//获取当前登录的帐号
                account = auth?.FirstOrDefault(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }

            var obj = new DBLoggerModel
            {
                CallTime = DateTime.Now,

                //获取调用账号（如果是匿名则表示没有登录）
                AccountName = account,

                //请求IP
                RequestIP = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(), // context.HttpContext.Request.Host.Host,

                //调用的方法
                MethodName = methodName, //context.ActionDescriptor.DisplayName,

                //调用的控制器
                ControllerName = controllerName,

                //消耗时间
                SpendTime = stopwatch.ElapsedMilliseconds,

                //请求方式POST,GET
                RequestType = context.HttpContext.Request.Method,

                //请求参数
                RequestParameters = string.Join(",", requestPara)
            };

            //只处理以下几种情况
            if ((context.Result as JsonResult) != null)
            {
                obj.ResponseParameters = QWPlatform.SystemLibrary.Utils.Strings.ObjectToJson((context.Result as JsonResult)?.Value, true, true);
            }
            else if ((context.Result as ContentResult) != null)
            {
                obj.ResponseParameters = QWPlatform.SystemLibrary.Utils.Strings.ObjectToJson((context.Result as ContentResult)?.Content, true, true);
            }
            else if ((context.Result as ObjectResult) != null)
            {
                obj.ResponseParameters = QWPlatform.SystemLibrary.Utils.Strings.ObjectToJson((context.Result as ObjectResult)?.Value, true, true);
            }

            obj.ResponseParameters = obj.ResponseParameters?
                                        .Replace("\\r\\n", "")
                                        .Replace("\\", "")
                                        .Replace(" ", "");

            //写入日志库中
            var logger = await OrleansClient.Instance.CreateInstance<IDBLogger>();
            logger.Add(obj);

            //输出日志
            Logger.Instance.Debug(obj.ToJson());
        }
    }
}