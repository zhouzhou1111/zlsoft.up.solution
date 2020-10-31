using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary.Utils;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Models.DB.BusinessSys;
using UP.Web.Models.Admin.BussinessSys;
using System.Security.Cryptography;

namespace UP.Web.Controllers.Admin.BusinessSysManager
{
    /// <summary>
    /// 产品应用信息管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class AppInformationController : BasicsController
    {
        /// <summary>
        /// 分页查询所有产品应用信息的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询所有产品应用信息的列表", "分页查询产品应用信息的列表", "胡家源", "2020-09-23")]
        public IActionResult GetAppInformationList(BussinessSysPram model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化应用信息接口
                var AppInformation = this.GetInstance<IAppInformation>();
                //分页查询应用信息列表
                var AppInformationList = AppInformation.GetAppInformationList(model.page_num, model.page_size, model.keyword,model.productid)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", AppInformationList));
            }
            catch (Exception ex)
            {
                LogError("分页查询产品应用信息失败", ex);
                resModel.msg = "分页查询产品应用信息异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改应用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改应用信息", "新增或修改应用信息", "胡家源", "2020-09-23")]
        public IActionResult AddoUpdate(AppInformation model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "保存应用信息失败!");
            var row = 0;
            try
            {
                var datetime = DateTime.Now;
                //新增应用信息
                if (string.IsNullOrEmpty(model.appid))
                {
                    //生成公钥,私钥
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    model.公钥 = Convert.ToBase64String(rsa.ExportCspBlob(false));
                    model.私钥 = Convert.ToBase64String(rsa.ExportCspBlob(true));
                    model.appid = string.Format("{0:yyyyMMddHHmmssf}", datetime);
                    //判断appid是否存在
                    var appinfo = this.Query<AppInformation>().Where("appid", model.appid).Exists();
                    if (appinfo)
                    {
                        result.msg = "appid重复,请重新保存!";
                        return Json(result);
                    }
                    model.数据标识 = 1;
                    model.登记人id = loginUser.id;
                    model.登记时间 = datetime;
                    row = this.Add(model).Execute();
                }
                //修改
                else
                {
                    //修改变更人,变更时间
                    model.变更人id = loginUser.id;
                    model.变更时间 = DateTime.Now;
                    row = this.Update(model).Columns("应用名称", "应用状态", "变更时间", "变更人id", "使用平台", "产品id", "有效期")
                            .Where("appid", model.appid).Execute();
                }
                if (row >0)
                {
                    result.msg = "保存应用信息成功!";
                    result.code = (int)ResponseCode.Error;
                }
            }
            catch (Exception ex)
            {
                LogError("保存应用信息失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 根据应用信息ID 查询应用信息信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据应用信息ID 查询应用信息信息", "根据应用信息ID查询应用信息信息", "胡家源", "2020-09-23")]
        public IActionResult GetAppInformationInfo(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询应用信息失败!");

            try
            {
                if (!model.appid.IsNullOrEmpty())
                {
                    //查询应用信息
                    var AppInformation = this.Query<AppInformation>().Where("appid", model.appid).GetModel();
                    //判断应用信息是否为空
                    if (AppInformation != null)
                    {
                        result.data = AppInformation;
                        result.msg = "查询应用信息成功";
                        result.code = (int)ResponseCode.Success;
                    }
                    else
                    {
                        result.msg = "服务器内部异常";
                    }
                }
                else
                {
                    result.msg = "传入参数异常";
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogError("传入参数异常", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }


        /// <summary>
        /// 根据应用信息ID删除应用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据应用信息ID删除应用信息", "根据应用信息ID删除应用信息", "胡家源", "2020-09-23")]
        public IActionResult DelAppInformation(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "删除应用信失败");
            try
            {
                //根据appid获取应用消息
                if (!model.appid.IsNullOrEmpty())
                {
                    //查询应用信息
                    var AppInformation = this.Query<AppInformation>().Where("appid", model.appid).GetModel();
                    //判断应用信息是否为空
                    if (AppInformation != null)
                    {
                        //删除应用信息
                        AppInformation.数据标识 = 0;
                        var row = this.Update(AppInformation).Columns("数据标识").Where("appid", model.appid).Execute();
                        if (row >0)
                        {
                            result.code = (int)ResponseCode.Success;
                            result.msg = "应用信息删除成功";
                        }
                    }
                    else
                    {
                        result.msg = "服务器内部异常";
                    }
                }
                else
                {
                    result.msg = "传入参数异常";
                    result.code = (int)ResponseCode.Error;
                }
            }
            catch (Exception ex)
            {
                LogError("传入参数异常", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }


        /// <summary>
        /// 根据应用信息appid修改应用状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据应用信息appid修改应用状态", "根据应用信息appid修改应用状态", "胡家源", "2020-09-23")]
        public IActionResult UpdateAppInformationState(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "修改应用信息状态成功!");
            try
            {
                //修改产品状态
                AppInformation AppInformation = new AppInformation() {  应用状态 = model.state };
                var row = this.Update(AppInformation).Columns("应用状态")
                              .Where("appid", model.appid).Execute();
                if (row < 1)
                {
                    result.msg = "服务器内部异常!";
                    result.code = (int)ResponseCode.Error;
                }

            }
            catch (Exception ex)
            {
                LogError("传入参数异常", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }
    }
}
