/****************************************************
* 功能：角色管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/14 11:04:03
*********************************************************/

using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UP.Basics;
using UP.Models.Admin.RoleRight;
using UP.Models.DB.RoleRight;

namespace UP.Logics.Admin.Role
{
    public class RoleLogic
    {
        /// <summary>
        /// 分页查询角色列表
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ListPageModel<RoleUserDto> GetRoleList(int pageNum, int pageSize)
        {
            ListPageModel<RoleUserDto> item = null;
            try
            {
                List<string> listPara = new List<string>();
                //listPara.Add("是否停用");
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("AA00001-分页查询角色", null, listPara.ToArray());
                    int total = 0;
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr);
                    //执行SQL脚本
                    var items = sqlBuilder.Paging(pageNum, pageSize).Parameters("state", 1)
                             .GetModelList<RoleUserDto>(out total);
                    item = new ListPageModel<RoleUserDto>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取科室人员异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 角色模块数量
        /// </summary>
        /// <returns></returns>
        public List<RoleModularDto> SelectRolemodularList()
        {
            List<RoleModularDto> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色模块数量信息
                    var sqlStr = db.GetSql("AA00002-角色模块数量", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr);
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<RoleModularDto>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取角色模块数量异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 查询角色的模块(胡家源)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModulesRoleAuth> SelRoleModularList(string id)
        {
            List<ModulesRoleAuth> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    if (!id.IsNullOrEmpty())
                    {
                        param.Add("传入角色id");
                        sqlBuilder.Parameters("角色ID", id);
                    }
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00003-查询角色模块", null, param.ToArray());
                    //执行SQL脚本
                    item = sqlBuilder.SqlText(sqlStr).GetModelList<ModulesRoleAuth>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询角色的模块异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 查询角色模块授权
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModulesRoleAuth> AuthSelectRoleModular(string id)
        {
            List<ModulesRoleAuth> item = null;
            try
            {
                //AA00033-获取角色模块授权
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00004-获取角色模块授权", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr).Parameters("roleid", id);
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<ModulesRoleAuth>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询角色的模块异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 查询角色模块功能授权
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModulesFunctionDto> RoleFunctionAuthorization(string roleModularId, string modularId)
        {
            List<ModulesFunctionDto> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00005-获取角色模块功能授权", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr)
                        .Parameters("roleModularId", roleModularId)
                        .Parameters("modularId", modularId);
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<ModulesFunctionDto>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询角色的模块异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 保存角色的模块
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="modularids">"模块ID"，多个以逗号分隔</param>
        /// <returns></returns>
        public ResponseModel RoleModularSava(string roleid, string modularids)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "保存角色的模块成功!");
            if (roleid.IsNullOrEmpty())
            {
                result.code = (int)ResponseCode.Error;
                result.msg = "请选择角色";
                return result;
            }
            try
            {
                using (var db = new DbContext())
                {
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        var arr = roleid.Split(',');
                        var count = 0;
                        foreach (var id in arr)
                        {
                            if (id.IsNullOrEmpty())
                            {
                                continue;
                            }
                            //执行SQL脚本
                            var dt = db.Select("系统_角色模块").Columns("模块id").Where("角色id", id.ToInt32()).Select();      //获取修改前模块id
                            //遍历模块id删除角色功能
                            foreach (DataRow row in dt.Rows)
                            {
                                db.Delete("系统_角色功能").Where("模块id", row["模块id"]).Execute();
                            }
                            //删除角色模块
                            db.Delete("系统_角色模块").Where("角色id", id.ToInt32()).Execute();

                            var array = modularids.Split(',');
                            //遍历修改后模块id列表
                            foreach (var item in array)
                            {
                                if (!string.IsNullOrEmpty(item))    //模块id非空
                                {
                                    count = db.Insert("系统_角色模块").Column("角色id", id.ToInt32()).Column("模块id", item).Execute();    //添加角色模块
                                    if (count <= 0)
                                    {
                                        db.Rollback();
                                        return false;
                                    }
                                }
                            }
                        }
                        return true;
                    });
                    if (!value)
                    {
                        result.code = ResponseCode.Error.ToInt32();
                        result.msg = "保存角色的模块失败!";
                    }
                }
            }
            catch (Exception)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "保存角色的模块失败!";
            }
            return result;
        }

        /// <summary>
        /// 查询角色模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModulesFunction> SelectRoleModularList(string id)
        {
            List<ModulesFunction> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00031-角色功能列表", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr).Parameters("角色模块id", id);
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<ModulesFunction>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询角色的模块异常错误!", ex);
            }
            return item;
        }

