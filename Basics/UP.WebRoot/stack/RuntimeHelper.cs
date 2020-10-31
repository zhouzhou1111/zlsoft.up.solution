using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QWPlatform.SystemLibrary;
using UP.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace UP.WebRoot
{
    /// <summary>
    /// 注入扩展
    /// </summary>
    public static class RuntimeHelper
    {
        public const string MyAllowSpecificOrigins = "any";

        /// <summary>
        /// 添加应用组件的依赖注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddWebDi(this IServiceCollection services)
        {
            //获取当前启动路径
            var startPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var files = System.IO.Directory.GetFiles(startPath, "ZLSoft.UP.*.dll");
            var builder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0); ;
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFrom(file);
                builder.AddApplicationPart(assembly);
            }
        }

        /// <summary>
        /// 程序集依赖注入
        /// </summary>
        /// <param name="services">服务实例</param>
        /// <param name="assemblyName">程序集名称。不带DLL</param>
        /// <param name="serviceLifetime">依赖注入的类型 可为空</param>
        public static void AddAssembly(this IServiceCollection services, string assemblyName, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services) + "为空");

            if (String.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName) + "为空");

            var assembly = RuntimeHelper.GetAssemblyByName(assemblyName);

            if (assembly == null)
                throw new DllNotFoundException(nameof(assembly) + ".dll不存在");
            //找到当前程序集的类集合
            var types = assembly.GetTypes();
            //过滤筛选（是类文件，并且不是抽象类，不是泛型）
            var list = types.Where(o => o.IsClass && !o.IsAbstract && !o.IsGenericType).ToList();
            if (list == null && !list.Any())
                return; 

            //遍历获取到的类
            foreach (var type in list)
            {
                //然后获取到类对应的接口
                var interfacesList = type.GetInterfaces();
                //校验接口存在则继续
                if (interfacesList == null || !interfacesList.Any())
                    continue;
                //获取到接口（第一个）
                var inter = interfacesList.First();
                switch (serviceLifetime)
                {
                    //根据条件，选择注册依赖的方法
                    case ServiceLifetime.Scoped:
                        //将获取到的接口和类注册进去
                        services.AddScoped(inter, type);
                        break;
                    case ServiceLifetime.Singleton:
                        services.AddScoped(inter, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(inter, type);
                        break;
                }
            }
        }

        /// <summary>
        /// 添加指定的跨域处理
        /// </summary>
        /// <param name="services">服务组件</param>
        public static void AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                    builder => 
                                    builder.SetIsOriginAllowed(a => true)
                                    .AllowAnyMethod()
                                   .AllowAnyHeader()
                                    .AllowCredentials());
            });
        }

        /// <summary>
        /// 通过程序集的名称加载程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {

            var path = Assembly.GetEntryAssembly().Location;
            var filepath = System.IO.Path.Combine(path, assemblyName + ".dll");
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(filepath));
        }

        /// <summary>
        /// 获取所有已打上特性标识的接口
        /// </summary>
        /// <returns></returns>
        public static List<ActionDescModel> GetAllActions()
        {
            //待返回对象的集合（获取到的所有忆授权的方法）
            var list = new List<ActionDescModel>();
            var path = AppContext.BaseDirectory;//Assembly.GetEntryAssembly().;
            var files = System.IO.Directory.GetFiles(path, "*.dll");

            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);

                    //只处理从Controller派生过来的类， 
                    var types = assembly.GetTypes()
                                        .Where(type => typeof(Controller).IsAssignableFrom(type) ||
                                                       typeof(ControllerBase).IsAssignableFrom(type));

                    foreach (var type in types)
                    {
                        //获取控制器名称
                        string controllerName = type.Name.Replace("Controller", "");

                        //所有成员的集合
                        var members = type.GetMembers();
                        foreach (var member in members)
                        {
                            if (member.MemberType != MemberTypes.Method)
                            {// 不是方法时，跳过
                                continue;
                            }

                            var postAttr = member.GetCustomAttribute<HttpPostAttribute>();
                            var getAttr = member.GetCustomAttribute<HttpGetAttribute>();
                            var deleteAttr = member.GetCustomAttribute<HttpDeleteAttribute>();
                            var putAttr = member.GetCustomAttribute<HttpPutAttribute>();
                            var ripAuth = member.GetCustomAttribute<RIPAuthorityAttribute>();

                            //只检查这4种属性的接口方法
                            if (postAttr != null ||
                                getAttr != null ||
                                deleteAttr != null ||
                                putAttr != null)
                            {
                                var model = new ActionDescModel
                                {
                                    ControllerName = controllerName,//控制器名称
                                    ActionName = member.Name,//接口方法名称
                                    FullClassName = type.FullName,
                                    MethodCNName = ripAuth?.MethodName,
                                    ActionDescription = ripAuth?.Description,
                                    Author = ripAuth?.Author,
                                    UpdateTime = ripAuth?.UpdateTime
                                };
                                list.Add(model);
                            }
                        }// end for members
                        //}//end if 判断是否从指定的Controller中派生过来
                    }//end for types
                }
                catch
                {//加载程序集异常（因为可能有的dll不是c#或net框架的，只需要加载），这里不用写入日志及记录异常                    
                }
            }//end for files

            return list;
        }
    }
}
