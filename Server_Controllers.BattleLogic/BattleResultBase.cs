using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers.BattleLogic
{
	public class BattleResultBase
	{
		private BattleBaseData _myData;

		private BattleBaseData _enemyData;

		private bool _practiceFlag;

		private ExecBattleKinds _execKinds;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		private Mem_mapclear _cleard;

		private Mst_mapcell2 _nowCell;

		private bool _rebellionBattle;

		private List<MapItemGetFmt> getAirCellItems;

		public BattleBaseData MyData
		{
			get
			{
				return this._myData;
			}
			set
			{
				this._myData = value;
			}
		}

		public BattleBaseData EnemyData
		{
			get
			{
				return this._enemyData;
			}
			set
			{
				this._enemyData = value;
			}
		}

		public bool PracticeFlag
		{
			get
			{
				return this._practiceFlag;
			}
			set
			{
				this._practiceFlag = value;
			}
		}

		public ExecBattleKinds ExecKinds
		{
			get
			{
				return this._execKinds;
			}
			set
			{
				this._execKinds = value;
			}
		}

		public Dictionary<int, BattleShipSubInfo> F_SubInfo
		{
			get
			{
				return this._f_SubInfo;
			}
			set
			{
				this._f_SubInfo = value;
			}
		}

		public Dictionary<int, BattleShipSubInfo> E_SubInfo
		{
			get
			{
				return this._e_SubInfo;
			}
			set
			{
				this._e_SubInfo = value;
			}
		}

		public Mem_mapclear Cleard
		{
			get
			{
				return this._cleard;
			}
			set
			{
				this._cleard = value;
			}
		}

		public Mst_mapcell2 NowCell
		{
			get
			{
				return this._nowCell;
			}
			set
			{
				this._nowCell = value;
			}
		}

		public bool RebellionBattle
		{
			get
			{
				return this._rebellionBattle;
			}
			set
			{
				this._rebellionBattle = value;
			}
		}

		public List<MapItemGetFmt> GetAirCellItems
		{
			get
			{
				return this.getAirCellItems;
			}
			set
			{
				this.getAirCellItems = value;
			}
		}

		private BattleResultBase()
		{
		}

		public BattleResultBase(IMakeBattleData battleInstance) : this()
		{
			battleInstance.GetBattleResultBase(this);
		}
	}
}
