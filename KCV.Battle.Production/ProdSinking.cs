using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdSinking : BaseAnimation
	{
		public enum SinkingType
		{
			None,
			ProdSinkingRepairGoddess,
			ProdSinkingRepairTeam,
			ProdSinking
		}

		[SerializeField]
		private UISprite _uiLightLine;

		[SerializeField]
		private UISprite _uiMask;

		[SerializeField]
		private UISprite _uiRipple;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiBrightSpot;

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private UITexture _uiRepairCard;

		[SerializeField]
		private UITexture _uiShipTexture;

		[SerializeField]
		private ParticleSystem _psSinkingSmoke;

		[SerializeField]
		private List<UISprite> _listDrops;

		[SerializeField]
		private List<UISprite> _listLostMessages;

		private Action _actOnRestore;

		private UIPanel _uiPanel;

		private List<Vector3> _listShipOffs;

		private List<Texture2D> _listShipTexture;

		private ShipModel_Defender _clsShipModel;

		private ProdSinking.SinkingType _iType;

		public ShipModel_Defender shipModel
		{
			get
			{
				return this._clsShipModel;
			}
		}

		public ProdSinking.SinkingType sinkingType
		{
			get
			{
				return this._iType;
			}
		}

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
		}

		private bool isRepair
		{
			get
			{
				return this._iType == ProdSinking.SinkingType.ProdSinkingRepairGoddess || this._iType == ProdSinking.SinkingType.ProdSinkingRepairTeam;
			}
		}

		public static ProdSinking Instantiate(ProdSinking prefab, Transform parent)
		{
			ProdSinking prodSinking = Object.Instantiate<ProdSinking>(prefab);
			prodSinking.get_transform().set_parent(parent);
			prodSinking.get_transform().localScaleZero();
			prodSinking.get_transform().localPositionZero();
			return prodSinking;
		}

		protected override void Awake()
		{
			base.Awake();
			this._iType = ProdSinking.SinkingType.None;
			this._listShipTexture = new List<Texture2D>(2);
			this._listShipOffs = new List<Vector3>(2);
			this.panel.widgetsAreStatic = true;
			this._psSinkingSmoke.SetActive(false);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref this._uiLightLine);
			Mem.Del(ref this._uiMask);
			Mem.Del(ref this._uiRipple);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiBrightSpot);
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.Del<UITexture>(ref this._uiRepairCard);
			Mem.Del<UITexture>(ref this._uiShipTexture);
			Mem.Del(ref this._psSinkingSmoke);
			if (this._listDrops != null)
			{
				this._listDrops.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe<UISprite>(ref this._listDrops);
			if (this._listLostMessages != null)
			{
				this._listLostMessages.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe<UISprite>(ref this._listLostMessages);
			Mem.Del<Action>(ref this._actOnRestore);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelListSafe<Vector3>(ref this._listShipOffs);
			Mem.DelListSafe<Texture2D>(ref this._listShipTexture);
			Mem.Del<ShipModel_Defender>(ref this._clsShipModel);
			Mem.Del<ProdSinking.SinkingType>(ref this._iType);
		}

		public void SetSinkingData(ShipModel_Defender ship)
		{
			this._clsShipModel = ship;
			this._iType = this.GetSinkingType(ship);
			this._listShipTexture = KCV.Battle.Utils.ShipUtils.LoadTexture2Sinking(ship, this.isRepair);
			this._listShipOffs = KCV.Battle.Utils.ShipUtils.GetShipOffsPos2Sinking(ship, this.isRepair, MstShipGraphColumn.CutInSp1);
			this._uiShipTexture.mainTexture = this._listShipTexture.get_Item(0);
			this._uiShipTexture.MakePixelPerfect();
			this._uiShipTexture.get_transform().set_localPosition(this._listShipOffs.get_Item(0));
		}

		private void SetRepairCard(ProdSinking.SinkingType iType)
		{
			switch (iType)
			{
			case ProdSinking.SinkingType.None:
			case ProdSinking.SinkingType.ProdSinking:
				this._uiRepairCard.mainTexture = null;
				this._uiRepairCard.localSize = Vector3.get_zero();
				break;
			case ProdSinking.SinkingType.ProdSinkingRepairGoddess:
				this._uiRepairCard.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(43, 1);
				this._uiRepairCard.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(1);
				break;
			case ProdSinking.SinkingType.ProdSinkingRepairTeam:
				this._uiRepairCard.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(42, 1);
				this._uiRepairCard.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(1);
				break;
			}
		}

		public override void Play(Action callback)
		{
			this.Play(null, null, callback);
		}

		public void Play(Action onStart, Action onRestore, Action onFinished)
		{
			if (this._iType == ProdSinking.SinkingType.None)
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
				return;
			}
			this._actOnRestore = onRestore;
			base.get_transform().localScaleOne();
			this.SetRepairCard(this._iType);
			this.panel.widgetsAreStatic = false;
			base.Play(this._iType, onFinished);
			Dlg.Call(ref onStart);
		}

		private ProdSinking.SinkingType GetSinkingType(ShipModel_Defender model)
		{
			switch (model.DamageEventAfter)
			{
			case DamagedStates.Gekichin:
				return ProdSinking.SinkingType.ProdSinking;
			case DamagedStates.Youin:
				return ProdSinking.SinkingType.ProdSinkingRepairTeam;
			case DamagedStates.Megami:
				return ProdSinking.SinkingType.ProdSinkingRepairGoddess;
			default:
				return ProdSinking.SinkingType.None;
			}
		}

		private void PlaySmoke()
		{
			this._psSinkingSmoke.SetActive(true);
			this._psSinkingSmoke.Play();
		}

		private void PlaySinkingVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlaySinkingVoice(this._clsShipModel);
		}

		private void RestoredShip()
		{
			this._uiShipTexture.mainTexture = this._listShipTexture.get_Item(1);
			this._uiShipTexture.MakePixelPerfect();
			this._uiShipTexture.get_transform().set_localPosition(this._listShipOffs.get_Item(1));
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.Restored(this._clsShipModel);
			Dlg.Call(ref this._actOnRestore);
		}

		protected override void onAnimationFinished()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				this._psSinkingSmoke.SetActive(false);
				base.get_transform().localScaleZero();
				this.panel.widgetsAreStatic = true;
			});
			base.onAnimationFinished();
		}

		private void PlayDamageSE()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_053);
		}

		private void CheckTrophy()
		{
			BattleTaskManager.GetBattleManager().IncrementRecoveryItemCountWithTrophyUnlock();
		}
	}
}
