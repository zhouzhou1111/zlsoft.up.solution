﻿using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace UP.Basics.Common
{
    /// <summary>
    /// Http请求信息帮助
    /// 内部调用，由管理类提交封装
    /// </summary>
    public class WebServiceHttpHelper
    {
        private static WebServiceHttpHelper m_Helper;
        /// <summary>
        /// 单例
        /// </summary>
        public static WebServiceHttpHelper Helper
        {
            get { return m_Helper ?? (m_Helper = new WebServiceHttpHelper()); }
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        /// <param name="strUrl">请求地址</param>
        /// <param name="requestMode">请求方式</param>
        /// <param name="parameters">参数</param>
        /// <param name="requestCoding">请求编码</param>
        /// <param name="responseCoding">响应编码</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <returns>返回：请求成功响应信息，失败返回null</returns>
        public static string GetResponseString(string strUrl, HttpMethod requestMode, Dictionary<string, string> parameters, Encoding requestCoding, Encoding responseCoding, int timeout = 300*10)
        {
            string url = VerifyUrl(strUrl);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

            HttpWebResponse webResponse = null;
            switch (requestMode)
            {
                case HttpMethod.Get:
                    webResponse = GetRequest(webRequest, timeout);
                    break;
                case HttpMethod.Post:
                    webResponse = PostRequest(webRequest, parameters, timeout, requestCoding);
                    break;
            }

            if (webResponse != null && webResponse.StatusCode == HttpStatusCode.OK)
            {
                using (Stream newStream = webResponse.GetResponseStream())
                {
                    if (newStream != null)
                        using (StreamReader reader = new StreamReader(newStream, responseCoding))
                        {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                }
            }
            return null;
        }


        /// <summary>
        /// get 请求指定地址返回响应数据
        /// </summary>
        /// <param name="webRequest">请求</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <returns>返回：响应信息</returns>
        private static HttpWebResponse GetRequest(HttpWebRequest webRequest, int timeout)
        {
            try
            {
                webRequest.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
                webRequest.Headers.Add("Accept-Language", "zh-cn,en-US,en;q=0.5");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.UserAgent = "DefaultUserAgent";
                webRequest.Timeout = timeout;
                webRequest.Method = "GET";

                // 接收返回信息
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"调用远程地址失败,url:{webRequest.RequestUri.ToString()}", ex);
                return null;
            }
        }


        /// <summary>
        /// post 请求指定地址返回响应数据
        /// </summary>
        /// <param name="webRequest">请求</param>
        /// <param name="parameters">传入参数</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <param name="requestCoding">请求编码</param>
        /// <returns>返回：响应信息</returns>
        private static HttpWebResponse PostRequest(HttpWebRequest webRequest, Dictionary<string, string> parameters, int timeout, Encoding requestCoding)
        {
            try
            {
                // 拼接参数
                string postStr = string.Empty;
                if (parameters != null)
                {
                    parameters.All(o =>
                    {
                        if (string.IsNullOrEmpty(postStr))
                            postStr = string.Format("{0}={1}", o.Key, HttpUtility.UrlEncode(o.Value));
                        else
                            postStr += string.Format("&{0}={1}", o.Key, HttpUtility.UrlEncode(o.Value));

                        return true;
                    });
                }
                byte[] byteArray = requestCoding.GetBytes(postStr);
                webRequest.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
                webRequest.Headers.Add("Accept-Language", "zh-cn,en-US,en;q=0.5");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.UserAgent = "DefaultUserAgent";
                webRequest.Timeout = timeout;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Method = "POST";

                // 将参数写入流
                using (Stream newStream = webRequest.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Close();
                }

                // 接收返回信息
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"调用远程地址失败,url:{webRequest.RequestUri.ToString()}", ex);
                return null;
            }
        }


        /// <summary>
        /// 验证URL
        /// </summary>
        /// <param name="url">待验证 URL</param>
        /// <returns></returns>
        private static string VerifyUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL 地址不可以为空！");

            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                return url;
            if (url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                return url;
            return string.Format("http://{0}", url);
        }
    }

}
