/*********************************************************
 * 功能：结果过滤器设置
 * 描述：结果过滤器：它可以在执行Action结果之前执行，且执行Action成功后执行，使用逻辑必须围绕view或格式化执行结果
 * 作者：王海洋
 * 日期：2019-11-22
 *********************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QWPlatform.SystemLibrary;

namespace UP.WebRoot
{
    /// <summary>
    /// 结果过滤器
    /// </summary>
    public class UPResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //var objectResult = context.Result as ObjectResult;
            //if (objectResult != null)
            //{
            //    Logger.Instance.Info(objectResult.Value.ToJson());
            //}
        }
    }
}
