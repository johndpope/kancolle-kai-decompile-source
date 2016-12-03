using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicChestFurnitureDaruma : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Daruma;

		private Vector3 mVector3_DefaultPosition;

		protected override void OnAwake()
		{
			this.mVector3_DefaultPosition = this.mTexture_Daruma.get_transform().get_localPosition();
		}

		protected override void OnCalledActionEvent()
		{
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Daruma, false);
		}
	}
}
