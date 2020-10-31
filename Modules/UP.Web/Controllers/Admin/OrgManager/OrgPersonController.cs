using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Linq;
using UP.Basics;
using UP.Interface.Admin.Org;
using UP.Models.Admin.Org;
using UP.Models.DB.BasicData;
using UP.Web.Models.Admin.Org;

namespace UP.Web.Controllers.Admin.OrgManager
{
    /// <summary>
    /// 机构人员管理
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class OrgPersonController : BasicsController
    {
        /// <summary>
        /// 分页查询所有机构的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询机构人员列表", "分页查询所有机构人员的列表", "胡家源", "2020-09-17")]
        public IActionResult GetOrganPersonList(OrgParam model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化机构接口
                var org = this.GetInstance<IOrgPerson>();
                //分页查询机构列表
                var orgList = org.GetOrgPersonList(model.page_num, model.page_size, model.keyword, model.organid, model.userstate)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", orgList));
            }
            catch (Exception ex)
            {
                LogError("分页查询所有机构人员的列表失败", ex);
                resModel.msg = "分页查询所有机构人员的列表异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 根据机构id查询机构信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询机构人员信息", "查询机构人员信息", "胡家源", "2020-09-17")]
        public IActionResult GetOrganPersonInfo(OrgParam param)
        {
            var result = new ResponseModel(ResponseCode.Error, "查询失败!");
            try
            {
                //实例化机构接口
                var org = this.GetInstance<IOrgPerson>();
                var model = org.GetOrgPersonDtoInfo(param.orgpersonid)?.Result;
                result.data = model;
                if (result.data != null)
                {
                    result.code = (int)ResponseCode.Success;
                    result.msg = "查询成功";
                }
            }
            catch (Exception ex)
            {
                result.msg = "查询机构人员发生异常";
                Logger.Instance.Error("查询机构人员异常", ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 新增或修改机构人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改机构人员", "新增或修改机构人员", "胡家源", "2020-09-17")]
        public IActionResult AddOrganInfo(OrgPersonDto model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                var verifmsg = "";
                if (!CheckAccountNum(model.账户, model.账户id))
                {
                    verifmsg = "登录帐号重复";
                }
                if (!model.身份证号.IsNullOrEmpty())
                {
                    //查询身份证在该机构是否存在
                    //判断新增，修改
                    var exist = false;
                    if (model.id == 0)
                    {
                        exist = this.Query<OrgPerson>().Where("身份证号", model.身份证号).Where("机构id", model.机构id).Exists();
                    }
                    else
                    {
                        //查询该机构下不包含修改id的身份证出现过几次
                        var personlist = this.Query<OrgPerson>().Where("身份证号", model.身份证号).Where("机构id", model.机构id).GetModelList();
                        exist = personlist == null ? false : personlist.Where(d => d.id != model.id).Any();
                    }
                    if (exist)
                    {
                        verifmsg = verifmsg.IsNullOrEmpty() ? "身份证号已存在" : verifmsg += ",身份证号已存在";
                    }
                }
                if (!verifmsg.IsNullOrEmpty())
                {
                    resModel.code = (int)ResponseCode.Error;
                    resModel.msg = verifmsg;
                    return Json(resModel);
                }
                //实例化机构接口
                var org = this.GetInstance<IOrgPerson>();
                var data = org.EditPerson(model);
                if (data.Result.code == ResponseCode.Success.ToInt32())
                {
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "操作成功";
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
        /// 验证账号重复
        /// </summary>
        /// <param name="dlzh">账号</param>
        /// <param name="ryid">根据账户id判断操作类型</param>
        /// <returns></returns>
        private bool CheckAccountNum(string dlzh, int ryid)
        {
            //查询人员集合
            var accountList = this.Query<Account>().Where("账户", dlzh).GetModelList();
            //ryid!=0为修改
            if (accountList != null)
            {
                if (ryid != 0)
                {
                    accountList = accountList.Where(d => d.id != ryid).ToList();
                }
                return accountList.Any() ? false : true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 重置机构人员密码(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("重置人员密码", "重置人员密码", "胡家源", "2020-09-11")]
        public IActionResult ResetPassword(OrgParam param)
        {
            var result = new ResponseModel(ResponseCode.Success, "重置成功!");
            try
            {
                if (param.accountid == 0)
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "缺少参数";
                    return Json(result);
                }
                //通过机构人员id查询账户
                Account account = new Account()
                {
                    密码 = Strings.StrToMD5("123456")
                };
                var count = this.Update(account).Columns("密码").Where("id", param.accountid).Execute();
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
        public IActionResult UpOrgPeopleState(OrgParam param)
        {
            var result = new ResponseModel(ResponseCode.Success, "修改机构人员状态成功!");
            try
            {
                if (param.accountid == 0 || param.orgpersonid == 0)
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "缺少参数";
                    return Json(result);
                }
                //实例化机构接口
                var org = this.GetInstance<IOrgPerson>();
                var data = org.UpdateOrgPersonState(param.accountid, param.orgpersonid, param.state);
                if (data.Result.code == ResponseCode.Success.ToInt32())
                {
                    result.code = (int)ResponseCode.Success;
                    result.msg = "操作成功";
                }
            }
            catch (Exception ex)
            {
                LogError("修改机构人员状态操作失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }
    }
}