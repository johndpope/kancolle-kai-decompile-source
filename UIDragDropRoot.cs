using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	public static Transform root;

	private void OnEnable()
	{
		UIDragDropRoot.root = base.get_transform();
	}

	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.get_transform())
		{
			UIDragDropRoot.root = null;
		}
	}
}
