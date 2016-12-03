using KCV.Battle.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class UISlotItemHexButton : UIHexButton
	{
		private static readonly float HEX_BUTTON_SIZE = 390f;

		private UITexture _uiSlotItem;

		protected override void Awake()
		{
			base.Awake();
			Util.FindParentToChild<UITexture>(ref this._uiSlotItem, base.get_transform(), "SlotItem");
			this._uiSlotItem.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(4);
			this._uiSlotItem.alpha = 0.01f;
			this._uiHexBtn.localSize = new Vector2(UISlotItemHexButton.HEX_BUTTON_SIZE, UISlotItemHexButton.HEX_BUTTON_SIZE);
			this._uiHexBtn.alpha = 0.01f;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._uiSlotItem);
		}

		public override void SetDepth(int nDepth)
		{
			this._uiHexBtn.depth = nDepth;
			this._uiSlotItem.depth = nDepth + 1;
		}

		public void SetSlotItem(SlotitemModel_Battle model)
		{
			SlotItemUtils.LoadTexture(ref this._uiSlotItem, model);
		}
	}
}
