using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Object"), ExecuteInEditMode]
public class UIDragObject : MonoBehaviour
{
	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public Transform target;

	public UIPanel panelRegion;

	public Vector3 scrollMomentum = Vector3.get_zero();

	public bool restrictWithinPanel;

	public UIRect contentRect;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	[SerializeField]
	protected Vector3 scale = new Vector3(1f, 1f, 0f);

	[HideInInspector, SerializeField]
	private float scrollWheelFactor;

	private Plane mPlane;

	private Vector3 mTargetPos;

	private Vector3 mLastPos;

	private Vector3 mMomentum = Vector3.get_zero();

	private Vector3 mScroll = Vector3.get_zero();

	private Bounds mBounds;

	private int mTouchID;

	private bool mStarted;

	private bool mPressed;

	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.get_isPlaying())
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
		this.mTargetPos = ((!(this.target != null)) ? Vector3.get_zero() : this.target.get_position());
	}

	private void OnDisable()
	{
		this.mStarted = false;
	}

	private void FindPanel()
	{
		this.panelRegion = ((!(this.target != null)) ? null : UIPanel.Find(this.target.get_transform().get_parent()));
		if (this.panelRegion == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	private void UpdateBounds()
	{
		if (this.contentRect)
		{
			Transform cachedTransform = this.panelRegion.cachedTransform;
			Matrix4x4 worldToLocalMatrix = cachedTransform.get_worldToLocalMatrix();
			Vector3[] worldCorners = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
			}
			this.mBounds = new Bounds(worldCorners[0], Vector3.get_zero());
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(worldCorners[j]);
			}
		}
		else
		{
			this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.panelRegion == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transform = UICamera.currentCamera.get_transform();
					this.mPlane = new Plane(((!(this.panelRegion != null)) ? transform.get_rotation() : this.panelRegion.cachedTransform.get_rotation()) * Vector3.get_back(), UICamera.lastWorldPosition);
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float num = 0f;
			if (this.mPlane.Raycast(ray, ref num))
			{
				Vector3 point = ray.GetPoint(num);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.get_zero();
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				Vector3 localPosition = this.target.get_localPosition();
				this.Move(vector);
				if (this.restrictWithinPanel)
				{
					this.mBounds.set_center(this.mBounds.get_center() + (this.target.get_localPosition() - localPosition));
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	private void Move(Vector3 worldDelta)
	{
		if (this.panelRegion != null)
		{
			this.mTargetPos += worldDelta;
			this.target.set_position(this.mTargetPos);
			Vector3 localPosition = this.target.get_localPosition();
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			this.target.set_localPosition(localPosition);
			UIScrollView component = this.panelRegion.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		else
		{
			Transform expr_9C = this.target;
			expr_9C.set_position(expr_9C.get_position() + worldDelta);
		}
	}

	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.get_zero(), 20f, deltaTime);
		if (this.mMomentum.get_magnitude() < 0.0001f)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.panelRegion == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
			if (this.restrictWithinPanel && this.panelRegion != null)
			{
				this.UpdateBounds();
				if (this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelMovement();
				}
				else
				{
					this.CancelSpring();
				}
			}
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
			if (this.mMomentum.get_magnitude() < 0.0001f)
			{
				this.CancelMovement();
			}
		}
		else
		{
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	public void CancelMovement()
	{
		if (this.target != null)
		{
			Vector3 localPosition = this.target.get_localPosition();
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			this.target.set_localPosition(localPosition);
		}
		this.mTargetPos = ((!(this.target != null)) ? Vector3.get_zero() : this.target.get_position());
		this.mMomentum = Vector3.get_zero();
		this.mScroll = Vector3.get_zero();
	}

	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.set_enabled(false);
		}
	}

	private void OnScroll(float delta)
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()))
		{
			this.mScroll -= this.scrollMomentum * (delta * 0.05f);
		}
	}
}
