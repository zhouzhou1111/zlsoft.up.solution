/****************************************************
* 功能：接口公共请求头信息处理类
* 描述：
* 作者：贺伟
* 日期：2020/05/20
*********************************************************/

using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace UP.Basics.Request
{
    /// <summary>
    /// 接口公共请求头信息
    /// </summary>
    public class ApiRequestHeader
    {
        /// <summary>
        /// 应用id 必填，放入request的header中
        /// </summary>
        //[RequiredAttribute(36, "应用id不能为空")]
        public string appid { get; set; }

        /// <summary>
        /// 请求时间戳，东8区，精度毫秒 必填，放入request的header中
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 请求签名(AES的加密种子，由RSA加密请求时间戳生成) 必填，放入request的header中
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 登录返回的身份令牌，放入request的header中
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 设备id，移动端请求传入设备id，pc传入mac
        /// </summary>
        public string deviceid { get; set; }

        /// <summary>
        /// 请求参数的集合，最大长度不限，除公共参数外所有请求参数进行AES加密(附件上传不进行加密)
        /// </summary>
        public string biz_content { get; set; }

        /// <summary>
        /// 请求头验证状态
        /// </summary>
        public bool initi_status { get; set; }

        /// <summary>
        /// 所有传入的参数集合。部分传入参数经过了加密，需要经过解密或其他处理后才放入这个集合中读取。
        /// 用于取代原有的JObject[]这种写法 。
        /// </summary>
        public NameValueCollection all_parameters { get; set; }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ApiRequestHeader() { }

        /// <summary>
        /// 获取请求的消息头信息
        /// </summary>
        /// <param name="context">上下文请求信息</param>
        public ApiRequestHeader(HttpContext context, bool isEn = true)
        {
            var headers = context.Request.Headers;
            //从http请求的头里面获取应用信息
            appid = Tools.GetHeaderValue(headers, "appid");
            //请求时间戳，东8区，精度毫秒，AES的加密种子
            timestamp = Tools.GetHeaderValue(headers, "timestamp");
            //设备id
            deviceid = Tools.GetHeaderValue(headers, "deviceid");
            if (isEn)
            {
                //请求内容
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    var bodyRead = reader.ReadToEndAsync();
                    biz_content = bodyRead.Result;  //把body赋值给bodyStr
                }
            }
            //登录token
            var tokenstr = Tools.GetHeaderValue(headers, "Authorization");
            if (!tokenstr.IsNullOrEmpty())
            {
                token = tokenstr.Replace("Bearer ", "").Trim();
            }
            //请求签名(RSA加密请求时间戳生成)
            sign = Tools.GetHeaderValue(headers, "sign");
            if (!sign.IsNullOrEmpty())
                sign = HttpUtility.UrlDecode(sign, System.Text.Encoding.UTF8);
        }
    }
}