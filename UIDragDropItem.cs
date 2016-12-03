using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	public enum Restriction
	{
		None,
		Horizontal,
		Vertical,
		PressAndHold
	}

	public UIDragDropItem.Restriction restriction;

	public bool cloneOnDrag;

	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	public bool interactable = true;

	[NonSerialized]
	protected Transform mTrans;

	[NonSerialized]
	protected Transform mParent;

	[NonSerialized]
	protected Collider mCollider;

	[NonSerialized]
	protected Collider2D mCollider2D;

	[NonSerialized]
	protected UIButton mButton;

	[NonSerialized]
	protected UIRoot mRoot;

	[NonSerialized]
	protected UIGrid mGrid;

	[NonSerialized]
	protected UITable mTable;

	[NonSerialized]
	protected float mDragStartTime;

	[NonSerialized]
	protected UIDragScrollView mDragScrollView;

	[NonSerialized]
	protected bool mPressed;

	[NonSerialized]
	protected bool mDragging;

	[NonSerialized]
	protected UICamera.MouseOrTouch mTouch;

	protected virtual void Start()
	{
		this.mTrans = base.get_transform();
		this.mCollider = base.get_gameObject().GetComponent<Collider>();
		this.mCollider2D = base.get_gameObject().GetComponent<Collider2D>();
		this.mButton = base.GetComponent<UIButton>();
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	protected virtual void OnPress(bool isPressed)
	{
		if (!this.interactable)
		{
			return;
		}
		if (isPressed)
		{
			this.mTouch = UICamera.currentTouch;
			this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
			this.mPressed = true;
		}
		else
		{
			if (this.mDragging)
			{
				this.StopDragging(null);
			}
			this.mPressed = false;
			this.mTouch = null;
		}
	}

	protected virtual void Update()
	{
		if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressed && !this.mDragging && this.mDragStartTime < RealTime.time)
		{
			this.StartDragging();
		}
	}

	protected virtual void OnDragStart()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.get_enabled() || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
			{
				return;
			}
		}
		this.StartDragging();
	}

	protected virtual void StartDragging()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging)
		{
			if (this.cloneOnDrag)
			{
				this.mPressed = false;
				GameObject gameObject = NGUITools.AddChild(base.get_transform().get_parent().get_gameObject(), base.get_gameObject());
				gameObject.get_transform().set_localPosition(base.get_transform().get_localPosition());
				gameObject.get_transform().set_localRotation(base.get_transform().get_localRotation());
				gameObject.get_transform().set_localScale(base.get_transform().get_localScale());
				UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
				if (component != null)
				{
					component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
				}
				if (this.mTouch != null && this.mTouch.pressed == base.get_gameObject())
				{
					this.mTouch.current = gameObject;
					this.mTouch.pressed = gameObject;
					this.mTouch.dragged = gameObject;
					this.mTouch.last = gameObject;
				}
				UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
				component2.mTouch = this.mTouch;
				component2.mPressed = true;
				component2.mDragging = true;
				component2.Start();
				component2.OnDragDropStart();
				if (UICamera.currentTouch == null)
				{
					UICamera.currentTouch = this.mTouch;
				}
				this.mTouch = null;
				UICamera.Notify(base.get_gameObject(), "OnPress", false);
				UICamera.Notify(base.get_gameObject(), "OnHover", false);
			}
			else
			{
				this.mDragging = true;
				this.OnDragDropStart();
			}
		}
	}

	protected virtual void OnDrag(Vector2 delta)
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging || !base.get_enabled() || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
	}

	protected virtual void OnDragEnd()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.get_enabled() || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.StopDragging(UICamera.hoveredObject);
	}

	public void StopDragging(GameObject go)
	{
		if (this.mDragging)
		{
			this.mDragging = false;
			this.OnDragDropRelease(go);
		}
	}

	protected virtual void OnDragDropStart()
	{
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.set_enabled(false);
		}
		if (this.mButton != null)
		{
			this.mButton.isEnabled = false;
		}
		else if (this.mCollider != null)
		{
			this.mCollider.set_enabled(false);
		}
		else if (this.mCollider2D != null)
		{
			this.mCollider2D.set_enabled(false);
		}
		this.mParent = this.mTrans.get_parent();
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.set_parent(UIDragDropRoot.root);
		}
		Vector3 localPosition = this.mTrans.get_localPosition();
		localPosition.z = 0f;
		this.mTrans.set_localPosition(localPosition);
		TweenPosition component = base.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.set_enabled(false);
		}
		SpringPosition component2 = base.GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.set_enabled(false);
		}
		NGUITools.MarkParentAsChanged(base.get_gameObject());
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	protected virtual void OnDragDropMove(Vector2 delta)
	{
		Transform expr_06 = this.mTrans;
		expr_06.set_localPosition(expr_06.get_localPosition() + delta);
	}

	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!this.cloneOnDrag)
		{
			if (this.mButton != null)
			{
				this.mButton.isEnabled = true;
			}
			else if (this.mCollider != null)
			{
				this.mCollider.set_enabled(true);
			}
			else if (this.mCollider2D != null)
			{
				this.mCollider2D.set_enabled(true);
			}
			UIDragDropContainer uIDragDropContainer = (!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface);
			if (uIDragDropContainer != null)
			{
				this.mTrans.set_parent((!(uIDragDropContainer.reparentTarget != null)) ? uIDragDropContainer.get_transform() : uIDragDropContainer.reparentTarget);
				Vector3 localPosition = this.mTrans.get_localPosition();
				localPosition.z = 0f;
				this.mTrans.set_localPosition(localPosition);
			}
			else
			{
				this.mTrans.set_parent(this.mParent);
			}
			this.mParent = this.mTrans.get_parent();
			this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
			this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
			if (this.mDragScrollView != null)
			{
				base.StartCoroutine(this.EnableDragScrollView());
			}
			NGUITools.MarkParentAsChanged(base.get_gameObject());
			if (this.mTable != null)
			{
				this.mTable.repositionNow = true;
			}
			if (this.mGrid != null)
			{
				this.mGrid.repositionNow = true;
			}
			this.OnDragDropEnd();
		}
		else
		{
			NGUITools.Destroy(base.get_gameObject());
		}
	}

	protected virtual void OnDragDropEnd()
	{
	}

	[DebuggerHidden]
	protected IEnumerator EnableDragScrollView()
	{
		UIDragDropItem.<EnableDragScrollView>c__Iterator2 <EnableDragScrollView>c__Iterator = new UIDragDropItem.<EnableDragScrollView>c__Iterator2();
		<EnableDragScrollView>c__Iterator.<>f__this = this;
		return <EnableDragScrollView>c__Iterator;
	}
}
