using System;
using UnityEngine;

public class prefabTest : MonoBehaviour
{
	public GameObject go;

	private void Awake()
	{
	}

	private void Start()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.go);
		gameObject.get_transform().positionX(123f);
	}

	private void Update()
	{
	}
}
