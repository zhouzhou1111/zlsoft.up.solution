using QWPlatform.SystemLibrary;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// 实现基础业务构造的功能器
    /// why2020-04-14
    /// </summary>
    public class BaseBusinessExecute<T> : IBusinessExecute<T> where T : class, new()
    {
        private ExecuteModel executeModel = new ExecuteModel();
        //连接orleans的客户端（静态化处理，只需要产生一个）
        private static OrleansClient _client = new OrleansClient();

        /// <summary>
        /// 增加Model
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IBusinessExecute<T> Add(T t)
        {
            executeModel.DatabaseAction = DBAction.Insert;
            executeModel.Entity = t;

            return this;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        public IBusinessExecute<T> Delete()
        {
            executeModel.DatabaseAction = DBAction.Delete;
            T t = new T();
            executeModel.Entity = t;
            return this;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IBusinessExecute<T> Update(T t)
        {
            executeModel.DatabaseAction = DBAction.Update;
            executeModel.Entity = t;
            return this;
        }

        /// <summary>
        /// 设置列集合
        /// </summary>
        /// <param name="columns">列集合</param>
        /// <returns></returns>
        public IBusinessExecute<T> Columns(params string[] columns)
        {
            executeModel.Columns = columns;
            return this;
        }

        /// <summary>
        /// 查询器
        /// </summary>
        /// <returns></returns>
        public IBusinessExecute<T> Select()
        {
            executeModel.DatabaseAction = DBAction.Select;
            return this;
        }

        /// <summary>
        /// 检查值是否存在
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            //创建一个实例对象
            var dbtable = this.CreateInstance();

            T t = new T();
            executeModel.Entity = t;

            //查询并返回结果
            var task = dbtable.Exists(executeModel);
            return task.Result;
        }

        /// <summary>
        /// 执行删除，或更新，或插入
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            //创建一个实例对象
            var dbtable = this.CreateInstance();
            Task<int> task = null;
            switch (this.executeModel.DatabaseAction)
            {
                case DBAction.Insert://插入了插入操作
                    task = dbtable.Insert(this.executeModel.Entity);
                    break;
                case DBAction.Update://执行更新操作
                    task = dbtable.Update(executeModel);
                    break;
                case DBAction.Delete://执行删除操作
                    task = dbtable.Delete(executeModel);
                    break;
            }

            //返回异步的结果
            return task.Result;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public IBusinessExecute<T> Order(string column)
        {
            //设置排序字段
            this.executeModel.OrderColumn = column;
            return this;
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <returns></returns>
        public T GetModel()
        {
            //创建一个实例对象
            var dbtable = this.CreateInstance();

            T t = new T();
            executeModel.Entity = t;
            var task = dbtable.GetModel(executeModel);
            var data = task.Result as byte[];
            if (data != null)
            {
                var dt = QWPlatform.SystemLibrary.Serialize.SerializeHelper.DeserializeBinary(data) as DataTable;
                if (dt != null)
                {
                    return dt.ToModel<T>();
                }
            }//end if

            return null;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<T> GetModelList()
        {
            //创建一个实例对象
            var dbtable = this.CreateInstance();

            T t = new T();
            executeModel.Entity = t;
            var task = dbtable.GetModelList(executeModel);
            var data = task.Result?.Value;

            if (data != null)
            {//如果查询到结果，需要转换为list
                var dt = QWPlatform.SystemLibrary.Serialize.SerializeHelper.DeserializeBinary(data) as DataTable;
                if (dt != null)
                {
                    return dt.ToList<T>();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListPageModel<T> GetPageList(int page, int size)
        {
            //创建一个实例对象
            var dbtable = this.CreateInstance();

            T t = new T();
            executeModel.Entity = t;
            executeModel.Page = page;
            executeModel.PageSize = size;

            var task = dbtable.GetModelPageList(executeModel);
            var data = task.Result?.Value;
            var total = task.Result.Total;

            if (data != null)
            {//如果查询到结果，需要转换为list
                var dt = QWPlatform.SystemLibrary.Serialize.SerializeHelper.DeserializeBinary(data) as DataTable;
                if (dt != null)
                {
                    return new ListPageModel<T>
                    {
                        Total = total,
                        PageList = dt.ToList<T>()
                    };
                }
            }

            return null;
        }


        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBusinessExecute<T> Where(string key, object value)
        {
            executeModel.Conditions.Add(new ConditionModel
            {//默认为相等的条件
                Key = key,
                Oper = Operational.Equal,
                Value = value,
                Where = WhereType.And
            });

            return this;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IBusinessExecute<T> Where(string key, Operational oper, object value)
        {
            executeModel.Conditions.Add(new ConditionModel
            {
                Key = key,
                Oper = oper,
                Value = value,
                Where = WhereType.And
            });

            return this;
        }

        /// <summary>
        /// 使用了Or条件查询或更新
        /// </summary>
        /// <param name="key">条件名称</param>
        /// <param name="oper">操作符方式</param>
        /// <param name="value">操作值</param>
        /// <returns></returns>
        public IBusinessExecute<T> OrWhere(string key, Operational oper, object value)
        {
            executeModel.Conditions.Add(new ConditionModel
            {
                Key = key,
                Oper = oper,
                Value = value,
                Where = WhereType.Or
            });

            return this;
        }


        /// <summary>
        /// 创建一个DBTable的实例
        /// </summary>
        /// <returns></returns>
        private IDBTable CreateInstance()
        {
            if (_client == null)
            {
                _client = new OrleansClient();
            }

            return _client.CreateInstance<IDBTable>().Result;
        }

    }
}
