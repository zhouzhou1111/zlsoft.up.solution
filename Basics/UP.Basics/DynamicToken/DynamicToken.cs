using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UP.Basics.DynamicToken
{
    public class DynamicToken
    {
        private static List<GuidKey> guidKeyList;
        private static Timer keysClearTimer;
        private static object obj = new object();
        /// <summary>
        /// 初始化指令牌(Keys)
        /// </summary>
        public static void InitKeys()
        {
            try
            {
                guidKeyList = new List<GuidKey>();
                keysClearTimer = new Timer(new TimerCallback(ClearKeys), null, 0, 600000);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("初始化指令牌失败：", ex);
            }
        }
        /// <summary>
        /// 清空超时的key
        /// </summary>
        /// <param name="state"></param>
        private static void ClearKeys(object state)
        {
            //得到超时一分钟的key列表
            List<GuidKey> guidKeyTimeOutList = guidKeyList.Where(p => p.AddTime.AddMinutes(10) < DateTime.Now).ToList();
            lock (guidKeyList)
            {
                foreach (GuidKey guidKey in guidKeyTimeOutList)
                {
                    guidKeyList.Remove(guidKey);
                }
            }
        }

        /// <summary>
        /// 新生成一个Key
        /// </summary>
        /// <returns></returns>
        public static string NewKey()
        {
            string guid = NewRandom().ToStringEx();//Guid.NewGuid().ToString().ToUpper();
            lock (guidKeyList)
            {
                guidKeyList.Add(new GuidKey() { AddTime = DateTime.Now, Guid = guid });
            }
            return guid;
        }

        /// <summary>
        /// 是否包含一个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContainKey(string key)
        {
            bool isContain = false;
            List<GuidKey> findKeyList = guidKeyList.Where(p => p.Guid == key).ToList();
            if (findKeyList.Count > 0)
            {
                isContain = true;
                //每个令牌只能使用一次，一次登录之后，移除令牌
                lock (guidKeyList)
                {
                    foreach (GuidKey item in findKeyList)
                        guidKeyList.Remove(item);
                }
            }
            return isContain;
        }

        /// <summary>
        /// 获取一个tokenID
        /// </summary>
        /// <returns></returns>
        public static string NewTokenID()
        {
            lock (obj)
            {
                Guid guid = Guid.NewGuid();
                return BitConverter.ToString(guid.ToByteArray()).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// 获取一个6位验证码随机数
        /// </summary>
        /// <returns></returns>
        private static int NewRandom()
        {
            lock (obj)
            {
                Random r = new Random(System.Environment.TickCount);
                int i = r.Next(100000, 999999);
                return i;
            }
        }
    }
    public class GuidKey
    {
        public string Guid { get; set; }
        public DateTime AddTime { get; set; }
    }

}
