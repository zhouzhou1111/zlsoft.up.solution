using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using UP.Basics;
using UP.Interface.Admin.Sync;

namespace UP.Web.Controllers.Admin.SyncManager
{

    /// <summary>
    /// 测试控制器
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class TestController : BasicsController
    {
        private ITest _test;
        public TestController()
        {
            _test = this.GetInstance<ITest>();
        }

        /// <summary>
        /// 根据id获取数据模型
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回数据模型</returns>
        [HttpPost]
        [UPEncryption("", false, false)]
        [AllowAnonymous]
        public IActionResult GetModel(string id, string name, int sex, DateTime birthday)
        {
            var model = _test.GetModelById(id).Result;

            var resModel = new ResponseModel(ResponseCode.Success, "完成查询", model);
            return Json(resModel);
        }


    }
}
