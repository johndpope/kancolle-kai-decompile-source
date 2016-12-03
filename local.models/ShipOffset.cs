using Common.Struct;
using Server_Models;
using System;

namespace local.models
{
	public class ShipOffset
	{
		private int _gra_id;

		public ShipOffset(int gra_id)
		{
			this._gra_id = gra_id;
		}

		public Point GetBoko(bool damaged)
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraph.Boko_d_x, mst_shipgraph.Boko_d_y);
			}
			return new Point(mst_shipgraph.Boko_n_x, mst_shipgraph.Boko_n_y);
		}

		public Point GetFace(bool damaged)
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraph.Face_d_x, mst_shipgraph.Face_d_y);
			}
			return new Point(mst_shipgraph.Face_n_x, mst_shipgraph.Face_n_y);
		}

		public Point GetSlotItemCategory(bool damaged)
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraph.Slotitem_category_d_x, mst_shipgraph.Slotitem_category_d_y);
			}
			return new Point(mst_shipgraph.Slotitem_category_n_x, mst_shipgraph.Slotitem_category_n_y);
		}

		public Point GetShipDisplayCenter(bool damaged)
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraph.Ship_display_center_d_x, mst_shipgraph.Ship_display_center_d_y);
			}
			return new Point(mst_shipgraph.Ship_display_center_n_x, mst_shipgraph.Ship_display_center_n_y);
		}

		public AreaBy2Point GetFaceAreaAtWedding()
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			return new AreaBy2Point(mst_shipgraph.Weda_x, mst_shipgraph.Weda_y, mst_shipgraph.Wedb_x, mst_shipgraph.Wedb_y);
		}

		public Point GetLive2dSize()
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			return new Point(mst_shipgraph.L2dSize_W, mst_shipgraph.L2dSize_H);
		}

		public Point GetLive2dBias()
		{
			Mst_shipgraph mst_shipgraph = Mst_DataManager.Instance.Mst_shipgraph.get_Item(this._gra_id);
			return new Point(mst_shipgraph.L2dBias_X, mst_shipgraph.L2dBias_Y);
		}

		public Point GetFoot_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraphbattle.Foot_d_x, mst_shipgraphbattle.Foot_d_y);
			}
			return new Point(mst_shipgraphbattle.Foot_x, mst_shipgraphbattle.Foot_y);
		}

		public Point GetPog_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraphbattle.Pog_d_x, mst_shipgraphbattle.Pog_d_y);
			}
			return new Point(mst_shipgraphbattle.Pog_x, mst_shipgraphbattle.Pog_y);
		}

		public Point GetPogSp_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraphbattle.Pog_sp_d_x, mst_shipgraphbattle.Pog_sp_d_y);
			}
			return new Point(mst_shipgraphbattle.Pog_sp_x, mst_shipgraphbattle.Pog_sp_y);
		}

		public Point GetPogSpEnsyu_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraphbattle.Pog_sp_ensyu_d_x, mst_shipgraphbattle.Pog_sp_ensyu_d_y);
			}
			return new Point(mst_shipgraphbattle.Pog_sp_ensyu_x, mst_shipgraphbattle.Pog_sp_ensyu_y);
		}

		public Point GetCutin_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraphbattle.Cutin_d_x, mst_shipgraphbattle.Cutin_d_y);
			}
			return new Point(mst_shipgraphbattle.Cutin_x, mst_shipgraphbattle.Cutin_y);
		}

		public Point GetCutinSp1_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			if (damaged)
			{
				return new Point(mst_shipgraphbattle.Cutin_sp1_d_x, mst_shipgraphbattle.Cutin_sp1_d_y);
			}
			return new Point(mst_shipgraphbattle.Cutin_sp1_x, mst_shipgraphbattle.Cutin_sp1_y);
		}

		public double GetScaleMag_InBattle(bool damaged)
		{
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._gra_id);
			return mst_shipgraphbattle.Scale_mag;
		}
	}
}
