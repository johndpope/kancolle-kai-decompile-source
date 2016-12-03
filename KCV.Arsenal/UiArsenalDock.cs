using Common.Enum;
using KCV.PopupString;
using KCV.Production;
using KCV.Utils;
using local.models;
using local.utils;
using Server_Common;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDock : MonoBehaviour
	{
		private enum DockMode
		{
			Close,
			Show,
			Lock
		}

		[SerializeField]
		private UITexture _uiBg;

		[SerializeField]
		private UITexture _uiBg2;

		[SerializeField]
		private UIButton _uiStartBtn;

		[SerializeField]
		private UIButton _uiGetBtn;

		[SerializeField]
		private UIButton _lockBtn;

		[SerializeField]
		private UILabel _uiTurnLabel;

		[SerializeField]
		private GameObject _lockObj;

		[SerializeField]
		private GameObject _lockFrameObj;

		[SerializeField]
		private Animation _lockAnim;

		[SerializeField]
		private ButtonLightTexture btnLight;

		private UISprite _uiStartBtnSprite;

		private UISprite _uiHighBtnSprite;

		private UISprite _uiGetBtnSprite;

		private UISprite _lockBtnSprite;

		private int _number;

		private int _limit;

		private bool _isCreate;

		private Coroutine cor;

		private ShipModelMst _ship;

		private BuildDockModel _dock;

		private ProdReceiveShip _prodReceiveShip;

		private UiArsenalDockShipManager _dockShipManager;

		private TaskMainArsenalManager taskMainArsenalManager;

		private IReward_Ship _rewardShip;

		private UiArsenalDock.DockMode dockMode;

		[SerializeField]
		public UIButton _uiHighBtn;

		public UITexture _uiLockedDockL;

		public UITexture _uiLockedDockR;

		public bool IsFirstHight;

		public bool IsShowHigh;

		public bool IsHight;

		public bool IsHightEnd;

		public bool IsLarge;

		public bool isCompleteVoicePlayable;

		public UiArsenalDockMini _dockMiniMamager;

		public UiArsenalShipManager _shipSManager;

		public bool init(TaskMainArsenalManager taskMainArsenalManager, int num)
		{
			this.taskMainArsenalManager = taskMainArsenalManager;
			this._number = num;
			this._limit = 0;
			this.IsLarge = false;
			this.IsFirstHight = false;
			this.IsShowHigh = false;
			this.IsHight = false;
			this.IsHightEnd = false;
			this.isCompleteVoicePlayable = false;
			this.dockMode = UiArsenalDock.DockMode.Close;
			Util.FindParentToChild<UITexture>(ref this._uiBg, base.get_transform(), "Bg");
			Util.FindParentToChild<UITexture>(ref this._uiBg2, base.get_transform(), "Bg2");
			Util.FindParentToChild<UIButton>(ref this._uiStartBtn, base.get_transform(), "ButtonStart");
			Util.FindParentToChild<UISprite>(ref this._uiStartBtnSprite, this._uiStartBtn.get_transform(), "Background");
			Util.FindParentToChild<UIButton>(ref this._uiGetBtn, base.get_transform(), "ButtonGet");
			Util.FindParentToChild<UISprite>(ref this._uiGetBtnSprite, this._uiGetBtn.get_transform(), "Background");
			Util.FindParentToChild<UIButton>(ref this._uiHighBtn, base.get_transform(), "ButtonHight");
			Util.FindParentToChild<UISprite>(ref this._uiHighBtnSprite, this._uiHighBtn.get_transform(), "Background");
			Util.FindParentToChild<UILabel>(ref this._uiTurnLabel, base.get_transform(), "LabelTurn");
			if (this._lockObj == null)
			{
				this._lockObj = base.get_transform().FindChild("LockObj").get_gameObject();
			}
			if (this._lockFrameObj == null)
			{
				this._lockFrameObj = this._lockObj.get_transform().FindChild("FrameObject").get_gameObject();
			}
			Util.FindParentToChild<UIButton>(ref this._lockBtn, this._lockObj.get_transform(), "LockButton");
			Util.FindParentToChild<UISprite>(ref this._lockBtnSprite, this._lockBtn.get_transform(), "Background");
			Util.FindParentToChild<Animation>(ref this._lockAnim, base.get_transform(), "LockBtn");
			Util.FindParentToChild<UITexture>(ref this._uiLockedDockL, this._lockObj.get_transform(), "FrameObject/LockFrameL");
			Util.FindParentToChild<UITexture>(ref this._uiLockedDockR, this._lockObj.get_transform(), "FrameObject/LockFrameR");
			Util.FindParentToChild<UiArsenalDockMini>(ref this._dockMiniMamager, this._uiBg2.get_transform(), "Panel");
			Util.FindParentToChild<UiArsenalShipManager>(ref this._shipSManager, this._uiBg2.get_transform(), "Panel/ShipManager");
			Util.FindParentToChild<ButtonLightTexture>(ref this.btnLight, this._uiHighBtn.get_transform(), "ButtonLight");
			this._dockMiniMamager.init(this._number);
			this._shipSManager.init(this._number);
			this._close();
			this._isCreate = true;
			return true;
		}

		private void OnDestroy()
		{
			if (this.cor != null)
			{
				base.StopCoroutine(this.cor);
			}
			this.cor = null;
			Mem.Del<UITexture>(ref this._uiBg);
			Mem.Del<UITexture>(ref this._uiBg2);
			Mem.Del<UIButton>(ref this._uiStartBtn);
			Mem.Del<UIButton>(ref this._uiGetBtn);
			Mem.Del<UIButton>(ref this._lockBtn);
			Mem.Del<UILabel>(ref this._uiTurnLabel);
			Mem.Del<GameObject>(ref this._lockObj);
			Mem.Del<GameObject>(ref this._lockFrameObj);
			Mem.Del<Animation>(ref this._lockAnim);
			Mem.Del<ButtonLightTexture>(ref this.btnLight);
			Mem.Del(ref this._uiStartBtnSprite);
			Mem.Del(ref this._uiHighBtnSprite);
			Mem.Del(ref this._uiGetBtnSprite);
			Mem.Del(ref this._lockBtnSprite);
			Mem.Del<Coroutine>(ref this.cor);
			Mem.Del<ShipModelMst>(ref this._ship);
			Mem.Del<BuildDockModel>(ref this._dock);
			Mem.Del<ProdReceiveShip>(ref this._prodReceiveShip);
			Mem.Del<UiArsenalDockShipManager>(ref this._dockShipManager);
			Mem.Del<IReward_Ship>(ref this._rewardShip);
			Mem.Del<UiArsenalDock.DockMode>(ref this.dockMode);
			Mem.Del<UIButton>(ref this._uiHighBtn);
			Mem.Del<UITexture>(ref this._uiLockedDockL);
			Mem.Del<UITexture>(ref this._uiLockedDockR);
			Mem.Del<UiArsenalDockMini>(ref this._dockMiniMamager);
			Mem.Del<UiArsenalShipManager>(ref this._shipSManager);
		}

		public bool SelectDockMode()
		{
			return this.dockMode != UiArsenalDock.DockMode.Close;
		}

		public bool GetLockDockMode()
		{
			return this.dockMode == UiArsenalDock.DockMode.Lock;
		}

		public KdockStates GetDockState()
		{
			return this._dock.State;
		}

		public void DisableParticles()
		{
			if (this._dockMiniMamager != null)
			{
				this._dockMiniMamager.DisableParticles();
			}
		}

		public void EnableParticles()
		{
			if (this._dockMiniMamager != null)
			{
				this._dockMiniMamager.EnableParticles();
			}
		}

		public void _close()
		{
			this.dockMode = UiArsenalDock.DockMode.Close;
			this._uiBg.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_bg_none") as Texture2D);
			this._uiBg2.alpha = 0f;
			this._uiGetBtn.get_transform().set_localScale(Vector3.get_zero());
			this._uiStartBtn.get_transform().set_localScale(Vector3.get_zero());
			this._uiHighBtn.get_transform().set_localScale(Vector3.get_zero());
			this._uiTurnLabel.alpha = 0f;
			this._lockObj.SetActive(true);
			this._lockFrameObj.SetActive(true);
			this._lockBtn.get_transform().set_localScale(Vector3.get_zero());
			this.btnLight.StopAnim();
		}

		public void ShowKeyLock()
		{
			this.dockMode = UiArsenalDock.DockMode.Lock;
			this._lockObj.SetActive(true);
			this._lockFrameObj.SetActive(true);
			this._lockBtn.get_transform().set_localScale(Vector3.get_one());
			this._lockBtnSprite.spriteName = "btn_addDock";
		}

		private bool isDockOpenEnable()
		{
			return 0 < TaskMainArsenalManager.arsenalManager.NumOfKeyPossessions;
		}

		public void HideKeyLock()
		{
			this._lockObj.SetActive(false);
		}

		public void _setShow()
		{
			this._setShow(false);
		}

		public void _setShow(bool DockOpen)
		{
			this._dock = TaskMainArsenalManager.arsenalManager.GetDock(this._number + 1);
			this._close();
			if (this.dockMode == UiArsenalDock.DockMode.Close)
			{
				this.dockMode = UiArsenalDock.DockMode.Show;
			}
			this._uiBg.alpha = 1f;
			this._uiBg2.alpha = 1f;
			this._uiBg.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_bg_1") as Texture2D);
			if (!DockOpen && this.dockMode != UiArsenalDock.DockMode.Close)
			{
				this._lockObj.SetActive(false);
			}
			this.updateSpeedUpIcon();
			if (this._dock.IsLarge())
			{
				this._uiBg2.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_build2_bg") as Texture2D);
			}
			else if (this._dock.IsTunker())
			{
				this._uiBg2.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_build3_bg") as Texture2D);
			}
			else
			{
				this._uiBg2.mainTexture = (Resources.Load("Textures/Arsenal/dock/kenzo_build1_bg") as Texture2D);
			}
			if (this._dock.State == KdockStates.COMPLETE)
			{
				this._ship = this._dock.Ship;
				if (this.IsHight)
				{
					this._shipSManager.set(this._ship, this._dock, true);
					this._uiGetBtn.get_transform().set_localScale(Vector3.get_zero());
					this._uiHighBtn.get_transform().set_localScale(Vector3.get_one());
					this._uiTurnLabel.alpha = 1f;
					this._limit = this._dock.GetTurn();
					this._uiTurnLabel.text = string.Empty + this._limit.ToString();
					if (this.IsFirstHight)
					{
						this.PlayFirstHightAnimate();
					}
					else
					{
						this.StartSpeedUpAnimate();
					}
				}
				else
				{
					this._shipSManager.set(this._ship, this._dock, false);
					this.endConstruct();
					if (this.IsHightEnd)
					{
						this._dockMiniMamager.PlayEndHightAnimate();
					}
					else
					{
						this._dockMiniMamager.PlayConstCompAnimation();
					}
				}
			}
			else if (this._dock.State == KdockStates.CREATE)
			{
				this._ship = this._dock.Ship;
				this._shipSManager.set(this._ship, this._dock, false);
				this._uiGetBtn.get_transform().set_localScale(Vector3.get_zero());
				this._uiHighBtn.get_transform().set_localScale(Vector3.get_one());
				this._uiTurnLabel.alpha = 1f;
				this._limit = this._dock.GetTurn();
				this._uiTurnLabel.text = string.Empty + this._limit.ToString();
				this._dockMiniMamager.PlayConstStartAnimation();
			}
			else
			{
				this._dockMiniMamager.StopConstAnimation();
				this._dockMiniMamager.PlayIdleAnimation();
				this._uiStartBtn.get_transform().set_localScale(Vector3.get_one());
			}
		}

		public void endConstruct()
		{
			this._limit = 0;
			this._uiGetBtn.get_transform().set_localScale(Vector3.get_one());
			this._uiBg2.alpha = 1f;
			this._uiHighBtn.get_transform().set_localScale(Vector3.get_zero());
			this.btnLight.StopAnim();
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			ShipModel shipModel = (currentDeck == null) ? new ShipModel(1) : currentDeck.GetFlagShip();
			if (this.cor == null && !this.taskMainArsenalManager.IsShipGetViewAllDock())
			{
				this.cor = base.StartCoroutine(this.PlayCompleteVoiceWaitForFlagOn(shipModel));
			}
		}

		[DebuggerHidden]
		private IEnumerator PlayCompleteVoiceWaitForFlagOn(ShipModel shipModel)
		{
			UiArsenalDock.<PlayCompleteVoiceWaitForFlagOn>c__Iterator75 <PlayCompleteVoiceWaitForFlagOn>c__Iterator = new UiArsenalDock.<PlayCompleteVoiceWaitForFlagOn>c__Iterator75();
			<PlayCompleteVoiceWaitForFlagOn>c__Iterator.shipModel = shipModel;
			<PlayCompleteVoiceWaitForFlagOn>c__Iterator.<$>shipModel = shipModel;
			<PlayCompleteVoiceWaitForFlagOn>c__Iterator.<>f__this = this;
			return <PlayCompleteVoiceWaitForFlagOn>c__Iterator;
		}

		public void SetFirstHight()
		{
			this.IsHight = true;
			this.IsFirstHight = true;
		}

		public void setSelect(bool select)
		{
			if (select)
			{
				this._lockBtnSprite.spriteName = "btn_addDock_on";
				UISelectedObject.SelectedOneObjectBlink(this._uiBg.get_gameObject(), true);
				UISelectedObject.SelectedOneObjectBlink(this._uiLockedDockL.get_gameObject(), true);
				UISelectedObject.SelectedOneObjectBlink(this._uiLockedDockR.get_gameObject(), true);
				this._uiStartBtnSprite.spriteName = "btn_build_on";
				this._uiGetBtnSprite.spriteName = "btn_get_on";
				if (!this.IsHight)
				{
					if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(this._number + 1))
					{
						this._uiHighBtnSprite.spriteName = "btn_item_on";
					}
					else
					{
						this._uiHighBtnSprite.spriteName = "btn_item_off";
						this.btnLight.StopAnim();
					}
				}
				else
				{
					this._uiHighBtnSprite.spriteName = "btn_item_off";
				}
			}
			else
			{
				this._lockBtnSprite.spriteName = "btn_addDock";
				UISelectedObject.SelectedOneObjectBlink(this._uiBg.get_gameObject(), false);
				UISelectedObject.SelectedOneObjectBlink(this._uiLockedDockL.get_gameObject(), false);
				UISelectedObject.SelectedOneObjectBlink(this._uiLockedDockR.get_gameObject(), false);
				this._uiStartBtnSprite.spriteName = "btn_build";
				this._uiGetBtnSprite.spriteName = "btn_get";
				if (!this.IsHight)
				{
					if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(this._number + 1))
					{
						this._uiHighBtnSprite.spriteName = "btn_item";
					}
					else
					{
						this._uiHighBtnSprite.spriteName = "btn_item_off";
						this.btnLight.StopAnim();
					}
				}
				else
				{
					this._uiHighBtnSprite.spriteName = "btn_item_off";
				}
			}
		}

		public void PlayFirstHightAnimate()
		{
			this.IsHight = true;
			this.IsShowHigh = false;
			this._dockMiniMamager.PlayFirstHighAnimation();
		}

		public void StartSpeedUpAnimate()
		{
			this.IsHight = true;
			this.IsShowHigh = false;
			this._dockMiniMamager.PlayHalfwayHightAnimation();
		}

		public void endSpeedUpAnimate()
		{
			this.IsShowHigh = false;
			this.IsHightEnd = true;
			this.IsHight = false;
			this._setShow();
		}

		public void updateSpeedUpIcon()
		{
			bool flag = TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(this._number + 1);
			if (flag)
			{
				this._uiHighBtnSprite.spriteName = "btn_item_on";
				if (!this.IsHight && !this.IsShowHigh)
				{
					UISelectedObject.SelectedOneButtonZoomUpDown(this._uiHighBtn.get_gameObject(), false);
					this.btnLight.PlayAnim();
				}
			}
			else
			{
				this._uiHighBtnSprite.spriteName = "btn_item_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._uiHighBtn.get_gameObject(), false);
				this.btnLight.StopAnim();
			}
		}

		public void setConstruct()
		{
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].DisableParticles();
			}
			ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.NormalConstruct);
		}

		private void setFocus()
		{
			if (this.taskMainArsenalManager.CurrentMode != TaskMainArsenalManager.Mode.DOCK_FOCUS)
			{
				this.taskMainArsenalManager.unsetHexFocus();
				this.taskMainArsenalManager.focusDock();
			}
			this.taskMainArsenalManager.selectDock(this._number);
		}

		public void DockFrameEL()
		{
			if (UICamera.touchCount > 1)
			{
				return;
			}
			if (ArsenalTaskManager._clsArsenal.checkDialogOpen() || !TaskMainArsenalManager.IsControl)
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			ArsenalTaskManager._clsArsenal.DockIndex = this._number;
			TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.KENZOU;
			this.setConstruct();
			ArsenalTaskManager._clsArsenal._isEnd = true;
			TaskMainArsenalManager.IsControl = false;
			this.setFocus();
		}

		public void HighSpeedIconEL()
		{
			if (UICamera.touchCount > 1)
			{
				return;
			}
			if (ArsenalTaskManager._clsArsenal.checkDialogOpen() || !TaskMainArsenalManager.IsControl)
			{
				return;
			}
			this.setFocus();
			if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip_ChangeHighSpeed(this._number + 1))
			{
				this._uiHighBtnSprite.spriteName = "btn_item_on";
				ArsenalTaskManager._clsArsenal.showHighSpeedDialog(this._number);
				this.IsShowHigh = true;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void DockOpenBtnEL()
		{
			if (UICamera.touchCount > 1)
			{
				return;
			}
			if (ArsenalTaskManager._clsArsenal.checkDialogOpen() || !TaskMainArsenalManager.IsControl)
			{
				return;
			}
			this.setFocus();
			if (TaskMainArsenalManager.arsenalManager.IsValidOpenNewDock())
			{
				ArsenalTaskManager._clsArsenal._dockOpenDialogManager.showDialog(this._number);
				ArsenalTaskManager._clsArsenal.showDockOpenDialog();
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDockKey));
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void StartDockOpen()
		{
			TaskMainArsenalManager.arsenalManager.OpenNewDock();
			this._lockAnim.Play();
			this._setShow(true);
			this._lockBtn.SetActive(false);
		}

		public bool CheckStateEmpty()
		{
			if (this._dock != null)
			{
				this._dock = TaskMainArsenalManager.arsenalManager.GetDock(this._number + 1);
				return this._dock.State != KdockStates.CREATE && this._dock.State != KdockStates.COMPLETE;
			}
			return false;
		}

		public void GetShipBtnEL()
		{
			if (UICamera.touchCount > 1)
			{
				return;
			}
			if (ArsenalTaskManager._clsArsenal.checkDialogOpen() || !TaskMainArsenalManager.IsControl)
			{
				return;
			}
			if (this.IsHight)
			{
				return;
			}
			this.setFocus();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			ArsenalTaskManager._clsArsenal.setTutorialVisible(false);
			if (this._dock.IsTunker())
			{
				if (TaskMainArsenalManager.arsenalManager.IsValidGetCreatedTanker(this._number + 1))
				{
					int countNoMove = TaskMainArsenalManager.arsenalManager.GetNonDeploymentTankerCount().GetCountNoMove();
					int createdTanker = TaskMainArsenalManager.arsenalManager.GetCreatedTanker(this._number + 1);
					int afterNum = countNoMove + createdTanker;
					this._shipSManager.init(this._number);
					this._setShow();
					ArsenalTaskManager._clsArsenal.showTankerDialog(createdTanker, countNoMove, afterNum);
				}
			}
			else if (TaskMainArsenalManager.arsenalManager.IsValidGetCreatedShip(this._number + 1))
			{
				this.IsHight = false;
				this._rewardShip = TaskMainArsenalManager.arsenalManager.GetCreatedShip(this._number + 1);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				TaskMainArsenalManager.IsControl = false;
				TaskMainArsenalManager.isTouchEnable = false;
				Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.createReciveShip(observer)).Subscribe(delegate(bool _)
				{
					this._prodReceiveShip.SetActive(true);
					this._prodReceiveShip.Play(new Action(this._onShipGetFinished));
				});
				this.DelayActionFrame(3, delegate
				{
					this._shipSManager.init(this._number);
					this._setShow();
				});
			}
			else if (Comm_UserDatas.Instance.User_basic.IsMaxChara())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotGetArsenalByLimitShip));
			}
			else if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotGetArsenalByLimitItem));
			}
		}

		[DebuggerHidden]
		private IEnumerator createReciveShip(IObserver<bool> observer)
		{
			UiArsenalDock.<createReciveShip>c__Iterator76 <createReciveShip>c__Iterator = new UiArsenalDock.<createReciveShip>c__Iterator76();
			<createReciveShip>c__Iterator.observer = observer;
			<createReciveShip>c__Iterator.<$>observer = observer;
			<createReciveShip>c__Iterator.<>f__this = this;
			return <createReciveShip>c__Iterator;
		}

		private void _onShipGetFinished()
		{
			if (this._prodReceiveShip != null)
			{
				this._prodReceiveShip.ReleaseShipTextureAndBackgroundTexture();
				Object.Destroy(this._prodReceiveShip.get_gameObject());
			}
			this._prodReceiveShip = null;
			TrophyUtil.Unlock_At_BuildShip(this._rewardShip.Ship.MstId);
			this._rewardShip = null;
			TaskMainArsenalManager.IsControl = true;
			TaskMainArsenalManager.isTouchEnable = true;
			ArsenalTaskManager._clsArsenal.hideDialog();
			this._dockMiniMamager.StopConstAnimation();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			ArsenalTaskManager._clsArsenal.setTutorialVisible(true);
		}

		public bool IsShipGetView()
		{
			return this._rewardShip != null;
		}
	}
}
