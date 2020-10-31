/*********************************************************
* 功能：mvc的基类控制器
* 描述：所有mvc必须继承该类
* 作者：王海洋
* 日期：2019-11-22
*********************************************************/

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QWPlatform.SystemLibrary;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// 所有控制器的基类
    /// </summary>
    //[Authorize]
    public class BaseController : Controller
    {
        //连接orleans的客户端（静态化处理，只需要产生一个）
        private static OrleansClient _client;

        //锁定对象
        private static object locker = new object();

        #region 日志信息

        /// <summary>
        /// 写入错误类日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        protected void LogError(string msg, Exception ex = null)
        {
            Logger.Instance.Error(msg, ex);
        }

        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="msg">写入消息的内容</param>
        protected void LogInfo(string msg)
        {
            Logger.Instance.Info(msg);
        }

        #endregion 日志信息

        #region 用户登录信息

        /// <summary>
        /// 获取当前用户信息（供子类调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected UserModel GetUserInfo()
        {
            //获取消息头传入的token
            var token = Tools.GetHeaderValue(HttpContext.Request.Headers, "Authorization");
            if (!token.IsNullOrEmpty())
            {
                token = token.Replace("Bearer ", "").Trim();
                //userinfo:{usertype.ToInt32()}_{usersource.ToInt32()}_{key}
                var userinfo_key = GetLoginCacheAccount(token);
                if (!string.IsNullOrEmpty(userinfo_key))
                {
                    //如果能从缓存中获取，就从缓存中获取。否则就要从数据库中获取
                    var obj = CacheManager.Create().Get<UserModel>(userinfo_key);
                    if (obj != null)
                    {
                        //更新缓存有效期为2小时
                        CacheManager.Create().Set(userinfo_key, obj, new TimeSpan(2, 0, 0));
                        return obj;
                    }
                    else
                    {
                        //登录账号
                        var account = userinfo_key.Split("_").Last();
                        //用户类型 管理员=1,医生=2,其他=3
                        var usertype = (UserType)Convert.ToInt32(userinfo_key.Split("_")[0]);
                        //从数据库中获取
                        var model = GetUserFromDatabase(account, usertype);
                        //缓存起来，下次使用,目前设置有效期为2小时
                        CacheManager.Create().Set(userinfo_key, model, new TimeSpan(2, 0, 0));
                        return model;
                    }
                }
            }

            //未找到，直接返回空值
            return null;
        }


        /// <summary>
        /// 获取请求来源信息
        /// </summary>
        /// <returns></returns>
        protected UserAppLyInfo GetUserAppLyInfo()
        {
            UserAppLyInfo applyItem = null;
            //获取消息头传入的appid
            var appid = Tools.GetHeaderValue(HttpContext.Request.Headers, "appid");
            if (!appid.IsNullOrEmpty())
            {
                var userApplyinfo_key = "user_applyitem:" + appid;
                try
                {
                    applyItem = CacheManager.Create().Get<UserAppLyInfo>(userApplyinfo_key);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error("获取请求来源信息发生异常", ex);
                }
            }
            return applyItem;
        }

        /// <summary>
        /// 获取医生基本信息，不包含医生角色及权限（供子类调用）
        /// </summary>
        /// <param name="docids">医生id列表</param>
        /// <returns></returns>
        protected List<UserModel> GetDoctorAccountItems(IEnumerable<int> docids)
        {
            var accountObj = this.GetInstance<ISysUser>();
            var items = accountObj.GetUserAccountItems(docids)?.Result;
            return items;
        }

        /// <summary>
        /// 从数据库获取用户信息
        /// </summary>
        /// <param name="code">登录账号</param>
        /// <param name="usertype">用户类型 管理员=1,医生=2,其他=3</param>
        /// <returns></returns>
        protected virtual UserModel GetUserFromDatabase(string code, UserType usertype)
        {
            UserModel item = null;
            //根据用户类型获取用户信息
            switch (usertype)
            {
                case UserType.管理员:
                    var accountWxObj = this.GetInstance<ISysUser>();
                    item = accountWxObj.GetAccountInfo(code)?.Result;
                    break;

                case UserType.医生:
                    var accountObj = this.GetInstance<ISysUser>();
                    item = accountObj.GetAccountInfo(code)?.Result;
                    break;

                case UserType.其他:
                    break;

                default:
                    break;
            }
            return item;
        }

        /// <summary>
        /// 根据指定的用户名删除登录的key（退出登录时使用）
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveLoginCacheToken(string key)
        {
            //删除缓存
            CacheManager.Create().Remove(key);
        }

        #endregion 用户登录信息

        #region 创建Orleans实例

        /// <summary>
        /// 创建一个接口的实例
        /// </summary>
        /// <typeparam name="T">指定泛型的接口</typeparam>
        /// <returns></returns>
        protected T GetInstance<T>() where T : IBasic
        {
            lock (locker)
            {
                if (_client == null)
                {
                    _client = new OrleansClient();
                }
            }

            return _client.CreateInstance<T>().Result;
        }

        #endregion 创建Orleans实例

        #region WebApi调用

        /// <summary>
        /// 通过Get的方式调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="apiName"></param>
        /// <param name="pragm"></param>
        /// <returns></returns>
        protected T GetApi<T>(string url, string apiName, string pragm = "")
        {
            var client = new RestSharpClient(url);

            var request = client.Execute(string.IsNullOrEmpty(pragm)
                                                ? new RestRequest(apiName, Method.GET)
                                                : new RestRequest($"{apiName}/{pragm}", Method.GET));

            if (request.StatusCode != HttpStatusCode.OK)
            {
                return (T)Convert.ChangeType(request.ErrorMessage, typeof(T));
            }

            T result = (T)Convert.ChangeType(request.Content, typeof(T));

            return result;
        }

        /// <summary>
        /// 通过put方式调用
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected T PostApi<T>(object model, string url)
        {
            var client = new RestClient($"{url}");
            IRestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.AddObject(model);
            var result = client.Execute(request);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return (T)Convert.ChangeType(result.ErrorMessage, typeof(T));
            }
            T data = (T)Convert.ChangeType(result.Content, typeof(T));
            return data;
        }

        /// <summary>
        /// 发起POST请求，并获取请求返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">数据实体</param>
        /// <param name="url">接口地址</param>
        protected async Task<T> PostApiAsync<T>(object obj, string url)
        {
            //序列化设置
            var setting = new JsonSerializerSettings();
            //解决枚举类型序列化时，被转换成数字的问题
            setting.Converters.Add(new StringEnumConverter());
            setting.NullValueHandling = NullValueHandling.Ignore;
            var retdata = await HttpPostAsync(url, JsonConvert.SerializeObject(obj, setting));
            return JsonConvert.DeserializeObject<T>(retdata);
        }

        private async Task<string> HttpPostAsync(string url, string postData, string certPath = "", string certPwd = "")
        {
            var request = CreateJsonRequest(url, HttpMethod.Post, postData, certPath, certPwd);
            return await GetResponseStringAsync(request);
        }

        //创建请求的json
        private HttpWebRequest CreateJsonRequest(string url, HttpMethod method, string postData = "", string certpath = "", string certpwd = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToString();
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
            if (!string.IsNullOrEmpty(certpath) && !string.IsNullOrEmpty(certpwd))
            {
                X509Certificate2 cer = new X509Certificate2(certpath, certpwd,
                    X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                request.ClientCertificates.Add(cer);
            }
            if (method == HttpMethod.Post)
            {
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(postData);
                }
            }
            return request;
        }

        private async Task<string> GetResponseStringAsync(HttpWebRequest request)
        {
            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();//获取响应
                }
            }
        }

        #endregion WebApi调用

        #region 缓存控制

        /// <summary>
        /// 添加登录缓存信息
        /// </summary>
        /// <param name="key">登录账户</param>
        /// <param name="token">登录成功身份令牌</param>
        /// <param name="usertype">用户身份类型</param>
        /// <param name="usersource">用户请求来源</param>
        protected void AddLoginCacheToken(string key, string token, UserType usertype, UserSource usersource)
        {
            lock (locker)
            {
                //Source_UserType_key
                var userinfo_key = $"userinfo:{usersource.ToInt32()}_{usertype.ToInt32()}_{key}";
                RemoveLoginCacheToken(userinfo_key);
                //从数据库中查询
                var model = GetUserFromDatabase(key, usertype);
                //缓存起来，下次使用,目前设置有效期为2小时
                CacheManager.Create().Set(userinfo_key, model, new TimeSpan(2, 0, 0));
                //缓存token设置为2小时
                var userinfo_token = $"userinfo_token:{token}";
                CacheManager.Create().Set(userinfo_token, userinfo_key, new TimeSpan(2, 0, 0));
            }
        }


        /// <summary>
        /// 添加平台身份认证缓存信息
        /// </summary>
        /// <param name="account">登录账户</param>
        /// <param name="authtoken">身份认证json信息</param>
        protected void AddAuthCacheToken(string account, string authtoken)
        {
            lock (locker)
            {
                //获取请求来源信息
                var userAppLyInfo = this.GetUserAppLyInfo();
                //Source_key
                var userinfo_key = $"auth:{userAppLyInfo.sourse}:{account}";
                RemoveLoginCacheToken(userinfo_key);

                //缓存起来，下次使用,目前设置有效期为2小时
                CacheManager.Create().Set(userinfo_key, authtoken, new TimeSpan(2, 0, 0));
            }
        }


        /// <summary>
        /// 根据token获取对应的登录账号信息
        /// </summary>
        /// <param name="token">登录获取的令牌信息</param>
        /// <returns>登录账号(userinfo:{usersource.ToInt32()}_{usertype.ToInt32()}_{key})</returns>
        protected string GetLoginCacheAccount(string token)
        {
            var userinfo_token = $"userinfo_token:{token}";
            //userinfo_key = $"userinfo:{usersource.ToInt32()}_{usertype.ToInt32()}_{key}";
            var userinfo_key = CacheManager.Create().Get<string>(userinfo_token);
            if (!userinfo_key.IsNullOrEmpty())
            {
                //更新token缓存2小时
                CacheManager.Create().Set(token, userinfo_key, new TimeSpan(2, 0, 0));
                //最后的下划线的内容才是登录账号
                return userinfo_key;
            }
            return null;
        }

        #endregion 缓存控制

        #region 常用业务执行器

        /// <summary>
        /// 执行Model的插入功能
        /// </summary>
        /// <typeparam name="T">Model的类型</typeparam>
        /// <param name="t">Model的实体</param>
        /// <returns>返回执行器</returns>
        protected IBusinessExecute<T> Add<T>(T t) where T : class, new()
        {
            return new BaseBusinessExecute<T>().Add(t);
        }

        /// <summary>
        /// 更新Model功能
        /// </summary>
        /// <typeparam name="T">Model的类型</typeparam>
        /// <param name="t">参数实体</param>
        /// <returns>返回执行器</returns>
        protected IBusinessExecute<T> Update<T>(T t) where T : class, new()
        {
            return new BaseBusinessExecute<T>().Update(t);
        }

        /// <summary>
        /// 执行删除功能
        /// </summary>
        /// <typeparam name="T">Model的类型</typeparam>
        /// <param name="t">参数实体</param>
        /// <returns>返回执行器</returns>
        protected IBusinessExecute<T> Delete<T>() where T : class, new()
        {
            return new BaseBusinessExecute<T>().Delete();
        }

        /// <summary>
        /// 使用查询器
        /// </summary>
        /// <returns>返回一个执行器</returns>
        protected IBusinessExecute<T> Query<T>() where T : class, new()
        {
            return new BaseBusinessExecute<T>().Select();
        }

        #endregion 常用业务执行器
    }
}