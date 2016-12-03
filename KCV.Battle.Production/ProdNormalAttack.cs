using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdNormalAttack : BaseProdAttackShelling
	{
		protected override bool InitAttackerFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras.get_Item(0);
			List<Vector3> camTargetPos = this.CalcCloseUpCamPos(fieldCam.get_transform().get_position(), this.CalcCamTargetPos(true, false), false);
			base.alterWaveDirection = this._listBattleShips.get_Item(0).fleetType;
			this.GraAddDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0));
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_936);
			fieldCam.get_transform().LTMove(camTargetPos.get_Item(0), BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0)).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE.get_Item(0)).setOnComplete(delegate
			{
				this.PlayShellingSlot(this._clsHougekiModel.GetSlotitem(), BaseProdLine.AnimationName.ProdNormalAttackLine, this._listBattleShips.get_Item(0).shipModel.IsFriend(), 0.033f);
				this.PlayShipAnimation(this._listBattleShips.get_Item(0), UIBattleShip.AnimationName.ProdShellingNormalAttack, 0.4f);
				fieldCam.motionBlur.set_enabled(false);
				fieldCam.get_transform().LTMove(camTargetPos.get_Item(1), BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(1)).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE.get_Item(1)).setOnComplete(delegate
				{
					this.ChkAttackCntForNextPhase();
				});
			});
			return false;
		}

		protected override bool InitRotateFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			battleFieldCamera.motionBlur.set_enabled(false);
			this.GraSubDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0));
			this.RotateFocusTowardsTarget2RotateFieldCam(this._listBattleShips.get_Item(1).spPointOfGaze);
			this.RotateFocusTowardTarget2MoveFieldCam(this._listBattleShips.get_Item(1).spPointOfGaze, delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDefenderFocus), new StatementMachine.StatementMachineUpdate(this.UpdateDefenderFocus));
			});
			return false;
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
			if (base.isSkipAttack)
			{
				base.alterWaveDirection = this._listBattleShips.get_Item(1).fleetType;
			}
			this.GraAddDimCameraMaskAlpha((!this._isSkipAttack) ? BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0) : 0f);
			fieldCam.get_transform().LTMove(camTargetPos.get_Item(0), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(0)).setOnComplete(delegate
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

		protected override bool InitDefenderFocusErx(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			battleFieldCamera.motionBlur.set_enabled(true);
			this.GraSubDimCameraMaskAlpha(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(0));
			Vector3 spPointOfGaze = this._listBattleShips.get_Item(1).spPointOfGaze;
			base.RotateFocusTowardTarget2MoveFieldCam_version2(spPointOfGaze, delegate
			{
				this.OnFinished();
			});
			return false;
		}
	}
}
