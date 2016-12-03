using Common.Enum;
using KCV.Battle.Utils;
using local.managers;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutAdvancingWithdrawal : BaseBattleCutState
	{
		private ProdBCAdvancingWithdrawal _prodBCAdvancingWithdrawal;

		public override bool Init(object data)
		{
			Observable.FromCoroutine<ProdBCAdvancingWithdrawal>((IObserver<ProdBCAdvancingWithdrawal> observer) => this.CreateAdvancingWithdrawal(observer)).Subscribe(delegate(ProdBCAdvancingWithdrawal x)
			{
				x.Play(new Action<AdvancingWithdrawalType>(this.OnDecideAdvancingWithdrawal));
			});
			return false;
		}

		public override bool Run(object data)
		{
			if (this._prodBCAdvancingWithdrawal != null)
			{
				this._prodBCAdvancingWithdrawal.Run();
			}
			return base.IsCheckPhase(BattleCutPhase.AdvancingWithdrawal);
		}

		public override bool Terminate(object data)
		{
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateAdvancingWithdrawal(IObserver<ProdBCAdvancingWithdrawal> observer)
		{
			StateBattleCutAdvancingWithdrawal.<CreateAdvancingWithdrawal>c__Iterator10F <CreateAdvancingWithdrawal>c__Iterator10F = new StateBattleCutAdvancingWithdrawal.<CreateAdvancingWithdrawal>c__Iterator10F();
			<CreateAdvancingWithdrawal>c__Iterator10F.observer = observer;
			<CreateAdvancingWithdrawal>c__Iterator10F.<$>observer = observer;
			<CreateAdvancingWithdrawal>c__Iterator10F.<>f__this = this;
			return <CreateAdvancingWithdrawal>c__Iterator10F;
		}

		private void OnDecideAdvancingWithdrawal(AdvancingWithdrawalType iType)
		{
			switch (iType)
			{
			case AdvancingWithdrawalType.Withdrawal:
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						Mst_DataManager.Instance.PurgeUIBattleMaster();
						RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawal(BattleCutManager.GetMapManager(), ShipRecoveryType.None));
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					});
				}
				break;
			case AdvancingWithdrawalType.Advance:
				BattleCutManager.EndBattleCut(ShipRecoveryType.None);
				break;
			case AdvancingWithdrawalType.AdvancePrimary:
			{
				MapManager mapManager = BattleCutManager.GetMapManager();
				mapManager.ChangeCurrentDeck();
				BattleCutManager.EndBattleCut(ShipRecoveryType.None);
				break;
			}
			}
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
			SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
			Object.Destroy(this._prodBCAdvancingWithdrawal.get_gameObject());
			Mem.Del<ProdBCAdvancingWithdrawal>(ref this._prodBCAdvancingWithdrawal);
		}
	}
}
