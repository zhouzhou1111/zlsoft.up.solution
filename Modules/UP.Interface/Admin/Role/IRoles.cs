/****************************************************
* 功能：角色管理接口层
* 描述：
* 作者：胡家源
* 日期：2020/09/10 19:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.DB.RoleRight;
using UP.Models.Admin.RoleRight;

namespace UP.Interface.Admin.Role
{
    public interface IRoles : IBasic
    {
        /// <summary>
        /// 分页查询角色列表
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<ListPageModel<RoleUserDto>> GetRoleList(int pageNum, int pageSize);

        /// <summary>
        /// 查询模块数量
        /// </summary>
        /// <returns></returns>
        Task<List<RoleModularDto>> SelectRolemodularList();

        /// <summary>
        /// 查询角色的模块(陈洁)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ModulesRoleAuth>> SelRoleModularList(string id);

        /// <summary>
        /// 查询角色模块授权(陈洁)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ModulesRoleAuth>> AuthSelectRoleModular(string id);

        /// <summary>
        /// 角色模块功能授权
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ModulesFunctionDto>> RoleFunctionAuthorization(string roleModularId, string modularId);

        /// <summary>
        /// 保存角色的模块
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="modularids">"模块ID"，多个以逗号分隔</param>
        /// <returns></returns>
        Task<ResponseModel> RoleModularSava(string roleid, string modularids);

        /// <summary>
        /// 查询角色模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ModulesFunction>> SelectRoleModularList(string id);

        /// <summary>
        /// 查询角色功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<RoleModulesFunctionDto>> SelectRoleFunctionList(string id,string mkid);

        /// <summary>
        /// 保存角色的功能
        /// </summary>
        /// <param name="roleid">角色模块ID</param>
        /// <param name="fids">功能ID，多个以逗号分隔</param>
        /// <returns></returns>
        Task<ResponseModel> RoleFunctionSava(string roleid, string modularid, string fids);

        /// <summary>
        /// 查询系统人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<SystemUser>> GetDepartmentPerson(string  name);

        /// <summary>
        /// 新增人员角色
        /// </summary>
        /// <param name="ryids">人员ID，多个以逗号分隔</param>
        /// <param name="jsid">角色ID</param>
        /// <returns></returns>
        Task<ResponseModel> AddrolePerson(string ryids, string jsid);

        /// <summary>
        /// 删除人员角色
        /// </summary>
        /// <param name="ryids">人员ID，多个以逗号分隔</param>
        /// <param name="jsid">角色ID</param>
        /// <returns></returns>
        Task<ResponseModel> DelrolePerson(string ryids, string jsid);

        /// <summary>
        /// 查询角色人员
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="txCx"></param>
        /// <param name="jsid"></param>
        /// <returns></returns>
        Task<ListPageModel<SystemRoleDto>> RolePersonSelectList(int pageNum, int pageSize, string txCx, string jsid);


        /// <summary>
        ///  查询待分配角色人员
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="txCx"></param>
        /// <param name="jsid"></param>
        /// <returns></returns>
        Task<ListPageModel<SystemUser>> OrgPersonSelectList(int pageNum, int pageSize, string txCx, int state);
        /// <summary>
        /// 根据角色和模块路径查询功能
        /// </summary>
        /// <param name="url">地址Url</param>
        /// <param name="roleids">角色id</param>
        /// <returns></returns>
        Task<List<RoleBtnVer>> getRoleBtnVerList(string url, string roleids);
    }
}
