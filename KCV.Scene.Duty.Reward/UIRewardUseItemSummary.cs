using KCV.Scene.Port;
using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIRewardUseItemSummary : BaseUISummary<Reward_Useitem>
	{
		[SerializeField]
		private UILabel mLabelName;

		[SerializeField]
		private UILabel mLabelValue;

		[SerializeField]
		private UISprite mSpriteIcon;

		public override void Initialize(int index, Reward_Useitem model)
		{
			base.Initialize(index, model);
			this.mLabelName.text = model.Name;
			this.mLabelValue.text = model.Count.ToString();
			this.mSpriteIcon.spriteName = string.Format("item_{0}", model.Id);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSpriteIcon);
		}
	}
}
