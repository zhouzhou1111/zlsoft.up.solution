namespace UP.Basics
{
    /// <summary>
    /// 接口描述原型
    /// </summary>
    public class ActionDescModel
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 接口方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 类型名称，带命令空间
        /// </summary>
        public string FullClassName { get; set; }

        /// <summary>
        /// 方法中文名称
        /// </summary>
        public string MethodCNName { get; set; }

        /// <summary>
        /// 方法描述
        /// </summary>
        public string ActionDescription { get; set; }

        /// <summary>
        /// 作者/或编辑人
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 修改或创建日期时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
