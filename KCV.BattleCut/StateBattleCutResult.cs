using KCV.Battle.Utils;
using System;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutResult : BaseBattleCutState
	{
		private ProdBCResult _prodBCResult;

		public override bool Init(object data)
		{
			this._prodBCResult = ProdBCResult.Instantiate(BattleCutManager.GetPrefabFile().prefabProdResult.GetComponent<ProdBCResult>(), BattleCutManager.GetSharedPlase());
			this._prodBCResult.StartAnimation(new Action(this.OnResultAnimFinished));
			return false;
		}

		public override bool Run(object data)
		{
			this._prodBCResult.Run();
			return base.IsCheckPhase(BattleCutPhase.Result);
		}

		public override bool Terminate(object data)
		{
			Object.Destroy(this._prodBCResult.get_gameObject());
			Mem.Del<ProdBCResult>(ref this._prodBCResult);
			return false;
		}

		private void OnResultAnimFinished()
		{
			if (BattleCutManager.GetBattleManager().IsPractice)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
				SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
				{
					BattleCutManager.EndBattleCut();
				});
			}
			else
			{
				BattleCutManager.ReqPhase(BattleCutPhase.ClearReward);
			}
		}
	}
}
