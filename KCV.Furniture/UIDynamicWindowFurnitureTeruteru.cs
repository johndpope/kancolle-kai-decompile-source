using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureTeruteru : UIDynamicWindowFurniture
	{
		[SerializeField]
		private Texture[] mTexture2ds_Teruteru;

		[SerializeField]
		private UITexture mTexture_Teruteru;

		protected override void OnUpdate()
		{
			base.UpdateWindow();
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			FurnitureModel furnitureModel = uiFurnitureModel.GetFurnitureModel();
			DateTime dateTime = uiFurnitureModel.GetDateTime();
			int outPlaceGraphicType = base.GetOutPlaceGraphicType(furnitureModel);
			int outPlaceTimeType = base.GetOutPlaceTimeType(dateTime.get_Hour());
			Texture mainTexture = base.RequestOutPlaceTexture(outPlaceGraphicType, outPlaceTimeType);
			this.mTexture_WindowBackground.mainTexture = mainTexture;
			this.mTexture_Teruteru.mainTexture = this.mTexture2ds_Teruteru[2];
		}

		protected override void OnCalledActionEvent()
		{
			this.Animation();
		}

		private void Animation()
		{
			if (!DOTween.IsTweening(this))
			{
				int[] array = new int[]
				{
					2,
					1,
					0,
					1,
					2,
					3,
					4,
					3,
					2,
					1,
					2,
					3,
					2
				};
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.SetId<Sequence>(sequence, this);
				int[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					int index2 = array2[i];
					int index = index2;
					TweenCallback tweenCallback = delegate
					{
						this.mTexture_Teruteru.mainTexture = this.mTexture2ds_Teruteru[index];
					};
					TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
					TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
				}
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTexture2ds_Teruteru, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Teruteru, false);
		}
	}
}
