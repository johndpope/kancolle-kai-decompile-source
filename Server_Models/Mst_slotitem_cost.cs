using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem_cost : Model_Base
	{
		public const int CHARGE_COST = 5;

		private int _id;

		private int _cost;

		private static string _tableName = "mst_slotitem_cost";

		public int Id
		{
			get
			{
				return this._id;
			}
			private set
			{
				this._id = value;
			}
		}

		public int Cost
		{
			get
			{
				return this._cost;
			}
			private set
			{
				this._cost = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_slotitem_cost._tableName;
			}
		}

		public int GetAddNum(int onslotKeisu)
		{
			return 0;
		}

		public static OnslotChangeType GetSlotChangeCost(int preMemSlotRId, int afterMemSlotRId, out int preCost, out int afterCost)
		{
			preCost = 5;
			afterCost = 5;
			return OnslotChangeType.PlaneToPlane;
		}

		public static int GetSlotChangeBauxiteNum(OnslotChangeType type, int preCost, int afterCost, int onslotKeisu)
		{
			return 0;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Cost = int.Parse(element.Element("Cost").get_Value());
		}
	}
}
