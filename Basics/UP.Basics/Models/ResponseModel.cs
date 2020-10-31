/********************************************************
* 功能：响应的标准model 
* 描述：响应的标准model
* 作者：王海洋
* 日期：2020-01-21
*********************************************************/

namespace UP.Basics
{
    /// <summary>
    /// 响应的标准model
    /// </summary>
    public class ResponseModel
    {

        /// <summary>
        /// 结果代码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 服务器响应时间戳
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 向客户端响应数据请求模型
        /// </summary>
        /// <param name="code">响应的枚举代码</param>
        /// <param name="msg">响应的消息</param>
        /// <param name="data">响应的数据对象</param>
        public ResponseModel(ResponseCode code, string msg, object data = null, long timespan = 0)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.timestamp = timespan;
        }

        /// <summary>
        /// 向客户端响应数据请求模型
        /// </summary>
        /// <param name="code">自定义代码</param>
        /// <param name="msg">响应的消息</param>
        /// <param name="data">响应的数据对象</param>
        public ResponseModel(int code, string msg, object data = null, long timespan = 0)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.timestamp = timespan;
        }
    }
}
