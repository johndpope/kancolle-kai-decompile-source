using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class RebellionMapManager : MapManager
	{
		public DeckModel Deck_Main
		{
			get
			{
				return this._mainDeck;
			}
		}

		public DeckModel Deck_Sub
		{
			get
			{
				return this._subDeck;
			}
		}

		public RebellionMapManager(MapModel map, DeckModel mainDeck, DeckModel subDeck) : base(mainDeck, map)
		{
			this._mainDeck = mainDeck;
			this._subDeck = subDeck;
			if (this._subDeck == null)
			{
				this._deck = this._mainDeck;
			}
			else
			{
				this._deck = this._subDeck;
			}
			this._Init();
		}

		public RebellionBattleManager BattleStart(BattleFormationKinds1 formation_id)
		{
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(this._req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = base.GetNextCellEnemyFleetName();
			RebellionBattleManager rebellionBattleManager = new RebellionBattleManager(nextCellEnemyFleetName);
			rebellionBattleManager.__Init__(reqBattle, base.NextEventType, formation_id, this._map, base.IsNextFinal(), isBoss, this._deck == this._subDeck);
			return rebellionBattleManager;
		}

		public RebellionBattleManager BattleStart_Write(BattleFormationKinds1 formation_id)
		{
			DebugBattleMaker.SerializeBattleStart();
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(this._req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = base.GetNextCellEnemyFleetName();
			RebellionBattleManager_Write rebellionBattleManager_Write = new RebellionBattleManager_Write(nextCellEnemyFleetName);
			rebellionBattleManager_Write.__Init__(reqBattle, base.NextEventType, formation_id, this._map, base.IsNextFinal(), isBoss, this._deck == this._subDeck);
			return rebellionBattleManager_Write;
		}

		public RebellionBattleManager BattleStart_Read()
		{
			bool is_boss = base.NextCategory == enumMapEventType.War_Boss;
			return new RebellionBattleManager_Read(is_boss, base.IsNextFinal(), this._map);
		}

		public bool RebellionEnd()
		{
			bool flag = this._req_map.RebellionEnd();
			if (flag)
			{
				base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
			}
			this._deck = (this._mainDeck = (this._subDeck = null));
			this._map = null;
			this._cells = null;
			this._next_cell = null;
			this._req_map = null;
			this._passed = null;
			return flag;
		}

		protected override void _Init()
		{
			Api_Result<Map_ResultFmt> api_Result;
			if (this._subDeck == null)
			{
				api_Result = this._req_map.StartResisted(base.Map.AreaId, this._mainDeck.Id, 0);
			}
			else
			{
				api_Result = this._req_map.StartResisted(base.Map.AreaId, this._subDeck.Id, this._mainDeck.Id);
			}
			if (api_Result.state == Api_Result_State.Success)
			{
				this._next_cell = api_Result.data;
				Dictionary<int, User_MapCellInfo> user_mapcell = this._req_map.User_mapcell;
				using (Dictionary<int, User_MapCellInfo>.KeyCollection.Enumerator enumerator = user_mapcell.get_Keys().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						CellModel cellModel = new CellModel(user_mapcell.get_Item(current));
						this._cells.Add(cellModel);
					}
				}
				this._cells.Sort((CellModel a, CellModel b) => a.CellNo.CompareTo(b.CellNo));
			}
			this._passed = new List<int>();
		}
	}
}
