/****************************************************
* 功能：角色管理PC端api
* 描述：
* 作者：胡家源
* 日期：2020/09/10 16:04:03
*********************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary;
using System;
using System.Linq;
using UP.Basics;
using UP.Interface.Admin.Role;
using UP.Models.DB.RoleRight;
using UP.Web.Models.Admin.ModulesFunctionInterface;
using UP.Web.Models.Admin.Role;
using UP.Web.Models.Public;

namespace UP.Web.Controllers.Admin.RoleManager
{
    /// <summary>
    /// 角色管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class RoleManagerController : BasicsController
    {
        /// <summary>
        /// 新增或修改角色（胡家源）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改角色", "新增或修改角色", "胡家源", "2020-09-09")]
        public IActionResult AddRoles(RoleInfo entity)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            if (!entity.名称.IsNotNullOrEmpty())
            {
                resModel.msg = "角色名称不能为空！";
                resModel.msg = "操作异常";
                return Json(resModel);
            }

            try
            {
                var count = 0;//数据库执行返回结果
                if (entity.id.IsNullOrEmpty())
                {
                    entity.创建人 = loginUser.id;
                    entity.创建时间 = DateTime.Now;
                    entity.数据标识 = 1;
                    //角色名称是否存在
                    var value = this.Query<RoleInfo>().Where("名称", entity.名称.Trim()).Exists();
                    //角色名称已存在
                    if (value)
                    {
                        resModel.msg = "角色名称已存在";
                        return Json(resModel);
                    }
                    //名称不存在，新增
                    count = this.Add<RoleInfo>(entity).Execute();
                }
                else
                {
                    //角色名称是否存在
                    var roleList = this.Query<RoleInfo>().Where("名称", entity.名称.Trim()).GetModelList();
                    if (roleList != null && roleList.Any(d => d.id != entity.id && d.名称 == entity.名称))
                    {
                        resModel.msg = "角色名称已存在";
                        return Json(resModel);
                    }
                    //修改模块
                    count = this.Update<RoleInfo>(entity)
                        .Columns("名称", "描述", "工作单位", "是否停用")
                        .Where("id", entity.id).Execute();
                }
                //成功
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "执行成功";
                }
            }
            catch (Exception ex)
            {
                LogError("新增或修改角色", ex);
                resModel.msg = "操作异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询角色列表(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RIPAuthority("查询角色列表", "查询角色列表（分页）", "胡家源", "2020-09-09")]
        public IActionResult RoleList(RoleParam param)
        {
            //排班接口
            var role = this.GetInstance<IRoles>();
            //查询所有角色
            var result = role.GetRoleList(param.page_num, param.page_size)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 查询单个角色(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询单个角色", "查询单个角色", "胡家源", "2020-09-09")]
        public IActionResult RoleInfo(QueryModelParm param)
        {
            var result = new ResponseModel(ResponseCode.Error, "查询失败!");
            try
            {
                result.data = this.Query<RoleInfo>().Where("id", param.id).GetModel();
                if (result.data != null)
                {
                    result.code = (int)ResponseCode.Success;
                    result.msg = "查询成功";
                }
            }
            catch (Exception ex)
            {
                result.msg = "查询角色发生异常";
                Logger.Instance.Error("查询角色发生异常", ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 查询模块数量(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询模块数量", "查询角色模块数量", "胡家源", "2020-09-09")]
        public IActionResult RolemodularList()
        {
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询模块数量
            var result = role.SelectRolemodularList()?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 删除角色（胡家源）
        /// </summary>
        /// <param name="param">角色id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除角色", "传入角色id删除角色", "胡家源", "2020-09-09")]
        public IActionResult DelModules(QueryModelParm param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "删除失败", false);
            try
            {
                //查询角色模块，返回的r为bool值，true为存在，反之 不存在
                var r = this.Query<RoleModularInfo>()
                              .Where("角色id", param.id)
                              .Exists();
                if (r)//判断该角色是否配置模块
                {
                    resModel.msg = "该角色已配置模块，无法删除";
                    return Json(resModel);
                }
                var entity = new RoleInfo();
                entity.id = param.id;
                entity.数据标识 = 0;
                var count = this.Update<RoleInfo>(entity)
                        .Columns("数据标识")
                        .Where("id", entity.id).Execute();
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.data = true;
                    resModel.msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                LogError("删除角色", ex);
                resModel.msg = "操作异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 禁用/启用角色(胡家源)
        /// </summary>
        /// <param name="param">
        /// "id":角色id
        /// "state":禁用/启用状态 1-启用 0-禁用
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("禁用/启用角色", "根据角色id禁用/启用角色,需要传入id和state的值", "胡家源", "2020-09-09")]
        public IActionResult DisableRole(QueryStateParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败", false);
            try
            {
                //判断是否传入id
                if (!param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                var entity = new RoleInfo()
                {
                    id = param.id,
                    是否停用 = param.state
                };
                //管理员禁用/启用角色
                var count = this.Update<RoleInfo>(entity)
                    .Columns("是否停用")
                    .Where("id", entity.id)
                    .Execute();
                if (count > 0)//判断不为空
                {
                    resModel.data = true;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "修改成功";
                }
            }
            catch (Exception ex)
            {
                LogError("禁用/启用角色", ex);
                resModel.msg = "修改异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询角色的模块(胡家源)
        /// </summary>
        /// <param name="param">角色id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询角色的模块", "根据角色id查询角色的模块", "胡家源", "2020-09-09")]
        public IActionResult SelRoleModularList(QueryModelParm param)
        {
            //判断是否传入id
            if (param.id.IsNullOrEmpty())
            {
                return Json(new ResponseModel(ResponseCode.Error, "数据异常"));
            }
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询角色所有的模块
            var result = role.SelRoleModularList(param.id)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 删除角色模块(胡家源)
        /// </summary>
        /// <param name="param">"id":角色id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除角色模块", "传入角色模块id删除角色模块", "胡家源", "2020-09-09")]
        public IActionResult DelRoleModules(QueryModelParm param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "删除失败", false);
            try
            {
                //返回的r为bool值，true为存在，反之 不存在
                var r = this.Query<RoleFunction>()
                              .Where("id", param.id)
                              .Exists();
                if (r)//判断该角色是否配置模块
                {
                    resModel.msg = "该角色已配置角色模块功能，无法删除";
                    return Json(resModel);
                }
                var count = this.Delete<RoleModularInfo>()
                        .Where("id", param.id).Execute();
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.data = true;
                    resModel.msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                LogError("删除角色", ex);
                resModel.msg = "操作异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 查询角色模块授权(胡家源)
        /// </summary>
        /// <param name="param">"id":角色id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询角色模块授权", "根据角色id角色模块授权", "胡家源", "2020-09-09")]
        public IActionResult SelectRoleModularTree(QueryModelParm param)
        {
            //判断是否传入id
            if (param.id.IsNullOrEmpty())
            {
                return Json(new ResponseModel(ResponseCode.Error, "数据异常"));
            }
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询角色所有的模块
            var result = role.AuthSelectRoleModular(param.id)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 查询角色模块功能授权(胡家源)
        /// </summary>
        /// <param name="param">
        /// "rolemid":角色模块id
        /// "modularid":模块id
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询角色模块功能授权", "根据角色模块IDid查询角色模块功能授权", "胡家源", "2020-09-09")]
        public IActionResult RoleModularFunctionAuthList(AuthParam param)
        {
            //判断是否传入id
            if (param.rolemid.IsNullOrEmpty() || param.modularid.IsNullOrEmpty())
            {
                return Json(new ResponseModel(ResponseCode.Error, "数据异常"));
            }
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询角色所有模块的功能授权
            var result = role.RoleFunctionAuthorization(param.rolemid, param.modularid)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 保存角色的模块(胡家源)
        /// </summary>
        /// <param name="param">
        /// "roleid"：角色ID
        /// "modularids": "模块ids" 多个以逗号分隔
        /// </param>
        /// <returns></returns>。
        [HttpPost]
        [RIPAuthority("保存角色的模块", "保存角色的模块", "胡家源", "2020-09-09")]
        public IActionResult SavaRoleModular(SaveParam param)
        {
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询角色所有的模块
            var result = role.RoleModularSava(param.roleid, param.modularids)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 查询角色模块功能(胡家源)
        /// </summary>
        /// <param name="param">"id":角色模块id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询角色模块功能", "根据角色模块id获取角色模块功能", "胡家源", "2020-09-09")]
        public IActionResult GetRoleFunctionList(RoleMkParm param)
        {
            //判断是否传入id
            if (param.roleid.IsNullOrEmpty() || param.mkid.IsNullOrEmpty())
            {
                return Json(new ResponseModel(ResponseCode.Error, "数据异常"));
            }
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询角色所有的模块
            var result = role.SelectRoleFunctionList(param.roleid, param.mkid)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 保存角色的功能(胡家源)
        /// </summary>
        /// <param name="param">
        /// "roleid":角色ID
        /// "modularid":模块id
        /// "fids":"功能ID"，多个以逗号分隔
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("保存角色的功能", "保存角色的功能", "胡家源", "2020-09-09")]
        public IActionResult SavaRoleFunction(SaveRFParam param)
        {
            //模块接口
            var role = this.GetInstance<IRoles>();
            //保存角色的功能
            var result = role.RoleFunctionSava(param.roleid, param.modularid, param.fids)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 删除角色模块功能(胡家源)
        /// </summary>
        /// <param name="param">"id":角色id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除角色模块功能", "传入角色模块功能id删除角色模块功能", "胡家源", "2020-09-09")]
        public IActionResult DelRoleModulesFunc(QueryModelParm param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "删除失败", false);
            try
            {
                var count = this.Delete<RoleFunction>().Where("id", param.id).Execute();
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.data = true;
                    resModel.msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                LogError("删除角色", ex);
                resModel.msg = "操作异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 查询系统人员(胡家源)
        /// </summary>
        /// <param name="param">"id":科室id</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询科室人员", "根据科室id获取人员", "胡家源", "2020-09-09")]
        public IActionResult DepartmentPerson(QueryModelParm param)
        {
            //模块接口
            var role = this.GetInstance<IRoles>();
            //查询角色所有的模块
            var result = role.GetDepartmentPerson(param.keyword)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 新增人员角色(胡家源)
        /// </summary>
        /// <param name="param">
        /// "id":"人员ID"，多个以逗号分隔
        /// "jsid":角色ID
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增人员角色", "新增人员角色", "胡家源", "2020-09-09")]
        public IActionResult AddOrganStaffRelation(AddOSRParam param)
        {
            //模块接口
            var role = this.GetInstance<IRoles>();
            //新增人员角色
            var result = role.AddrolePerson(param.id, param.jsid)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 删除人员角色(胡家源)
        /// </summary>
        /// <param name="param">
        /// "id":"人员ID"，多个以逗号分隔
        /// "jsid":角色ID
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除人员角色", "删除人员角色", "胡家源", "2020-09-09")]
        public IActionResult DelOrganStaffRelation(AddOSRParam param)
        {
            //模块接口
            var role = this.GetInstance<IRoles>();
            //删除人员角色
            var result = role.DelrolePerson(param.id, param.jsid)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 查询角色人员(胡家源)
        /// </summary>
        /// <param name="param">
        /// "id":角色id
        /// "txcx":查询条件
        /// "page_num":页码
        /// "page_size":分页数据量
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询角色人员", "查询角色人员", "胡家源", "2020-09-09")]
        public IActionResult RolePersonList(PersonListParam param)
        {
            //接口
            var role = this.GetInstance<IRoles>();
            //查询角色人员
            var result = role.RolePersonSelectList(param.page_num, param.page_size, param.txcx, param.id)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 查询待分配角色人员(胡家源)
        /// </summary>
        /// /// <param name="param">
        /// "jsid":角色id
        /// "sfqy":用户状态，1-启用，其他禁用
        /// "txcx":查询条件
        /// "page_num":页码
        /// "page_size":分页数据量
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询待分配角色人员", "查询待分配角色人员", "胡家源", "2020-09-09")]
        public IActionResult OrgPersonList(OrgPersonParam param)
        {
            //接口
            var role = this.GetInstance<IRoles>();
            //查询角色人员
            var result = role.OrgPersonSelectList(param.page_num, param.page_size, param.txcx, param.sfqy)?.Result;
            return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
        }

        /// <summary>
        /// 根据角色和模块路径查询功能
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据角色和模块路径查询功能", "根据角色和模块路径查询功能", "胡家源", "2020-09-16")]
        public IActionResult getUrlBtnVer(ModulesFuncInterfaceParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败", false);
            var userId = loginUser.id;
            //获取角色id
            this.Query<RoleInfo>().Where("id", param.id).GetModel();
            var roleList = this.Query<SystemUserRole>().Where("人员id", userId).GetModelList();
            if (roleList.Any())
            {
                //接口
                var role = this.GetInstance<IRoles>();
                var roleids = string.Join(",", roleList.Select(d => d.role_id).ToList());
                //查询角色人员
                var result = role.getRoleBtnVerList(param.Url, roleids)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", result));
            }
            else
            {
                resModel.code = (int)ResponseCode.Success;
                resModel.data = null;
                resModel.msg = "查询成功";
                return Json(resModel);
            }
        }
    }
}