using Common.Enum;
using System;
using UnityEngine;

namespace KCV.Title
{
	public class TitleUtils
	{
		public static AudioSource PlayOpenDifficultyVoice()
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance == null || SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(9997, 1), null);
		}

		public static AudioSource PlayDifficultyVoice(DifficultKind iKind, bool isDecide, Action onFinished)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance == null || SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				return null;
			}
			int voiceNum = 0;
			switch (iKind)
			{
			case DifficultKind.TEI:
				voiceNum = ((!isDecide) ? 10 : 11);
				break;
			case DifficultKind.HEI:
				voiceNum = ((!isDecide) ? 20 : 21);
				break;
			case DifficultKind.OTU:
				voiceNum = ((!isDecide) ? 30 : 31);
				break;
			case DifficultKind.KOU:
				voiceNum = ((!isDecide) ? 40 : 41);
				break;
			case DifficultKind.SHI:
				voiceNum = ((!isDecide) ? 50 : 51);
				break;
			}
			return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(9997, voiceNum), onFinished);
		}
	}
}
