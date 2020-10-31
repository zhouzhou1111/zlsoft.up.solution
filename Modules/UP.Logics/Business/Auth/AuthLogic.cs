/*********************************************************
* 功能：平台统一身份认证逻辑
* 描述：
* 作者：贺伟
* 日期：2020/09/19
**********************************************************/

using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using UP.Basics;
using UP.Basics.Models;
using UP.Models.Business.Auth;

namespace UP.Logics.Business.Auth
{
    /// <summary>
    /// 身份认证逻辑方法
    /// </summary>
    public class AuthLogic
    {
        /// <summary>
        /// 平台身份认证
        /// </summary>
        /// <param name="account">登录账户</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public JsonMsg<AuthInfo> GetUserAuth(string account, string password)
        {
            string passwordMD5 = QWPlatform.SystemLibrary.Utils.Strings.StrToMD5(password);
            var result = JsonMsg<AuthInfo>.OK(null, "平台身份认证成功!");
            AuthInfo authInfo = null;
            try
            {
                using (var db = new DbContext())
                {
                    //定义sql执行对象
                    var sqlBuilder = db.Sql("");
                    //SQL动态参数
                    List<string> list = new List<string>();
                    //如果包括account_name参数则使用account_name
                    sqlBuilder.Parameters("account", account);
                    list.Add("account");
                    sqlBuilder.Parameters("password", passwordMD5);
                    list.Add("password");
                    //获取执行的sql
                    string sql = db.GetSql("AB00001-平台身份认证", null, list.ToArray());
                    //执行数据查询
                    var items = sqlBuilder.SqlText(sql).GetModelList<AuthInfo>();
                    if (items == null || !items.Any())
                    {
                        result.msg = "账户或密码不正确！";
                        result.code = ResponseCode.Error;
                        return result;
                    }
                    authInfo = items.FirstOrDefault(x => x.account_status == 1);
                    if (authInfo == null)
                    {
                        result.msg = "账户已被停用,请与管理员联系！";
                        result.code = ResponseCode.Error;
                        return result;
                    }
                    authInfo = items.FirstOrDefault(x => x.account_status == 1 && x.status == 1);
                    if (authInfo == null)
                    {
                        result.msg = "人员信息已被停用,请与管理员联系！";
                        result.code = ResponseCode.Error;
                        return result;
                    }
                    result.data = authInfo;
                }
            }
            catch (Exception ex)
            {
                result.msg = "平台身份认证发生异常：";
                result.code = ResponseCode.Error;
                Logger.Instance.Error(result.msg, ex);
            }
            return result;
        }
    }
}