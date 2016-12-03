using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV
{
	public class AppInformation : SingletonMonoBehaviour<AppInformation>
	{
		public enum CurveType
		{
			Live2DEnter,
			_NUM
		}

		public enum LoadType
		{
			Ship,
			Yousei,
			White
		}

		[Serializable]
		public struct CommonCurve
		{
			public AnimationCurve curve;

			public AppInformation.CurveType type;
		}

		public Generics.Scene NextLoadScene;

		public AppInformation.LoadType NextLoadType;

		[SerializeField]
		private int _nCurrentAreaID;

		[SerializeField]
		private int _nCurrentPlayingBgmID = -1;

		[SerializeField]
		private DeckModel _clsCurrentDeck;

		[SerializeField]
		private MapAreaModel _areaModel;

		public bool SlogDraw;

		public int BattleCount;

		public AppInformation.CommonCurve[] curves;

		public static Dictionary<AppInformation.CurveType, AnimationCurve> curveDic;

		public int ReleaseSetNo;

		public int OpenAreaNum;

		public DeckModel[] prevStrategyDecks;

		public int CurrentAreaID
		{
			get
			{
				if (this._clsCurrentDeck == null)
				{
					return 1;
				}
				return this._clsCurrentDeck.AreaId;
			}
		}

		public MapAreaModel CurrentDeckAreaModel
		{
			get
			{
				return this._areaModel;
			}
			set
			{
				this._areaModel = value;
			}
		}

		public int FlagShipID
		{
			get
			{
				return this.FlagShipModel.MstId;
			}
		}

		public ShipModel FlagShipModel
		{
			get
			{
				if (this._clsCurrentDeck == null)
				{
					return null;
				}
				return this._clsCurrentDeck.GetFlagShip();
			}
		}

		public DeckModel CurrentDeck
		{
			get
			{
				return this._clsCurrentDeck;
			}
			set
			{
				this._clsCurrentDeck = value;
			}
		}

		public int CurrentDeckID
		{
			get
			{
				if (this._clsCurrentDeck == null)
				{
					return 1;
				}
				return this._clsCurrentDeck.Id;
			}
		}

		public int currentPlayingBgmID
		{
			get
			{
				return this._nCurrentPlayingBgmID;
			}
			set
			{
				this._nCurrentPlayingBgmID = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (AppInformation.curveDic == null)
			{
				AppInformation.curveDic = new Dictionary<AppInformation.CurveType, AnimationCurve>();
			}
			if (this.curves != null)
			{
				for (int i = 0; i < this.curves.Length; i++)
				{
					if (!AppInformation.curveDic.ContainsKey(this.curves[i].type))
					{
						AppInformation.curveDic.Add(this.curves[i].type, this.curves[i].curve);
					}
				}
			}
			this.NextLoadType = AppInformation.LoadType.Ship;
			this.NextLoadScene = Generics.Scene.Scene_BEF;
		}

		public bool IsValidMoveToScene(Generics.Scene sceneType)
		{
			if (sceneType == Generics.Scene.Repair)
			{
				int nDockMax = this._areaModel.NDockMax;
				return this.IsValidMoveToRepairScene(nDockMax);
			}
			if (sceneType != Generics.Scene.Arsenal)
			{
				return true;
			}
			int currentAreaID = this.CurrentAreaID;
			return this.IsValidMoveToArsenalScene(currentAreaID);
		}

		private bool IsValidMoveToRepairScene(int areaInDockCount)
		{
			return 0 < areaInDockCount;
		}

		private bool IsValidMoveToArsenalScene(int areaId)
		{
			int num = 1;
			return areaId == num;
		}

		public void FirstUpdateEnd()
		{
			base.StartCoroutine(this.FirstUpdateEndCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator FirstUpdateEndCoroutine()
		{
			return new AppInformation.<FirstUpdateEndCoroutine>c__Iterator54();
		}
	}
}
