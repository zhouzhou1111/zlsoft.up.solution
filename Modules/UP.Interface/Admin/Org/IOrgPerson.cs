/****************************************************
* 功能：机构人员管理接口层
* 描述：
* 作者：胡家源
* 日期：2020/09/17 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.Org;
using UP.Models.DB.BasicData;

namespace UP.Interface.Admin.Org
{
   public interface IOrgPerson:IBasic
    {
        /// <summary>
        /// 分页查询机构人员列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="orgId">机构id</param>
        /// <returns></returns>
        Task<ListPageModel<OrgPersonDto>> GetOrgPersonList(int pageNum, int pageSize, string keyword, int orgId, string state);

        /// <summary>
        /// 新增或修改机构人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseModel> EditPerson(OrgPersonDto model);

        /// <summary>
        /// 根据id获取机构人员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrgPersonDto> GetOrgPersonDtoInfo(int id);

        /// <summary>
        /// 修改机构人员及账户状态
        /// </summary>
        /// <param name="accountid">账户id</param>
        /// <param name="orgpersonid">机构人员id</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        Task<ResponseModel> UpdateOrgPersonState(int accountid, int orgpersonid, int state);
    }
}
