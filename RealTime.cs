using System;
using UnityEngine;

public class RealTime : MonoBehaviour
{
	public static float time
	{
		get
		{
			return Time.get_unscaledTime();
		}
	}

	public static float deltaTime
	{
		get
		{
			return Time.get_unscaledDeltaTime();
		}
	}
}
