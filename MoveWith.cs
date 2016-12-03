using System;
using UnityEngine;

[ExecuteInEditMode]
public class MoveWith : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	public Transform target
	{
		get
		{
			return this._target;
		}
		set
		{
			this._target = value;
		}
	}

	private void OnDestroy()
	{
		this._target = null;
	}

	private void LateUpdate()
	{
		base.get_transform().set_position(this._target.get_position());
	}
}
