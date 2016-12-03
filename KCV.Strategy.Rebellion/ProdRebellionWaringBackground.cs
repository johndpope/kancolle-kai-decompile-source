using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class ProdRebellionWaringBackground : BaseAnimation
	{
		public static ProdRebellionWaringBackground Instantiate(ProdRebellionWaringBackground prefab, Transform parent)
		{
			ProdRebellionWaringBackground prodRebellionWaringBackground = Object.Instantiate<ProdRebellionWaringBackground>(prefab);
			prodRebellionWaringBackground.get_transform().set_parent(parent);
			prodRebellionWaringBackground.get_transform().localScaleOne();
			prodRebellionWaringBackground.get_transform().localScaleZero();
			return prodRebellionWaringBackground;
		}
	}
}
