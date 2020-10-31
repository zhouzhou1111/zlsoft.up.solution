using System.Collections.Generic;

namespace UP.Basics.LayuiModels
{
    /// <summary>
    /// 树控件(Layui)
    /// </summary>
    public class TreeModel
    {
        public TreeModel()
        {
            this.Children = new List<TreeModel>();
        }

        /// <summary>
        /// 节点标题
        /// </summary>
        [Newtonsoft.Json.JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 节点唯一索引值，用于对指定节点进行各类操作
        /// </summary>
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Newtonsoft.Json.JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// 节点字段名
        /// </summary>
        [Newtonsoft.Json.JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [Newtonsoft.Json.JsonProperty("children")]
        public List<TreeModel> Children { get; set; }

        /// <summary>
        /// 点击节点弹出新窗口对应的 url。需开启 isJump 参数
        /// </summary>
        [Newtonsoft.Json.JsonProperty("href")]
        public string Href { get; set; }

        /// <summary>
        /// 节点是否初始展开，默认 false
        /// </summary>
        [Newtonsoft.Json.JsonProperty("spread")]
        public bool Spread { get; set; } = false;

        /// <summary>
        /// 节点是否初始为选中状态（如果开启复选框的话），默认 false
        /// </summary>
        [Newtonsoft.Json.JsonProperty("checked")]
        public bool Checked { get; set; } = false;

        /// <summary>
        /// 节点是否为禁用状态。默认 false
        /// </summary>
        [Newtonsoft.Json.JsonProperty("disabled")]
        public bool Disabled { get; set; } = false;
    }
}
