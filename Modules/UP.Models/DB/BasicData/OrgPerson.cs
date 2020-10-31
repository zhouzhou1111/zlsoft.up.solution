/*********************************************************
* 功能：机构人员实体
* 描述：
* 作者：胡家源
* 日期：2020/9/17 09:00
**********************************************************/
using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Runtime.Serialization;
namespace UP.Models.DB.BasicData
{
	/// <summary>
	/// b_机构人员
	/// </summary>
	[Table("b_机构人员")]
	[DataContract]
	public class OrgPerson
    {
		/// <summary>
		/// id:人员id
		/// </summary>
		//[PrimaryKey(true)]
		[JsonProperty("id")]
		public int id { get; set; }

		/// <summary>
		/// 机构id:主要所属机构id,如果多点执行则由业务系统管理
		/// </summary>
		[JsonProperty("organid")]
		
		public int 机构id { get; set; }

		/// <summary>
		/// 姓名:
		/// </summary>
		[JsonProperty("name")]
		public string 姓名 { get; set; }

		/// <summary>
		/// 简码:
		/// </summary>
		[JsonProperty("scode")]
		public string 简码 { get; set; }

		/// <summary>
		/// 性别:引用性别编码
		/// </summary>
		[JsonProperty("sex")]
		public string 性别 { get; set; }

		/// <summary>
		/// 民族:引用民族编码
		/// </summary>
		[JsonProperty("nation")]
		public string 民族 { get; set; }

		/// <summary>
		/// 学历:引用学历编码
		/// </summary>
		[JsonProperty("education")]
		public string 学历 { get; set; }

		/// <summary>
		/// 出生日期:
		/// </summary>
		[JsonProperty("birthday")]
		public string 出生日期 { get; set; }

		/// <summary>
		/// 身份证号:
		/// </summary>
		[JsonProperty("card_number")]
		public string 身份证号 { get; set; }

		/// <summary>
		/// 电话:
		/// </summary>
		[JsonProperty("phone")]
		public string 电话 { get; set; }

		/// <summary>
		/// 手机:
		/// </summary>
		[JsonProperty("cell_phone")]
		public string 手机 { get; set; }

		/// <summary>
		/// 邮箱:
		/// </summary>
		[JsonProperty("mailbox")]
		public string 邮箱 { get; set; }

		/// <summary>
		/// 职务:引用职务编码
		/// </summary>
		[JsonProperty("job")]
		public string 职务 { get; set; }

		/// <summary>
		/// 简介:
		/// </summary>
		[JsonProperty("introduce")]
		public string 简介 { get; set; }

		/// <summary>
		/// 擅长:
		/// </summary>
		[JsonProperty("begood")]
		public string 擅长 { get; set; }

		/// <summary>
		/// 状态:数据状态:-1-删除;0-停用;1-正常
		/// </summary>
		[JsonProperty("state")]
		public decimal 状态 { get; set; }
	}
}
