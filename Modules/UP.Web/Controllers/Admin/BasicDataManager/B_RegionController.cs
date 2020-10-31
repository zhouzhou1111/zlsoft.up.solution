using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.Models.Common;
using UP.Basics;
using UP.Interface.Admin.BasicData;
using UP.Models.DB.BasicData;
using UP.Web.Models.Admin.BasicData;

namespace UP.Web.Controllers.Admin.BasicDataManager
{
    /// <summary>
    /// 行政区划管理(胡家源)
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class B_RegionController : BasicsController
    {
        /// <summary>
        /// 查询行政区划列表(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询行政区划列表", "查询行政区划列表", "胡家源", "2020-10-09")]
        public IActionResult GetRegionList(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化行政区划接口
                var Region = this.GetInstance<IB_Region>();
                //查询行政区划列表
                var RegionList = Region.GetRegionList(param.pcode,param.prop,param.code)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", RegionList));
            }
            catch (Exception ex)
            {
                LogError("查询行政区划列表失败", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改行政区划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改行政区划", "新增或修改行政区划", "胡家源", "2020-10-09")]
        public IActionResult AddorUpdate(b_region model)
        {
            //实例化行政区划接口
            var Region = this.GetInstance<IB_Region>();
            //查询行政区划列表
            var result = Region.AddorUpdate(model)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据id查询行政区划
        /// </summary>
        /// <param name="id">guid</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id查询行政区划", "根据id查询行政区划", "胡家源", "2020-10-09")]
        public IActionResult GetRegion(BasicDataParam param)
        {
            //实例化行政区划接口
            var Region = this.GetInstance<IB_Region>();
            //查询行政区划信息
            var result = Region.GetRegion(param.id)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据id修改行政区划状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id修改行政区划状态", "根据id修改行政区划状态", "胡家源", "2020-10-09")]
        public IActionResult UpdateRegionState(BasicDataParam param)
        {
            //实例化行政区划接口
            var Region = this.GetInstance<IB_Region>();
            //修改行政区划状态
            var result = Region.UpdateRegionState(param.id, param.state)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 验证编码是否重复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority(" 验证编码是否重复", " 验证编码是否重复", "胡家源", "2020-10-09")]
        public IActionResult CheckCode(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "验证编码重复失败");
            //实例化行政区划接口
            var Region = this.GetInstance<IB_Region>();
            //验证编码是否重复
            var result = Region.CheckCode(param.code, param.id)?.Result;
            return Json(result);
        } 
    }
}
