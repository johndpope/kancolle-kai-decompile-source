using Server_Models;
using System;

namespace local.models
{
	public abstract class RevampRecipeModelBase
	{
		protected Mst_slotitem_remodel _mst;

		public int RecipeId
		{
			get
			{
				return this._mst.Id;
			}
		}

		public int Fuel
		{
			get
			{
				return this._mst.Req_material1;
			}
		}

		public int Ammo
		{
			get
			{
				return this._mst.Req_material2;
			}
		}

		public int Steel
		{
			get
			{
				return this._mst.Req_material3;
			}
		}

		public int Baux
		{
			get
			{
				return this._mst.Req_material4;
			}
		}

		public virtual int DevKit
		{
			get
			{
				return this._mst.Req_material5;
			}
		}

		public virtual int RevKit
		{
			get
			{
				return this._mst.Req_material6;
			}
		}

		public Mst_slotitem_remodel __mst__
		{
			get
			{
				return this._mst;
			}
		}

		public RevampRecipeModelBase(Mst_slotitem_remodel mst)
		{
			this._mst = mst;
		}
	}
}
