using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Basics;
using UP.Models.DB.BasicData;

namespace UP.Logics.Admin.BasicData
{
    public class aboBloodTypeLogic
    {
        /// <summary>
        /// 分页获取abc血型数据
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="keyword">查询条件</param>
        /// <returns></returns>
        public ListPageModel<aboBloodType> GetaboBloodTypeList(int pageNum, int pageSize, string keyword)
        {
            ListPageModel<aboBloodType> item = null;
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
                    //获取用户基本信息
                    var sqlStr = db.GetSql("EA00001-分页获取abc血型", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<aboBloodType>(out int total);
                    item = new ListPageModel<aboBloodType>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取abc血型数据错误!", ex);
            }
            return item;
        }
    }
}
