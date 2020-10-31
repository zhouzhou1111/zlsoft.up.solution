/****************************************************
* 功能：公共接口服务
* 描述：
* 作者：贺伟
* 日期：2020/06/18
*********************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Collections.Generic;
using UP.Basics;
using UP.Interface.Api;
using UP.Models.Api;
using UP.Web.Models.Public;

namespace UP.Web.Controllers.Api
{
    /// <summary>
    ///公共接口服务
    /// </summary>
    [ApiGroup(ApiGroupNames.PUBLIC)]
    public class PublicController : BasicsController
    {
        /// <summary>
        ///查询字典信息列表
        /// </summary>
        /// <param name="param">字典表类型: b_民族 = 1, b_社会性别 = 2, b_学历 = 3, b_职务 = 4</param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        [RIPAuthority("查询字典信息列表", "字典表类型: b_民族 = 1, b_社会性别 = 2, b_学历 = 3, b_职务 = 4", "贺伟", "2020-06-18")]
        //[ProducesResponseType(typeof(List<DicInfo>), 200)]
        public IActionResult GetDicItems(ServerDicParam param)
        {
            //获取客户端接口
            var publicLogic = this.GetInstance<IPublic>();
            //请求参数转换成业务逻辑需要的请求参数对象
            var rparam = param.CopyModel<DicParam>();
            //查询数据
            var resultMsg = publicLogic.GetDicItems(rparam)?.Result;
            var result = new ResponseModel(resultMsg.code, resultMsg.msg, resultMsg.data);
            return Json(result);
        }
        
        /// <summary>
        /// 获取数据库连接字符串，加密后的串
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetDbConstr()
        {
            return Ok("加密后的数据库连接串");
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetDbTime()
        {
            return Ok(DateTime.Now);
        }
    }
}