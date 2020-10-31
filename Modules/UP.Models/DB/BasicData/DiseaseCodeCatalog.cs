using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BasicData
{
	/// <summary>
	///b_疾病编码目录
	/// </summary>
	[Table("b_疾病编码目录")]
	[DataContract]
	public class DiseaseCodeCatalog
	{
		/// <summary>
		/// ID:
		/// </summary>
		[JsonProperty("id")]
		public string ID { get; set; }

		/// <summary>
		/// 编码:
		/// </summary>
		[JsonProperty("code")]
		public string 编码 { get; set; }

		/// <summary>
		/// 序号:
		/// </summary>
		[JsonProperty("ordernum")]
		public string 序号 { get; set; }

		/// <summary>
		/// 附码:
		/// </summary>
		[JsonProperty("additionalcode")]
		public string 附码 { get; set; }

		/// <summary>
		/// 名称:
		/// </summary>
		[JsonProperty("name")]
		public string 名称 { get; set; }

		/// <summary>
		/// 拼音码:
		/// </summary>
		[JsonProperty("pinyincode")]
		public string 拼音码 { get; set; }

		/// <summary>
		/// 五笔码:
		/// </summary>
		[JsonProperty("wubicode")]
		public string 五笔码 { get; set; }

		/// <summary>
		/// 说明:
		/// </summary>
		[JsonProperty("explain")]
		public string 说明 { get; set; }

		/// <summary>
		/// 性别限制:
		/// </summary>
		[JsonProperty("sexrestriction")]
		public string 性别限制 { get; set; }

		/// <summary>
		/// 疗效限制:
		/// </summary>
		[JsonProperty("curativeffect")]
		public string 疗效限制 { get; set; }

		/// <summary>
		/// 类型:
		/// </summary>
		[JsonProperty("type")]
		public string 类型 { get; set; }

		/// <summary>
		/// 类别:
		/// </summary>
		[JsonProperty("category")]
		public string 类别 { get; set; }

		/// <summary>
		/// 分类ID:
		/// </summary>
		[JsonProperty("classificationid")]
		public string 分类ID { get; set; }

		/// <summary>
		/// 建档时间:
		/// </summary>
		[JsonProperty("filingtime")]
		public DateTime 建档时间 { get; set; }

		/// <summary>
		/// 撤档时间:
		/// </summary>
		[JsonProperty("Shifttime")]
		public DateTime 撤档时间 { get; set; }

		/// <summary>
		/// 优先推荐:
		/// </summary>
		[JsonProperty("recommend")]
		public string 优先推荐 { get; set; }

	}
}
