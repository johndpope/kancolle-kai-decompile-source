using DG.Tweening;
using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicChestFurnitureKagatan : UIDynamicFurniture
	{
		private enum ShipType
		{
			Kubo,
			Goei
		}

		private UIDynamicChestFurnitureKagatan.ShipType mCurrentShipType;

		[Header("加賀"), SerializeField]
		private UITexture mTexture_Kaga;

		[Header("かが"), SerializeField]
		private UITexture mTexture_KAGA;

		[Header("妖精"), SerializeField]
		private UISprite mSprite_Tansuyokusei;

		[Header("DEBUG"), SerializeField]
		private bool mIsMoving;

		protected override void OnAwake()
		{
			this.mSprite_Tansuyokusei.alpha = 0.1f;
			this.mTexture_KAGA.alpha = 1E-06f;
			this.mSprite_Tansuyokusei.spriteName = "00";
			this.mCurrentShipType = UIDynamicChestFurnitureKagatan.ShipType.Kubo;
		}

		private void OnClick()
		{
			this.OnCalledActionEvent();
		}

		protected override void OnCalledActionEvent()
		{
			if (!this.mIsMoving)
			{
				IEnumerator enumerator = this.OnCalledActionEventCoroutine();
				base.StartCoroutine(enumerator);
			}
		}

		[DebuggerHidden]
		private IEnumerator OnCalledActionEventCoroutine()
		{
			UIDynamicChestFurnitureKagatan.<OnCalledActionEventCoroutine>c__Iterator5F <OnCalledActionEventCoroutine>c__Iterator5F = new UIDynamicChestFurnitureKagatan.<OnCalledActionEventCoroutine>c__Iterator5F();
			<OnCalledActionEventCoroutine>c__Iterator5F.<>f__this = this;
			return <OnCalledActionEventCoroutine>c__Iterator5F;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Kaga, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_KAGA, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Tansuyokusei);
		}
	}
}
