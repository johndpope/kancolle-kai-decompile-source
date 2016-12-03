using DG.Tweening;
using local.models;
using System;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollListShipInfo : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_ShipCard;

		private Vector3 mVector3ShipCardDefaultPosition;

		private void Start()
		{
			this.mVector3ShipCardDefaultPosition = this.mTexture_ShipCard.get_transform().get_localPosition();
		}

		public void Initialize(ShipModel model)
		{
			if (model == null)
			{
				return;
			}
			this.mTexture_ShipCard.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, 3);
			TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOShakePosition(this.mTexture_ShipCard.get_transform(), 0.3f, 5f, 10, 90f, false), delegate
			{
				ShortcutExtensions.DOLocalMove(this.mTexture_ShipCard.get_transform(), this.mVector3ShipCardDefaultPosition, 0.1f, false);
			});
		}
	}
}
