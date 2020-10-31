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
    public class QueryModelParm
    {
        /// <summary>
        ///主键id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public string pid { get; set; }

        /// <summary>
        ///名称/关键字
        /// </summary>
        public string keyword { get; set; }
    }
}