/****************************************************
* 功能：机构管理PC端api
* 描述：
* 作者：胡家源
* 日期：2020/09/17 10:41:03
*********************************************************/

using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary;
using System;
using System.Linq;
using UP.Basics;
using UP.Interface.Admin.Org;
using UP.Models.DB.BasicData;
using UP.Web.Models.Admin.Org;

namespace UP.Web.Controllers.Admin.OrgManager
{
    /// <summary>
    /// 机构管理
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class OrganController : BasicsController
    {
        /// <summary>
        /// 分页查询所有机构的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询机构列表", "分页查询所有机构的列表", "胡家源", "2020-09-17")]
        public IActionResult GetOrganList(OrgParam model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化机构接口
                var org = this.GetInstance<IOrgan>();
                //分页查询机构列表
                var orgList = org.GetOrganPageList(model.page_num, model.page_size, model.keyword)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", orgList));
            }
            catch (Exception ex)
            {
                LogError("分页查询机构列表失败", ex);
                resModel.msg = "分页查询机构列表异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 根据上级id查询所有机构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据上级id查询所有机构", "根据pid查询所有机构", "胡家源", "2020-09-17")]
        public IActionResult GetOrganListByPid(OrgParam model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化机构接口
                var org = this.GetInstance<IOrgan>();
                //分页查询机构列表
                var orgList = org.GetOrganListByPid(model.pid)?.Result;
                return Json(new ResponseModel(200, "ok", orgList));
            }
            catch (Exception ex)
            {
                LogError("分页查询机构列表失败", ex);
                resModel.msg = "分页查询机构列表异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询机构列表", "查询所有机构的列表", "胡家源", "2020-09-17")]
        public IActionResult GetAllOrganList()
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化机构接口
                var org = this.GetInstance<IOrgan>();
                //分页查询机构列表
                var orgList = org.GetOrganList(null)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", orgList));
            }
            catch (Exception ex)
            {
                LogError("查询机构列表失败", ex);
                resModel.msg = "查询机构列表异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 根据机构id查询机构信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询机构信息", "查询机构信息", "胡家源", "2020-09-17")]
        public IActionResult GetOrganInfo(OrgParam param)
        {
            var result = new ResponseModel(ResponseCode.Error, "查询失败!");
            try
            {
                //实例化机构接口
                var org = this.GetInstance<IOrgan>();
                var model = this.Query<Organ>().Where("id", param.organid).GetModel();
                //上级机构名称
                var pName = "无";
                if (model != null)
                {
                    pName = model.上级id == 0 ? pName : this.Query<Organ>().Where("id", model.上级id).GetModel().名称;
                }
                result.data = org.GetOrganDtoInfo(model, pName)?.Result;
                if (result.data != null)
                {
                    result.code = (int)ResponseCode.Success;
                    result.msg = "查询成功";
                }
            }
            catch (Exception ex)
            {
                result.msg = "查询机构发生异常";
                Logger.Instance.Error("查询机构异常", ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 新增或修改机构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改机构", "新增或修改机构", "胡家源", "2020-09-17")]
        public IActionResult AddOrganInfo(Organ model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            if (!model.名称.IsNotNullOrEmpty())
            {
                resModel.msg = "机构名称不能为空！";
                resModel.msg = "操作异常";
                return Json(resModel);
            }

            try
            {
                var count = 0;//数据库执行返回结果
                //重置简码
                model.简码 = Basics.Utils.Strings.GetFirstPY(model.名称.Trim());
                if (model.id == 0)
                {
                    //机构名称是否存在
                    var value = this.Query<Organ>().Where("名称", model.名称.Trim()).Exists();
                    //机构名称已存在
                    if (value)
                    {
                        resModel.msg = "机构名称已存在";
                        return Json(resModel);
                    }
                    //名称不存在，新增
                    count = this.Add<Organ>(model).Execute();
                }
                else
                {
                    //机构名称是否存在
                    var roleList = this.Query<Organ>().Where("名称", model.名称.Trim()).GetModelList();
                    if (roleList != null && roleList.Any(d => d.id != model.id && d.名称 == model.名称))
                    {
                        resModel.msg = "机构名称已存在";
                        return Json(resModel);
                    }
                    //修改机构
                    count = this.Update<Organ>(model)
                        .Columns("上级id", "名称", "编码", "简称", "行政级别", "简码", "性质", "机构码", "电话", "地址", "执业证代码", "组织机构代码证", "所属行政区划")
                        .Where("id", model.id).Execute();
                }
                //成功
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "执行成功";
                }
            }
            catch (Exception ex)
            {
                LogError("新增或修改机构", ex);
                resModel.msg = "操作异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteOrgan(OrgParam model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "删除失败", false);
            try
            {
                //查询是否有下级机构
                var findOrgList = this.Query<Organ>().Where("上级id", model.organid).Exists();
                if (findOrgList)
                {
                    resModel.msg = "该机构拥有下级机构,删除失败";
                    return Json(resModel);
                }
                //查询机构下是否有人员
                var finOrgPersonList = this.Query<OrgPerson>().Where("机构id", model.organid).Exists();
                if (finOrgPersonList)
                {
                    resModel.msg = "该机构拥有人员,删除失败";
                    return Json(resModel);
                }
                var count = this.Delete<Organ>().Where("id", model.organid).Execute();
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.data = true;
                    resModel.msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                LogError("删除机构", ex);
                resModel.msg = "操作异常";
            }

            return Json(resModel);
        }
    }
}