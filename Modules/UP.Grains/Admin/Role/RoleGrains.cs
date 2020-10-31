/****************************************************
* 功能：角色管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/10 16:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Role;
using UP.Logics.Admin.Role;
using UP.Models.Admin.RoleRight;
using UP.Models.DB.RoleRight;

namespace UP.Grains.Admin.Role
{
    public class RoleGrains : BasicGrains<RoleLogic>, IRoles
    {
        /// <summary>
        /// 分页查询角色列表
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<ListPageModel<RoleUserDto>> GetRoleList(int pageNum, int pageSize)
        {
            return Task.FromResult(this.Logic.GetRoleList(pageNum, pageSize));
        }


        /// <summary>
        /// 角色模块数量
        /// </summary>
        /// <returns></returns>
        public Task<List<RoleModularDto>> SelectRolemodularList()
        {
            return Task.FromResult(this.Logic.SelectRolemodularList());
        }

        /// <summary>
        /// 查询角色的模块(陈洁)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<ModulesRoleAuth>> SelRoleModularList(string id)
        {
            return Task.FromResult(this.Logic.SelRoleModularList(id));
        }

        /// <summary>
        /// 查询角色模块授权
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<ModulesRoleAuth>> AuthSelectRoleModular(string id)
        {
            return Task.FromResult(this.Logic.AuthSelectRoleModular(id));
        }

        /// <summary>
        /// 角色模块功能授权
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<ModulesFunctionDto>> RoleFunctionAuthorization(string roleModularId, string modularId)
        {
            return Task.FromResult(this.Logic.RoleFunctionAuthorization(roleModularId, modularId));
        }

        /// <summary>
        /// 保存角色的模块
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="modularids">"模块ID"，多个以逗号分隔</param>
        /// <returns></returns>
        public Task<ResponseModel> RoleModularSava(string roleid, string modularids)
        {
            return Task.FromResult(this.Logic.RoleModularSava(roleid, modularids));
        }

        /// <summary>
        /// 查询角色模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<ModulesFunction>> SelectRoleModularList(string id)
        {
            return Task.FromResult(this.Logic.SelectRoleModularList(id));
        }

        /// <summary>
        /// 查询角色功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<RoleModulesFunctionDto>> SelectRoleFunctionList(string id, string mkid)
        {
            return Task.FromResult(this.Logic.SelectRoleFunctionList(id, mkid));
        }

        /// <summary>
        /// 保存角色的功能
        /// </summary>
        /// <param name="rolemid">角色模块ID</param>
        /// <param name="fids">功能ID，多个以逗号分隔</param>
        /// <returns></returns>
        public Task<ResponseModel> RoleFunctionSava(string roleid, string modularid, string fids)
        {
            return Task.FromResult(this.Logic.RoleFunctionSava(roleid, modularid, fids));
        }

        /// <summary>
        /// 查询系统人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<SystemUser>> GetDepartmentPerson(string  name)
        {
            return Task.FromResult(this.Logic.GetDepartmentPerson(name));
        }

        /// <summary>
        /// 新增人员角色
        /// </summary>
        /// <param name="ryids">人员ID，多个以逗号分隔</param>
        /// <param name="jsid">角色ID</param>
        /// <returns></returns>
        public Task<ResponseModel> AddrolePerson(string ryids, string jsid)
        {
            return Task.FromResult(this.Logic.AddrolePerson(ryids, jsid));
        }

        /// <summary>
        /// 删除人员角色
        /// </summary>
        /// <param name="ryids">人员ID，多个以逗号分隔</param>
        /// <param name="jsid">角色ID</param>
        /// <returns></returns>
        public Task<ResponseModel> DelrolePerson(string ryids, string jsid)
        {
            return Task.FromResult(this.Logic.DelrolePerson(ryids, jsid));
        }

        /// <summary>
        /// 查询角色人员
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="txCx"></param>
        /// <param name="jsid"></param>
        /// <returns></returns>
        public Task<ListPageModel<SystemRoleDto>> RolePersonSelectList(int pageNum, int pageSize, string txCx, string jsid)
        {
            return Task.FromResult(this.Logic.RolePersonSelectList(pageNum, pageSize, txCx, jsid));
        }

        /// <summary>
        /// 查询待分配角色人员
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="txCx"></param>
        /// <param name="jsid"></param>
        /// <returns></returns>
        public Task<ListPageModel<SystemUser>> OrgPersonSelectList(int pageNum, int pageSize, string txCx, int state)
        {
            return Task.FromResult(this.Logic.OrgPersonSelectList(pageNum, pageSize, txCx, state));
        }

        /// <summary>
        /// 根据角色和模块路径查询功能
        /// </summary>
        /// <param name="url">地址Url</param>
        /// <param name="roleids">角色id</param>
        /// <returns></returns>
        public Task<List<RoleBtnVer>> getRoleBtnVerList(string url, string roleids)
        {
            return Task.FromResult(this.Logic.getRoleBtnVerList(url, roleids));
        }
    }
}
