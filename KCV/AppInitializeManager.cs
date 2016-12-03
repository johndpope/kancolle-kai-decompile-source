using System;
using UnityEngine;

namespace KCV
{
	public class AppInitializeManager : MonoBehaviour
	{
		public static bool IsInitialize;

		private void Awake()
		{
			Debug.Log("AppInitializeManager Start");
			if (AppInitializeManager.IsInitialize)
			{
				return;
			}
			App.InitLoadMasterDataManager();
			int num = 0;
			while (!App.isMasterInit)
			{
				num++;
			}
			App.Initialize();
			AppInitializeManager.IsInitialize = true;
			Debug.Log("AppInitializeManager Start END");
			if (SceneInitializer.strReturnScene != null)
			{
				Application.LoadLevel(SceneInitializer.strReturnScene);
				return;
			}
		}
	}
}
