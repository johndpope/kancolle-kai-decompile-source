using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Viewport Camera"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UIViewport : MonoBehaviour
{
	public Camera sourceCamera;

	public Transform topLeft;

	public Transform bottomRight;

	public float fullSize = 1f;

	private Camera mCam;

	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.get_main();
		}
	}

	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.get_position());
			Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.get_position());
			Rect rect = new Rect(vector.x / (float)Screen.get_width(), vector2.y / (float)Screen.get_height(), (vector2.x - vector.x) / (float)Screen.get_width(), (vector.y - vector2.y) / (float)Screen.get_height());
			float num = this.fullSize * rect.get_height();
			if (rect != this.mCam.get_rect())
			{
				this.mCam.set_rect(rect);
			}
			if (this.mCam.get_orthographicSize() != num)
			{
				this.mCam.set_orthographicSize(num);
			}
		}
	}
}
