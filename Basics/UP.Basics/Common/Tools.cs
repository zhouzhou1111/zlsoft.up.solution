/****************************************************
* 功能：接口公共请求工具类
* 描述：
* 作者：贺伟
* 日期：2020/05/20
*********************************************************/
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using QWPlatform.SystemLibrary.Utils;

namespace UP.Basics
{
    /// <summary>
    /// 接口公共请求工具类
    /// </summary>
    public static class Tools
    {
        //中心医院id
        public const int 中心医院= 128;

        /// <summary>
        /// 获取header消息头信息
        /// </summary>
        /// <returns></returns>
        public static string GetHeaderValue(IHeaderDictionary header, string name)
        {
            var value = string.Empty;
            try
            {
                var valueObjs = header[name];
                if ( valueObjs.Count>0)
                {
                    value = valueObjs.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                value = string.Empty;
            }
            return value;
        }


        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeToInt(DateTime time)
        {
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = Convert.ToInt64(ts.TotalMilliseconds / 1000);
            return t;
        }
        
        /// <summary>
        /// 校验手机号格式
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public static bool VerifyPhone(string phone)
        {
            string RegexIDNumber = @"^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\d{8}$";
            Regex regex = new Regex(RegexIDNumber);
            //手机号为空或格式不正确
            if (string.IsNullOrEmpty(phone) || !regex.IsMatch(phone))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验身份证号格式
        /// </summary>
        /// <param name="idcard">身份证号</param>
        /// <returns></returns>
        public static bool VerifyIDCard(string idcard, int sex)
        {
            //男
            if(sex == 1)
            {
                return Strings.IdentityCodeValidateForMan(idcard);
            }
            else if(sex == 2)
            {
                return Strings.IdentityCodeValidateForWoman(idcard);
            }

            return Strings.IdentityCodeValidate(idcard);
        }

        public static T CopyModel<T>(this object obj)
        {
            //对象序列化Json
            string strJson = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(strJson);
        }

        /// <summary>
        /// 根据出生日期计算年龄
        /// </summary>
        /// <param name="bdate">出生日期</param>
        /// <returns></returns>
        public static int GetAgeByBirthdate(DateTime? bdate)
        {
            DateTime now = DateTime.Now;
            int age = 0;
            if (bdate != null)
            {
                var birthdate = bdate.ToDateTime();
                age = now.Year - birthdate.Year;
                if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
                {
                    age--;
                }
            }
            return age < 0 ? 0 : age;
        }
    }
}
