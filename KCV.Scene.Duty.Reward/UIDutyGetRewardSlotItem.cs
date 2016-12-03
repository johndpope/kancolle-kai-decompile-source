using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyGetRewardSlotItem : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_SlotItem;

		[SerializeField]
		private UILabel mLabel_Name;

		public void Initialize(Reward_Slotitem reward_Slotitem)
		{
			this.mTexture_SlotItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(reward_Slotitem.Id, 1);
			this.mLabel_Name.text = reward_Slotitem.Name;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_SlotItem, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Name);
		}
	}
}
