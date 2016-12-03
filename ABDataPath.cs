using System;
using UnityEngine;

public class ABDataPath
{
	public static string AssetBundlePath
	{
		get
		{
			return string.Format("{0}", Application.get_streamingAssetsPath());
		}
	}
}
