using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.PSVita;

[RequireComponent(typeof(Camera))]
public class PSVitaMovie : MonoBehaviour
{
	[SerializeField]
	private RenderTexture _renderTexture;

	private string _strMoviePath = string.Empty;

	private string _strSubTitleText = string.Empty;

	private long _lSubTitleTimeStamp;

	private bool _isPlaying;

	private bool _isBufferingSuccess;

	private bool _isPause;

	private PSVitaVideoPlayer.Looping _iLooping;

	private PSVitaVideoPlayer.Mode _iMode;

	private PSVitaVideoPlayer.PlayParams _strPlayParams = default(PSVitaVideoPlayer.PlayParams);

	private Action _actOnStop;

	private Action _actOnReady;

	private Action _actOnPlay;

	private Action _actOnPause;

	private Action _actOnBuffering;

	private Action _actOnTimedTextDelivery;

	private Action _actOnWarningID;

	private Action _actOnEncryption;

	private Action _actOnFinished;

	public bool isPlaying
	{
		get
		{
			return this._isPlaying;
		}
	}

	public bool isPause
	{
		get
		{
			return this._isPause;
		}
		private set
		{
			this._isPause = value;
		}
	}

	public string moviePath
	{
		get
		{
			return this._strMoviePath;
		}
		private set
		{
			this._strMoviePath = value;
		}
	}

	public RenderTexture renderTexture
	{
		get
		{
			return this._renderTexture;
		}
		set
		{
			if (this._renderTexture != value)
			{
				this._renderTexture = value;
			}
		}
	}

	public long currentTime
	{
		get
		{
			return PSVitaVideoPlayer.get_videoTime();
		}
	}

	public long movieDuration
	{
		get
		{
			return PSVitaVideoPlayer.get_videoDuration();
		}
	}

	public float volume
	{
		set
		{
			PSVitaVideoPlayer.SetVolume(value);
		}
	}

	public PSVitaVideoPlayer.TrickSpeeds trickSpeed
	{
		set
		{
			PSVitaVideoPlayer.SetTrickSpeed(value);
		}
	}

	private void OnPostRender()
	{
		PSVitaVideoPlayer.Update();
		if (this.isPlaying)
		{
			if (!this._isBufferingSuccess && this.currentTime != 0L)
			{
				this.OnEvent(5);
			}
			else if (this._isBufferingSuccess && this.currentTime == 0L && this.movieDuration == 0L && this._iLooping != 1)
			{
				this.AutoOnFinished();
			}
		}
	}

	private void OnDestroy()
	{
		this._renderTexture = null;
		this._strMoviePath = null;
		this._actOnStop = null;
		this._actOnReady = null;
		this._actOnPlay = null;
		this._actOnPause = null;
		this._actOnBuffering = null;
		this._actOnTimedTextDelivery = null;
		this._actOnWarningID = null;
		this._actOnEncryption = null;
	}

