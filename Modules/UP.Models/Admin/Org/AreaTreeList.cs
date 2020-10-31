using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.Org
{
   public class AreaTreeList
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public string sjid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 树形图标
        /// </summary>
        public string iconCls { get; set; }

        public List<AreaTreeList> children { get; set; }
    }
}
