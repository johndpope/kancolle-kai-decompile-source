using System;
using UnityEngine;

[ExecuteInEditMode]
public class BillboardObject : MonoBehaviour
{
	[SerializeField]
	private Transform _billboardTarget;

	[SerializeField]
	private bool _isBillboard = true;

	[SerializeField]
	private bool _isEnableVerticalRotation;

	public Transform billboardTarget
	{
		get
		{
			return this._billboardTarget;
		}
		set
		{
			this._billboardTarget = value;
		}
	}

	public bool isBillboard
	{
		get
		{
			return this._isBillboard;
		}
		set
		{
			this._isBillboard = value;
		}
	}

	public bool isEnableVerticalRotation
	{
		get
		{
			return this._isEnableVerticalRotation;
		}
		set
		{
			this._isEnableVerticalRotation = value;
		}
	}

	private void LateUpdate()
	{
		if (!this._isBillboard)
		{
			return;
		}
		if (this._billboardTarget == null)
		{
			return;
		}
		if (this.isEnableVerticalRotation)
		{
			base.get_transform().LookAt(this._billboardTarget.get_position());
		}
		else
		{
			Vector3 position = this._billboardTarget.get_position();
			position.y = base.get_transform().get_position().y;
			base.get_transform().LookAt(position);
		}
	}
}
