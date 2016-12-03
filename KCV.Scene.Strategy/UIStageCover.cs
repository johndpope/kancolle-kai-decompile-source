using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Strategy
{
	public class UIStageCover : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Cover;

		[SerializeField]
		private UITexture mTexture_Lock;

		[SerializeField]
		private UITexture mTexture_ClearEmblem;

		[SerializeField]
		private Transform mTransform_BossGaugeArea;

		public MapModel Model
		{
			get;
			private set;
		}

		public void Initialize(MapModel mapModel)
		{
			this.ReleaseUITexture(ref this.mTexture_Cover, true);
			this.Model = mapModel;
			if (this.Model != null)
			{
				if (this.Model.MapHP != null)
				{
					this.InitializeBossGauge(this.Model.MapHP);
				}
				this.mTexture_Cover.mainTexture = ResourceManager.LoadStageCover(this.Model.AreaId, this.Model.No);
				if (this.Model.Cleared)
				{
					this.mTexture_ClearEmblem.SetActive(true);
				}
				else
				{
					this.mTexture_ClearEmblem.SetActive(false);
				}
				if (!this.Model.Map_Possible)
				{
					this.mTexture_Lock.SetActive(true);
				}
				else
				{
					this.mTexture_Lock.SetActive(false);
				}
			}
		}

		private void InitializeBossGauge(MapHPModel mapHPModel)
		{
			GameObject original = Resources.Load("Prefabs/Common/MapHP/UIMapHP_3") as GameObject;
			UIMapHP component = Util.Instantiate(original, this.mTransform_BossGaugeArea.get_gameObject(), false, false).GetComponent<UIMapHP>();
			component.Initialize(mapHPModel);
			component.Play();
		}

		private void ReleaseUITexture(ref UITexture uiTexture, bool unloadUnUsedAsset = false)
		{
			if (uiTexture != null)
			{
				if (uiTexture.mainTexture != null && unloadUnUsedAsset)
				{
					Resources.UnloadAsset(uiTexture.mainTexture);
				}
				uiTexture.mainTexture = null;
			}
		}

		private void OnDestroy()
		{
			this.ReleaseUITexture(ref this.mTexture_Cover, true);
			this.mTexture_Cover = null;
			this.ReleaseUITexture(ref this.mTexture_Lock, false);
			this.mTexture_Lock = null;
			this.ReleaseUITexture(ref this.mTexture_ClearEmblem, false);
			this.mTexture_ClearEmblem = null;
		}

		internal void SelfRelease()
		{
			this.ReleaseUITexture(ref this.mTexture_Cover, true);
		}
	}
}
