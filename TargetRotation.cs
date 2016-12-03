using System;
using UnityEngine;

public class TargetRotation : MonoBehaviour
{
	public float angle = 30f;

	public Transform target;

	private Vector3 targetPos;

	public Vector3 startPos;

	public bool horizontal;

	public bool vertical;

	private void Start()
	{
		this.targetPos = this.target.get_position();
		base.get_transform().Rotate(new Vector3(0f, 0f, 0f), 0);
		this.startPos = new Vector3(base.get_transform().get_localPosition().x, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z);
	}

	private void Update()
	{
	}

	private void OnValidate()
	{
		this.targetPos = this.target.get_position();
		base.get_transform().set_localPosition(new Vector3(this.startPos.x, this.startPos.y, this.startPos.z));
		Vector3 vector = new Vector3(1f, 0f, 0f);
		if (this.horizontal)
		{
			vector = new Vector3(0f, 1f, 0f);
		}
		Vector3 vector2 = base.get_transform().TransformDirection(vector);
		base.get_transform().RotateAround(this.targetPos, vector2, this.angle);
		base.get_transform().set_rotation(new Quaternion(0f, 0f, 0f, 0f));
	}
}
