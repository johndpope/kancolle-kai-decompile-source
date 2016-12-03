using System;
using UnityEngine;

public class circleMove : MonoBehaviour
{
	public GameObject target;

	public Vector3 offset;

	public float x;

	public float y;

	public float z;

	public float a;

	public void Start()
	{
	}

	public void Update()
	{
		if (this.target != null)
		{
			Vector3 vector = Quaternion.Euler(0f, this.y, 0f) * this.offset;
			base.get_transform().set_position(vector + this.target.get_transform().get_position());
		}
	}

	private void OnValidate()
	{
		if (this.target != null)
		{
			this.a = Mathf.Sin(this.y / 360f * 3.14159274f);
			Vector3 vector = Quaternion.Euler(this.x, this.y, this.z) * this.offset * Mathf.Sin(this.y * 3.14159274f);
			base.get_transform().set_position(vector + this.target.get_transform().get_position());
		}
	}
}
