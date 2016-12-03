using Common.Enum;
using DG.Tweening;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIButtonManager)), RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleList : MonoBehaviour
	{
		private enum AnimationType
		{
			ShowHide
		}

		private UIWidget mWidgetThis;

		[SerializeField]
		private UIPracticeBattleListChild[] mUIPracticeBattleListChildren_PracticeTargetAll;

		[SerializeField]
		private UIGrid mGrid_Focasable;

		[SerializeField]
		private Transform mTransform_TouchBackArea;

		[SerializeField]
		private Transform mTransform_ObjectPool;

		private UIPracticeBattleListChild[] mUIPracticeBattleListChildren_Focasable;

		private List<DeckModel> mRivalDecks;

		private UIButtonManager mUIButtonManager;

		private Action<DeckModel, List<IsGoCondition>> mOnSelectedDeckListener;

		private UIPracticeBattleListChild mFocus;

		private Action mOnBackCallBack;

		private KeyControl mKeyController;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mUIButtonManager = base.GetComponent<UIButtonManager>();
			this.mUIButtonManager.IndexChangeAct = delegate
			{
				UIPracticeBattleListChild component = this.mUIButtonManager.nowForcusButton.get_transform().get_parent().GetComponent<UIPracticeBattleListChild>();
				this.ChangeFocus(component, false);
			};
			this.mWidgetThis.alpha = 0f;
		}

		public void Initialize(List<DeckModel> rivalDecks, PracticeManager deckCheckUtil)
		{
			this.mRivalDecks = rivalDecks;
			base.get_transform().set_localScale(Vector3.get_one());
			List<UIPracticeBattleListChild> list = new List<UIPracticeBattleListChild>();
			int num = 0;
			UIPracticeBattleListChild[] array = this.mUIPracticeBattleListChildren_PracticeTargetAll;
			for (int i = 0; i < array.Length; i++)
			{
				UIPracticeBattleListChild uIPracticeBattleListChild = array[i];
				uIPracticeBattleListChild.alpha = 0f;
				uIPracticeBattleListChild.get_transform().set_localPosition(Vector3.get_zero());
				uIPracticeBattleListChild.get_transform().set_parent(this.mTransform_ObjectPool);
				uIPracticeBattleListChild.SetActive(false);
				uIPracticeBattleListChild.SetOnClickListener(null);
				bool flag = num < this.mRivalDecks.get_Count();
				if (flag)
				{
					DeckModel deckModel = this.mRivalDecks.get_Item(num);
					List<IsGoCondition> conditions = deckCheckUtil.IsValidPractice(deckModel.Id);
					uIPracticeBattleListChild.Initialize(deckModel, conditions);
					uIPracticeBattleListChild.SetOnClickListener(new Action<UIPracticeBattleListChild>(this.OnClickChild));
					uIPracticeBattleListChild.alpha = 1f;
					uIPracticeBattleListChild.get_transform().set_parent(this.mGrid_Focasable.get_transform());
					uIPracticeBattleListChild.get_transform().set_localPosition(Vector3.get_zero());
					uIPracticeBattleListChild.get_transform().set_localScale(Vector3.get_one());
					uIPracticeBattleListChild.SetActive(true);
					uIPracticeBattleListChild.ParentHasChanged();
					list.Add(uIPracticeBattleListChild);
				}
				num++;
			}
			this.mUIPracticeBattleListChildren_Focasable = list.ToArray();
		}

		private void OnClickChild(UIPracticeBattleListChild child)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.ChangeFocus(child, true);
			this.OnDeckSelectedDeck(child.GetDeckModel(), child.GetConditions());
		}

		private void OnDeckSelectedDeck(DeckModel deckModel, List<IsGoCondition> conditions)
		{
			if (this.mOnSelectedDeckListener != null)
			{
				this.mOnSelectedDeckListener.Invoke(deckModel, conditions);
			}
		}

		public void SetOnSelectedDeckListener(Action<DeckModel, List<IsGoCondition>> onSelectedDeckListener)
		{
			this.mOnSelectedDeckListener = onSelectedDeckListener;
		}

		public void Show(Action onFinishedanimation)
		{
			this.ChangeFocus(this.mUIPracticeBattleListChildren_Focasable[0], false);
			if (DOTween.IsTweening(UIPracticeBattleList.AnimationType.ShowHide))
			{
				DOTween.Kill(UIPracticeBattleList.AnimationType.ShowHide, false);
			}
			Tween tween = TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), UIPracticeBattleList.AnimationType.ShowHide), DOVirtual.Float(this.mWidgetThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mWidgetThis.alpha = alpha;
			})), DOVirtual.Float(0f, 66f, 0.4f, delegate(float cellHeight)
			{
				this.mGrid_Focasable.cellHeight = cellHeight;
				this.mGrid_Focasable.Reposition();
			})), delegate
			{
				if (onFinishedanimation != null)
				{
					onFinishedanimation.Invoke();
				}
			});
		}

		private void ChangeFocus(UIPracticeBattleListChild child, bool needSe)
		{
			if (this.mFocus != null)
			{
				this.mFocus.RemoveHover();
			}
			this.mFocus = child;
			if (this.mFocus != null)
			{
				this.mFocus.Hover();
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
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

		public void Hide(Action onFinishedAnimation)
		{
			if (DOTween.IsTweening(UIPracticeBattleList.AnimationType.ShowHide))
			{
				DOTween.Kill(UIPracticeBattleList.AnimationType.ShowHide, false);
			}
			Tween tween = DOVirtual.Float(1f, 0f, 0.2f, delegate(float alpha)
			{
				this.mWidgetThis.alpha = alpha;
			});
			TweenSettingsExtensions.SetId<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), tween), ShortcutExtensions.DOScale(base.get_transform(), new Vector3(0.9f, 0.9f), 0.4f)), UIPracticeBattleList.AnimationType.ShowHide);
			if (onFinishedAnimation != null)
			{
				onFinishedAnimation.Invoke();
			}
		}

		public void SetOnBackCallBack(Action OnCancelBattleTargetSelect)
		{
			this.mOnBackCallBack = OnCancelBattleTargetSelect;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
			if (this.mKeyController != null)
			{
				this.mTransform_TouchBackArea.SetActive(true);
			}
			else
			{
				this.mTransform_TouchBackArea.SetActive(false);
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnClickChild(this.mFocus);
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnBack();
				}
				else if (this.mKeyController.keyState.get_Item(8).down)
				{
					int num = Array.IndexOf<UIPracticeBattleListChild>(this.mUIPracticeBattleListChildren_Focasable, this.mFocus);
					int num2 = num - 1;
					if (0 <= num2)
					{
						this.ChangeFocus(this.mUIPracticeBattleListChildren_Focasable[num2], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					int num3 = Array.IndexOf<UIPracticeBattleListChild>(this.mUIPracticeBattleListChildren_Focasable, this.mFocus);
					int num4 = num3 + 1;
					if (num4 < this.mUIPracticeBattleListChildren_Focasable.Length)
					{
						this.ChangeFocus(this.mUIPracticeBattleListChildren_Focasable[num4], true);
					}
				}
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(UIPracticeBattleList.AnimationType.ShowHide))
			{
				DOTween.Kill(UIPracticeBattleList.AnimationType.ShowHide, false);
			}
			this.mWidgetThis = null;
			this.mUIPracticeBattleListChildren_PracticeTargetAll = null;
			this.mGrid_Focasable = null;
			this.mTransform_TouchBackArea = null;
			this.mTransform_ObjectPool = null;
			this.mUIPracticeBattleListChildren_Focasable = null;
			this.mRivalDecks = null;
			this.mUIButtonManager = null;
		}
	}
}
