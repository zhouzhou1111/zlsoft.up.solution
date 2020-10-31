using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.Models;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using UP.Basics;

namespace UP.Logics
{
    public class DBTableLogic
    {
        /// <summary>
        /// 执行对象插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(dynamic entity)
        {
            using (var db = new DbContext())
            {
                //生成操作时间
                //entity.oper_time = db.Database().GetDateTime;
                return db.Insert(entity).Execute();
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public int Delete(ExecuteModel executeModel)
        {
            using (var db = new DbContext())
            {
                //获取当前类名
                var tableName = GetTableName(executeModel.Entity.GetType());
                var delBuilder = db.Delete(tableName);
                //获取条件sql
                var whereSql = GetConitionsql(executeModel.Conditions);
                delBuilder.Where(whereSql);
                executeModel.Conditions?.ForEach(p =>
                {//先设置参数，组装SQL

                    delBuilder.Parameters(p.Key, p.Value);
                });

                return delBuilder.Execute();
            }
        }


        /// <summary>
        /// 更新数据表
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public int Update(ExecuteModel executeModel)
        {
            using (var db = new DbContext())
            {
                //生成操作时间
                //executeModel.Entity.oper_time = db.Database().GetDateTime;

                //设置不立即设置为根据主键更新，后续根据和条件更新
                IUpdateBuilder updateBuilder = db.Update(executeModel.Entity, false);
                if (executeModel.Columns != null && executeModel.Columns.Length > 0)
                {//如果更新有指定列，则指定列
                    updateBuilder.Column(executeModel.Columns);
                }

                //获取条件sql
                var whereSql = GetConitionsql(executeModel.Conditions);
                updateBuilder.Where(whereSql);
                executeModel.Conditions?.ForEach(p =>
                {//先设置参数，组装SQL 
                    updateBuilder.Parameters(p.Key, p.Value);
                });

                //执行更新
                return updateBuilder.Execute();
            }
        }

        /// <summary>
        /// 检查记录是否存
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public bool Exists(ExecuteModel executeModel)
        {
            using (var db = new DbContext())
            {
                //查询器
                var selectBuilder = CreateBuilder(db, executeModel);
                selectBuilder.Columns("1");

                //返回执行结果
                return selectBuilder.Exists();
            }
        }


        /// <summary>
        /// 查询出一个model数据
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public object GetModel(ExecuteModel executeModel)
        {
            using (var db = new DbContext())
            {
                //查询器
                var selectBuilder = CreateBuilder(db, executeModel);
                if (executeModel.Columns != null && executeModel.Columns.Length > 0)
                {//如果更新有指定列，则指定列
                    selectBuilder.Columns(executeModel.Columns);
                }
                else
                {//未指定列
                    selectBuilder.Columns("*");
                }

                var dt = selectBuilder.Select();
                if (dt.HasData())
                {
                    //将对象序列化
                    return QWPlatform.SystemLibrary.Serialize.SerializeHelper.SerializeBinary(dt);
                }

                return null;
            }
        }

        //获取Model列表
        public RIPListPageModel GetModelList(ExecuteModel executeModel)
        {
            using (var db = new DbContext())
            {
                //查询器
                var selectBuilder = CreateBuilder(db, executeModel);
                if (executeModel.Columns != null && executeModel.Columns.Length > 0)
                {//如果更新有指定列，则指定列
                    selectBuilder.Columns(executeModel.Columns);
                }
                else
                {//未指定列
                    selectBuilder.Columns("*");
                }

                var dt = selectBuilder.Select();
                if (dt.HasData())
                {
                    //将对象序列化
                    var data = QWPlatform.SystemLibrary.Serialize.SerializeHelper.SerializeBinary(dt);
                    return new RIPListPageModel
                    {
                        Total = dt.Rows.Count,
                        Value = data
                    };
                }

                return null;
            }
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="executeModel"></param>
        /// <returns></returns>
        public RIPListPageModel GetModelPageList(ExecuteModel executeModel)
        {
            using (var db = new DbContext())
            {
                //查询器
                var selectBuilder = CreateBuilder(db, executeModel);
                if (executeModel.Columns != null && executeModel.Columns.Length > 0)
                {//如果更新有指定列，则指定列
                    selectBuilder.Columns(executeModel.Columns);
                }
                else
                {//未指定列
                    selectBuilder.Columns("*");
                }

                //设置分页码
                selectBuilder.Paging(executeModel.Page, executeModel.PageSize);
                var total = 0;
                var dt = selectBuilder.Select(out total);
                if (dt.HasData())
                {
                    //将对象序列化
                    var data = QWPlatform.SystemLibrary.Serialize.SerializeHelper.SerializeBinary(dt);
                    return new RIPListPageModel
                    {
                        Total = total,
                        Value = data
                    };
                }

                return null;
            }
        }

        #region 内部方法
        //获取表名称
        private string GetTableName(Type modelType)
        {
            var tabAttrObj = modelType.GetCustomAttributes(typeof(TableAttribute), true);
            if (tabAttrObj.Length > 0)
            {
                var tabAttr = tabAttrObj.First() as TableAttribute;
                if (tabAttr != null)
                {
                    return tabAttr.TableName;
                }
            }
            else
            {//直接返回model的名称
                return modelType.Name;
            }

            throw new ArgsException("未设置Model的表名称" + modelType.FullName, "null", new Exception());
        }

        //获取where条件的sql语句
        private string GetConitionsql(List<ConditionModel> conditions)
        {
            //使用了And的条件
            var andlist = new List<string>();

            //使用了Or条件
            var orList = new List<string>();
            conditions?.ForEach(p =>
            {
                var pNames = "";
                //默认设置条件为=符号
                var where = $" {p.Key}=@{p.Key} ";
                switch (p.Oper)
                {
                    case Operational.NotEqual:
                        where = $" {p.Key}<>@{p.Key} ";
                        break;
                    case Operational.Greater:
                        where = $" {p.Key}>@{p.Key} ";
                        break;
                    case Operational.Less:
                        where = $" {p.Key}<@{p.Key} ";
                        break;
                    case Operational.In:
                        pNames = GetParams(p.Key, p.Value);
                        where = $" {p.Key} In ({pNames}) ";
                        break;
                    case Operational.NotIn:
                        pNames = GetParams(p.Key, p.Value);
                        where = $" {p.Key} Not In ({pNames}) ";
                        break;
                    case Operational.Like:
                        where = $" {p.Key} Like '%' || @{p.Key} || '%' ";
                        break;
                    case Operational.LeftLike:
                        where = $" {p.Key} Like '%' || @{p.Key} ";
                        break;
                    case Operational.RightLike:
                        where = $" {p.Key} Like  @{p.Key} || '%' ";
                        break;
                    case Operational.Equal:
                    default:
                        break;
                }

                if (p.Where == WhereType.And)
                    andlist.Add(where);
                else if (p.Where == WhereType.Or)
                    orList.Add(where);
            });

            var andWhereSql = andlist.ToWhereString(" AND ", "");
            var orWhereSql = orList.ToWhereString(" OR ", "");

            if (andlist.Count > 0 && orList.Count > 0)
            {//如果包括了And条件与Or条件
                return $" {andWhereSql} OR {orWhereSql} ";
            }
            else if (andlist.Count > 0)
            {//只有And条件
                return andWhereSql;
            }
            else
            {//只有Or条件
                return orWhereSql;
            }
        }

        //创建一个查询对象构造器
        private ISelectBuilder CreateBuilder(DbContext db, ExecuteModel executeModel)
        {
            //获取当前类名
            var tableName = GetTableName(executeModel.Entity.GetType());

            //查询器
            ISelectBuilder selectBuilder = db.Select(tableName);

            //获取条件sql
            var whereSql = GetConitionsql(executeModel.Conditions);
            //添加SQL条件
            selectBuilder.Where(whereSql);
            executeModel.Conditions?.ForEach(p =>
            {//先设置参数，组装SQL  
                if (p.Oper == Operational.In || p.Oper == Operational.NotIn)
                {//In 或 not in 通过设置参数方式
                    GetParams(p.Key, p.Value, selectBuilder);
                }
                else
                {
                    selectBuilder.Parameters(p.Key, p.Value);
                }
            });

            if (executeModel.OrderColumn.IsNotNullOrEmpty())
            {//如果包括了排序字段，则使用排序
                selectBuilder.OrderBy(executeModel.OrderColumn);
            }//end if            

            //返回组装的对象
            return selectBuilder;
        }

        //根据值类型，动态组装参数名称或值
        private string GetParams(string key, object value, ISelectBuilder selectBuilder = null)
        {
            if (value != null)
            {
                var list = new List<string>();
                var type = value.GetType();
                var array = value as Array;
                if (array != null)
                {//数组类型
                    for (int i = 0; i < array.Length; i++)
                    {
                        var pName = $"@{key}_{i}";
                        list.Add(pName);
                        if (selectBuilder != null)
                        {//设置参数到查询器中
                            var val = array.GetValue(i);
                            selectBuilder.Parameters($"{key}_{i}", val);
                        }
                    }//end for

                    //各参数之间使用
                    return string.Join(",", list);
                }//end if

                return string.Empty;
            }

            throw new ArgumentNullException("使用了In或NotIn方式查询，但没有给出范围值");
        }
        #endregion


    }//end class
}//end namespace
