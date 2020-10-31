using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BusinessSys
{
	/// <summary>
	///系统_产品
	/// </summary>
	[Table("系统_产品")]
	[DataContract]
	public class Product
	{
		/// <summary>
		/// id:产品id
		/// </summary>
		[PrimaryKey(true)]
		[JsonProperty("id")]
		public int id { get; set; }

		/// <summary>
		/// 名称:产品名称
		/// </summary>
		[JsonProperty("name")]
		public string 名称 { get; set; }

		/// <summary>
		/// 简称:产品简称
		/// </summary>
		[JsonProperty("abbreviation")]
		public string 简称 { get; set; }

		/// <summary>
		/// 产品ip:产品的ip地址
		/// </summary>
		[JsonProperty("productip")]
		public string 产品ip { get; set; }

		/// <summary>
		/// 端口:产品访问端口
		/// </summary>
		[JsonProperty("port")]
		public int 端口 { get; set; }

		/// <summary>
		/// 状态:数据状态:-1-删除;0-停用;1-正常
		/// </summary>
		[JsonProperty("state")]
		public decimal 状态 { get; set; }

		/// <summary>
		/// 排序:用于产品的排序id
		/// </summary>
		[JsonProperty("sort")]
		public int 排序 { get; set; }

	}
}
