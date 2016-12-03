using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	public delegate void OnFinished();

	public static SpringPosition current;

	public Vector3 target = Vector3.get_zero();

	public float strength = 10f;

	public bool worldSpace;

	public bool ignoreTimeScale;

	public bool updateScrollView;

	public SpringPosition.OnFinished onFinished;

	[HideInInspector, SerializeField]
	private GameObject eventReceiver;

	[HideInInspector, SerializeField]
	public string callWhenFinished;

	private Transform mTrans;

	private float mThreshold;

	private UIScrollView mSv;

	private void Start()
	{
		this.mTrans = base.get_transform();
		if (this.updateScrollView)
		{
			this.mSv = NGUITools.FindInParents<UIScrollView>(base.get_gameObject());
		}
	}

	private void Update()
	{
		float deltaTime = (!this.ignoreTimeScale) ? Time.get_deltaTime() : RealTime.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.get_position()).get_sqrMagnitude() * 0.001f;
			}
			this.mTrans.set_position(NGUIMath.SpringLerp(this.mTrans.get_position(), this.target, this.strength, deltaTime));
			if (this.mThreshold >= (this.target - this.mTrans.get_position()).get_sqrMagnitude())
			{
				this.mTrans.set_position(this.target);
				this.NotifyListeners();
				base.set_enabled(false);
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.get_localPosition()).get_sqrMagnitude() * 1E-05f;
			}
			this.mTrans.set_localPosition(NGUIMath.SpringLerp(this.mTrans.get_localPosition(), this.target, this.strength, deltaTime));
			if (this.mThreshold >= (this.target - this.mTrans.get_localPosition()).get_sqrMagnitude())
			{
				this.mTrans.set_localPosition(this.target);
				this.NotifyListeners();
				base.set_enabled(false);
			}
		}
		if (this.mSv != null)
		{
			this.mSv.UpdateScrollbars(true);
		}
	}

	private void NotifyListeners()
	{
		SpringPosition.current = this;
		if (this.onFinished != null)
		{
			this.onFinished();
		}
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, this, 1);
		}
		SpringPosition.current = null;
	}

	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.get_enabled())
		{
			springPosition.mThreshold = 0f;
			springPosition.set_enabled(true);
		}
		return springPosition;
	}
}
