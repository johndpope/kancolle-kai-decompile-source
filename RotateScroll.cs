using System;
using UnityEngine;

public class RotateScroll : MonoBehaviour
{
	public GameObject obj;

	public float speed = 0.05f;

	private void Update()
	{
		if (Input.get_touchCount() > 0)
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.get_phase())
			{
			case 0:
				Debug.Log("touch  Panel start");
				break;
			case 1:
			{
				Vector2 vector = new Vector2(touch.get_position().x, (float)Screen.get_height() - touch.get_position().y);
				Vector2 vector2 = Camera.get_main().ScreenToWorldPoint(vector);
				float num = touch.get_deltaPosition().x * this.speed * 10f;
				float num2 = touch.get_deltaPosition().y * this.speed * 10f;
				float num3;
				if (base.get_transform().get_position().x > vector2.x)
				{
					if (base.get_transform().get_position().y > vector2.y)
					{
						num3 = -num2 - num;
					}
					else
					{
						num3 = -num2 + num;
					}
				}
				else if (base.get_transform().get_position().y > vector2.y)
				{
					num3 = num2 - num;
				}
				else
				{
					num3 = num2 + num;
				}
				this.obj.get_transform().Rotate(0f, 0f, num3, 0);
				break;
			}
			case 3:
				Debug.Log("touch  Panel end");
				break;
			}
		}
	}
}
