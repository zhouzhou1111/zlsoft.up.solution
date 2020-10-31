/****************************************************
* 功能：人员管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/11 09:52:01
*********************************************************/
using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UP.Basics;
using UP.Models.Admin.User;
using UP.Models.DB.RoleRight;

namespace UP.Logics.Admin.User
{
   public class SystemUserLogic
    {
        /// <summary>
        /// 分页查询人员列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ListPageModel<SystemUser> GetSystemUserList(UserSelect model)
        {
            ListPageModel<SystemUser> listPage = new ListPageModel<SystemUser>();
            try
            {
                int total = 0;
                var items = new List<SystemUser>();
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件
                    if (model.txcx.IsNotNullOrEmpty())
                    {
                        param.Add("查询条件");
                        sqlBuilder.Parameters("strwhere", model.txcx);
                    }
                    if (model.sfqy.IsNotNullOrEmpty())
                    {
                        param.Add("用户状态");
                        sqlBuilder.Parameters("SFQY", int.Parse(model.sfqy));
                    }
                    var sqlStr = db.GetSql("BA00003-分页查询人员列表", null, param.ToArray());
                    //执行SQL脚本
                    items = sqlBuilder.SqlText(sqlStr)
                            .Paging(model.page_num, model.page_size)
                            .GetModelList<SystemUser>(out total);
                }
                listPage.Total = total;
                listPage.PageList = items;
                return listPage;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取人员异常错误!", ex);
            }
            return listPage;
        }
    }
}
