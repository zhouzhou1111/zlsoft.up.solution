/*********************************************************
* 功能：平台统一身份认证接业务口层
* 描述：
* 作者：贺伟
* 日期：2020/09/19
**********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UP.Basics;
using UP.Basics.Models;
using UP.Models.Business.Auth;

namespace UP.Interface.Business.Auth
{
    /// <summary>
    /// 身份认证逻辑接口
    /// </summary>
    public interface IAuthLogic : IBasic
    {
        /// <summary>
        /// 平台身份认证
        /// </summary>
        /// <param name="account">登录账户</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        Task<JsonMsg<AuthInfo>> GetUserAuth(string account, string password);
    }
}