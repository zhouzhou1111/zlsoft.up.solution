/***********************************************************
* 功能：针对单个对象信息条件对象
* 描述：
* 作者：贺伟
* 日期：2020-05-15
*********************************************************/

namespace UP.Web.Models.Public
{
    /// <summary>
    /// 针对单个对象信息条件对象
    /// </summary>
    public class QueryPageParm
    {
        /// <summary>
        ///主键id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///名称/关键字
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int pageindex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int pagesize { get; set; }
    }
}