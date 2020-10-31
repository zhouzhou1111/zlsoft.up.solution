using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using QWPlatform.SystemLibrary;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;


namespace UP.Basics
{
    /// <summary>
    /// Orleans服务实现类
    /// </summary>
    public class OrleansServer
    {
        /// <summary>
        /// 启动一个服务
        /// </summary>
        /// <param name="assemblys">应用程序集</param>
        /// <returns></returns>
        public static async Task<int> RunMainAsync(params Assembly[] assemblys)
        {
            try
            {
                Console.Title = "UP.Orleans服务端";
                foreach (var item in assemblys)
                {//准备加载以下服务组件
                    var name = assemblys[0].GetName();
                    Logger.Instance.Info($"正在加载以下服务组件:{item.GetName().Name}");
                }

                await StartSilo(assemblys);

                Logger.Instance.Info("完成服务启动");

                //异步停止服务,
                //await host.StopAsync();
                //Console.Read();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        /// <summary>
        /// 启动Silo
        /// </summary>
        /// <param name="assemblys"></param>
        /// <returns></returns>
        private static async Task<ISiloHost> StartSilo(params Assembly[] assemblys)
        {
            var orleansConfig = OrleansConfig.ReadConfig();

            // define the cluster configuration
            var builder = new SiloHostBuilder()
                            .UseLocalhostClustering()
                            .Configure<ClusterOptions>(options =>
                            {//群集标识
                                options.ClusterId = "dev";
                                options.ServiceId = "OrleansBasics";
                            })
                            .Configure<EndpointOptions>(options =>
                            {//网关设置
                                //options.AdvertisedIPAddress = IPAddress.Parse("127.0.0.1");//监听外网地址
                                options.AdvertisedIPAddress = IPAddress.Parse(orleansConfig.GatewayAddress);
                                options.SiloPort = orleansConfig.SiloPort;
                                options.GatewayPort = orleansConfig.GatewayPort;
                            })
                            .ConfigureApplicationParts(
                                    parts =>
                                    {
                                        foreach (var assembly in assemblys)
                                        {//注入程序集
                                            parts.AddApplicationPart(assembly).WithReferences();
                                        }
                                    }
                             )
                            .ConfigureLogging(logging => logging.AddConsole());

            if (orleansConfig.UseDashboard)
            {//使用监控
                builder.UseDashboard();
            }

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

    }
}
