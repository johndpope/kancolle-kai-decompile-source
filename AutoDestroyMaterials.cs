using System;
using UnityEngine;

public class AutoDestroyMaterials : MonoBehaviour
{
	[NonSerialized]
	public bool IsRoot = true;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}
}
