using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(Animation))]
	public class ProdSortieEnd : BaseAnimation
	{
		private UITexture _uiLabel;

		private UITexture _uiOverlay;

		public static ProdSortieEnd Instantiate(ProdSortieEnd prefab, Transform parent)
		{
			ProdSortieEnd prodSortieEnd = Object.Instantiate<ProdSortieEnd>(prefab);
			prodSortieEnd.get_transform().set_parent(parent);
			prodSortieEnd.get_transform().localScaleOne();
			prodSortieEnd.get_transform().localPositionZero();
			return prodSortieEnd;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._uiLabel);
			Mem.Del<UITexture>(ref this._uiOverlay);
		}

		public override void Play(Action callback)
		{
			base.get_transform().localScaleOne();
			base.Play(callback);
		}
	}
}
