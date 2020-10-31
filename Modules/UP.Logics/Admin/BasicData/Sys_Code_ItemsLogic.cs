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
    public class Sys_Code_ItemsLogic
    {

        /// <summary>
        /// 根据基础数据id分页查询基础数据
        /// </summary>
        /// <param name="catgoryid"></param>
        /// <returns></returns>
        public ListPageModel<sys_code_items> GetitemsListByCid(string catgoryid, int pageNum, int pageSize, string keyword)
        {
            ListPageModel<sys_code_items> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件不为空
                    if (!keyword.IsNullOrEmpty())
                    {
                        param.Add("keyword");
                        sqlBuilder.Parameters("keyword", keyword);
                    }
                    if (!catgoryid.IsNullOrEmpty())
                    {
                        param.Add("catgoryid");
                        sqlBuilder.Parameters("catgoryid", catgoryid);
                    }

                    //获取用户基本信息
                    var sqlStr = db.GetSql("EA00005-根据基础数据分类查询基础数据", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<sys_code_items>(out int total);
                    item = new ListPageModel<sys_code_items>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询基础数据异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 新增或者修改基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddorUpdate(sys_code_items model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "保存基础数据失败!");
            var row = 0;
            try
            {
                using (var db = new DbContext())
                {
                    //判断分类名称是否存在
                    var dataList = db.Select("sys_code_items").Columns("id").Where("code", model.code).Where("code_catgory_id", model.code_catgory_id).GetModelList<sys_code_items>();
                    model.pinyin = Basics.Utils.Strings.GetFirstPY(model.name.Trim());
                    if (model.id.IsNullOrEmpty())
                    {
                        var guid = Guid.NewGuid().ToString();
                        model.id = guid;
                        if (dataList != null && dataList.Any())
                        {
                            result.msg = "基础数据编码重复";
                            return result;
                        }
                        row = db.Insert("sys_code_items").Column("id", model.id).Column("parent_id", model.parent_id).Column("name", model.name).Column("describe", model.describe).Column("code", model.code).Column("pinyin", model.pinyin).Column("stdd_code", model.stdd_code).Column("update_time", DateTime.Now).Column("sno", model.sno).Column("code_catgory_id", model.code_catgory_id).Column("status", model.status).Execute();
                    }
                    else
                    {
                        //判断分类名称是否存在
                        if (dataList != null && dataList.Any(d => d.id != model.id))
                        {
                            result.msg = "基础数据编码重复";
                            return result;
                        }
                        row = db.Update("sys_code_items").Column("parent_id", model.parent_id).Column("name", model.name).Column("describe", model.describe).Column("code", model.code).Column("pinyin", model.pinyin).Column("stdd_code", model.stdd_code).Column("code_catgory_id", model.code_catgory_id).Column("status", model.status).Column("update_time", DateTime.Now).Column("sno", model.sno).Where("id", model.id).Execute();
                    }
                    if (row == 1)
                    {
                        result.msg = "保存成功";
                        result.code = (int)ResponseCode.Success;
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Error("保存基础数据失败", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }


        /// <summary>
        /// 根据分类id查询基础数据信息
        /// </summary>
        /// <param name="id">基础数据id</param>
        /// <returns></returns>
        public ResponseModel GetItems(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                if (!id.IsNullOrEmpty())
                {
                    using (var db = new DbContext())
                    {
                        //获取基础数据分类
                        var sqlStr = db.GetSql("EA00007-根据基础数据id查询基础数据", null, null);
                        //执行SQL脚本
                        var data = db.Sql(sqlStr).Parameters("id", id).GetModel<ItemsDto>();
                        resModel.data = data;
                        resModel.code = (int)ResponseCode.Success;
                        resModel.msg = "查询成功";
                    }
                }
                else
                {
                    resModel.msg = "参数异常";
                }
                return resModel;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询基础数据信息失败", ex);
                resModel.msg = "查询异常";
            }
            return resModel;
        }


        /// <summary>
        /// 修改基础数据状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        public ResponseModel UpdateItemsState(string id, int state)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "修改基础数据状态失败!");
            var row = 0;
            try
            {
                using (var db = new DbContext())
                {
                    row = db.Update("sys_code_items").Column("status", state).Column("update_time", DateTime.Now).Where("id", id).Execute();
                }
                if (row == 1)
                {
                    result.msg = "修改基础数据状态成功";
                    result.code = (int)ResponseCode.Success;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("修改基础数据状态失败", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }

        /// <summary>
        /// 根据分类id查询基础数据
        /// </summary>
        /// <param name="catgoryid">分类id</param>
        /// <param name="id">修改时不包含本身id</param>
        /// <returns></returns>
        public List<ItemsDto> GetitemsListByCid(string catgoryid, string id)
        {
            List<ItemsDto> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //获取用户基本信息
                    var sqlStr = db.GetSql("EA00008-根据分类id查询基础数据树", null, null);
                    //执行SQL脚本
                    item = sqlBuilder.SqlText(sqlStr).Parameters("cid", catgoryid).GetModelList<ItemsDto>(out int total);
                    if (!id.IsNullOrEmpty())
                    {
                        item = item.Where(d => d.id != id).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询基础数据异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 根据id验证基础数据表是否存在
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ResponseModel TableIsExist(string cid)
        {
            var result = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //根据id查询基础分类
                using (var db = new DbContext())
                {
                    var data = db.Select("sys_code_catgory").Columns("id").Columns("name").Columns("ref_table").Columns("stdd_code").Columns("stdd_source").Where("id", cid).GetModel<sys_code_catgory>();
                    if (data != null)
                    {
                        if (!data.ref_table.IsNullOrEmpty())
                        {
                            //判断基础数据表名是否存在
                            try
                            {
                                var sqls2 = db.Sql($"SELECT * FROM  {data.ref_table}").Select();
                                result.msg = "基础数据表已存在";
                                result.data = 1;
                                result.code = (int)ResponseCode.Success;
                            }
                            catch (Exception ex)
                            {
                                result.msg = "基础数据表不存在";
                                result.data = 0;
                                result.code = (int)ResponseCode.Success;
                            }
                        }
                        else
                        {
                            result.msg = "请完善基础数据分类信息动态表名";
                        }
                    }
                    else
                    {
                        result.msg = "未找到基础数据分类";
                    }

                };
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("验证基础数据分类异常", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }

        /// <summary>
        /// 同步基础数据
        /// </summary>
        /// <param name="cid">分类id</param>
        /// <param name="type">判断是否删除表</param>
        /// <returns></returns>
        public bool SynchroBasicTable(string cid, int type)
        {
            var istrue = false;
            var row = 0;
            try
            {
                //根据id查询基础分类
                using (var db = new DbContext())
                {
                    var data = db.Select("sys_code_catgory").Columns("name").Columns("ref_table").Where("id", cid).GetModel<sys_code_catgory>();
                    //判断基础数据表名是否存在
                    try
                    {
                        var sql = "";
                        if (type != 0)
                        {
                            //删除表,创建表
                            sql = $"Drop TABLE {data.ref_table}";
                            row = db.Sql(sql).Execute();
                        }
                        //,parent_id  varchar(36) 无pid字段 暂时注释,创建表
                        sql = $"CREATE TABLE {data.ref_table} (id  varchar(36),name  varchar(50), code  varchar(50), pinyin  varchar(50),describe  varchar(200),status   numeric(1),update_time timestamp, sno  int )";
                        row = db.Sql(sql).Execute();
                        //查询该类型基础数据条数
                        var count = db.Select("sys_code_items").Columns("id").Where("code_catgory_id", cid).GetModelList<sys_code_items>().Count;
                        //插入数据
                        //, parent_id无pid字段 暂时注释
                        sql = $"Insert into {data.ref_table} select id, name,code,pinyin,describe,status,update_time,sno from  sys_code_items where code_catgory_id='{cid}'";
                        row= db.Sql(sql).Execute();
                        //同步条数与查询数量相同,返回true为同步成功
                        if (count == row)
                        {
                            istrue = true;
                        }
                        else
                        {
                            istrue = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Error("同步基础数据出错", ex);
                    }
                };
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("同步基础数据表失败", ex);

            }
            return istrue;
        }
    }
}
