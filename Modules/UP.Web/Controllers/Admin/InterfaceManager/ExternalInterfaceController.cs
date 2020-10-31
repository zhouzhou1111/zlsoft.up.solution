using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Interface;
using UP.Models.DB.Interface;
using UP.Web.Models.Admin.Interface;

namespace UP.Web.Controllers.Admin.InterfaceManager
{
    /// <summary>
    /// 外部接口管理控制器 ( 肖烜 )
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class ExternalInterfaceController : BasicsController
    {
        #region 接口分类

        /// <summary>
        /// 获取接口分组树形数据接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("获取接口分组树形数据接口", "获取接口分组树形数据接口", "肖烜", "2020-10-14")]
        public IActionResult GetInterfaceGroupTree()
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.GetInterfaceGroupTree()?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("获取外部注册接口分组发生异常，", ex);
                resModel.msg = "获取外部注册接口分组发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增、编辑接口分组接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增、编辑接口分组接口", "新增、编辑接口分组接口", "肖烜", "2020-10-14")]
        public IActionResult EditInterfaceGroup(InterfaceCatgoryParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var model = external.EditInterfaceGroup(param.ConvertToDBModel())?.Result;
                return Json(model);
            }
            catch (Exception ex)
            {
                LogError("编辑接口分组时发生异常，", ex);
                resModel.msg = "编辑接口分组时发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除接口分组接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RIPAuthority("删除接口分组接口", "删除接口分组接口", "肖烜", "2020-10-14")]
        public IActionResult DeleteInterfaceGroup(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var model = external.DeleteInterfaceGroup(id)?.Result;
                return Json(model);
            }
            catch (Exception ex)
            {
                LogError("删除接口分组时发生异常，", ex);
                resModel.msg = "删除接口分组时发生异常";
            }
            return Json(resModel);
        }

        #endregion 接口分类

        #region 接口注册

        /// <summary>
        /// 根据分组ID获取接口注册列表[分页]
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("获取接口注册列表", "获取接口注册列表", "肖烜", "2020-10-14")]
        public IActionResult GetInterfaceList(InterfaceItemSearchParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.GetInterfaceList(param.CatgoryId, param.PageNumber, param.PageSize)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("获取接口注册列表时发生异常，", ex);
                resModel.msg = "获取接口注册列表时发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增、修改外部注册接口   [自动生成接口路径]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增、修改外部注册接口", "新增、修改外部注册接口", "肖烜", "2020-10-14")]
        public IActionResult EditInterfaceItem(InterfaceItemEditParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var model = external.EditInterfaceItem(param.ConvertToDBModel())?.Result;
                return Json(model);
            }
            catch (Exception ex)
            {
                LogError("编辑注册接口时发生异常，", ex);
                resModel.msg = "编辑注册接口时发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除外部注册接口
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [RIPAuthority("删除外部注册接口", "删除外部注册接口", "肖烜", "2020-10-14")]
        public IActionResult DeleteInterfaceItem(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var model = external.DeleteInterfaceItem(id)?.Result;
                return Json(model);
            }
            catch (Exception ex)
            {
                LogError("删除外部注册接口时发生异常，", ex);
                resModel.msg = "删除外部注册接口时发生异常";
            }
            return Json(resModel);
        }

        #endregion 接口注册

        #region 接口参数

        /// <summary>
        /// 获取外部注册接口参数列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("获取外部注册接口参数列表", "获取外部注册接口参数列表", "肖烜", "2020-10-14")]
        public IActionResult GetInterfaceParamList(string interfaceId)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.GetInterfaceParamList(interfaceId)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("获取外部注册接口参数列表发生异常，", ex);
                resModel.msg = "获取外部注册接口参数列表发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增、修改外部注册接口参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("新增、修改外部注册接口参数", "新增、修改外部注册接口参数", "肖烜", "2020-10-14")]
        public IActionResult EditInterfaceParam(InterfaceParamEditParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.EditInterfaceParam(param.ConvertToDBModel())?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("编辑外部注册接口参数发生异常，", ex);
                resModel.msg = "编辑外部注册接口参数发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除外部注册接口参数
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        [HttpDelete]
        [RIPAuthority("删除外部注册接口参数", "删除外部注册接口参数", "肖烜", "2020-10-14")]
        public IActionResult DeleteInterfaceParam(string paramId)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.DeleteInterfaceParam(paramId)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("删除外部注册接口参数发生异常，", ex);
                resModel.msg = "删除外部注册接口参数发生异常";
            }
            return Json(resModel);
        }

        #endregion 接口参数

        #region 同步方法

        /// <summary>
        /// 获取外部注册接口同步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("获取外部注册接口同步方法列表", "获取外部注册接口同步方法列表", "肖烜", "2020-10-14")]
        public IActionResult GetInterfaceSynFunList(string interfaceId)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.GetInterfaceSynFunList(interfaceId)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("获取外部注册接口同步方法列表发生异常，", ex);
                resModel.msg = "获取外部注册接口同步方法列表发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增、编辑外部注册接口同步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("新增、编辑外部注册接口同步方法", "新增、编辑外部注册接口同步方法", "肖烜", "2020-10-14")]
        public IActionResult EditInterfaceSynFun(InterfaceSyncEditParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.EditInterfaceSynFun(param.ConvertToDBModel())?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("编辑外部注册接口同步方法发生异常，", ex);
                resModel.msg = "编辑外部注册接口同步方法发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除外部注册接口同步方法
        /// </summary>
        /// <param name="synFunId"></param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("删除外部注册接口同步方法", "删除外部注册接口同步方法", "肖烜", "2020-10-14")]
        public IActionResult DeleteInterfaceSynFun(string synFunId)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.DeleteInterfaceSynFun(synFunId)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("删除外部注册接口同步方法发生异常，", ex);
                resModel.msg = "删除外部注册接口同步方法发生异常";
            }
            return Json(resModel);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 获取外部注册接口异步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("获取外部注册接口异步方法列表", "获取外部注册接口异步方法列表", "肖烜", "2020-10-14")]
        public IActionResult GetInterfaceAsyncFunList(string interfaceId)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.GetInterfaceAsyncFunList(interfaceId)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("获取外部注册接口异步方法列表发生异常，", ex);
                resModel.msg = "获取外部注册接口异步方法列表发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增、编辑外部注册接口异步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增、编辑外部注册接口异步方法", "新增、编辑外部注册接口异步方法", "肖烜", "2020-10-14")]
        public IActionResult EditInterfaceAsyncFun(InterfaceAsyncEditParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.EditInterfaceAsyncFun(param.ConvertToDBModel())?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("编辑外部注册接口异步方法列表发生异常，", ex);
                resModel.msg = "编辑外部注册接口异步方法列表发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除外部注册接口异步方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [RIPAuthority("删除外部注册接口异步方法", "删除外部注册接口异步方法", "肖烜", "2020-10-14")]
        public IActionResult DeleteInterfaceAsyncFun(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.DeleteInterfaceAsyncFun(id)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("删除外部注册接口异步方法列表发生异常，", ex);
                resModel.msg = "删除外部注册接口异步方法列表发生异常";
            }
            return Json(resModel);
        }

        #endregion 异步方法

        #region 接口授权

        /// <summary>
        /// 获取外部注册接口授权列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("获取外部注册接口授权列表", "获取外部注册接口授权列表", "肖烜", "2020-10-14")]
        public IActionResult GetInterfaceAuthList(string interfaceId)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.GetInterfaceAuthList(interfaceId)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("获取外部注册接口授权列表发生异常，", ex);
                resModel.msg = "获取外部注册接口授权列表发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增、修改外部注册接口授权列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增、修改外部注册接口授权列表", "新增、修改外部注册接口授权列表", "肖烜", "2020-10-14")]
        public IActionResult EditInterfaceAuth(InterfaceAuthEditParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.EditInterfaceAuth(param.ConvertToDBModel())?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("编辑外部注册接口授权列表发生异常，", ex);
                resModel.msg = "编辑外部注册接口授权列表发生异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除外部注册接口授权
        /// </summary>
        /// <param name="id">授权ID</param>
        /// <returns></returns>
        [HttpGet]
        [RIPAuthority("删除外部注册接口授权", "删除外部注册接口授权", "肖烜", "2020-10-14")]
        public IActionResult DeleteInterfaceAuth(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var external = this.GetInstance<IExternalInterface>();
                var result = external.DeleteInterfaceAuth(id)?.Result;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogError("删除外部注册接口授权列表发生异常，", ex);
                resModel.msg = "删除外部注册接口授权列表发生异常";
            }
            return Json(resModel);
        }

        #endregion 接口授权
    }
}