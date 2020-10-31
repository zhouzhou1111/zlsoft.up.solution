using System.Collections.Generic;

namespace UP.Basics
{
    /// <summary>
    /// 单表执行器
    /// </summary>
    public interface IBusinessExecute<T> where T : class, new()
    {
        /// <summary>
        /// 增加一个model
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        IBusinessExecute<T> Add(T t);

        /// <summary>
        /// 执行删除
        /// </summary> 
        /// <returns></returns>
        IBusinessExecute<T> Delete();

        /// <summary>
        /// 修改model
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns></returns>
        IBusinessExecute<T> Update(T t);

        /// <summary>
        /// 设置指定的更新列集合
        /// </summary>
        /// <param name="columns">指定的列集合</param>
        /// <returns></returns>
        IBusinessExecute<T> Columns(params string[] columns);

        /// <summary>
        /// 执行的条件器
        /// </summary>
        /// <param name="key">条件名</param>
        /// <param name="value">条件值</param>
        /// <returns>返回构造对象</returns>
        IBusinessExecute<T> Where(string key, object value);

        /// <summary>
        /// 带运行符号的条件
        /// </summary>
        /// <param name="key">条件名</param>
        /// <param name="oper">运算操作符</param>
        /// <param name="value">运算值</param>
        /// <returns></returns>
        IBusinessExecute<T> Where(string key, Operational oper, object value);

        /// <summary>
        /// 带有指定条件的or运算
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="oper">操作符</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        IBusinessExecute<T> OrWhere(string key, Operational oper, object value);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <returns></returns>
        IBusinessExecute<T> Select();

        /// <summary>
        /// 使用排序列
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        IBusinessExecute<T> Order(string column);

        /// <summary>
        /// 获取Model原型
        /// </summary>
        /// <returns>返回原型</returns>
        T GetModel();

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns></returns>
        List<T> GetModelList();

        /// <summary>
        /// 获取分页查询
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">大小</param>
        /// <returns></returns>
        ListPageModel<T> GetPageList(int page, int size);

        /// <summary>
        /// 检查值是否存在
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        int Execute();
    }
}
