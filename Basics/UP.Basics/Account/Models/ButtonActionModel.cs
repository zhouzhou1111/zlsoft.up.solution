namespace UP.Basics
{
    /// <summary>
    /// 各个按钮访问的接口集合模型
    /// </summary>
    public class ButtonActionModel
    {
        /// <summary>
        /// 接口权限ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 模块id
        /// </summary>
        public int menu_id { get; set; }

        /// <summary>
        /// 功能id
        /// </summary>
        public int fun_id { get; set; }

        /// <summary>
        /// 命名空间名称
        /// </summary>
        public string name_space { get; set; }

        /// <summary>
        /// 类型命名
        /// </summary>
        public string class_name { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string method { get; set; }
    }
}