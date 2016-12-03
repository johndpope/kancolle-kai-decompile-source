using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelStartRightInfo : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.3f;

		[SerializeField]
		private UITexture shipTypeMarkIcon;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UILabel labelLevel;

		[SerializeField]
		private UiStarManager stars;

		[SerializeField]
		private UIRemodelEquipSlotItem[] slots;

		private ShipModel ship;

		private Vector3 showPos = new Vector3(250f, 0f);

		private Vector3 hidePos = new Vector3(950f, 0f);

		private KeyControl mKeyController;

		private bool isShown;

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
			this.Show(false);
		}

		public void Init(ShipModel ship)
		{
			this.ship = ship;
			this.labelName.text = ship.Name;
			this.labelLevel.text = ship.Level.ToString();
			this.stars.init(ship.Srate);
			this.shipTypeMarkIcon.mainTexture = ResourceManager.LoadShipTypeIcon(ship);
			UIRemodelEquipSlotItem[] array = this.slots;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodelEquipSlotItem uIRemodelEquipSlotItem = array[i];
				uIRemodelEquipSlotItem.Hide();
			}
			int j;
			for (j = 0; j < ship.SlotitemList.get_Count(); j++)
			{
				this.slots[j].Initialize(j, ship);
				this.slots[j].SetOnUIRemodelEquipSlotItemActionListener(new UIRemodelEquipSlotItem.UIRemodelEquipSlotItemAction(this.OnUIRemodelEquipSlotItemActionListener));
				this.slots[j].Show();
			}
			if (ship.HasExSlot())
			{
				this.slots[j].Initialize(j, ship.SlotitemEx, ship);
				this.slots[j].Show();
			}
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			if (animation)
			{
				TweenPosition tweenPosition = RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.3f, delegate
				{
					this.isShown = true;
				});
				tweenPosition.PlayForward();
			}
			else
			{
				this.isShown = true;
				base.get_transform().set_localPosition(this.showPos);
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			this.isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.3f, delegate
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

		private void OnUIRemodelEquipSlotItemActionListener(UIRemodelEquipSlotItem.ActionType actionType, UIRemodelEquipSlotItem actionObject)
		{
			if (this.isShown)
			{
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouShortCut(actionObject.GetModel());
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTypeMarkIcon, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelLevel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTypeMarkIcon, false);
			if (this.slots != null)
			{
				for (int i = 0; i < this.slots.Length; i++)
				{
					this.slots[i] = null;
				}
			}
			this.slots = null;
			this.stars = null;
			this.ship = null;
			this.mKeyController = null;
		}
	}
}
