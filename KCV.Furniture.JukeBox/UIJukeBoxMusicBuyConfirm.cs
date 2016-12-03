using DG.Tweening;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIButtonManager)), RequireComponent(typeof(UIPanel))]
	public class UIJukeBoxMusicBuyConfirm : MonoBehaviour
	{
		private UIButtonManager mButtonManager;

		private UIPanel mPanelThis;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Coin_From;

		[SerializeField]
		private UILabel mLabel_Coin_To;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private Transform mTransform_Configuable;

		private Action mOnRequestBackToRoot;

		private Action mOnSelectPositiveListener;

		private Action mOnSelectNegativeListener;

		private UIButton mButtonCurrentFocus;

		private Action mOnRequestChangeScene;

		private KeyControl mKeyController;

		private bool mIsValidBuy;

		private UIButton[] mButtonFocasable;

		private Mst_bgm_jukebox mMst_bgm_jukebox;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0f;
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				int num = Array.IndexOf<UIButton>(this.mButtonFocasable, this.mButtonManager.nowForcusButton);
				if (0 <= num)
				{
					this.ChangeFocus(this.mButtonManager.nowForcusButton);
				}
			};
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocus(this.mButton_Negative);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num = Array.IndexOf<UIButton>(this.mButtonFocasable, this.mButton_Positive);
					if (0 <= num)
					{
						this.ChangeFocus(this.mButton_Positive);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.mKeyController.ClearKeyAll();
					this.mKeyController.firstUpdate = true;
					if (this.mButton_Negative.Equals(this.mButtonCurrentFocus))
					{
						this.OnClickNegative();
					}
					else if (this.mButton_Positive.Equals(this.mButtonCurrentFocus))
					{
						this.OnClickPositive();
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnClickNegative();
				}
				else if (this.mKeyController.IsRDown())
				{
					this.OnRequestChangeScene();
				}
				else if (this.mKeyController.IsLDown())
				{
					this.OnRequestBackToRoot();
				}
			}
		}

		public void SetOnRequestBackToRoot(Action onRequestBackToRoot)
		{
			this.mOnRequestBackToRoot = onRequestBackToRoot;
		}

		private void OnRequestBackToRoot()
		{
			if (this.mOnRequestBackToRoot != null)
			{
				this.mOnRequestBackToRoot.Invoke();
			}
		}

		public void Initialize(Mst_bgm_jukebox jukeBoxBGM, int walletInFurnitureCoin, bool isValidBuy)
		{
			this.mMst_bgm_jukebox = jukeBoxBGM;
			List<UIButton> list = new List<UIButton>();
			list.Add(this.mButton_Negative);
			this.mIsValidBuy = isValidBuy;
			this.mLabel_Name.text = jukeBoxBGM.Name;
			this.mLabel_Coin_From.text = walletInFurnitureCoin.ToString();
			this.mLabel_Coin_To.text = (walletInFurnitureCoin - jukeBoxBGM.R_coins).ToString();
			this.mLabel_Price.text = jukeBoxBGM.R_coins.ToString();
			if (this.mIsValidBuy)
			{
				list.Add(this.mButton_Positive);
				this.mButton_Positive.set_enabled(true);
				this.mButton_Positive.isEnabled = true;
			}
			else
			{
				this.mButton_Positive.set_enabled(false);
				this.mButton_Positive.isEnabled = false;
			}
			bool flag = this.mMst_bgm_jukebox.Bgm_flag == 1;
			if (flag)
			{
				this.mTransform_Configuable.get_gameObject().SetActive(true);
			}
			else
			{
				this.mTransform_Configuable.get_gameObject().SetActive(false);
			}
			this.mButtonFocasable = list.ToArray();
			this.mButtonManager.UpdateButtons(this.mButtonFocasable);
			this.ChangeFocus(this.mButton_Negative);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void OnClickPositive()
		{
			this.SelectPositive();
		}

		private void OnClickNegative()
		{
			this.SelectNegative();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPositive()
		{
			this.SelectPositive();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNegative()
		{
			this.SelectNegative();
		}

		public void SetOnSelectPositiveListener(Action listener)
		{
			this.mOnSelectPositiveListener = listener;
		}

		public void SetOnSelectNegativeListener(Action listener)
		{
			this.mOnSelectNegativeListener = listener;
		}

		private void SelectPositive()
		{
			if (this.mOnSelectPositiveListener != null)
			{
				this.mOnSelectPositiveListener.Invoke();
			}
		}

		private void SelectNegative()
		{
			if (this.mOnSelectNegativeListener != null)
			{
				this.mOnSelectNegativeListener.Invoke();
			}
		}

		private void ChangeFocus(UIButton changeTarget)
		{
			if (this.mButtonCurrentFocus != null)
			{
				if (this.mButtonCurrentFocus.isEnabled)
				{
					this.mButtonCurrentFocus.SetState(UIButtonColor.State.Normal, true);
				}
				else
				{
					this.mButtonCurrentFocus.SetState(UIButtonColor.State.Disabled, true);
				}
			}
			this.mButtonCurrentFocus = changeTarget;
			if (this.mButtonCurrentFocus != null)
			{
				this.mButtonCurrentFocus.SetState(UIButtonColor.State.Hover, true);
			}
		}

		public void StartState()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), this);
		}

		public void CloseState()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.mButtonFocasable = null;
			this.mKeyController = null;
			this.mIsValidBuy = false;
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), this);
		}

		public void Release()
		{
			this.mButtonManager = null;
			this.mPanelThis = null;
			this.mButton_Negative.onClick.Clear();
			this.mButton_Negative = null;
			this.mButton_Positive.onClick.Clear();
			this.mButton_Positive = null;
			this.mKeyController = null;
			this.mIsValidBuy = false;
		}

		public void SetOnRequestChangeScene(Action onRequestChangeScene)
		{
			this.mOnRequestChangeScene = onRequestChangeScene;
		}

		private void OnRequestChangeScene()
		{
			if (this.mOnRequestChangeScene != null)
			{
				this.mOnRequestChangeScene.Invoke();
			}
		}

		private void OnDestroy()
		{
			this.mButtonManager = null;
			this.mPanelThis = null;
			this.mButton_Negative = null;
			this.mButton_Positive = null;
			this.mLabel_Name = null;
			this.mLabel_Coin_From = null;
			this.mLabel_Coin_To = null;
			this.mLabel_Price = null;
			this.mTransform_Configuable = null;
			this.mOnRequestBackToRoot = null;
			this.mOnSelectPositiveListener = null;
			this.mOnSelectNegativeListener = null;
			this.mButtonCurrentFocus = null;
			this.mOnRequestChangeScene = null;
			this.mKeyController = null;
			this.mButtonFocasable = null;
			this.mMst_bgm_jukebox = null;
		}
	}
}
