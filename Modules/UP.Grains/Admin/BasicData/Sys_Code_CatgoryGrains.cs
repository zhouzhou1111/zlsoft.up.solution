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
    public class Sys_Code_CatgoryGrains : BasicGrains<Sys_Code_CatgoryLogic>, ISys_Code_Catgory
    {
        /// <summary>
        /// 新增或者修改基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> AddorUpdate(sys_code_catgory model)
        {
            return Task.FromResult(this.Logic.AddorUpdate(model));
        }

        /// <summary>
        /// 根据分类id查询分类信息
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        public Task<ResponseModel> GetCatgory(string id)
        {
            return Task.FromResult(this.Logic.GetCatgory(id));  
        }

        public Task<List<CatgoryTreeDto>> GetSys_Code_CatgoryList()
        {
            return Task.FromResult(this.Logic.GetSys_Code_CatgoryList());
        }

        /// <summary>
        /// 修改基础数据分类状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        public Task<ResponseModel> UpdateCatgoryState(string id, int state)
        {
            return Task.FromResult(this.Logic.UpdateCatgoryState(id, state));
        }
    }
}
