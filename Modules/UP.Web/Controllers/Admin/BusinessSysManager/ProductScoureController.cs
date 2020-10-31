using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary.Utils;
using System;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Models.DB.BusinessSys;
using UP.Web.Models.Admin.BussinessSys;

namespace UP.Web.Controllers.Admin.BusinessSysManager
{
    /// <summary>
    /// 产品服务源管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class ProductScoureController : BasicsController
    {
        /// <summary>
        /// 分页查询所有产品服务源的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询所有产品服务源的列表", "分页查询产品服务源的列表", "胡家源", "2020-09-21")]
        public IActionResult GetProductScoureList(BussinessSysPram model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化服务源接口
                var productScoure = this.GetInstance<IProductSource>();
                //分页查询服务源列表
                var productScoureList = productScoure.GetProductSourceList(model.page_num, model.page_size, model.keyword, model.productid)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", productScoureList));
            }
            catch (Exception ex)
            {
                LogError("分页查询产品服务源失败", ex);
                resModel.msg = "分页查询产品服务源异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改服务源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改服务源", "新增或修改服务源", "胡家源", "2020-09-21")]
        public IActionResult AddoUpdate(ProductSource model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "保存服务源成功!");
            var row = 0;
            try
            {
                //新增服务源
                if (model.id == 0)
                {
                    model.密码 = Strings.StrToMD5(model.密码);
                    row = this.Add(model).Execute();
                }
                //修改
                else
                {
                    row = this.Update(model).Columns("产品id", "名称", "服务地址", "授权码", "用户名", "授权方式")
                            .Where("id", model.id).Execute();
                }
                if (row < 1)
                {
                    result.msg = "服务器内部异常!";
                    result.code = (int)ResponseCode.Error;
                }
            }
            catch (Exception ex)
            {
                LogError("保存服务源失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 根据服务源ID 查询服务源信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据服务源ID 查询服务源信息", "根据服务源ID查询服务源信息", "胡家源", "2020-09-21")]
        public IActionResult GetProductScoureInfo(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询服务源成功!");

            try
            {
                if (model.productScoureid != 0)
                {
                    //查询服务源
                    var poductScoure = this.Query<ProductSource>().Where("id", model.productScoureid).GetModel();
                    //判断服务源是否为空
                    if (poductScoure != null)
                    {
                        result.data = poductScoure;
                    }
                    else
                    {
                        result.msg = "服务器内部异常";
                        result.code = (int)ResponseCode.Error;
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
                //记录日志
                LogError("传入参数异常", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 根据服务源ID删除服务源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据服务源ID删除服务源", "根据服务源ID删除服务源", "胡家源", "2020-09-21")]
        public IActionResult DelProductScoure(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "删除服务源成功!");
            try
            {
                //删除服务源
                var row = this.Delete<ProductSource>().Where("id", model.productScoureid).Execute();
                if (row < 1)
                {
                    result.code = (int)ResponseCode.Error;
                    result.msg = "服务源删除失败";
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
        /// 重置服务源密码(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("重置服务源密码", "重置服务源密码", "胡家源", "2020-09-21")]
        public IActionResult ResetPassword(BussinessSysPram model)
        {
            var result = new ResponseModel(ResponseCode.Success, "重置成功!");
            try
            {
                if (model.productScoureid == 0)
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "缺少参数";
                    return Json(result);
                }
                //通过机构人员id查询账户
                ProductSource ProductSource = new ProductSource()
                {
                    密码 = Strings.StrToMD5("123456")
                };
                var count = this.Update(ProductSource).Columns("密码").Where("id", model.productScoureid).Execute();
                if (count > 0)
                {
                    result.code = (int)ResponseCode.Success;
                }
                else
                {
                    result.code = (int)ResponseCode.Error;
                    result.msg = "重置失败";
                }
            }
            catch (Exception ex)
            {
                LogError("重置服务源密码操作失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }
    }
}