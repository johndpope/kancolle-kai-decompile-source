using KCV;
using System;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
	public static string strReturnScene;

	public string InitSceneName;

	private void Awake()
	{
		DebugUtils.SLog("SceneInitializer Awake START");
		if (AppInitializeManager.IsInitialize)
		{
			SceneInitializer.strReturnScene = null;
			Object.Destroy(base.get_gameObject());
			return;
		}
		SceneInitializer.strReturnScene = string.Copy(Application.get_loadedLevelName());
		if (this.InitSceneName != string.Empty)
		{
			Application.LoadLevel(this.InitSceneName);
		}
		DebugUtils.SLog("SceneInitializer Awake END");
	}

	private void Start()
	{
	}
}
