/****************************************************
* 功能：登录接口相关
* 描述：李伟修改
* 作者：陈洁
* 日期：2020/05/15 19:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Basics.Models;
using UP.Models.Admin.Login;

namespace UP.Interface.Admin.Login
{
    public interface ILogin : IBasic
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="account_name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<JsonMsg<LoginInfoModel>> DoctorLogin(string account_name, string password);



        /// <summary>
        /// 获取医生模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuModel>> Get_DoctorModules(string userId);
    }
}