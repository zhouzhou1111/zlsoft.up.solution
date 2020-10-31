using System;

namespace UP.Basics
{
    /// <summary>
    /// 日志模型
    /// </summary>
    [Serializable]
    public class DBLoggerModel
    {
        /// <summary>
        /// 调用时间
        /// </summary>
        public DateTime CallTime { get; set; }

        /// <summary>
        /// 调用账号
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 调用请求IP地址
        /// </summary>
        public string RequestIP { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 调用方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 开消时间（ms）
        /// </summary>
        public long SpendTime { get; set; }

        /// <summary>
        /// 请求的参数
        /// </summary>
        public string RequestParameters { get; set; }

        /// <summary>
        /// 响应的参数
        /// </summary>
        public string ResponseParameters { get; set; }
    }
}
