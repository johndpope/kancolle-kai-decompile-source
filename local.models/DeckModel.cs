using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class DeckModel : DeckModelBase
	{
		private Mem_deck _mem_deck;

		public override int Id
		{
			get
			{
				return this._mem_deck.Rid;
			}
		}

		public override string Name
		{
			get
			{
				return this._mem_deck.Name;
			}
		}

		public override int AreaId
		{
			get
			{
				return this._mem_deck.Area_id;
			}
		}

		public int MissionId
		{
			get
			{
				return this._mem_deck.Mission_id;
			}
		}

		public MissionStates MissionState
		{
			get
			{
				return this._mem_deck.MissionState;
			}
		}

		public int MissionCompleteTurn
		{
			get
			{
				return this._mem_deck.CompleteTime;
			}
		}

		public int MissionRemainingTurns
		{
			get
			{
				return this._mem_deck.GetRequireMissionTime();
			}
		}

		public DeckModel(Mem_deck mem_deck, Dictionary<int, ShipModel> ships)
		{
			this._mem_deck = mem_deck;
			this.__Update__(mem_deck, ships);
		}

		public override bool IsActionEnd()
		{
			return this._mem_deck.IsActionEnd;
		}

		public List<IsGoCondition> IsValidMove()
		{
			List<IsGoCondition> result = new List<IsGoCondition>();
			this._IsValid_NoActionEnd(ref result);
			this._IsValid_NoMission(ref result);
			this._IsValid_ExistFlagShip(ref result);
			this._IsValid_NoBling(ref result);
			this._IsValid_NoRepair(ref result);
			return result;
		}

		public List<IsGoCondition> IsValidSortie()
		{
			List<IsGoCondition> result = new List<IsGoCondition>();
			this._IsValid_NoActionEnd(ref result);
			this._IsValid_NoMission(ref result);
			this._IsValid_ExistFlagShip(ref result);
			this._IsValid_NoBling(ref result);
			this._IsValid_NoRepair(ref result);
			this._IsValid_NoTaihaFlagShip(ref result);
			this._IsValid_NeedSupply(ref result, 0);
			return result;
		}

		public List<IsGoCondition> IsValidRebellion()
		{
			return this.IsValidSortie();
		}

		public List<IsGoCondition> IsValidMission()
		{
			List<IsGoCondition> list = new List<IsGoCondition>();
			this._IsValid_NoActionEnd(ref list);
			this._IsValid_NoMission(ref list);
			if (this.Id == 1)
			{
				list.Add(IsGoCondition.Deck1);
			}
			this._IsValid_ExistFlagShip(ref list);
			this._IsValid_NoBling(ref list);
			this._IsValid_NoRepair(ref list);
			this._IsValid_NoTaihaFlagShip(ref list);
			return list;
		}

		public List<IsGoCondition> IsValidPractice()
		{
			List<IsGoCondition> result = new List<IsGoCondition>();
			this._IsValid_NoActionEnd(ref result);
			this._IsValid_NoMission(ref result);
			this._IsValid_ExistFlagShip(ref result);
			this._IsValid_NoBling(ref result);
			this._IsValid_NoRepair(ref result);
			this._IsValid_NoTaihaFlagShip(ref result);
			this._IsValid_ConditionRed(ref result);
			this._IsValid_NeedSupply(ref result, 70);
			return result;
		}

		public bool IsInSupportMission()
		{
			Mst_mission2 mst_mission;
			return Mst_DataManager.Instance.Mst_mission.TryGetValue(this.MissionId, ref mst_mission) && mst_mission.IsSupportMission();
		}

		public void __Update__(Mem_deck mem_deck, Dictionary<int, ShipModel> ships)
		{
			base._Update(this._mem_deck.Ship, ships);
		}

		private void _IsValid_NoActionEnd(ref List<IsGoCondition> list)
		{
			if (this.IsActionEnd())
			{
				list.Add(IsGoCondition.ActionEndDeck);
			}
		}

		private void _IsValid_NoMission(ref List<IsGoCondition> list)
		{
			if (this.MissionState != MissionStates.NONE)
			{
				list.Add(IsGoCondition.Mission);
			}
		}

		private void _IsValid_NoRepair(ref List<IsGoCondition> list)
		{
			if (base.HasRepair())
			{
				list.Add(IsGoCondition.HasRepair);
			}
		}

		private void _IsValid_ExistFlagShip(ref List<IsGoCondition> list)
		{
			if (base.Count == 0)
			{
				list.Add(IsGoCondition.InvalidDeck);
			}
		}

		private void _IsValid_NoTaihaFlagShip(ref List<IsGoCondition> list)
		{
			ShipModel ship = base.GetShip(0);
			if (ship != null && ship.DamageStatus == DamageState.Taiha)
			{
				list.Add(IsGoCondition.FlagShipTaiha);
			}
		}

		private void _IsValid_NoBling(ref List<IsGoCondition> list)
		{
			if (base.HasBling())
			{
				list.Add(IsGoCondition.HasBling);
			}
		}

		private void _IsValid_ConditionRed(ref List<IsGoCondition> list)
		{
			if (this._ships.Find((ShipModel ship) => ship != null && ship.ConditionState == FatigueState.Distress) != null)
			{
				list.Add(IsGoCondition.ConditionRed);
			}
		}

		private void _IsValid_NeedSupply(ref List<IsGoCondition> list, int threshold)
		{
			ShipModel shipModel;
			if (threshold == 0)
			{
				shipModel = this._ships.Find((ShipModel s) => s != null && (s.FuelRate == 0.0 || s.AmmoRate == 0.0));
			}
			else
			{
				shipModel = this._ships.Find((ShipModel s) => s != null && (s.FuelRate < (double)threshold || s.AmmoRate < (double)threshold));
			}
			if (shipModel != null)
			{
				list.Add(IsGoCondition.NeedSupply);
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("[{0}]{1}:[", this.Id, this.Name);
			ShipModel[] ships = base.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				text += string.Format("{0}({1},{2})", shipModel.Name, shipModel.MstId, shipModel.MemId);
				if (i + 1 < ships.Length)
				{
					text += string.Format(", ", new object[0]);
				}
			}
			text += string.Format("]", new object[0]);
			return text + string.Format((!this.IsActionEnd()) ? string.Empty : "[行動終了]", new object[0]);
		}

		public string ToDetailString()
		{
			string text = string.Empty;
			text += string.Format("[{0}]{1}", this.Id, this.Name);
			text += string.Format("{0}\n", (!this.IsActionEnd()) ? string.Empty : "[行動終了]");
			ShipModel[] ships = base.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				text += string.Format("  {0}(Lv:{1} Mst:{2} Mem:{3}", new object[]
				{
					shipModel.Name,
					shipModel.Level,
					shipModel.MstId,
					shipModel.MemId
				});
				text += string.Format(" 速:{0} ", shipModel.Soku);
				text += string.Format(" 燃/弾:{0}/{1} ", shipModel.Fuel, shipModel.Ammo);
				for (int j = 0; j < shipModel.SlotCount; j++)
				{
					SlotitemModel slotitemModel = shipModel.SlotitemList.get_Item(j);
					if (slotitemModel == null)
					{
						text += string.Format("[-]", new object[0]);
					}
					else
					{
						text += string.Format("[{0}({1}:{2})]", slotitemModel.Name, slotitemModel.MstId, slotitemModel.MemId);
					}
				}
				text += ")\n";
			}
			return text;
		}
	}
}
