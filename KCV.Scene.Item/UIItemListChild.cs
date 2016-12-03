using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIWidget))]
	public class UIItemListChild : MonoBehaviour
	{
		[SerializeField]
		private UISprite mSprite_Icon;

		[SerializeField]
		private UILabel mLabel_Count;

		private UIWidget mWidgetThis;

		private Action<UIItemListChild> mOnTouchListener;

		public ItemlistModel mModel
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mSprite_Icon.spriteName = string.Empty;
		}

		public void Initialize(ItemlistModel model)
		{
			this.mModel = model;
			this.mLabel_Count.text = this.mModel.Count.ToString();
			bool flag = this.mModel != null && 0 < this.mModel.Count;
			if (flag)
			{
				this.mSprite_Icon.SetActive(true);
				this.mLabel_Count.SetActive(true);
				this.mSprite_Icon.spriteName = string.Format("item_{0}", this.mModel.MstId);
				this.mLabel_Count.text = this.mModel.Count.ToString();
			}
			else
			{
				this.mSprite_Icon.spriteName = string.Empty;
				this.mLabel_Count.text = string.Empty;
				this.mSprite_Icon.SetActive(false);
				this.mLabel_Count.SetActive(false);
			}
		}

		public bool IsFosable()
		{
			return this.mModel != null && 0 < this.mModel.Count;
		}

		public void Focus()
		{
			NGUITools.AdjustDepth(base.get_transform().get_gameObject(), 1);
		}

		public void RemoveFocus()
		{
			NGUITools.AdjustDepth(base.get_transform().get_gameObject(), -1);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Icon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Count);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			this.mModel = null;
			this.mOnTouchListener = null;
		}

		public void SetOnTouchListener(Action<UIItemListChild> onClickListener)
		{
			this.mOnTouchListener = onClickListener;
		}

		public void OnTouchItemListChild()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}
	}
}
