using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Basics;

namespace UP.Logics.Admin.RoleManager
{
    public class RoleManagerLogic
    {
        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> SearchRoleList(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-角色分类", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询角色分类】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 角色模块数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel SelecRoleModularCountList(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-角色模块数量", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询角色分类】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddRoleInfo(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-增加角色", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【增加角色】:", ex);
            }
            return result;
        }



        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel UpdateRole(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-修改角色", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【修改角色】:", ex);
            }
            return result;
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel DeleteRole(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-删除数据", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【删除数据】:", ex);
            }
            return result;
        }

        /// <summary>
        /// 禁用启用角色
        /// </summary>
        /// <returns></returns>
        public ResponseModel DisableRole(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-禁用启用角色", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【禁用启用角色】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 查询角色模块信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> SelRoleModularList(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-角色模块信息", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【角色模块信息】:", ex);
            }
            return result;
        }
        /// <summary>
        /// 删除角色模块
        /// </summary>
        /// <returns></returns>
        public ResponseModel DelRoleModules()
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-删除角色模块", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【删除角色模块】:", ex);
            }
            return result;
        }

        /// <summary>
        /// 查询角色模块授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> SelectRoleModularTree(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-查询角色模块授权", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询角色模块授权】:", ex);
            }
            return result;
        }
        /// <summary>
        /// 查询角色模块功能授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> RoleModularFunctionAuthList(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-角色模块功能授权", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【角色模块功能授权】:", ex);
            }
            return result;
        }

        /// <summary>
        /// 保存角色模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel SavaRoleModular(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-保存角色模块", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【保存角色模块】:", ex);
            }
            return result;
        }

        /// <summary>
        /// 查询角色模块功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> GetRoleFunctionList(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-角色模块功能授权", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【角色模块功能授权】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 保存角色的功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel SavaRoleFunction(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-保存角色的功能", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【保存角色的功能】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 查询角色人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> DepartmentPerson(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-查询角色人员", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询角色人员】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 新增人员角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddOrganStaffRelation(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-新增人员角色", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【新增人员角色】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 删除人员角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel DelOrganStaffRelation(object model)
        {
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-删除人员角色", null, null);

                    //执行SQL脚本
                    result.data = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【删除人员角色】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 查询角色人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> RolePersonList(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-角色列表-查询角色人员", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询角色人员】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 查询机构人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object OrgPersonList(object model)
        {
            object Infos = new object();
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-查询机构人员", null, null);

                    //执行SQL脚本
                    Infos = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询机构人员】:", ex);
            }
            return Infos;
        }

    }
}
