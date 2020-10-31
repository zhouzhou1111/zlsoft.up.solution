using System;
using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public interface IDBLogger : IBasic
    {
        /// <summary>
        /// 添加日志写入库中
        /// </summary>
        /// <param name="model"></param>
        void Add(DBLoggerModel model);

        /// <summary>
        /// 查询日志列表，按日期倒序
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">每行大小</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="ip">请求IP</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="methodName">方法名称</param>
        /// <returns>返回查询结果的json数据</returns>
        Task<string> GetLogs(int page, int rows, DateTime startTime, DateTime endTime, string ip, string controllerName, string methodName);
    }
}
