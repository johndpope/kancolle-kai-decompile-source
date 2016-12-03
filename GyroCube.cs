using System;
using UnityEngine;

public class GyroCube : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.get_transform().set_rotation(Input.get_gyro().get_attitude());
	}
}
