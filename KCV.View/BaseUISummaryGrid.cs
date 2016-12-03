using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.View
{
	public class BaseUISummaryGrid<View, Model> : MonoBehaviour where View : BaseUISummary<Model> where Model : class
	{
		[SerializeField]
		private int MAX_VIEW_ITEMS = 5;

		private int mCurrentPageIndex;

		[SerializeField]
		private View mPrefab;

		[SerializeField]
		private UIGrid mTarget;

		private Model[] mModels;

		private Stack<Transform> mCacheListObjects;

		public virtual void Initialize(Model[] models)
		{
			this.mCacheListObjects = new Stack<Transform>(this.MAX_VIEW_ITEMS);
			this.mModels = models;
			this.ClearViews();
		}

		public void CreateView(Model[] models)
		{
			int num = 0;
			for (int i = 0; i < models.Length; i++)
			{
				Model model = models[i];
				Transform transform = null;
				if (0 < this.mCacheListObjects.get_Count())
				{
					transform = this.mCacheListObjects.Pop();
				}
				View view = (View)((object)null);
				if (transform == null)
				{
					view = this.GenerateView(this.mTarget, this.mPrefab, model);
				}
				else
				{
					transform.get_gameObject().SetActive(true);
					view = transform.GetComponent<View>();
				}
				view.get_transform().set_parent(this.mTarget.get_transform());
				view.Initialize(num++, model);
			}
		}

		public void ClearViews()
		{
			List<Transform> childList = this.mTarget.GetChildList();
			using (List<Transform>.Enumerator enumerator = childList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform current = enumerator.get_Current();
					this.mCacheListObjects.Push(current);
					current.set_parent(null);
					current.get_gameObject().SetActive(false);
				}
			}
			while (0 < this.mCacheListObjects.get_Count())
			{
				NGUITools.Destroy(this.mCacheListObjects.Pop().get_gameObject());
			}
		}

		public virtual View GenerateView(UIGrid target, View prefab, Model model)
		{
			return Util.Instantiate(prefab.get_gameObject(), target.get_gameObject(), false, false).GetComponent<View>();
		}

		public virtual void OnFinishedCreateViews()
		{
			View[] summaryViews = this.GetSummaryViews();
			this.mTarget.Reposition();
			View[] array = summaryViews;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				view.Show();
			}
		}

		public virtual View[] GetSummaryViews()
		{
			return this.mTarget.get_gameObject().GetComponentsInChildren<View>();
		}

		public virtual View GetSummaryView(int index)
		{
			View[] summaryViews = this.GetSummaryViews();
			if (summaryViews == null)
			{
				return (View)((object)null);
			}
			if (index < 0)
			{
				return (View)((object)null);
			}
			if (summaryViews.Length <= index)
			{
				return (View)((object)null);
			}
			return summaryViews[index];
		}

		private Model[] GetPageInModels(int pageIndex)
		{
			return Enumerable.ToArray<Model>(Enumerable.Take<Model>(Enumerable.Skip<Model>(this.mModels, pageIndex * this.MAX_VIEW_ITEMS), this.MAX_VIEW_ITEMS));
		}

		private void ChangePage(int pageIndex)
		{
			this.mCurrentPageIndex = pageIndex;
			Model[] pageInModels = this.GetPageInModels(pageIndex);
			this.ClearViews();
			this.CreateView(pageInModels);
			this.mTarget.Reposition();
			this.OnFinishedCreateViews();
		}

		public int GetPageSize()
		{
			int num = this.mModels.Length;
			int num2 = num / this.MAX_VIEW_ITEMS;
			return num2 + ((num % this.MAX_VIEW_ITEMS != 0) ? 1 : 0);
		}

		public virtual bool GoToPage(int pageIndex)
		{
			if (pageIndex < 0)
			{
				return false;
			}
			if (this.GetPageSize() <= pageIndex)
			{
				return false;
			}
			this.ChangePage(pageIndex);
			return true;
		}

		public int GetCurrentPageIndex()
		{
			return this.mCurrentPageIndex;
		}

		public int GetCurrentViewCount()
		{
			return this.GetSummaryViews().Length;
		}

		private void OnDestroy()
		{
			this.mPrefab = (View)((object)null);
			this.mTarget = null;
			this.mModels = null;
			this.mCacheListObjects = null;
		}
	}
}
