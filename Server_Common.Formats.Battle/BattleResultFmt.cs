using Common.Enum;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Server_Common.Formats.Battle
{
	public class BattleResultFmt
	{
		private BattleWinRankKinds _winRank;

		private int _basicLevel;

		private string _questName;

		private int _mvpShip;

		private int _getBaseExp;

		[XmlIgnore]
		private SerializableDictionary<int, int> _getShipExp;

		[XmlIgnore]
		private SerializableDictionary<int, List<int>> _levelUpInfo;

		private List<int> _enemyId;

		private string _enemyName;

		private bool _firstClear;

		private bool _firstAreaComplete;

		private int _getSpoint;

		private List<MapItemGetFmt> _getAirReconnaissanceItems;

		private List<ItemGetFmt> _getItem;

		public ItemGetFmt AreaClearRewardItem;

		private List<ItemGetFmt> _getEventItem;

		private ExMapRewardInfo _exMapReward;

		private EscapeInfo _escapeInfo;

		private List<int> _newOpenMapId;

		private List<int> _reOpenMapId;

		public BattleWinRankKinds WinRank
		{
			get
			{
				return this._winRank;
			}
			set
			{
				this._winRank = value;
			}
		}

		public int BasicLevel
		{
			get
			{
				return this._basicLevel;
			}
			set
			{
				this._basicLevel = value;
			}
		}

		public string QuestName
		{
			get
			{
				return this._questName;
			}
			set
			{
				this._questName = value;
			}
		}

		public int MvpShip
		{
			get
			{
				return this._mvpShip;
			}
			set
			{
				this._mvpShip = value;
			}
		}

		public int GetBaseExp
		{
			get
			{
				return this._getBaseExp;
			}
			set
			{
				this._getBaseExp = value;
			}
		}

		[XmlIgnore]
		public SerializableDictionary<int, int> GetShipExp
		{
			get
			{
				return this._getShipExp;
			}
			set
			{
				this._getShipExp = value;
			}
		}

		[XmlIgnore]
		public SerializableDictionary<int, List<int>> LevelUpInfo
		{
			get
			{
				return this._levelUpInfo;
			}
			set
			{
				this._levelUpInfo = value;
			}
		}

		public List<int> EnemyId
		{
			get
			{
				return this._enemyId;
			}
			set
			{
				this._enemyId = value;
			}
		}

		public string EnemyName
		{
			get
			{
				return this._enemyName;
			}
			set
			{
				this._enemyName = value;
			}
		}

		public bool FirstClear
		{
			get
			{
				return this._firstClear;
			}
			set
			{
				this._firstClear = value;
			}
		}

		public bool FirstAreaComplete
		{
			get
			{
				return this._firstAreaComplete;
			}
			set
			{
				this._firstAreaComplete = value;
			}
		}

		public int GetSpoint
		{
			get
			{
				return this._getSpoint;
			}
			set
			{
				this._getSpoint = value;
			}
		}

		public List<MapItemGetFmt> GetAirReconnaissanceItems
		{
			get
			{
				return this._getAirReconnaissanceItems;
			}
			set
			{
				this._getAirReconnaissanceItems = value;
			}
		}

		public List<ItemGetFmt> GetItem
		{
			get
			{
				return this._getItem;
			}
			set
			{
				this._getItem = value;
			}
		}

		public List<ItemGetFmt> GetEventItem
		{
			get
			{
				return this._getEventItem;
			}
			set
			{
				this._getEventItem = value;
			}
		}

		public ExMapRewardInfo ExMapReward
		{
			get
			{
				return this._exMapReward;
			}
			set
			{
				this._exMapReward = value;
			}
		}

		public EscapeInfo EscapeInfo
		{
			get
			{
				return this._escapeInfo;
			}
			set
			{
				this._escapeInfo = value;
			}
		}

		public List<int> NewOpenMapId
		{
			get
			{
				return this._newOpenMapId;
			}
			set
			{
				this._newOpenMapId = value;
			}
		}

		public List<int> ReOpenMapId
		{
			get
			{
				return this._reOpenMapId;
			}
			set
			{
				this._reOpenMapId = value;
			}
		}

		public BattleResultFmt()
		{
			this.EnemyId = new List<int>();
			this.NewOpenMapId = new List<int>();
			this.ReOpenMapId = new List<int>();
		}
	}
}
