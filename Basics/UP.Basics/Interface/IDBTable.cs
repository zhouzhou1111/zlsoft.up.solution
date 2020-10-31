using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// 常见业务操作接口
    /// </summary>
    public interface IDBTable : IBasic
    {
        /// <summary>
        /// 执行数据插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Insert(object entity);

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="executeModel">删除的执行器原型</param>
        /// <returns>返回删除结果</returns>
        Task<int> Delete(ExecuteModel executeModel);

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="executeModel">更新的执行器原型</param>
        /// <returns>返回更新结果</returns>
        Task<int> Update(ExecuteModel executeModel);

        /// <summary>
        /// 检查值是否在
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        Task<bool> Exists(ExecuteModel executeModel);

        /// <summary>
        /// 获取单独的一个model
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        Task<object> GetModel(ExecuteModel executeModel);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        Task<RIPListPageModel> GetModelList(ExecuteModel executeModel);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        Task<RIPListPageModel> GetModelPageList(ExecuteModel executeModel);

    }
}
