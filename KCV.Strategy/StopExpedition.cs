using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StopExpedition : MonoBehaviour
	{
		public enum ActionType
		{
			StartMission,
			NotStartMission,
			Shown,
			Hiden
		}

		public enum CheckType
		{
			CallTankerCountUp,
			CallTankerCountDown,
			CanStartCheck
		}

		public delegate void UIMissionWithTankerConfirmPopUpAction(StopExpedition.ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject);

		public delegate bool UIMissionWithTankerConfirmPopUpCheck(StopExpedition.CheckType actionType, UIMissionWithTankerConfirmPopUp calledObject);

		[SerializeField]
		private UILabel mLabel_RequireDay;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIMissionShipBanner[] mUIMissionShipBanners;

		[SerializeField]
		private UILabel mLabel_Message;

		[SerializeField]
		private UILabel mLabel_MissionName;

		[SerializeField]
		private DialogAnimation dialogAnim;

		[SerializeField]
		private UIPanel RightPanel;

		private UIButton mFocusButton;

		private KeyControl mKeyController;

		private StopExpedition.UIMissionWithTankerConfirmPopUpAction mUIMissionWithTankerConfirmPopUpAction;

		private StopExpedition.UIMissionWithTankerConfirmPopUpCheck mUIMissionWithTankerConfirmPopUpTankerCheck;

		private Coroutine mInitializeCoroutine;

		private int mHasTankerCount;

		private MissionManager missionMng;

		public int SettingTankerCount
		{
			get;
			private set;
		}

		public DeckModel MissionStartDeckModel
		{
			get;
			private set;
		}

		public void StartPanel(MissionManager missionMng)
		{
			this.missionMng = missionMng;
			base.get_transform().set_localScale(Vector3.get_zero());
			Transform child = base.get_transform().FindChild("RightPanel/RequireDay").GetChild(1);
			Object.Destroy(child.GetComponent<UISprite>());
			UILabel uILabel = child.AddComponent<UILabel>();
			child.localPositionX(-25f);
			uILabel.fontSize = 22;
			uILabel.text = "残り遠征日数";
			uILabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			uILabel.font = this.mLabel_RequireDay.font;
			uILabel.color = new Color32(75, 75, 75, 255);
			this.dialogAnim.OpenAction = delegate
			{
				this.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
				this.RightPanel.SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				this.GetKeyController();
			};
			this.dialogAnim.StartAnim(DialogAnimation.AnimType.POPUP, true);
		}

		public void Initialize(DeckModel deckModel)
		{
			this.mLabel_Message.alpha = 0.01f;
			this.MissionStartDeckModel = deckModel;
			this.ChangeFocusButton(this.mButton_Negative);
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			int missionId = currentDeck.MissionId;
			MissionModel mission = this.missionMng.GetMission(missionId);
			this.mLabel_RequireDay.text = currentDeck.MissionRemainingTurns.ToString();
			this.mLabel_MissionName.text = mission.Name;
			if (this.mInitializeCoroutine != null)
			{
				base.StopCoroutine(this.mInitializeCoroutine);
				this.mInitializeCoroutine = null;
			}
			this.mInitializeCoroutine = base.StartCoroutine(this.InitailizeCoroutine(deckModel, delegate
			{
				this.mInitializeCoroutine = null;
			}));
		}

		[DebuggerHidden]
		private IEnumerator InitailizeCoroutine(DeckModel deckModel, Action callBack)
		{
			StopExpedition.<InitailizeCoroutine>c__Iterator149 <InitailizeCoroutine>c__Iterator = new StopExpedition.<InitailizeCoroutine>c__Iterator149();
			<InitailizeCoroutine>c__Iterator.deckModel = deckModel;
			<InitailizeCoroutine>c__Iterator.callBack = callBack;
			<InitailizeCoroutine>c__Iterator.<$>deckModel = deckModel;
			<InitailizeCoroutine>c__Iterator.<$>callBack = callBack;
			<InitailizeCoroutine>c__Iterator.<>f__this = this;
			return <InitailizeCoroutine>c__Iterator;
		}

		public KeyControl GetKeyController()
		{
			if (this.mKeyController == null)
			{
				this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			}
			return this.mKeyController;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocusButton(this.mButton_Negative);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.ChangeFocusButton(this.mButton_Positive);
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.mFocusButton.SendMessage("OnClick");
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
					this.mButton_Negative.SendMessage("OnClick");
				}
			}
		}

		public void OnClickPositiveButton()
		{
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			this.missionMng.MissionStop(currentDeck.Id);
			this.mKeyController = null;
			this.dialogAnim.CloseAction = new Action(this.DestroyThis);
			this.dialogAnim.StartAnim(DialogAnimation.AnimType.FEAD, false);
			StrategyTopTaskManager.GetCommandMenu().DeckEnableCheck();
		}

		public void OnClickNegativeButton()
		{
			this.mKeyController = null;
			this.dialogAnim.CloseAction = new Action(this.DestroyThis);
			this.dialogAnim.StartAnim(DialogAnimation.AnimType.FEAD, false);
		}

		public void SetOnUIMissionWithTankerConfirmPopUpAction(StopExpedition.UIMissionWithTankerConfirmPopUpAction action)
		{
		}

		public void SetOnUIMissionWithTankerConfirmPopUpTankerCheckDelegate(StopExpedition.UIMissionWithTankerConfirmPopUpCheck method)
		{
		}

		private void UpdateSettingTankerCountLabel(int value, bool isPoor)
		{
		}

		private void ChangeFocusButton(UIButton target)
		{
			if (target == null)
			{
				this.mButton_Negative.SetState(UIButtonColor.State.Normal, true);
				this.mButton_Positive.SetState(UIButtonColor.State.Hover, true);
				return;
			}
			this.mFocusButton = target;
			if (target.Equals(this.mButton_Negative))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.mButton_Negative.SetState(UIButtonColor.State.Hover, true);
				this.mButton_Positive.SetState(UIButtonColor.State.Normal, true);
			}
			else if (target.Equals(this.mButton_Positive))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.mButton_Negative.SetState(UIButtonColor.State.Normal, true);
				this.mButton_Positive.SetState(UIButtonColor.State.Hover, true);
			}
		}

		private void DestroyThis()
		{
			KeyControlManager.Instance.KeyController = StrategyTopTaskManager.GetCommandMenu().keyController;
			Object.Destroy(base.get_gameObject());
		}
	}
}
