using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///b_疾病编码分类
	/// </summary>
	[Table("b_疾病编码分类")]
	[DataContract]
	public class DiseaseCodeSort
	{
		/// <summary>
		/// id:
		/// </summary>
		[JsonProperty("id")]
		public string id { get; set; }

		/// <summary>
		/// 上级id:
		/// </summary>
		[JsonProperty("pid")]
		public string 上级id { get; set; }

		/// <summary>
		/// 序号:
		/// </summary>
		[JsonProperty("ordernum")]
		public string 序号 { get; set; }

		/// <summary>
		/// 名称:
		/// </summary>
		[JsonProperty("name")]
		public string 名称 { get; set; }

		/// <summary>
		/// 简码:
		/// </summary>
		[JsonProperty("scode")]
		public string 简码 { get; set; }

		/// <summary>
		/// 类别:
		/// </summary>
		[JsonProperty("type")]
		public string 类别 { get; set; }

		/// <summary>
		/// 范围:
		/// </summary>
		[JsonProperty("range")]
		public string 范围 { get; set; }

	}
}