        public List<RoleModulesFunctionDto> SelectRoleFunctionList(string id, string mkid)
        {
            List<RoleModulesFunctionDto> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00006-查询角色功能", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr).Parameters("roleid", id).Parameters("mkid", mkid); ;
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<RoleModulesFunctionDto>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询角色的模块异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 保存角色的功能
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="modularid">模块ID</param>
        /// <param name="fids">功能ID，多个以逗号分隔</param>
        /// <returns></returns>
        public ResponseModel RoleFunctionSava(string roleid, string modularid, string fids)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "保存角色的功能成功!");
            try
            {
                using (var db = new DbContext())
                {
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        var count = 0;
                        db.Delete("系统_角色功能").Where("模块id", modularid).Where("角色id", roleid).Execute();
                        var array = fids.Split(',');
                        //插入功能ID列表
                        foreach (var item in array)
                        {
                            if (!string.IsNullOrEmpty(item))    //功能ID非空
                            {
                                var funcid = int.Parse(item);
                                count = db.Insert("系统_角色功能").Column("角色id", roleid).Column("模块id", modularid).Column("功能id", funcid).Execute();      //新增角色功能
                                if (count <= 0)
                                {
                                    db.Rollback();
                                    return false;
                                }
                            }
                        }
                        return true;
                    });
                    //事务执行失败
                    if (!value)
                    {
                        result.code = (int)ResponseCode.Error;
                        result.msg = "保存角色的功能失败";
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "保存角色的功能失败!";
            }
            return result;
        }

