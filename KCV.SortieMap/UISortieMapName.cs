using local.managers;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UISortieMapName : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiAreaName;

		[SerializeField]
		private UITexture _uiBackground;

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this._uiAreaName);
			Mem.Del<UITexture>(ref this._uiBackground);
		}

		public void SetMapInformation(MapManager manager)
		{
			this._uiAreaName.text = manager.Map.Name;
		}
	}
}
