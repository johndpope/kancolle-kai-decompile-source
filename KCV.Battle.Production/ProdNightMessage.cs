using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdNightMessage : BaseAnimation
	{
		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdNightMessage Instantiate(ProdNightMessage prefab, Transform parent)
		{
			ProdNightMessage prodNightMessage = Object.Instantiate<ProdNightMessage>(prefab);
			prodNightMessage.get_transform().set_parent(parent);
			prodNightMessage.get_transform().set_localPosition(Vector3.get_zero());
			prodNightMessage.get_transform().localScaleZero();
			return prodNightMessage;
		}

		protected override void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			base.OnDestroy();
		}

		public override void Play(Action callback)
		{
			base.get_transform().localScaleOne();
			base.Play(callback);
		}

		private void PlayMessageSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
		}
	}
}
