using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QWPlatform.SystemLibrary.Utils;

namespace UP.Basics.Plugins.ZlsoftFtp
{
    /// <summary>
    /// ftp请求结果
    /// </summary>
    public class FtpAjaxResult
    {
        private string _errorMessage;
        private decimal _integral;
        private object _data;

        public FtpAjaxResult(AjaxResultType code, string msg)
        {
            this.code = code;
            this.errormessage = msg;
        }

        public FtpAjaxResult()
        {
            //设置默认值
            code = AjaxResultType.其他异常;
        }

        /// <summary>
        /// Ajax请求返回结果类型
        /// </summary>
        //[DataMember(Name = "code")]
        [JsonProperty("code")]
        public AjaxResultType code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        //[DataMember(Name = "errormessage")]
        [JsonProperty("errormessage")]
        public string errormessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                //转义成json字符串格式，防止解析出错
                //_errorMessage = JsonHelper.ToJsonString(value);
                _errorMessage = value;
            }
        }

        /// <summary>
        /// 返回的自定义数据
        /// </summary>
        [JsonProperty("data")]
        public object data
        {
            get
            {
                return _data;
            }
            set
            {
                //转义成json字符串格式，防止解析出错
                //_data = JsonHelper.ToJsonString(value);
                // _data = "<" + value + ">";
                _data = value;
            }
        }

        /// <summary>
        /// 服务器时间戳，东8区，精度毫秒  返回参数的AES加密种子
        /// </summary>
        //[DataMember(Name = "timestamp")]
        [JsonProperty("timestamp")]
        public long timestamp { get; set; }

        /// <summary>
        /// 将当前对象序列化为json字符串，用于前端解析。
        /// </summary>
        /// <returns></returns>
        public string CreateResultString()
        {
            //失败状态没有错误信息时，把枚举类型作为错误信息返回
            if (this.code != AjaxResultType.成功 && this.errormessage.IsNullOrEmpty())
            {
                this.errormessage = this.code.ToString();
            }
            var rs = Strings.ObjectToJson(this);
            /*//去除换行符
            if (!rs.IsNullOrEmpty()) {
                rs.Replace("\r\n","");
            }*/
            this.data = null;
            this.errormessage = null;//释放接口资源
            return rs;
        }

        /// <summary>
        /// Ajax请求返回结果类型。
        ///成功状态只有一个（100）。失败类型的值从200开始递增。其他的自定义异常在这个枚举中维护
        /// </summary>
        public enum AjaxResultType
        {
            成功 = 100,
            失败 = 200,
            登录超时 = 201,
            数据库操作异常 = 202,
            请求缺少参数 = 203,
            无查询结果 = 204,
            参数异常 = 205,

            /// <summary>
            /// 一般用于一个页面处理多种业务的情况
            /// </summary>
            缺少tag操作参数 = 206,

            内部异常 = 207,
            逻辑异常 = 208,
            token验证失败 = 300,
            其他异常 = 900
        }

    }
}