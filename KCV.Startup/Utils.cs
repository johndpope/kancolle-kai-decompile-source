using System;
using UnityEngine;

namespace KCV.Startup
{
	public class Utils
	{
		public static bool ChkNGWard(string word)
		{
			return false;
		}

		public static AudioSource PlayAdmiralNameVoice()
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance == null || SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(9997, 60), null);
		}

		public static AudioSource PlayDescriptionVoice(int nVoiceNum, Action onFinished)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance == null || SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(9998, nVoiceNum), onFinished);
		}

		public static AudioSource StopDescriptionVoice()
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<SoundManager>.Instance.StopVoice();
		}
	}
}
