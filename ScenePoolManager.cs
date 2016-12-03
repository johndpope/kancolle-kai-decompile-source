using System;
using UnityEngine;

public class ScenePoolManager : SingletonMonoBehaviour<ScenePoolManager>
{
	[SerializeField]
	private GameObject[] SceneObjects;

	public GameObject NowSceneObject;

	public void ChangeSceneObject()
	{
	}
}
