using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.Org
{
    public class AreaModel
    {
        /// <summary>
        /// 上级编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 当前id
        /// </summary>
        public long tid { get; set; }
    }
}
