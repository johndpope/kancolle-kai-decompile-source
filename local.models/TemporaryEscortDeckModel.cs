using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class TemporaryEscortDeckModel : __EscortDeckModel__
	{
		private int _id;

		private DeckShips _deckships;

		private string _name;

		public override int Id
		{
			get
			{
				return this._id;
			}
		}

		public override int AreaId
		{
			get
			{
				return this._id;
			}
		}

		public override string Name
		{
			get
			{
				return (!(this._name != string.Empty)) ? base.Name : this._name;
			}
		}

		public override int Turn
		{
			get
			{
				return 0;
			}
		}

		public DeckShips DeckShips
		{
			get
			{
				return this._deckships;
			}
		}

		public override string __Name__
		{
			get
			{
				return this._name;
			}
		}

		public TemporaryEscortDeckModel(int id, DeckShips deckships, string name, Dictionary<int, ShipModel> ships) : base(null, null)
		{
			this._id = id;
			this._deckships = deckships;
			this._name = name;
			if (this._deckships != null)
			{
				base._Update(deckships, ships);
			}
		}

		public void ChangeName(string new_name)
		{
			this._name = new_name;
		}

		public void __Update__(Dictionary<int, ShipModel> ships)
		{
			base._Update(this._deckships, ships);
		}
	}
}
