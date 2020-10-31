using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.Org
{
    public class OrgParam
    {
        /// <summary>
        /// 条件查询关键字
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        public int organid { get; set; }
        /// <summary>
        /// 机构人员id
        /// </summary>
        public int orgpersonid { get; set; }
        /// <summary>
        /// 账户id
        /// </summary>
        public int accountid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string userstate { get; set; }
        /// <summary>
        /// 人员状态
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int page_num { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int page_size { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public int pid { get; set; }
    }
}
