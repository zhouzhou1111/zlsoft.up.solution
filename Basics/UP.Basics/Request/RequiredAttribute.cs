/****************************************************
* 功能：请求头信息必填标记
* 描述：
* 作者：贺伟
* 日期：2020/05/20
*********************************************************/

using System;
using System.Reflection;

namespace UP.Basics.Request
{
    /// <summary>
    /// 验证不为空
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : Attribute
    {
        /// <summary>
        /// 最大长度
        /// </summary>
        public int length;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string dsc;

        /// <summary>
        /// 验证不为空构造函数
        /// </summary>
        /// <param name="length">最大长度</param>
        /// <param name="dsc">返回信息</param>
        public RequiredAttribute(int length, string dsc)
        {
            this.length = length;
            this.dsc = dsc;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ValidateRequired<T>(T entity, ref string msg) where T : class
        {
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties();//通过反射获取所有属性
            foreach (var item in properties)
            {
                if (item.IsDefined(typeof(RequiredAttribute), true))//判断该属性是否被RequiredAttribute特性进行标识
                {
                    //字段被RequiredAttribute标识了
                    var value = item.GetValue(entity);//反射获取属性值
                    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        //如果字段值为null 或""  "  ",则验证不通过
                        //获取该属性设置的必填属性
                        RequiredAttribute attribute = item.GetCustomAttribute(typeof(RequiredAttribute), true) as RequiredAttribute;
                        if (attribute != null)
                        {
                            msg = attribute.dsc;
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    }
}