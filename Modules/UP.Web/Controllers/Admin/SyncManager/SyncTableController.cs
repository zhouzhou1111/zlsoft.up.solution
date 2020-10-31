/*
 * why:2020-09-10
 * 实现数据表同步的webapi接口
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.Sync;

namespace UP.Web.Controllers.Admin.SyncManager
{
    /// <summary>
    /// 数据表同步控制器
    /// why:2020-09-10
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class SyncTableController : BasicsController
    {
        private ISyncTable _syncTable;
        public SyncTableController()
        {
            _syncTable = this.GetInstance<ISyncTable>();
        }

        /// <summary>
        /// 初始化表同步
        /// </summary>
        /// <returns>返回同步情况</returns>
        [HttpPost]
        [UPEncryption("", false, false)]
        [AllowAnonymous]
        public async Task<IActionResult> InitTable(string sourceid)
        { 
            var r = await _syncTable.InitTable(sourceid);

            var resModel = new ResponseModel(ResponseCode.Success, "开始同步");
            return Json(resModel);
        }

        /// <summary>
        /// 获取同步表的信息
        /// 由于同步表是持续执行的，所以通过该方法获取指定数据源同步表情况
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetSyncTableInfo(int sourceid)
        {
            var resModel = new ResponseModel(ResponseCode.Success, "查询完成");
            return Json(resModel);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TestAsync()
        { 
            //var r = await isystable.InitTable(51);
            return Ok("ok");
        }

        /// <summary>
        /// 获取所有基础目录表
        /// </summary>
        /// <returns>返回基础目录表</returns>
        [HttpPost]
        [UPEncryption("", false, false)]
        [AllowAnonymous]
        public async Task<IActionResult> GetBaseTables()
        {
            var list = await _syncTable.GetBaseTables();
            var resModel = new ResponseModel(ResponseCode.Success, "查询完成", list);
            return Json(resModel);
        }
    }
}