using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UICamera)), RequireComponent(typeof(Camera))]
	public class BattleCutInCamera : BaseCamera
	{
		protected override void Awake()
		{
			base.Awake();
		}
	}
}
