using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Interface;
using UP.Logics.Admin.Interface;
using UP.Models.DB.Interface;

namespace UP.Grains.Admin.Interface
{
    /// <summary>
    /// 外部接口管理业务逻辑
    /// </summary>
    public class ExternalInterfaceGrains : BasicGrains<ExternalInterfaceLogic>, IExternalInterface
    {
        #region 接口分组

        /// <summary>
        /// 获取外部接口分组树形数据
        /// </summary>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceGroupTree()
        {
            var list = Logic.GetInterfaceGroupTree();
            return Task.FromResult(new ResponseModel(ResponseCode.Success, "查询成功", list));
        }

        /// <summary>
        /// 新增、修改接口分类
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceGroup(InterfaceCatgory model)
        {
            var exist = Logic.ExistInterfaceGroupCode(model.id, model.parent_id, model.code);
            if (exist)
            {
                return Task.FromResult(new ResponseModel(ResponseCode.Error, "同级分组不允许编码相同"));
            }

            return Logic.EditInterfaceGroup(model) ?
                 Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功")) :
                 Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        /// <summary>
        /// 删除接口分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceGroup(string id)
        {
            return Logic.DeleteInterfaceGroup(id) ?
                Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功")) :
                Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
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
        public Task<ResponseModel> GetInterfaceList(string catgoryId, int pageNumber, int pageSize)
        {
            var result = Logic.GetInterfaceList(catgoryId, pageNumber, pageSize);
            if (result == null)
                return Task.FromResult(new ResponseModel(ResponseCode.Error, "查询列表时发生异常，详情见日志！"));
            return Task.FromResult(new ResponseModel(ResponseCode.Success, "查询成功", result));
        }

        /// <summary>
        /// 新增、修改外部接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceItem(InterfaceItem model)
        {
            //1. 获取当前最新的接口全路径
            var path = Logic.GetCurrInterfacePath(model.catgory_id, model.code);
            model.path = path;
            //2. 判断是否重复编码【相同分组】
            var exist = Logic.ExistInterfaceCode(model.id, model.catgory_id, model.code);
            if (exist)
            {
                return Task.FromResult(new ResponseModel(ResponseCode.Error, "接口编码重复"));
            }
            return Logic.EditInterfaceItem(model) ?
                  Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
                  : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        /// <summary>
        /// 删除外部接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceItem(string id)
        {
            return Logic.DeleteInterfaceItem(id) ?
               Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
               : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        #endregion 接口注册

        #region 接口参数

        /// <summary>
        /// 获取外部注册接口参数列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>

        public Task<ResponseModel> GetInterfaceParamList(string interfaceId)
        {
            var result = Logic.GetInterfaceParamList(interfaceId);
            return Task.FromResult(new ResponseModel(ResponseCode.Success, "查询成功", result));
        }

        /// <summary>
        /// 新增、修改外部注册接口参数
        /// </summary>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceParam(InterfaceParam param)
        {
            return Logic.EditInterfaceParam(param) ?
                   Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
                   : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        /// <summary>
        /// 删除外部注册接口参数
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceParam(string paramId)
        {
            return Logic.DeleteInterfaceParam(paramId) ?
               Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
               : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        #endregion 接口参数

        #region 同步方法

        /// <summary>
        /// 获取外部注册接口同步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceSynFunList(string interfaceId)
        {
            var result = Logic.GetInterfaceSynFunList(interfaceId);
            return Task.FromResult(new ResponseModel(ResponseCode.Success, "查询成功", result));
        }

        /// <summary>
        /// 新增、编辑外部注册接口同步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceSynFun(InterfaceSync param)
        {
            return Logic.EditInterfaceSynFun(param) ?
                  Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
                  : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        /// <summary>
        /// 删除外部注册接口同步方法
        /// </summary>
        /// <param name="synFunId"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceSynFun(string synFunId)
        {
            return Logic.DeleteInterfaceSynFun(synFunId) ?
           Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
           : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 获取外部注册接口异步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceAsyncFunList(string interfaceId)
        {
            var result = Logic.GetInterfaceAsyncFunList(interfaceId);
            return Task.FromResult(new ResponseModel(ResponseCode.Success, "查询成功", result));
        }

        /// <summary>
        /// 新增、编辑外部注册接口异步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceAsyncFun(InterfaceAsync param)
        {
            return Logic.EditInterfaceAsyncFun(param) ?
                Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
                : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        /// <summary>
        /// 删除外部注册接口异步方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceAsyncFun(string id)
        {
            return Logic.DeleteInterfaceAsyncFun(id) ?
                 Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
                 : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        #endregion 异步方法

        #region 接口授权

        /// <summary>
        /// 获取外部注册接口授权列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceAuthList(string interfaceId)
        {
            var result = Logic.GetInterfaceAuthList(interfaceId);
            return Task.FromResult(new ResponseModel(ResponseCode.Success, "查询成功", result));
        }

        /// <summary>
        /// 新增、修改外部注册接口授权列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceAuth(InterfaceAuth param)
        {
            return Logic.EditInterfaceAuth(param) ?
                Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
                : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        /// <summary>
        /// 删除外部注册接口授权
        /// </summary>
        /// <param name="id">授权ID</param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceAuth(string id)
        {
            return Logic.DeleteInterfaceAuth(id) ?
            Task.FromResult(new ResponseModel(ResponseCode.Success, "操作成功"))
            : Task.FromResult(new ResponseModel(ResponseCode.Error, "操作失败"));
        }

        #endregion 接口授权
    }
}