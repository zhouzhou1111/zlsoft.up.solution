/****************************************************
* 功能：接口管理PC端api
* 描述：
* 作者：胡家源
* 日期：2020/09/15 14:04:03
*********************************************************/

using Microsoft.AspNetCore.Mvc;
using System;
using UP.Basics;
using UP.Interface.Admin.ModulesInterface;
using UP.Models.DB.RoleRight;
using UP.Web.Models.Admin.ModulesFunctionInterface;
using UP.Web.Models.Admin.ModulesManager;
using UP.Web.Models.Public;

namespace UP.Web.Controllers.Admin.ModulesFuncInterfaceManager
{
    /// <summary>
    /// 模块功能接口管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class ModulesFuncInterfaceController : BasicsController
    {
        /// <summary>
        /// 新增或修改接口(胡家源)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改接口", "新增或修改接口", "胡家源", "2020-09-15")]
        public IActionResult AddModulesFuncInterface([FromBody] ModulesFunctionInterface entity)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                if (entity.模块id == 0 || entity.功能id == 0)
                {
                    resModel.msg = "请求参数异常";
                    return Json(resModel);
                }
                var count = 0;//数据库执行返回结果
                if (entity.id == 0)
                {
                    entity.登记人 = loginUser.id;
                    entity.登记时间 = DateTime.Now;
                    //ID为空为新增
                    count = this.Add<ModulesFunctionInterface>(entity).Execute();
                }
                else
                {
                    entity.更新时间 = DateTime.Now;
                    //修改模块
                    count = this.Update<ModulesFunctionInterface>(entity)
                        .Columns("命名空间名称", "控制器名称", "方法名", "序号", "更新时间")
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
                LogError("新增或修改接口", ex);
                resModel.msg = "操作异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 查询接口列表(胡家源)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询接口列表", " 根据模块id和功能id查询接口的列表", "胡家源", "2020-09-12")]
        public IActionResult SelectInterFaceList(ModulesFuncInterfaceParam model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "查询接口列表操作失败");
            try
            {
                //查询模块功能接口列表
                var interFace = this.GetInstance<IModulesInterface>();
                var interFaceList = interFace.getInterfaceList(model.mkid, model.gnid, model.jsid, model.type)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", interFaceList));
            }
            catch (Exception ex)
            {
                LogError("查询接口列表操作失败", ex);
                resModel.msg = "查询异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 停用模块功能接口(胡家源)
        /// </summary>
        /// <param name="param">
        /// {
        /// "id":服务项目id,
        /// }
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("停用接口", "根据id停用接口", "胡家源", "2020-09-15")]
        public IActionResult DisableInterFace(QueryStateParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败", false);
            try
            {
                //判断是否传入id
                if (param.id.IsNullOrEmpty())
                {
                    resModel.msg = "传入接口id为空";
                    return Json(resModel);
                }
                var model = this.Query<ModulesFunctionInterface>().Where("id", param.id).GetModel();
                model.是否停用 = model.是否停用 == 1 ? 0 : 1;
                model.停用时间 = DateTime.Now;
                //管理员禁用/启用模块
                var state = this.Update<ModulesFunctionInterface>(model)
                     .Columns("是否停用", "停用时间")
                    .Where("id", model.id)
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
                LogError("停用启用模板功能接口", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 根据模块功能接口id查询模块功能接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据id查询模块功能接口对象", "根据id查询模块功能接口对象", "胡家源", "2020-09-15")]
        public IActionResult getFuncInterface(ModulesFuncInterfaceParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败", false);
            try
            {
                //判断是否传入id
                if (param.id == 0)
                {
                    resModel.msg = "数据异常";
                    return Json(resModel);
                }
                var model = this.Query<ModulesFunctionInterface>().Where("id", param.id).GetModel();
                if (model != null)//判断不为空
                {
                    resModel.data = model;
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "修改成功";
                }
            }
            catch (Exception ex)
            {
                LogError("查询模块功能接口对象异常", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 保存角色模块功能接口
        /// </summary>
        /// <param name="param">角色id,模块id,功能id,接口ids</param>
        /// <returns></returns>
        ///   [HttpPost]
        [HttpPost]
        [RIPAuthority("保存角色模块功能接口", "角色模块功能接口", "胡家源", "2020-09-16")]
        public IActionResult SavaRoleFunctionInterface(ModulesFuncInterfaceParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败", false);
            try
            {
                //判断是否传入id
                if (param.jsid == 0 || param.mkid == 0 || param.gnid == 0)
                {
                    resModel.msg = "请求参数异常";
                    return Json(resModel);
                }
                //查询模块功能接口列表
                var interFace = this.GetInstance<IModulesInterface>();
                var interFaceList = interFace.saveRoleModuleFuncInterFace(param.jsid, param.mkid, param.gnid, param.jkids)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "保存成功!", interFaceList));
            }
            catch (Exception ex)
            {
                LogError("停用启用模板功能接口", ex);
                resModel.msg = "修改异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 根据角色id,模块id和功能id查询接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据角色id,模块id和功能id查询接口", "根据角色id,模块id和功能id查询接口", "胡家源", "2020-09-23")]
        public IActionResult GetInterfaceList(ModulesFuncInterfaceParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "参数异常", false);
            try
            {
                //查询模块功能接口列表
                var interFace = this.GetInstance<IModulesInterface>();
                var interFaceList = interFace.getCheckInterfaceList(param.mkid, param.gnid, param.jsid)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", interFaceList));
            }
            catch (Exception ex)
            {
                LogError("参数异常", ex);
                resModel.msg = "参数异常";
            }

            return Json(resModel);
        }

        /// <summary>
        /// 修改模块功能接口状态（启用=0,禁用=1）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("修改模块功能接口状态", "修改模块功能接口状态", "胡家源", "2020-09-25")]
        public IActionResult UpdateModulesFuncInterFaceState(ModulesParam param)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "修改模块功能接口状态失败");
            try
            {
                ModulesFunctionInterface modulesfuncinterfaceinfo = new ModulesFunctionInterface() { 是否停用 = param.disable };
                //执行修改方法
                var row = this.Update<ModulesFunctionInterface>(modulesfuncinterfaceinfo).Columns("是否停用").Where("id", param.id).Execute();
                if (row > 0)
                {
                    resModel.msg = "保存成功";
                    resModel.code = (int)ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                LogError("修改模块功能接口状态失败", ex);
                resModel.msg = "修改模块功能接口状态失败";
            }
            return Json(resModel);
        }
    }
}