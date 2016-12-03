using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutFlagshipWreck : BaseBattleCutState
	{
		private ProdFlagshipWreck _prodFlagshipWreck;

		private AsyncOperation _async;

		public override bool Init(object data)
		{
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
			this._prodFlagshipWreck = ProdFlagshipWreck.Instantiate(BattleCutManager.GetPrefabFile().prefabProdFlagshipWreck.GetComponent<ProdFlagshipWreck>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().Ships_f[0], BattleCutManager.GetMapManager().Deck, BattleCutManager.GetKeyControl(), true);
			this._prodFlagshipWreck.Play(delegate
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
					SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
					Mst_DataManager.Instance.PurgeUIBattleMaster();
					SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
					RetentionData.SetData(BattleUtils.GetRetentionDataFlagshipWreck(BattleCutManager.GetMapManager(), ShipRecoveryType.None));
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			});
			return false;
		}

		public override bool Terminate(object data)
		{
			if (this._prodFlagshipWreck != null && this._prodFlagshipWreck.get_gameObject() != null)
			{
				Object.Destroy(this._prodFlagshipWreck.get_gameObject());
			}
			this._prodFlagshipWreck = null;
			return false;
		}

		public override bool Run(object data)
		{
			this._prodFlagshipWreck.Run();
			return base.IsCheckPhase(BattleCutPhase.FlagshipWreck);
		}
	}
}
