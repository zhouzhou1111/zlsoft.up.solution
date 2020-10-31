/********************************************************
* 功能：分页原型 
* 描述：分页原型
* 作者：王海洋
* 日期：2020-01-21
*********************************************************/
using System.Collections.Generic;

namespace UP.Basics
{
    /// <summary>
    /// 平台分页原型
    /// </summary>
    public class ListPageModel<T> where T : class
    {
        /// <summary>
        /// 总数量 
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 列表集合
        /// </summary>
        public List<T> PageList { get; set; }
    }

    /// <summary>
    /// 没有带具体的数据类型（主要是传递 DataTable时使用）
    /// </summary>
    public class RIPListPageModel
    {
        /// <summary>
        /// 总数量 
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 具体传递的值
        /// </summary>
        public byte[] Value { get; set; }
    }
}
