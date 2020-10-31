using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace UP.OraleansService
{
    class Program
    {
        static ManualResetEventSlim _mainEvent = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            Console.WriteLine("开始启动服务...");
            CultureInfo.CurrentCulture = new CultureInfo("zh-CN")
            {
                DateTimeFormat = new DateTimeFormatInfo
                {
                    ShortDatePattern = "yyyy-MM-dd",
                    ShortTimePattern = "HH:mm:ss",
                    AMDesignator = "",
                    PMDesignator = ""
                }
            };

            try
            {
                //Assembly. 读取指定文件夹下的相关应用程序集
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Services");
                if (Directory.Exists(path))
                {//找到路径，查找服务的dll, 

                    //存储所有找到结果集
                    var list = new List<Assembly>();
                    var files = Directory.GetFiles(path, "*.*.dll", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {//循环每个文件，加载到应用集中。
                        var assembly = Assembly.LoadFrom(file);
                        list.Add(assembly);
                        Console.WriteLine($"load dll name :{assembly.FullName}");
                    }

                    if (list.Count > 0)
                    {
                        //启动
                        var result = UP.Basics.OrleansServer.RunMainAsync(list.ToArray()).Result;
                        if (result != 0)
                        {
                            Console.WriteLine("完成启动服务!");
                            _mainEvent.Wait();//如果启动成功，一直阻止线程运行。防止退出
                        }
                        Console.ReadKey();

                    }
                }
                else
                {
                    Console.WriteLine($"未找到路径{path},无法启动服务,请创建Services文件夹，并建议按模块分类放置不同的dll.");
                    Console.ReadKey();

                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"启动服务异常" + ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
