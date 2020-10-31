using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UP.Basics;
using UP.Interface.Admin.Sync;
using UP.Interface.Drug;
using UP.Models.Drug;

namespace UP.Web.Controllers.drug
{
    [ApiGroup(ApiGroupNames.Admin)]
    public class DrugController : BasicsController
    {
        //实现接口
        private IDrug _drug;
        
        public DrugController()
        {
            _drug = this.GetInstance<IDrug>();
        }

        /// <summary>
        /// 获取药品列表    
        /// </summary>
        /// <param name="code">药品编码查找药品</param>
        /// <returns></returns>
        [HttpPost] 
        [UPEncryption("",false,false)]
        [AllowAnonymous]
       // [ProducesResponseType(typeof(LoginInfoModel), 200)]
         public IActionResult GetDrugList(string code)
        {
            //获取数据
            var result = new ResponseModel(ResponseCode.Error, "药品列表获取失败");
            var model = _drug.GetDrugList(code).Result;
            result.data = model;
            result.code = ResponseCode.Success.ToInt32();
            result.msg = "获取药品信息成功!";
            
            return Json(result);
        }

        /// <summary>
        /// 新增药品
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [UPEncryption("", false, false)]
        [AllowAnonymous]
        // [ProducesResponseType(typeof(LoginInfoModel), 200)]
        public IActionResult DrugAdd(DrugBasic drugBasic)
        {
           
            //获取数据
            var result = new ResponseModel(ResponseCode.Error, "药品列表获取失败");
            var model = _drug.DrugAdd(drugBasic).Result;
            result.data = model;
            result.code = ResponseCode.Success.ToInt32();
            result.msg = "获取药品信息成功!";

            return Json(result);
        }
    }
}
