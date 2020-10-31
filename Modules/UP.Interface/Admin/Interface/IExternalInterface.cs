using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.DB.Interface;

namespace UP.Interface.Admin.Interface
{
    /// <summary>
    /// 外部接口管理接口
    /// </summary>
    public interface IExternalInterface : IBasic
    {
        #region 接口分组

        /// <summary>
        /// 获取外部接口分组树形数据
        /// </summary>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceGroupTree();

        /// <summary>
        /// 新增、修改接口分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceGroup(InterfaceCatgory model);

        /// <summary>
        /// 删除接口分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceGroup(string id);

        #endregion 接口分组

        #region 接口注册

        /// <summary>
        /// 获取外部接口注册列表
        /// </summary>
        /// <param name="catgoryId">分组ID</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceList(string catgoryId, int pageNumber, int pageSize);

        /// <summary>
        /// 新增、修改外部接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceItem(InterfaceItem model);

        /// <summary>
        /// 删除外部接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceItem(string id);

        #endregion 接口注册

        #region 接口参数

        /// <summary>
        /// 获取外部注册接口参数列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>

        public Task<ResponseModel> GetInterfaceParamList(string interfaceId);

        /// <summary>
        /// 新增、修改外部注册接口参数
        /// </summary>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceParam(InterfaceParam param);

        /// <summary>
        /// 删除外部注册接口参数
        /// </summary>
        /// <param name="paramId"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceParam(string paramId);

        #endregion 接口参数

        #region 同步方法

        /// <summary>
        /// 获取外部注册接口同步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceSynFunList(string interfaceId);

        /// <summary>
        /// 新增、编辑外部注册接口同步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceSynFun(InterfaceSync param);

        /// <summary>
        /// 删除外部注册接口同步方法
        /// </summary>
        /// <param name="synFunId"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceSynFun(string synFunId);

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 获取外部注册接口异步方法列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceAsyncFunList(string interfaceId);

        /// <summary>
        /// 新增、编辑外部注册接口异步方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceAsyncFun(InterfaceAsync param);

        /// <summary>
        /// 删除外部注册接口异步方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceAsyncFun(string id);

        #endregion 异步方法

        #region 接口授权

        /// <summary>
        /// 获取外部注册接口授权列表
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        public Task<ResponseModel> GetInterfaceAuthList(string interfaceId);

        /// <summary>
        /// 新增、修改外部注册接口授权列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ResponseModel> EditInterfaceAuth(InterfaceAuth param);

        /// <summary>
        /// 删除外部注册接口授权
        /// </summary>
        /// <param name="id">授权ID</param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteInterfaceAuth(string id);

        #endregion 接口授权
    }
}