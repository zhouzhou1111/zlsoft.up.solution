using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using QWPlatform.IService;

using UP.Models.Admin.Sync;

namespace UP.Logics.Admin.Sync
{
    /// <summary>
    /// 数据逻辑层
    /// </summary>
    public class TestLogic
    {
        /// <summary>
        /// 根据id获取到数据模型
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回结果</returns>
        public sys_database GetModelById(string id)
        {
            using (var db = new DbContext())
            {
                var model = db.Select("sys_database")
                            .Columns("*")
                            .Where("id", id)
                            .GetModel<sys_database>();

                //异步返回结果
                return model;
            }
        }


    }
}
