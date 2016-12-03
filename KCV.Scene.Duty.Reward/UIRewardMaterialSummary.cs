using Common.Enum;
using KCV.Scene.Port;
using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIRewardMaterialSummary : BaseUISummary<IReward>
	{
		[SerializeField]
		private UILabel mLabelName;

		[SerializeField]
		private UILabel mLabelValue;

		[SerializeField]
		private UISprite mSpriteIcon;

		public override void Initialize(int index, IReward model)
		{
			base.Initialize(index, model);
			if (model is IReward_Material)
			{
				IReward_Material material = (IReward_Material)model;
				this.InitializeMaterial(material);
			}
			else if (model is Reward_SPoint)
			{
				Reward_SPoint spoint = (Reward_SPoint)model;
				this.InitializeSPoint(spoint);
			}
			else if (model is IReward_Useitem)
			{
				IReward_Useitem useItem = (IReward_Useitem)model;
				this.InitializeUseItem(useItem);
			}
		}

		private void InitializeUseItem(IReward_Useitem useItem)
		{
			this.mLabelName.text = useItem.Name;
			this.mLabelValue.text = useItem.Count.ToString();
			this.mSpriteIcon.spriteName = string.Format("item_{0}", useItem.Id);
		}

		private void InitializeMaterial(IReward_Material material)
		{
			this.mLabelName.text = material.Name;
			this.mLabelValue.text = material.Count.ToString();
			int num = 0;
			switch (material.Type)
			{
			case enumMaterialCategory.Fuel:
				num = 31;
				break;
			case enumMaterialCategory.Bull:
				num = 32;
				break;
			case enumMaterialCategory.Steel:
				num = 33;
				break;
			case enumMaterialCategory.Bauxite:
				num = 34;
				break;
			case enumMaterialCategory.Build_Kit:
				num = 2;
				break;
			case enumMaterialCategory.Repair_Kit:
				num = 1;
				break;
			case enumMaterialCategory.Dev_Kit:
				num = 3;
				break;
			case enumMaterialCategory.Revamp_Kit:
				num = 4;
				break;
			}
			this.mSpriteIcon.spriteName = string.Format("item_{0}", num);
		}

		private void InitializeSPoint(Reward_SPoint spoint)
		{
			this.mSpriteIcon.spriteName = string.Format("item_spoint", new object[0]);
			this.mLabelName.text = "戦略ポイント";
			this.mLabelValue.text = spoint.SPoint.ToString();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSpriteIcon);
		}
	}
}
