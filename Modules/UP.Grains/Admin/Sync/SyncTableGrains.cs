/*
 * why:2020-09-10
 * 数据表同步的slio层
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Sync;
using UP.Logics.Admin.Sync;

namespace UP.Grains.Admin.Sync
{
    public class SyncTableGrains : BasicGrains<SyncTableLogic>, ISyncTable
    {
        /// <summary>
        /// 获取基础目录表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetBaseTables()
        {
            return await this.Logic.GetBaseTables();
        }

        /// <summary>
        /// 初始化表数据
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <returns></returns>
        public async Task<bool> InitTable(string dataSourceId)
        {
            return await this.Logic.InitAllTable(dataSourceId);
        }
    }
}
