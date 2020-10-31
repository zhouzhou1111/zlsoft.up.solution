/****************************************************
* 功能：系统_产品数据源业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/17 9:04:03
*********************************************************/

using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using UP.Basics;
using UP.Models.Admin.BusinessSys;
using UP.Models.Admin.Org;
using UP.Models.DB.BusinessSys;

namespace UP.Logics.Admin.BussinessSys
{
    public class DataSourceLogic
    {
        /// <summary>
        /// 分页获取系统_产品数据源
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="productid">产品id</param>
        /// <param name="databasetype">数据库类型</param>
        /// <returns></returns>
        public ListPageModel<DataSourceDto> GetDataSourceList(int pageNum, int pageSize, string keyword,int productid,int databasetype)
        {
            ListPageModel<DataSourceDto> item = null;
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
                    if (databasetype!=0)
                    {
                        param.Add("databasetype");
                        sqlBuilder.Parameters("databasetype", databasetype);
                    }
                    //获取用户基本信息
                    var sqlStr = db.GetSql("DA00002-分页查询产品数据源", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<DataSourceDto>(out int total);
                    item = new ListPageModel<DataSourceDto>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取系统_产品数据源信息错误!", ex);
            }
            return item;
        }
    }
}
