using System;
using UnityEngine;

public class UIRotate : MonoBehaviour
{
	public float speed;

	public float rotateRange;

	private float rotateCount;

	private bool UseReverse;

	private void Start()
	{
		if (this.rotateRange != 0f)
		{
			this.UseReverse = true;
		}
		this.rotateCount = 0f;
	}

	private void Update()
	{
		if (this.UseReverse && (this.rotateCount < -this.rotateRange || this.rotateCount > this.rotateRange))
		{
			this.speed *= -1f;
		}
		base.get_gameObject().get_transform().Rotate(0f, 0f, this.speed * Time.get_deltaTime(), 0);
		this.rotateCount += this.speed * Time.get_deltaTime();
	}
}
