/*
 * why:2020-09-10
 * 数据同步接口层
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;

namespace UP.Interface.Admin.Sync
{
    /// <summary>
    /// 同步表接口
    /// </summary>
    public interface ISyncTable : IBasic
    {
        /// <summary>
        /// 初始化同步表
        /// </summary>
        /// <param name="dataSourceId">数据源id</param>
        /// <returns></returns>
        Task<bool> InitTable(string dataSourceId);

        /// <summary>
        /// 获取基础目录表
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetBaseTables();
    }
}
