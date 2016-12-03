using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdMapPoint : BaseAnimation
	{
		private int _sPoint;

		private void _init()
		{
			base.Awake();
			this._sPoint = 0;
		}

		private void OnDestroy()
		{
			base.OnDestroy();
		}

		public override void Play(Action callback)
		{
			UILabel component = base.get_transform().FindChild("Panel/Label").GetComponent<UILabel>();
			component.text = "×" + this._sPoint;
			base.Play(callback);
			SoundUtils.PlaySE(SEFIleInfos.RewardGet);
		}

		public static ProdMapPoint Instantiate(ProdMapPoint prefab, Transform parent, int sPoint)
		{
			ProdMapPoint prodMapPoint = Object.Instantiate<ProdMapPoint>(prefab);
			prodMapPoint.get_transform().set_parent(parent);
			prodMapPoint.get_transform().set_localScale(Vector3.get_one());
			prodMapPoint.get_transform().set_localPosition(Vector3.get_zero());
			prodMapPoint._sPoint = sPoint;
			return prodMapPoint;
		}
	}
}
