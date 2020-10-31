/*********************************************************
* 功能：资源过程器,做缓存使用
* 描述：这个ResourceFiltersAttribute是最适合做缓存了,在这里做缓存有什么好处?因为这个OnResourceExecuting是在控制器实例化之前运营，如果能再这里获取ViewReuslt就不必实例化控制器，在走一次视图了，提升性能
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UP.WebRoot
{
    /// <summary>
    /// 资源过程器,做缓存使用 
    /// </summary>
    public class UPResourceFilterAttribute :IResourceFilter 
    {
        private static readonly Dictionary<string, object> _Cache = new Dictionary<string, object>();
        private string _cacheKey;

        /// <summary>
        /// 这个方法会在控制器实例化之前之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();//这个是请求地址，它肯定是指向的视图
            if (_Cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _Cache[_cacheKey] as ViewResult;
                if (cachedValue != null)
                {
                    context.Result = cachedValue;
                }
            }
        }
        /// <summary>
        /// 这个方法是是Action的OnResultExecuted过滤器执行完之后在执行的（每次执行完Action之后得到就是一个ViewResult）
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();//这个是请求地址
            if (!string.IsNullOrEmpty(_cacheKey) && !_Cache.ContainsKey(_cacheKey))
            {
                //因为这个方法是是Action的OnResultExecuted过滤器执行完之后在执行的，所以context.Result必然有值了，这个值就是Action方法执行后得到的ViewResult
                var result = context.Result as ViewResult;
                if (result != null)
                {
                    _Cache.Add(_cacheKey, result);
                }
            }
        }
    }
}
