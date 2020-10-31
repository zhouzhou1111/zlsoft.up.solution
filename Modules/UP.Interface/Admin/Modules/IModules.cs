/****************************************************
* 功能：模块管理接口层
* 描述：
* 作者：胡家源
* 日期：2020/09/10 19:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.DB.RoleRight;

namespace UP.Interface.Admin.Modules
{
    public interface IModules : IBasic
    {
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<ModulesInfo>> SelectModules();

    }
}
