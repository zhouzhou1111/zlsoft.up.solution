using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.BussinessSys
{
    public class BussinessSysPram
    {
        /// <summary>
        /// 条件查询关键字
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        /// 服务源id
        /// </summary>
        public int productScoureid { get; set; }
        /// <summary>
        /// 数据源id
        /// </summary>
        public int datasoureid { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public int productid { get; set; }
        /// <summary>
        /// 应用信息appidid
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 状态
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
        /// 数据库类型（数据源：筛选条件）
        /// </summary>

        public int databasetype { get; set; }


    }
}
