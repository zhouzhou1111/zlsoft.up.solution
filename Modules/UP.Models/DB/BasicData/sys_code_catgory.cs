using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///sys_code_catgory
	/// </summary>
	[DataContract]
	public class sys_code_catgory
	{
		/// <summary>
		/// id:
		/// </summary>
		[JsonProperty("id")]
		public string id { get; set; }

		/// <summary>
		/// parent_id:
		/// </summary>
		[JsonProperty("parent_id")]
		public string parent_id { get; set; }

		/// <summary>
		/// name:
		/// </summary>
		[JsonProperty("name")]
		public string name { get; set; }

		/// <summary>
		/// describe:
		/// </summary>
		[JsonProperty("describe")]
		public string describe { get; set; }

		/// <summary>
		/// ref_table:动态生成的表名(非必填项目)
		/// </summary>
		[JsonProperty("ref_table")]
		public string ref_table { get; set; }

		/// <summary>
		/// stdd_code:引用外部的标准(非必填项目)
		/// </summary>
		[JsonProperty("stdd_code")]
		public string stdd_code { get; set; }

		/// <summary>
		/// stdd_source:设置的数据来源于某个标准
		/// </summary>
		[JsonProperty("stdd_source")]
		public string stdd_source { get; set; }

		/// <summary>
		/// status:用来标记数据的启用/停用状态，0-停用，1-启用,-1-未生效
		/// </summary>
		[JsonProperty("status")]
		public decimal status { get; set; }

		/// <summary>
		/// update_time:最后一次的更新时间
		/// </summary>
		[JsonProperty("update_time")]
		public DateTime update_time { get; set; }

	}
}
