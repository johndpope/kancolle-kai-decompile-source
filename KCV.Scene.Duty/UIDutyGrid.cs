using DG.Tweening;
using KCV.Display;
using KCV.Utils;
using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyGrid : BaseUISummaryGrid<UIDutySummary, DutyModel>
	{
		private enum ChangeType
		{
			None,
			Left,
			Right,
			Update
		}

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		private KeyControl mKeyController;

		private UIDutySummary mHoverSummary;

		private UIDutySummary.UIDutySummaryAction mSummarySelectedCallBack;

		private UIDutyGrid.ChangeType mListChangeType;

		private Action mOnChangePageDutyGrid;

		private void Awake()
		{
			this.mUIDisplaySwipeEventRegion.SetOnSwipeListener(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.OnSwipeEventListener));
		}

		private void OnSwipeEventListener(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (this.mKeyController == null)
			{
				return;
			}
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				float num = 0.1f;
				if (num < Math.Abs(movePercentageX))
				{
					if (0f < movePercentageX)
					{
						this.MoveToLeftPage();
					}
					else if (movePercentageX < 0f)
					{
						this.MoveToRightPage();
					}
				}
			}
		}

		public void SetOnSummarySelectedCallBack(UIDutySummary.UIDutySummaryAction summaryActionCallBack)
		{
			this.mSummarySelectedCallBack = summaryActionCallBack;
		}

		public override void Initialize(DutyModel[] models)
		{
			this.mListChangeType = UIDutyGrid.ChangeType.Update;
			base.Initialize(models);
			this.OnChangePage();
		}

		public override UIDutySummary GenerateView(UIGrid target, UIDutySummary prefab, DutyModel model)
		{
			UIDutySummary uIDutySummary = base.GenerateView(target, prefab, model);
			uIDutySummary.SetCallBackSummaryAction(new UIDutySummary.UIDutySummaryAction(this.UIDutySummaryActionCallBack));
			return uIDutySummary;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					if (this.mHoverSummary != null)
					{
						int num = this.mHoverSummary.GetIndex() - 1;
						if (0 <= num)
						{
							this.ChangeHoverSummary(this.GetSummaryView(num), true);
						}
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					if (this.mHoverSummary != null)
					{
						int num2 = this.mHoverSummary.GetIndex() + 1;
						if (num2 < this.GetSummaryViews().Length)
						{
							this.ChangeHoverSummary(this.GetSummaryView(num2), true);
						}
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mSummarySelectedCallBack != null && this.mHoverSummary != null)
					{
						this.mSummarySelectedCallBack(UIDutySummary.SelectType.Hover, this.mHoverSummary);
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
				else if (this.mKeyController.keyState.get_Item(5).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.MoveToLeftPage();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.MoveToRightPage();
				}
				else if (this.mKeyController.keyState.get_Item(2).down && this.mSummarySelectedCallBack != null && this.mHoverSummary != null)
				{
					this.mSummarySelectedCallBack(UIDutySummary.SelectType.CallDetail, this.mHoverSummary);
				}
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchMoveLeftPage()
		{
			if (this.mKeyController == null)
			{
				return;
			}
			this.MoveToLeftPage();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchMoveRightPage()
		{
			if (this.mKeyController == null)
			{
				return;
			}
			this.MoveToRightPage();
		}

		private void MoveToLeftPage()
		{
			this.mListChangeType = UIDutyGrid.ChangeType.Left;
			if (this.GoToPage(base.GetCurrentPageIndex() - 1))
			{
				this.OnChangePage();
				this.ChangeHoverSummary(this.GetSummaryView(0), false);
				this.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void MoveToRightPage()
		{
			this.mListChangeType = UIDutyGrid.ChangeType.Right;
			if (this.GoToPage(base.GetCurrentPageIndex() + 1))
			{
				this.OnChangePage();
				this.ChangeHoverSummary(this.GetSummaryView(0), false);
				this.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void UIDutySummaryActionCallBack(UIDutySummary.SelectType type, UIDutySummary targetObject)
		{
			this.ChangeHoverSummary(targetObject, false);
			if (this.mSummarySelectedCallBack != null)
			{
				this.mSummarySelectedCallBack(type, targetObject);
			}
		}

		public override bool GoToPage(int pageIndex)
		{
			return this.GoToPage(pageIndex, true);
		}

		public bool GoToPage(int pageIndex, bool focus)
		{
			bool flag = base.GoToPage(pageIndex);
			if (flag)
			{
				this.mListChangeType = UIDutyGrid.ChangeType.Update;
				if (focus)
				{
					this.ChangeHoverSummary(this.GetSummaryView(0), false);
				}
			}
			return flag;
		}

		private void ChangeHoverSummary(UIDutySummary summary, bool changedSEFlag)
		{
			if (this.mHoverSummary != null)
			{
				this.mHoverSummary.RemoveHover();
				this.mHoverSummary.DepthBack();
			}
			this.mHoverSummary = summary;
			if (this.mHoverSummary != null)
			{
				if (changedSEFlag)
				{
					this.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.mHoverSummary.Hover();
				this.mHoverSummary.DepthFront();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public KeyControl GetKeyController()
		{
			if (this.mKeyController == null)
			{
				this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			}
			return this.mKeyController;
		}

		public void FirstFocus()
		{
			UIDutySummary summaryView = this.GetSummaryView(0);
			this.ChangeHoverSummary(summaryView, false);
		}

		private void PlaySE(SEFIleInfos seType)
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

		public override void OnFinishedCreateViews()
		{
			this.OnFinishedCreateViewsCoroutine();
		}

		private void OnFinishedCreateViewsCoroutine()
		{
			UIDutySummary[] summaryViews = this.GetSummaryViews();
			int num = 0;
			UIDutyGrid.ChangeType changeType = this.mListChangeType;
			int num2;
			if (changeType != UIDutyGrid.ChangeType.Left)
			{
				if (changeType != UIDutyGrid.ChangeType.Right)
				{
					num2 = 0;
				}
				else
				{
					num2 = 1;
				}
			}
			else
			{
				num2 = -1;
			}
			UIDutySummary[] array = summaryViews;
			for (int i = 0; i < array.Length; i++)
			{
				UIDutySummary uIDutySummary = array[i];
				uIDutySummary.SetDepth(summaryViews.Length - num);
				Vector3 localPosition = uIDutySummary.get_gameObject().get_transform().get_localPosition();
				Sequence sequence = DOTween.Sequence();
				switch (this.mListChangeType)
				{
				case UIDutyGrid.ChangeType.None:
				{
					uIDutySummary.get_gameObject().get_transform().set_localPosition(new Vector3(localPosition.x, 0f, localPosition.z));
					Sequence sequence2 = DOTween.Sequence();
					Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(uIDutySummary.get_transform(), localPosition.y, 0.6f, false), 15);
					TweenSettingsExtensions.Append(sequence2, tween);
					UIDutySummary dsum = uIDutySummary;
					if (num != 0)
					{
						Tween tween2 = DOVirtual.Float(0f, 1f, 0.6f, delegate(float alpha)
						{
							dsum.GetPanel().alpha = alpha;
						});
						TweenSettingsExtensions.Join(sequence2, tween2);
					}
					uIDutySummary.Show();
					TweenSettingsExtensions.Join(sequence, sequence2);
					break;
				}
				case UIDutyGrid.ChangeType.Left:
				case UIDutyGrid.ChangeType.Right:
				{
					uIDutySummary.get_gameObject().get_transform().set_localPosition(new Vector3(960f * (float)num2, localPosition.y, localPosition.z));
					Tween tween3 = TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveX(uIDutySummary.get_transform(), 0f, 0.6f, false), 0.05f * (float)num), 15);
					uIDutySummary.Show();
					TweenSettingsExtensions.Join(sequence, tween3);
					break;
				}
				case UIDutyGrid.ChangeType.Update:
				{
					uIDutySummary.get_gameObject().get_transform().set_localPosition(new Vector3(localPosition.x, 0f, localPosition.z));
					Sequence sequence3 = DOTween.Sequence();
					Tween tween4 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(uIDutySummary.get_transform(), localPosition.y, 0.6f, false), 15);
					TweenSettingsExtensions.Append(sequence3, tween4);
					UIDutySummary dsumt = uIDutySummary;
					if (num != 0)
					{
						Tween tween5 = DOVirtual.Float(0f, 1f, 0.7f, delegate(float alpha)
						{
							dsumt.GetPanel().alpha = alpha;
						});
						TweenSettingsExtensions.Join(sequence3, tween5);
					}
					uIDutySummary.Show();
					TweenSettingsExtensions.Join(sequence, sequence3);
					break;
				}
				}
				num++;
			}
		}

		internal void SetOnChangePageListener(Action onChangePageDutyGrid)
		{
			this.mOnChangePageDutyGrid = onChangePageDutyGrid;
		}

		private void OnChangePage()
		{
			if (this.mOnChangePageDutyGrid != null)
			{
				this.mOnChangePageDutyGrid.Invoke();
			}
		}

		private void OnDestroy()
		{
			this.mUIDisplaySwipeEventRegion = null;
			this.mKeyController = null;
			this.mHoverSummary = null;
			this.mSummarySelectedCallBack = null;
		}
	}
}
