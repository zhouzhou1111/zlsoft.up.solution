/*********************************************************
* 功能：数据的新增、编辑、重复性验证
* 描述：对单表操作的数据提供基础操作功能
* 作者：李钢
* 日期：2020-03-27
*********************************************************/

using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using UP.Basics;

namespace UP.Logics.DBTable
{
    public class BasicDealWithLogic
    {
        //重复性验证:不存在返回 true 存在返回 false
        public string CheckExists(string tableName, string id, Dictionary<string, object> valuePairs)
        {
            //验证结果信息
            bool tag = false;
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "不存在!", true);
            try
            {
                using (var db = new DbContext())
                {
                    var selectBuilder = db.Select(tableName).Columns("id");
                    //当自身id不为空时
                    if (!string.IsNullOrEmpty(id))
                    {
                        selectBuilder.Where("id!=@id").Parameters("id", id);
                    }//end if

                    //验证字典集合
                    foreach (var item in valuePairs)
                    {
                        selectBuilder.Where(item.Key, item.Value);
                    }//end foreach
                    //验证结果
                    tag = selectBuilder.Exists();
                }//end using

                //如果存在
                if (tag)
                {
                    result.msg = "已存在!";
                    result.data = false;
                }//end if
            }
            catch (Exception ex)
            {
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "验证失败!";
            }
            return Strings.ObjectToJson(result);
        }

        //编辑数据实体
        public ResponseModel EditModel(object entity)
        {
            //提示信息
            var msg = "新增";
            //执行时解析成对应实体
            dynamic model = entity;
            //验证结果
            bool tag = false;

            using (var db = new DbContext())
            {
                //生成操作时间
                model.oper_time = db.Database().GetDateTime;
                //id为空时进行新增操作
                if (string.IsNullOrEmpty(model.id))
                {
                    //生成guid
                    model.id = Strings.GuidID();
                    tag = db.Insert(model).Execute() > 0;
                }
                else
                {
                    msg = "修改";
                    //进行修改操作
                    tag = db.Update(model).Where("id", model.id).Execute() > 0;
                }//end if
            }//end using

            //提示信息
            var format = new ResponseModel(ResponseCode.Success, msg + "成功!");
            if (!tag)
            {
                format.code = ResponseCode.Error.ToInt32();
                format.msg = msg += "失败!";
            }//end if
            return format;
        }

        //新增数据实体
        public ResponseModel AddModel(object entity)
        {
            //提示信息
            var format = new ResponseModel(ResponseCode.Success, "新增成功!");
            //执行时解析成对应实体
            dynamic model = entity;
            //验证结果
            bool tag = false;
            using (var db = new DbContext())
            {
                //生成Guid
                model.id = Strings.GuidID();
                //生成操作时间
                model.oper_time = db.Database().GetDateTime;
                tag = db.Insert(model).Execute() > 0;
            }//end using

            //操作失败时
            if (!tag)
            {
                format.code = ResponseCode.Error.ToInt32();
                format.msg = "新增失败!";
            }//end if
            return format;
        }

        //修改数据实体
        public ResponseModel UpdateModel(object entity)
        {
            //提示信息
            var format = new ResponseModel(ResponseCode.Success, "修改成功!");
            //执行时解析成对应实体
            dynamic model = entity;
            //验证结果
            bool tag = false;

            using (var db = new DbContext())
            {
                //生成操作时间
                model.oper_time = db.Database().GetDateTime;
                //修改表数据
                tag = db.Update(model).Where("id", model.id).Execute() > 0;
            }//end using

            //操作失败时
            if (!tag)
            {
                format.code = ResponseCode.Error.ToInt32();
                format.msg = "修改失败!";
            }//end if
            return format;
        }


        //根据id返回实体数据
        public string GetModelInfo(object entity)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Forbidden, "数据不存在!");
            //运行时解析
            dynamic model = entity;
            //获取实体类型
            Type t = model.GetType();
            //保存属性名
            List<string> columnList = new List<string>();
            //取出表名
            var tableName = t.Name;
            //保存结果
            DataTable dt = new DataTable();
            //遍历实体类型属性集合
            foreach (var item in t.GetProperties())
            {
                //获取属性名称
                var name = item.Name;
                columnList.Add(name);
            }//end foreach

            using (var db = new DbContext())
            {
                //查找语句
                dt = db.Select(tableName).Columns(string.Join(",", columnList)).Where("id", model.id).Select();
            }
            //获取表数据id
            var id = dt.GetValueByName<string>("id");
            //id存在时
            if (!string.IsNullOrEmpty(id))
            {
                result.code = ResponseCode.Success.ToInt32();
                result.msg = "查询成功!";
                result.data = dt;
            }
            return Strings.ObjectToJson(result);
        }

        //删除功能（单表删除、物理删除）
        public string DeleteModel(object entity)
        {
            //提示信息
            var resultInfor = new ResponseModel(ResponseCode.Success, "删除成功！");
            //运行时解析
            dynamic model = entity;
            //获取实体类型
            Type t = model.GetType();
            //取出表名
            var tableName = t.Name;
            //验证结果
            bool tag = false;
            try
            {
                using (var db = new DbContext())
                {
                    //删除语句
                    tag = db.Delete(tableName).Where("id", model.id).Execute() > 0;
                }
                //删除失败时
                if (!tag)
                {
                    resultInfor.code = ResponseCode.Error.ToInt32();
                    resultInfor.msg = "删除失败！";
                }
            }
            catch (Exception ex)
            {
                resultInfor.code = ResponseCode.Error.ToInt32();
                resultInfor.msg = "删除失败！";
            }
            return Strings.ObjectToJson(resultInfor);
        }
    }
}
