using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	public delegate void OnCenterCallback(GameObject centeredObject);

	public float springStrength = 8f;

	public float nextPageThreshold;

	public SpringPanel.OnFinished onFinished;

	public UICenterOnChild.OnCenterCallback onCenter;

	private UIScrollView mScrollView;

	private GameObject mCenteredObject;

	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	private void Start()
	{
		this.Recenter();
	}

	private void OnEnable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = this;
			this.Recenter();
		}
	}

	private void OnDisable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = null;
		}
	}

	private void OnDragFinished()
	{
		if (base.get_enabled())
		{
			this.Recenter();
		}
	}

	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	[ContextMenu("Execute")]
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.get_gameObject());
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.set_enabled(false);
				return;
			}
			if (this.mScrollView)
			{
				this.mScrollView.centerOnChild = this;
				UIScrollView expr_94 = this.mScrollView;
				expr_94.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(expr_94.onDragFinished, new UIScrollView.OnDragNotification(this.OnDragFinished));
			}
			if (this.mScrollView.horizontalScrollBar != null)
			{
				UIProgressBar expr_D6 = this.mScrollView.horizontalScrollBar;
				expr_D6.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_D6.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				UIProgressBar expr_118 = this.mScrollView.verticalScrollBar;
				expr_118.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_118.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transform = base.get_transform();
		if (transform.get_childCount() == 0)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 vector3 = NGUIMath.SpringDampen(ref vector2, 9f, 2f);
		Vector3 vector4 = vector - vector3 * 0.01f;
		float num = 3.40282347E+38f;
		Transform target = null;
		int num2 = 0;
		int num3 = 0;
		int i = 0;
		int childCount = transform.get_childCount();
		int num4 = 0;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			if (child.get_gameObject().get_activeInHierarchy())
			{
				float num5 = Vector3.SqrMagnitude(child.get_position() - vector4);
				if (num5 < num)
				{
					num = num5;
					target = child;
					num2 = i;
					num3 = num4;
				}
				num4++;
			}
			i++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.get_transform() == transform.GetChild(num2))
		{
			Vector3 vector5 = UICamera.currentTouch.totalDelta;
			vector5 = base.get_transform().get_rotation() * vector5;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num6;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num6 = vector5.get_magnitude();
				}
				else
				{
					num6 = vector5.y;
				}
			}
			else
			{
				num6 = vector5.x;
			}
			if (Mathf.Abs(num6) > this.nextPageThreshold)
			{
				UIGrid component = base.GetComponent<UIGrid>();
				if (component != null && component.sorting != UIGrid.Sorting.None)
				{
					List<Transform> childList = component.GetChildList();
					if (num6 > this.nextPageThreshold)
					{
						if (num3 > 0)
						{
							target = childList.get_Item(num3 - 1);
						}
						else
						{
							target = ((!(base.GetComponent<UIWrapContent>() == null)) ? childList.get_Item(childList.get_Count() - 1) : childList.get_Item(0));
						}
					}
					else if (num6 < -this.nextPageThreshold)
					{
						if (num3 < childList.get_Count() - 1)
						{
							target = childList.get_Item(num3 + 1);
						}
						else
						{
							target = ((!(base.GetComponent<UIWrapContent>() == null)) ? childList.get_Item(0) : childList.get_Item(childList.get_Count() - 1));
						}
					}
				}
				else
				{
					Debug.LogWarning("Next Page Threshold requires a sorted UIGrid in order to work properly", this);
				}
			}
		}
		this.CenterOn(target, vector);
	}

	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.get_gameObject();
			Vector3 vector = cachedTransform.InverseTransformPoint(target.get_position());
			Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 vector3 = vector - vector2;
			if (!this.mScrollView.canMoveHorizontally)
			{
				vector3.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				vector3.y = 0f;
			}
			vector3.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.get_localPosition() - vector3, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}
}
