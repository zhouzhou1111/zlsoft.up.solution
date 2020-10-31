/*********************************************************
* 功能：API基类控制器
* 描述：
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/
using Microsoft.AspNetCore.Mvc;

namespace UP.Basics
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : BaseController
    {

    }
}
