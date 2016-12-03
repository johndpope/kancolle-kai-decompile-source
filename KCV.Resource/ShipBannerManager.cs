using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Resource
{
	public class ShipBannerManager : MonoBehaviour
	{
		private ShipResource mShipResource_Banner_Normal;

		private ShipResource mShipResource_Banner_Damaged;

		private ShipResource mShipResource_Card_Normal;

		private ShipResource mShipResource_Card_Damaged;

		private ShipResource mShipResource_Full_Normal;

		private ShipResource mShipResource_Full_Damaged;

		private IEnumerator mInitializeCoroutine;

		private void Awake()
		{
			this.mShipResource_Banner_Normal = new ShipResource(TextureType.Banner, StateType.Normal);
			this.mShipResource_Banner_Damaged = new ShipResource(TextureType.Banner, StateType.Damaged);
			this.mShipResource_Card_Normal = new ShipResource(TextureType.Card, StateType.Normal);
			this.mShipResource_Card_Damaged = new ShipResource(TextureType.Card, StateType.Damaged);
			this.mShipResource_Full_Normal = new ShipResource(TextureType.Full, StateType.Normal);
			this.mShipResource_Full_Damaged = new ShipResource(TextureType.Full, StateType.Damaged);
		}

		public void Initialize(ShipModel[] shipModels)
		{
			if (this.mInitializeCoroutine != null)
			{
				base.StopCoroutine(this.mInitializeCoroutine);
				this.mInitializeCoroutine = null;
			}
			this.mInitializeCoroutine = this.InitializeCoroutine(shipModels);
			base.StartCoroutine(this.mInitializeCoroutine);
		}

		[DebuggerHidden]
		private IEnumerator InitializeCoroutine(ShipModel[] shipModels)
		{
			ShipBannerManager.<InitializeCoroutine>c__Iterator1CB <InitializeCoroutine>c__Iterator1CB = new ShipBannerManager.<InitializeCoroutine>c__Iterator1CB();
			<InitializeCoroutine>c__Iterator1CB.shipModels = shipModels;
			<InitializeCoroutine>c__Iterator1CB.<$>shipModels = shipModels;
			<InitializeCoroutine>c__Iterator1CB.<>f__this = this;
			return <InitializeCoroutine>c__Iterator1CB;
		}

		public Texture GetShipBanner(ShipModel shipModel)
		{
			if (shipModel.IsDamaged())
			{
				return this.GetDamagedBanner(shipModel.GetGraphicsMstId());
			}
			return this.GetNormalBanner(shipModel.GetGraphicsMstId());
		}

		public Texture GetNormalBanner(int masterId)
		{
			return this.mShipResource_Banner_Normal.GetResource(masterId);
		}

		public Texture GetDamagedBanner(int masterId)
		{
			return this.mShipResource_Banner_Damaged.GetResource(masterId);
		}
	}
}
