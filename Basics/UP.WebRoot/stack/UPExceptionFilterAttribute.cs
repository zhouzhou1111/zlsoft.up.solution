/*********************************************************
* 功能：异常处理器
* 描述：异常过滤器：被应用全局策略处理未处理的异常发生前异常被写入响应体
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/
using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QWPlatform.SystemLibrary;
using UP.Basics;

namespace UP.WebRoot
{
    /// <summary>
    /// 异常处理基础类
    /// </summary>
    public class UPExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _moprovider;
        public UPExceptionFilterAttribute(IModelMetadataProvider moprovider)
        {
            this._moprovider = moprovider;
        }


        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            string controllerName = (string)context.RouteData.Values["controller"];
            string actionName = (string)context.RouteData.Values["action"];

            //记录日志
            var msg = string.Format("访问控制器:{0},方法:{1},出现错误", controllerName, actionName);
            Logger.Instance.Error(msg, exception);

            //异常处理
            var model = new ResponseModel(ResponseCode.Error, msg, null);
            context.HttpContext.Response.Headers.Add("Content-Type", new Microsoft.Extensions.Primitives.StringValues("applaction/json"));
            context.HttpContext.Response.WriteAsync(model.ToJson() , Encoding.UTF8);
            context.ExceptionHandled = true;//异常已经处理，不要再次处理了
        }

        //判断是否为ajax请求
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }
}