        /// <summary>
        /// 查询系统人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SystemUser> GetDepartmentPerson(string name)
        {
            List<SystemUser> item = null;
            var param = new List<string> { "name" };
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlBuilder = db.Sql("");

                    if (name.IsNotNullOrEmpty())
                    {
                        param.Add("姓名");
                        sqlBuilder.Parameters("name", name);
                    }
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00007-查询系统人员", null, param.ToArray());
                    //执行SQL脚本
                    item = sqlBuilder.SqlText(sqlStr).GetModelList<SystemUser>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询系统人员异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 新增人员角色
        /// </summary>
        /// <param name="ryids">人员ID，多个以逗号分隔</param>
        /// <param name="jsid">角色ID</param>
        /// <returns></returns>
        public ResponseModel AddrolePerson(string ryids, string jsid)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "新增人员角色成功!");
            try
            {
                if (string.IsNullOrEmpty(ryids))
                {
                    result.code = (int)ResponseCode.Error;
                    result.msg = "参数异常";
                    return result;
                }
                using (var db = new DbContext())
                {
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        var array = ryids.Split(',');
                        //插入人员ID列表
                        foreach (var item in array)
                        {
                            if (!string.IsNullOrEmpty(item))    //人员ID非空
                            {
                                var model = new Models.DB.RoleRight.SystemUserRole()
                                {
                                    role_id = jsid,
                                    account_id = item
                                };
                                var count = db.Insert(model).Execute();
                                if (count <= 0)
                                {
                                    db.Rollback();
                                    return false;
                                }
                            }
                        }
                        return true;
                    });
                    //事务执行失败
                    if (!value)
                    {
                        result.code = (int)ResponseCode.Error;
                        result.msg = "新增人员角色失败";
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "新增人员角色操作发生异常!";
                Logger.Instance.Error("新增人员角色操作发生异常!", ex);
            }
            return result;
        }

        /// <summary>
        /// 删除人员角色
        /// </summary>
        /// <param name="ryids">人员ID，多个以逗号分隔</param>
        /// <param name="jsid">角色ID</param>
        /// <returns></returns>
        public ResponseModel DelrolePerson(string ryids, string jsid)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "删除人员角色成功!");
            try
            {
                using (var db = new DbContext())
                {
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        var array = ryids.Split(',');
                        //插入人员ID列表
                        foreach (var item in array)
                        {
                            if (!string.IsNullOrEmpty(item))    //人员ID非空
                            {
                                var zhid = int.Parse(item);
                                var count = db.Delete("系统_人员角色").Where("人员id", zhid).Where("角色id", jsid).Execute();
                                if (count <= 0)
                                {
                                    db.Rollback();
                                    return false;
                                }
                            }
                        }
                        return true;
                    });
                    //事务执行失败
                    if (!value)
                    {
                        result.code = (int)ResponseCode.Error;
                        result.msg = "删除人员角色失败";
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "删除人员角色失败!";
            }
            return result;
        }

        /// <summary>
        /// 查询角色人员--分页
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="ksId">科室id</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ListPageModel<SystemRoleDto> RolePersonSelectList(int pageNum, int pageSize, string txCx, string jsid)
        {
            ListPageModel<SystemRoleDto> item = null;
            var param = new List<string>();
            try
            {
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件不为空
                    if (txCx.IsNotNullOrEmpty())
                    {
                        param.Add("条件查询");
                        sqlBuilder.Parameters("Tx_Cx", txCx);
                    }
                    if (!jsid.IsNullOrEmpty())
                    {
                        param.Add("角色id");
                        sqlBuilder.Parameters("JSID", jsid);
                    }
                    //获取用户基本信息
                    var sqlStr = db.GetSql("AA00008-获取角色人员列表", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<SystemRoleDto>(out int total);
                    item = new ListPageModel<SystemRoleDto>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取科室人员异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 查询待分配角色人员--分页
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="ksId">科室id</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ListPageModel<SystemUser> OrgPersonSelectList(int pageNum, int pageSize, string txCx, int state)
        {
            ListPageModel<SystemUser> item = null;
            var param = new List<string>();
            try
            {
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //用户状态不为空
                    if (state.ToString().IsNotNullOrEmpty())
                    {
                        param.Add("用户状态");
                        sqlBuilder.Parameters("SFQY", state);
                    }
                    if (txCx.IsNotNullOrEmpty())
                    {
                        param.Add("查询条件");
                        sqlBuilder.Parameters("strwhere", txCx);
                    }
                    //获取用户基本信息
                    var sqlStr = db.GetSql("AA00009-获取待分配角色人员", null, param.ToArray());
                    int total = 0;
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<SystemUser>(out total);
                    item = new ListPageModel<SystemUser>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取人员异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 根据角色和模块路径查询功能
        /// </summary>
        /// <param name="url">地址Url</param>
        /// <param name="roleids">角色ids</param>
        /// <returns></returns>
        public List<RoleBtnVer> getRoleBtnVerList(string url, string roleids)
        {
            List<RoleBtnVer> items = new List<RoleBtnVer>();
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00016-根据角色查询功能", null, null);
                    var ids = roleids.Split(',').Select(d => Convert.ToInt32(d));
                    foreach (var roleid in ids)
                    {
                        //执行SQL脚本
                        ISqlBuilder sqlBuilder = db.Sql(sqlStr).Parameters("roleid", roleid); ;
                        //执行SQL脚本
                        var item = sqlBuilder.GetModelList<RoleBtnVer>();
                        items.AddRange(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询功能异常错误!", ex);
            }
            return items;
        }
    }
}