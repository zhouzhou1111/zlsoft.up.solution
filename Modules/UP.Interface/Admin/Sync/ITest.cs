using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualBasic.CompilerServices;

using UP.Basics;
using UP.Models.Admin.Sync;

namespace UP.Interface.Admin.Sync
{
    /// <summary>
    /// 测试接口
    /// </summary>
    public interface ITest : IBasic
    {
        /// <summary>
        /// 根据id获取数据模型
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<sys_database> GetModelById(string id);

        /// <summary>
        /// 实现数据增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddModel(sys_database model);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> StopDrug(string id);
    }
}
