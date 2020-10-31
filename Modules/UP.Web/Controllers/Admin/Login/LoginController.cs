using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Collections.Generic;
using UP.Basics;
using UP.Basics.Security;
using UP.Interface.Admin.Login;
using UP.Models.Admin.Login;
using UP.Models.DB.RoleRight;
using UP.Web.Models.Admin.Login;

namespace UP.Web.Controllers.Admin.Login
{
    /// <summary>
    /// PC登录接口
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class LoginController : BasicsController
    {
        /// <summary>
        /// 认证对象
        /// </summary>
        private IJwt _jwt;

        /// <summary>
        /// 注入jwt
        /// </summary>
        /// <param name="jwt"></param>
        public LoginController(IJwt jwt)
        {
            _jwt = jwt;
        }

        /// <summary>
        /// 人员PC端-登录
        /// </summary>
        /// <param name="model">人员PC端-登录入参</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginInfoModel), 200)]
        [RIPAuthority("人员PC端-登录", "人员PC端-登录，成功则返回token及登录人信息", "胡家源", "2020-09-10")]
        public IActionResult Login(LoginPara model)
        {
            //PC端医生使用手机号登录  Source_UserType_key
            var ckUser = $"loginFail:{UserSource.PC.ToInt32()}_{UserType.医生.ToInt32()}_{model.account_num}";
            var cnt = CacheManager.Create().Get<int>(ckUser);
            if (cnt >= _jwt.TryCount)
            {
                //登录连续失败设置上限以上，锁定账号30分钟后才能再次登录
                return Json(new ResponseModel(ResponseCode.NotAuthority, $"当前账户连续登录失败{_jwt.TryCount}次以上,请30分钟后再尝试."));
            }
            //调用登录
            var loginInfo = this.GetInstance<ILogin>();
            var result = new ResponseModel(ResponseCode.Success, "人员登录成功!");
            try
            {
                var logicResult = loginInfo.DoctorLogin(model.account_num, model.password)?.Result;
                //登录成功
                if (logicResult.code == ResponseCode.Success)
                {
                    //登录成功 ，登录失败次数清零
                    CacheManager.Create().Remove(ckUser);
                    var loginKey = model.account_num;
                    //生成jwt
                    Dictionary<string, string> clims = new Dictionary<string, string>();
                    //Source_UserType_key
                    clims.Add("userName", $"userinfo:{UserSource.PC.ToInt32()}_{UserType.医生.ToInt32()}_{model.account_num}");
                    //获取到一个新的token
                    var value = _jwt.GetToken(clims);
                    //token添加到缓存中(设置缓存2小时失效)
                    AddLoginCacheToken(loginKey, value, UserType.医生, UserSource.PC);
                    //初始化一次用户登录信息
                    var accountObj = this.GetInstance<ISysUser>();
                    var userModel = accountObj.GetAccountInfo(model.account_num)?.Result;
                    //输出信息返回token
                    logicResult.data.is_super_admin = userModel.is_super_admin;
                    //将token赋值给登录返回信息
                    logicResult.data.token = value;
                    result.data = logicResult.data;
                    //返回结果
                    return Json(result);
                }
                else
                {
                    cnt++;
                    //缓存有效期设置为30分钟
                    CacheManager.Create().Set(ckUser, cnt, new TimeSpan(0, 30, 0));
                    return Json(new ResponseModel(ResponseCode.NotAuthority, $"登录失败,还有{_jwt.TryCount - cnt}次机会!失败原因：{logicResult.msg}"));
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "人员登录失败：" + ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 获取登录验证码(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [UPEncryptionAttribute("接口请求参数不需要加密，输出参数也不需要加密", false, false)]
        [RIPAuthority("生成验证码", "医生PC端", "胡家源", "2020-09-10")]
        public IActionResult GetCode()
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询成功!");

            //调用图片码
            string randomCode = SecurityCode.CreateRandomCode(4);
            if (!string.IsNullOrEmpty(randomCode))
            {
                //字符串加缓存
                string keys = "ImageCode_Login" + DateTime.Now.ToString("yyyyMMddHHmmssfffffff");//Guid.NewGuid()
                TimeSpan ts1 = new TimeSpan(0, 5, 0);
                CacheManager.Create().Set(keys, randomCode, ts1);
                //读取图片
                byte[] imgCode = SecurityCode.CreateValidateGraphic(randomCode);
                string base64str = Convert.ToBase64String(imgCode);

                var ImagCode = new
                {
                    imgcode = "data:image/png;base64," + base64str,
                    imgkey = keys,
                };
                result.data = ImagCode;
            }
            return Json(result);
        }

        /// <summary>
        /// 登录人员模块权限(胡家源)
        /// </summary>
        /// <param name="param">医生id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RIPAuthority("菜单模块下的子集合", "查询菜单", "胡家源", "2020-09-10")]
        public IActionResult GetModules(GetModulesParam param)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询成功!");

            if (param.userid == 0)
            {
                result.msg = "人员Id错误";
                return Json(result);
            }
            var loginInfo = this.GetInstance<ILogin>();

            result.data = loginInfo.Get_DoctorModules(loginUser.id).Result;

            return Json(result);
        }

        /// <summary>
        /// 获取已登录用户的信息，返回用户信息及可访问的菜单权限等
        /// </summary>
        /// <param name="param">人员id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("获取用户信息", "包括当前用户基本信息及返回菜单及菜单下的功能权限，接口权限", "胡家源", "2020-09-10")]
        public IActionResult GetUser(GetModulesParam param)
        {
            if (loginUser != null)
            {
                return Json(new ResponseModel(ResponseCode.Success, "获取成功", loginUser));
            }
            else
            {
                return Json(new ResponseModel(ResponseCode.NotAuthority, "获取用户失败,返回信息为空;请检查是否传正常的token"));
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("修改密码", "医生APP端", "胡家源", "2020-09-10")]
        public IActionResult UpdatePassword(UpdatePasswordParam param)
        {
            var resultMsg = new ResponseModel(ResponseCode.Error, "修改失败", false);
            try
            {
                if (string.IsNullOrEmpty(param.old_password) || string.IsNullOrEmpty(param.new_password))
                {
                    resultMsg.msg = "参数验证不通过";
                    return Json(resultMsg);
                }
                var doctor = this.Query<SystemUser>()
                    .Where("id", param.userid)
                    .Where("登录密码", Strings.StrToMD5(param.old_password)).GetModel();
                if (doctor == null)
                {
                    resultMsg.msg = "用户信息不存在！";
                    return Json(resultMsg);
                }
                if (doctor.密码 != Strings.StrToMD5(param.old_password))
                {
                    resultMsg.msg = "原密码输入错误！";
                    return Json(resultMsg);
                }

                //请求参数转换成业务逻辑需要的请求参数对象
                var entity = new SystemUser()
                {
                    id = param.userid,
                    密码 = Strings.StrToMD5(param.new_password),//加密
                };
                var count = this.Update<SystemUser>(entity)
                    .Columns("密码")
                    .Where("id", entity.id)
                    .Execute();
                if (count > 0)//判断不为空
                {
                    resultMsg.data = true;
                    resultMsg.code = (int)ResponseCode.Success;
                    resultMsg.msg = "修改成功";
                }
            }
            catch (Exception ex)
            {
                LogError("修改密码", ex);
                resultMsg.msg = "修改异常";
            }
            return Json(resultMsg);
        }
    }
}