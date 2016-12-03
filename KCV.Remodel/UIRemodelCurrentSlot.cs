using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelCurrentSlot : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.2f;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UISprite ItemIcon;

		[SerializeField]
		private UISprite mLock_Icon;

		private SlotitemModel slotItem;

		private Vector3 showPos = new Vector3(-240f, -170f);

		private Vector3 hidePos = new Vector3(-840f, -170f);

		private void Awake()
		{
			this.labelName.text = string.Empty;
			this.ItemIcon.spriteName = string.Empty;
			this.slotItem = null;
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Init(SlotitemModel slotItem)
		{
			this.slotItem = slotItem;
			if (slotItem != null)
			{
				this.labelName.text = slotItem.Name;
				if (slotItem.IsLocked())
				{
					this.mLock_Icon.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLock_Icon.get_transform().set_localScale(Vector3.get_zero());
				}
				this.ItemIcon.spriteName = "icon_slot" + slotItem.Type4.ToString();
			}
			else
			{
				this.mLock_Icon.get_transform().set_localScale(Vector3.get_zero());
			}
		}

		public void Show()
		{
			this.Show(true);
			if (this.slotItem != null)
			{
				base.get_gameObject().SetActive(true);
			}
			else
			{
				base.get_gameObject().SetActive(false);
			}
		}

		public void Show(bool animation)
		{
			base.get_gameObject().SetActive(true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, null);
			}
			else
			{
				base.get_transform().set_localPosition(this.showPos);
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, delegate
				{
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.ItemIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLock_Icon);
			this.slotItem = null;
		}
	}
}
