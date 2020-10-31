/****************************************************
* 功能：模块管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/10 16:04:03
*********************************************************/

using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using UP.Models.DB.RoleRight;

namespace UP.Logics.Admin.Modules
{

    public class ModulesLogic
    {
        public List<ModulesInfo> SelectModulesList()
        {
            List<ModulesInfo> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取模块列表
                    var sqlStr = db.GetSql("AA00010-查询模块列表", null, null);
                    //执行SQL脚本
                    item = db.Sql(sqlStr).GetModelList<ModulesInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询模块列表异常错误!", ex);
            }
            return item;
        }
    }
}
