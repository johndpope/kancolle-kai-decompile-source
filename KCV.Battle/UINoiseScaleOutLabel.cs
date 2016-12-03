using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIWidget)), RequireComponent(typeof(NoiseMove))]
	public class UINoiseScaleOutLabel : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float intervalTime;

			public float scaleOutTime;

			public float scaleOutScale;

			public void Dispose()
			{
				Mem.Del<float>(ref this.intervalTime);
				Mem.Del<float>(ref this.scaleOutTime);
				Mem.Del<float>(ref this.scaleOutScale);
			}
		}

		[SerializeField]
		private UIWidget _uiForeground;

		[Header("[Animation Properties]"), SerializeField]
		private UINoiseScaleOutLabel.Params _strParams;

		private UIWidget _uiWidget;

		private UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		private void Awake()
		{
			this._uiForeground.alpha = 0f;
			this._uiForeground.depth = this.widget.depth + 1;
		}

		private void OnDestroy()
		{
			Mem.Del<UIWidget>(ref this._uiForeground);
			Mem.DelIDisposableSafe<UINoiseScaleOutLabel.Params>(ref this._strParams);
			Mem.Del<UIWidget>(ref this._uiWidget);
		}

		public void Play()
		{
			Observable.Interval(TimeSpan.FromSeconds((double)this._strParams.intervalTime)).Subscribe(delegate(long _)
			{
				this._uiForeground.alpha = 1f;
				this._uiForeground.get_transform().LTValue(1f, 0f, this._strParams.scaleOutTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiForeground.alpha = x;
				});
				this._uiForeground.get_transform().localScaleOne();
				this._uiForeground.get_transform().LTScale(Vector3.get_one() * this._strParams.scaleOutScale, this._strParams.scaleOutTime).setEase(LeanTweenType.linear);
			}).AddTo(base.get_gameObject());
		}
	}
}
