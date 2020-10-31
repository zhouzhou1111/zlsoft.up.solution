using System.Threading.Tasks;
using UP.Basics;
using UP.Logics;

namespace UP.Grains
{
    public class DBTableGrains : BasicGrains<DBTableLogic>, IDBTable
    {
        /// <summary>
        /// 插入数据 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> Insert(dynamic entity)
        {
            return Task.FromResult(this.Logic.Insert(entity));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public Task<int> Delete(ExecuteModel executeModel)
        {
            return Task.FromResult(this.Logic.Delete(executeModel));
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public Task<int> Update(ExecuteModel executeModel)
        {
            return Task.FromResult(this.Logic.Update(executeModel));
        }

        /// <summary>
        /// 检查记录是否存在
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public Task<bool> Exists(ExecuteModel executeModel)
        {
            return Task.FromResult(this.Logic.Exists(executeModel));
        }

        /// <summary>
        /// 查询一个model数据
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public Task<object> GetModel(ExecuteModel executeModel)
        {
            return Task.FromResult(this.Logic.GetModel(executeModel));
        }

        /// <summary>
        /// 查询一个model列表
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public Task<RIPListPageModel> GetModelList(ExecuteModel executeModel)
        {
            return Task.FromResult(this.Logic.GetModelList(executeModel));
        }

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public Task<RIPListPageModel> GetModelPageList(ExecuteModel executeModel)
        {
            return Task.FromResult(this.Logic.GetModelPageList(executeModel));
        }
    }
}
