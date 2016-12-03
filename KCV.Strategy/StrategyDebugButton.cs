using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDebugButton : MonoBehaviour
	{
		[SerializeField]
		private UIButton button;

		public UILabel[] labels;

		public Action Act;
	}
}
