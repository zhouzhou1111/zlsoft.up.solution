using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UP.Basics;
using UP.Models.Admin.BasicData;
using UP.Models.DB.BasicData;

namespace UP.Logics.Admin.BasicData
{
    public class NationalityLogic
    {
        /// <summary>
        /// 分页获取国籍数据
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="keyword">查询条件</param>
        /// <returns></returns>
        public ListPageModel<BasicDataDto> GetNationalityList(int pageNum, int pageSize, string keyword)
        {
            ListPageModel<BasicDataDto> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    //var sqlBuilder = db.Sql("");
                    //////查询条件不为空
                    ////if (keyword.IsNotNullOrEmpty())
                    ////{
                    ////    param.Add("keyword");
                    ////    sqlBuilder.Parameters("keyword", keyword);
                    ////}
                    ////获取用户基本信息
                    //var sqlStr = db.GetSql("EA00003-分页获取国籍血型", null, param.ToArray());
                    ////执行SQL脚本
                    //var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<Nationality>(out int total);
                    //item = new ListPageModel<Nationality>()
                    //{
                    //    Total = total,
                    //    PageList = items
                    //};

                    //获取角色的模块信息
                    //var sqlStr = db.GetSql("EA00004-分页查询基础数据只包含编码名称简码", null, null);
                    string tablename = "b_国籍";
                    //执行SQL脚本
                    var items = db.Sql($"select *,(select count(*)  from {tablename} ) as total  from   {tablename}  order by 编码  limit {pageSize} offset (({pageNum}-1)*{pageSize})").GetModelList<BasicDataDto>();
                    item = new ListPageModel<BasicDataDto>()
                    {
                        Total = items != null && items.Any() ? items.FirstOrDefault().total : 0,
                        PageList = items
                    };

                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取国籍数据错误!", ex);
            }
            return item;
        }
    }
}
