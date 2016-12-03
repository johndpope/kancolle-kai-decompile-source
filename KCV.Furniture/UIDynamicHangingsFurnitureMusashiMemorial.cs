using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicHangingsFurnitureMusashiMemorial : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_MusashiRoll;

		[SerializeField]
		private Texture mTexture2d_TypeA;

		[SerializeField]
		private Texture mTexture2d_TypeB;

		[SerializeField]
		private ParticleSystem mParticleSystem_Petal;

		protected override void OnAwake()
		{
			this.mParticleSystem_Petal.Stop();
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			this.InitializeMusashiRoll(uiFurnitureModel.GetDateTime());
		}

		private void InitializeMusashiRoll(DateTime dateTime)
		{
			bool flag = dateTime.get_Hour() == 0 || dateTime.get_Hour() % 2 == 0;
			if (flag)
			{
				this.mTexture_MusashiRoll.mainTexture = this.mTexture2d_TypeA;
			}
			else
			{
				this.mTexture_MusashiRoll.mainTexture = this.mTexture2d_TypeB;
			}
			this.mParticleSystem_Petal.Play();
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_MusashiRoll, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeA, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_TypeB, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mParticleSystem_Petal);
		}
	}
}
