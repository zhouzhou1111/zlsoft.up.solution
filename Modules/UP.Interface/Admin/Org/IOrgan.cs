/****************************************************
* 功能：机构管理接口层
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
    public  interface IOrgan:IBasic
    {
        /// <summary>
        /// 分页查询机构列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <returns></returns>
        Task<ListPageModel<OrganDto>> GetOrganPageList(int pageNum, int pageSize, string keyword);

        // <summary>
        /// 根据上级机构id查询
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">条件查询关键字</param>
        /// <param name="pid">上级id</param>
        /// <returns></returns>
        Task<List<OrganDto>> GetOrganListByPid(int pid);
        /// <summary>
        /// 查询机构列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<Organ>> GetOrganList(string keyword);


        /// <summary>
        /// 根据机构id获取机构信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrganDto> GetOrganDtoInfo(Organ orginfo,string pName);
    }
}
