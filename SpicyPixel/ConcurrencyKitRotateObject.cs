using System;
using UnityEngine;

namespace SpicyPixel
{
	public class ConcurrencyKitRotateObject : MonoBehaviour
	{
		private void Start()
		{
		}

		private void Update()
		{
			base.get_transform().RotateAround(Vector3.get_up(), -2f * Time.get_deltaTime());
		}
	}
}
