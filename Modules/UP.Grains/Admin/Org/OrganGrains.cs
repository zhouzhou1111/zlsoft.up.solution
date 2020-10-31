/****************************************************
* 功能：机构管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/17 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Modules;
using UP.Interface.Admin.Org;
using UP.Logics.Admin.Modules;
using UP.Logics.Admin.Org;
using UP.Models.Admin.Org;
using UP.Models.DB.BasicData;

namespace UP.Grains.Admin.Org
{
    public class OrganGrains : BasicGrains<OrganLogic>, IOrgan
    {
        public Task<OrganDto> GetOrganDtoInfo(Organ orginfo,string pName)
        {
            return Task.FromResult(this.Logic.GetOrganDtoInfo(orginfo, pName));
        }

        /// <summary>
        /// 查询机构列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public Task<List<Organ>> GetOrganList(string keyword)
        {
            return Task.FromResult(this.Logic.GetOrganList( keyword));
        }

        /// <summary>
        /// 根据上级机构id查询
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">条件查询关键字</param>
        /// <param name="pid">上级id</param>
        /// <returns></returns>
        public Task<List<OrganDto>> GetOrganListByPid(int pid)
        {
            return Task.FromResult(this.Logic.GetOrganListByPid(pid));
        }

        /// <summary>
        /// 分页查询机构列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <returns></returns>
        public Task<ListPageModel<OrganDto>> GetOrganPageList(int pageNum, int pageSize, string keyword)
        {
           return Task.FromResult(this.Logic.GetOrganPageList(pageNum, pageSize, keyword));
        }
    }
}
