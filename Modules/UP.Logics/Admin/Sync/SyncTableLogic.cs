/*
 * why:2020-09-10
 * 同步表的逻辑层
 */
using QWPlatform.DataIntface;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UP.Models.Admin.Sync;

namespace UP.Logics.Admin.Sync
{
    /// <summary>
    /// 同步表逻辑层
    /// </summary>
    public class SyncTableLogic
    {
        //基础数据模式名称
        private const string MODULE_NAME = "qw_base";

        //是否正在同步中
        private static Dictionary<int, bool> DicRunInitSync = new Dictionary<int, bool>();

        //获取基础表集合
        public Task<List<string>> GetBaseTables()
        {
            using (var db = new DbContext())
            {
                var sql = db.GetSql("PG00002-获取基础表目录", null, null);
                var dt = db.Sql(sql)
                          .Select();

                var list = new List<string>();
                if (dt.HasData())
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        var tabName = dataRow.GetValueByName<string>("tablename");
                        list.Add(tabName);
                    }


                    return Task.FromResult(list);
                }

                return Task.FromResult(list);
            }
        }

        /// <summary>
        /// 指定数据源同步表(所有表同步)
        /// 1.查询需要同步的基础数据表《系统_同步基础表》
        /// 2.将所有《系统_同步基础表》查询出来存放到Redis中缓存
        /// 3.将所有《系统_同步基础表》查询出来生成每个表的结构脚本
        /// 4.在数据源中检查是否存在表，不存在，则创建脚本，存在则比较结构是否一致，如果不一致则删除表，重新生成一个新表结构
        /// 5.开始同步数据，并记录到《系统_同步进度》表中。记录各表的情况。以便查询
        /// </summary>
        /// <param name="dataSourceId"></param>
        public Task<bool> InitAllTable(string dataSourceId)
        {
            List<sys_sync_table> tableList = null;
            using (var db = new DbContext())
            {//查询基础表
                tableList = db.Select("sys_sync_table")
                             .Columns("*")
                             .Where("database_id", dataSourceId)
                             .OrderBy("sno")
                             .GetModelList<sys_sync_table>();
                if (tableList.Count == 0)
                {
                    Logger.Instance.Warn("未在sys_sync_table表中配置同步数据表");
                    return Task.FromResult(false);
                }
            }

            //获取数据源信息
            var dbSrouce = GetConnectionStr(dataSourceId);
            if (dbSrouce != null)
            {//获取到目标源信息

                switch (dbSrouce.Item1)
                {
                    case DBType.Oracle:
                        break;
                    case DBType.OracleNoClient:
                        break;
                    case DBType.SQLServer:
                        break;
                    case DBType.PostGreSql://PG库
                        PostgresInitTable(dbSrouce.Item2, tableList, dataSourceId);
                        break;
                    case DBType.MySql:
                        break;
                    case DBType.DB2:
                        break;
                    case DBType.SQLite:
                        break;
                    default:
                        break;
                }
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// 指数据源及表进行单个同步
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Task<bool> InitTable(string dataSourceId, string tableName)
        {
            var dbSrouce = GetConnectionStr(dataSourceId);
            if (dbSrouce != null)
            {//获取到目标源信息

                List<sys_sync_table> tableList = null;
                using (var db = new DbContext())
                {//查询基础表
                    tableList = db.Select("sys_sync_table")
                                 .Columns("*")
                                 .Where("database_id", dataSourceId)
                                 .Where("table_name", tableName)
                                 .GetModelList<sys_sync_table>();
                    if (tableList.Count == 0)
                    {
                        Logger.Instance.Warn("未在sys_sync_table表中配置同步数据表");
                        return Task.FromResult(false);
                    }
                }

                switch (dbSrouce.Item1)
                {
                    case DBType.Oracle:
                        break;
                    case DBType.OracleNoClient:
                        break;
                    case DBType.SQLServer:
                        break;
                    case DBType.PostGreSql://PG库
                        PostgresInitTable(dbSrouce.Item2, tableList, dataSourceId);
                        break;
                    case DBType.MySql:
                        break;
                    case DBType.DB2:
                        break;
                    case DBType.SQLite:
                        break;
                    default:
                        break;
                }
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="dataSourceId">数据源id</param>
        /// <returns></returns>
        private Tuple<DBType, string> GetConnectionStr(string dataSourceId)
        {
            sys_database model = null;
            using (var db = new DbContext())
            {
                //查询同步的数据源
                model = db.Select("sys_database")
                        .Columns("*")
                        .Where("id", dataSourceId)
                        .GetModel<sys_database>();
            }//end using

            if (model != null)
            {
                string conStr = string.Empty;
                switch (model.dbtype)
                {
                    case 1://Oracle
                        conStr = $"Data Source={model.serverip}:{model.port}/{model.instance};User Id={model.user};Password={model.password};Pooling=true;Connection Timeout=60;";
                        return new Tuple<DBType, string>(DBType.OracleNoClient, conStr);
                    case 2://PG
                        conStr = $"Server={model.serverip};Port={model.port};Database={model.instance};User Id={model.user};Password={model.password};";
                        return new Tuple<DBType, string>(DBType.PostGreSql, conStr);
                    case 3://SqlServer
                        conStr = $"Server={model.serverip},{model.port};Database={model.instance};User Id={model.user};Password={model.password};";
                        return new Tuple<DBType, string>(DBType.SQLServer, conStr);
                    case 4://MySQL,default port :3306
                        conStr = $"Server={model.serverip};Port={model.port};Database={model.instance};Uid={model.user};Pwd={model.password};";
                        return new Tuple<DBType, string>(DBType.MySql, conStr);
                    default:
                        break;
                }
            }

            return null;
        }


        #region PG库同步
        //PG数据库同步
        private void PostgresInitTable(string conStr, List<sys_sync_table> tables, string dataSourceId)
        {
            using (var db = new DbContext(conStr, DBType.PostGreSql))
            {//连接到指定同步目标库上，开始检查表
                using (var updb = new DbContext())
                {
                    tables?.ForEach(table =>
                    {
                        try
                        {
                            //检查表是否在同步，如果同步则退出，未同步则可以同步
                            var r = updb.Select("sys_sync_table")
                                        .Columns("1")
                                        .Where("database_id", dataSourceId)
                                        .Where("table_name", table.table_name)
                                        .Where("is_sync", 1)
                                        .Exists();
                            if (!r)
                            {//没有同步，才可以继续同步
                                var columns = new List<string>();

                                //将状态设置为更同步中
                                updb.Update("sys_sync_table")
                                    .Column("is_sync", 1)
                                    .Where("database_id", dataSourceId)
                                    .Where("table_name", table.table_name)
                                    .Execute();

                                //从UP平台查询出原始表的结构
                                var createTableSql = GetPGCreateTableSql(updb, table.table_name, columns);

                                //创建生成表
                                db.Sql(createTableSql).Execute();

                                //查询up表中数据量，并向业务库同步（异步任务？）
                                var total = updb.Select($"{MODULE_NAME}.{table.table_name}")
                                                .Columns("count(1)")
                                                .Select<int>();

                                if (total > 1000)
                                {//如果数据大于1000行则使用异步同步
                                    Logger.Instance.Info("数据量超过1000行使用异步进行数据传输");

                                    //异步调用
                                    SyncPGBigTableData(columns, table.table_name, conStr, total);
                                }
                                else
                                {//小于1000，则直接同步调用
                                    var dt = updb.Select($"{MODULE_NAME}.{table.table_name}")
                                              .Columns("*")
                                              .Select();

                                    //执行数据表数据插入
                                    var cnt = InsertPGTableValue(db, columns, dt, table.table_name);
                                    if (total == cnt)
                                    {//更新进度
                                     //本地记录日志
                                        Logger.Instance.Info($"同步表：{table.table_name},{cnt}行");
                                        //UpdateProcess(updb, table.table_name,dataSourceId, total, cnt);
                                    }
                                    else
                                    {//部分同步失败记录
                                        Logger.Instance.Warn($"同步表:{table.table_name},{cnt}行,实际行:{total}");
                                    }
                                }//end if 检查是否大于 1000行数据

                                //完成数据插入，更新状态，设置为完成同步
                                updb.Update("sys_sync_table")
                                    .Column("is_sync", 0)
                                    .Where("database_id", dataSourceId)
                                    .Where("table_name", table.table_name)
                                    .Execute();
                            }//end if 是否正在同步
                        }//end try
                        catch (Exception ex)
                        {
                            Logger.Instance.Error("初始化表异常", ex);
                        }
                    });
                }//end using
            }//end using
        }

        //根据表名称及列名，创建出建表脚本
        private string GetPGCreateTableSql(DbContext db, string tabName, List<string> columns)
        {
            //查询数据表结构的脚本
            var sql = db.GetSql("PG00002-数据表结构", null, null);

            //查询出表名称 
            var dt = db.Sql(sql)
                      .Parameters("tabName", tabName)
                      .Select();
            if (dt.HasData())
            {
                //生成脚本
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine();
                sqlBuilder.Append($"drop table if exists {MODULE_NAME}.{tabName};");
                sqlBuilder.AppendLine();
                sqlBuilder.Append($"create table {MODULE_NAME}.{tabName}(");

                var list = new List<CreateTabModel>();
                foreach (DataRow dataRow in dt.Rows)
                {
                    var colName = dataRow.GetValueByName<string>("列名");
                    var type = dataRow.GetValueByName<string>("类型");
                    var remark = dataRow.GetValueByName<string>("备注");
                    var notNull = dataRow.GetValueByName<string>("非空");

                    var notnullVal = (notNull == "t") ? "NOT NULL" : "";

                    CreateTabModel model = new CreateTabModel
                    {
                        ColumnName = colName,
                        ColumnType = type,
                        Remark = remark,
                        NotNull = notnullVal
                    };
                    //添加列表
                    list.Add(model);

                    //添加列
                    columns.Add(colName);

                    sqlBuilder.AppendLine();
                    sqlBuilder.Append($"     {colName} {type} {notnullVal},");
                }

                //查询主键
                sql = db.GetSql("PG00002-查询表主键", null, null);
                var pkdt = db.Sql(sql).Parameters("tabName", tabName).Select();
                if (pkdt.HasData())
                {//包括主键
                    var cname = pkdt.GetValueByName<string>("colname");
                    var pkname = pkdt.GetValueByName<string>("pk_name");
                    sqlBuilder.AppendLine();
                    sqlBuilder.Append($"     constraint {pkname} primary key ({cname})");
                }
                else
                {//如果没有主键则需要移除最后一位","号
                    sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
                }

                sqlBuilder.AppendLine();
                sqlBuilder.Append(");");

                //生成列注释
                list?.ForEach(c =>
                {
                    if (c.Remark.IsNotNullOrEmpty())
                    {
                        sqlBuilder.AppendLine();
                        sqlBuilder.Append($"comment on column {MODULE_NAME}.{tabName}.{c.ColumnName} is '{c.Remark}';");
                    }
                });

                return sqlBuilder.ToString();
            }

            return string.Empty;
        }

        //同步大数据的表使用PG库
        private Task<bool> SyncPGBigTableData(List<string> columns, string tabName, string conStr, int total)
        {
            string orderColumn = "id";
            var r = columns.Exists(c => c == "sno");
            if (r)
            {//如果存储排序列，则使用排序列，否则使用id进行排序
                orderColumn = "sno";
            }

            return Task.Run<bool>(() =>
            {
                //每次取100条记录，检查需要取多少次。
                var pageSize = Math.Ceiling(total / 100.0);
                //当前页面数量
                var currentPage = 1;

                using (var updb = new DbContext())
                {//基础平台库

                    using (var db = new DbContext(conStr, DBType.PostGreSql))
                    {//连接到业务PG库上

                        //每次取100条(循环取)
                        while (currentPage <= pageSize)
                        {//分页查询出源表记录，每次查询
                            var dt = updb.Select($"{MODULE_NAME}." + tabName)
                                 .Columns("*")
                                 .Paging(currentPage, 100)
                                 .OrderBy(orderColumn)    //按主键进行排序，否则会重复获取
                                 .Select();

                            //将数据插入到目标表中
                            this.InsertPGTableValue(db, columns, dt, tabName);

                            currentPage++;
                        }//end while
                    }//end using db
                }//end using updb

                return false;
            });
        }

        //向数据表中插入值
        private int InsertPGTableValue(DbContext db, List<string> columns, DataTable dt, string tabName)
        {
            //处理的总数量
            var total = 0;
            //查询源表有哪些列？然后根据列读取值并同步
            foreach (DataRow dr in dt.Rows)
            {//循环每行数据 
                var insert = db.Insert($"{MODULE_NAME}.{tabName}");
                columns?.ForEach(colName =>
                {//循环所有列 
                    var value = dr.GetValueByName(colName);

                    //对各列进行添加
                    insert.Column(colName, value);
                });

                //最后执行脚本
                var cnt = insert.Execute();

                total += cnt;
            }//结束循环，完成执行

            //返回处理的总数量
            return total;
        }

        #endregion
    }
}
