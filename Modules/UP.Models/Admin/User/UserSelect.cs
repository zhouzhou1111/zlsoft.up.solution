using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.User
{
    /// <summary>
    /// 人员列表查询条件
    /// </summary>
    public class UserSelect
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        public string sfqy { get; set; }
        /// <summary>
        /// 条件查询
        /// </summary>
        public string txcx { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int page_num { get; set; }
        /// <summary>
        /// 页数量
        /// </summary>
        public int page_size { get; set; }
    }
}
