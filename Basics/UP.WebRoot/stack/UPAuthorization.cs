/*********************************************************
* 功能：请求权限过滤器
* 描述：权限过滤器：它在Filter Pipleline中首先运行，并用于决定当前用户是否有请求权限。如果没有请求权限直接返回。
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UP.Basics;

namespace UP.WebRoot
{
    /// <summary>
    /// 请求权限过滤器
    /// </summary>
    public class UPAuthorization : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 认证对象
        /// </summary>
        private IJwt _jwt;

        /// <summary>
        /// 注入jwt
        /// </summary>
        public UPAuthorization(IJwt jwt)
        {
            //设置jwt的验证对象
            _jwt = jwt;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //验证请求是否是Options
            if (context.HttpContext.Request.Method == HttpMethod.Options.ToString())
            {
                context.Result = new JsonResult(new ResponseModel(ResponseCode.Success, "接口Options请求成功"));
            }
            else
            {
                //检查是否有不验证权限的特性标识AllowAnonymousAttribute
                //如果有则不验证权限
                var methodinfo = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo;

                var allowanony = methodinfo.GetCustomAttributes(true).Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
                if (allowanony)
                {
                    //找到到允许不授权AllowAnonymous的特性标识，可以直接访问接口
                    return;
                }
                //自定义授权
                var ripAuth = methodinfo.GetCustomAttributes(typeof(RIPAuthorityAttribute), true).FirstOrDefault() as RIPAuthorityAttribute;

                //获取访问令牌信息
                var headers = context.HttpContext.Request.Headers;
                //登录token
                string token = Tools.GetHeaderValue(headers, "Authorization"); ;
                if (!token.IsNullOrEmpty())
                {
                    token = token.Replace("Bearer ", "").Trim();
                    if (_jwt.ValidateToken(token, out Dictionary<string, string> Clims))
                    {
                        foreach (var item in Clims)
                        {
                            context.HttpContext.Items.Add(item.Key, item.Value);
                        }
                        if (ripAuth != null && ripAuth.IsPublic)
                        {
                            //授权不为空 同时表示这个方法是登录就可以调用则直接允许通过
                            return;
                        }
                        //通过认证后，再检查是否具有授权这个人，访问这个接口？从缓存中获取到这个人员的授权信息
                        var actionObj = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor);
                        //这里是接口名称
                        var methodName = actionObj.MethodInfo.Name;
                        //获取到类型名
                        var className = actionObj.ControllerTypeInfo.FullName;
                        //todo:why-后续判断这个人是否具有访问这个接口的权限
                        //获取到用户登录信息
                        var model = CacheManager.Create().Get<UserModel>(_jwt.LoginAccount);
                        if (model != null)
                        {
                            //#TODO 后续需要实现菜单接口配置
                            return;
                            //超级管理员，可以访问所有的接口
                            if (model.is_super_admin == 1)
                            {
                                return;
                            }
                            //有效的用户信息
                            if (model.actions != null &&
                                model.actions.Exists(p => p.class_name == className && p.method == methodName))
                            {
                                //有正确的授权，可以访问
                                return;
                            }
                        }
                    }
                }
                context.Result = new JsonResult(new ResponseModel(ResponseCode.Forbidden, "对不起您没有访问权限,用户信息过期或无效!"));
            }
        }
    }
}