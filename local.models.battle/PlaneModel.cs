using System;

namespace local.models.battle
{
	public class PlaneModel : PlaneModelBase
	{
		private ShipModel_Attacker _parent;

		public ShipModel_Attacker Parent
		{
			get
			{
				return this._parent;
			}
		}

		public PlaneModel(ShipModel_Attacker parent, int slotitem_mst_id) : base(slotitem_mst_id)
		{
			this._parent = parent;
		}
	}
}
