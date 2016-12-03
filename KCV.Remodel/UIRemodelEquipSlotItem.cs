using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget)), SelectionBase]
	public class UIRemodelEquipSlotItem : MonoBehaviour
	{
		public enum ActionType
		{
			OnTouch
		}

		public delegate void UIRemodelEquipSlotItemAction(UIRemodelEquipSlotItem.ActionType actionType, UIRemodelEquipSlotItem actionObject);

		private UIWidget mWidget;

		[SerializeField]
		private UISprite mSprite_TypeIcon;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_PlaneCount;

		[SerializeField]
		private UIButton mButton_Action;

		[SerializeField]
		private SlotItemLevelStar levelStar;

		[SerializeField]
		private UISprite mLock_Icon;

		[SerializeField]
		private UISprite PlaneSkill;

		private Vector3 PlaneNumPos_NoSkill = new Vector3(165f, 0f, 0f);

		private Vector3 PlaneNumPos_SkillPos = new Vector3(165f, -14f, 0f);

		private Transform ExSlotButtonFrame;

		private UIRemodelEquipSlotItem.UIRemodelEquipSlotItemAction mUIRemodelEquipSlotItemAction;

		private SlotitemModel mModel;

		private bool mEnabled;

		private static readonly Vector3 ExSize = new Vector3(0.8f, 0.8f, 0.8f);

		public UIButton ExSlotButton
		{
			get;
			private set;
		}

		public int index
		{
			get;
			private set;
		}

		public bool isExSlot
		{
			get;
			private set;
		}

		public SlotitemModel GetModel()
		{
			return this.mModel;
		}

		private void Awake()
		{
			this.mWidget = base.GetComponent<UIWidget>();
		}

		private void OnAction(UIRemodelEquipSlotItem.ActionType actionType, UIRemodelEquipSlotItem actionObject)
		{
			if (this.mUIRemodelEquipSlotItemAction != null)
			{
				this.mUIRemodelEquipSlotItemAction(actionType, actionObject);
			}
		}

		public void Initialize(int index, ShipModel shipModel)
		{
			SlotitemModel itemModel = shipModel.SlotitemList.get_Item(index);
			this.Initialize(index, itemModel, shipModel);
		}

		public void Initialize(int index, SlotitemModel itemModel, ShipModel shipModel)
		{
			this.index = index;
			this.mModel = itemModel;
			bool flag = false;
			this.isExSlot = (shipModel.HasExSlot() && shipModel.SlotCount <= index);
			base.get_transform().SetActiveChildren(true);
			if (this.ExSlotButton != null)
			{
				this.ExSlotButton.get_transform().get_parent().SetActive(false);
			}
			if (itemModel != null)
			{
				this.mLabel_Name.text = this.mModel.Name;
				if (itemModel.IsLocked())
				{
					this.mLock_Icon.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLock_Icon.get_transform().set_localScale(Vector3.get_zero());
				}
				this.mSprite_TypeIcon.spriteName = "icon_slot" + itemModel.Type4;
				flag = itemModel.IsPlane(true);
				if (flag)
				{
					this.mLabel_PlaneCount.text = shipModel.TousaiMax[index].ToString();
				}
			}
			else
			{
				this.mLabel_Name.text = "-";
				this.mLock_Icon.get_transform().set_localScale(Vector3.get_zero());
				this.mSprite_TypeIcon.spriteName = "icon_slot_notset";
			}
			this.mLabel_PlaneCount.SetActive(flag);
			this.SetPlaneSkill(itemModel);
			this.InitializeButtonColor(this.mButton_Action);
			this.levelStar.Init(itemModel);
			if (this.isExSlot)
			{
				this.ChangeExMode();
			}
			else
			{
				this.ChangeNormalMode();
			}
		}

		private void SetPlaneSkill(SlotitemModel item)
		{
			if (item != null && item.IsPlane())
			{
				int skillLevel = item.SkillLevel;
				if (skillLevel == 0)
				{
					this.PlaneSkill.SetActive(false);
					this.mLabel_PlaneCount.get_transform().set_localPosition(this.PlaneNumPos_NoSkill);
				}
				else
				{
					this.PlaneSkill.SetActive(true);
					this.PlaneSkill.spriteName = "skill_" + skillLevel;
					this.PlaneSkill.MakePixelPerfect();
					this.mLabel_PlaneCount.get_transform().set_localPosition(this.PlaneNumPos_SkillPos);
				}
			}
			else
			{
				this.PlaneSkill.SetActive(false);
			}
		}

		public void InitExSlotButton(bool isEnable)
		{
			base.get_transform().SetActiveChildren(false);
			this.ChangeNormalMode();
			if (this.ExSlotButton == null)
			{
				GameObject gameObject = Util.InstantiatePrefab("Remodel/ExSlotBtn", base.get_gameObject(), false);
				this.ExSlotButton = gameObject.get_transform().FindChild("ExSlotButton").GetComponent<UIButton>();
				this.ExSlotButtonFrame = gameObject.get_transform().FindChild("ExSlotButtonFrame");
			}
			this.ExSlotButton.get_transform().get_parent().SetActive(true);
			this.ExSlotButton.get_transform().SetActive(true);
			this.ExSlotButtonFrame.SetActive(false);
			this.isExSlot = true;
			this.ExSlotButton.isEnabled = isEnable;
			this.ExSlotButton.onClick.Add(Util.CreateEventDelegate(base.get_transform().get_parent().get_parent().GetComponent<UIRemodelEquipSlotItems>(), "OpenExSlotDialog", null));
			if (isEnable)
			{
				this.ExSlotButton.SetState(UIButtonColor.State.Normal, true);
			}
			else
			{
				this.ExSlotButton.SetState(UIButtonColor.State.Disabled, true);
			}
		}

		public void UnSetSlotItem()
		{
			this.mModel = null;
			this.mLabel_Name.text = "-";
			this.mSprite_TypeIcon.spriteName = "icon_slot_notset";
		}

		private void InitializeButtonColor(UIButton target)
		{
			target.hover = Util.CursolColor;
			target.defaultColor = Color.get_white();
			target.pressed = Color.get_white();
			target.disabledColor = Color.get_white();
		}

		public void SetOnUIRemodelEquipSlotItemActionListener(UIRemodelEquipSlotItem.UIRemodelEquipSlotItemAction action)
		{
			this.mUIRemodelEquipSlotItemAction = action;
		}

		public void OnClick()
		{
			if (this.mEnabled)
			{
				this.OnAction(UIRemodelEquipSlotItem.ActionType.OnTouch, this);
			}
		}

		public void Hover()
		{
			if (this.isExSlot && this.ExSlotButton != null && this.ExSlotButton.get_isActiveAndEnabled())
			{
				if (this.ExSlotButton.isEnabled)
				{
					this.ExSlotButton.SetState(UIButtonColor.State.Hover, true);
				}
				this.ExSlotButtonFrame.SetActive(true);
			}
			else
			{
				this.mButton_Action.SetState(UIButtonColor.State.Hover, true);
				UISelectedObject.SelectedOneObjectBlink(this.mButton_Action.get_gameObject(), true);
			}
		}

		public void RemoveHover()
		{
			if (this.isExSlot && this.ExSlotButton != null && this.ExSlotButton.get_isActiveAndEnabled())
			{
				if (this.ExSlotButton.isEnabled)
				{
					this.ExSlotButton.SetState(UIButtonColor.State.Normal, true);
				}
				this.ExSlotButtonFrame.SetActive(false);
			}
			else
			{
				this.mButton_Action.SetState(UIButtonColor.State.Normal, true);
				UISelectedObject.SelectedOneObjectBlink(this.mButton_Action.get_gameObject(), false);
			}
		}

		public void Show()
		{
			this.mEnabled = true;
			base.get_gameObject().SetActive(true);
		}

		public void Hide()
		{
			this.mEnabled = false;
			this.mModel = null;
			base.get_gameObject().SetActive(false);
			this.RemoveHover();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidget);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_TypeIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Name);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_PlaneCount);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Action);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLock_Icon);
			this.levelStar = null;
			this.mUIRemodelEquipSlotItemAction = null;
			this.mModel = null;
		}

		private void ChangeExMode()
		{
			base.get_transform().set_localScale(UIRemodelEquipSlotItem.ExSize);
			base.get_transform().localPositionX(-40f);
			this.mLabel_Name.fontSize = 30;
		}

		private void ChangeNormalMode()
		{
			base.get_transform().set_localScale(Vector3.get_one());
			base.get_transform().localPositionX(0f);
			this.mLabel_Name.fontSize = 24;
		}
	}
}
