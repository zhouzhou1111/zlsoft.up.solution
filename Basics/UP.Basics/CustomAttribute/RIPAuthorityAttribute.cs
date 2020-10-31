using System;

namespace UP.Basics
{
    /// <summary>
    /// 实现授权体系统的特性说明
    /// </summary>
    public class RIPAuthorityAttribute : Attribute
    {
        /// <summary>
        /// 设置权限的方法名称
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 创建人或编辑人
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 创建时间或编辑时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 是否公共通用授权（登录就可以访问的接口）
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 设置方法的权限描述
        /// </summary>
        /// <param name="name">方法名称</param>
        /// <param name="desc">方法说明</param>
        public RIPAuthorityAttribute(string name, string desc = "")
        {
            this.MethodName = name;
            this.Description = desc;
        }

        /// <summary>
        /// 设置对接口方法的描述
        /// </summary>
        /// <param name="name">方法中文名</param>
        /// <param name="desc">方法描述</param>
        /// <param name="author">创建或编辑人</param>
        /// <param name="updateTime">更新时间</param>
        public RIPAuthorityAttribute(string name, string desc = "", string author = "", string updateTime = "")
        {
            this.MethodName = name;
            this.Description = desc;
            this.Author = author;
            this.UpdateTime = updateTime;
        }

        /// <summary>
        /// 设置对接口方法的描述
        /// </summary>
        /// <param name="name">方法中文名</param>
        /// <param name="desc">方法描述</param>
        /// <param name="author">创建或编辑人</param>
        /// <param name="updateTime">更新时间</param>
        /// <param name="ispublic">登录用户都可以访问的接口（不需要授权）</param>
        public RIPAuthorityAttribute(string name, string desc = "", string author = "", string updateTime = "", bool ispublic = false)
        {
            this.MethodName = name;
            this.Description = desc;
            this.Author = author;
            this.UpdateTime = updateTime;

            //设置为true表示为公共访问接口，不需要授权即可以访问，在编写代码时确定
            this.IsPublic = ispublic;
        }
    }
}
