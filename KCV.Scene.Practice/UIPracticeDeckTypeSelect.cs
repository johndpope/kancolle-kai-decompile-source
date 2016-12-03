using Common.Enum;
using DG.Tweening;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIButtonManager)), RequireComponent(typeof(UIPanel))]
	public class UIPracticeDeckTypeSelect : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private UIButtonManager mButtonManager;

		[SerializeField]
		private UIPracticeDeckTypeSelectChild[] mUIPracticeDeckTypeSelectChildrenAll;

		[SerializeField]
		private UIGrid mGridFocasable;

		[SerializeField]
		private Transform mTransform_TouchBackArea;

		[SerializeField]
		private Transform mTransform_ObjectPools;

		private UIPracticeDeckTypeSelectChild[] mUIPracticeDeckTypeSelectChildrenFocusable;

		private Tween mTweenShowHide;

		private Action<DeckPracticeType> mOnSelectedDeckPracticeTypeCallBack;

		private UIPracticeDeckTypeSelectChild mFocus;

		private Action mOnBackCallBack;

		private KeyControl mKeyController;

		private List<DeckPracticeType> mDeckPracticeTypes;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				UIButton nowForcusButton = this.mButtonManager.nowForcusButton;
				this.ChangeFocus(nowForcusButton.GetComponent<UIPracticeDeckTypeSelectChild>());
			};
			this.mPanelThis.alpha = 0f;
		}

		public void OnDeckTypeSelect(UIPracticeDeckTypeSelectChild selectedView)
		{
			this.ChangeFocus(selectedView);
			this.OnSelectedDeckPracticeType(this.mFocus.GetDeckPracticeType());
		}

		private void ChangeFocus(UIPracticeDeckTypeSelectChild target)
		{
			if (this.mFocus != null)
			{
				this.mFocus.RemoveHover();
			}
			this.mFocus = target;
			if (this.mFocus != null)
			{
				this.mFocus.Hover();
			}
		}

		public void SetOnSelectedDeckPracticeTypeCallBack(Action<DeckPracticeType> OnSelectedDeckPracticeTypeCallBack)
		{
			this.mOnSelectedDeckPracticeTypeCallBack = OnSelectedDeckPracticeTypeCallBack;
		}

		public void SetOnBackCallBack(Action OnBackCallBack)
		{
			this.mOnBackCallBack = OnBackCallBack;
		}

		public void OnTouchBack()
		{
			this.OnBack();
		}

		private void OnBack()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			if (this.mOnBackCallBack != null)
			{
				this.mOnBackCallBack.Invoke();
			}
		}

		private void OnSelectedDeckPracticeType(DeckPracticeType deckPracticeType)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			if (this.mOnSelectedDeckPracticeTypeCallBack != null)
			{
				this.mOnSelectedDeckPracticeTypeCallBack.Invoke(deckPracticeType);
			}
		}

		public void SetKeyController(KeyControl keyControl)
		{
			this.mKeyController = keyControl;
			if (this.mKeyController != null)
			{
				this.mTransform_TouchBackArea.SetActive(true);
			}
			else
			{
				this.mTransform_TouchBackArea.SetActive(false);
			}
		}

		public void Initialize(Dictionary<DeckPracticeType, bool> selectableDeckPracticeTypes)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			this.mGridFocasable.cellHeight = 0f;
			this.mUIPracticeDeckTypeSelectChildrenFocusable = null;
			this.mFocus = null;
			List<UIPracticeDeckTypeSelectChild> list = new List<UIPracticeDeckTypeSelectChild>();
			UIPracticeDeckTypeSelectChild[] array = this.mUIPracticeDeckTypeSelectChildrenAll;
			for (int i = 0; i < array.Length; i++)
			{
				UIPracticeDeckTypeSelectChild uIPracticeDeckTypeSelectChild = array[i];
				uIPracticeDeckTypeSelectChild.get_gameObject().SetActive(false);
				uIPracticeDeckTypeSelectChild.get_transform().set_localPosition(Vector3.get_zero());
				uIPracticeDeckTypeSelectChild.SetOnClickListener(null);
				DeckPracticeType deckPracticeType = uIPracticeDeckTypeSelectChild.GetDeckPracticeType();
				bool flag = selectableDeckPracticeTypes.ContainsKey(deckPracticeType);
				if (flag)
				{
					list.Add(uIPracticeDeckTypeSelectChild);
					uIPracticeDeckTypeSelectChild.get_gameObject().SetActive(true);
					uIPracticeDeckTypeSelectChild.get_transform().set_parent(this.mGridFocasable.get_transform());
					uIPracticeDeckTypeSelectChild.get_transform().set_localPosition(Vector3.get_zero());
					uIPracticeDeckTypeSelectChild.get_transform().set_localScale(Vector3.get_one());
					uIPracticeDeckTypeSelectChild.ParentHasChanged();
					uIPracticeDeckTypeSelectChild.SetOnClickListener(new Action<UIPracticeDeckTypeSelectChild>(this.OnDeckTypeSelect));
				}
				else
				{
					uIPracticeDeckTypeSelectChild.get_transform().set_parent(this.mTransform_ObjectPools);
				}
			}
			this.mUIPracticeDeckTypeSelectChildrenFocusable = list.ToArray();
		}

		public void Show(Action onFinishedanimation)
		{
			this.ChangeFocus(this.mUIPracticeDeckTypeSelectChildrenFocusable[0]);
			if (this.mTweenShowHide != null && TweenExtensions.IsPlaying(this.mTweenShowHide))
			{
				TweenExtensions.Kill(this.mTweenShowHide, false);
			}
			this.mTweenShowHide = TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			})), TweenSettingsExtensions.SetEase<Tweener>(DOVirtual.Float(0f, 72f, 0.4f, delegate(float cellHeight)
			{
				this.mGridFocasable.cellHeight = cellHeight;
				this.mGridFocasable.Reposition();
			}), 12)), delegate
			{
				if (onFinishedanimation != null)
				{
					onFinishedanimation.Invoke();
				}
			});
		}

		public void Hide(Action onFinishedAnimation)
		{
			if (this.mTweenShowHide != null && TweenExtensions.IsPlaying(this.mTweenShowHide))
			{
				TweenExtensions.Kill(this.mTweenShowHide, false);
			}
			this.mTweenShowHide = DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.2f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			});
			TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), this.mTweenShowHide), ShortcutExtensions.DOScale(base.get_transform(), new Vector3(0.9f, 0.9f), 0.4f));
			if (onFinishedAnimation != null)
			{
				onFinishedAnimation.Invoke();
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					int num = Array.IndexOf<UIPracticeDeckTypeSelectChild>(this.mUIPracticeDeckTypeSelectChildrenFocusable, this.mFocus);
					int num2 = num - 1;
					if (0 <= num2)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
						this.ChangeFocus(this.mUIPracticeDeckTypeSelectChildrenFocusable[num2]);
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					int num3 = Array.IndexOf<UIPracticeDeckTypeSelectChild>(this.mUIPracticeDeckTypeSelectChildrenFocusable, this.mFocus);
					int num4 = num3 + 1;
					if (num4 < this.mUIPracticeDeckTypeSelectChildrenFocusable.Length)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
						this.ChangeFocus(this.mUIPracticeDeckTypeSelectChildrenFocusable[num4]);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnDeckTypeSelect(this.mFocus);
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnBack();
				}
			}
		}

		public void DisableButtonAll()
		{
			UIPracticeDeckTypeSelectChild[] array = this.mUIPracticeDeckTypeSelectChildrenAll;
			for (int i = 0; i < array.Length; i++)
			{
				UIPracticeDeckTypeSelectChild uIPracticeDeckTypeSelectChild = array[i];
				uIPracticeDeckTypeSelectChild.Enabled(false);
			}
		}
	}
}
