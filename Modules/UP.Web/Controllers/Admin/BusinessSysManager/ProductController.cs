using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary.Utils;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Interface.Admin.Org;
using UP.Models.DB.BusinessSys;
using UP.Web.Models.Admin.BussinessSys;
namespace UP.Web.Controllers.Admin.BusinessSysManager
{
    /// <summary>
    /// 产品管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class ProductController : BasicsController
    {
        /// <summary>
        /// 查询所有产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询所有产品", "查询所有产品", "胡家源", "2020-09-22")]
        public IActionResult GetProductList()
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询产品成功!");
            try
            {
              //查询状态为0和状态为1的所有产品
               var  row = this.Query<Product>().GetModelList().Where(d=>d.状态>=0).OrderBy(d=>d.排序);
               result.data = row;
            }
            catch (Exception ex)
            {
                LogError("查询产品失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 分页查询所有产品的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询所有产品的列表", "分页查询产品的列表", "胡家源", "2020-09-22")]
        public IActionResult GetProductPageList(BussinessSysPram model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化产品接口
                var product = this.GetInstance<IProduct>();
                //分页查询产品列表
                var productList = product.GetProductList(model.page_num, model.page_size, model.keyword,model.state)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", productList));
            }
            catch (Exception ex)
            {
                LogError("分页查询产品失败", ex);
                resModel.msg = "分页查询产品异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改产品", "新增或修改产品", "胡家源", "2020-09-22")]
        public IActionResult AddoUpdate(Product model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "保存产品成功!");
            var row = 0;
            try
            {
                //新增产品
                if (model.id == 0)
                {
                    //默认状态正常
                    model.状态 = 1;
                    row = this.Add(model).Execute();
                }
                //修改
                else
                {
                    row = this.Update(model).Columns("名称", "简称", "产品ip", "端口", "排序")
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
                LogError("保存产品失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 根据产品ID 查询产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据产品ID 查询产品信息", "根据产品ID查询产品信息", "胡家源", "2020-09-22")]
        public IActionResult GetProductInfo(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询产品成功!");

            try
            {
                if (model.productid != 0)
                {
                    //查询产品
                    var poduct = this.Query<Product>().Where("id", model.productid).GetModel();
                    //判断产品是否为空
                    if (poduct != null)
                    {
                        result.data = poduct;
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
        /// 根据产品修改产品状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据产品ID修改产品状态", "根据产品ID修改产品状态", "胡家源", "2020-09-22")]
        public IActionResult DelProductScoure(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "修改产品状态成功!");
            try
            {
                //修改产品状态
                Product product = new Product() { 状态 =model.state};
               var row = this.Update(product).Columns("状态")
                             .Where("id", model.productid).Execute();
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
