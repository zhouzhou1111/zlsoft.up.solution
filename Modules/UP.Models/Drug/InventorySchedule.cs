using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Drug
{
	/// <summary>
	///InventorySchedule
	/// </summary>
	public class InventorySchedule
	{
		/// <summary>
		/// ID:
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// DrugID:
		/// </summary>
		public int DrugID { get; set; }

		/// <summary>
		/// Count:
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Operator:
		/// </summary>
		public string Operator { get; set; }

		/// <summary>
		/// OperatorTime:
		/// </summary>
		public DateTime OperatorTime { get; set; }

		/// <summary>
		/// Storehouse:
		/// </summary>
		public string Storehouse { get; set; }

	}
}
