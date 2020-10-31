
using System;using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.BasicData;
using UP.Models.DB.BasicData;

namespace UP.Interface.Admin.BasicData
{
    public interface ISys_Code_Catgory : IBasic
    {
        /// <summary>
        /// 获取基础数据分类列表
        /// </summary>
        /// <returns></returns>
        Task<List<CatgoryTreeDto>> GetSys_Code_CatgoryList();
        /// <summary>
        /// 新增或者修改基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseModel> AddorUpdate(sys_code_catgory model);

        /// <summary>
        /// 根据分类id查询分类信息
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        Task<ResponseModel> GetCatgory(string id);

        /// <summary>
        /// 修改基础数据分类状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        Task<ResponseModel> UpdateCatgoryState(string id, int state);
    }
}