	public PSVitaMovie Play(string moviePath)
	{
		this._isPlaying = false;
		this._isBufferingSuccess = false;
		this._isPause = false;
		this.moviePath = moviePath;
		if (this.renderTexture == null && this._iMode == 1)
		{
			this.renderTexture = new RenderTexture(1024, 1024, 0, 0);
		}
		PSVitaVideoPlayer.Init(this.renderTexture);
		Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayMovie(observer)).Subscribe(delegate(bool observer)
		{
			if (observer)
			{
				this.OnEvent(3);
			}
			else
			{
				this.OnEvent(32);
			}
		});
		return this;
	}

	[DebuggerHidden]
	private IEnumerator PlayMovie(IObserver<bool> observer)
	{
		PSVitaMovie.<PlayMovie>c__Iterator1AE <PlayMovie>c__Iterator1AE = new PSVitaMovie.<PlayMovie>c__Iterator1AE();
		<PlayMovie>c__Iterator1AE.observer = observer;
		<PlayMovie>c__Iterator1AE.<$>observer = observer;
		<PlayMovie>c__Iterator1AE.<>f__this = this;
		return <PlayMovie>c__Iterator1AE;
	}

	public PSVitaMovie PlayEx(string moviePath)
	{
		this.moviePath = moviePath;
		PSVitaVideoPlayer.Init(this.renderTexture);
		if (PSVitaVideoPlayer.PlayEx(this.moviePath, this._strPlayParams))
		{
			this.OnEvent(3);
		}
		else
		{
			this.OnEvent(32);
		}
		return this;
	}

	public void Stop()
	{
		this.OnEvent(1);
	}

	public bool Pause()
	{
		this.OnEvent(4);
		this.isPause = true;
		return PSVitaVideoPlayer.Pause();
	}

	public bool Resume()
	{
		this.isPause = false;
		return PSVitaVideoPlayer.Resume();
	}

	public void ImmediateOnFinished()
	{
		PSVitaVideoPlayer.Stop();
		this.AutoOnFinished();
	}

	private void AutoOnFinished()
	{
		this._isPlaying = false;
		this._isBufferingSuccess = false;
		if (this._actOnFinished != null)
		{
			this._actOnFinished.Invoke();
		}
	}

	public bool JumpToTime(ulong jumpTimeMsec)
	{
		return PSVitaVideoPlayer.JumpToTime(jumpTimeMsec);
	}

	public PSVitaMovie SetPlayParams(PSVitaVideoPlayer.PlayParams param)
	{
		this._strPlayParams = param;
		return this;
	}

	public PSVitaMovie SetLooping(PSVitaVideoPlayer.Looping iLooping)
	{
		this._iLooping = iLooping;
		return this;
	}

	public PSVitaMovie SetMode(PSVitaVideoPlayer.Mode iMode)
	{
		this._iMode = iMode;
		return this;
	}

	public PSVitaMovie SetOnStop(Action onStop)
	{
		this._actOnStop = onStop;
		return this;
	}

	public PSVitaMovie SetOnReady(Action onReady)
	{
		this._actOnReady = onReady;
		return this;
	}

	public PSVitaMovie SetOnPlay(Action onPlay)
	{
		this._actOnPlay = onPlay;
		return this;
	}

	public PSVitaMovie SetOnPause(Action onPause)
	{
		this._actOnPause = onPause;
		return this;
	}

	public PSVitaMovie SetOnBuffering(Action onBuffering)
	{
		this._actOnBuffering = onBuffering;
		return this;
	}

	public PSVitaMovie SetOnTimedTextDelivery(Action onTimedTextDelivery)
	{
		this._actOnTimedTextDelivery = onTimedTextDelivery;
		return this;
	}

	public PSVitaMovie SetOnWarningID(Action onWarningID)
	{
		this._actOnWarningID = onWarningID;
		return this;
	}

	public PSVitaMovie SetOnEncryption(Action onEncryption)
	{
		this._actOnEncryption = onEncryption;
		return this;
	}

	public PSVitaMovie SetOnFinished(Action onFinished)
	{
		this._actOnFinished = onFinished;
		return this;
	}

	private void OnEvent(PSVitaVideoPlayer.MovieEvent iEventID)
	{
		switch (iEventID)
		{
		case 1:
			PSVitaVideoPlayer.Stop();
			this._isPlaying = false;
			if (this._actOnStop != null)
			{
				this._actOnStop.Invoke();
			}
			this._strSubTitleText = string.Empty;
			break;
		case 2:
			if (this._actOnReady != null)
			{
				this._actOnReady.Invoke();
			}
			break;
		case 3:
			this._isPlaying = true;
			if (this._actOnPlay != null)
			{
				this._actOnPlay.Invoke();
			}
			break;
		case 4:
			if (this._actOnPause != null)
			{
				this._actOnPause.Invoke();
			}
			break;
		case 5:
			this._isBufferingSuccess = true;
			if (this._actOnBuffering != null)
			{
				this._actOnBuffering.Invoke();
			}
			break;
		default:
			if (iEventID != 16)
			{
				if (iEventID != 32)
				{
					if (iEventID == 48)
					{
						if (this._actOnEncryption != null)
						{
							this._actOnEncryption.Invoke();
						}
					}
				}
				else
				{
					if (this._actOnWarningID != null)
					{
						this._actOnWarningID.Invoke();
					}
					this.ImmediateOnFinished();
				}
			}
			else
			{
				this._strSubTitleText = PSVitaVideoPlayer.get_subtitleText();
				if (this._actOnTimedTextDelivery != null)
				{
					this._actOnTimedTextDelivery.Invoke();
				}
			}
			break;
		}
	}
}
