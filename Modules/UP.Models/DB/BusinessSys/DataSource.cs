using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BusinessSys
{
	/// <summary>
	///系统_产品数据源
	/// </summary>
	[Table("系统_产品数据源")]
	[DataContract]
	public class DataSource
    {
		/// <summary>
		/// id:产品id
		/// </summary>
		[PrimaryKey(true)]
		[JsonProperty("id")]
		public int id { get; set; }

		/// <summary>
		/// 产品id:关联的产品id
		/// </summary>
		[JsonProperty("productid")]
		public int 产品id { get; set; }

		/// <summary>
		/// 名称:
		/// </summary>
		[JsonProperty("name")]
		public string 名称 { get; set; }

		/// <summary>
		/// 数据库类型:1：Oracle; 2：Pg;3：SqlServer;4：MySql
		/// </summary>
		[JsonProperty("databasetype")]
		public decimal 数据库类型 { get; set; }

		/// <summary>
		/// ip:
		/// </summary>
		[JsonProperty("ip")]
		public string ip { get; set; }

		/// <summary>
		/// 端口:
		/// </summary>
		[JsonProperty("port")]
		public int 端口 { get; set; }

		/// <summary>
		/// 实例名:mysql,pg,sqlserver则是数据库名
		/// </summary>
		[JsonProperty("examplename")]
		public string 实例名 { get; set; }

		/// <summary>
		/// 用户名:
		/// </summary>
		[JsonProperty("account")]
		public string 用户名 { get; set; }

		/// <summary>
		/// 密码:加密存储各数据库的访问密码
		/// </summary>
		[JsonProperty("password")]
		public string 密码 { get; set; }

		/// <summary>
		/// 说明:数据源的配置信息
		/// </summary>
		[JsonProperty("explain")]
		public string 说明 { get; set; }
	}
}
