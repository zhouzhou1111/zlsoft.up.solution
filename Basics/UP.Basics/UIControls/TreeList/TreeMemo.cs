/*********************************************************
* 功能：数据表转换树形列表特性
* 描述：用于描述数据ID,父ID及子节点
* 作者：赵洪文
* 日期：2020-01-10
*********************************************************/
using System;

namespace UP.Basics
{
    /// <summary>
    /// 树形目录关键字描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TreeMemoAttribute : Attribute
    {
        /// <summary>
        /// 标识字段 默认值[ID]
        /// </summary>
        public string ID { set; get; } = "ID";
        /// <summary>
        /// 父标识字段 默认值[上级ID]
        /// </summary>
        public string PID { set; get; } = "上级ID";
        /// <summary>
        /// 子节点列表  默认值[Children]
        /// </summary>
        public string Children { get; set; } = "Children";
    }
}
