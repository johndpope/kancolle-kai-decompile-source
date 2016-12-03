using DG.Tweening;
using System;
using UnityEngine;

public class UIDOTweenPosition : MonoBehaviour
{
	[SerializeField]
	private Vector3 mVector3_From;

	[SerializeField]
	private Vector3 mVector3_To;

	[SerializeField]
	private LoopType mLoopType_Type;

	[SerializeField, Tooltip("Minus is Infinite")]
	private int mLoop;

	[SerializeField]
	private Ease mEase;

	[SerializeField]
	private float mDuration;

	[SerializeField]
	private float mDelay;

	private Tween mTween;

	private void Start()
	{
		if (this.mLoop < 0)
		{
			this.mLoop = -2147483648;
		}
		this.mTween = TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions.DOLocalMove(base.get_transform(), this.mVector3_To, this.mDuration, false), this.mLoop, this.mLoopType_Type), this.mDelay), this.mEase);
	}
}
