using local.models;
using System;
using UnityEngine;

namespace KCV.Utils
{
	public class ShipUtils
	{
		public static Texture2D LoadTexture(int mstID, int texNum)
		{
			if (SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				DebugUtils.Error("リソースマネージャーが存在しません。");
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mstID, texNum);
		}

		[Obsolete("")]
		public static Texture2D LoadTexture(ShipModel model, int texNum)
		{
			return ShipUtils.LoadTexture(model.GetGraphicsMstId(), texNum);
		}

		public static Texture2D LoadTexture(ShipModel model, bool isDamaged)
		{
			return ShipUtils.LoadTexture(model.GetGraphicsMstId(), (!isDamaged) ? 9 : 10);
		}

		public static Texture2D LoadTexture(ShipModel model)
		{
			return ShipUtils.LoadTexture(model, model.IsDamaged());
		}

		public static Texture2D LoadBannerTexture(IShipModel model, bool isDamaged)
		{
			return ShipUtils.LoadTexture(model.MstId, (!isDamaged) ? 1 : 2);
		}

		public static Texture2D LoadBannerTexture(ShipModel model)
		{
			return ShipUtils.LoadBannerTexture(model, model.IsDamaged());
		}

		public static Texture2D LoadBannerTexture(ShipModel_BattleAll model)
		{
			return ShipUtils.LoadBannerTexture(model, model.DamagedFlgEnd);
		}

		public static Texture2D LoadBannerTexture(ShipModel_BattleResult model)
		{
			bool isDamaged = model.IsFriend() && model.IsDamaged();
			return ShipUtils.LoadBannerTexture(model, isDamaged);
		}

		public static Texture2D LoadCardTexture(IShipModel model, bool isDamaged)
		{
			return ShipUtils.LoadTexture(model.MstId, (!isDamaged) ? 3 : 4);
		}

		public static Texture2D LoadCardTexture(ShipModel model)
		{
			return ShipUtils.LoadCardTexture(model, model.IsDamaged());
		}

		public static void SetTexture(UITexture tex, ShipModel model, int texNum)
		{
			tex.mainTexture = ShipUtils.LoadTexture(model.MstId, texNum);
			if (tex.mainTexture == null)
			{
				return;
			}
			if (ResourceManager.SHIP_TEXTURE_SIZE.ContainsKey(texNum))
			{
				tex.localSize = ResourceManager.SHIP_TEXTURE_SIZE.get_Item(texNum);
			}
			else
			{
				tex.MakePixelPerfect();
			}
		}

		public static int GetShipStandingTextureID(bool isFriend, bool isDamaged)
		{
			if (!isFriend)
			{
				return 9;
			}
			if (isDamaged)
			{
				return 10;
			}
			return 9;
		}

		private static AudioSource PlayShipVoice(int mstId, int voiceNum, int channel, Action onFinished)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(mstId, voiceNum), channel, true, onFinished);
			}
			return null;
		}

		public static AudioSource PlayEndingVoice(ShipModel model, int voiceNum)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(model.GetVoiceMstId(voiceNum), voiceNum), 0, false, null);
			}
			return null;
		}

		public static AudioSource PlayShipVoice(ShipModelMst model, int voiceNum, int channel, Action onFinished)
		{
			return ShipUtils.PlayShipVoice(model.GetVoiceMstId(voiceNum), voiceNum, channel, onFinished);
		}

		public static AudioSource PlayShipVoice(ShipModelMst model, int voiceNum)
		{
			return ShipUtils.PlayShipVoice(model, voiceNum, 0, null);
		}

		public static AudioSource PlayShipVoice(ShipModelMst model, int voiceNum, Action onFinished)
		{
			return ShipUtils.PlayShipVoice(model, voiceNum, 0, onFinished);
		}

		public static AudioSource PlayPortVoice(int voiceNum)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayOneShotVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, voiceNum));
			}
			return null;
		}

		public static AudioSource PlayPortVoice(int voiceNum, Action Onfinished)
		{
			return ShipUtils.PlayShipVoice(0, voiceNum, 0, Onfinished);
		}

		public static AudioSource PlayTitleVoice(int voiceNum)
		{
			return ShipUtils.PlayShipVoice(9999, voiceNum, 0, null);
		}

		public static AudioSource StopShipVoice(int channel)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.StopVoice(channel);
			}
			return null;
		}

		public static AudioSource StopShipVoice()
		{
			return ShipUtils.StopShipVoice(0);
		}

		public static AudioSource StopShipVoice(AudioSource source, bool isCallOnFinished, float fDuration)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopVoice(source, isCallOnFinished, fDuration);
			}
			return null;
		}
	}
}
