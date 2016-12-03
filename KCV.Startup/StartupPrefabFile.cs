using System;
using UnityEngine;

namespace KCV.Startup
{
	[Serializable]
	public class StartupPrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabUITutorialConfirmDialog;

		[SerializeField]
		private Transform _prefabCtrlPictureStoryShow;

		[SerializeField]
		private Transform _prefabProdSecretaryShipMovie;

		public Transform prefabUITutorialConfirmDialog
		{
			get
			{
				return this._prefabUITutorialConfirmDialog;
			}
		}

		public Transform prefabCtrlPictureStoryShow
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabCtrlPictureStoryShow);
			}
		}

		public Transform prefabProdSecretaryShipMovie
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdSecretaryShipMovie);
			}
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<Transform>(ref this._prefabUITutorialConfirmDialog);
			Mem.Del<Transform>(ref this._prefabCtrlPictureStoryShow);
			Mem.Del<Transform>(ref this._prefabProdSecretaryShipMovie);
			base.Dispose(disposing);
		}
	}
}
