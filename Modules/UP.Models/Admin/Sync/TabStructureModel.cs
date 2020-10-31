using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.Sync
{
    /// <summary>
    /// 数据表结构
    /// </summary>
    public class TabStructureModel
    {
        /// <summary>
        /// 字段顺序
        /// </summary>
        public int attnum { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string field { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// 是否非空 
        /// </summary>
        public string notnull { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
    }
}
