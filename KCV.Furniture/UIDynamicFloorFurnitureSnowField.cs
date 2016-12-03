using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicFloorFurnitureSnowField : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_TheWorks;

		[SerializeField]
		private Texture mTexture2d_TypeA;

		[SerializeField]
		private Texture mTexture2d_TypeB;

		[SerializeField]
		private Texture mTexture2d_TypeC;

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			this.InitializeTheWorks(uiFurnitureModel.GetDateTime());
		}

		private void InitializeTheWorks(DateTime dateTime)
		{
			if (4 <= dateTime.get_Hour() && dateTime.get_Hour() < 10)
			{
				this.mTexture_TheWorks.mainTexture = this.mTexture2d_TypeA;
				this.mTexture_TheWorks.SetDimensions(135, 90);
			}
			else if (10 <= dateTime.get_Hour() && dateTime.get_Hour() < 16)
			{
				this.mTexture_TheWorks.mainTexture = this.mTexture2d_TypeB;
				this.mTexture_TheWorks.SetDimensions(158, 125);
			}
			else if (16 <= dateTime.get_Hour() && dateTime.get_Hour() < 22)
			{
				this.mTexture_TheWorks.mainTexture = null;
				this.mTexture_TheWorks.SetDimensions(0, 0);
			}
			else if (22 <= dateTime.get_Hour() || dateTime.get_Hour() < 4)
			{
				this.mTexture_TheWorks.mainTexture = this.mTexture2d_TypeC;
				this.mTexture_TheWorks.SetDimensions(132, 89);
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_TheWorks, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeA, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeB, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeC, false);
		}
	}
}
