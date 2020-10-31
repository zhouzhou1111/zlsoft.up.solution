using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.Interface
{
    /// <summary>
    /// 外部接口查询参数实体类
    /// </summary>
    public class InterfaceItemSearchParam
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string CatgoryId { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
    }
}