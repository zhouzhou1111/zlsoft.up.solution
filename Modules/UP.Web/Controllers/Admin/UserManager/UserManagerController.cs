/****************************************************
* 功能：人员管理PC端api
* 描述：
* 作者：胡家源
* 日期：2020/09/10 16:04:03
*********************************************************/

using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Linq;
using UP.Basics;
using UP.Interface.Admin.User;
using UP.Models.Admin.User;
using UP.Models.DB.RoleRight;
using UP.Web.Models.Public;

namespace UP.Web.Controllers.Admin.UserManager
{
    /// <summary>
    /// 模块管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class UserManagerController : BasicsController
    {
        /// <summary>
        /// 查询人员(胡家源)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询人员", "条件查询", "胡家源", "2020-09-11")]
        public IActionResult GetUserList(UserSelect entity)
        {
            ResponseModel resModel = new ResponseModel((int)ResponseCode.Success, "查询人员成功");
            try
            {
                UserSelect model = new UserSelect()
                {
                    sfqy = entity.sfqy,
                    txcx = entity.txcx,
                    page_num = entity.page_num,
                    page_size = entity.page_size
                };
                //机构接口
                var user = this.GetInstance<IUser>();
                //调用业务接口
                var result = user.GetSystemUserList(model)?.Result;
                resModel.data = result;
            }
            catch (System.Exception e)
            {
                LogError("查询人员失败", e);
                resModel.code = (int)ResponseCode.Error;
                resModel.msg = "服务器内部异常";
            }
            return Json(resModel);
        }

        [HttpPost]
        [RIPAuthority("新增修改人员", "新增修改人员", "胡家源", "2020-09-11")]
        public IActionResult EditUser(SystemUser model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "新增人员成功!");
            try
            {
                //返回结果
                var row = 0;
                //重置简码
                model.简码 = Basics.Utils.Strings.GetFirstPY(model.姓名.Trim());
                if (!CheckAccountNum(model.账号, model.id))
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "登录帐号重复";
                    return Json(result);
                }
                //新增
                if (model.id == 0)
                {
                    model.数据标识 = 1;
                    model.用户状态 = 1;
                    model.登记时间 = DateTime.Now;

                    //重置登录密码
                    if (!string.IsNullOrEmpty(model.密码))
                    {
                        model.密码 = Strings.StrToMD5(model.密码);//加密
                    }
                    row = this.Add(model).Execute();
                    if (row < 1)
                    {
                        result.msg = "新增人员失败!";
                        result.code = (int)ResponseCode.Error;
                    }
                    result.data = row;
                }
                //修改
                else
                {
                    model.编辑时间 = DateTime.Now;
                    row = this.Update(model).Columns("姓名", "简码", "性别", "手机", "邮箱", "工作单位", "照片", "账号", "编辑时间")
                       .Where("id", model.id).Execute();
                    if (row < 1)
                    {
                        result.msg = "修改人员失败!";
                        result.code = (int)ResponseCode.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("保存人员失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 重置机构人员密码(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("重置人员密码", "重置人员密码", "胡家源", "2020-09-11")]
        public IActionResult ResetPassword(QueryModelParm param)
        {
            var result = new ResponseModel(ResponseCode.Success, "重置成功!");
            try
            {
                if (param.id.IsNullOrEmpty())
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "缺少参数";
                    return Json(result);
                }
                SystemUser user = new SystemUser()
                {
                    密码 = Strings.StrToMD5("123456")
                };
                var count = this.Update(user).Columns("密码").Where("id", param.id).Execute();
                if (count > 0)
                {
                    result.code = (int)ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                LogError("重置人员密码操作失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 修改人员用户状态(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("修改人员用户状态", "修改人员用户信息", "胡家源", "2020-09-11")]
        public IActionResult UpOrgPeopleState(QueryStateParam param)
        {
            var result = new ResponseModel(ResponseCode.Success, "修改用户状态成功!");
            try
            {
                if (!param.id.IsNullOrEmpty())
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "缺少参数";
                    return Json(result);
                }
                SystemUser user = new SystemUser()
                {
                    用户状态 = param.state
                };
                var cuteCount = this.Update(user).Columns("用户状态").Where("id", param.id).Execute();
                if (cuteCount == 0)
                {
                    result.msg = "修改用户状态失败";
                }
            }
            catch (Exception ex)
            {
                LogError("修改人员用户状态操作失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 验证账号重复
        /// </summary>
        /// <param name="dlzh">账号</param>
        /// <param name="ryid">根据账户id判断操作类型</param>
        /// <returns></returns>
        private bool CheckAccountNum(string dlzh, int ryid)
        {
            //查询人员集合
            var userList = this.Query<SystemUser>().Where("账号", dlzh).GetModelList();
            //ryid!=0为修改
            if (userList != null)
            {
                if (ryid != 0)
                {
                    userList = userList.Where(d => d.id != ryid).ToList();
                }
                return userList.Any() ? false : true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 查询单个人员信息(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询单个人员信息", "人员信息查询", "胡家源", "2020-09-12")]
        public IActionResult FindUser(QueryModelParm param)
        {
            ResponseModel resModel = new ResponseModel((int)ResponseCode.Success, "查询人员信息成功");
            try
            {
                //根据ID获取人员信息
                var user = this.Query<SystemUser>().Where("id", param.id).GetModel();
                resModel.data = user;
            }
            catch (System.Exception e)
            {
                LogError("查询单个人员信息失败", e);
                resModel.code = (int)ResponseCode.Error;
                resModel.msg = "服务器内部异常";
            }  //调用业务接口
            return Json(resModel);
        }
    }
}