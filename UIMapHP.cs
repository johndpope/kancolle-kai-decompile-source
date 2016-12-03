using DG.Tweening;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIMapHP : MonoBehaviour
{
	private const int MAX_GAUGE_SIZE = 180;

	private const int MIN_GAUGE_SIZE = 0;

	[SerializeField]
	private UITexture mTexture_Base;

	[SerializeField]
	private UITexture mTexture_Light;

	[SerializeField]
	private UITexture mTexture_Gauge;

	private UIWidget mWidgetThis;

	private MapHPModel mMapHPModel;

	private Tweener mTweener;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 0.001f;
	}

	public void Initialize(MapHPModel model)
	{
		this.mWidgetThis.alpha = 1f;
		this.mMapHPModel = model;
		float percentage = (float)(this.mMapHPModel.NowValue % this.mMapHPModel.MaxValue);
		this.InitializeHPGauge(percentage);
	}

	public void Play()
	{
		if (this.mTweener != null)
		{
			TweenExtensions.Kill(this.mTweener, false);
			this.mTweener = null;
		}
		this.mTweener = TweenSettingsExtensions.SetLoops<Tweener>(DOVirtual.Float(this.mTexture_Light.alpha, 0.3f, 0.8f, delegate(float alpha)
		{
			this.mTexture_Light.alpha = alpha;
		}), 2147483647, 1);
	}

	public void Stop()
	{
		if (this.mTweener != null)
		{
			TweenExtensions.Kill(this.mTweener, false);
			this.mTweener = null;
		}
	}

	private void InitializeHPGauge(float percentage)
	{
		this.mTexture_Gauge.width = (int)(180f * percentage);
	}
}
