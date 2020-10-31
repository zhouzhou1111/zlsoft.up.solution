/***********************************************************
* 功能：角色管理接口，查询角色模块功能授权参数
* 描述：
* 作者：zjy
* 日期：2020-05-24
*********************************************************/

namespace UP.Web.Models.Admin.Role
{
    /// <summary>
    /// 查询角色模块功能授权参数
    /// </summary>
    public class AuthParam
    {
        /// <summary>
        /// 角色模块id
        /// </summary>
        public string rolemid { get; set; }
        /// <summary>
        /// 模块id
        /// </summary>
        public string modularid { get; set; }

    }
}
