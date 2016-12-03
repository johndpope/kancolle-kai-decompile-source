using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class SortieMapManager : MapManager
	{
		public SortieMapManager(DeckModel deck, MapModel map, List<MapModel> maps) : base(deck, map, maps)
		{
			this._Init();
		}

		public virtual SortieBattleManager BattleStart(BattleFormationKinds1 formationId)
		{
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(this._req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = base.GetNextCellEnemyFleetName();
			SortieBattleManager sortieBattleManager = new SortieBattleManager(nextCellEnemyFleetName);
			sortieBattleManager.__Init__(reqBattle, base.NextEventType, formationId, this._map, this._maps, base.IsNextFinal(), isBoss);
			return sortieBattleManager;
		}

		public SortieBattleManager BattleStart_Write(BattleFormationKinds1 formationId)
		{
			DebugBattleMaker.SerializeBattleStart();
			Api_req_SortieBattle reqBattle = new Api_req_SortieBattle(this._req_map);
			bool isBoss = base.NextCategory == enumMapEventType.War_Boss;
			string nextCellEnemyFleetName = base.GetNextCellEnemyFleetName();
			SortieBattleManager_Write sortieBattleManager_Write = new SortieBattleManager_Write(nextCellEnemyFleetName);
			sortieBattleManager_Write.__Init__(reqBattle, base.NextEventType, formationId, this._map, this._maps, base.IsNextFinal(), isBoss);
			return sortieBattleManager_Write;
		}

		public SortieBattleManager BattleStart_Read()
		{
			bool is_boss = base.NextCategory == enumMapEventType.War_Boss;
			return new SortieBattleManager_Read(is_boss, base.IsNextFinal(), this._map);
		}

		public override TurnState MapEnd()
		{
			TurnState result = base.MapEnd();
			this._deck = (this._mainDeck = (this._subDeck = null));
			this._map = null;
			this._cells = null;
			this._next_cell = null;
			this._req_map = null;
			this._passed = null;
			return result;
		}

		protected override void _Init()
		{
			Api_Result<Map_ResultFmt> api_Result = this._req_map.Start(base.Map.AreaId, base.Map.No, base.Deck.Id);
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
