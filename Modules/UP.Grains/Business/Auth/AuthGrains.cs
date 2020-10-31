/*********************************************************
* 功能：平台统一身份认证接业务组件
* 描述：
* 作者：贺伟
* 日期：2020/09/19
**********************************************************/

using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UP.Basics;
using UP.Basics.Models;
using UP.Interface.Business.Auth;
using UP.Logics.Business.Auth;
using UP.Models.Business.Auth;

namespace UP.Grains.Business.Auth
{
    /// <summary>
    /// 身份认证逻辑接口
    /// </summary>
    public class AuthGrains : BasicGrains<AuthLogic>, IAuthLogic
    {
        /// <summary>
        /// 平台身份认证
        /// </summary>
        /// <param name="account">登录账户</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public Task<JsonMsg<AuthInfo>> GetUserAuth(string account, string password)
        {
            //返回对象
            var result = JsonMsg<AuthInfo>.Error(null, "平台身份认证失败!");
            if (account.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                result.msg = "账户及密码不能为空";
            }
            try
            {
                result = this.Logic.GetUserAuth(account, password);
            }
            catch (Exception ex)
            {
                result.msg = "平台身份认证发生异常:" + ex.Message;
                Logger.Instance.Error(result.msg, ex);
            }
            return Task.FromResult(result);
        }
    }
}