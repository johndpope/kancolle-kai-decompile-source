using KCV;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	[Serializable]
	public class AudioSourceObserver : IDisposable
	{
		private AudioSource _asAudioSource;

		private IDisposable _disFinishedDisposable;

		private IDisposable _disStopDisposable;

		private Action _actOnFinished;

		public AudioSource source
		{
			get
			{
				return this._asAudioSource;
			}
			private set
			{
				this._asAudioSource = value;
			}
		}

		public float clipLength
		{
			get
			{
				return (!(this._asAudioSource.get_clip() != null)) ? 0f : this._asAudioSource.get_clip().get_length();
			}
		}

		public AudioSourceObserver(AudioSource source)
		{
			this._asAudioSource = source;
			this._disFinishedDisposable = null;
			this._disStopDisposable = null;
			this._actOnFinished = null;
		}

		public void Dispose()
		{
			Mem.Del<AudioSource>(ref this._asAudioSource);
			Mem.DelIDisposableSafe<IDisposable>(ref this._disFinishedDisposable);
			Mem.DelIDisposableSafe<IDisposable>(ref this._disStopDisposable);
			Mem.Del<Action>(ref this._actOnFinished);
		}

		public SoundManager.AudioSourceObserver PlayOneShot(AudioClip clip, float fVolume)
		{
			return this.Play(clip, fVolume, true, false, null);
		}

		public SoundManager.AudioSourceObserver Play(AudioClip clip, float fVolume, bool isObserver, Action onFinished)
		{
			return this.Play(clip, fVolume, false, isObserver, onFinished);
		}

		private SoundManager.AudioSourceObserver Play(AudioClip clip, float fVolume, bool isOneShot, bool isObserver, Action onFinished)
		{
			if (clip == null)
			{
				return this;
			}
			Mem.DelIDisposableSafe<IDisposable>(ref this._disFinishedDisposable);
			Mem.DelIDisposableSafe<IDisposable>(ref this._disStopDisposable);
			this._actOnFinished = onFinished;
			this._asAudioSource.set_volume(fVolume);
			if (isOneShot)
			{
				this._asAudioSource.PlayOneShot(clip);
			}
			else
			{
				this._asAudioSource.set_clip(clip);
				this._asAudioSource.Play();
				if (!this._asAudioSource.get_loop())
				{
					TimeSpan arg_A5_0 = TimeSpan.FromSeconds((double)this._asAudioSource.get_clip().get_length());
					IScheduler arg_A5_1;
					if (isObserver)
					{
						IScheduler mainThread = Scheduler.MainThread;
						arg_A5_1 = mainThread;
					}
					else
					{
						arg_A5_1 = Scheduler.MainThreadIgnoreTimeScale;
					}
					this._disFinishedDisposable = Observable.Timer(arg_A5_0, arg_A5_1).Subscribe(delegate(long _)
					{
						this.OnAudioPlayFinished();
					});
				}
			}
			return this;
		}

		private void OnAudioPlayFinished()
		{
			Dlg.Call(ref this._actOnFinished);
			this.Clear();
		}

		public SoundManager.AudioSourceObserver Stop(bool isCallOnFinished, float fDuration)
		{
			if (this._disFinishedDisposable != null)
			{
				this._disFinishedDisposable.Dispose();
			}
			this._disStopDisposable = null;
			if (fDuration == 0f || !this._asAudioSource.get_isPlaying())
			{
				if (isCallOnFinished)
				{
					Dlg.Call(ref this._actOnFinished);
				}
				this.Clear();
			}
			else
			{
				this._disStopDisposable = SoundManager.Utils.Fade(this._asAudioSource, 0f, fDuration, delegate
				{
					if (isCallOnFinished)
					{
						Dlg.Call(ref this._actOnFinished);
					}
					this.Clear();
				});
			}
			return this;
		}

		public SoundManager.AudioSourceObserver Stop(bool isCallOnFinished)
		{
			return this.Stop(isCallOnFinished, 0f);
		}

		public SoundManager.AudioSourceObserver StopFade(float fDuration, Action onFinished)
		{
			Mem.DelIDisposableSafe<IDisposable>(ref this._disFinishedDisposable);
			this._disStopDisposable = SoundManager.Utils.Fade(this._asAudioSource, 0f, fDuration, delegate
			{
				this._asAudioSource.Stop();
				Mem.DelIDisposableSafe<IDisposable>(ref this._disStopDisposable);
				Dlg.Call(ref onFinished);
			});
			return this;
		}

		public void Clear()
		{
			Mem.DelIDisposableSafe<IDisposable>(ref this._disFinishedDisposable);
			Mem.DelIDisposableSafe<IDisposable>(ref this._disStopDisposable);
			this.UnLoadAudioSourceClip();
			Mem.Del<Action>(ref this._actOnFinished);
		}

		private void UnLoadAudioSourceClip()
		{
			if (this._asAudioSource != null)
			{
				if (this._asAudioSource.get_isPlaying())
				{
					this._asAudioSource.Stop();
				}
				this._asAudioSource.set_clip(null);
			}
		}
	}

	public class Utils
	{
		public static IDisposable Fade(AudioSource source, float fTo, float fDuration, Action onFinished)
		{
			float time = 0f;
			float tempVolime = source.get_volume();
			return (from x in Observable.EveryUpdate()
			select time += Time.get_deltaTime()).TakeWhile((float x) => fDuration >= x).Subscribe(delegate(float _)
			{
				source.set_volume(Mathe.Lerp(tempVolime, fTo, _ / fDuration));
			}, delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public static float GetClipLength(AudioClip clip)
		{
			if (clip == null)
			{
				return 0f;
			}
			return clip.get_length();
		}
	}

	public const int SIMULTANEOUS_PLAYBACK_NUM = 18;

	private SoundVolume _clsVolume = new SoundVolume();

	private SoundManager.AudioSourceObserver _clsBGMObserver;

	private List<SoundManager.AudioSourceObserver> _listSEObserver;

	private List<SoundManager.AudioSourceObserver> _listVoiceObserver;

	public SoundVolume soundVolume
	{
		get
		{
			return this._clsVolume;
		}
	}

	public AudioSource bgmSource
	{
		get
		{
			return this._clsBGMObserver.source;
		}
	}

	public List<SoundManager.AudioSourceObserver> voiceSource
	{
		get
		{
			return this._listVoiceObserver;
		}
	}

	public List<SoundManager.AudioSourceObserver> seSourceObserver
	{
		get
		{
			return this._listSEObserver;
		}
	}

	public bool isBGMPlaying
	{
		get
		{
			return this._clsBGMObserver.source.get_isPlaying();
		}
	}

	public bool isAnySEPlaying
	{
		get
		{
			using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listSEObserver.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundManager.AudioSourceObserver current = enumerator.get_Current();
					if (current.source.get_isPlaying())
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool isAnyVoicePlaying
	{
		get
		{
			using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listVoiceObserver.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundManager.AudioSourceObserver current = enumerator.get_Current();
					if (current.source.get_isPlaying())
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public float rawBGMVolume
	{
		get
		{
			return this._clsBGMObserver.source.get_volume();
		}
		set
		{
			SoundVolume arg_1F_0 = this.soundVolume;
			float num = Mathe.MinMax2F01(value);
			this._clsBGMObserver.source.set_volume(num);
			arg_1F_0.BGM = num;
		}
	}

	public float rawVoiceVolume
	{
		get
		{
			return this._listVoiceObserver.get_Item(0).source.get_volume();
		}
		set
		{
			float val = Mathe.MinMax2F01(value);
			this.soundVolume.Voice = val;
			this._listVoiceObserver.ForEach(delegate(SoundManager.AudioSourceObserver x)
			{
				x.source.set_volume(val);
			});
		}
	}

	public float rawSEVolume
	{
		get
		{
			return this._listSEObserver.get_Item(0).source.get_volume();
		}
		set
		{
			float num = Mathe.MinMax2F01(value);
			this.soundVolume.SE = num;
			using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listSEObserver.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundManager.AudioSourceObserver current = enumerator.get_Current();
					current.source.set_volume(num);
				}
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this._clsBGMObserver = new SoundManager.AudioSourceObserver(base.get_transform().FindChild("BGM").AddComponent<AudioSource>());
		this._clsBGMObserver.source.set_priority(0);
		this._clsBGMObserver.source.set_playOnAwake(false);
		this._clsBGMObserver.source.set_loop(true);
		this._listVoiceObserver = new List<SoundManager.AudioSourceObserver>(18);
		for (int i = 0; i < 18; i++)
		{
			this._listVoiceObserver.Add(new SoundManager.AudioSourceObserver(base.get_transform().FindChild("Voice").AddComponent<AudioSource>()));
			this._listVoiceObserver.get_Item(i).source.set_playOnAwake(false);
			this._listVoiceObserver.get_Item(i).source.set_loop(false);
		}
		this._listSEObserver = new List<SoundManager.AudioSourceObserver>(18);
		for (int j = 0; j < 18; j++)
		{
			this._listSEObserver.Add(new SoundManager.AudioSourceObserver(base.get_transform().FindChild("SE").AddComponent<AudioSource>()));
			this._listSEObserver.get_Item(j).source.set_playOnAwake(false);
			this._listSEObserver.get_Item(j).source.set_loop(false);
		}
		this._clsVolume.Init(0.6f, 1f, 1f, false);
	}

	public AudioSource PlayBGM(AudioClip clip)
	{
		return this.PlayBGM(clip, true);
	}

	public AudioSource PlayBGM(BGMFileInfos file)
	{
		return this.PlayBGM(file, true);
	}

	public AudioSource PlayBGM(BGMFileInfos file, bool isLoop)
	{
		return this.PlayBGM(SoundFile.LoadBGM(file), isLoop);
	}

	public AudioSource PlayBGM(AudioClip clip, bool isLoop)
	{
		if (clip == null)
		{
			return this._clsBGMObserver.source;
		}
		if (this._clsBGMObserver.source.get_clip() == clip && this._clsBGMObserver.source.get_isPlaying())
		{
			return this._clsBGMObserver.source;
		}
		this._clsBGMObserver.source.set_loop(isLoop);
		this._clsBGMObserver.Play(clip, this.soundVolume.BGM, true, null);
		return this._clsBGMObserver.source;
	}

	[Obsolete("KCV.Utils.SoundUtils::PlaySwotchBGM()を使用してください", false)]
	public AudioSource SwitchBGM(BGMFileInfos file)
	{
		return this.SwitchBGM(SoundFile.LoadBGM(file), true);
	}

	private AudioSource SwitchBGM(AudioClip clip, bool isLoop)
	{
		if (clip == null)
		{
			return this._clsBGMObserver.source;
		}
		if (this._clsBGMObserver.source.get_clip() != null && clip == this._clsBGMObserver.source.get_clip() && this._clsBGMObserver.source.get_isPlaying())
		{
			return this._clsBGMObserver.source;
		}
		this._clsBGMObserver.StopFade(0.05f, delegate
		{
			this.PlayBGM(clip, isLoop);
		});
		return this._clsBGMObserver.source;
	}

	public AudioSource StopBGM()
	{
		return this._clsBGMObserver.Stop(false).source;
	}

	public AudioSource StopFadeBGM(float duration, Action onFinished)
	{
		return this._clsBGMObserver.StopFade(duration, delegate
		{
			Dlg.Call(ref onFinished);
		}).source;
	}

	public AudioSource PlaySE(SEFIleInfos file, Action onFinished)
	{
		return this.PlaySE(SoundFile.LoadSE(file), false, onFinished);
	}

	public AudioSource PlayOneShotSE(SEFIleInfos info)
	{
		return this.PlaySE(SoundFile.LoadSE(info), true, null);
	}

	public AudioSource PlaySE(AudioClip clip, bool isOneShot, Action onFinished)
	{
		using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listSEObserver.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SoundManager.AudioSourceObserver current = enumerator.get_Current();
				if (!current.source.get_isPlaying())
				{
					return (!isOneShot) ? current.Play(clip, this._clsVolume.SE, isOneShot, onFinished).source : current.PlayOneShot(clip, this._clsVolume.SE).source;
				}
			}
		}
		return null;
	}

	public AudioSource StopSE()
	{
		return this._listSEObserver.get_Item(0).Stop(false).source;
	}

	public AudioSource StopSE(AudioSource source, bool isCallOnFinished, float fDuration)
	{
		if (source == null)
		{
			return null;
		}
		using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listSEObserver.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SoundManager.AudioSourceObserver current = enumerator.get_Current();
				if (current.source == source)
				{
					return current.Stop(isCallOnFinished, fDuration).source;
				}
			}
		}
		return null;
	}

	public void StopSE(float duration)
	{
		this._listSEObserver.get_Item(0).Stop(false, duration);
	}

	public void StopAllSE(float fDuration)
	{
		using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listSEObserver.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SoundManager.AudioSourceObserver current = enumerator.get_Current();
				current.Stop(false, fDuration);
			}
		}
	}

	public AudioSource PlayVoice(AudioClip clip)
	{
		return this.PlayVoice(clip, 0);
	}

	public AudioSource PlayVoice(AudioClip clip, int index)
	{
		return this.PlayVoice(clip, index, true, null);
	}

	public AudioSource PlayVoice(AudioClip clip, int nIndex, bool isObserver, Action onFinished)
	{
		return this._listVoiceObserver.get_Item(nIndex).Play(clip, this._clsVolume.Voice, isObserver, onFinished).source;
	}

	public AudioSource PlayOneShotVoice(AudioClip clip)
	{
		return this._listVoiceObserver.get_Item(0).PlayOneShot(clip, this._clsVolume.Voice).source;
	}

	public AudioSource PlayVoice(AudioClip clip, Action onFinished)
	{
		return this.PlayVoice(clip, 0, true, onFinished);
	}

	public float VoiceLength(int nIndex)
	{
		return this._listVoiceObserver.get_Item(nIndex).clipLength;
	}

	public float VoiceLength(AudioClip clip)
	{
		return SoundManager.Utils.GetClipLength(clip);
	}

	public AudioSource StopVoice()
	{
		return this.StopVoice(0);
	}

	public AudioSource StopVoice(int index)
	{
		if (this._listVoiceObserver != null && index < this._listVoiceObserver.get_Count() - 1 && this._listVoiceObserver.get_Item(index).source.get_isPlaying())
		{
			return this._listVoiceObserver.get_Item(index).Stop(false).source;
		}
		return null;
	}

	public AudioSource StopVoice(AudioSource source, bool isCallOnFinished, float fDuration)
	{
		if (source == null)
		{
			return null;
		}
		using (List<SoundManager.AudioSourceObserver>.Enumerator enumerator = this._listVoiceObserver.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SoundManager.AudioSourceObserver current = enumerator.get_Current();
				if (current.source == source)
				{
					return current.Stop(isCallOnFinished, fDuration).source;
				}
			}
		}
		return null;
	}

	public bool isVoicePlaying(int index)
	{
		return this._listVoiceObserver.get_Item(index) != null && this._listVoiceObserver.get_Item(index).source.get_isPlaying();
	}

	public void Release()
	{
		this.ReleaseBGM();
		this.ReleaseSE();
		this.ReleaseVoice();
	}

	public void ReleaseBGM()
	{
		this._clsBGMObserver.Clear();
	}

	public void ReleaseSE()
	{
		this._listSEObserver.ForEach(delegate(SoundManager.AudioSourceObserver x)
		{
			x.Clear();
		});
	}

	public void ReleaseVoice()
	{
		this._listVoiceObserver.ForEach(delegate(SoundManager.AudioSourceObserver x)
		{
			x.Clear();
		});
	}

	public AudioSource SwitchBGM(AudioClip file)
	{
		return this.SwitchBGM(file, true);
	}

	[Obsolete("KCV.Utils.SoundUtilsに追加して使用してください。", false)]
	public AudioSource GeneratePlayJukeAudioSource(BGMFileInfos file)
	{
		AudioClip clip = SoundFile.LoadBGM(file);
		this._clsBGMObserver.source.set_clip(clip);
		return this._clsBGMObserver.source;
	}
}
