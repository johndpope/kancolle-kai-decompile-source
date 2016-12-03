using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class SearchLight : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float _brightness = 1f;

		public Color _baseColor = Color.get_white();

		public LensFlare _flare;

		private void OnDestroy()
		{
			Mem.Del<float>(ref this._brightness);
			Mem.Del<Color>(ref this._baseColor);
			Mem.Del<LensFlare>(ref this._flare);
		}

		private void Update()
		{
			this._flare.set_color(this._baseColor * this._brightness);
		}
	}
}
