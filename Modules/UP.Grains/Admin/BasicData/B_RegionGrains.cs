using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BasicData;
using UP.Logics.Admin.BasicData;
using UP.Models.DB.BasicData;

namespace UP.Grains.Admin.BasicData
{
    public class B_RegionGrains : BasicGrains<B_RegionLogic>, IB_Region
    {
        /// <summary>
        /// 新增或者修改行政区划信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> AddorUpdate(b_region model)
        {
            return Task.FromResult(this.Logic.AddorUpdate(model));
        }

        /// <summary>
        /// 验证编码是否重复
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="id">id,判断新增还是修改</param>
        /// <returns></returns>
        public Task<ResponseModel> CheckCode(string code, string id)
        {
            return Task.FromResult(this.Logic.CheckCode(code,id));
        }


        /// <summary>
        /// 根据行政区划id查询行政区划信息信息
        /// </summary>
        /// <param name="id">行政区划id</param>
        /// <returns></returns>
        public Task<ResponseModel> GetRegion(string id)
        {
            return Task.FromResult(this.Logic.GetRegion(id));
        }
        /// <summary>
        /// 查询行政区划
        /// </summary>
        /// <param name="parent_code">上级编码</param>
        /// <param name="prop">性质</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public Task<List<b_region>> GetRegionList(string parent_code, int prop, string code)
        {
            return Task.FromResult(this.Logic.GetRegionList(parent_code, prop, code));
        }

        /// <summary>
        /// 修改行政区划状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        public Task<ResponseModel> UpdateRegionState(string id, int state)
        {
            return Task.FromResult(this.Logic.UpdateRegionState(id, state));
        }
    }
}
