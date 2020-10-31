using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.RoleRight;
using UP.Models.DB.RoleRight;

namespace UP.Interface.Admin.ModulesInterface
{
    public interface IModulesInterface : IBasic
    {
        /// <summary>
        /// 获取模块功能接口列表
        /// </summary>
        /// <param name="mkid">模块id</param>
        /// <param name="gnid">功能id</param>
        /// <param name="roleid">角色id</param>
        /// <param name="type">0为加载全部,1为加载未停用</param>
        /// <returns></returns>
        Task<List<ModulesFunctionInterfaceDto>> getInterfaceList(int mkid, int gnid,int roleid,int type);
        /// <summary>
        /// 保存角色模块功能接口
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <param name="mkid">模块id</param>
        /// <param name="gnid">功能id</param>
        /// <param name="interfaceids">接口ids</param>
        /// <returns></returns>
        Task<ResponseModel> saveRoleModuleFuncInterFace(int roleid, int mkid, int gnid, string interfaceids);

        /// <summary>
        /// 获取模块功能接口列表
        /// </summary>
        /// <param name="mkid">模块id</param>
        /// <param name="gnid">功能id</param>
        /// <param name="roleid">角色id</param>
        /// <returns></returns>
        Task<List<ModulesFunctionInterfaceDto>> getCheckInterfaceList(int mkid, int gnid, int roleid);
    }
}
