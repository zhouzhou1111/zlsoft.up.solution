/********************************************************
* 功能：响应的标准数据模型model 
* 描述：响应的标准数据模型model 
* 作者：贺伟
* 日期：2020-06-13
*********************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Basics.Models
{
    /// <summary>
    /// 返回消息
    /// </summary>
    public class JsonMsg<T> where T : class
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public ResponseCode code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 服务器响应时间戳
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 数据获取成功
        /// </summary>
        /// <param name="obj">数据集信息</param>
        /// <param name="msg">返回消息说明</param>
        /// <param name="timespan">时间戳</param>
        /// <returns></returns>
        public static JsonMsg<T> OK(T obj, string msg = "成功", long timespan = 0)
        {
            return new JsonMsg<T>() { code = ResponseCode.Success, msg = msg, data = obj, timestamp = timespan };
        }
        /// <summary>
        /// 数据获取失败
        /// </summary>
        /// <param name="obj">数据集信息</param>
        /// <param name="msg">返回消息说明</param>
        /// <param name="timespan">时间戳</param>
        /// <returns></returns>
        public static JsonMsg<T> Error(T obj, string msg = "失败", long timespan = 0)
        {
            return new JsonMsg<T>() { code = ResponseCode.Error, msg = msg, data = obj, timestamp = timespan };
        }
    }
}
