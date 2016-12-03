using KCV.Scene.Port;
using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Duty
{
	[RequireComponent(typeof(UIButtonManager))]
	public class UIDutyStartButton : MonoBehaviour
	{
		private UIButtonManager mUIButtonManager;

		[SerializeField]
		private UISprite mSpriteYousei;

		[SerializeField]
		private UIButton mButtonPositive;

		[SerializeField]
		private UIButton mButtonNegative;

		[SerializeField]
		private UITexture mTexture_Ohyodo;

		[SerializeField]
		private Texture[] mTextureOhyodo;

		private Vector3 mYouseiDefaultPosition;

		private Action mPositiveSelectedCallBack;

		private Action mNegativeSelectedCallBack;

		private Action mSelectedCallBack;

		private UIButton mFocusButton;

		private UIButton _uiOverlayButton;

		private bool isSelected;

		private Action mGoToHouseYouseiCallBack;

		private void Awake()
		{
			this.mUIButtonManager = base.GetComponent<UIButtonManager>();
		}

		private void Start()
		{
			this.mUIButtonManager.IndexChangeAct = delegate
			{
				if (this.mUIButtonManager.nowForcusButton.Equals(this.mButtonNegative))
				{
					this.ChangeFocus(this.mUIButtonManager.nowForcusButton, false);
				}
				else if (this.mUIButtonManager.nowForcusButton.Equals(this.mButtonPositive))
				{
					this.FocusPositive();
				}
			};
			this._uiOverlayButton = GameObject.Find("OverlayBtn").GetComponent<UIButton>();
			EventDelegate.Add(this._uiOverlayButton.onClick, new EventDelegate.Callback(this.OnClickNegative));
			this.mYouseiDefaultPosition = this.mSpriteYousei.get_transform().get_localPosition();
			this.FocusPositive();
		}

		public void ClickFocusButton()
		{
			if (!this.isSelected)
			{
				this.mFocusButton.SendMessage("OnClick");
				UIUtil.AnimationOnFocus(this.mFocusButton.get_transform(), null);
			}
		}

		public void SetOnPositiveSelectedCallBack(Action action)
		{
			this.mPositiveSelectedCallBack = action;
		}

		public void SetOnNegativeSelectedCallBack(Action action)
		{
			this.mNegativeSelectedCallBack = action;
		}

		public void SetOnSelectedCallBack(Action action)
		{
			this.mSelectedCallBack = action;
		}

		public void FocusPositive()
		{
			if (this.mFocusButton == null || !this.mFocusButton.Equals(this.mButtonPositive))
			{
				this.ChangeFocus(this.mButtonPositive, true);
				Vector3 localPosition = this.mSpriteYousei.get_transform().get_localPosition();
				Vector3 vector = new Vector3(localPosition.x, localPosition.y + 40f, localPosition.z);
				this.mSpriteYousei.spriteName = "mini_06_c_01";
				Hashtable hashtable = new Hashtable();
				hashtable.Add("y", 45f);
				hashtable.Add("time", 0.3f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easetype", iTween.EaseType.easeOutQuint);
				iTween.MoveTo(this.mSpriteYousei.get_gameObject(), hashtable);
			}
		}

		public void FocusNegative(bool seFlag)
		{
			if (this.mFocusButton == null)
			{
				this.ChangeFocus(this.mButtonNegative, seFlag);
			}
			else if (!this.mFocusButton.Equals(this.mButtonNegative))
			{
				this.ChangeFocus(this.mButtonNegative, seFlag);
			}
		}

		private void ChangeFocus(UIButton targetButton, bool needSe)
		{
			if (this.mFocusButton != null && this.mFocusButton.Equals(this.mButtonPositive) && targetButton != null && targetButton.Equals(this.mButtonNegative))
			{
				this.RemoveFocus();
			}
			if (this.mFocusButton != null)
			{
				this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this.mFocusButton.get_gameObject(), false);
			}
			this.mFocusButton = targetButton;
			if (this.mFocusButton != null)
			{
				if (needSe)
				{
					this.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this.mFocusButton.get_gameObject(), true);
				UIUtil.AnimationOnFocus(this.mFocusButton.get_transform(), null);
			}
		}

		private void PlaySE(SEFIleInfos seType)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SoundUtils.PlaySE(seType);
			}
		}

		public void RemoveFocus()
		{
			this.GoToHouseYouse(null);
		}

		public void OnClickPositive()
		{
			if (!this.isSelected)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_028);
				this.ChangeFocus(this.mButtonPositive, false);
				this.isSelected = true;
				if (this.mSelectedCallBack != null)
				{
					this.mSelectedCallBack.Invoke();
				}
				this.mTexture_Ohyodo.mainTexture = this.mTextureOhyodo[1];
				iTween.tweens.Clear();
				Vector3 vector = new Vector3(this.mYouseiDefaultPosition.x, this.mYouseiDefaultPosition.y + 40f, this.mYouseiDefaultPosition.z);
				Hashtable hashtable = new Hashtable();
				this.mSpriteYousei.spriteName = "mini_06_c_02";
				hashtable.Add("y", 80f);
				hashtable.Add("time", 0.3f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easetype", iTween.EaseType.easeInBack);
				hashtable.Add("oncomplete", "OnClickedGoToHouseYouse");
				hashtable.Add("oncompletetarget", base.get_gameObject());
				iTween.MoveTo(this.mSpriteYousei.get_gameObject(), hashtable);
				this.mButtonNegative.isEnabled = false;
				this.mButtonPositive.isEnabled = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void OnClickNegative()
		{
			if (!this.isSelected)
			{
				this.ChangeFocus(this.mButtonNegative, false);
				this.isSelected = true;
				if (this.mSelectedCallBack != null)
				{
					this.mSelectedCallBack.Invoke();
				}
				if (this.mNegativeSelectedCallBack != null)
				{
					this.mNegativeSelectedCallBack.Invoke();
				}
				this.mButtonNegative.isEnabled = false;
				this.mButtonPositive.isEnabled = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			}
		}

		private void OnClickedGoToHouseYouse()
		{
			this.GoToHouseYouse(delegate
			{
				if (this.mPositiveSelectedCallBack != null)
				{
					this.mPositiveSelectedCallBack.Invoke();
				}
			});
		}

		private void GoToHouseYouse(Action callBack)
		{
			this.mGoToHouseYouseiCallBack = callBack;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("y", this.mYouseiDefaultPosition.y);
			hashtable.Add("time", 0.3f);
			hashtable.Add("isLocal", true);
			hashtable.Add("easetype", iTween.EaseType.easeInBack);
			hashtable.Add("oncomplete", "OnClickAnimationFinished");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			iTween.MoveTo(this.mSpriteYousei.get_gameObject(), hashtable);
		}

		private void OnClickAnimationFinished()
		{
			if (this.mGoToHouseYouseiCallBack != null)
			{
				this.mGoToHouseYouseiCallBack.Invoke();
			}
		}

		public void OnTocuh()
		{
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextureOhyodo, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSpriteYousei);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButtonPositive);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButtonNegative);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Ohyodo, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mFocusButton);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiOverlayButton);
			this.mPositiveSelectedCallBack = null;
			this.mNegativeSelectedCallBack = null;
			this.mSelectedCallBack = null;
			this.mUIButtonManager = null;
		}
	}
}
