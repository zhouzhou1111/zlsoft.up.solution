using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace UP.Logics.Admin.Sync
{
    /// <summary>
    /// 创建表脚本
    /// </summary>
    public class CreateTabModel
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否可为空
        /// </summary>
        public string NotNull { get; set; }
    }
}
