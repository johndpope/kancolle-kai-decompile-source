using KCV.EscortOrganize;
using KCV.Utils;
using local.models;
using Server_Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class DeployMainPanel : MonoBehaviour
	{
		private enum MAIN_BTN
		{
			HAIBI,
			GOEI,
			OK
		}

		[SerializeField]
		private UIButtonManager btnManager;

		[SerializeField]
		private DeployShip[] DeployShips;

		[SerializeField]
		private DeployMaterials deployMaterials;

		[SerializeField]
		private UILabel TankerNum;

		[SerializeField]
		private TaskDeployTop top;

		private GameObject EscortOrganize;

		[SerializeField]
		private YesNoButton BackConfirm;

		private string[] ButtonsName;

		private KeyControl keyController;

		private Debug_Mod debugMod;

		private bool isPlayVoice;

		[SerializeField]
		private CommonDialog ConfirmDialog;

		private ShipModel EscortFlagShipModel;

		[SerializeField]
		private UITexture Landscape;

		private Coroutine PanelShowCor;

		[SerializeField]
		private ButtonLightTexture btnLight;

		private bool mIsEndPhase;

		private List<int> NormalArea;

		private List<int> NorthArea;

		private List<int> SouthArea;

		public DeployMainPanel()
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(8);
			list.Add(9);
			list.Add(11);
			list.Add(12);
			this.NormalArea = list;
			list = new List<int>();
			list.Add(3);
			list.Add(13);
			this.NorthArea = list;
			list = new List<int>();
			list.Add(2);
			list.Add(4);
			list.Add(5);
			list.Add(6);
			list.Add(7);
			list.Add(10);
			list.Add(14);
			this.SouthArea = list;
			base..ctor();
		}

		public void Init(bool isGoeiChange)
		{
			this.mIsEndPhase = false;
			if (this.keyController == null)
			{
				this.keyController = new KeyControl(0, 2, 0.4f, 0.1f);
			}
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			this.SetLandscape();
			this.TankerNum.text = this.top.TankerCount.ToString();
			this.deployMaterials.updateMaterials(this.top.areaID, this.top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
			KeyControlManager.Instance.KeyController = this.keyController;
			EscortDeckModel editDeck = EscortOrganizeTaskManager.GetEscortManager().EditDeck;
			this.InitializeEscortDeckIcons(editDeck);
			StrategyTopTaskManager.Instance.UIModel.OverCamera.SetActive(true);
			this.top.isChangeMode = false;
			this.btnManager.setFocus(0);
			if (isGoeiChange)
			{
				this.keyController.IsRun = false;
				this.ChangeCharactertoEscortFlagShip();
			}
			this.DelayAction(0.3f, delegate
			{
				if (this.EscortOrganize == null)
				{
					GameObject gameObject = Util.Instantiate(StrategyTopTaskManager.GetCommandMenu().EscortOrganize, base.get_transform().get_parent().get_gameObject(), false, false);
					gameObject.SetActive(false);
					gameObject.get_transform().localPositionX(9999f);
					this.EscortOrganize = gameObject;
				}
				bool flag = StrategyTopTaskManager.GetLogicManager().IsValidDeploy(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID, this.top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
				if (flag)
				{
					this.btnLight.PlayAnim();
				}
				else
				{
					this.btnLight.StopAnim();
				}
			});
		}

		private void InitializeEscortDeckIcons(EscortDeckModel escortDeckModel)
		{
			for (int i = 0; i < this.DeployShips.Length; i++)
			{
				if (i < escortDeckModel.Count)
				{
					this.DeployShips[i].Initialize(escortDeckModel.GetShip(i));
				}
				else
				{
					this.DeployShips[i].InitializeDefailt();
				}
			}
		}

		private void ChangeStateHaibi()
		{
			this.top.isChangeMode = true;
			this.top.isDeployPanel = true;
			if (this.PanelShowCor != null)
			{
				base.StopCoroutine(this.PanelShowCor);
			}
		}

		private void ChangeStateGoei()
		{
			base.get_gameObject().SafeGetTweenAlpha(1f, 0f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			base.get_gameObject().GetComponent<TweenAlpha>().onFinished.Clear();
			this.mIsEndPhase = true;
			StrategyTopTaskManager.Instance.UIModel.Character.isEnter = true;
			StrategyTopTaskManager.Instance.UIModel.Character.Exit(delegate
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}, false);
			this.DelayAction(0.3f, delegate
			{
				this.EscortOrganize.SetActive(true);
				EscortOrganizeTaskManager.Init();
				this.DelayActionFrame(3, delegate
				{
					TweenAlpha.Begin(this.EscortOrganize, 0.2f, 1f);
					App.isFirstUpdate = true;
				});
			});
			if (this.PanelShowCor != null)
			{
				base.StopCoroutine(this.PanelShowCor);
			}
		}

		private void ChangeStateCommandMenu()
		{
			if (this.mIsEndPhase)
			{
				return;
			}
			bool flag = StrategyTopTaskManager.GetLogicManager().IsValidDeploy(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID, this.top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
			if (flag)
			{
				StrategyTopTaskManager.GetLogicManager().Deploy(this.top.areaID, this.top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
				StrategyTopTaskManager.CreateLogicManager();
				this.isPlayVoice = false;
				this.EscortFlagShipModel = null;
				this.top.backToCommandMenu();
				this.mIsEndPhase = true;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				if (this.PanelShowCor != null)
				{
					base.StopCoroutine(this.PanelShowCor);
				}
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				if (tutorial.GetStep() == 8 && !tutorial.GetStepTutorialFlg(9))
				{
					tutorial.SetStepTutorialFlg(9);
					CommonPopupDialog.Instance.StartPopup("「はじめての配備！」 達成");
					SoundUtils.PlaySE(SEFIleInfos.SE_012);
				}
				StrategyTopTaskManager.Instance.setActiveStrategy(true);
			}
			else
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				CommonPopupDialog.Instance.StartPopup("変更がありません", 0, CommonPopupDialogMessage.PlayType.Long);
			}
		}

		public bool Run()
		{
			if (this.mIsEndPhase)
			{
				return false;
			}
			this.keyController.Update();
			int index = this.keyController.Index;
			if (this.keyController.IsUpDown())
			{
				this.btnManager.movePrevButton();
			}
			else if (this.keyController.IsDownDown())
			{
				this.btnManager.moveNextButton();
			}
			else if (this.keyController.keyState.get_Item(1).down)
			{
				this.btnManager.Decide();
			}
			else if (this.keyController.keyState.get_Item(0).down)
			{
				this.OnTouchBackArea();
			}
			else if (this.keyController.keyState.get_Item(5).down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		private void OpenConfirmDialog()
		{
			this.keyController.IsRun = false;
			this.ConfirmDialog.isUseDefaultKeyController = false;
			this.ConfirmDialog.OpenDialog(1, DialogAnimation.AnimType.POPUP);
			this.ConfirmDialog.setCloseAction(delegate
			{
				this.keyController.IsRun = true;
				KeyControlManager.Instance.KeyController = this.keyController;
			});
			this.BackConfirm.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f), true);
			this.BackConfirm.SetOnSelectPositiveListener(delegate
			{
				this.ChangeStateBack();
				this.ConfirmDialog.CloseDialog();
			});
			this.BackConfirm.SetOnSelectNegativeListener(delegate
			{
				this.ConfirmDialog.CloseDialog();
			});
		}

		public void OnTouchBackArea()
		{
			if (this.mIsEndPhase)
			{
				return;
			}
			bool flag = StrategyTopTaskManager.GetLogicManager().IsValidDeploy(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID, this.top.TankerCount, EscortOrganizeTaskManager.GetEscortManager());
			if (flag)
			{
				this.OpenConfirmDialog();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			else
			{
				this.ChangeStateBack();
				this.mIsEndPhase = true;
			}
		}

		private void ChangeStateBack()
		{
			if (this.PanelShowCor != null)
			{
				return;
			}
			bool flag = !this.top.isDeployPanel;
			if (flag)
			{
				this.PanelShowCor = null;
				this.isPlayVoice = false;
				this.EscortFlagShipModel = null;
				this.top.backToCommandMenu();
				StrategyTopTaskManager.Instance.setActiveStrategy(true);
			}
		}

		private void ChangeCharactertoEscortFlagShip()
		{
			StrategyShipCharacter Character = StrategyTopTaskManager.Instance.UIModel.Character;
			ShipModel FlagShip = EscortOrganizeTaskManager.GetEscortManager().EditDeck.GetFlagShip();
			bool flag = this.EscortFlagShipModel == null || this.EscortFlagShipModel != FlagShip;
			float fromAlpha = 0f;
			UIPanel component = base.get_gameObject().GetComponent<UIPanel>();
			if (component != null)
			{
				fromAlpha = component.alpha;
			}
			base.get_gameObject().GetComponent<TweenAlpha>().onFinished.Clear();
			if (Character.isEnter && flag)
			{
				Character.Exit(delegate
				{
					this.EnterEscortFlagShip(Character, FlagShip);
				}, true);
				this.PanelShowCor = this.DelayAction(0.3f, delegate
				{
					this.get_gameObject().SafeGetTweenAlpha(fromAlpha, 1f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
					this.get_gameObject().GetComponent<TweenAlpha>().onFinished.Clear();
					this.DelayAction(0.1f, delegate
					{
						this.keyController.IsRun = true;
					});
					this.PanelShowCor = null;
				});
			}
			else
			{
				this.EnterEscortFlagShip(Character, FlagShip);
				base.get_gameObject().SafeGetTweenAlpha(fromAlpha, 1f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				base.get_gameObject().GetComponent<TweenAlpha>().onFinished.Clear();
				this.DelayAction(0.1f, delegate
				{
					this.keyController.IsRun = true;
				});
			}
		}

		private void EnterEscortFlagShip(StrategyShipCharacter Character, ShipModel FlagShip)
		{
			if (FlagShip != null)
			{
				Character.ChangeCharacter(FlagShip);
				this.DelayActionFrame(2, delegate
				{
					if (this.EscortFlagShipModel != FlagShip)
					{
						this.isPlayVoice = false;
					}
					Character.Enter(delegate
					{
						if (!this.isPlayVoice)
						{
							Character.PlayVoice(EscortOrganizeTaskManager.GetEscortManager().EditDeck);
							this.EscortFlagShipModel = FlagShip;
							this.isPlayVoice = true;
						}
					});
				});
			}
		}

		public void DestroyEscortOrganize()
		{
			if (this.EscortOrganize != null)
			{
				Object.Destroy(this.EscortOrganize);
			}
		}

		[Obsolete("外部UI[輸送船団ボタン]から参照して使用します")]
		public void OnClickYusousendan()
		{
			bool flag = !this.top.isDeployPanel && !this.mIsEndPhase && this.keyController.IsRun;
			if (flag)
			{
				this.keyController.Index = 0;
				this.ChangeStateHaibi();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		[Obsolete("外部UI[海上護衛部隊ボタン]から参照して使用します")]
		public void OnClickKaijougoeiButai()
		{
			bool flag = !this.top.isDeployPanel && !this.mIsEndPhase && this.keyController.IsRun;
			if (flag)
			{
				this.keyController.Index = 1;
				this.ChangeStateGoei();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		[Obsolete("外部UI[決定ボタン]から参照して使用します")]
		public void OnClickKettei()
		{
			bool flag = !this.top.isDeployPanel && !this.mIsEndPhase && this.keyController.IsRun;
			if (flag)
			{
				this.keyController.Index = 2;
				this.ChangeStateCommandMenu();
			}
		}

		private void SetLandscape()
		{
			int areaID = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
			if (this.NormalArea.Exists((int x) => x == areaID))
			{
				this.Landscape.mainTexture = (Resources.Load("Textures/Strategy/Deploy/popup_bg1") as Texture);
				this.Landscape.get_transform().localPositionY(101f);
			}
			else if (this.NorthArea.Exists((int x) => x == areaID))
			{
				this.Landscape.mainTexture = (Resources.Load("Textures/Strategy/Deploy/popup_bg3") as Texture);
				this.Landscape.get_transform().localPositionY(101f);
			}
			else if (this.SouthArea.Exists((int x) => x == areaID))
			{
				this.Landscape.mainTexture = (Resources.Load("Textures/Strategy/Deploy/popup_bg2") as Texture);
				this.Landscape.get_transform().localPositionY(110f);
			}
		}
	}
}
