using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace UP.WebRoot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "UP.Web������";
            CreateHostBuilder(args).Build().Run();
            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        //�������󳤶�
                        options.Limits.MaxRequestBodySize = null;
                        options.Limits.MaxRequestBufferSize = int.MaxValue;
                        options.Limits.MaxRequestLineSize = int.MaxValue;
                    });
                    webBuilder.UseStartup<Startup>();

                    #if DEBUG
                    //ֱ�Ӵ��������ַ���������Ӧ�ÿ��Է���
                    //System.Diagnostics.Process.Start("explorer.exe", "http://localhost:10010");
                    #endif
                });
    }
}