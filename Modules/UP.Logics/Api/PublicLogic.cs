using Microsoft.Net.Http.Headers;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UP.Basics;
using UP.Basics.Models;
using UP.Models.Api;

namespace UP.Logics.Api.Public
{
    /// <summary>
    /// 公共业务逻辑层
    /// </summary>
    public class PublicLogic
    {

        /// <summary>
        /// 查询数据字典信息
        /// </summary>
        /// <returns></returns>
        public JsonMsg<List<DicInfo>> GetDicItems(DicParam entity)
        {
            //提示信息
            var result = JsonMsg<List<DicInfo>>.OK(null, "查询成功");
            //个人id必传
            if (entity.type <= 0)
            {
                result.code = ResponseCode.Error;
                result.msg = "请选择需要查询的字典信息!";
                return result;
            }
            var param = new List<string>();
            try
            {
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    DicType dictype = (DicType)entity.type;
                    //字典表名
                    string tablename = dictype.ToString();
                    //获取字典信息
                    var sqlStr = $"SELECT '{tablename}' as 表名,编码, 简码, 名称  FROM { tablename}";
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr)
                        .GetModelList<DicInfo>();
                    result.data = items;
                }
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Error;
                result.msg = "查询数据字典信息发生异常!";
                Logger.Instance.Error(result.msg, ex);
            }
            return result;
        } 
    }
}