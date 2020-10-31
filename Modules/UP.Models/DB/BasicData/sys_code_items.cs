using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///sys_code_items
	/// </summary>
	[DataContract]
	public class sys_code_items
	{
		/// <summary>
		/// id:
		/// </summary>
		[JsonProperty("id")]
		public string id { get; set; }

		/// <summary>
		/// parent_id:关联的上级id
		/// </summary>
		[JsonProperty("parent_id")]
		public string parent_id { get; set; }

		/// <summary>
		/// code_catgory_id:关联的分类id
		/// </summary>
		[JsonProperty("code_catgory_id")]
		public string code_catgory_id { get; set; }

		/// <summary>
		/// name:显示内容
		/// </summary>
		[JsonProperty("name")]
		public string name { get; set; }

		/// <summary>
		/// code:通常是编码值内容
		/// </summary>
		[JsonProperty("code")]
		public string code { get; set; }

		/// <summary>
		/// pinyin:一般是记录拼音首字母
		/// </summary>
		[JsonProperty("pinyin")]
		public string pinyin { get; set; }

		/// <summary>
		/// describe:
		/// </summary>
		[JsonProperty("describe")]
		public string describe { get; set; }

		/// <summary>
		/// stdd_code:引用外部的标准
		/// </summary>
		[JsonProperty("stdd_code")]
		public string stdd_code { get; set; }

		/// <summary>
		/// use_sign:用来标记数据的启用/停用状态，0-停用，1-启用,-1-未生效
		/// </summary>
		[JsonProperty("status")]
		public decimal status { get; set; }

		/// <summary>
		/// update_time:最后一次的更新时间
		/// </summary>
		[JsonProperty("update_time")]
		public DateTime update_time { get; set; }

		/// <summary>
		/// sno:排序号
		/// </summary>
		[JsonProperty("sno")]
		public int sno { get; set; }

	}
}
