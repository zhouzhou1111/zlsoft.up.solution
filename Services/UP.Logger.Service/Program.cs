using EasyNetQ;
using EasyNetQ.Topology;
using QWPlatform.IService;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using QWPlatform.SystemLibrary.LogManager;

namespace UP.Logger.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region 获取配置文件中MQ连接串

            var mqConn = string.Empty;
            try
            {
                var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "log4net.config");
                //获取MQ连接串
                ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
                ecf.ExeConfigFilename = filepath;
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
                mqConn = config.AppSettings.Settings["UPMQConntion"].Value;
            }
            catch (Exception e)
            {
                QWPlatform.SystemLibrary.Logger.Instance.Error("【日志记录服务】：服务启动失败！读取配置发生异常：" + e?.Message);
                return;
            }

            #endregion 获取配置文件中MQ连接串

            var bus = RabbitHutch.CreateBus(mqConn);

            IExchange ex = bus.Advanced.ExchangeDeclare("Logger.Direct", ExchangeType.Direct);

            IQueue qu = bus.Advanced.QueueDeclare("LoggerQueue");
            bus.Advanced.Bind(ex, qu, "UP.Logger");
            bus.Advanced.Consume(qu, (body, properties, info) => Task.Factory.StartNew(() =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                    var model = LoggerModel.ConvertLoggerMessage(message);

                    #region 写入日志库

                    using (var db = new DbContext())
                    {
                        var result = db.Insert(model).Execute();
                        if (result <= 0)
                        {
                            throw new Exception("写入日志数据库失败！");
                        }
                    }

                    #endregion 写入日志库
                }
                catch (Exception e)
                {
                    //1. 这个异常如何处理？是否考虑直接写入数据库？
                    QWPlatform.SystemLibrary.Logger.Instance.Error("【日志记录服务】：写入数据失败," + e?.Message);
                    throw e;
                }
            }));
            Console.WriteLine("启动日志服务成功！");
            Console.ReadLine();
            bus.Dispose();
        }
    }
}