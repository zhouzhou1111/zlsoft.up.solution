using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Basics
{
    /// <summary>
    /// 检验结果标记类
    /// </summary>
    public class LaboratoryAttribute : Attribute
    {
        /// <summary>
        /// 项目名
        /// </summary>
        public string ProjectName { get; private set; }


        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }


        /// <summary>
        /// 项目英文名
        /// </summary>
        public string ProjectE { get; private set; }
        /// <summary>
        /// 设置检验结果描述
        /// </summary>
        /// <param name="name">项目名</param>
        /// <param name="code">项目编码</param>
        /// <param name="ename">项目英文名</param>
        public LaboratoryAttribute(string name, string code, string ename)
        {
            this.ProjectName = name;
            this.ProjectCode = code;
            this.ProjectE = ename;
        }
    }
}
