using DG.Tweening;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public class UICircleMenu<Model, View> : MonoBehaviour where View : UICircleCategory<Model>
	{
		[SerializeField]
		private View mPrefab_View;

		[SerializeField]
		private Transform mTransform_Categorys;

		[SerializeField]
		private Vector3[] mVector3DefaultPosition;

		[SerializeField]
		private int[] mDefaultDepths;

		protected View mCenterView;

		[SerializeField]
		private float ANIMATION_TIME_MOVE = 0.2f;

		[SerializeField]
		private Ease ANIMATION_EASE_MOVE = 3;

		private int mDisplayCategoryValue;

		private UIPanel mPanelThis;

		protected Model[] mCategories;

		public bool mIsAnimationNow
		{
			get;
			protected set;
		}

		public void Initialize(Model[] categories, int displayCategoryValue)
		{
			this.mCategories = categories;
			this.mDisplayCategoryValue = displayCategoryValue;
			int num = 0;
			for (int i = 0; i < categories.Length; i++)
			{
				Model category = categories[i];
				View component = Util.Instantiate(this.mPrefab_View.get_gameObject(), this.mTransform_Categorys.get_gameObject(), false, false).GetComponent<View>();
				component.Initialize(num, category);
				num++;
			}
			View[] views = this.GetViews();
			this.InitDefaultPosition(views, 6f);
			this.InitDefaultDepth(views);
			int num2 = 0;
			View[] array = views;
			for (int j = 0; j < array.Length; j++)
			{
				View view = array[j];
				view.get_transform().set_localPosition(this.mVector3DefaultPosition[num2]);
				num2++;
			}
			this.Reposition(0);
			this.mCenterView = views[0];
			View[] array2 = views;
			for (int k = 0; k < array2.Length; k++)
			{
				View view2 = array2[k];
				if (this.mCenterView.Equals(view2))
				{
					view2.OnFirstDisplay(this.ANIMATION_TIME_MOVE, true, 30);
				}
				else
				{
					view2.OnFirstDisplay(this.ANIMATION_TIME_MOVE, false, 30);
				}
			}
			this.mIsAnimationNow = false;
		}

		private void InitDefaultPosition(View[] views, float radius)
		{
			this.mVector3DefaultPosition = new Vector3[views.Length];
			int num = views.Length / 2;
			for (int i = 0; i < views.Length; i++)
			{
				float num2 = (float)this.GetLoopIndex(i) * 3.14159274f * 2f / (float)views.Length;
				float num3 = (float)(360 / views.Length) * radius;
				float num4 = Mathf.Sin(num2);
				float num5 = 0f;
				float num6 = 0f;
				this.mVector3DefaultPosition[i] = new Vector3(num4, num5, num6) * num3;
			}
			this.InitOriginalDefaultPosition(ref this.mVector3DefaultPosition);
		}

		protected virtual void InitOriginalDefaultPosition(ref Vector3[] defaultPositions)
		{
		}

		private void InitDefaultDepth(View[] views)
		{
			this.mDefaultDepths = new int[views.Length];
			int num = views.Length / 2;
			for (int i = 0; i < views.Length; i++)
			{
				if (i != 0)
				{
					if (i <= num)
					{
						this.mDefaultDepths[i] = num - i;
					}
					else if (views.Length % 2 == 1 && i == num + 1)
					{
						this.mDefaultDepths[i] = 0;
					}
					else
					{
						this.mDefaultDepths[i] = num - (views.Length - i) % num;
					}
				}
				else
				{
					this.mDefaultDepths[i] = num;
				}
			}
		}

		public Vector3[] GetDefaultPositions()
		{
			return this.mVector3DefaultPosition;
		}

		public void Next()
		{
			View[] views = this.GetViews();
			int num = Array.IndexOf<View>(views, this.mCenterView);
			int loopIndex = this.GetLoopIndex(num + 1);
			this.ChangeCenterView(views[loopIndex], true);
			this.Reposition(loopIndex);
		}

		public void Prev()
		{
			View[] views = this.GetViews();
			int num = Array.IndexOf<View>(views, this.mCenterView);
			int loopIndex = this.GetLoopIndex(num - 1);
			this.ChangeCenterView(views[loopIndex], true);
			this.Reposition(loopIndex);
		}

		protected int GetLoopIndex(int value)
		{
			if (value == 0)
			{
				return 0;
			}
			if (value == -1)
			{
				return this.mCategories.Length - 1;
			}
			if (value < 0)
			{
				return this.mCategories.Length + value;
			}
			return value % this.mCategories.Length;
		}

		public void Show()
		{
			View[] views = this.GetViews();
			View[] array = views;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				view.Show();
			}
		}

		private void Reposition(int centerIndex)
		{
			View[] views = this.mTransform_Categorys.GetComponentsInChildren<View>();
			int num = views.Length / 2;
			int i;
			for (i = 0; i < views.Length; i++)
			{
				int nextDepth = this.mDefaultDepths[this.GetLoopIndex(i - centerIndex)];
				if (nextDepth < 1)
				{
					views[i].OnOutDisplay(this.ANIMATION_TIME_MOVE, this.ANIMATION_EASE_MOVE, delegate
					{
						views[i].SetDepth(nextDepth, false);
					});
				}
				else if (views[i].GetDepth() < 1 && 1 <= nextDepth)
				{
					views[i].SetDepth(nextDepth, false);
					views[i].OnInDisplay(this.ANIMATION_TIME_MOVE, this.ANIMATION_EASE_MOVE, delegate
					{
					});
				}
				this.OnMoveView(views[i], this.GetDefaultPositions()[this.GetLoopIndex(i - centerIndex)]);
			}
		}

		private void OnMoveView(View view, Vector3 moveTo)
		{
			this.mIsAnimationNow = true;
			TweenExtensions.PlayForward(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(view.get_transform(), moveTo, this.ANIMATION_TIME_MOVE, false), this.ANIMATION_EASE_MOVE), delegate
			{
				this.mIsAnimationNow = false;
			}));
		}

		private void ChangeCenterView(View nextCenterView, bool needSe)
		{
			if (this.mCenterView != null)
			{
				this.mCenterView.OnOtherThanCenter(this.ANIMATION_TIME_MOVE, this.ANIMATION_EASE_MOVE);
			}
			this.mCenterView = nextCenterView;
			if (this.mCenterView != null)
			{
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.mCenterView.OnCenter(this.ANIMATION_TIME_MOVE, this.ANIMATION_EASE_MOVE);
			}
		}

		public virtual void OnReposition(int nextCenterPosition, View[] views)
		{
		}

		public View[] GetViews()
		{
			return this.mTransform_Categorys.GetComponentsInChildren<View>();
		}
	}
}
