using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;

	public static T Instance
	{
		get
		{
			if (SingletonMonoBehaviour<T>.instance == null)
			{
				SingletonMonoBehaviour<T>.instance = (T)((object)Object.FindObjectOfType(typeof(T)));
				if (SingletonMonoBehaviour<T>.instance == null)
				{
					return (T)((object)null);
				}
			}
			return SingletonMonoBehaviour<T>.instance;
		}
		set
		{
			SingletonMonoBehaviour<T>.instance = value;
		}
	}

	public static T GetInstance()
	{
		return SingletonMonoBehaviour<T>.instance;
	}

	protected virtual void Awake()
	{
		this.CheckInstance();
	}

	public bool CheckInstance()
	{
		if (SingletonMonoBehaviour<T>.instance != null && SingletonMonoBehaviour<T>.instance != this)
		{
			DebugUtils.SLog("★★SingletonObject Destroy " + base.get_gameObject().get_name());
			Object.Destroy(base.get_gameObject());
			return false;
		}
		SingletonMonoBehaviour<T>.instance = SingletonMonoBehaviour<T>.Instance;
		Object.DontDestroyOnLoad(base.get_gameObject());
		return true;
	}

	[Obsolete]
	public static bool exist()
	{
		return SingletonMonoBehaviour<T>.instance != null;
	}
}
