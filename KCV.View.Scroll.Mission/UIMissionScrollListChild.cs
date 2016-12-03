using Common.Enum;
using Common.Struct;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.View.Scroll.Mission
{
	public class UIMissionScrollListChild : UIScrollListChild<MissionModel>
	{
		public enum ActionType
		{
			Back,
			Action,
			Description
		}

		public delegate void UIMissionScrollListChildAction(UIMissionScrollListChild.ActionType actionType, UIMissionScrollListChild calledObject);

		[SerializeField]
		private UILabel mLabel_Title;

		[SerializeField]
		private UISprite mSprite_Status;

		[SerializeField]
		private UISprite mSprite_Reward_00;

		[SerializeField]
		private UISprite mSprite_Reward_01;

		[SerializeField]
		private UISprite mSprite_Reward_02;

		[SerializeField]
		private UISprite mSprite_Reward_03;

		[SerializeField]
		private UITexture mTexture_BackgroundDesign;

		private UIMissionScrollListChild.UIMissionScrollListChildAction mUIMissionScrollListChildActionCallBack;

		private UIButton mButton_Focus;

		private KeyControl mKeyController;

		public void SetOnUIMissionScrollListChildAction(UIMissionScrollListChild.UIMissionScrollListChildAction action)
		{
			this.mUIMissionScrollListChildActionCallBack = action;
		}

		protected override void InitializeChildContents(MissionModel model, bool clickable)
		{
			this.mLabel_Title.text = model.Name;
			switch (model.State)
			{
			case MissionClearKinds.NEW:
				this.mSprite_Status.spriteName = "icon_new";
				this.mSprite_Status.MakePixelPerfect();
				goto IL_84;
			case MissionClearKinds.CLEARED:
				this.mSprite_Status.spriteName = "icon_check";
				this.mSprite_Status.MakePixelPerfect();
				goto IL_84;
			}
			this.mSprite_Status.spriteName = string.Empty;
			IL_84:
			this.InitializeRewardIcon(model.GetRewardMaterials());
			this.mTexture_BackgroundDesign.mainTexture = Resources.Load<Texture>(string.Format("Textures/Mission/sea/{0}", this.AreaIdToSeaSpriteName(model.AreaId)));
		}

		private string AreaIdToSeaSpriteName(int areaId)
		{
			switch (areaId)
			{
			case 1:
			case 8:
			case 9:
			case 11:
			case 12:
				return "list_sea" + 1;
			case 2:
			case 4:
			case 5:
			case 6:
			case 7:
			case 10:
			case 14:
				return "list_sea" + 2;
			case 3:
			case 13:
				return "list_sea" + 3;
			case 15:
			case 16:
			case 17:
				return "list_sea" + 4;
			default:
				return string.Empty;
			}
		}

		private void InitializeRewardIcon(MaterialInfo materialInfo)
		{
			if (0 < materialInfo.Ammo)
			{
				this.mSprite_Reward_00.spriteName = "item_" + this.enumMaterialCategoryToId(enumMaterialCategory.Bull);
			}
			else
			{
				this.mSprite_Reward_00.spriteName = string.Empty;
			}
			if (0 < materialInfo.Baux)
			{
				this.mSprite_Reward_01.spriteName = "item_" + this.enumMaterialCategoryToId(enumMaterialCategory.Bauxite);
			}
			else
			{
				this.mSprite_Reward_01.spriteName = string.Empty;
			}
			if (0 < materialInfo.Fuel)
			{
				this.mSprite_Reward_02.spriteName = "item_" + this.enumMaterialCategoryToId(enumMaterialCategory.Fuel);
			}
			else
			{
				this.mSprite_Reward_02.spriteName = string.Empty;
			}
			if (0 < materialInfo.Steel)
			{
				this.mSprite_Reward_03.spriteName = "item_" + this.enumMaterialCategoryToId(enumMaterialCategory.Steel);
			}
			else
			{
				this.mSprite_Reward_03.spriteName = string.Empty;
			}
		}

		private int enumMaterialCategoryToId(enumMaterialCategory type)
		{
			switch (type)
			{
			case enumMaterialCategory.Fuel:
				return 31;
			case enumMaterialCategory.Bull:
				return 32;
			case enumMaterialCategory.Steel:
				return 33;
			case enumMaterialCategory.Bauxite:
				return 34;
			default:
				return 0;
			}
		}

		public override void Hover()
		{
			base.Hover();
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mButton_Focus.Equals(this.mButton_Action))
					{
						this.CallBackAction(UIMissionScrollListChild.ActionType.Action, this);
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.CallBackAction(UIMissionScrollListChild.ActionType.Back, this);
				}
			}
		}

		public KeyControl GetKeyController()
		{
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			return this.mKeyController;
		}

		public void RemoveKeyFocus()
		{
			this.mKeyController = null;
			this.Hover();
		}

		private void CallBackAction(UIMissionScrollListChild.ActionType actionType, UIMissionScrollListChild calledObject)
		{
			if (this.mUIMissionScrollListChildActionCallBack != null)
			{
				this.mUIMissionScrollListChildActionCallBack(actionType, calledObject);
			}
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Title);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Status);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Reward_00);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Reward_01);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Reward_02);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Reward_03);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_BackgroundDesign, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Focus);
			this.mUIMissionScrollListChildActionCallBack = null;
			this.mKeyController = null;
		}
	}
}
