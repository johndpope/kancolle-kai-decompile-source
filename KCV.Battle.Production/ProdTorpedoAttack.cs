using KCV.Battle.Utils;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoAttack : BaseProdAttackShelling
	{
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
				this.PlayShipAnimation(this._listBattleShips.get_Item(0), UIBattleShip.AnimationName.ProdShellingNormalAttack, 0.4f);
				fieldCam.motionBlur.set_enabled(false);
				fieldCam.get_transform().LTMove(camTargetPos.get_Item(1), BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME.get_Item(1)).setEase(BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE.get_Item(1)).setOnComplete(delegate
				{
					this.ChkAttackCntForNextPhase();
				});
			});
			return false;
		}

		protected override bool InitDefenderFocus(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras.get_Item(0);
			Vector3 calcDefenderCamStartPos = this.CalcDefenderCamStartPos;
			this.SetFieldCamera(false, calcDefenderCamStartPos, this._listBattleShips.get_Item(1).spPointOfGaze);
			List<Vector3> camTargetPos = this.CalcCloseUpCamPos(fieldCam.get_transform().get_position(), this.CalcCamTargetPos(false, false), base.isProtect);
			base.alterWaveDirection = this._listBattleShips.get_Item(0).fleetType;
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

		protected override void PlayDefenderCritical(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.CriticalDamage, true);
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			splashMiss.get_transform().set_parent(base.particleParent);
			splashMiss.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			splashMiss.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 0.9f));
			splashMiss.get_transform().positionY(0f);
			splashMiss.SetActive(true);
			splashMiss.Play();
			this.PlayHpGaugeDamage(ship, base.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
			base.PlayDamageVoice(ship, this._clsHougekiModel.Defender.DamageEventAfter);
		}

		protected override void PlayDefenderNormal(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			splashMiss.get_transform().set_parent(base.particleParent);
			splashMiss.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			splashMiss.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 0.9f));
			splashMiss.get_transform().positionY(0f);
			splashMiss.SetActive(true);
			splashMiss.Play();
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.NomalDamage, false);
			base.PlayDamageVoice(ship, this._clsHougekiModel.Defender.DamageEventAfter);
			this.PlayHpGaugeDamage(ship, base.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected override void PlayDefenderGard(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			splashMiss.get_transform().set_parent(base.particleParent);
			splashMiss.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			splashMiss.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 0.9f));
			splashMiss.get_transform().positionY(0f);
			splashMiss.SetActive(true);
			splashMiss.Play();
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.Gard, true);
			base.PlayDamageVoice(ship, this._clsHougekiModel.Defender.DamageEventAfter);
			this.PlayHpGaugeDamage(ship, base.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected override void PlayDefenderMiss(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			KCV.Battle.Utils.SoundUtils.PlayDamageSE(HitState.Miss, true);
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			splashMiss.get_transform().set_parent(base.particleParent);
			splashMiss.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			splashMiss.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 5f));
			splashMiss.get_transform().positionY(0f);
			splashMiss.SetActive(true);
			splashMiss.Play();
			this.PlayHpGaugeDamage(ship, base.hitState);
		}
	}
}
