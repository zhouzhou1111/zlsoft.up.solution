using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Drug
{
	/// <summary>
	///Inventory
	/// </summary>
	public class Inventory
	{
		/// <summary>
		/// ID:唯一ID
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// DrugID:
		/// </summary>
		public int DrugID { get; set; }

		/// <summary>
		/// Amount:库存数量
		/// </summary>
		public int Amount { get; set; }
		 

	/// <summary>
	/// UpdateTime:更新时间
	/// </summary>
	public DateTime UpdateTime { get; set; }

	/// <summary>
	/// Operator:
	/// </summary>
	public string Operator { get; set; }

	/// <summary>
	/// StroehouseID:
	/// </summary>
	public int StroehouseID { get; set; }

}
}
