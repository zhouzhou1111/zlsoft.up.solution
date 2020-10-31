/***********************************************************
* 功能：角色管理接口，保存角色的功能参数
* 描述：
* 作者：zjy
* 日期：2020-05-24
*********************************************************/

namespace UP.Web.Models.Admin.Role
{
    /// <summary>
    /// 保存角色的功能参数
    /// </summary>
    public class SaveRFParam
    {
        /// <summary>
        /// 角色模块ID
        /// </summary>
        public string rolemid { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string roleid { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public string modularid { get; set; }
        /// <summary>
        /// 功能id,多个以逗号分隔
        /// </summary>
        public string fids { get; set; }
    }
}
