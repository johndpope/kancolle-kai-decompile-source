using System;
using UnityEngine;

namespace LT
{
	public class LeanAudioOptions
	{
		public Vector3[] vibrato;

		public int frequencyRate = 44100;

		public LeanAudioOptions setFrequency(int frequencyRate)
		{
			this.frequencyRate = frequencyRate;
			return this;
		}

		public LeanAudioOptions setVibrato(Vector3[] vibrato)
		{
			this.vibrato = vibrato;
			return this;
		}
	}
}
