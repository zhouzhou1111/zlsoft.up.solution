using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UP.Basics;
using UP.Models.Admin.BasicData;
using UP.Models.Api;

namespace UP.Logics.Admin.BasicData
{
    public class BasicDataLogic
    {
        /// <summary>
        ///  新增或修改数据字典信息,只包含（编码,名称,简码字段的表）
        /// </summary>
        /// <param name="type">表名枚举类型</param>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="scode">简码</param>
        /// <param name="oldcode">旧编码</param>
        /// <param name="typeData">type为b_过敏源枚举时,值为药物字段,b_卫生机构类别时，值为层次字段,其他无效</param>
        /// <returns></returns>
        public ResponseModel UpdateDicItems(int type, string code, string name, string scode, string oldcode, int typeData = 0)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Error, "保存基础信息失败!");
            //字典表名
            string tablename = "";
            try
            {
                if (isTure(type))
                {
                    result.msg = "请求参数异常";
                    return result;
                }
                DicType dictype = (DicType)type;
                //字典表名
                tablename = dictype.ToString();
                //判断是否有该表名
                if (tablename.IsNullOrEmpty())
                {
                    result.msg = "未找到该基础数据枚举！";
                }
                using (var db = new DbContext())
                {
                    var items = 0;
                    var sqlBuilder = db.Sql("");
                    bool row = false;
                    //判断编码是否存在
                    var data = db.Sql($"select * from {tablename} where 编码='{code}'").Exists();
                    //如果该编码存在
                    if (data)
                    {
                        //判断旧编码是否有值,没值代表新增,编码重复
                        if (string.IsNullOrEmpty(oldcode))
                        {
                            row = true;
                        }
                        //判断旧编码和编码是否一致,不一致则为编码重复
                        else if (oldcode != code)
                        {
                            row = true;
                        }
                        else
                        {
                            row = false;
                        }
                        if (row)
                        {
                            result.msg = "编码已存在！";
                            return result;
                        }
                    }
                    //通过旧编码是否有值判断为新增还是修改
                    if (string.IsNullOrEmpty(oldcode))
                    {
                        if (type == (int)DicType.b_过敏源 || type == (int)DicType.b_卫生机构类别)
                        {
                            items = db.Sql($"insert into {tablename} values('{code}','{name}','{scode}','{typeData}')").Execute();
                        }
                        else
                        {
                            items = db.Sql($"insert into {tablename} values('{code}','{name}','{scode}')").Execute();
                        }
                    }
                    else
                    {
                        //判断旧编码是否有值
                        if (oldcode.IsNullOrEmpty())
                        {
                            result.msg = "服务器异常！";
                        }
                        var sqlStr = "";
                        var sqlwhere = "";
                        if (type == (int)DicType.b_过敏源 || type == (int)DicType.b_卫生机构类别)
                        {
                            if (type == (int)DicType.b_卫生机构类别)
                            {
                                sqlwhere = $",层次='{typeData}'";
                                //修改b_卫生机构类别基础信息
                                //sqlStr = $"Update {tablename} set 编码='{code}',简码='{scode}',名称='{name}',层次='{typeData}' where 编码='{oldcode}'";
                            }
                            else
                            {
                                sqlwhere = $",药物='{typeData}'";
                                //修改b_过敏源基础信息
                                //sqlStr = $"Update {tablename} set 编码='{code}',简码='{scode}',名称='{name}',药物='{typeData}' where 编码='{oldcode}'";
                            }
                        }
                        //修改基础信息
                        sqlStr = $"Update {tablename} set 编码='{code}',简码='{scode}',名称='{name}' {sqlwhere} where 编码='{oldcode}'";
                        //执行SQL脚本
                        items = sqlBuilder.SqlText(sqlStr).Execute();
                    }
                    //保存成功
                    if (items > 0)
                    {
                        result.code = (int)ResponseCode.Success;
                        result.msg = "保存成功！";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("基础数据表名：" + tablename + "修改失败", ex);
            }
            return result;
        }


        /// <summary>
        /// 分页获取基础数据
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="keyword">查询条件</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        public ResponseModel GetBasicDataList(int pageNum, int pageSize, string keyword, int type)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Error, "分页获取基础数据信息失败!");
            ListPageModel<BasicDataDto> item = null;
            //字典表名
            string tablename = "";
            try
            {
                if (isTure(type))
                {
                    result.msg = "请求参数异常";
                    return result;
                }
                DicType dictype = (DicType)type;
                //字典表名
                tablename = dictype.ToString();
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlwhere = "";
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        sqlwhere = " and (编码 like '%" + keyword + "%' or 名称 like '%" + keyword + "%' or 简码 like '%" + keyword + "%')";
                    }
                    var sql = $"select *,(select count(*)  from {tablename} where 1=1 {sqlwhere}  ) as total  from   {tablename} where 1=1 {sqlwhere}  order by 编码  limit {pageSize} offset (({pageNum}-1)*{pageSize})";

