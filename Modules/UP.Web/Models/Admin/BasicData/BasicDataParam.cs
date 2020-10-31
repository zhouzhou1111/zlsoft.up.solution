using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.BasicData
{
    public class BasicDataParam
    {

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 基础数据枚举类型
        /// </summary>
        public int tabletype { get; set; }

        /// <summary>
        /// 条件查询关键字
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int page_num { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int page_size { get; set; }

        /// <summary>
        /// 基础数据guid
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public string cid { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int  type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 上级编码
        /// </summary>
        public string pcode { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public int prop { get; set; }

    }
}
