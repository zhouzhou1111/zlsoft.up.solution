using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///b_region
	/// </summary>
	[DataContract]
	public class b_region
	{
		/// <summary>
		/// id:主键ID（业务系统直接引用）
		/// </summary>
		[JsonProperty("id")]
		public string id { get; set; }

		/// <summary>
		/// code:同一来源不允许重复
		/// </summary>
		[JsonProperty("code")]
		public string code { get; set; }

		/// <summary>
		/// parent_code:上级编码
		/// </summary>
		[JsonProperty("parent_code")]
		public string parent_code { get; set; }

		/// <summary>
		/// name:
		/// </summary>
		[JsonProperty("name")]
		public string name { get; set; }

		/// <summary>
		/// pinyin:
		/// </summary>
		[JsonProperty("pinyin")]
		public string pinyin { get; set; }

		/// <summary>
		/// prop:0省 1市级（地市级）2区县级 3乡镇、街道级 4 村级
		/// </summary>
		[JsonProperty("prop")]
		public int prop { get; set; }

		/// <summary>
		/// status:用来标记数据的启用/停用状态，0-停用，1-启用,-1-未生效
		/// </summary>
		[JsonProperty("status")]
		public int status { get; set; }

		/// <summary>
		/// location_coordinate:
		/// </summary>
		[JsonProperty("location_coordinate")]
		public string location_coordinate { get; set; }

		/// <summary>
		/// range_coordinate:
		/// </summary>
		[JsonProperty("range_coordinate")]
		public string range_coordinate { get; set; }

		/// <summary>
		/// source:1:民政部，2:统计局
		/// </summary>
		[JsonProperty("source")]
		public int source { get; set; }

		/// <summary>
		/// update_time:最后更新时间
		/// </summary>
		[JsonProperty("update_time")]
		public DateTime update_time { get; set; }

		/// <summary>
		/// year:设置当前行政区划的来源年份
		/// </summary>
		[JsonProperty("year")]
		public int year { get; set; }

	}
}
