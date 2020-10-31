/*********************************************************
* 功能：机构实体
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
	/// b_机构
	/// </summary>
	[Table("b_机构")]
	[DataContract]
	public class Organ
	{ /// <summary>
	  /// id:机构id
	  /// </summary>
	    [PrimaryKey(true)]
		[JsonProperty("id")]
		public int id { get; set; }

		/// <summary>
		/// 上级id:
		/// </summary>
		[JsonProperty("pid")]
		public int 上级id { get; set; }

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
		/// 简称:机构简称
		/// </summary>
		[JsonProperty("abbreviation")]
		public string 简称 { get; set; }

		/// <summary>
		/// 行政级别:目前固定填写：0:省(自治区、直辖市);1:市(地级市、地区、州);2:县(区、县级市);3:乡(镇、街道办事处);4:村(居委会);5:组;9:其他，后续从基础编码表中读取
		/// </summary>
		[JsonProperty("adminlevel")]
		public int 行政级别 { get; set; }

		/// <summary>
		/// 简码:机构首字母简写
		/// </summary>
		[JsonProperty("scode")]
		public string 简码 { get; set; }

		/// <summary>
		/// 性质:目前固定填写为：1-社区中心;2、乡镇卫生院; 3-服务站 4-村卫生室 5-妇幼保健院 6-综合医院,7 卫健委（管理机构），后续从基础编码表中读取
		/// </summary>
		[JsonProperty("nature")]
		public int 性质 { get; set; }

		/// <summary>
		/// 机构码:
		/// </summary>
		[JsonProperty("orgcode")]
		public string 机构码 { get; set; }

		/// <summary>
		/// 电话:
		/// </summary>
		[JsonProperty("phone")]
		public string 电话 { get; set; }

		/// <summary>
		/// 地址:
		/// </summary>
		[JsonProperty("hosaddress")]
		public string 地址 { get; set; }

		/// <summary>
		/// 执业证代码:
		/// </summary>
		[JsonProperty("licensecode")]
		public string 执业证代码 { get; set; }

		/// <summary>
		/// 组织机构代码证:
		/// </summary>
		[JsonProperty("orgcodecertificate")]
		public string 组织机构代码证 { get; set; }

		/// <summary>
		/// 所属行政区划:
		/// </summary>
		[JsonProperty("admindivision")]
		public string 所属行政区划 { get; set; }
	}
}
