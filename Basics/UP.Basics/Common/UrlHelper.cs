/****************************************************
* 功能：URL工具类
* 描述：
* 作者：贺伟
* 日期：2020/05/20
*********************************************************/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace UP.Basics
{

    /// <summary>
    /// URL工具类
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// 将url参数字符串（a=b&amp;c=d 格式）解析成NameValueCollection对象。
        /// 方法内部对参数字符串进行url解码，前端将表单序列化后的参数提交即可
        /// </summary>
        /// <param name="_queryString">url参数字符串</param>
        /// <returns>NameValueCollection对象</returns>
        public static NameValueCollection ParseUrlParameter(string _queryString)
        {
            if (_queryString.IsNullOrEmpty())
            {
                return null;
            }
            //先拆分再解析，否则值里的"&"和"="符号会影响解析结果
            NameValueCollection c = new NameValueCollection();
            string[] s1 = _queryString.Split('&');
            string[] tempArray = null;
            foreach (var i in s1)
            {
                tempArray = i.Split('=');
                //解码后保存
                c.Add(HttpUtility.UrlDecode(tempArray[0].Trim()), HttpUtility.UrlDecode(tempArray[1].Trim()));
            }
            return c;
        }
        /// <summary>
        /// url构造参数转为Json请求体
        /// </summary>
        /// <param name="_queryString">url参数</param>
        /// <returns></returns>
        public static string ParameterToJsonString(string _queryString)
        {
            if (_queryString.IsNullOrEmpty())
            {
                return null;
            }
            //先拆分再解析，否则值里的"&"和"="符号会影响解析结果
            Dictionary<string, string> nameValue = new Dictionary<string, string>();
            string[] s1 = _queryString.Split('&');
            foreach (var i in s1)
            {
                string[] tempArray = i.Split('=');
                string keys = HttpUtility.UrlDecode(tempArray[0].Trim()).ToLower();
                //解码后保存
                nameValue.Add(keys, HttpUtility.UrlDecode(tempArray[1].Trim()));
            }
            var json = JsonConvert.SerializeObject(nameValue);
            return json;
        }

        /// <summary>
        /// 将url参数字符串（a=b&amp;c=d 格式）解析成NameValueCollection对象。
        /// </summary>
        /// <param name="s">http请求输入流</param>
        /// <returns>NameValueCollection对象</returns>
        public static NameValueCollection ParseUrlParameter(Stream s)
        {
            string pams = GetPostData(s);
            return ParseUrlParameter(pams);
        }
        /// <summary>
        /// 将url参数字符串（a=b&amp;c=d 格式）解析成NameValueCollection对象。
        /// 方法内部对参数字符串进行url解码，前端将表单序列化后的参数提交即可
        /// </summary>
        /// <param name="_queryString">url参数字符串</param>   
        /// <param name="splitSymbol">分隔符</param>
        /// <returns>NameValueCollection对象</returns>
        public static NameValueCollection ParseUrlParameter(string _queryString, string splitSymbol)
        {
            if (_queryString.IsNullOrEmpty())
            {
                return null;
            }
            //先拆分再解析，否则值里的"&"和"="符号会影响解析结果
            NameValueCollection c = new NameValueCollection();
            string[] s1 = _queryString.Split(splitSymbol.ToCharArray());
            string[] tempArray = null;
            foreach (var i in s1)
            {
                tempArray = i.Split('=');
                //解码后保存
                c.Add(HttpUtility.UrlDecode(tempArray[0].Trim()), HttpUtility.UrlDecode(tempArray[1].Trim()));
            }
            return c;
        }
        /// <summary>
        /// 从请求输入流中读取post参数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string GetPostData(Stream s)
        {
            string pams = string.Empty;
            using (StreamReader sr = new StreamReader(s))
            {
                pams = sr.ReadToEnd();
            }
            return pams;
        }
    }
}
