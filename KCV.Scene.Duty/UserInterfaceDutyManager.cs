using AnimationOrTween;
using Common.Enum;
using DG.Tweening;
using KCV.Battle.Production;
using KCV.Dialog;
using KCV.Scene.Duty.Reward;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UserInterfaceDutyManager : MonoBehaviour
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private Transform mTransform_AllClearText;

		[SerializeField]
		private UIDutyDetail mPrefabDutyDetail;

		[SerializeField]
		private UIDutyDetailCheck mPrefabDutyDetailCheck;

		[SerializeField]
		private UIGetRewardDialog mPrefabUIDutyRewardMaterialsDialog;

		[SerializeField]
		private UIDutyRewardExchangeItem mPrefabUIDutyRewardExchangeItem;

		[SerializeField]
		private ProdRewardGet mPrefabRewardShip;

		[SerializeField]
		private ModalCamera mModalCamera;

		[SerializeField]
		private UIDutyOhyodo mPrefab_DutyOhyodo;

		[SerializeField]
		private UIDutyGrid mDutyGrid;

		[SerializeField]
		private UILabel mLabel_DutyCount;

		[SerializeField]
		private Font mUseFont;

		[SerializeField]
		private UITexture mTexture_LeftArrow;

		[SerializeField]
		private UITexture mTexture_RightArrow;

		[SerializeField]
		private UITexture mTexture_LeftArrowShadow;

		[SerializeField]
		private UITexture mTexture_RightArrowShadow;

		private DutyManager mDutyManager;

		private KeyControl mFocusKeyController;

		public bool _DeteilMode;

		[SerializeField]
		private int FIND_MISSION_ID = 120;

		[SerializeField]
		private float arrowDuration = 0.3f;

		[SerializeField]
		private float arrowAlpha = 0.6f;

		[SerializeField]
		private int arrowMove = 20;

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UserInterfaceDutyManager.<Start>c__Iterator79 <Start>c__Iterator = new UserInterfaceDutyManager.<Start>c__Iterator79();
			<Start>c__Iterator.<>f__this = this;
			return <Start>c__Iterator;
		}

		private void OnChangePageDutyGrid()
		{
			int num = this.mDutyGrid.GetCurrentPageIndex() + 1;
			int pageSize = this.mDutyGrid.GetPageSize();
			if (DOTween.IsTweening(this.mTexture_LeftArrow))
			{
				DOTween.Kill(this.mTexture_LeftArrow, false);
			}
			if (DOTween.IsTweening(this.mTexture_RightArrow))
			{
				DOTween.Kill(this.mTexture_RightArrow, false);
			}
			if (DOTween.IsTweening(this.mTexture_LeftArrowShadow))
			{
				DOTween.Kill(this.mTexture_LeftArrowShadow, false);
			}
			if (DOTween.IsTweening(this.mTexture_RightArrowShadow))
			{
				DOTween.Kill(this.mTexture_RightArrowShadow, false);
			}
			if (pageSize == 0 || pageSize == 1)
			{
				this.mTexture_RightArrow.SetActive(false);
				this.mTexture_LeftArrow.SetActive(false);
				this.mTexture_RightArrowShadow.SetActive(false);
				this.mTexture_LeftArrowShadow.SetActive(false);
				return;
			}
			if (1 < num)
			{
				this.mTexture_LeftArrow.SetActive(true);
				this.mTexture_LeftArrowShadow.SetActive(true);
				this.mTexture_LeftArrowShadow.alpha = 1f;
				this.mTexture_LeftArrowShadow.get_transform().set_localPosition(this.mTexture_LeftArrow.get_transform().get_localPosition());
				TweenSettingsExtensions.SetId<Tween>(TweenSettingsExtensions.SetLoops<Tween>(this.GenerateTweenArrow(this.mTexture_LeftArrowShadow, Direction.Forward), 2147483647), this.mTexture_LeftArrowShadow);
			}
			else
			{
				this.mTexture_LeftArrow.SetActive(false);
				this.mTexture_LeftArrowShadow.SetActive(false);
			}
			if (num < pageSize)
			{
				this.mTexture_RightArrow.SetActive(true);
				this.mTexture_RightArrowShadow.SetActive(true);
				this.mTexture_RightArrowShadow.alpha = 1f;
				this.mTexture_RightArrowShadow.get_transform().set_localPosition(this.mTexture_RightArrow.get_transform().get_localPosition());
				TweenSettingsExtensions.SetId<Tween>(TweenSettingsExtensions.SetLoops<Tween>(this.GenerateTweenArrow(this.mTexture_RightArrowShadow, Direction.Reverse), 2147483647), this.mTexture_RightArrowShadow);
			}
			else
			{
				this.mTexture_RightArrow.SetActive(false);
				this.mTexture_RightArrowShadow.SetActive(false);
			}
		}

		private void UpdateOrderPossibleDutyCount(int displayValue, bool animate)
		{
			if (DOTween.IsTweening(this.mLabel_DutyCount))
			{
				DOTween.Kill(this.mLabel_DutyCount, true);
			}
			if (animate)
			{
				Sequence sequence = DOTween.Sequence();
				Tween tween = ShortcutExtensions.DOScale(this.mLabel_DutyCount.get_transform(), new Vector3(1.15f, 1.15f, 1.15f), 0.1f);
				TweenCallback tweenCallback = delegate
				{
					this.mLabel_DutyCount.text = displayValue.ToString();
				};
				Tween tween2 = ShortcutExtensions.DOScale(this.mLabel_DutyCount.get_transform(), new Vector3(1f, 1f, 1f), 0.2f);
				TweenSettingsExtensions.SetEase<Sequence>(sequence, 15);
				TweenSettingsExtensions.AppendInterval(sequence, 0.25f);
				TweenSettingsExtensions.Append(sequence, tween);
				TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
				TweenSettingsExtensions.Append(sequence, tween2);
				TweenSettingsExtensions.SetId<Sequence>(sequence, this.mLabel_DutyCount);
			}
			else
			{
				this.mLabel_DutyCount.text = displayValue.ToString();
			}
		}

		[DebuggerHidden]
		private IEnumerator GreetingOhyodo(KeyControl keyController, Action onFinishedGreeting)
		{
			UserInterfaceDutyManager.<GreetingOhyodo>c__Iterator7A <GreetingOhyodo>c__Iterator7A = new UserInterfaceDutyManager.<GreetingOhyodo>c__Iterator7A();
			<GreetingOhyodo>c__Iterator7A.onFinishedGreeting = onFinishedGreeting;
			<GreetingOhyodo>c__Iterator7A.<$>onFinishedGreeting = onFinishedGreeting;
			<GreetingOhyodo>c__Iterator7A.<>f__this = this;
			return <GreetingOhyodo>c__Iterator7A;
		}

		private void Update()
		{
			if (this.mFocusKeyController != null)
			{
				this.mFocusKeyController.Update();
				if (this.mFocusKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					this.mFocusKeyController.ClearKeyAll();
					this.mFocusKeyController.firstUpdate = true;
					this.mFocusKeyController = null;
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
			}
		}

		private void ChangeKeyController(KeyControl keyController)
		{
			if (this.mFocusKeyController != null)
			{
				this.mFocusKeyController.firstUpdate = true;
				this.mFocusKeyController.ClearKeyAll();
			}
			this.mFocusKeyController = keyController;
			if (this.mFocusKeyController != null)
			{
				this.mFocusKeyController.firstUpdate = true;
				this.mFocusKeyController.ClearKeyAll();
			}
		}

		private DutyModel[] GetDuties()
		{
			return this.mDutyManager.GetDuties(true);
		}

		private KeyControl ShowUIDutyDetail(UIDutySummary summary)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			UIDutyDetail dutyDetail = null;
			this._DeteilMode = true;
			dutyDetail = Util.Instantiate(this.mPrefabDutyDetail.get_gameObject(), this.mModalCamera.get_gameObject(), false, false).GetComponent<UIDutyDetail>();
			dutyDetail.Initialize(summary.GetModel());
			dutyDetail.SetDutyDetailCallBack(delegate(UIDutyDetail.SelectType selectedType)
			{
				if (selectedType == UIDutyDetail.SelectType.Positive)
				{
					this.mDutyManager.StartDuty(summary.GetModel().No);
					this.UpdateOrderPossibleDutyCount(this.mDutyManager.MaxExecuteCount - this.mDutyManager.GetExecutedDutyList().get_Count(), true);
					DutyModel duty = this.mDutyManager.GetDuty(summary.GetModel().No);
					summary.Initialize(summary.GetIndex(), duty);
					TutorialModel tutorial = this.mDutyManager.UserInfo.Tutorial;
					if (tutorial.GetStep() == 0 && !tutorial.GetStepTutorialFlg(1))
					{
						tutorial.SetStepTutorialFlg(1);
						CommonPopupDialog.Instance.StartPopup("「はじめての任務！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
				dutyDetail.Hide(delegate
				{
					this._DeteilMode = false;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					KeyControl keyController = this.mDutyGrid.GetKeyController();
					Object.Destroy(dutyDetail.get_gameObject());
					this.mModalCamera.Close();
					this.ChangeKeyController(keyController);
				});
			});
			return dutyDetail.Show();
		}

		private KeyControl ShowUIDetailCheck(UIDutySummary summary)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			UIDutyDetailCheck dutyDetail = null;
			dutyDetail = Util.Instantiate(this.mPrefabDutyDetailCheck.get_gameObject(), this.mModalCamera.get_gameObject(), false, false).GetComponent<UIDutyDetailCheck>();
			dutyDetail.Initialize(summary.GetModel());
			dutyDetail.SetDutyDetailCheckClosedCallBack(delegate
			{
				dutyDetail.Hide(delegate
				{
					this._DeteilMode = false;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					KeyControl keyController = this.mDutyGrid.GetKeyController();
					Object.Destroy(dutyDetail.get_gameObject());
					this.mModalCamera.Close();
					this.ChangeKeyController(keyController);
				});
			});
			return dutyDetail.Show();
		}

		private void UIDutySummaryEventCallBack(UIDutySummary.SelectType type, UIDutySummary summary)
		{
			switch (type)
			{
			case UIDutySummary.SelectType.Action:
			case UIDutySummary.SelectType.Hover:
				this.mDutyGrid.SetKeyController(null);
				switch (summary.GetModel().State)
				{
				case QuestState.WAITING_START:
					this.PlaySe(SEFIleInfos.CommonEnter2);
					if (this.mDutyManager.GetExecutedDutyList().get_Count() < this.mDutyManager.MaxExecuteCount)
					{
						this.mModalCamera.Show();
						KeyControl keyController = this.ShowUIDutyDetail(summary);
						this.ChangeKeyController(keyController);
					}
					else
					{
						this.mModalCamera.Show();
						KeyControl keyController = this.ShowUIDetailCheck(summary);
						this.ChangeKeyController(keyController);
					}
					break;
				case QuestState.RUNNING:
				{
					this.PlaySe(SEFIleInfos.SE_028);
					this.mDutyManager.Cancel(summary.GetModel().No);
					this.UpdateOrderPossibleDutyCount(this.mDutyManager.MaxExecuteCount - this.mDutyManager.GetExecutedDutyList().get_Count(), true);
					DutyModel duty = this.mDutyManager.GetDuty(summary.GetModel().No);
					summary.Initialize(summary.GetIndex(), duty);
					this.ChangeKeyController(this.mDutyGrid.GetKeyController());
					break;
				}
				case QuestState.COMPLETE:
				{
					List<DutyModel.InvalidType> invalidTypes = summary.GetModel().GetInvalidTypes();
					if (invalidTypes.get_Count() == 0)
					{
						this.PlaySe(SEFIleInfos.SE_012);
						this.mModalCamera.Show();
						IReward[] rewards = this.mDutyManager.RecieveRewards(summary.GetModel().No).ToArray();
						IEnumerator enumerator = this.ReciveReward(rewards);
						base.StartCoroutine(enumerator);
						TutorialModel tutorial = this.mDutyManager.UserInfo.Tutorial;
						if (tutorial.GetStep() == 6 && !tutorial.GetStepTutorialFlg(7))
						{
							tutorial.SetStepTutorialFlg(7);
							CommonPopupDialog.Instance.StartPopup("「任務完了！」 達成");
							SoundUtils.PlaySE(SEFIleInfos.SE_012);
						}
					}
					else
					{
						switch (invalidTypes.get_Item(0))
						{
						case DutyModel.InvalidType.MAX_SHIP:
							CommonPopupDialog.Instance.StartPopup("艦が保有上限に達しています");
							break;
						case DutyModel.InvalidType.MAX_SLOT:
							CommonPopupDialog.Instance.StartPopup("装備が保有上限に達しています");
							break;
						case DutyModel.InvalidType.LOCK_TARGET_SLOT:
							CommonPopupDialog.Instance.StartPopup("該当装備がロックされています");
							break;
						}
						this.ChangeKeyController(this.mDutyGrid.GetKeyController());
					}
					break;
				}
				}
				break;
			case UIDutySummary.SelectType.CallDetail:
			{
				this._DeteilMode = true;
				this.mDutyGrid.SetKeyController(null);
				this.mModalCamera.Show();
				KeyControl keyController = this.ShowUIDetailCheck(summary);
				this.ChangeKeyController(keyController);
				break;
			}
			case UIDutySummary.SelectType.Back:
				summary.Hover();
				this.ChangeKeyController(this.mDutyGrid.GetKeyController());
				break;
			}
		}

		[DebuggerHidden]
		private IEnumerator OnReciveSpoint(Reward_SPoint spoint)
		{
			UserInterfaceDutyManager.<OnReciveSpoint>c__Iterator7B <OnReciveSpoint>c__Iterator7B = new UserInterfaceDutyManager.<OnReciveSpoint>c__Iterator7B();
			<OnReciveSpoint>c__Iterator7B.spoint = spoint;
			<OnReciveSpoint>c__Iterator7B.<$>spoint = spoint;
			<OnReciveSpoint>c__Iterator7B.<>f__this = this;
			return <OnReciveSpoint>c__Iterator7B;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardMaterials(IReward_Material[] materials)
		{
			UserInterfaceDutyManager.<OnReciveRewardMaterials>c__Iterator7C <OnReciveRewardMaterials>c__Iterator7C = new UserInterfaceDutyManager.<OnReciveRewardMaterials>c__Iterator7C();
			<OnReciveRewardMaterials>c__Iterator7C.materials = materials;
			<OnReciveRewardMaterials>c__Iterator7C.<$>materials = materials;
			<OnReciveRewardMaterials>c__Iterator7C.<>f__this = this;
			return <OnReciveRewardMaterials>c__Iterator7C;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardItems(Reward_Useitems useItems)
		{
			UserInterfaceDutyManager.<OnReciveRewardItems>c__Iterator7D <OnReciveRewardItems>c__Iterator7D = new UserInterfaceDutyManager.<OnReciveRewardItems>c__Iterator7D();
			<OnReciveRewardItems>c__Iterator7D.useItems = useItems;
			<OnReciveRewardItems>c__Iterator7D.<$>useItems = useItems;
			<OnReciveRewardItems>c__Iterator7D.<>f__this = this;
			return <OnReciveRewardItems>c__Iterator7D;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardSlotItem(IReward_Slotitem reward_Slotitem)
		{
			UserInterfaceDutyManager.<OnReciveRewardSlotItem>c__Iterator7E <OnReciveRewardSlotItem>c__Iterator7E = new UserInterfaceDutyManager.<OnReciveRewardSlotItem>c__Iterator7E();
			<OnReciveRewardSlotItem>c__Iterator7E.reward_Slotitem = reward_Slotitem;
			<OnReciveRewardSlotItem>c__Iterator7E.<$>reward_Slotitem = reward_Slotitem;
			<OnReciveRewardSlotItem>c__Iterator7E.<>f__this = this;
			return <OnReciveRewardSlotItem>c__Iterator7E;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardOpenDeckPractice(Reward_DeckPracitce reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardOpenDeckPractice>c__Iterator7F <OnReciveRewardOpenDeckPractice>c__Iterator7F = new UserInterfaceDutyManager.<OnReciveRewardOpenDeckPractice>c__Iterator7F();
			<OnReciveRewardOpenDeckPractice>c__Iterator7F.reward = reward;
			<OnReciveRewardOpenDeckPractice>c__Iterator7F.<$>reward = reward;
			<OnReciveRewardOpenDeckPractice>c__Iterator7F.<>f__this = this;
			return <OnReciveRewardOpenDeckPractice>c__Iterator7F;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardTransportCraft(Reward_TransportCraft reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardTransportCraft>c__Iterator80 <OnReciveRewardTransportCraft>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardTransportCraft>c__Iterator80();
			<OnReciveRewardTransportCraft>c__Iterator.reward = reward;
			<OnReciveRewardTransportCraft>c__Iterator.<$>reward = reward;
			<OnReciveRewardTransportCraft>c__Iterator.<>f__this = this;
			return <OnReciveRewardTransportCraft>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardItem(Reward_Useitem reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardItem>c__Iterator81 <OnReciveRewardItem>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardItem>c__Iterator81();
			<OnReciveRewardItem>c__Iterator.reward = reward;
			<OnReciveRewardItem>c__Iterator.<$>reward = reward;
			<OnReciveRewardItem>c__Iterator.<>f__this = this;
			return <OnReciveRewardItem>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardShip(IReward_Ship reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardShip>c__Iterator82 <OnReciveRewardShip>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardShip>c__Iterator82();
			<OnReciveRewardShip>c__Iterator.reward = reward;
			<OnReciveRewardShip>c__Iterator.<$>reward = reward;
			<OnReciveRewardShip>c__Iterator.<>f__this = this;
			return <OnReciveRewardShip>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardDeck(Reward_Deck reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardDeck>c__Iterator83 <OnReciveRewardDeck>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardDeck>c__Iterator83();
			<OnReciveRewardDeck>c__Iterator.reward = reward;
			<OnReciveRewardDeck>c__Iterator.<$>reward = reward;
			<OnReciveRewardDeck>c__Iterator.<>f__this = this;
			return <OnReciveRewardDeck>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardLargeBuild(Reward_LargeBuild reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardLargeBuild>c__Iterator84 <OnReciveRewardLargeBuild>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardLargeBuild>c__Iterator84();
			<OnReciveRewardLargeBuild>c__Iterator.reward = reward;
			<OnReciveRewardLargeBuild>c__Iterator.<$>reward = reward;
			<OnReciveRewardLargeBuild>c__Iterator.<>f__this = this;
			return <OnReciveRewardLargeBuild>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardExchangeSlotItem(IReward_Exchange_Slotitem reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardExchangeSlotItem>c__Iterator85 <OnReciveRewardExchangeSlotItem>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardExchangeSlotItem>c__Iterator85();
			<OnReciveRewardExchangeSlotItem>c__Iterator.reward = reward;
			<OnReciveRewardExchangeSlotItem>c__Iterator.<$>reward = reward;
			<OnReciveRewardExchangeSlotItem>c__Iterator.<>f__this = this;
			return <OnReciveRewardExchangeSlotItem>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnReciveRewardFurniture(Reward_Furniture reward)
		{
			UserInterfaceDutyManager.<OnReciveRewardFurniture>c__Iterator86 <OnReciveRewardFurniture>c__Iterator = new UserInterfaceDutyManager.<OnReciveRewardFurniture>c__Iterator86();
			<OnReciveRewardFurniture>c__Iterator.reward = reward;
			<OnReciveRewardFurniture>c__Iterator.<$>reward = reward;
			<OnReciveRewardFurniture>c__Iterator.<>f__this = this;
			return <OnReciveRewardFurniture>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ReciveReward(IReward[] rewards)
		{
			UserInterfaceDutyManager.<ReciveReward>c__Iterator87 <ReciveReward>c__Iterator = new UserInterfaceDutyManager.<ReciveReward>c__Iterator87();
			<ReciveReward>c__Iterator.rewards = rewards;
			<ReciveReward>c__Iterator.<$>rewards = rewards;
			<ReciveReward>c__Iterator.<>f__this = this;
			return <ReciveReward>c__Iterator;
		}

		private bool CanStartDuty(DutyModel dutyModel)
		{
			if (dutyModel.State == QuestState.RUNNING || dutyModel.State == QuestState.COMPLETE)
			{
				return false;
			}
			int count = this.mDutyManager.GetRunningDutyList().get_Count();
			return count + 1 <= this.mDutyManager.MaxExecuteCount;
		}

		private void PlaySe(SEFIleInfos seType)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				bool flag = true;
				if (flag)
				{
					SoundUtils.PlaySE(seType);
				}
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this.mLabel_DutyCount))
			{
				DOTween.Kill(this.mLabel_DutyCount, false);
			}
			if (DOTween.IsTweening(this.mTexture_LeftArrow))
			{
				DOTween.Kill(this.mTexture_LeftArrow, false);
			}
			if (DOTween.IsTweening(this.mTexture_RightArrow))
			{
				DOTween.Kill(this.mTexture_RightArrow, false);
			}
			if (DOTween.IsTweening(this.mTexture_LeftArrowShadow))
			{
				DOTween.Kill(this.mTexture_LeftArrowShadow, false);
			}
			if (DOTween.IsTweening(this.mTexture_RightArrowShadow))
			{
				DOTween.Kill(this.mTexture_RightArrowShadow, false);
			}
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_DutyCount);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_LeftArrow, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_RightArrow, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_LeftArrowShadow, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_RightArrowShadow, false);
			this.mTransform_AllClearText = null;
			this.mPrefabDutyDetail = null;
			this.mPrefabDutyDetailCheck = null;
			this.mPrefabUIDutyRewardMaterialsDialog = null;
			this.mPrefabRewardShip = null;
			this.mModalCamera = null;
			this.mPrefab_DutyOhyodo = null;
			this.mDutyGrid = null;
			this.mDutyManager = null;
			this.mFocusKeyController = null;
		}

		private Tween GenerateTweenArrow(UITexture arrow, Direction direction)
		{
			int num = 0;
			switch (direction + 1)
			{
			case Direction.Toggle:
				num = this.arrowMove;
				break;
			case (Direction)2:
				num = -this.arrowMove;
				break;
			}
			if (DOTween.IsTweening(arrow))
			{
				DOTween.Kill(arrow, false);
			}
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), arrow);
			Tween tween = ShortcutExtensions.DOLocalMoveX(arrow.get_transform(), arrow.get_transform().get_localPosition().x + (float)num, this.arrowDuration, false);
			Tween tween2 = DOVirtual.Float(arrow.alpha, 0f, this.arrowAlpha, delegate(float alpha)
			{
				arrow.alpha = alpha;
			});
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Join(sequence, tween2);
			return sequence;
		}
	}
}
