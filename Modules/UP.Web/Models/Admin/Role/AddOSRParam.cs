/***********************************************************
* 功能：角色管理接口，新增人员角色参数
* 描述：
* 作者：zjy
* 日期：2020-05-24
*********************************************************/

namespace UP.Web.Models.Admin.Role
{
    /// <summary>
    /// 新增人员角色参数
    /// </summary>
    public class AddOSRParam
    {
        /// <summary>
        /// 人员ID，多个以逗号分隔
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string jsid { get; set; }
    }
}
