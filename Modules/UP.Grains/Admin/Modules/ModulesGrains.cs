/****************************************************
* 功能：模块管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/11 16:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Modules;
using UP.Logics.Admin.Modules;
using UP.Models.DB.RoleRight;

namespace UP.Grains.Admin.Modules
{
    public class ModulesGrains : BasicGrains<ModulesLogic>, IModules
    {
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<List<ModulesInfo>> SelectModules()
        {
            return Task.FromResult(this.Logic.SelectModulesList());
        }
    }
}
