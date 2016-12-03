using System;

public class SoundVolume
{
	private float _fBGM = 1f;

	private float _fVoice = 1f;

	private float _fSE = 1f;

	private bool _isMute;

	public float BGM
	{
		get
		{
			return this._fBGM;
		}
		set
		{
			this._fBGM = Mathe.MinMax2F01(value);
		}
	}

	public float Voice
	{
		get
		{
			return this._fVoice;
		}
		set
		{
			this._fVoice = Mathe.MinMax2F01(value);
		}
	}

	public float SE
	{
		get
		{
			return this._fSE;
		}
		set
		{
			this._fSE = Mathe.MinMax2F01(value);
		}
	}

	public bool Mute
	{
		get
		{
			return this._isMute;
		}
		set
		{
			this._isMute = value;
		}
	}

	public void Init(float bgm = 0.6f, float voice = 1f, float se = 1f, bool mute = false)
	{
		this._fBGM = Mathe.MinMax2F01(bgm);
		this._fVoice = Mathe.MinMax2F01(voice);
		this._fSE = Mathe.MinMax2F01(se);
		this._isMute = mute;
	}
}
