using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Camera"), ExecuteInEditMode]
public class UIDragCamera : MonoBehaviour
{
	public UIDraggableCamera draggableCamera;

	private void Awake()
	{
		if (this.draggableCamera == null)
		{
			this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(base.get_gameObject());
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.draggableCamera != null)
		{
			this.draggableCamera.Press(isPressed);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.draggableCamera != null)
		{
			this.draggableCamera.Drag(delta);
		}
	}

	private void OnScroll(float delta)
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.draggableCamera != null)
		{
			this.draggableCamera.Scroll(delta);
		}
	}
}
