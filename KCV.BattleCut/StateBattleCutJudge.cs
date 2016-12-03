using KCV.Battle.Production;
using KCV.Battle.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutJudge : BaseBattleCutState
	{
		private ProdWinRankJudge _prodWinRunkJudge;

		public override bool Init(object data)
		{
			BattleCutManager.SetTitleText(BattleCutPhase.Battle_End);
			ProdBattleEnd prodBattleEnd = ProdBattleEnd.Instantiate(BattleCutManager.GetPrefabFile().prefabProdBattleEnd.GetComponent<ProdBattleEnd>(), BattleCutManager.GetSharedPlase());
			prodBattleEnd.Play(delegate
			{
				BattleDefines.SOUND_KEEP.BGMVolume = SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM;
				SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume * 0.5f;
				SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume * 0.5f;
				this._prodWinRunkJudge = ProdWinRankJudge.Instantiate(BattleCutManager.GetPrefabFile().prefabProdWinRunkJudge.GetComponent<ProdWinRankJudge>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().GetBattleResult(), true);
				Observable.FromCoroutine(new Func<IEnumerator>(this._prodWinRunkJudge.StartBattleJudge), false).Subscribe(delegate(Unit _)
				{
					BattleCutManager.ReqPhase(BattleCutPhase.Result);
				});
			});
			return false;
		}

		public override bool Run(object data)
		{
			return base.IsCheckPhase(BattleCutPhase.Judge);
		}

		public override bool Terminate(object data)
		{
			Object.Destroy(this._prodWinRunkJudge.get_gameObject());
			this._prodWinRunkJudge = null;
			return false;
		}
	}
}
