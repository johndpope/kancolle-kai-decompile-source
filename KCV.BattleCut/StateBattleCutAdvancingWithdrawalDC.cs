using Common.Enum;
using KCV.Battle.Utils;
using local.utils;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutAdvancingWithdrawalDC : BaseBattleCutState
	{
		private ProdBCAdvancingWithdrawalDC _prodBCAdvancingWithdrawalDC;

		public override bool Init(object data)
		{
			Observable.FromCoroutine<ProdBCAdvancingWithdrawalDC>((IObserver<ProdBCAdvancingWithdrawalDC> observer) => this.CreateBCAdvancingWithdrawalDC(observer)).Subscribe(delegate(ProdBCAdvancingWithdrawalDC x)
			{
				x.Play(new Action<AdvancingWithdrawalDCType, ShipRecoveryType>(this.OnDecideAdvancingWithdrawal));
			});
			return false;
		}

		public override bool Terminate(object data)
		{
			return base.Terminate(data);
		}

		public override bool Run(object data)
		{
			if (this._prodBCAdvancingWithdrawalDC != null)
			{
				this._prodBCAdvancingWithdrawalDC.Run();
			}
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateBCAdvancingWithdrawalDC(IObserver<ProdBCAdvancingWithdrawalDC> observer)
		{
			StateBattleCutAdvancingWithdrawalDC.<CreateBCAdvancingWithdrawalDC>c__Iterator110 <CreateBCAdvancingWithdrawalDC>c__Iterator = new StateBattleCutAdvancingWithdrawalDC.<CreateBCAdvancingWithdrawalDC>c__Iterator110();
			<CreateBCAdvancingWithdrawalDC>c__Iterator.observer = observer;
			<CreateBCAdvancingWithdrawalDC>c__Iterator.<$>observer = observer;
			<CreateBCAdvancingWithdrawalDC>c__Iterator.<>f__this = this;
			return <CreateBCAdvancingWithdrawalDC>c__Iterator;
		}

		private void OnDecideAdvancingWithdrawal(AdvancingWithdrawalDCType iType, ShipRecoveryType iRecoveryType)
		{
			RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(BattleCutManager.GetMapManager(), iRecoveryType));
			switch (iType + 1)
			{
			case AdvancingWithdrawalDCType.Youin:
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						Mst_DataManager.Instance.PurgeUIBattleMaster();
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					});
				}
				break;
			case AdvancingWithdrawalDCType.Megami:
			case AdvancingWithdrawalDCType.AdvancePrimary:
				TrophyUtil.Unlock_At_GoNext();
				BattleCutManager.EndBattleCut(iRecoveryType);
				break;
			case (AdvancingWithdrawalDCType)4:
				BattleCutManager.GetMapManager().ChangeCurrentDeck();
				BattleCutManager.EndBattleCut(iRecoveryType);
				break;
			}
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
			SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
			Object.Destroy(this._prodBCAdvancingWithdrawalDC.get_gameObject());
			Mem.Del<ProdBCAdvancingWithdrawalDC>(ref this._prodBCAdvancingWithdrawalDC);
		}
	}
}
