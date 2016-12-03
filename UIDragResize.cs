using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	public UIWidget target;

	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	public int minWidth = 100;

	public int minHeight = 100;

	public int maxWidth = 100000;

	public int maxHeight = 100000;

	private Plane mPlane;

	private Vector3 mRayPos;

	private Vector3 mLocalPos;

	private int mWidth;

	private int mHeight;

	private bool mDragging;

	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float num;
			if (this.mPlane.Raycast(currentRay, ref num))
			{
				this.mRayPos = currentRay.GetPoint(num);
				this.mLocalPos = this.target.cachedTransform.get_localPosition();
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float num;
			if (this.mPlane.Raycast(currentRay, ref num))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.set_localPosition(this.mLocalPos);
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 vector = currentRay.GetPoint(num) - this.mRayPos;
				cachedTransform.set_position(cachedTransform.get_position() + vector);
				Vector3 vector2 = Quaternion.Inverse(cachedTransform.get_localRotation()) * (cachedTransform.get_localPosition() - this.mLocalPos);
				cachedTransform.set_localPosition(this.mLocalPos);
				NGUIMath.ResizeWidget(this.target, this.pivot, vector2.x, vector2.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
			}
		}
	}

	private void OnDragEnd()
	{
		this.mDragging = false;
	}
}
