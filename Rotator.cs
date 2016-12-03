using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	private void Update()
	{
		base.get_transform().Rotate(0f, 30f * Time.get_deltaTime(), 0f);
	}
}
