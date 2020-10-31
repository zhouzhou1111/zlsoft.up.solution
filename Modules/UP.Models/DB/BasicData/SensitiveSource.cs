using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///b_过敏源
	/// </summary>
	[Table("b_过敏源")]
	[DataContract]
	public class SensitiveSource
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
		/// 药物:
		/// </summary>
		[JsonProperty("medicine")] 
		public decimal 药物 { get; set; }

	}
}
