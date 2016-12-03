using System;

namespace KCV.Battle.Utils
{
	public struct SoundKeep
	{
		private float _bgmVol;

		private float _seVol;

		private float _voiceVol;

		public float BGMVolume
		{
			get
			{
				return this._bgmVol;
			}
			set
			{
				this._bgmVol = value;
			}
		}

		public float SeVolume
		{
			get
			{
				return this._seVol;
			}
			set
			{
				this._seVol = value;
			}
		}

		public float VoiceVolume
		{
			get
			{
				return this._voiceVol;
			}
			set
			{
				this._voiceVol = value;
			}
		}
	}
}
