using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///b_疾病编码类别
	/// </summary>
	[Table("b_疾病编码类别")]
	[DataContract]
	public class DiseaseCodeType
	{
		/// <summary>
		/// 编码:
		/// </summary>
		[JsonProperty("code")]
		public string 编码 { get; set; }

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
		/// 说明:
		/// </summary>
		[JsonProperty("explain")]
		public string 说明 { get; set; }

		/// <summary>
		/// 排序:
		/// </summary>
		[JsonProperty("ordernum")]
		public string 排序 { get; set; }

	}
}
