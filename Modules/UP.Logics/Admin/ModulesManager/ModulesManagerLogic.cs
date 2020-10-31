using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Basics;

namespace UP.Logics.Admin.ModulesManager
{
    public  class ModulesManagerLogic
    {
        /// <summary>
        /// 查询所有模块列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> GetArticleType(object model)
        {
            List<object> result = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-文章分类", null, null);

                    //执行SQL脚本
                    result = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询所有模块列表】:", ex);
            }
            return result;
        }


        /// <summary>
        /// 查询单个模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetModuleAloneInfo(object model)
        {
            object Infos = new object();
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-查询单个模块", null, null);

                    //执行SQL脚本
                    Infos = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询单个模块】:", ex);
            }
            return Infos;
        }


        /// <summary>
        /// 增加模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddModules(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-增加模块", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【增加模块】:", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "响应失败!";
            }
            return result;
        }


        /// <summary>
        /// 修改模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel UpdateModules(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-修改模块", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【修改模块】:", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "响应失败!";
            }
            return result;
        }



        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel DeleteModules(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-删除模块", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【删除模块】:", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "响应失败!";
            }
            return result;
        }


        /// <summary>
        /// 新增模块功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddModuleFunction(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-新增模块功能", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【新增模块功能】:", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "响应失败!";
            }
            return result;
        }


        /// <summary>
        /// 修改模块功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel UpdateModuleFunction(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-修改模块功能", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【修改模块功能】:", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "响应失败!";
            }
            return result;
        }

        /// <summary>
        /// 修改模块功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel DeleteModuleFunction(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "请求成功!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-删除模块功能", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr).Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【删除模块功能】:", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "响应失败!";
            }
            return result;
        }

        /// <summary>
        /// 查询模块功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<object> SelectFunctionList(object model)
        {
            List<object> functionlist = new List<object>();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-查询模块功能", null, null);

                    //执行SQL脚本
                    functionlist = db.Sql(sqlStr).Parameters("Id", 0).GetModelList<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询模块功能】:", ex);
            }
            return functionlist;
        }

        /// <summary>
        /// 查询单个模块功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object SelectAloneFunction(object model)
        {
            object functionlist = new object();
            //提示信息
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置-查询单个模块功能", null, null);

                    //执行SQL脚本
                    functionlist = db.Sql(sqlStr).Parameters("Id", 0).GetModel<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("模块配置【查询单个模块功能】:", ex);
            }
            return functionlist;
        }
    }
}
