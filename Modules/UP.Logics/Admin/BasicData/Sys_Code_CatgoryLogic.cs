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
    public class Sys_Code_CatgoryLogic
    {
        /// <summary>
        /// 获取基础数据分类列表
        /// </summary>
        /// <returns></returns>
        public List<CatgoryTreeDto> GetSys_Code_CatgoryList()
        {
            List<sys_code_catgory> item = null;
            List<CatgoryTreeDto> items = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取基础数据分类
                    var sqlStr = db.GetSql("EA00004-查询基础数据分类列表", null, null);
                    //执行SQL脚本
                    item = db.Sql(sqlStr).GetModelList<sys_code_catgory>();
                     items = GetCatgoryTreeItems(item, "0");

                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询基础数据分类异常错误!", ex);
            }
            return items;
        }

        public List<CatgoryTreeDto> GetCatgoryTreeItems(List<sys_code_catgory> CatgoryList, string pid)
        {
            List<CatgoryTreeDto> itemsList = new List<CatgoryTreeDto>();
            var children = CatgoryList.Where(t => t.parent_id==""||t.parent_id == pid).ToList();
            foreach (var item in children)
            {
                CatgoryTreeDto model = new CatgoryTreeDto()
                {
                    describe = item.describe,
                    id = item.id,
                    name = item.name,
                    parent_id = item.parent_id,
                    ref_table = item.ref_table,
                    status = item.status,
                    stdd_code = item.stdd_code,
                    stdd_source = item.stdd_source,
                    text = item.name,
                    update_time = item.update_time,
                    Items = GetCatgoryTreeItems(CatgoryList, item.id)
                };
                itemsList.Add(model);
            }
            return itemsList;
        }

        /// <summary>
        /// 新增或者修改基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddorUpdate(sys_code_catgory model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "保存基础数据分类失败!");
            var msg = "";
            var row = 0;
            try
            {
                using (var db = new DbContext())
                {
                    //判断分类名称是否存在
                    var dataList = db.Select("sys_code_catgory").Columns("id").Where("name", model.name).GetModelList<sys_code_catgory>();
                    //验证动态表名是否存在
                    var dataList2 = new List<sys_code_catgory>();
                    if (!string.IsNullOrEmpty(model.ref_table))
                    {
                        dataList2 = db.Select("sys_code_catgory").Columns("id").Where("ref_table", model.ref_table).GetModelList<sys_code_catgory>();
                    }
                    if (model.id.IsNullOrEmpty())
                    {
                        var guid = Guid.NewGuid().ToString();
                        model.id = guid;
                        if (dataList!=null&&dataList.Any())
                        {
                            msg = "分类名称重复,";
                        }
                        if (dataList2!= null && dataList2.Any())
                        {
                            msg += "动态表名重复,";
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            msg = msg.Substring(0, msg.Length - 1);
                            result.msg = msg;
                            return result;
                        }
                        row = db.Insert("sys_code_catgory").Column("id", model.id).Column("parent_id", model.parent_id).Column("name", model.name).Column("describe", model.describe).Column("ref_table", model.ref_table).Column("stdd_code", model.stdd_code).Column("stdd_source", model.stdd_source).Column("status", model.status).Column("update_time", DateTime.Now).Execute();
                    }
                    else
                    {
                        //判断分类名称是否存在 
                        if (dataList != null && dataList.Any(d=>d.id!=model.id))
                        {
                            msg = "分类名称重复,";
                        }
                        if (dataList2 != null && dataList2.Any(d => d.id != model.id))
                        {
                            msg += "动态表名重复,";
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            msg = msg.Substring(0, msg.Length - 1);
                            result.msg = msg;
                            return result;
                        }
                        row = db.Update("sys_code_catgory").Column("parent_id", model.parent_id).Column("name", model.name).Column("describe", model.describe).Column("ref_table", model.ref_table).Column("stdd_code", model.stdd_code).Column("stdd_source", model.stdd_source).Column("status", model.status).Column("update_time", DateTime.Now).Where("id", model.id).Execute();
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
                Logger.Instance.Error("保存基础数据分类失败", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }


        /// <summary>
        /// 根据分类id查询分类信息
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        public ResponseModel GetCatgory(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                if (!id.IsNullOrEmpty())
                {
                    using (var db = new DbContext())
                    {
                        //获取基础数据分类
                        var sqlStr = db.GetSql("EA00006-根据基础数据分类id查询分类", null, null);
                        //执行SQL脚本
                        var data = db.Sql(sqlStr).Parameters("id", id).GetModel<CatgoryDto>();
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
                Logger.Instance.Error("查询基础数据分类信息失败", ex);
                resModel.msg = "查询异常";
            }
            return resModel;
        }


        /// <summary>
        /// 修改基础数据分类状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        public ResponseModel UpdateCatgoryState(string id, int state)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "修改基础数据分类状态失败!");
            var row = 0;
            try
            {
                using (var db = new DbContext())
                {
                    row = db.Update("sys_code_catgory").Column("status", state).Column("update_time", DateTime.Now).Where("id", id).Execute();
                }
                if (row == 1)
                {
                    result.msg = "修改基础数据分类状态成功";
                    result.code = (int)ResponseCode.Success;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("修改基础数据分类状态失败", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }

    }
}
