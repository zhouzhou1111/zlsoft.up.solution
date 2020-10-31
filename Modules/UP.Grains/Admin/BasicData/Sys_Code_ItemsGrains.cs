using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BasicData;
using UP.Logics.Admin.BasicData;
using UP.Models.Admin.BasicData;
using UP.Models.DB.BasicData;

namespace UP.Grains.Admin.BasicData
{
    public class Sys_Code_ItemsGrains : BasicGrains<Sys_Code_ItemsLogic>, ISys_Code_Items
    {
        /// <summary>
        /// 新增或者修改基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> AddorUpdate(sys_code_items model)
        {
            return Task.FromResult(this.Logic.AddorUpdate(model));
        }
        /// <summary>
        /// 根据分类id查询基础数据信息
        /// </summary>
        /// <param name="id">基础数据id</param>
        /// <returns></returns>
        public Task<ResponseModel> GetItems(string id)
        {
            return Task.FromResult(this.Logic.GetItems(id));
        }

        /// <summary>
        /// 根据基础数据id分页查询基础数据
        /// </summary>
        /// <param name="catgoryid"></param>
        /// <returns></returns>
        public Task<ListPageModel<sys_code_items>> GetitemsListByCid(string catgoryid, int pageNum, int pageSize, string keyword)
        {
            return Task.FromResult(this.Logic.GetitemsListByCid(catgoryid,pageNum,pageSize,keyword));
        }

        /// <summary>
        /// 根据分类id查询基础数据
        /// </summary>
        /// <param name="catgoryid">分类id</param>
        /// <param name="id">修改时不包含本身id</param>
        /// <returns></returns>
        public Task<List<ItemsDto>> GetitemsListByCid(string catgoryid, string id)
        {

            return Task.FromResult(this.Logic.GetitemsListByCid(catgoryid, id));
        }

        /// <summary>
        /// 同步基础数据
        /// </summary>
        /// <param name="cid">分类id</param>
        /// <param name="type">判断是否删除表</param>
        /// <returns></returns>
        public Task<bool> SynchroBasicTable(string cid, int type)
        {
            return Task.FromResult(this.Logic.SynchroBasicTable(cid, type));
        }

        /// <summary>
        /// 根据id验证基础数据表是否存在
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public Task<ResponseModel> TableIsExist(string cid)
        {
            return Task.FromResult(this.Logic.TableIsExist(cid));
        }

        /// <summary>
        /// 修改基础数据状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        public Task<ResponseModel> UpdateItemsState(string id, int state)
        {
            return Task.FromResult(this.Logic.UpdateItemsState(id,state));
        }
    }
}
