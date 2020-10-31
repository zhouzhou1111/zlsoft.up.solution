/*********************************************************
* 功能：webapi请求接口
* 描述：
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/
using RestSharp;
using System;

namespace UP.Basics
{
    /// <summary>
    /// restsharp定义接口
    /// </summary>
    public interface IRestSharp
    {
        /// 同步执行方法
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        IRestResponse Execute(IRestRequest request);

        /// <summary>
        /// 同步执行方法
        /// </summary>
        /// <typeparam name="T">返回值</typeparam>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        T Execute<T>(IRestRequest request) where T : new();

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse> callback);

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>> callback) where T : new();
    }
}
