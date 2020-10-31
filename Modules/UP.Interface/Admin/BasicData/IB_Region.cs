using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.DB.BasicData;

namespace UP.Interface.Admin.BasicData
{
  public  interface IB_Region: IBasic
    {
        /// <summary>
        /// 查询行政区划
        /// </summary>
        /// <param name="parent_code">上级编码</param>
        /// <param name="prop">性质</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        Task<List<b_region>> GetRegionList(string parent_code, int prop, string code);


        /// <summary>
        /// 新增或者修改行政区划信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseModel> AddorUpdate(b_region model);

        /// <summary>
        /// 根据行政区划id查询行政区划信息信息
        /// </summary>
        /// <param name="id">行政区划id</param>
        /// <returns></returns>
        Task<ResponseModel> GetRegion(string id);
        /// <summary>
        /// 修改行政区划状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        Task<ResponseModel> UpdateRegionState(string id, int state);


        /// <summary>
        /// 验证编码是否重复
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="id">id,判断新增还是修改</param>
        /// <returns></returns>
        Task<ResponseModel> CheckCode(string code, string id);
    }
}
