using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemListChildDetail : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Description;

		[SerializeField]
		private UISprite mSprite_Icon;

		[SerializeField]
		private UIButton mButton_Use;

		[SerializeField]
		private UIItemYousei mItemYousei;

		private KeyControl mKeyController;

		private Action<ItemlistModel> mUseCallBack;

		private Action mOnCalcelCallBack;

		private ItemlistModel mModel;

		private void Awake()
		{
			this.mSprite_Icon.spriteName = string.Empty;
		}

		public void UpdateInfo(ItemlistModel model)
		{
			this.mModel = model;
			bool flag = this.Usable();
			if (flag)
			{
				base.get_transform().SetActive(true);
				this.mLabel_Name.text = model.Name;
				this.mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(15, 1, model.Description);
				this.mSprite_Icon.spriteName = string.Format("item_{0}", this.mModel.MstId);
				bool flag2 = this.mModel.IsUsable();
				if (flag2)
				{
					this.mButton_Use.get_transform().SetActive(true);
				}
				else
				{
					this.mButton_Use.get_transform().SetActive(false);
				}
			}
			else
			{
				base.get_transform().SetActive(false);
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnUse();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnCancel();
				}
			}
		}

		public void OnTouchUse()
		{
			this.OnUse();
		}

		public bool Usable()
		{
			return this.mModel != null && 0 < this.mModel.Count;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
			if (this.mKeyController != null)
			{
				this.mButton_Use.SetState(UIButtonColor.State.Hover, true);
			}
		}

		public void SetOnUseCallBack(Action<ItemlistModel> onUseCallBack)
		{
			this.mUseCallBack = onUseCallBack;
		}

		private void OnUse()
		{
			if (this.mUseCallBack != null)
			{
				this.mUseCallBack.Invoke(this.mModel);
				this.mButton_Use.SetState(UIButtonColor.State.Normal, true);
			}
		}

		public void SetOnCancelCallBack(Action onCancelCallBack)
		{
			this.mOnCalcelCallBack = onCancelCallBack;
		}

		private void OnCancel()
		{
			if (this.mOnCalcelCallBack != null)
			{
				this.mOnCalcelCallBack.Invoke();
				this.mButton_Use.SetState(UIButtonColor.State.Normal, true);
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Name);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Description);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Icon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Use);
			this.mItemYousei = null;
			this.mKeyController = null;
			this.mUseCallBack = null;
			this.mOnCalcelCallBack = null;
			this.mModel = null;
		}

		internal void Clean()
		{
			this.mModel = null;
			this.mLabel_Name.text = string.Empty;
			this.mLabel_Description.text = string.Empty;
			this.mSprite_Icon.spriteName = string.Empty;
		}
	}
}
