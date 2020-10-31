using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UP.Basics;
using UP.Interface.Admin.BasicData;
using UP.Models.DB.BasicData;
using UP.Web.Models.Admin.BasicData;

namespace UP.Web.Controllers.Admin.BasicDataManager
{
    /// <summary>
    /// 基础数据
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class Sys_Code_ItemsController : BasicsController
    {
        /// <summary>
        /// 查询基础数据分类列表(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询基础数据分类列表", "查询基础数据分类列表", "胡家源", "2020-09-28")]
        public IActionResult GetitemsListByCid(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化基础数据接口
                var Items = this.GetInstance<ISys_Code_Items>();
                //查询基础数据列表
                var CatgoryList = Items.GetitemsListByCid(param.id, param.page_num, param.page_size, param.keyword)?.Result;
                resModel.code = (int)ResponseCode.Success;
                resModel.msg = "查询成功!";
                resModel.data = CatgoryList;
            }
            catch (Exception ex)
            {
                LogError("查询基础数据列表失败", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改基础数据", "新增或修改基础数据", "胡家源", "2020-09-28")]
        public IActionResult AddorUpdate(sys_code_items model)
        {

            //实例化基础数据接口
            var Items = this.GetInstance<ISys_Code_Items>();
            //查询基础数据列表
            var result = Items.AddorUpdate(model)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据id查询基础数据信息
        /// </summary>
        /// <param name="id">guid</param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id查询基础数据信息", "根据id查询基础数据信息", "胡家源", "2020-09-28")]
        public IActionResult GetItems(BasicDataParam param)
        {
            //实例化基础数据接口
            var Items = this.GetInstance<ISys_Code_Items>();
            //查询基础数据信息
            var result = Items.GetItems(param.id)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据id修改基础数据状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id修改基础数据状态", "根据id修改基础数据状态", "胡家源", "2020-09-28")]
        public IActionResult UpdateItemsState(BasicDataParam param)
        {
            //实例化基础数据接口
            var Items = this.GetInstance<ISys_Code_Items>();
            //查询基础数据列表
            var result = Items.UpdateItemsState(param.id, param.state)?.Result;
            return Json(result);
        }

        /// <summary>
        /// 根据分类id获取基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据分类id获取基础数据", "根据分类id获取基础数据", "胡家源", "2020-09-28")]
        public IActionResult GetItemsList(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "根据分类id获取基础数据失败");
            try
            {
                //实例化基础数据接口
                var Items = this.GetInstance<ISys_Code_Items>();
                //查询基础数据列表
                var result = Items.GetitemsListByCid(param.cid, param.id)?.Result;
                resModel.code = (int)ResponseCode.Success;
                resModel.msg = "查询成功";
                resModel.data = result;
            }
            catch (Exception ex)
            {
                LogError("根据分类id获取基础数据失败", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 根据id验证基础数据表是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id验证基础数据表是否存在", "根据id验证基础数据表是否存在", "胡家源", "2020-09-29")]
        public IActionResult TableIsExist(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "根据id验证基础数据表是否存在失败");
            try
            {
                //实例化基础数据接口
                var Items = this.GetInstance<ISys_Code_Items>();
                //根据id验证基础数据表是否存在
                resModel = Items.TableIsExist(param.cid)?.Result;
            }
            catch (Exception ex)
            {
                LogError("根据id验证基础数据表是否存在失败", ex);
                resModel.msg = "根据id验证基础数据表是否存在异常";
            }
            return Json(resModel);
        }

        /// <summary>
        ///  同步基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority(" 同步基础数据", " 同步基础数据", "胡家源", "2020-09-29")]
        public IActionResult SynchroBasicTable(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "同步失败");
            try
            {
                //实例化基础数据接口
                var Items = this.GetInstance<ISys_Code_Items>();
                //查询基础数据列表
                var istrue = Items.SynchroBasicTable(param.cid, param.type)?.Result;
                if (istrue.Value)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "同步成功";
                }
            }
            catch (Exception ex)
            {
                LogError("同步失败", ex);
                resModel.msg = "同步失败";
            }
            return Json(resModel);
        }


        /// <summary>
        /// 验证编码是否重复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority(" 验证编码是否重复", " 验证编码是否重复", "胡家源", "2020-09-29")]
        public IActionResult CheckCode(BasicDataParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "验证编码重复失败");
            try
            {
                //判断分类名称是否存在
                var istrue = false;
                var dataList = this.Query<sys_code_items>().Where("code", param.code).Where("code_catgory_id", param.cid).GetModelList();
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
                LogError("验证编码重复失败", ex);
                resModel.msg = "内部异常";
            }
            return Json(resModel);
        }
    }
}
