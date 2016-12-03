using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)"), ExecuteInEditMode]
public class UIButtonKeys : UIKeyNavigation
{
	public UIButtonKeys selectOnClick;

	public UIButtonKeys selectOnUp;

	public UIButtonKeys selectOnDown;

	public UIButtonKeys selectOnLeft;

	public UIButtonKeys selectOnRight;

	protected override void OnEnable()
	{
		this.Upgrade();
		base.OnEnable();
	}

	public void Upgrade()
	{
		if (this.onClick == null && this.selectOnClick != null)
		{
			this.onClick = this.selectOnClick.get_gameObject();
			this.selectOnClick = null;
			NGUITools.SetDirty(this);
		}
		if (this.onLeft == null && this.selectOnLeft != null)
		{
			this.onLeft = this.selectOnLeft.get_gameObject();
			this.selectOnLeft = null;
			NGUITools.SetDirty(this);
		}
		if (this.onRight == null && this.selectOnRight != null)
		{
			this.onRight = this.selectOnRight.get_gameObject();
			this.selectOnRight = null;
			NGUITools.SetDirty(this);
		}
		if (this.onUp == null && this.selectOnUp != null)
		{
			this.onUp = this.selectOnUp.get_gameObject();
			this.selectOnUp = null;
			NGUITools.SetDirty(this);
		}
		if (this.onDown == null && this.selectOnDown != null)
		{
			this.onDown = this.selectOnDown.get_gameObject();
			this.selectOnDown = null;
			NGUITools.SetDirty(this);
		}
	}
}
