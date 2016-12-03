using KCV.Utils;
using System;
using UniRx;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(Animation))]
	public class BaseAnimation : MonoBehaviour
	{
		protected Animation _animAnimation;

		protected bool _isFinished;

		protected bool _isForceFinished;

		protected Action _actForceCallback;

		protected Action _actCallback;

		public Animation animation
		{
			get
			{
				if (this._animAnimation == null)
				{
					base.GetComponent<Animation>();
				}
				return this._animAnimation;
			}
		}

		public bool isFinished
		{
			get
			{
				return this._isFinished;
			}
		}

		public bool isForceFinished
		{
			get
			{
				return this._isForceFinished;
			}
		}

		public virtual bool isPlaying
		{
			get
			{
				return this._animAnimation.get_isPlaying();
			}
		}

		protected virtual void Awake()
		{
			this._animAnimation = this.SafeGetComponent<Animation>();
			this.Init();
		}

		protected virtual void OnDestroy()
		{
			this.UnInit();
			this._animAnimation = null;
		}

		public virtual bool Init()
		{
			this._isFinished = false;
			this._isForceFinished = false;
			this._actCallback = null;
			this._actForceCallback = null;
			return true;
		}

		public virtual bool UnInit()
		{
			this._isFinished = false;
			this._isForceFinished = false;
			this._actForceCallback = null;
			this._actCallback = null;
			return true;
		}

		public virtual void Play()
		{
			this._animAnimation.Play();
		}

		public virtual void Play(Action callback)
		{
			this.Init();
			this._actCallback = callback;
			this._animAnimation.Play();
		}

		public virtual void Play(Enum iEnum, Action callback)
		{
			this.Init();
			this._actCallback = callback;
			this._animAnimation.Play(iEnum.ToString());
		}

		public virtual void Play(Action forceCallback, Action callback)
		{
			this.Init();
			this._actCallback = callback;
			this._actForceCallback = forceCallback;
			this._animAnimation.Play();
		}

		public virtual void Play(Enum iEnum, Action forceCallback, Action callback)
		{
			this.Init();
			this._actCallback = callback;
			this._actForceCallback = forceCallback;
			this._animAnimation.Play(iEnum.ToString());
		}

		public virtual void Discard()
		{
			if (base.get_gameObject() != null)
			{
				Object.Destroy(base.get_gameObject());
			}
		}

		protected virtual void onAnimationFinished()
		{
			this._isFinished = true;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}

		protected virtual void onAnimationFinishedAfterDiscard()
		{
			this.onAnimationFinished();
			Observable.Timer(TimeSpan.FromSeconds(0.15000000596046448)).Subscribe(delegate(long _)
			{
				base.get_gameObject().Discard();
			});
		}

		protected virtual void OnForceAnimationFinished()
		{
			this._isForceFinished = true;
			if (this._actForceCallback != null)
			{
				this._actForceCallback.Invoke();
			}
		}

		protected virtual void _playSE(SEFIleInfos info)
		{
			SoundUtils.PlaySE(info);
		}

		protected virtual Texture2D _loadResources(string fileName)
		{
			return Resources.Load(string.Format("{0}", fileName)) as Texture2D;
		}

		[Obsolete("", false)]
		protected virtual void OnAnimationFinished()
		{
			this._isFinished = true;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this.DelayAction(0.15f, delegate
			{
				base.get_gameObject().Discard();
			});
		}
	}
}
