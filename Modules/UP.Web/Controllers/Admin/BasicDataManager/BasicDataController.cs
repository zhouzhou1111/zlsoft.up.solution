using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UP.Basics;
using UP.Interface.Admin.BasicData;
using UP.Models.Admin.BasicData;
using UP.Web.Models.Admin.BasicData;

namespace UP.Web.Controllers.Admin.BasicDataManager
{
    /// <summary>
    /// 基础数据管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class BasicDataController : BasicsController
    {
        /// <summary>
        /// 分页查询国籍的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("分页查询基础数据的列表", "分页查询基础数据列表", "胡家源", "2020-09-27")]
        public IActionResult GetBasicDataList(BasicDataParam model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化基础数据接口
                var basicdata = this.GetInstance<IBasicData>();
                //分页查询基础数据列表
                var dataList = basicdata.GetBasicDataList(model.page_num, model.page_size, model.keyword, model.tabletype)?.Result;
                return Json( dataList);
            }
            catch (Exception ex)
            {
                LogError("分页查询产品国籍失败", ex);
                resModel.msg = "分页查询产品国籍异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改国籍
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改基础数据", "新增或修改基础数据", "胡家源", "2020-09-24")]
        public IActionResult AddoUpdate(BasicDataDto model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "保存基础数据失败!");
            try
            {
                //重置简码
                model.简码 = Basics.Utils.Strings.GetFirstPY(model.名称.Trim());
                //实例化基础数据接口
                var BasicData = this.GetInstance<IBasicData>();
                result = BasicData.UpdateDicItems(model.tabletype, model.编码, model.名称, model.简码, model.oldcode,model.typeData)?.Result;
            }
            catch (Exception ex)
            {
                LogError("服务器异常,保存基础数据", ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取基础数据信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询基础数据信息", "查询基础数据信息", "胡家源", "2020-09-27")]
        public IActionResult GetBasicInfo(BasicDataDto model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "获取基础数据信息失败!");
            try
            {
                //实例化基础数据接口
                var BasicData = this.GetInstance<IBasicData>();
                result = BasicData.GetBasicDataInfo(model.编码,model.tabletype)?.Result;
            }
            catch (Exception ex)
            {
                LogError("服务器异常,查询基础数据失败", ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 删除基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除基础数据", "删除基础数据", "胡家源", "2020-09-27")]
        public IActionResult DelBasicInfo(BasicDataDto model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "删除基础数据失败!");
            try
            {
                //实例化基础数据接口
                var BasicData = this.GetInstance<IBasicData>();
                result = BasicData.DelBasicData(model.编码, model.tabletype)?.Result;
            }
            catch (Exception ex)
            {
                LogError("服务器异常,删除基础数据失败", ex);
            }
            return Json(result);
        }
    }
}
