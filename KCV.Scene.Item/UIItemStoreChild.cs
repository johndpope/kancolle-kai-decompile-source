using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIWidget))]
	public class UIItemStoreChild : MonoBehaviour, UIScrollListItem<ItemStoreModel, UIItemStoreChild>
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Description;

		[SerializeField]
		private UITexture mTexture_Icon;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UITexture mTexture_Background;

		private int mRealIndex;

		private ItemStoreModel mItemStoreModel;

		[Header("DEBUG"), SerializeField]
		private int msterId;

		private Action<UIItemStoreChild> mOnTouchListener;

		private Transform mTransform;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 1E-07f;
		}

		public void Release()
		{
			this.mLabel_Name.text = string.Empty;
			this.mLabel_Price.text = string.Empty;
			this.mLabel_Description.text = string.Empty;
			this.mTexture_Icon.mainTexture = null;
		}

		public void Initialize(int realIndex, ItemStoreModel model)
		{
			this.msterId = model.MstId;
			this.mItemStoreModel = model;
			this.mRealIndex = realIndex;
			this.mLabel_Name.text = model.Name;
			this.mLabel_Price.text = model.Price.ToString();
			this.mLabel_Description.text = model.Description;
			this.mTexture_Icon.mainTexture = UserInterfaceItemManager.RequestItemStoreIcon(this.mItemStoreModel.MstId);
			if (model.Count == 0)
			{
				this.mWidgetThis.alpha = 0.45f;
			}
			else
			{
				this.mWidgetThis.alpha = 1f;
			}
		}

		public void InitializeDefault(int realIndex)
		{
			this.mRealIndex = realIndex;
			this.mItemStoreModel = null;
			this.mLabel_Name.text = string.Empty;
			this.mLabel_Price.text = string.Empty;
			this.mLabel_Description.text = string.Empty;
			this.mTexture_Icon.mainTexture = null;
			this.mWidgetThis.alpha = 1E-07f;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}

		public ItemStoreModel GetModel()
		{
			return this.mItemStoreModel;
		}

		public int GetHeight()
		{
			return 108;
		}

		public void SetOnTouchListener(Action<UIItemStoreChild> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		[Obsolete("Inspector上で設定して使用します")]
		private void OnClick()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Background.get_gameObject(), true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Background.get_gameObject(), false);
		}

		public Transform GetTransform()
		{
			if (this.mTransform == null)
			{
				this.mTransform = base.get_transform();
			}
			return this.mTransform;
		}

		private void OnDestroy()
		{
			if (this.mTexture_Icon != null)
			{
				this.mTexture_Icon.mainTexture = null;
			}
			if (this.mLabel_Name != null)
			{
				this.mLabel_Name.text = string.Empty;
			}
			if (this.mLabel_Description != null)
			{
				this.mLabel_Description.text = string.Empty;
			}
			if (this.mLabel_Price != null)
			{
				this.mLabel_Price.text = string.Empty;
			}
			if (this.mWidgetThis != null)
			{
				this.mWidgetThis.RemoveFromPanel();
			}
			if (this.mTexture_Background != null)
			{
				this.mTexture_Background.mainTexture = null;
			}
			this.mWidgetThis = null;
			this.mLabel_Name = null;
			this.mLabel_Description = null;
			this.mTexture_Icon = null;
			this.mLabel_Price = null;
			this.mTexture_Background = null;
		}
	}
}
