using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using QWPlatform.SystemLibrary.LogManager;
using QWPlatform.SystemLibrary.Utils;

namespace UP.Logger.Service
{
    [QWPlatform.Models.Table(tabName: "logger.sys_log")]
    public class LoggerModel
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        public int 系统分类 { get; set; }

        /// <summary>
        /// UP平台Redies Key
        /// </summary>
        public string 接口关键字 { get; set; }

        /// <summary>
        /// 日志关键字
        /// </summary>
        public string 日志关键字 { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string 日志内容 { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public int 日志等级 { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string ip地址 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime 记录时间 { get; set; }

        /// <summary>
        /// 异常对象Json字符串
        /// </summary>
        public string 异常信息 { get; set; }

        /// <summary>
        /// 转换日志消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static LoggerModel ConvertLoggerMessage(string message)
        {
            try
            {
                var model = Strings.JsonToModel<UpLoggerInfo>(message);
                var result = new LoggerModel
                {
                    id = model.Id,
                    接口关键字 = model.ApiId,
                    日志关键字 = model.Key,
                    系统分类 = model.System.ToInt32(),
                    日志内容 = model.Message,
                    ip地址 = model.FromIP,
                    日志等级 = model.Level.ToInt32(),
                    记录时间 = model.LogDateTime,
                    异常信息 = model.Ex != null ? Strings.ObjectToJson(model.Ex) : string.Empty
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}