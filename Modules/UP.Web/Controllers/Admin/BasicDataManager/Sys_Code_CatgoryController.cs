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
    /// 基础数据分类
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class Sys_Code_CatgoryController : BasicsController
    {
        /// <summary>
        /// 查询基础数据分类列表(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询基础数据分类列表", "查询基础数据分类列表", "胡家源", "2020-09-28")]
        public IActionResult GetCatgoryList()
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化基础数据分类接口
                var Catgory = this.GetInstance<ISys_Code_Catgory>();
                //查询基础数据分类列表
                var CatgoryList = Catgory.GetSys_Code_CatgoryList()?.Result;
                resModel.code = (int)ResponseCode.Success;
                resModel.data = CatgoryList;
                resModel.msg = "查询成功!";
            }
            catch (Exception ex)
            {
                LogError("查询基础数据分类列表失败", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改基础数据分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改基础数据分类", "新增或修改基础数据分类", "胡家源", "2020-09-28")]
        public IActionResult AddorUpdate(sys_code_catgory model)
        {
            //实例化基础数据分类接口
            var Catgory = this.GetInstance<ISys_Code_Catgory>();
            //查询基础数据分类列表
            var result = Catgory.AddorUpdate(model)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据id查询类型分类
        /// </summary>
        /// <param name="id">guid</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id查询类型分类", "根据id查询类型分类", "胡家源", "2020-09-28")]
        public IActionResult GetCatgory(BasicDataParam param)
        {
            //实例化基础数据分类接口
            var Catgory = this.GetInstance<ISys_Code_Catgory>();
            //查询基础数据分类信息
            var result = Catgory.GetCatgory(param.id)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据id修改分类状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id修改分类状态", "根据id修改分类状态", "胡家源", "2020-09-28")]
        public IActionResult UpdateCatgoryState(BasicDataParam param)
        {
            //实例化基础数据分类接口
            var Catgory = this.GetInstance<ISys_Code_Catgory>();
            //根据id修改分类状态
            var result = Catgory.UpdateCatgoryState(param.id, param.state)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 验证名称是否重复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority(" 验证名称是否重复", " 验证名称是否重复", "胡家源", "2020-09-29")]
        public IActionResult CheckName(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "验证名称重复失败");
            try
            {
                //判断分类名称是否存在
                var istrue = false;
                var dataList = this.Query<sys_code_catgory>().Where("name", param.name).GetModelList();
                if (!string.IsNullOrEmpty(param.id))
                {
                    istrue = dataList != null && dataList.Any(d => d.id != param.id);
                }
                else
                {
                    istrue = dataList != null && dataList.Any();
                }
                resModel.code = (int)ResponseCode.Success;
                resModel.msg = "验证成功";
                resModel.data = istrue;
            }
            catch (Exception ex)
            {
                LogError("验证名称重复失败", ex);
                resModel.msg = "内部异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 验证动态表名是否重复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority(" 验证动态表名是否重复", " 验证动态表名是否重复", "胡家源", "2020-09-30")]
        public IActionResult CheckTableName(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "验证动态表名重复失败");
            try
            {
                //判断动态表名是否存在
                var istrue = false;
                var dataList = this.Query<sys_code_catgory>().Where("ref_table", param.name).GetModelList();
                if (!string.IsNullOrEmpty(param.id))
                {
                    istrue = dataList != null && dataList.Any(d => d.id != param.id);
                }
                else
                {
                    istrue = dataList != null && dataList.Any();
                }
                resModel.code = (int)ResponseCode.Success;
                resModel.msg = "验证成功";
                resModel.data = istrue;
            }
            catch (Exception ex)
            {
                LogError("验证动态表名重复失败", ex);
                resModel.msg = "内部异常";
            }
            return Json(resModel);
        }
    }
}
