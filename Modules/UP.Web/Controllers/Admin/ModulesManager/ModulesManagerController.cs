/****************************************************
* 功能：模块管理PC端api
* 描述：
* 作者：胡家源
* 日期：2020/09/10 16:04:03
*********************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using UP.Basics;
using UP.Interface.Admin.Modules;
using UP.Models.DB.RoleRight;
using UP.Web.Models.Admin.ModulesManager;
using UP.Web.Models.Public;

namespace UP.Web.Controllers.Admin.ModulesManager
{
    /// <summary>
    /// 模块管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class ModulesManagerController : BasicsController
    {
        /// <summary>
        /// 新增或修改模块(胡家源)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改模块", "新增或修改模块配置", "胡家源", "2020-09-10")]
        public IActionResult AddModules([FromBody]ModulesInfo entity)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var count = 0;//数据库执行返回结果
                if (entity.id.IsNullOrEmpty())
                {
                    //根据模块名称验证模块是否存在
                    var ModulesInfo = this.Query<ModulesInfo>().Where("parent_id", entity.parent_id).Where("name", entity.name).Exists();
                    if (ModulesInfo)
                    {
                        resModel.msg = "该模块名称已存在！";
                        return Json(resModel);
                    }
                    //ID为空为新增
                    entity.status = 1;
                    count = this.Add<ModulesInfo>(entity).Execute();
                }
                else
                {
                    //根据模块名称验证模块是否存在
                    var ModulesList = this.Query<ModulesInfo>().Where("parent_id", entity.parent_id).Where("name", entity.name).GetModelList();
                    var isTrue = ModulesList!=null&&ModulesList.Where(d => d.id != entity.id).Any();
                    if (isTrue)
                    {
                        resModel.msg = "该模块名称已存在！";
                        return Json(resModel);
                    }
                    //修改模块
                    entity.update_time = DateTime.Now;
                    count = this.Update<ModulesInfo>(entity)
                        .Columns("parent_id", "name", "code", "title", "path", "status", "sno", "icon", "is_firstpage", "resource_type", "is_child_page", "describe", "update_time")
                        .Where("id", entity.id).Execute();
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
                LogError("新增或修改模块", ex);
                resModel.msg = "操作异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询模块列表(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询模块列表", "管理员查询所有模块的列表", "胡家源", "2020-09-10")]
        public IActionResult SelectModulesList()
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //查询模块列表
                var modules = this.GetInstance<IModules>();
                //管理员查询所有模块的列表
                var modulesList = modules.SelectModules()?.Result;
                //if (modulesList!=null)//判断不为空
                //{
                //    resModel.data = modulesList;
                //    resModel.code = (int)ResponseCode.Success;
                //    resModel.msg = "查询成功";
                //}
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", modulesList));
            }
            catch (Exception ex)
            {
                LogError("查询模块列表失败", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询单个模块详细信息(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询单个模块详细信息", "根据id查询单个模块详细信息", "胡家源", "2020-09-10")]
        public IActionResult AloneModules(ModulesParam param)//int id
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                //判断是否传入id
                if (param.id == 0)
                {
                    resModel.msg = "查询数据异常";
                    return Json(resModel);
                }
                //查询单个模块信息
                var model = this.Query<ModulesInfo>()
                               .Where("id", param.id)//param.id
                               .GetModel();
                if (model != null)//非空判断
                {
                    resModel.data = model;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "查询成功";
                }
            }
            catch (Exception ex)
            {
                LogError("查询单个模块功能信息", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 禁用/启用模块(胡家源)
        /// </summary>
        /// <param name="param">
        /// {
        /// "id":服务项目id,
        /// "state":禁用/启用状态 0-启用 1-禁用
        /// }
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("禁用/启用模块", "根据id禁用/启用模块,需要传入id和state的值", "胡家源", "2020-09-10")]
        public IActionResult DisableModules(QueryStateParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败", false);
            try
            {
                //判断是否传入id
                if (!param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                var entity = new ModulesInfo()
                {
                    id = param.id,
                    status = param.state
                };
                //管理员禁用/启用模块
                var state = this.Update<ModulesInfo>(entity)
                    .Columns("status")
                    .Where("id", entity.id)
                    .Execute();
                if (state > 0)//判断不为空
                {
                    resModel.data = true;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "修改成功";
                }
            }
            catch (Exception ex)
            {
                LogError("新增或修改模块", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 删除模块(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除模块", "根据id删除模块,需要传入id的值", "胡家源", "2020-09-10")]
        public IActionResult DelModules(QueryStateParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "删除失败", false);
            try
            {
                //判断是否传入id
                if (!param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                var entity = new ModulesInfo()
                {
                    id = param.id,
                    status = 0
                };
                //管理员禁用/启用模块
                var state = this.Update<ModulesInfo>(entity)
                    .Columns("status")
                    .Where("id", entity.id)
                    .Execute();
                if (state > 0)//判断不为空
                {
                    resModel.data = true;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                LogError("删除模块", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改模块功能(胡家源)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改模块功能", "根据id是否为空判断新增或修改模块的功能", "胡家源", "2020-09-10")]
        public IActionResult editFunction(ModulesFunction entity)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var count = 0;//数据库执行返回结果
                if (entity.id.IsNullOrEmpty())
                {
                    //根据模块功能名称验证模块是否存在
                    var ModulesFunction = this.Query<ModulesFunction>().Where("模块id", entity.模块id).Where("名称", entity.名称).Exists();
                    if (ModulesFunction)
                    {
                        resModel.msg = "该模块功能名称已存在！";
                        return Json(resModel);
                    }
                    entity.数据标识 =1;
                    entity.id = Guid.NewGuid().ToString();
                    //ID为空为新增
                    count = this.Add<ModulesFunction>(entity).Execute();
                }
                else
                {
                    //根据模块功能名称验证模块是否存在
                    var ModulesFunctionList = this.Query<ModulesFunction>().Where("模块id", entity.模块id).Where("名称", entity.名称).GetModelList();
                    var isTrue = ModulesFunctionList!=null&& ModulesFunctionList.Where(d => d.id != entity.id).Any();
                    if (isTrue)
                    {
                        resModel.msg = "该模块功能名称已存在！";
                        return Json(resModel);
                    }
                    //修改模块功能
                    if (entity.是否停用 == 1)
                    {
                        entity.停用人 = loginUser.id;
                        entity.停用时间 = DateTime.Now;
                    }
                    else
                    {
                        entity.停用人 =null;
                        entity.停用时间 =null;
                    }
                
                    count = this.Update<ModulesFunction>(entity)
                        .Columns("模块id", "名称", "描述", "是否停用", "停用人", "停用时间", "顺序", "简码", "图片", "显示方式")
                        .Where("id", entity.id).Execute();
                }
                //大于0修改成功
                if (count > 0)
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "执行成功";
                }
            }
            catch (Exception ex)
            {
                LogError("新增或修改模块功能", ex);
                resModel.msg = "操作异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 禁用/启用模块功能(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("禁用/启用模块功能", "根据id禁用/启用模块功能,需要传入id和state的值", "胡家源", "2020-09-10")]
        public IActionResult DisableFunction(QueryStateParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败", false);
            try
            {
                //判断是否传入id
                if (!param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                var entity = new ModulesFunction()
                {
                    id = param.id,
                    是否停用 = param.state
                };
                ////停用
                if (entity.是否停用 == 1)
                {
                    entity.停用人 = loginUser.id;
                    entity.停用时间 = DateTime.Now;
                }
                else
                {
                    entity.停用人 = null;
                    entity.停用时间 = null;
                }
                //管理员禁用/启用模块
                var state = this.Update<ModulesFunction>(entity)
                    .Columns("是否停用")
                    .Where("id", entity.id)
                    .Execute();
                if (state > 0)//大于0修改成功
                {
                    resModel.data = true;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "修改成功";
                }
            }
            catch (Exception ex)
            {
                LogError("禁用/启用模块功能异常", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 删除模块功能(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("删除模块功能", "根据id删除模块功能,需要传入id值", "胡家源", "2020-09-10")]
        public IActionResult DelFunction(QueryStateParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "删除失败", false);
            try
            {
                //判断是否传入id
                if (!param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                var entity = new ModulesFunction()
                {
                    id = param.id,
                    数据标识 = 0
                };
                entity.停用人 = loginUser.id;
                entity.停用时间 = DateTime.Now;
                //管理员禁用/启用模块
                var state = this.Update<ModulesFunction>(entity)
                    .Columns("数据标识", "停用人", "停用时间")
                    .Where("id", entity.id)
                    .Execute();
                if (state > 0)//大于0修改成功
                {
                    resModel.data = true;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "删除成功";
                }
            }
            catch (Exception ex)
            {
                LogError("删除模块功能异常", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 查询单个模块功能信息(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询单个模块功能信息", "根据id查询单个模块功能信息", "胡家源", "2020-09-10")]
        public IActionResult SingleFunction(QueryModelParm param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                //判断是否传入id
                if (param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                //查询单个模块信息
                var model = this.Query<ModulesFunction>()
                               .Where("id", param.id)
                               .GetModel();
                //if (model!=null)//非空判断
                //{
                //    resModel.data = model;
                //    resModel.code = (int)ResponseCode.Success;
                //    resModel.msg = "查询成功";
                //}
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", model));
            }
            catch (Exception ex)
            {
                LogError("查询单个模块功能信息", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询模块功能(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询模块的所有功能", "根据模块mkid查询单个模块所有的功能信息", "胡家源", "2020-09-10")]
        public IActionResult SelectFunctionList(QueryModelParm param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询失败");
            try
            {
                //判断是否传入id
                if (param.id.IsNullOrEmpty())
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                //查询单个模块所有的功能信息
                var modelList = this.Query<ModulesFunction>()
                               .Where("模块id", param.id)
                               .Where("数据标识", 1)
                               .Order("顺序")
                               .GetModelList();
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", modelList));
            }
            catch (Exception ex)
            {
                LogError("查询模块功能", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 修改模块状态（启用=0,禁用=1）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("修改模块状态", "修改模块状态", "胡家源", "2020-09-25")]
        public IActionResult UpdateModulesState(ModulesParam param)//int id
        {
            var resModel = new ResponseModel(ResponseCode.Error, "修改模块状态失败");
            try
            {
                ModulesInfo modulesinfo = new ModulesInfo() { status = param.disable };
                //执行修改方法
                 var row=this.Update<ModulesInfo>(modulesinfo).Columns("status").Where("id", param.id).Execute();
                if (row>0)
                {
                    resModel.msg = "保存成功";
                    resModel.code = (int)ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                LogError("修改模块状态失败", ex);
                resModel.msg = "修改模块状态失败";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 修改模块功能状态（启用=0,禁用=1）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("修改模块功能状态", "修改模块功能状态", "胡家源", "2020-09-25")]
        public IActionResult UpdateModulesFuncState(ModulesParam param)//int id
        {
            var resModel = new ResponseModel(ResponseCode.Error, "修改模块功能状态失败");
            try
            {
                ModulesFunction modulesfuncinfo = new ModulesFunction() { 是否停用 = param.disable };
                //执行修改方法
                var row = this.Update<ModulesFunction>(modulesfuncinfo).Columns("是否停用").Where("id", param.id).Execute();
                if (row > 0)
                {
                    resModel.msg = "保存成功";
                    resModel.code = (int)ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                LogError("修改模块功能状态失败", ex);
                resModel.msg = "修改模块功能状态失败";
            }
            return Json(resModel);
        }
    }
}