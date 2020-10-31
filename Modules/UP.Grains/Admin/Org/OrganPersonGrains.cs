/****************************************************
* 功能：机构人员管理业务层
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
    public class OrganPersonGrains : BasicGrains<OrgPersonLogic>, IOrgPerson
    {
        /// <summary>
        /// 新增或修改机构人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditPerson(OrgPersonDto model)
        {
            return Task.FromResult(this.Logic.EditPerson(model));
        }

        /// <summary>
        /// 根据id获取机构人员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<OrgPersonDto> GetOrgPersonDtoInfo(int id)
        {
            return Task.FromResult(this.Logic.GetOrgPersonDtoInfo(id));
        }

        /// <summary>
        /// 分页查询机构人员列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="orgId">机构id</param>
        /// <returns></returns>
        public Task<ListPageModel<OrgPersonDto>> GetOrgPersonList(int pageNum, int pageSize, string keyword, int orgId,string userstate)
        {
            return Task.FromResult(this.Logic.GetOrgPersonList(pageNum, pageSize, keyword, orgId, userstate));
        }

        /// <summary>
        /// 修改机构人员及账户状态
        /// </summary>
        /// <param name="accountid">账户id</param>
        /// <param name="orgpersonid">机构人员id</param>
        /// <param name="state">状态</param>
        /// <returns></returns>

        public Task<ResponseModel> UpdateOrgPersonState(int accountid, int orgpersonid, int state)
        {
            return Task.FromResult(this.Logic.UpdateOrgPersonState(accountid, orgpersonid, state));
        }
    }
}
