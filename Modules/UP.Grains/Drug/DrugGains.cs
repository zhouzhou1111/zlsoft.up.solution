using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Drug;
using UP.Logics.Drug;
using UP.Models.Drug;

namespace UP.Grains.Drug
{
    public class DrugGains : BasicGrains<DrugLogic>,IDrug
    {
        /// <summary>
        /// 新增药品
        /// </summary>
        /// <param name="drugBasic">药品模板</param>
        /// <returns></returns>
        public Task<int> DrugAdd(DrugBasic drugBasic)
        {
            return this.Logic.DrugAdd(drugBasic);
        }
        /// <summary>
        /// 获取药品信息
        /// </summary>
        /// <param name="searchChar">查询参数</param>         
        /// <returns>返回药品模型</returns>
        public Task<List<DrugBasic>> GetDrugList(string searchChar)
        {
            return this.Logic.getDrugList(searchChar);
        }

        
    }
}
