using System.Collections.Generic;

namespace UP.Basics
{
    /// <summary>
    /// 执行器对象
    /// </summary>
    public class ExecuteModel
    {
        //初始化
        public ExecuteModel()
        {
            Conditions = new List<ConditionModel>();
        }
        /// <summary>
        /// 数据库操作类型
        /// </summary>
        public DBAction DatabaseAction { get; set; }

        /// <summary>
        /// 指定的列集合
        /// </summary>
        public string[] Columns { get; set; }

        /// <summary>
        /// 执行条件
        /// </summary>
        public List<ConditionModel> Conditions { get; set; }

        /// <summary>
        /// 获取数据的每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 执行的对象
        /// </summary>
        public dynamic Entity { get; set; }

        /// <summary>
        /// 排序字段（用于查询）
        /// </summary>
        public string OrderColumn { get; set; }

    }

    /// <summary>
    /// 执行条件的model
    /// </summary>
    public class ConditionModel
    {
        /// <summary>
        /// 条件名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 运行关系符
        /// </summary>
        public Operational Oper { get; set; }

        /// <summary>
        /// 条件值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 条件运算符类型
        /// </summary>
        public WhereType Where { get; set; }
    }
}
