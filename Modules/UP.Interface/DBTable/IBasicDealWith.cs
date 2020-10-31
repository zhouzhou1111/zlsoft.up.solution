/*********************************************************
* 功能：数据的新增、编辑、重复性验证
* 描述：对单表操作的数据提供基础操作功能
* 作者：贺伟
* 日期：2020-03-27
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;

namespace UP.Interface.DBTable
{
    /// <summary>
    /// 基础处理
    /// </summary>
    public interface IBasicDealWith : IBasic
    {
        /// <summary>
        /// 重复性验证
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="id">自身id</param>
        /// <param name="valuePairs">验证字典集合</param>
        /// <returns>重复性提示信息:true代表不存在,false代表已存在</returns>
        Task<string> CheckExists(string tableName, string id, Dictionary<string, object> valuePairs);

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作提示信息</returns>
        Task<ResponseModel> EditModel(object entity);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作提示信息</returns>
        Task<ResponseModel> AddModel(object entity);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作提示信息</returns>
        Task<ResponseModel> UpdateModel(object entity);


        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>实体数据</returns>
        Task<string> GetModelInfo(object entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">数据实体，需传输实体id</param>
        /// <returns></returns>
        Task<string> DeleteModel(object entity);
    }
}