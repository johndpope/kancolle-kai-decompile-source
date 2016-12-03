using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Orthographic Camera"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UIOrthoCamera : MonoBehaviour
{
	private Camera mCam;

	private Transform mTrans;

	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.get_transform();
		this.mCam.set_orthographic(true);
	}

	private void Update()
	{
		float num = this.mCam.get_rect().get_yMin() * (float)Screen.get_height();
		float num2 = this.mCam.get_rect().get_yMax() * (float)Screen.get_height();
		float num3 = (num2 - num) * 0.5f * this.mTrans.get_lossyScale().y;
		if (!Mathf.Approximately(this.mCam.get_orthographicSize(), num3))
		{
			this.mCam.set_orthographicSize(num3);
		}
	}
}
