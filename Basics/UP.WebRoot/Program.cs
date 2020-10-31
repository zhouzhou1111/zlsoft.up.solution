using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace UP.WebRoot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "UP.Web服务器";
            CreateHostBuilder(args).Build().Run();
            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        //限制请求长度
                        options.Limits.MaxRequestBodySize = null;
                        options.Limits.MaxRequestBufferSize = int.MaxValue;
                        options.Limits.MaxRequestLineSize = int.MaxValue;
                    });
                    webBuilder.UseStartup<Startup>();

                    #if DEBUG
                    //直接打开浏览器地址，正常情况应该可以访问
                    //System.Diagnostics.Process.Start("explorer.exe", "http://localhost:10010");
                    #endif
                });
    }
}