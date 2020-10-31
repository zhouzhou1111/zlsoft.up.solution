using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Utils;
using System.IO;
using System.Net;

namespace UP.Basics
{
    /// <summary>
    /// 连接端配置信息
    /// </summary>
    public class OrleansConfig
    {
        /// <summary>
        /// 读取配置文件信息
        /// </summary>
        /// <returns></returns>
        public static OrleansConfig ReadConfig()
        {
            //读取配置文件路径
            var file = Path.Combine(Strings.AppDomainPath, "config/OrleansServer.json");
            if (File.Exists(file))
            {//文件存在，读取配置信息
                var jsonFileHelper = new JsonFileHelper(file);
                return jsonFileHelper.Read<OrleansConfig>("Orleans");
            }

            throw new FileNotFoundException("OrleansServer.json不存在" + file);
        }

        /// <summary>
        /// 网关地址
        /// </summary>
        public int GatewayPort { get; set; }

        /// <summary>
        /// 网关地址
        /// </summary>
        public string GatewayAddress { get; set; }

        /// <summary>
        /// 服务端仓库端口
        /// </summary>
        public int SiloPort { get; set; }

        /// <summary>
        /// 是否启动性能监控（服务端监控）
        /// </summary>
        public bool UseDashboard { get; set; }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(this.GatewayAddress), this.GatewayPort);
        }
    }
}
