using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using UP.Basics;
using UP.Basics.Request;
using UP.Basics.Security;
using UP.Models.Apply;
using UP.Models.DB.Log;

namespace UP.Web
{
    /// <summary>
    /// 所有系统模块模块的基础控制器类
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasicsController : BaseController
    {
        /// <summary>
        /// 接口公共请求头信息
        /// </summary>
        protected ApiRequestHeader requestHeader = null;

        /// <summary>
        /// 接口请求名称，中文名
        /// </summary>
        protected string interfaceName = string.Empty;

        //接口请求开始时间
        private static DateTime begintime = DateTime.Now;

        //客户端请求 的IP地址
        private string ipaddress = string.Empty;

        //登录用户信息
        protected UserModel loginUser = null;

        //请求来源的应用信息
        protected UserAppLyInfo userAppLyInfo = null;

        /// <summary>
        /// 获取当前应用系统所有方法的集合
        /// </summary>
        /// <returns></returns>
        protected List<ActionDescModel> GetAllActions()
        {
            //待返回对象的集合（获取到的所有忆授权的方法）
            var list = new List<ActionDescModel>();
            var path = AppContext.BaseDirectory;
            var files = System.IO.Directory.GetFiles(path, "*.dll");
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);

                    //只处理从Controller派生过来的类，
                    var types = assembly.GetTypes()
                                        .Where(type => typeof(Controller).IsAssignableFrom(type) ||
                                                       typeof(ControllerBase).IsAssignableFrom(type));

                    foreach (var type in types)
                    {
                        //获取控制器名称
                        string controllerName = type.Name.Replace("Controller", "");
                        //所有成员的集合
                        var members = type.GetMembers();
                        foreach (var member in members)
                        {
                            if (member.MemberType != MemberTypes.Method)
                            {
                                // 不是方法时，跳过
                                continue;
                            }
                            var postAttr = member.GetCustomAttribute<HttpPostAttribute>();
                            var getAttr = member.GetCustomAttribute<HttpGetAttribute>();
                            var deleteAttr = member.GetCustomAttribute<HttpDeleteAttribute>();
                            var putAttr = member.GetCustomAttribute<HttpPutAttribute>();
                            var ripAuth = member.GetCustomAttribute<RIPAuthorityAttribute>();
                            //只检查这4种属性的接口方法
                            if (postAttr != null ||
                                getAttr != null ||
                                deleteAttr != null ||
                                putAttr != null)
                            {
                                var model = new ActionDescModel
                                {
                                    ControllerName = controllerName,//控制器名称
                                    ActionName = member.Name,//接口方法名称
                                    FullClassName = type.FullName,
                                    MethodCNName = ripAuth?.MethodName,
                                    ActionDescription = ripAuth?.Description,
                                    Author = ripAuth?.Author,
                                    UpdateTime = ripAuth?.UpdateTime
                                };
                                list.Add(model);
                            }
                        }
                    }
                }
                catch
                {
                    //加载程序集异常（因为可能有的dll不是c#或net框架的，只需要加载），这里不用写入日志及记录异常
                }
            }
            return list;
        }

        /// <summary>
        /// 复写父类的该方法。执行控制器中的方法之前先执行该方法。从而实现过滤的功能。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //调用父类的该方法。
            base.OnActionExecuting(filterContext);
            begintime = DateTime.Now;
            requestHeader = new ApiRequestHeader();
            var headers = Request.Headers;

            //获取nginx代理配置的客户ip信息 ---nginx必须配置 proxy set header X-Real-IP $remote addr
            ipaddress = filterContext.HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (ipaddress.IsNullOrEmpty())
            {
                //获取IP地址
                ipaddress = filterContext.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            //获取登录用户信息
            loginUser = this.GetUserInfo();
            //获取请求来源信息
            userAppLyInfo = this.GetUserAppLyInfo();
        }

        /// <summary>
        /// 日志监控
        /// </summary>
        /// <param name="inparm">w未加密入参</param>
        /// <param name="outparm">输出参数</param>
        protected void ApiLog_Moni(object inparm = null, ResponseModel outparm = null)
        {
            //请求地址
            string request_address = Request.Scheme + "://" + Request.Host.Value + Request.Path;
            //接口地址
            string inte_address = Request.Path;
            //获取客户端请求IP
            string ip = ipaddress;
            //请求端口
            string port = Request.Host.Port.ToString();
            //接口名
            string inte_name = Request.Path;
            try
            {
                var inparmstr = "";
                if (inparm != null)
                {
                    inparmstr = inparm.ToJson();
                }
                var outparmstr = "";
                if (outparm != null)
                {
                    outparmstr = outparm.ToJson();
                }
                //用户类型
                int usertype = 3;
                if (loginUser != null)
                {
                    usertype = loginUser.usertype;
                }
                //请求耗时 毫秒
                decimal ts = Convert.ToDecimal((DateTime.Now - begintime).TotalMilliseconds);
                //文件上传不记录上传内容
                AppOperationLog logInt = new AppOperationLog()
                {
                    创建时间 = DateTime.Now,
                    失败原因 = outparm.msg,
                    数据标识 = 1,
                    来源方式 = 1,
                    设备id = requestHeader.deviceid,
                    请求ip = ip,
                    请求参数 = inparmstr,
                    请求地址 = inte_name,
                    请求接口 = inte_name,
                    请求时间 = DateTime.Now,
                    请求模块 = inte_name,
                    请求状态 = outparm.code,
                    身份类型 = usertype,
                    输出信息 = outparmstr,
                    请求耗时 = ts
                };
                Logger.Instance.Info("请求信息》\r\n" + logInt.ToJson());
                this.Add<AppOperationLog>(logInt).Execute();
                // PApi.LogInset(logInt);
                logInt = null;//释放资源
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("记录接口请求日志发生异常", ex);
            }
        }

        /// <summary>
        /// 统一进行接口返回输出
        /// </summary>
        /// <param name="item">接口返回对象</param>
        /// <returns></returns>
        protected virtual JsonResult Json(ResponseModel item)
        {
            //获取服务器时间戳
            item.timestamp = Tools.ConvertDateTimeToInt(DateTime.Now);
            //记录日志
            ApiLog_Moni(Request.Headers, item);
            //判断接口请求参数是否加密
            var endpoint = GetEndpoint(HttpContext);
            //如果URL与操作不匹配，则端点为null
            if (endpoint != null)
            {
                //获取方法的自定义特性
                var enAttribute = endpoint.Metadata.GetMetadata<UPEncryptionAttribute>();
                //默认输入输出都需要加密
                if (enAttribute == null)
                    enAttribute = new UPEncryptionAttribute();
                //输出参数需要加密
                if (enAttribute.IsOutEncryption && item.data != null)
                {
                    string dataJson = Strings.ObjectToJson(item.data);
                    //输出数据进行AES加密
                    item.data = HttpUtility.UrlEncode(AESEncryptWeb.EncryptByAES(dataJson, item.timestamp.ToString()));
                }
            }
            var jsonResult = new JsonResult(item);
            jsonResult.StatusCode = 200;
            jsonResult.ContentType = "application/json";
            return jsonResult;
        }

        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static Endpoint GetEndpoint(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Features.Get<IEndpointFeature>()?.Endpoint;
        }
    }
}