using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Spring Panel"), RequireComponent(typeof(UIPanel))]
public class SpringPanel : MonoBehaviour
{
	public delegate void OnFinished();

	public static SpringPanel current;

	public Vector3 target = Vector3.get_zero();

	public float strength = 10f;

	public SpringPanel.OnFinished onFinished;

	private UIPanel mPanel;

	private Transform mTrans;

	private UIScrollView mDrag;

	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.get_transform();
	}

	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		bool flag = false;
		Vector3 localPosition = this.mTrans.get_localPosition();
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.get_localPosition(), this.target, this.strength, deltaTime);
		if ((vector - this.target).get_sqrMagnitude() < 0.01f)
		{
			vector = this.target;
			base.set_enabled(false);
			flag = true;
		}
		this.mTrans.set_localPosition(vector);
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.set_enabled(true);
		return springPanel;
	}
}
