using QWPlatform.IService;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.Interface;
using QWPlatform.SystemLibrary;
using UP.Basics;
using QWPlatform.Models;

namespace UP.Logics.Admin.Interface
{
    /// <summary>
    /// 外部接口管理数据库访问层
    /// </summary>
    public class ExternalInterfaceLogic
    {
        #region 接口分组

        /// <summary>
        ///  获取外部接口分组树形数据
        /// </summary>
        /// <returns></returns>
        public List<InterfaceCatgory> GetInterfaceGroupTree()
        {
            try
            {
                using (var db = new DbContext())
                {
                    string sql = @"SELECT a.id,a.code,a.describe,a.name,a.parent_id,a.sno FROM intfc_catgory a";
                    var list = db.Sql(sql).GetModelList<InterfaceCatgory>();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询注册接口分组时发生异常：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增、修改接口分组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditInterfaceGroup(InterfaceCatgory model)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (string.IsNullOrEmpty(model.id))
                    {
                        return db.Insert(model).Execute() > 0;
                    }
                    return db.Update(model).Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("更新外部接口分组时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除指定接口分组
        /// </summary>
        /// <returns></returns>
        public bool DeleteInterfaceGroup(string id)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Delete("intfc_catgory")
                        .Where("id", id)
                        .Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除指定接口分组时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        ///  检验接口分组编码重复【同级】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ExistInterfaceGroupCode(string id, string parentId, string code)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var result = db.Select("intfc_catgory")
                        .Where("parent_id", parentId)
                        .Where("code", code).GetModel<InterfaceCatgory>();
                    if (result == null)
                        return false;
                    return result.id != id;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("检验接口分组编码重复时发生异常：", ex);
                return false;
            }
        }

        #endregion 接口分组

        #region 接口注册

        /// <summary>
        /// 获取外部接口注册列表
        /// </summary>
        /// <param name="catgoryId">分组ID</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <returns></returns>
        public ListPageModel<InterfaceItem> GetInterfaceList(string catgoryId, int pageNumber, int pageSize)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var list = db.Select("intfc_items")
                        .Where("catgory_id", catgoryId).Paging(pageNumber, pageSize)
                        .GetModelList<InterfaceItem>(out int total);
                    return new ListPageModel<InterfaceItem>
                    {
                        PageList = list,
                        Total = total
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取外部接口注册列表时发生异常：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增、修改外部接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditInterfaceItem(InterfaceItem model)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (string.IsNullOrEmpty(model.id))
                    {
                        return db.Insert(model).Execute() > 0;
                    }
                    return db.Update(model).Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("编辑外部接口时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除外部接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteInterfaceItem(string id)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Delete("intfc_items")
                        .Where("id", id)
                        .Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除指定接口分组时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取当前接口全路径
        /// </summary>
        /// <param name="catgoryId"></param>
        /// <param name="currName"></param>
        /// <returns></returns>
        public string GetCurrInterfacePath(string catgoryId, string currName)
        {
            string sql = @"WITH RECURSIVE cte AS (
	                        SELECT A.ID,
		                        A.code,
		                        A.PARENT_ID,
		                        CAST ( A.code AS VARCHAR ( 4000 ) ) AS PATH
	                        FROM
		                        intfc_catgory A
	                        WHERE
		                        A.parent_id IS NULL UNION ALL
	                        SELECT K.ID,
		                        K.code,
		                        K.PARENT_ID,
		                        CAST ( C.PATH || '/' || K.code AS VARCHAR ( 4000 ) ) AS PATH
	                        FROM
		                        intfc_catgory
		                        K INNER JOIN cte C ON C.ID = K.parent_id
	                        ) SELECT '/api/' || PATH || '/' || @currName AS PATH
                        FROM  cte  WHERE ID = @parentId";
            try
            {
                using (var db = new DbContext())
                {
                    var dt = db.Sql(sql)
                         .Parameters("currName", currName)
                         .Parameters("parentId", catgoryId)
                         .Select();
                    return dt?.Rows[0]["PATH"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询当前接口全路径时发生异常：", ex);
                return "";
            }
        }

        /// <summary>
        /// 是否存在相同接口编码【同分组下】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="catgoryId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ExistInterfaceCode(string id, string catgoryId, string code)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var result = db.Select("intfc_items")
                        .Where("catgory_id", catgoryId)
                        .Where("code", code).GetModel<InterfaceItem>();
                    if (result == null)
                        return false;
                    return result.id != id;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("检验接口编码重复时发生异常：", ex);
                return false;
            }
        }

        #endregion 接口注册

        #region 接口参数

        /// <summary>
        /// 获取外部注册接口参数列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>

        public List<InterfaceParam> GetInterfaceParamList(string interfaceId)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Select("intfc_parameters")
                        .Where("items_id", interfaceId)
                        .GetModelList<InterfaceParam>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取外部接口参数列表时发生异常：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增、修改外部注册接口参数
        /// </summary>
        /// <returns></returns>
        public bool EditInterfaceParam(InterfaceParam param)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (string.IsNullOrEmpty(param.id))
                    {
                        return db.Insert(param).Execute() > 0;
                    }
                    return db.Update(param).Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("编辑外部接口参数时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除外部注册接口参数
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public bool DeleteInterfaceParam(string paramId)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Delete("intfc_parameters")
                        .Where("id", paramId)
                        .Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除指定接口参数时发生异常：", ex);
                return false;
            }
        }

        #endregion 接口参数

        #region 同步方法

        /// <summary>
        /// 获取外部注册接口同步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public List<InterfaceSync> GetInterfaceSynFunList(string interfaceId)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Select("intfc_sync")
                        .Where("items_id", interfaceId)
                        .GetModelList<InterfaceSync>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取外部接口同步方法列表时发生异常：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增、编辑外部注册接口同步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool EditInterfaceSynFun(InterfaceSync param)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (string.IsNullOrEmpty(param.id))
                    {
                        return db.Insert(param).Execute() > 0;
                    }
                    return db.Update(param).Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("编辑外部接口同步方法时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除外部注册接口同步方法
        /// </summary>
        /// <param name="synFunId"></param>
        /// <returns></returns>
        public bool DeleteInterfaceSynFun(string synFunId)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Delete("intfc_sync")
                        .Where("id", synFunId)
                        .Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除指定接口同步方法时发生异常：", ex);
                return false;
            }
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 获取外部注册接口异步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public List<InterfaceAsync> GetInterfaceAsyncFunList(string interfaceId)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Select("intfc_async")
                        .Where("items_id", interfaceId)
                        .GetModelList<InterfaceAsync>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取外部接口异步方法列表时发生异常：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增、编辑外部注册接口异步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool EditInterfaceAsyncFun(InterfaceAsync param)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (string.IsNullOrEmpty(param.id))
                    {
                        return db.Insert(param).Execute() > 0;
                    }
                    return db.Update(param).Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("编辑外部接口异步方法时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除外部注册接口异步方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteInterfaceAsyncFun(string id)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Delete("intfc_async")
                        .Where("id", id)
                        .Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除指定接口异步方法时发生异常：", ex);
                return false;
            }
        }

        #endregion 异步方法

        #region 接口授权

        /// <summary>
        /// 获取外部注册接口授权列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public List<InterfaceAuth> GetInterfaceAuthList(string interfaceId)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Select("intfc_auth")
                        .Where("items_id", interfaceId)
                        .GetModelList<InterfaceAuth>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取外部接口异步方法列表时发生异常：", ex);
                return null;
            }
        }

        /// <summary>
        /// 新增、修改外部注册接口授权列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool EditInterfaceAuth(InterfaceAuth param)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (string.IsNullOrEmpty(param.id))
                    {
                        return db.Insert(param).Execute() > 0;
                    }
                    return db.Update(param).Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("编辑外部接口授权时发生异常：", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除外部注册接口授权
        /// </summary>
        /// <param name="id">授权ID</param>
        /// <returns></returns>
        public bool DeleteInterfaceAuth(string id)
        {
            try
            {
                using (var db = new DbContext())
                {
                    return db.Delete("intfc_auth")
                        .Where("id", id)
                        .Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除指定接口授权时发生异常：", ex);
                return false;
            }
        }

        #endregion 接口授权
    }
}