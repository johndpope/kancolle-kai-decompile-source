using System;
using UnityEngine;

public class UILoadingText : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Animation>().Play();
	}
}
