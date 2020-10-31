using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.DBTable;
using UP.Logics.DBTable;

namespace UP.Grains.DBTable
{
    public class BasicDealWithGrains : BasicGrains<BasicDealWithLogic>, IBasicDealWith
    {
        /// <summary>
        /// 重复性验证
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">自身id</param>
        /// <param name="valuePairs">验证字典集合</param>
        /// <returns>重复性提示信息:true代表不存在,false代表已存在</returns>
        public Task<string> CheckExists(string tableName, string id, Dictionary<string, object> valuePairs)
        {
            return Task.FromResult(this.Logic.CheckExists(tableName, id, valuePairs));
        }

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作提示信息</returns>
        public Task<ResponseModel> EditModel(object entity)
        {
            return Task.FromResult(this.Logic.EditModel(entity));
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作提示信息</returns>
        public Task<ResponseModel> AddModel(object entity)
        {
            return Task.FromResult(this.Logic.AddModel(entity));
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作提示信息</returns>
        public Task<ResponseModel> UpdateModel(object entity)
        {
            return Task.FromResult(this.Logic.UpdateModel(entity));
        }


        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>实体数据</returns>
        public Task<string> GetModelInfo(object entity)
        {
            return Task.FromResult(this.Logic.GetModelInfo(entity));
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">数据实体，需传输实体id</param>
        /// <returns></returns>
        public Task<string> DeleteModel(object entity)
        {
            return Task.FromResult(this.Logic.DeleteModel(entity));
        }
    }
}