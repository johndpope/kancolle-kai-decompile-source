using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

public class UIDutyGetRewardFurniture : MonoBehaviour
{
	[SerializeField]
	private UILabel mLabel_Message;

	[SerializeField]
	private UITexture mTexture_Image;

	public void Initialize(Reward_Furniture rewardFurniture)
	{
		this.mLabel_Message.text = string.Format("{0}を\n手に入れました", rewardFurniture.Name);
		this.mTexture_Image.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(rewardFurniture.Type, rewardFurniture.MstId);
		this.mTexture_Image.MakePixelPerfect();
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Message);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Image, false);
	}
}
