/***********************************************************
* 功能：角色管理接口，保存角色的模块参数
* 描述：
* 作者：zjy
* 日期：2020-05-24
*********************************************************/

namespace UP.Web.Models.Admin.Role
{
    /// <summary>
    /// 保存角色的模块参数
    /// </summary>
    public class SaveParam
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string roleid { get; set; }
        /// <summary>
        /// 模块ID，多个以逗号分隔
        /// </summary>
        public string modularids { get; set; }
    }
}
