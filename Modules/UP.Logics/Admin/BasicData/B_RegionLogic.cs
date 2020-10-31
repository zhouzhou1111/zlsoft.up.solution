using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UP.Basics;
using UP.Models.Admin.Org;
using UP.Models.DB.BasicData;

namespace UP.Logics.Admin.BasicData
{
   public class B_RegionLogic
    {
        /// <summary>
        /// 查询行政区划
        /// </summary>
        /// <param name="parent_code">上级编码</param>
        /// <param name="prop">性质</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public List<b_region> GetRegionList(string parent_code, int prop, string code)
        {
            //提示信息
            List<b_region> regionList = new List<b_region>();
            var param = new List<string>();
            try
            {
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    if (!parent_code.IsNullOrEmpty() && parent_code != "0")
                    {
                        sqlBuilder.Parameters("parent_code", parent_code);
                        param.Add("parent_code");
                    }
                    if (prop > -1)
                    {
                        sqlBuilder.Parameters("prop", prop);
                        param.Add("prop");
                    }
                    if (!string.IsNullOrEmpty(code))
                    {
                        sqlBuilder.Parameters("code", code);
                        param.Add("code");
                    }

                    //获取区域信息
                    var sqlStr = db.GetSql("EA00009-查询行政区划信息", null, param.ToArray());
                    //执行SQL脚本
                    regionList = sqlBuilder.SqlText(sqlStr).GetModelList<b_region>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询区域行政异常!", ex);
            }
            return regionList;
        }

        /// <summary>
        /// 新增或者修改行政区划信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AddorUpdate(b_region model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "保存行政区划失败!");
            var row = 0;
            try
            {
                using (var db = new DbContext())
                {
                    //判断行政区划编码是否存在
                    var dataList = db.Select("b_region").Columns("id").Where("code", model.code).GetModelList<b_region>();
                    model.pinyin = Basics.Utils.Strings.GetFirstPY(model.name.Trim());
                    if (model.id.IsNullOrEmpty())
                    {
                        var guid = Guid.NewGuid().ToString();
                        model.id = guid;
                        if (dataList != null && dataList.Any())
                        {
                            result.msg = "编码重复！";
                            return result;
                        }
                        row = db.Insert("b_region").Column("id", model.id).Column("code", model.code).Column("parent_code", model.parent_code).Column("name", model.name).Column("pinyin", model.pinyin).Column("prop", model.prop).Column("status", model.status).Column("location_coordinate", model.location_coordinate).Column("range_coordinate", model.range_coordinate).Column("source", model.source).Column("update_time", DateTime.Now).Column("year", model.year).Execute();
                    }
                    else
                    {
                        //判断行政区划编码是否存在
                        if (dataList != null && dataList.Any(d => d.id != model.id))
                        {
                            result.msg = "编码重复！";
                            return result;
                        } 
                        row = db.Update("b_region").Column("id", model.id).Column("code", model.code).Column("parent_code", model.parent_code).Column("name", model.name).Column("pinyin", model.pinyin).Column("prop", model.prop).Column("status", model.status).Column("location_coordinate", model.location_coordinate).Column("range_coordinate", model.range_coordinate).Column("source", model.source).Column("update_time", DateTime.Now).Column("year", model.year).Where("id", model.id).Execute();
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
                Logger.Instance.Error("保存行政区划失败", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }


        /// <summary>
        /// 根据行政区划id查询行政区划信息信息
        /// </summary>
        /// <param name="id">行政区划id</param>
        /// <returns></returns>
        public ResponseModel GetRegion(string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                if (!id.IsNullOrEmpty())
                {
                    using (var db = new DbContext())
                    {
                        //获取行政区划信息
                        var sqlStr = db.GetSql("EA00010-根据行政区划id查询行政区划信息", null, null);
                        //执行SQL脚本
                        var data = db.Sql(sqlStr).Parameters("id", id).GetModel<b_region>();
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
                Logger.Instance.Error("查询行政区划基本信息失败", ex);
                resModel.msg = "查询异常";
            }
            return resModel;
        }


        /// <summary>
        /// 修改行政区划状态
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="state">状态,0=停用，1=启用，-1=未生效</param>
        /// <returns></returns>
        public ResponseModel UpdateRegionState(string id, int state)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Error, "修改行政区划状态失败!");
            var row = 0;
            try
            {
                using (var db = new DbContext())
                {
                    row = db.Update("b_region").Column("status", state).Column("update_time", DateTime.Now).Where("id", id).Execute();
                }
                if (row == 1)
                {
                    result.msg = "修改行政区划状态成功";
                    result.code = (int)ResponseCode.Success;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("修改行政区划状态失败", ex);
                result.msg = "服务器内部异常";
            }
            return result;
        }


        /// <summary>
        /// 验证编码是否重复
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="id">id,判断新增还是修改</param>
        /// <returns></returns>
        public ResponseModel CheckCode(string code,string id)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "验证编码重复失败");
            try
            {
                var istrue = false;
                using (var db = new DbContext())
                {
                    var dataList = db.Select("b_region").Columns("id").Where("code", code).GetModelList<b_region>();
                    if (!string.IsNullOrEmpty(id))
                    {
                        istrue = dataList != null && dataList.Any(d => d.id != id);
                    }
                    else
                    {
                        istrue = dataList != null && dataList.Any();
                    }
                    resModel.code = (int)ResponseCode.Success;
                    resModel.msg = "验证成功";
                    resModel.data = istrue;
                }
                  
               
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("验证编码重复失败", ex);
                resModel.msg = "内部异常";
            }
            return  resModel;
        }
    }
}
