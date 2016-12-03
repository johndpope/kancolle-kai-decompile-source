using KCV.Scene.Port;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodelEquipListChildNew : MonoBehaviour, UIScrollListItem<SlotitemModel, UIRemodelEquipListChildNew>
	{
		private const int HIDE_DEPTH = -100;

		private UIWidget mWidgetThis;

		[SerializeField]
		private UILabel ItemName;

		[SerializeField]
		private UISprite ItemIcon;

		[SerializeField]
		private UISprite LockedIcon;

		[SerializeField]
		private UILabel Rare;

		[SerializeField]
		private Transform mTransform_Background;

		[SerializeField]
		private SlotItemLevelStar levelStar;

		[SerializeField]
		private UISprite PlaneSkill;

		private Action<UIRemodelEquipListChildNew> mOnTouchListener;

		private Transform mTransformCache;

		private SlotitemModel mSlotItemModel;

		private bool locked;

		private string originalSpriteName;

		private int mRealIndex;

		protected void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 0f;
			this.originalSpriteName = this.LockedIcon.spriteName;
		}

		public void SwitchLockedIcon(bool Change)
		{
			if (Change)
			{
				UserInterfaceRemodelManager.instance.mRemodelManager.SlotLock(this.mSlotItemModel.MemId);
			}
			this.locked = !this.locked;
			this.SetLockedIconVisible(this.locked);
			if (this.locked)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			else
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
		}

		private void SetLockedIconVisible(bool visible)
		{
			this.LockedIcon.spriteName = ((!visible) ? "lock_weapon" : "lock_weapon_open");
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.ItemName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.ItemIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.LockedIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.Rare);
			this.mTransform_Background = null;
			this.levelStar = null;
			this.mOnTouchListener = null;
			this.mTransformCache = null;
			this.mSlotItemModel = null;
			this.PlaneSkill = null;
		}

		public void Initialize(int realIndex, SlotitemModel slotitemModel)
		{
			this.mRealIndex = realIndex;
			this.mSlotItemModel = slotitemModel;
			this.ItemName.text = this.mSlotItemModel.Name;
			this.Rare.text = Util.RareToString(this.mSlotItemModel.Rare);
			string text = this.mSlotItemModel.Type4.ToString();
			this.ItemIcon.spriteName = "icon_slot" + text;
			this.levelStar.Init(this.mSlotItemModel);
			this.SetPlaneSkill(this.mSlotItemModel);
			this.locked = this.mSlotItemModel.IsLocked();
			this.SetLockedIconVisible(this.locked);
			this.mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault(int realIndex)
		{
			this.mSlotItemModel = null;
			this.mRealIndex = realIndex;
			this.mWidgetThis.alpha = 1E-07f;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}

		public SlotitemModel GetModel()
		{
			return this.mSlotItemModel;
		}

		public int GetHeight()
		{
			return 75;
		}

		public void SetOnTouchListener(Action<UIRemodelEquipListChildNew> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		public void OnClickItem()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		public void OnClickLock()
		{
			this.SwitchLockedIcon(true);
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTransform_Background.get_gameObject(), true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTransform_Background.get_gameObject(), false);
		}

		public Transform GetTransform()
		{
			if (this.mTransformCache == null)
			{
				this.mTransformCache = base.get_transform();
			}
			return this.mTransformCache;
		}

		private void SetPlaneSkill(SlotitemModel item)
		{
			if (item != null && item.IsPlane())
			{
				int skillLevel = item.SkillLevel;
				if (skillLevel == 0)
				{
					this.PlaneSkill.SetActive(false);
				}
				else
				{
					this.PlaneSkill.SetActive(true);
					this.PlaneSkill.spriteName = "skill_" + skillLevel;
					this.PlaneSkill.MakePixelPerfect();
				}
			}
			else
			{
				this.PlaneSkill.SetActive(false);
			}
		}
	}
}
