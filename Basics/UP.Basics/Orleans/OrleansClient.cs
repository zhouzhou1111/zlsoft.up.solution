using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// Orleans的客户端
    /// </summary>
    public class OrleansClient : IDisposable
    {
        //临时缓存
        private  IClusterClient _client;

        //orleans的客户端
        private static OrleansClient _orleansClient;

        //锁定对象
        private static object locker = new object();

        //创建一个单实例
        public static OrleansClient Instance
        {
            get
            {
                lock (locker)
                {
                    if (_orleansClient == null)
                    {
                        _orleansClient = new OrleansClient();
                    }
                }

                //返回当前实例
                return _orleansClient;
            }
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> CreateInstance<T>() where T : IBasic
        {
            if (_client == null)
            {//如果客户端为空值，则创建一个连接
                _client = await ConnectClient();
            }

            return _client.GetGrain<T>(0);

        }

        /// <summary>
        /// 创建一个客户端
        /// </summary> 
        /// <returns></returns>
        private async Task<IClusterClient> ConnectClient()
        {
            var orleansConfig = OrleansConfig.ReadConfig();
            IClusterClient client;
            client = new ClientBuilder()
                //.UseLocalhostClustering(30001)
                .UseStaticClustering(orleansConfig.GetIPEndPoint())
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            return client;
        }


        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            _client?.Close();
        }
    }
}
