using System.Collections.Generic;

namespace UP.Basics.LayuiModels
{
    /// <summary>
    /// 导航菜单模型
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int code { get; set; }

        public string msg { get; set; }

        public List<MenuSubModel> data { get; set; }
    }

    /// <summary>
    /// 具体菜单
    /// </summary>
    public class MenuSubModel
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 子菜单项目
        /// </summary>
        public List<MenuSubModel> list { get; set; }
    }
}
