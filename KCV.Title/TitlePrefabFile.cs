using System;
using UnityEngine;

namespace KCV.Title
{
	[Serializable]
	public class TitlePrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabUIPressAnyKey;

		[SerializeField]
		private Transform _prefabCtrlTitleSelectMode;

		[SerializeField]
		private Transform _prefabCtrlDifficultySelect;

		public Transform prefabUIPressAnyKey
		{
			get
			{
				return this._prefabUIPressAnyKey;
			}
		}

		public Transform prefabCtrlTitleSelectMode
		{
			get
			{
				return this._prefabCtrlTitleSelectMode;
			}
		}

		public Transform prefabCtrlDifficultySelect
		{
			get
			{
				return this._prefabCtrlDifficultySelect;
			}
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<Transform>(ref this._prefabUIPressAnyKey);
			Mem.Del<Transform>(ref this._prefabCtrlTitleSelectMode);
			Mem.Del<Transform>(ref this._prefabCtrlDifficultySelect);
			base.Dispose(disposing);
		}
	}
}
