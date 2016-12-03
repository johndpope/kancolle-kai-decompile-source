using Common.Enum;
using DG.Tweening;
using KCV.Display;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Supply
{
	public class SupplyMainManager : MonoBehaviour, CommonDeckSwitchHandler
	{
		public enum ScreenStatus
		{
			SHIP_SELECT,
			RIGHT_PAIN_WORK,
			SUPPLY_PROCESS,
			SHIP_RECOVERY_ANIMATION
		}

		public SupplyManager SupplyManager;

		[SerializeField]
		private Texture[] mPreloads_Texture;

		[SerializeField]
		private UIShipSortButton mShipSortButton;

		[SerializeField]
		public ShipBannerContainer _shipBannerContainer;

		[SerializeField]
		public OtherShipListScrollNew _otherListParent;

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		[SerializeField]
		private UILabel _deckName;

		[SerializeField]
		private UITexture _bauxiteMsgSuccess;

		[SerializeField]
		private UITexture _bauxiteMsgIncomplete;

		[SerializeField]
		private Transform _mTrans_TurnEndStamp;

		[SerializeField]
		private SupplyRightPane _rightPane;

		[SerializeField]
		private CommonDeckSwitchManager _commonDeckSwitchManager;

		private bool _isControllDone;

		private DeckModel[] _decks;

		private DeckModel _currentDeck;

		private SupplyMainManager.ScreenStatus _status;

		public static SupplyMainManager Instance
		{
			get;
			private set;
		}

		public KeyControl KeyController
		{
			get;
			private set;
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			SupplyMainManager.<Start>c__IteratorCA <Start>c__IteratorCA = new SupplyMainManager.<Start>c__IteratorCA();
			<Start>c__IteratorCA.<>f__this = this;
			return <Start>c__IteratorCA;
		}

		public bool isNowDeckIsOther()
		{
			return this._commonDeckSwitchManager.currentDeck == null;
		}

		public bool isNowRightFocus()
		{
			return this._status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK;
		}

		private void OnSwipeListener(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				float num = 0.1f;
				if (num < Math.Abs(movePercentageX))
				{
					if (0f < movePercentageX)
					{
						SupplyMainManager.ScreenStatus status = this._status;
						if (status == SupplyMainManager.ScreenStatus.SHIP_SELECT)
						{
							this._commonDeckSwitchManager.ChangePrevDeck();
						}
					}
					else
					{
						SupplyMainManager.ScreenStatus status = this._status;
						if (status == SupplyMainManager.ScreenStatus.SHIP_SELECT)
						{
							this._commonDeckSwitchManager.ChangeNextDeck();
						}
					}
				}
			}
		}

		public void UpdateSupplyManager()
		{
			this.SupplyManager = new SupplyManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
		}

		public KeyControl GetKeyController()
		{
			if (this.KeyController != null)
			{
				this.KeyController.Update();
			}
			return this.KeyController;
		}

		public void SetControllDone(bool enabled)
		{
			this._isControllDone = enabled;
		}

		public void Update()
		{
			if (this.GetKeyController() == null)
			{
				return;
			}
			if (this._isControllDone)
			{
				return;
			}
			if (this.KeyController.IsUpDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						this._rightPane.SelectButtonLengthwise(true);
					}
				}
				else
				{
					this._shipBannerContainer.SelectLengthwise(true);
				}
			}
			if (this.KeyController.IsDownDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						this._rightPane.SelectButtonLengthwise(false);
					}
				}
				else
				{
					this._shipBannerContainer.SelectLengthwise(false);
				}
			}
			if (this.KeyController.IsLeftDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						this._rightPane.SelectButtonHorizontal(true);
					}
				}
			}
			if (this.KeyController.IsRightDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						this._rightPane.SelectButtonHorizontal(false);
					}
				}
				else
				{
					bool flag = this.change_2_RIGHT_PAIN_WORK(true);
					if (flag)
					{
						this._otherListParent.LockControl();
					}
				}
			}
			else if (this.KeyController.IsMaruDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						this._rightPane.DisideButton();
					}
				}
				else
				{
					this._shipBannerContainer.SwitchCurrentSelected();
				}
			}
			else if (this.KeyController.IsBatuDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						this.change_2_SHIP_SELECT(true);
					}
				}
				else
				{
					this.backPortTop();
				}
			}
			else if (this.KeyController.IsShikakuDown())
			{
				SupplyMainManager.ScreenStatus status = this._status;
				if (status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
				{
					if (status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
					{
						if (this.isNotOther())
						{
							this.change_2_SHIP_SELECT(false);
							this._shipBannerContainer.SwitchAllSelected();
						}
					}
				}
				else if (this.isNotOther())
				{
					this._shipBannerContainer.SwitchAllSelected();
					bool flag2 = this.change_2_RIGHT_PAIN_WORK(true);
					if (flag2)
					{
						this._otherListParent.LockControl();
					}
				}
			}
			else if (this.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}

		private bool change_2_RIGHT_PAIN_WORK(bool defaultFocus)
		{
			if (this._status != SupplyMainManager.ScreenStatus.SHIP_SELECT)
			{
				return false;
			}
			if (!this._rightPane.IsFocusable())
			{
				return false;
			}
			this.SetShipSelectFocus(false);
			if (defaultFocus)
			{
				this._rightPane.setFocus();
			}
			this._status = SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK;
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			return true;
		}

		public void change_2_SHIP_SELECT(bool defaultFocus)
		{
			if (this._status != SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK && this._status != SupplyMainManager.ScreenStatus.SHIP_RECOVERY_ANIMATION)
			{
				return;
			}
			this._status = SupplyMainManager.ScreenStatus.SHIP_SELECT;
			this.SetShipSelectFocus(true);
			this._rightPane.LostFocus();
			if (!this.isNotOther())
			{
				this._otherListParent.StartControl();
			}
		}

		public void change_2_SUPPLY_PROCESS(SupplyType supplyType)
		{
			if (this._status != SupplyMainManager.ScreenStatus.SHIP_SELECT && this._status != SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
			{
				return;
			}
			this._status = SupplyMainManager.ScreenStatus.SUPPLY_PROCESS;
			this._otherListParent.LockControl();
			this._commonDeckSwitchManager.keyControlEnable = false;
			if (supplyType == SupplyType.All)
			{
				ShipModel model;
				if (this.isNotOther() && this._currentDeck.GetShipCount() == this.getSelectedShipList().get_Count())
				{
					int mstId = this._currentDeck.GetFlagShip().MstId;
					model = this._currentDeck.GetFlagShip();
				}
				else
				{
					List<ShipModel> selectedShipList = this.getSelectedShipList();
					int num = Random.Range(0, selectedShipList.get_Count());
					int mstId = selectedShipList.get_Item(num).MstId;
					model = selectedShipList.get_Item(num);
				}
				ShipUtils.PlayShipVoice(model, 27);
			}
			this.SetShipSelectFocus(false);
			this._rightPane.LostFocus();
			this._rightPane.SetEnabled(false);
			this._rightPane.DoWindowOpenAnimation(supplyType);
			this.Supply(supplyType);
		}

		public void change_2_SHIP_RECOVERY_ANIMATION()
		{
			if (this._status != SupplyMainManager.ScreenStatus.SUPPLY_PROCESS)
			{
				return;
			}
			this._status = SupplyMainManager.ScreenStatus.SHIP_RECOVERY_ANIMATION;
			if (this.isNotOther())
			{
				this._shipBannerContainer.ProcessRecoveryAnimation();
			}
			else
			{
				this._otherListParent.ProcessRecoveryAnimation();
			}
			this._rightPane.DoWindowCloseAnimation();
		}

		public void ProcessSupplyFinished()
		{
			if (this._status != SupplyMainManager.ScreenStatus.SHIP_RECOVERY_ANIMATION)
			{
				return;
			}
			this._rightPane.SetEnabled(true);
			this.change_2_SHIP_SELECT(true);
			this.OnDeckChange(this._currentDeck);
			if (!this.isNotOther() && this._otherListParent.GetShipCount() == 0)
			{
				this._commonDeckSwitchManager.Init(this.SupplyManager, this._decks, this, this.KeyController, true);
			}
			this._commonDeckSwitchManager.keyControlEnable = true;
		}

		public bool IsShipSelectableStatus()
		{
			return this._status == SupplyMainManager.ScreenStatus.SHIP_SELECT || this._status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK;
		}

		private void backPortTop()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void OnDeckChange(DeckModel deck)
		{
			this._currentDeck = deck;
			if (this._currentDeck != null && this._currentDeck.IsActionEnd())
			{
				this._mTrans_TurnEndStamp.SetActive(true);
				ShortcutExtensions.DOKill(this._mTrans_TurnEndStamp, false);
				ShortcutExtensions.DOLocalRotate(this._mTrans_TurnEndStamp, new Vector3(0f, 0f, 300f), 0f, 1);
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(this._mTrans_TurnEndStamp, new Vector3(0f, 0f, 360f), 0.8f, 1), 30);
			}
			else
			{
				this._mTrans_TurnEndStamp.SetActive(false);
			}
			if (this.isNotOther())
			{
				this.mShipSortButton.SetActive(false);
				this.SupplyManager.InitForDeck(this._currentDeck.Id);
				this._deckName.text = deck.Name;
				this._mTrans_TurnEndStamp.get_transform().localPositionX(this._deckName.get_transform().get_localPosition().x + this._deckName.printedSize.x + 20f);
				this._shipBannerContainer.Show(true);
				this._shipBannerContainer.InitDeck(deck);
				this._otherListParent.Hide(true);
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = this._currentDeck;
			}
			else
			{
				this.mShipSortButton.SetActive(true);
				this.SupplyManager.InitForOther();
				this._deckName.text = "その他";
				this._shipBannerContainer.Hide(true);
				this._otherListParent.MemoryNextFocus();
				this._otherListParent.Initialize(this.KeyController, this.SupplyManager.Ships);
				this._otherListParent.Show(true);
				this._otherListParent.StartState();
			}
			this.UpdateRightPain();
			if (this._status == SupplyMainManager.ScreenStatus.RIGHT_PAIN_WORK)
			{
				this.change_2_SHIP_SELECT(true);
			}
		}

		private List<ShipModel> getSelectedShipList()
		{
			return (!this.isNotOther()) ? this._otherListParent.getSeletedModelList() : this._shipBannerContainer.getSeletedModelList();
		}

		public void UpdateRightPain()
		{
			this._rightPane.Refresh();
		}

		private void SetShipSelectFocus(bool focused)
		{
			if (this.isNotOther())
			{
				this._shipBannerContainer.SetFocus(focused);
			}
		}

		private bool isNotOther()
		{
			return this._currentDeck != null;
		}

		private void Supply(SupplyType supplyType)
		{
			bool flag;
			if (this.SupplyManager.Supply(supplyType, out flag))
			{
				if (flag)
				{
					this.animateBauxite(this._bauxiteMsgSuccess);
				}
			}
			else if (flag)
			{
				this.animateBauxite(this._bauxiteMsgIncomplete);
			}
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.SupplyManager);
			SupplyMainManager.Instance.UpdateSupplyManager();
		}

		public bool IsDeckSelectable(int index, DeckModel deck)
		{
			if (deck == null)
			{
				return this._otherListParent.GetShipCount() > 0;
			}
			return deck.GetShipCount() > 0;
		}

		private void animateBauxite(UITexture texture)
		{
			texture.GetComponent<Animation>().Play("SupplyBauxiteMessage");
		}

		private void OnDestroy()
		{
			for (int i = 0; i < this.mPreloads_Texture.Length; i++)
			{
				this.mPreloads_Texture[i] = null;
			}
			this.mPreloads_Texture = null;
			SupplyMainManager.Instance = null;
			this.SupplyManager = null;
			this.KeyController = null;
			this._shipBannerContainer = null;
			this._otherListParent = null;
			this._deckName = null;
			this._bauxiteMsgSuccess = null;
			this._bauxiteMsgIncomplete = null;
			this._mTrans_TurnEndStamp = null;
			this._rightPane = null;
			this._commonDeckSwitchManager = null;
			this._decks = null;
			this._currentDeck = null;
		}
	}
}
