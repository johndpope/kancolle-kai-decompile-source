using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAdvancingWithdrawal : BaseBattleTask
	{
		private ProdAdvancingWithDrawalSelect _prodAdvancingWithDrawalSelect;

		private AsyncOperation _async;

		private Action<ShipRecoveryType> _actOnGotoSortieMap;

		public TaskBattleAdvancingWithdrawal(Action<ShipRecoveryType> onGotoSortieMap)
		{
			this._actOnGotoSortieMap = onGotoSortieMap;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del<ProdAdvancingWithDrawalSelect>(ref this._prodAdvancingWithDrawalSelect);
			Mem.Del<Action<ShipRecoveryType>>(ref this._actOnGotoSortieMap);
			base.Dispose(isDisposing);
		}

		protected override bool Init()
		{
			if (BattleTaskManager.GetBattleManager().IsPractice)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					base.ImmediateTermination();
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
			else
			{
				BattleTaskManager.GetPrefabFile().battleShutter.ReqMode(BaseShutter.ShutterMode.Close, delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
					{
						BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
						cutInEffectCamera.blur.set_enabled(false);
						this._prodAdvancingWithDrawalSelect = ProdAdvancingWithDrawalSelect.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdAdvancingWithDrawalSelect.GetComponent<ProdAdvancingWithDrawalSelect>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), BattleTaskManager.GetRootType());
						this._prodAdvancingWithDrawalSelect.Play(new DelDecideHexButtonEx(this.DecideAdvancinsWithDrawalBtn));
					});
				});
			}
			return true;
		}

		protected override bool UnInit()
		{
			this._prodAdvancingWithDrawalSelect = null;
			return true;
		}

		protected override bool Update()
		{
			if (this._prodAdvancingWithDrawalSelect != null)
			{
				return this._prodAdvancingWithDrawalSelect.Run();
			}
			return this.ChkChangePhase(BattlePhase.AdvancingWithdrawal);
		}

		private void DecideAdvancinsWithDrawalBtn(UIHexButtonEx btn)
		{
			if (btn.index == 2)
			{
				MapManager mapManager = SortieBattleTaskManager.GetMapManager();
				mapManager.ChangeCurrentDeck();
			}
			if (BattleTaskManager.IsSortieBattle() && SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawal(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					if (btn.index == 0)
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					}
					else
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = true;
						Dlg.Call<ShipRecoveryType>(ref this._actOnGotoSortieMap, ShipRecoveryType.None);
					}
				});
			}
		}
	}
}
