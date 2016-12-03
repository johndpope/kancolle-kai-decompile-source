using System;
using UnityEngine;

public class DontDestroyObject : SingletonMonoBehaviour<DontDestroyObject>
{
	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(base.get_gameObject());
	}
}
