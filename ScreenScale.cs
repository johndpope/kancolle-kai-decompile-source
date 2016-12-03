using System;
using UnityEngine;

public class ScreenScale : MonoBehaviour
{
	public int maxHeight = 1024;

	private void Start()
	{
		float num = (float)this.maxHeight / (float)Screen.get_height();
		if (num > 1f)
		{
			num = 1f;
		}
		int num2 = (int)((float)Screen.get_width() * num);
		int num3 = (int)((float)Screen.get_height() * num);
		if (Screen.get_height() != num3)
		{
			Screen.SetResolution(num2, num3, true, 15);
		}
	}
}
