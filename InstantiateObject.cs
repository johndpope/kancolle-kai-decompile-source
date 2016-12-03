using System;
using UnityEngine;

public class InstantiateObject<T> : MonoBehaviour where T : Component
{
	public static T Instantiate(T prefab)
	{
		return Object.Instantiate<T>(prefab);
	}

	public static T Instantiate(T prefab, Transform parent)
	{
		T result = InstantiateObject<T>.Instantiate(prefab);
		result.get_transform().set_parent(parent);
		result.get_transform().localScaleOne();
		result.get_transform().localPositionZero();
		return result;
	}

	public static T Instantiate(T prefab, Transform parent, params object[] param)
	{
		return InstantiateObject<T>.Instantiate(prefab, parent);
	}
}
