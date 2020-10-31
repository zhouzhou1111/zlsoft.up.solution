/*********************************************************
* 功能：平台统一身份认证接口
* 描述：
* 作者：贺伟
* 日期：2020/09/19
**********************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Collections.Generic;
using UP.Basics;
using UP.Interface.Business.Auth;
using UP.Models.Business.Auth;
using UP.Web.Models.Business.Auth;

namespace UP.Web.Controllers.Business
{
    /// <summary>
    /// 平台统一身份认证
    /// </summary>
    [ApiGroup(ApiGroupNames.Business)]
    public class IdentityController : BasicsController
    {
        /// <summary>
        /// 认证对象
        /// </summary>
        private IJwt _jwt;

        /// <summary>
        /// 注入jwt
        /// </summary>
        /// <param name="jwt"></param>
        public IdentityController(IJwt jwt)
        {
            _jwt = jwt;
        }

        /// <summary>
        /// 平台统一身份认证
        /// </summary>
        /// <param name="model">人员登录信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthInfo), 200)]
        [UPEncryptionAttribute("不需要加密", false, false)]
        [RIPAuthority("平台统一身份认证", "平台统一身份认证，成功则返回token及认证信息", "贺伟", "2020-09-19")]
        public IActionResult UserAuth(AuthPara model)
        {
            //机构人员使用账户登录  Source_key
            var ckUser = $"authFail:{this.userAppLyInfo.sourse}_{model.account}";
            var cnt = CacheManager.Create().Get<int>(ckUser);
            if (cnt >= _jwt.TryCount)
            {
                //登录连续失败设置上限以上，锁定账号30分钟后才能再次登录
                return Json(new ResponseModel(ResponseCode.NotAuthority, $"当前账户连续获取身份令牌失败{_jwt.TryCount}次以上,请30分钟后再尝试."));
            }
            //调用登录
            var authLogic = this.GetInstance<IAuthLogic>();
            var result = new ResponseModel(ResponseCode.Success, "平台统一身份认证成功!");
            try
            {
                var authResult = authLogic.GetUserAuth(model.account, model.password)?.Result;
                //登录成功
                if (authResult.code == ResponseCode.Success)
                {
                    //登录成功 ，登录失败次数清零
                    CacheManager.Create().Remove(ckUser);
                    var loginKey = model.account;
                    //生成jwt
                    Dictionary<string, string> clims = new Dictionary<string, string>();
                    //Source_key
                    clims.Add("sourse", this.userAppLyInfo.sourse.ToString());
                    clims.Add("account", model.account);
                    //获取到一个新的token
                    var value = _jwt.GetToken(clims);
                    //服务器token有效期赋值
                    authResult.data.token_effective_period = Tools.ConvertDateTimeToInt(DateTime.Now.AddHours(2));
                    //将token赋值给登录返回信息
                    authResult.data.token = value;
                    result.data = authResult.data;
                    //token添加到缓存中(设置缓存2小时失效)
                    AddAuthCacheToken(loginKey, result.data.ToJson());
                    //返回结果
                    return Json(result);
                }
                else
                {
                    cnt++;
                    //缓存有效期设置为30分钟
                    CacheManager.Create().Set(ckUser, cnt, new TimeSpan(0, 30, 0));
                    return Json(new ResponseModel(ResponseCode.NotAuthority, $"平台统一身份认证失败,还有{_jwt.TryCount - cnt}次机会!失败原因：{authResult.msg}"));
                }
            }
            catch (Exception ex)
            {
                result.msg = "平台统一身份认证失败：" + ex.Message;
                result.code = ResponseCode.Error.ToInt32();
                Logger.Instance.Error(result.msg, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 通过身份令牌获取平台身份认证信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthInfo), 200)]
        [UPEncryptionAttribute("不需要加密", false, false)]
        [RIPAuthority("通过身份令牌获取平台身份认证信息", "通过身份令牌获取平台身份认证信息，成功则返回token及登录人信息", "贺伟", "2020-09-19")]
        public IActionResult GetUserAuthInfo()
        {
            var result = new ResponseModel(ResponseCode.Success, "获取平台身份认证信息成功!");
            try
            {
                //获取消息头传入的token
                var token = Tools.GetHeaderValue(HttpContext.Request.Headers, "Authorization");
                if (!token.IsNullOrEmpty())
                {
                    token = token.Replace("Bearer ", "").Trim();
                    //登录的应用平台
                    string sourse = "";
                    //登录账户
                    string account = "";
                    //通过token获取请求账户及来源
                    if (_jwt.ValidateToken(token, out Dictionary<string, string> clims))
                    {
                        foreach (var item in clims)
                        {
                            if (item.Key == "sourse")
                            {
                                sourse = item.Value;
                                continue;
                            }
                            else if (item.Key == "account")
                            {
                                account = item.Value;
                                continue;
                            }
                        }
                    }
                    if (!sourse.IsNullOrEmpty() && !account.IsNullOrEmpty())
                    {
                        //从缓存中获取认证的人员信息
                        //Source_key
                        var authUserinfo_key = $"auth:{sourse}:{account}";
                        string userJson = CacheManager.Create().Get<string>(authUserinfo_key);
                        if (!userJson.IsNullOrEmpty())
                        {
                            result.data = Strings.JsonToModel<AuthInfo>(userJson);
                        }
                        else
                        {
                            result.msg = "获取人员信息失败";
                            result.code = ResponseCode.Error.ToInt32();
                        }
                    }
                    else
                    {
                        result.msg = "token认证信息不合法";
                        result.code = ResponseCode.Error.ToInt32();
                    }
                }
            }
            catch (Exception ex)
            {
                result.msg = "获取平台身份认证信息失败：" + ex.Message;
                result.code = ResponseCode.Error.ToInt32();
                Logger.Instance.Error(result.msg, ex);
            }
            return Json(result);
        }
    }
}