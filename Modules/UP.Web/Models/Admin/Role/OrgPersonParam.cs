﻿/***********************************************************
* 功能：角色管理接口，查询机构人员参数
* 描述：
* 作者：zjy
* 日期：2020-05-24
*********************************************************/

namespace UP.Web.Models.Admin.Role
{
    /// <summary>
    /// 查询机构人员参数
    /// </summary>
    public class OrgPersonParam
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string jsid { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int sfqy { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public string txcx { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int page_num { get; set; }
        /// <summary>
        /// 分页数据量
        /// </summary>
        public int page_size { get; set; }
    }
}
