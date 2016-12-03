using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIToggle)), RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(UIWidget)), RequireComponent(typeof(UIButton))]
	public class UIRebellionParticipatingFleetInfo : MonoBehaviour, IRebellionOrganizeSelectObject
	{
		[Serializable]
		private class Delta
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiBackground;

			[SerializeField]
			private UISprite _uiForeground;

			[SerializeField]
			private float _fFlasingTime = 1f;

			private bool _isFlasing;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public bool isFlasing
			{
				get
				{
					return this._isFlasing;
				}
				set
				{
					if (value)
					{
						this._isFlasing = true;
						this._uiForeground.get_transform().LTCancel();
						this._uiForeground.get_transform().LTValue(0f, 1f, this._fFlasingTime).setEase(LeanTweenType.easeInSine).setLoopPingPong().setOnUpdate(delegate(float x)
						{
							this._uiForeground.alpha = x;
						});
					}
					else
					{
						this._isFlasing = value;
						this._uiForeground.get_transform().LTCancel();
						this._uiForeground.get_transform().LTValue(this._uiForeground.alpha, 0f, 0.2f).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
						{
							this._uiForeground.alpha = x;
						});
					}
				}
			}

			public bool Init()
			{
				this._isFlasing = false;
				this._uiForeground.alpha = 0f;
				this._uiForeground.get_transform().LTCancel();
				return true;
			}

			public bool UnInit()
			{
				this._uiForeground.get_transform().LTCancel();
				Mem.Del<Transform>(ref this._tra);
				Mem.Del(ref this._uiBackground);
				Mem.Del(ref this._uiForeground);
				Mem.Del<float>(ref this._fFlasingTime);
				return true;
			}
		}

		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private UITexture _uiShipBanner;

		[SerializeField]
		private UISprite _uiFleetNum;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private UIButton _uiButton;

		[SerializeField]
		private UIToggle _uiToggle;

		[SerializeField]
		private UIRebellionParticipatingFleetInfo.Delta _clsDelta;

		private RebellionFleetType _iType;

		private DeckModel _clsDeckMode;

		private UIWidget _uiWidget;

		public RebellionFleetType type
		{
			get
			{
				return this._iType;
			}
		}

		public DeckModel deckModel
		{
			get
			{
				return this._clsDeckMode;
			}
		}

		public int index
		{
			get;
			private set;
		}

		public UIButton button
		{
			get
			{
				return this._uiButton;
			}
			private set
			{
				this._uiButton = value;
			}
		}

		public UIToggle toggle
		{
			get
			{
				return this._uiToggle;
			}
			private set
			{
				this._uiToggle = value;
			}
		}

		public DelDicideRebellionOrganizeSelectBtn delDicideRebellionOrganizeSelectBtn
		{
			get;
			private set;
		}

		public bool isFlagShipExists
		{
			get
			{
				return this.deckModel != null && this.deckModel.GetFlagShip() != null;
			}
		}

		public UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		public static UIRebellionParticipatingFleetInfo Instantiate(UIRebellionParticipatingFleetInfo prefab, Transform parent, Vector3 pos)
		{
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo = Object.Instantiate<UIRebellionParticipatingFleetInfo>(prefab);
			uIRebellionParticipatingFleetInfo.get_transform().set_parent(parent);
			uIRebellionParticipatingFleetInfo.get_transform().localScaleOne();
			uIRebellionParticipatingFleetInfo.get_transform().set_localPosition(pos);
			uIRebellionParticipatingFleetInfo.Setup();
			return uIRebellionParticipatingFleetInfo;
		}

		private bool Setup()
		{
			if (this._uiBackground == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiBackground, base.get_transform(), "Background");
			}
			if (this._uiShipBanner == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiShipBanner, base.get_transform(), "Banner");
			}
			if (this._uiFleetNum == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiFleetNum, base.get_transform(), "FleetNum");
			}
			if (this._uiLabel == null)
			{
				Util.FindParentToChild<UILabel>(ref this._uiLabel, base.get_transform(), "Label");
			}
			if (this.button == null)
			{
				base.GetComponent<UIButton>();
			}
			this._uiShipBanner.localSize = ResourceManager.SHIP_TEXTURE_SIZE.get_Item(1);
			this._uiButton.onClick = Util.CreateEventDelegateList(this, "Decide", null);
			this._clsDelta.Init();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiShipBanner);
			Mem.Del(ref this._uiFleetNum);
			Mem.Del<UILabel>(ref this._uiLabel);
			Mem.Del<UIButton>(ref this._uiButton);
			Mem.Del<UIToggle>(ref this._uiToggle);
			this._clsDelta.UnInit();
			Mem.Del<UIRebellionParticipatingFleetInfo.Delta>(ref this._clsDelta);
			Mem.Del<RebellionFleetType>(ref this._iType);
			Mem.Del<UIRebellionParticipatingFleetInfo.Delta>(ref this._clsDelta);
			Mem.Del<UIWidget>(ref this._uiWidget);
		}

		public bool Init(RebellionFleetType iType, DelDicideRebellionOrganizeSelectBtn decideDelegate)
		{
			this._iType = iType;
			this._uiLabel.text = this.GetLabelString(iType);
			this.index = (int)iType;
			this.SetFleetInfo(null);
			this.delDicideRebellionOrganizeSelectBtn = decideDelegate;
			this._clsDelta.isFlasing = false;
			this.SetScale(iType);
			return true;
		}

		private void SetScale(RebellionFleetType iType)
		{
			base.get_transform().set_localScale((iType != RebellionFleetType.DecisiveBattleSupportFleet && iType != RebellionFleetType.VanguardSupportFleet) ? Vector3.get_one() : (Vector3.get_one() * 0.87f));
			this._clsDelta.transform.set_localScale((iType != RebellionFleetType.DecisiveBattleSupportFleet && iType != RebellionFleetType.VanguardSupportFleet) ? Vector3.get_one() : (Vector3.get_one() * 1.13f));
			this._uiLabel.get_transform().set_localScale((iType != RebellionFleetType.DecisiveBattleSupportFleet && iType != RebellionFleetType.VanguardSupportFleet) ? Vector3.get_one() : (Vector3.get_one() * 1.13f));
		}

		public void SetFleetInfo(DeckModel model)
		{
			this._clsDeckMode = model;
			if (model == null)
			{
				this._uiShipBanner.mainTexture = null;
				this._uiFleetNum.spriteName = string.Empty;
				this._clsDelta.isFlasing = false;
				return;
			}
			ShipModel flagShip = model.GetFlagShip();
			this._uiShipBanner.mainTexture = ShipUtils.LoadBannerTexture(flagShip);
			this._uiFleetNum.spriteName = string.Format("icon_deck{0}", model.Id);
			this._clsDelta.isFlasing = true;
		}

		private string GetLabelString(RebellionFleetType iType)
		{
			switch (iType)
			{
			case RebellionFleetType.VanguardFleet:
				return "前衛艦隊";
			case RebellionFleetType.VanguardSupportFleet:
				return "前衛支援艦隊";
			case RebellionFleetType.DecisiveBattlePrimaryFleet:
				return "決戦主力艦隊";
			case RebellionFleetType.DecisiveBattleSupportFleet:
				return "決戦支援艦隊";
			default:
				return string.Empty;
			}
		}

		public void Decide()
		{
			DebugUtils.Log("UIRebellionFleetInfo", base.get_gameObject().get_name());
			if (this.delDicideRebellionOrganizeSelectBtn != null)
			{
				this.delDicideRebellionOrganizeSelectBtn(this);
			}
		}
	}
}
