using System.Collections.Generic;

namespace UP.Basics
{
    /// <summary>
    /// 按钮实体
    /// </summary>
    public class ButtonModel
    {
        /// <summary>
        /// 功能id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 模块id
        /// </summary>
        public int menu_id { get; set; }

        /// <summary>
        /// 功能简码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int sort { get; set; }
    }
}