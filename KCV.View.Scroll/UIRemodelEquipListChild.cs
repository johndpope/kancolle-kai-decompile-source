using KCV.Remodel;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.View.Scroll
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class UIRemodelEquipListChild : UIScrollListChildNew<SlotitemModel, UIRemodelEquipListChild>
	{
		private const int HIDE_DEPTH = -100;

		[SerializeField]
		private UILabel ItemName;

		[SerializeField]
		private UISprite ItemIcon;

		[SerializeField]
		private UISprite LockedIcon;

		[SerializeField]
		private UILabel Rare;

		[SerializeField]
		private SlotItemLevelStar levelStar;

		private bool locked;

		private string originalSpriteName;

		private SlotitemModel _sm;

		protected void Awake()
		{
			base.Awake();
			this.originalSpriteName = this.LockedIcon.spriteName;
		}

		[DebuggerHidden]
		protected override IEnumerator InitializeCoroutine(SlotitemModel slotitemModel)
		{
			UIRemodelEquipListChild.<InitializeCoroutine>c__IteratorB1 <InitializeCoroutine>c__IteratorB = new UIRemodelEquipListChild.<InitializeCoroutine>c__IteratorB1();
			<InitializeCoroutine>c__IteratorB.slotitemModel = slotitemModel;
			<InitializeCoroutine>c__IteratorB.<$>slotitemModel = slotitemModel;
			<InitializeCoroutine>c__IteratorB.<>f__this = this;
			return <InitializeCoroutine>c__IteratorB;
		}

		public void OnClick2()
		{
			this.SwitchLockedIcon(true);
		}

		public void SwitchLockedIcon(bool Change)
		{
			if (Change)
			{
				UserInterfaceRemodelManager.instance.mRemodelManager.SlotLock(this._sm.MemId);
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

		protected override void OnCallDestroy()
		{
			this.ItemName = null;
			this.ItemIcon = null;
			this.Rare = null;
			this.levelStar = null;
			this.LockedIcon = null;
		}
	}
}
