using System;

namespace UP.Basics
{
    /// <summary>
    /// 实现个人既往史的特性说明
    /// </summary>
    public class PreviousHistoryAttribute : Attribute
    {
       

        /// <summary>
        /// 代码系统:ICD-10
        /// </summary>
        public string CodeType { get; private set; }


        /// <summary>
        /// 结果码
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 结果值
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 上一个值域判断节点
        /// </summary>
        public string Progress { get; private set; }

        /// <summary>
        /// 是否取时间值
        /// </summary>
        public bool IsTime { get; private set; }
        /// <summary>
        /// 设置个人既往史描述
        /// </summary>
        /// <param name="codeType">代码系统</param>
        /// <param name="value">结果值</param>
        /// <param name="code">结果码</param>
        /// <param name="progress">上一个值域判断节点</param>
        public PreviousHistoryAttribute(string codeType, string value, string code, string text, string progress = "", bool isTime = false)
        {
            this.CodeType = codeType;
            this.Code = code;
            this.Value = value;
            this.Text = text;
            this.Progress = progress;
            this.IsTime = isTime;
        }
    }

}
