using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdObservedShellingAttack : BaseProdAttackShelling
	{
		private ProdObservedShellingCutIn _prodObservedShellingCutIn;

		public ProdObservedShellingAttack()
		{
			this._prodObservedShellingCutIn = ProdObservedShellingCutIn.Instantiate(PrefabFile.Load<ProdObservedShellingCutIn>(PrefabFileInfos.BattleProdObservedShellingCutIn), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
		}

		protected override void OnDispose()
		{
			if (this._prodObservedShellingCutIn != null)
			{
				Object.Destroy(this._prodObservedShellingCutIn.get_gameObject());
			}
			this._prodObservedShellingCutIn = null;
			base.OnDispose();
		}

		protected override bool InitAttackerFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras.get_Item(0);
			List<Vector3> camTargetPos = this.CalcCloseUpCamPos(fieldCam.get_transform().get_position(), this.CalcCamTargetPos(true, true), false);
			base.alterWaveDirection = this._listBattleShips.get_Item(0).fleetType;
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_936);
			this.GraAddDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0));
			fieldCam.get_transform().LTMove(camTargetPos.get_Item(0), BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0)).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE.get_Item(0)).setOnComplete(delegate
			{
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Register(delegate
				{
					this._prodObservedShellingCutIn.get_transform().localScaleZero();
				});
				this._prodObservedShellingCutIn.SetObservedShelling(this._clsHougekiModel);
				this._prodObservedShellingCutIn.Play(delegate
				{
					this.ChkAttackCntForNextPhase();
				});
				fieldCam.get_transform().LTMove(camTargetPos.get_Item(1), BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(1)).setEase(LeanTweenType.linear);
			});
			return false;
		}

		protected override void OnCameraRotateStart()
		{
			this.PostProcessCutIn();
		}

		protected override bool InitDefenderFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras.get_Item(0);
			BattleFieldDimCamera fieldDimCamera = battleCameras.fieldDimCamera;
			fieldDimCamera.SetMaskPlaneAlpha(0f);
			Vector3 calcDefenderCamStartPos = this.CalcDefenderCamStartPos;
			this.SetFieldCamera(false, calcDefenderCamStartPos, this._listBattleShips.get_Item(1).spPointOfGaze);
			List<Vector3> camTargetPos = this.CalcCloseUpCamPos(fieldCam.get_transform().get_position(), this.CalcCamTargetPos(false, false), base.isProtect);
			base.alterWaveDirection = this._listBattleShips.get_Item(1).fleetType;
			this.GraAddDimCameraMaskAlpha((!this._isSkipAttack) ? BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0) : 0f);
			fieldCam.get_transform().LTMove(camTargetPos.get_Item(0), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(0)).setOnStart(delegate
			{
				this.PostProcessCutIn();
			}).setOnComplete(delegate
			{
				fieldCam.motionBlur.set_enabled(false);
				if (this.isProtect)
				{
					this.PlayProtectDefender(camTargetPos);
				}
				else
				{
					this.PlayDefenderEffect(this._listBattleShips.get_Item(1), this._listBattleShips.get_Item(1).pointOfGaze, fieldCam, 0.5f);
					this.ChkDamagedStateFmAnticipating(camTargetPos.get_Item(1));
				}
			});
			return false;
		}

		private void PostProcessCutIn()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			if (observerAction.Count != 0)
			{
				observerAction.Execute();
			}
		}
	}
}
