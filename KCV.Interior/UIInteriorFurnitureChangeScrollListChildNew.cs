using KCV.View.ScrollView;
using System;
using UnityEngine;

namespace KCV.Interior
{
	[RequireComponent(typeof(UIWidget))]
	public class UIInteriorFurnitureChangeScrollListChildNew : MonoBehaviour, UIScrollListItem<UIInteriorFurnitureChangeScrollListChildModelNew, UIInteriorFurnitureChangeScrollListChildNew>
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private Transform mEquipMark;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private GameObject mBackground;

		private Transform mCachedTransform;

		private int mRealIndex;

		private UIInteriorFurnitureChangeScrollListChildModelNew mModel;

		private Action<UIInteriorFurnitureChangeScrollListChildNew> mOnTouchListener;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
		}

		public void Initialize(int realIndex, UIInteriorFurnitureChangeScrollListChildModelNew model)
		{
			this.mRealIndex = realIndex;
			this.mModel = model;
			this.mLabel_Name.text = model.GetName();
			bool flag = model.IsConfiguredInDeck();
			if (flag)
			{
				this.mEquipMark.SetActive(true);
			}
			else
			{
				this.mEquipMark.SetActive(false);
			}
			this.mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault(int realIndex)
		{
			this.mRealIndex = realIndex;
			this.mModel = null;
			this.mWidgetThis.alpha = 1E-07f;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}

		public UIInteriorFurnitureChangeScrollListChildModelNew GetModel()
		{
			return this.mModel;
		}

		public int GetHeight()
		{
			return 88;
		}

		public void SetOnTouchListener(Action<UIInteriorFurnitureChangeScrollListChildNew> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		public void Touch()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mBackground, true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mBackground, false);
		}

		public Transform GetTransform()
		{
			if (this.mCachedTransform == null)
			{
				this.mCachedTransform = base.get_transform();
			}
			return this.mCachedTransform;
		}

		private void OnDestroy()
		{
			this.mEquipMark = null;
			this.mLabel_Name = null;
			this.mBackground = null;
			this.mCachedTransform = null;
			this.mModel = null;
		}
	}
}
