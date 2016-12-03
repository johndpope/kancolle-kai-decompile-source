using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicFloorFurnitureSandBeach : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_TheWorks;

		[SerializeField]
		private Texture mTexture2d_TypeA;

		[SerializeField]
		private Texture mTexture2d_TypeB;

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			DateTime dateTime = this.mFurnitureModel.GetDateTime();
			this.InitializeTheWorks(dateTime);
		}

		private void InitializeTheWorks(DateTime dateTime)
		{
			if (6 <= dateTime.get_Hour() && dateTime.get_Hour() < 18)
			{
				this.mTexture_TheWorks.mainTexture = this.mTexture2d_TypeA;
				this.mTexture_TheWorks.SetDimensions(147, 77);
			}
			else
			{
				this.mTexture_TheWorks.mainTexture = this.mTexture2d_TypeB;
				this.mTexture_TheWorks.SetDimensions(171, 103);
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_TheWorks, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeA, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeB, false);
		}
	}
}
