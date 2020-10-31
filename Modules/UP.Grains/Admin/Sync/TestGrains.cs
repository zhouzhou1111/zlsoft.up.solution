using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using UP.Basics;
using UP.Interface.Admin.Sync;
using UP.Logics.Admin.Sync;
using UP.Models.Admin.Sync;

namespace UP.Grains.Admin.Sync
{
    public class TestGrains : BasicGrains<TestLogic>, ITest
    {
        public Task<bool> AddModel(sys_database model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 实现通id获取模型
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回结果</returns>
        public Task<sys_database> GetModelById(string id)
        {
            var model = this.Logic.GetModelById(id);
            return Task.FromResult(model);
        }

        public Task<bool> StopDrug(string id)
        {
            throw new NotImplementedException();
        }
    }
}
