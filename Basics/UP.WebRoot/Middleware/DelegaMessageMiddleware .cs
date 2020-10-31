/****************************************************
* 功能：参数加密解密中间件
* 描述：
* 作者：贺伟
* 日期：2020/05/21
*********************************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UP.Basics;
using UP.Basics.Request;
using UP.Basics.Security;
using UP.Models.Apply;
using XC.RSAUtil;

namespace UP.WebRoot
{
    /// <summary>
    /// 参数加密解密中间件
    /// </summary>
    public class RequestResponseMiddleware : BaseController
    {
        private readonly RequestDelegate _next;

        //定义一个私有成员变量，用于Lock
        private static object lockobj = new object();

        public RequestResponseMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var requestpath = context.Request.Path.Value.ToLower();
            //GET请求不进行校验
            if (context.Request.Method.ToUpper() == "GET")
            {
                return this._next(context);
            }
            //富文本上传文件不进行校验
            if (requestpath.IndexOf("imagefile/imageupload") >= 0)
            {
                return this._next(context);
            }
            //判断接口请求参数是否加密
            var endpoint = GetEndpoint(context);
            //如果URL与操作不匹配，则端点为null
            if (endpoint != null)
            {
                //获取方法的自定义特性
                var enAttribute = endpoint.Metadata.GetMetadata<UPEncryptionAttribute>();
                //默认输入输出都需要加密
                if (enAttribute == null)
                    enAttribute = new UPEncryptionAttribute();
                //输入参数不加密，则直接跳过解密方法
                if (enAttribute != null && !enAttribute.IsInEncryption)
                {
                    //获取头信息
                    var headerResultNo = GetNoEnRequestHeaderData(context);
                    //header校验失败
                    if (headerResultNo.code != ResponseCode.Success.ToInt32())
                    {
                        //返回验证失败
                        string responStr = QWPlatform.SystemLibrary.Utils.Strings.ObjectToJson(headerResultNo);
                        context.Response.WriteAsync(responStr);
                        return Task.CompletedTask;
                    }
                    else
                    {
                        return this._next(context);
                    }
                }
            }
            //获取头信息
            var headerResult = GetEnRequestHeaderData(context);
            //header校验失败
            if (headerResult.code != ResponseCode.Success.ToInt32())
            {
                //返回验证失败
                string responStr = QWPlatform.SystemLibrary.Utils.Strings.ObjectToJson(headerResult);
                context.Response.WriteAsync(responStr);
                return Task.CompletedTask;
            }
            if (headerResult.data != null)
            {
                var request = context.Request;
                //创建一个流
                var requestBodyStream = new MemoryStream();
                //转化为字节
                byte[] content1 = Encoding.UTF8.GetBytes(headerResult.data.ToString());
                //把修改写入流中
                requestBodyStream.Write(content1, 0, content1.Length);
                //把修改后的内容赋值给请求body
                request.Body = requestBodyStream;
                request.Body.Seek(0, SeekOrigin.Begin);
                request.ContentLength = content1.Length;
                request.ContentType = "application/json";
            }
            return this._next(context);
        }

        /// <summary>
        /// 从请求头信息中获取加密请求的消息信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ResponseModel GetEnRequestHeaderData(HttpContext context)
        {
            //请求头信息验证未通过
            var resultobj = new ResponseModel(ResponseCode.BadRequest, "头信息验证失败");
            //实例化接口请求头信息
            var requestHeader = new ApiRequestHeader(context);
            if (requestHeader == null)
            {
                return resultobj;
            }
            var msg = "";
            //判断必填参数
            var required_result = RequiredAttribute.ValidateRequired(requestHeader, ref msg);
            //必填验证通过
            if (!required_result)
            {
                resultobj.msg = msg + "当前验证无法通过";
                return resultobj;
            }
            //获取平台所有的应用信息
            var userinfo_key = "applyitems";
            var applyItems = CacheManager.Create().Get<List<AppLyInfo>>(userinfo_key);
            if (applyItems == null || !applyItems.Any())
            {
                //数据库获取应用信息
                applyItems = this.Query<AppLyInfo>()
                     .Where("数据标识", 1)
                     .GetModelList();
                if (applyItems != null && applyItems.Any())
                {
                    var obj = CacheManager.Create().Set(userinfo_key, applyItems);
                }
            }
            if (applyItems == null || !applyItems.Any())
            {
                resultobj.msg = msg + "没有找到平台的应用信息";
                return resultobj;
            }
            //获取请求的平台应用信息
            var applyInfo = applyItems.FirstOrDefault(t => t.APPID == requestHeader.appid);
            if (applyInfo == null)
            {
                resultobj.msg = requestHeader.appid + "非法！";
                return resultobj;
            }
            string decrypt_timestamp = "";
            try
            {
                var decryptData = GetRequestRsaData(applyInfo, requestHeader.sign);
                //解密失败
                if (decryptData.code != ResponseCode.Success.ToInt32())
                {
                    resultobj = decryptData;
                    return resultobj;
                }
                decrypt_timestamp = decryptData.data.ToString();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("RSA解密发生异常", ex);
                resultobj.msg = "请求参数签名不合法";
                return resultobj;
            }
            //签名参数校验不通过
            if (decrypt_timestamp != requestHeader.timestamp)
            {
                resultobj.msg = "请求参数签名不合法";
                return resultobj;
            }
            //请求验签成功
            resultobj.code = ResponseCode.Success.ToInt32();
            if (!requestHeader.biz_content.IsNullOrEmpty())
            {
                try
                {
                    //json参数需要解密
                    string bodyData = HttpUtility.UrlDecode(requestHeader.biz_content, System.Text.Encoding.UTF8);
                    //AES解密请求的参数信息,秘钥长度必须32位，"以0左补齐"
                    string bodystring = AESEncryptWeb.DecryptByAES(bodyData, requestHeader.timestamp);
                    string content = bodystring; //UrlHelper.ParameterToJsonString(bodystring);
                    resultobj.data = content;
                    Logger.Instance.Info("接口请求信息" + content);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error("参数异常", ex);
                    resultobj.code = ResponseCode.Error.ToInt32();
                    resultobj.msg = ex.Message;
                }
            }
            return resultobj;
        }

        /// <summary>
        /// 加密请求的信息进行RSA解密
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ResponseModel GetRequestRsaData(AppLyInfo applyInfo, string sign)
        {
            //请求头信息验证未通过
            var resultobj = new ResponseModel(ResponseCode.BadRequest, "RSA解密失败");
            string decrypt_timestamp = "";
            try
            {
                //校验签名,(RSA加密请求时间戳生成)
                //签名运算,ANS密钥对
                RSAUtilBase rSAUtil = new RsaPkcs8Util(Encoding.UTF8, applyInfo.公钥, applyInfo.私钥);
                //lock (lockobj)
                //{
                // RSAEncryptionPaddingMode.Pkcs1
                decrypt_timestamp = rSAUtil.Decrypt(sign, RSAEncryptionPadding.Pkcs1);
                //}
                //请求验签信息解密成功
                resultobj.code = ResponseCode.Success.ToInt32();
                resultobj.data = decrypt_timestamp;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("RSA解密发生异常", ex);
                resultobj.msg = "请求参数签名不合法";
            }
            return resultobj;
        }

        /// <summary>
        /// 从请求头信息中获取不解密加密请求的消息信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ResponseModel GetNoEnRequestHeaderData(HttpContext context)
        {
            //请求头信息验证未通过
            var resultobj = new ResponseModel(ResponseCode.BadRequest, "头信息验证失败");
            //实例化接口请求头信息
            var requestHeader = new ApiRequestHeader(context, false);
            if (requestHeader == null)
            {
                return resultobj;
            }
            var msg = "";
            //请求验签成功
            resultobj.code = ResponseCode.Success.ToInt32();
            resultobj.msg = "请求验证成功";
            //return resultobj;

            //判断必填参数
            var required_result = RequiredAttribute.ValidateRequired(requestHeader, ref msg);
            //必填验证通过
            if (!required_result)
            {
                resultobj.msg = msg + "当前验证无法通过";
                return resultobj;
            }
            //获取平台所有的应用信息
            var userinfo_key = "applyitems";
            var applyItems = CacheManager.Create().Get<List<AppLyInfo>>(userinfo_key);
            if (applyItems == null || !applyItems.Any())
            {
                //数据库获取应用信息
                applyItems = this.Query<AppLyInfo>()
                     .Where("数据标识", 1)
                     .GetModelList();
                if (applyItems != null && applyItems.Any())
                {
                    var obj = CacheManager.Create().Set(userinfo_key, applyItems);
                }
            }
            if (applyItems == null || !applyItems.Any())
            {
                resultobj.msg = msg + "没有找到平台的应用信息";
                return resultobj;
            }
            //获取请求的平台应用信息
            var applyInfo = applyItems.FirstOrDefault(t => t.APPID == requestHeader.appid);
            if (applyInfo == null)
            {
                resultobj.msg = requestHeader.appid + "非法！";
                return resultobj;
            }
            else
            {
                //缓存当前登录的应用信息
                UserAppLyInfo userAppLyInfo = new UserAppLyInfo()
                {
                    app_id = applyInfo.APPID,
                    app_name = applyInfo.应用名称,
                    private_key = applyInfo.私钥,
                    public_key = applyInfo.公钥,
                    sourse = applyInfo.使用平台
                };
                var userApplyinfo_key = "user_applyitem:" + requestHeader.appid;
                var obj = CacheManager.Create().Set(userApplyinfo_key, userAppLyInfo.ToJson());
                //请求验签成功
                resultobj.code = ResponseCode.Success.ToInt32();
                resultobj.msg = "请求验证成功";
            }
            return resultobj;
        }

        /// <summary>
        /// 从请求头信息中验证外部接口调用验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ResponseModel GetExternalHeaderData(HttpContext context)
        {
            //请求头信息验证未通过
            var resultobj = new ResponseModel(ResponseCode.BadRequest, "头信息验证失败");
            //实例化接口请求头信息
            var requestHeader = new ApiRequestHeader(context);
            if (requestHeader == null)
            {
                return resultobj;
            }
            var msg = "";
            //获取平台所有的应用信息
            var userinfo_key = "applyitems";
            var applyItems = CacheManager.Create().Get<List<AppLyInfo>>(userinfo_key);
            if (applyItems == null || !applyItems.Any())
            {
                //数据库获取应用信息
                applyItems = this.Query<AppLyInfo>()
                     .Where("数据标识", 1)
                     .GetModelList();
                if (applyItems != null && applyItems.Any())
                {
                    var obj = CacheManager.Create().Set(userinfo_key, applyItems);
                }
            }
            if (applyItems == null || !applyItems.Any())
            {
                resultobj.msg = msg + "没有找到平台的应用信息";
                return resultobj;
            }
            //获取请求的平台应用信息
            var applyInfo = applyItems.FirstOrDefault(t => t.APPID == requestHeader.appid);
            if (applyInfo == null)
            {
                resultobj.msg = requestHeader.appid + "非法！";
                return resultobj;
            }
            //请求验证成功
            resultobj.code = ResponseCode.Success.ToInt32();
            return resultobj;
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

    public static class DelegaMessageMiddleBuilderExtension
    {
        /// <summary>
        /// api接口请求参数加解密-中间件
        /// </summary>
        /// <param name="app"></param>
        public static void DelegaMessageHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseMiddleware>();
        }
    }
}