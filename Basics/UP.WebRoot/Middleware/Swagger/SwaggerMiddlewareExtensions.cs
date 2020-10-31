using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UP.Basics;

namespace UP.WebRoot
{
    /// <summary>
    /// Swagger扩展中间件
    /// </summary>
    public static class SwaggerMiddlewareExtensions
    {
        /// <summary>
        /// 使用Swagger的文档接口
        /// </summary>
        /// <param name="services">当前服务</param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    string interfaceContent = @"
<br />
<h1>平台接口参数调用规则</h1>
1、api接口接入首先需要在运维申请平台应用信息，获取接口接入的app_id及接入应用的RSA应用公钥public_key.
<br /><br />
2、api接口请求公共参数请放入Request.Headers中，公共参数有：
appid:运维获取的app_id
timestamp:请求时间戳，东8区，精度毫秒
deviceid :设备id,移动端请求传入设备id，服务端传入mac
token:登录请求获取的token身份信息，除登录、没有特殊说明不需要token及公共接口外必填
sign: RSA使用应用的公钥public_key加密timestamp,并将加密结果进行url编码（特别备注，RSA，采用ASN模式）
<br /><br />
3、未标明需要加密的接口，请将接口参数信息放入body中，如{'name':'zhangsan'}
<br /><br />
4、接口要求参数需要加密的接口，请将参数进行加密后,直接将加密后的参数放入body,如'zzbb1w...'，biz_content生成规则：
a.除公共参数外的原始数据采用url参数字符串拼接(a=b&c=d&e=f)
b.将编码的原始数据进行AES进行加密，加密key使用公共参数的timestamp,加密key必须保证32位，不足32位则左填充0。（特别备注：AES加密[code:UTF-8密，vi (初始变量):0000000000000000, mode(加密模式):CBC,padding(填充方式):Pkcs7]）
c.将AES加密字符串进行url编码，得到加密后的参数biz_content
<br /><br />
5、接口请求返回code（状态码，200表示成功）、msg（接口消息）、data（接口数据）、timestamp（接口响应服务器时间）信息
<br /><br />
6、请求demo <a>查看客户端/h5接口请求示例</a><br /><br /><br />
<h1>第三方接口参数调用规则</h1>
1、api接口接入首先需要在运维申请平台应用信息，获取接口接入的应用app_id.
<br /><br />
2、api接口请求公共参数请放入Request.Headers中，公共参数有：
appid:运维获取的app_id
<br /><br />
3、api接口请求参数请放入body中
<br /><br />
4、接口请求返回code（状态码，200表示成功）、msg（接口消息）、data（接口数据）、timestamp（接口响应服务器时间）信息
";

                    c.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description + interfaceContent,
                        Contact = new OpenApiContact
                        {
                            Name = "智慧生殖平台接口 API",
                            Url = new Uri("http://192.168.31.77:10013"),
                        }
                    });
                });

                //没有加特性的分到这个NoGroup上
                c.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "无分组",
                    Description = "请接口开发人员尽快在控制器上标识模块分组特性，如 [ApiGroup(ApiGroupNames.Admin)] 表示系统集成模块"
                });

                //判断接口归于哪个分组
                c.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "NoGroup")
                    {
                        //当分组为NoGroup时，只要没加特性的都属于这个组
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        return apiDescription.GroupName == docName;
                    }
                });
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                // Set the comments path for the Swagger JSON and UI.
                var files = System.IO.Directory.GetFiles(AppContext.BaseDirectory, "ZLSoft.*.xml");
                var list = new List<string>();
                list.AddRange(files);
                list.Sort((a, b) => a.CompareTo(b));
                list.ForEach(file =>
                {
                    if (File.Exists(file))
                    {
                        //文件存在，则加载
                        c.IncludeXmlComments(file, true);
                    }
                });

                //解析方法及路径的冲突
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                //使用JWT认证
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {//注册使用Jwt在Swagger上加载权限窗口
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                //c.MapType<JsonPropertyAttribute>(() => new OpenApiSchema { Type = "string" });
                //安装并启用注释
                c.EnableAnnotations();
            });
        }
    }
}