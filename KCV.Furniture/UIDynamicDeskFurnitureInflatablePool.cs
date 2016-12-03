using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicDeskFurnitureInflatablePool : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Pool;

		[SerializeField]
		private Texture mTexture2d_Pool_On;

		[SerializeField]
		private Texture mTexture2d_Pool_Off;

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			this.InitializePool(uiFurnitureModel.GetDateTime());
		}

		private void InitializePool(DateTime dateTime)
		{
			if (10 <= dateTime.get_Hour() && dateTime.get_Hour() < 20)
			{
				this.mTexture_Pool.mainTexture = this.mTexture2d_Pool_On;
			}
			else
			{
				this.mTexture_Pool.mainTexture = this.mTexture2d_Pool_Off;
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Pool, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Pool_On, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Pool_Off, false);
		}
	}
}