                    //执行SQL脚本
                    var items = db.Sql(sql).GetModelList<BasicDataDto>();
                    item = new ListPageModel<BasicDataDto>()
                    {
                        Total = items != null && items.Any() ? items.FirstOrDefault().total : 0,
                        PageList = items
                    };
                    result.code = (int)ResponseCode.Success;
                    result.msg = "查询成功";
                    result.data = item;

                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取" + tablename + "数据错误!", ex);
            }
            return result;
        }


        /// <summary>
        /// 获取基础数据信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        public ResponseModel GetBasicDataInfo(string code, int type)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Error, "获取基础数据信息失败!");
            //字典表名
            string tablename = "";
            try
            {
                if (isTure(type) || string.IsNullOrEmpty(code))
                {
                    result.msg = "请求参数异常";
                    return result;
                }
                DicType dictype = (DicType)type;
                //字典表名
                tablename = dictype.ToString();
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sql = $"select *   from   {tablename} where 编码='{code}'";
                    //执行SQL脚本
                    var item = db.Sql(sql).GetModel<BasicDataDto>();
                    result.code = (int)ResponseCode.Success;
                    result.msg = "查询成功";
                    result.data = item;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取" + tablename + "数据信息信息异常", ex);
            }
            return result;
        }

        /// <summary>
        /// 删除基础数据信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        public ResponseModel DelBasicData(string code, int type)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Error, "删除基础数据信息失败!");
            //字典表名
            string tablename = "";
            try
            {
                if (isTure(type) || string.IsNullOrEmpty(code))
                {
                    result.msg = "请求参数异常";
                    return result;
                }
                DicType dictype = (DicType)type;
                //字典表名
                tablename = dictype.ToString();
                //判断是否有该表名
                if (tablename.IsNullOrEmpty())
                {
                    result.msg = "未找到基础数据枚举信息";
                    return result;
                }
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sql = $"Delete from   {tablename} where 编码='{code}'";
                    //执行SQL脚本
                    var item = db.Sql(sql).GetModel<BasicDataDto>();
                    result.code = (int)ResponseCode.Success;
                    result.msg = "删除成功";
                    result.data = item;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("删除" + tablename + "数据信息异常!", ex);
            }
            return result;
        }

        /// <summary>
        /// 判断传入类型是否在枚举类型中（只包含编码,名称,简码的基础数据枚举）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool isTure(int type)
        {
            var istrue = false;
            if ((type != DicType.b_abo血型.ToInt32() && type != DicType.b_rh血型.ToInt32() && type != DicType.b_国籍.ToInt32() && type != DicType.b_学历.ToInt32() && type != DicType.b_民族.ToInt32() && type != DicType.b_社会关系.ToInt32() && type != DicType.b_社会性别.ToInt32() && type != DicType.b_职业.ToInt32() && type != DicType.b_职务.ToInt32() && type != DicType.b_过敏源.ToInt32() && type != DicType.b_卫生机构类别.ToInt32()))
            {
                istrue = true;
            }
            return istrue;
        }
    }
}
