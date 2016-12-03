using Common.Enum;
using Common.Struct;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class UserInfoModel
	{
		private Mem_basic _basic;

		private Dictionary<int, ShipModel> _ships;

		private Dictionary<int, DeckModel> _decks;

		private Dictionary<int, __EscortDeckModel__> _escort_decks;

		private TutorialModel _tutorialFlgs;

		public DifficultKind Difficulty
		{
			get
			{
				return this._basic.Difficult;
			}
		}

		public string Name
		{
			get
			{
				return this._basic.Nickname;
			}
		}

		public int Level
		{
			get
			{
				return this._basic.UserLevel();
			}
		}

		public int Rank
		{
			get
			{
				return this._basic.UserRank();
			}
		}

		public int SPoint
		{
			get
			{
				return this._basic.Strategy_point;
			}
		}

		public int FCoin
		{
			get
			{
				return this._basic.Fcoin;
			}
		}

		public int DeckCount
		{
			get
			{
				return this._decks.get_Count();
			}
		}

		public int MaxDutyExecuteCount
		{
			get
			{
				return this._basic.Max_quest;
			}
		}

		public int[] DeckIDs
		{
			get
			{
				int[] array = new int[this._decks.get_Keys().get_Count()];
				this._decks.get_Keys().CopyTo(array, 0);
				return array;
			}
		}

		public TutorialModel Tutorial
		{
			get
			{
				return this._tutorialFlgs;
			}
		}

		public int StartMapCount
		{
			get
			{
				return Comm_UserDatas.Instance.User_trophy.Start_map_count;
			}
		}

		public int WinSCount
		{
			get
			{
				return Comm_UserDatas.Instance.User_trophy.Win_S_count;
			}
		}

		public int RevampCount
		{
			get
			{
				return Comm_UserDatas.Instance.User_trophy.Revamp_count;
			}
		}

		public UserInfoModel()
		{
			this._basic = null;
			this._ships = null;
			this._decks = null;
			this._escort_decks = null;
			this._tutorialFlgs = new TutorialModel();
			this._Init();
		}

		public int GetMaterialMaxNum()
		{
			return this._basic.GetMaterialMaxNum();
		}

		public List<ShipModel> __GetShipList__()
		{
			List<ShipModel> list = new List<ShipModel>();
			using (Dictionary<int, ShipModel>.ValueCollection.Enumerator enumerator = this._ships.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipModel current = enumerator.get_Current();
					list.Add(current);
				}
			}
			return list;
		}

		public DeckModel GetDeck(int deck_id)
		{
			if (this._decks.ContainsKey(deck_id))
			{
				return this._decks.get_Item(deck_id);
			}
			return null;
		}

		public DeckModel GetDeckByShipMemId(int ship_mem_id)
		{
			using (Dictionary<int, DeckModel>.ValueCollection.Enumerator enumerator = this._decks.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DeckModel current = enumerator.get_Current();
					if (current.HasShipMemId(ship_mem_id))
					{
						return current;
					}
				}
			}
			return null;
		}

		public DeckModel[] GetDecks()
		{
			List<DeckModel> list = new List<DeckModel>();
			list.AddRange(this._decks.get_Values());
			return list.ToArray();
		}

		public DeckModel[] GetDecksFromArea(int area_id)
		{
			List<DeckModel> list = new List<DeckModel>();
			list.AddRange(this._decks.get_Values());
			return list.FindAll((DeckModel deck) => deck.AreaId == area_id).ToArray();
		}

		public List<int> __GetShipMemIdInDecks__()
		{
			List<int> list = new List<int>();
			using (Dictionary<int, DeckModel>.ValueCollection.Enumerator enumerator = this._decks.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DeckModel current = enumerator.get_Current();
					list.AddRange(current.__GetShipMemIds__());
				}
			}
			return list;
		}

		public EscortDeckModel GetEscortDeck(int area_id)
		{
			if (this._escort_decks.ContainsKey(area_id))
			{
				return this._escort_decks.get_Item(area_id);
			}
			return null;
		}

		public EscortDeckModel GetEscortDeckByShipMemId(int ship_mem_id)
		{
			using (Dictionary<int, __EscortDeckModel__>.ValueCollection.Enumerator enumerator = this._escort_decks.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					__EscortDeckModel__ current = enumerator.get_Current();
					if (current.HasShipMemId(ship_mem_id))
					{
						return current;
					}
				}
			}
			return null;
		}

		public List<int> __GetShipMemIdInEscortDecks__()
		{
			List<int> list = new List<int>();
			using (Dictionary<int, __EscortDeckModel__>.ValueCollection.Enumerator enumerator = this._escort_decks.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EscortDeckModel current = enumerator.get_Current();
					list.AddRange(current.__GetShipMemIds__());
				}
			}
			return list;
		}

		public List<int> __GetShipMemIdInAllDecks__()
		{
			List<int> list = this.__GetShipMemIdInDecks__();
			list.AddRange(this.__GetShipMemIdInEscortDecks__());
			return list;
		}

		public HashSet<int> __GetShipMemIdHashInBothDeck__()
		{
			HashSet<int> hashSet = new HashSet<int>();
			using (Dictionary<int, DeckModel>.ValueCollection.Enumerator enumerator = this._decks.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DeckModel current = enumerator.get_Current();
					ShipModel[] ships = current.GetShips();
					for (int i = 0; i < ships.Length; i++)
					{
						if (ships[i] != null)
						{
							hashSet.Add(ships[i].MemId);
						}
					}
				}
			}
			using (Dictionary<int, __EscortDeckModel__>.ValueCollection.Enumerator enumerator2 = this._escort_decks.get_Values().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					EscortDeckModel current2 = enumerator2.get_Current();
					ShipModel[] ships = current2.GetShips();
					for (int j = 0; j < ships.Length; j++)
					{
						if (ships[j] != null)
						{
							hashSet.Add(ships[j].MemId);
						}
					}
				}
			}
			return hashSet;
		}

		public TemporaryEscortDeckModel __CreateEscortDeckClone__(int area_id)
		{
			__EscortDeckModel__ _EscortDeckModel__ = (__EscortDeckModel__)this.GetEscortDeck(area_id);
			if (_EscortDeckModel__ != null)
			{
				return _EscortDeckModel__.GetCloneDeck(this._ships);
			}
			return null;
		}

		public ShipModel GetShip(int ship_mem_id)
		{
			ShipModel result;
			this._ships.TryGetValue(ship_mem_id, ref result);
			return result;
		}

		public ShipModel GetFlagShip(int deck_id)
		{
			DeckModel deck = this.GetDeck(deck_id);
			return deck.GetFlagShip();
		}

		public int GetDeckID(int ship_mem_id)
		{
			DeckModel deckByShipMemId = this.GetDeckByShipMemId(ship_mem_id);
			return (deckByShipMemId != null) ? deckByShipMemId.Id : -1;
		}

		public int GetEscortDeckId(int ship_mem_id)
		{
			EscortDeckModel escortDeckByShipMemId = this.GetEscortDeckByShipMemId(ship_mem_id);
			return (escortDeckByShipMemId != null) ? escortDeckByShipMemId.Id : -1;
		}

		public MemberMaxInfo ShipCountData()
		{
			return local.utils.Utils.ShipCountData();
		}

		public MemberMaxInfo SlotitemCountData()
		{
			return local.utils.Utils.SlotitemCountData();
		}

		public int GetPortBGMId(int deck_id)
		{
			Mem_room mem_room;
			if (Comm_UserDatas.Instance.User_room.TryGetValue(deck_id, ref mem_room))
			{
				return mem_room.Bgm_id;
			}
			return 0;
		}

		private void _Init()
		{
			this._decks = new Dictionary<int, DeckModel>();
			this._escort_decks = new Dictionary<int, __EscortDeckModel__>();
			Api_get_Member api_get_Member = new Api_get_Member();
			Api_Result<Mem_basic> api_Result = api_get_Member.Basic();
			if (api_Result.state == Api_Result_State.Success)
			{
				this._basic = api_Result.data;
				this._tutorialFlgs.__Update__(this._basic);
			}
			this.__UpdateShips__(api_get_Member);
			this.__UpdateDeck__(api_get_Member);
			this.__UpdateEscortDeck__(api_get_Member);
		}

		public void __UpdateShips__()
		{
			this.__UpdateShips__(new Api_get_Member());
		}

		public void __UpdateShips__(Api_get_Member api_get_mem)
		{
			this._ships = new Dictionary<int, ShipModel>();
			Api_Result<Dictionary<int, Mem_ship>> api_Result = api_get_mem.Ship(null);
			if (api_Result.state == Api_Result_State.Success)
			{
				using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator = api_Result.data.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						this._ships.Add(current.Rid, new ShipModel(current));
					}
				}
			}
			this.__UpdateDeck__(api_get_mem);
		}

		public void __RemoveGekichinShips__(List<int> ids)
		{
			if (this._ships == null)
			{
				this._ships = new Dictionary<int, ShipModel>();
			}
			if (ids == null)
			{
				return;
			}
			for (int i = 0; i < ids.get_Count(); i++)
			{
				int num = ids.get_Item(i);
				if (this._ships.ContainsKey(num))
				{
					this._ships.Remove(num);
				}
			}
		}

		public void __UpdateDeck__()
		{
			this.__UpdateDeck__(new Api_get_Member());
		}

		public void __UpdateDeck__(Api_get_Member api_get_mem)
		{
			Api_Result<Dictionary<int, Mem_deck>> api_Result = api_get_mem.Deck();
			if (api_Result.state == Api_Result_State.Success)
			{
				using (Dictionary<int, Mem_deck>.KeyCollection.Enumerator enumerator = api_Result.data.get_Keys().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						Mem_deck mem_deck = api_Result.data.get_Item(current);
						DeckModel deckModel;
						if (this._decks.TryGetValue(current, ref deckModel))
						{
							deckModel.__Update__(mem_deck, this._ships);
						}
						else
						{
							this._decks.set_Item(current, new DeckModel(mem_deck, this._ships));
						}
					}
				}
			}
		}

		public void __UpdateEscortDeck__(Api_get_Member api_get_mem)
		{
			Api_Result<Dictionary<int, Mem_esccort_deck>> api_Result = api_get_mem.EscortDeck();
			if (api_Result.state == Api_Result_State.Success)
			{
				using (Dictionary<int, Mem_esccort_deck>.KeyCollection.Enumerator enumerator = api_Result.data.get_Keys().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						Mem_esccort_deck mem_escort_deck = api_Result.data.get_Item(current);
						__EscortDeckModel__ _EscortDeckModel__;
						if (this._escort_decks.TryGetValue(current, ref _EscortDeckModel__))
						{
							_EscortDeckModel__.__Update__(mem_escort_deck, this._ships);
						}
						else
						{
							this._escort_decks.set_Item(current, new __EscortDeckModel__(mem_escort_deck, this._ships));
						}
					}
				}
			}
		}

		public void __UpdateTemporaryEscortDeck__(TemporaryEscortDeckModel deck)
		{
			deck.__Update__(this._ships);
		}

		public override string ToString()
		{
			return string.Format("提督名:{0}  提督レベル:{1}  保有艦隊数:{2}  ゲーム難易度:{3}  所有戦略P:{4} Tutorial:({5})", new object[]
			{
				this.Name,
				this.Level,
				this.DeckCount,
				this.Difficulty,
				this.SPoint,
				this.Tutorial
			});
		}
	}
}
