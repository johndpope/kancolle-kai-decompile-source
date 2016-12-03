using System;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(Animation))]
	public class UIMVPIcon : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._uiBackground = null;
			this._uiForeground = null;
		}
	}
}
