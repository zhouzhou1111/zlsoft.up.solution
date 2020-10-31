/****************************************************
* 功能：机构人员管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/17 9:04:03
*********************************************************/

using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UP.Basics;
using UP.Models.Admin.Org;
using UP.Models.DB.BasicData;
namespace UP.Logics.Admin.Org
{
    public class OrgPersonLogic
    {
        /// <summary>
        /// 分页查询机构人员列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="orgId">机构id</param>
        /// <returns></returns>
        public ListPageModel<OrgPersonDto> GetOrgPersonList(int pageNum, int pageSize, string keyword, int orgId, string state)
        {
            ListPageModel<OrgPersonDto> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件不为空
                    if (keyword.IsNotNullOrEmpty())
                    {
                        param.Add("条件查询");
                        sqlBuilder.Parameters("keyword", keyword);
                    }
                    if (orgId != 0)
                    {
                        param.Add("机构id");
                        sqlBuilder.Parameters("orgid", orgId);
                    }
                    if (!state.IsNullOrEmpty())
                    {
                        param.Add("状态");
                        sqlBuilder.Parameters("UserState", state.ToInt32());
                    }
                    //获取用户基本信息
                    var sqlStr = db.GetSql("CA00002-分页查询机构人员", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<OrgPersonDto>(out int total);
                    //排序
                    item = new ListPageModel<OrgPersonDto>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取机构人员异常错误!", ex);
            }
            return item;
        }


        /// <summary>
        /// 新增或修改机构人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel EditPerson(OrgPersonDto model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "保存机构人员信息成功!");
            try
            {

                using (var db = new DbContext())
                {
                    //查询机构人员下一个序列
                    var sqlStr = db.GetSql("CA00006-查询机构人员下一个序列", null, null);
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr);
                    int item = 0;
                    //执行SQL脚本
                    if (sqlBuilder.Select().Rows[0][0] != null)
                    {
                        item = Convert.ToInt32((sqlBuilder.Select().Rows[0][0]));
                    }
                    //重置简码
                    model.简码 = Basics.Utils.Strings.GetFirstPY(model.姓名.Trim());
                    //机构人员
                    OrgPerson person = new OrgPerson();
                    person.出生日期 = model.出生日期;
                    person.姓名 = model.姓名;
                    person.学历 = model.学历;
                    person.性别 = model.性别;
                    person.手机 = model.手机;
                    person.擅长 = model.擅长;
                    person.机构id = model.机构id;
                    person.民族 = model.民族;
                    person.状态 = model.状态;
                    person.电话 = model.电话;
                    person.简介 = model.简介;
                    person.简码 = model.简码;
                    person.职务 = model.职务;
                    person.身份证号 = model.身份证号;
                    person.邮箱 = model.邮箱;

                    //账户
                    Account account = new Account();
                    account.姓名 = model.姓名;
                    account.账户 = model.账户;
                    account.状态 = model.状态;
                    if (!model.密码.IsNullOrEmpty())
                    {
                        account.密码 = Strings.StrToMD5(model.密码);
                    }
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        var count = 0;
                        if (model.id == 0)
                        {
                            //将下一个序列赋值给机构人员id
                            person.id = item;
                            //新增默认给状态赋值为启用
                            person.状态 = 1;
                            account.状态 = 1;
                            //新增机构人员
                            count = db.Insert<OrgPerson>(person).Execute();
                            if (count <= 0)
                            {
                                db.Rollback();
                                return false;
                            }
                            ////新增账户
                            account.人员id = item;
                            //保存账户
                            count = db.Insert<Account>(account).Execute();
                            if (count <= 0)
                            {
                                db.Rollback();
                                return false;
                            }
                        }
                        else
                        {
                            person.id = model.id;
                            //修改机构人员
                            count = db.Update<OrgPerson>(person).Where("id", person.id).Execute();
                            if (count <= 0)
                            {
                                db.Rollback();
                                return false;
                            }
                            //修改账户
                            account.人员id = model.id;
                            account.id = model.账户id;
                            count = db.Update<Account>(account).Execute();
                            if (count <= 0)
                            {
                                db.Rollback();
                                return false;
                            }
                        }
                        return true;
                    });
                    if (!value)
                    {
                        result.code = ResponseCode.Error.ToInt32();
                        result.msg = "保存机构人员信息失败!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "保存机构人员信息失败!";
            }
            return result;
        }

        /// <summary>
        /// 根据id获取机构人员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public OrgPersonDto GetOrgPersonDtoInfo(int id)
        {
            OrgPersonDto item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    //根据编码查询行政区划
                    var sqlStr = db.GetSql("CA00005-查询机构人员账户信息", null, null);
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr).Parameters("id", id);
                    //执行SQL脚本
                    item = sqlBuilder.GetModel<OrgPersonDto>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取机构人员信息异常!", ex);
            }
            return item;
        }

        /// <summary>
        /// 修改机构人员及账户状态
        /// </summary>
        /// <param name="accountid">账户id</param>
        /// <param name="orgpersonid">机构人员id</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ResponseModel UpdateOrgPersonState(int accountid, int orgpersonid, int state)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "保存机构人员信息成功!");
            try
            {
                using (var db = new DbContext())
                {
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        //修改机构人员状态
                        var count = 0;
                        count = db.Update("b_机构人员").Column("状态", state).Where("id", orgpersonid).Execute();
                        if (count <= 0)
                        {
                            db.Rollback();
                            return false;
                        }
                        //修改账户状态
                        count = db.Update("b_账户").Column("状态", state).Where("id", accountid).Execute();
                        if (count <= 0)
                        {
                            db.Rollback();
                            return false;
                        }
                        return true;
                    });
                    if (!value)
                    {
                        result.code = ResponseCode.Error.ToInt32();
                        result.msg = "修改机构人员状态失败!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "修改机构人员状态失败!";
            }
            return result;
        }
    }
}
