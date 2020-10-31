using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Drug;

namespace UP.Interface.Drug
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDrug:IBasic
    {
        /// <summary>
        /// 查询药品的数据模型
        /// </summary>
        Task<List<DrugBasic>> GetDrugList(string searchChar);


        /// <summary>
        /// 新增药品
        /// </summary>
        /// <param name="drugBasic">药品模板</param>
        /// <returns></returns>
        Task<int> DrugAdd(DrugBasic drugBasic);
    }
}
