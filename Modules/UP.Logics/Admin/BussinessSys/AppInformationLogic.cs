using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Basics;
using UP.Models.Admin.BusinessSys;

namespace UP.Logics.Admin.BussinessSys
{
   public class AppInformationLogic
    {
        /// <summary>
        /// 分页获取系统_应用信息
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="productid">产品id</param>
        /// <returns></returns>
        public ListPageModel<AppInformationDto> GetAppInformationList(int pageNum, int pageSize, string keyword, int productid)
        {
            ListPageModel<AppInformationDto> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件不为空
                    if (keyword.IsNotNullOrEmpty())
                    {
                        param.Add("keyword");
                        sqlBuilder.Parameters("keyword", keyword);
                    }
                    if (productid != 0)
                    {
                        param.Add("productid");
                        sqlBuilder.Parameters("productid", productid);
                    }
                    //获取用户基本信息
                    var sqlStr = db.GetSql("DA0004-分页查询应用信息", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<AppInformationDto>(out int total);
                    item = new ListPageModel<AppInformationDto>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取系统_应用信息错误!", ex);
            }
            return item;
        }
    }
}
