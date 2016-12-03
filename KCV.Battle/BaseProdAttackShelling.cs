using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseProdAttackShelling : IDisposable
	{
		protected int _nCurrentAttackCnt;

		protected bool _isFinished;

		protected bool _isPlaying;

		protected bool _isNextAttack;

		protected bool _isSkipAttack;

		protected Action _actOnFinished;

		protected HougekiModel _clsHougekiModel;

		protected StatementMachine _clsState;

		protected List<UIBattleShip> _listBattleShips;

		protected Dictionary<FleetType, bool> _dicAttackFleet;

		public HougekiModel hougekiModel
		{
			get
			{
				return this._clsHougekiModel;
			}
			private set
			{
				this._clsHougekiModel = value;
			}
		}

		public int currentAttackCnt
		{
			get
			{
				return this._nCurrentAttackCnt;
			}
		}

		public bool isFinished
		{
			get
			{
				return this._isFinished;
			}
		}

		public bool isPlaying
		{
			get
			{
				return this._isPlaying;
			}
		}

		public bool isSkipAttack
		{
			get
			{
				return !(this._listBattleShips.get_Item(0) == null) && this._isSkipAttack;
			}
		}

		protected bool isProtect
		{
			get
			{
				return this._clsHougekiModel != null && this._clsHougekiModel.GetProtectEffect();
			}
		}

		protected Transform particleParent
		{
			get
			{
				BattleField battleField = BattleTaskManager.GetBattleField();
				if (this._listBattleShips.get_Count() == 0)
				{
					return battleField.dicFleetAnchor.get_Item(FleetType.Friend);
				}
				return (!this._listBattleShips.get_Item(1).shipModel.IsFriend()) ? battleField.dicFleetAnchor.get_Item(FleetType.Enemy) : battleField.dicFleetAnchor.get_Item(FleetType.Friend);
			}
		}

		protected HitState hitState
		{
			get
			{
				return BattleUtils.ConvertBattleHitState2HitState(this._clsHougekiModel);
			}
		}

		protected StandingPositionType subjectStandingPosFmAnD
		{
			set
			{
				UIBattleShip arg_21_0 = this._listBattleShips.get_Item(0);
				this._listBattleShips.get_Item(1).standingPositionType = value;
				arg_21_0.standingPositionType = value;
			}
		}

		protected Generics.Layers subjectShipLayerFmAnD
		{
			set
			{
				UIBattleShip arg_21_0 = this._listBattleShips.get_Item(0);
				this._listBattleShips.get_Item(1).layer = value;
				arg_21_0.layer = value;
			}
		}

		protected FleetType alterWaveDirection
		{
			set
			{
				BattleTaskManager.GetBattleField().AlterWaveDirection(value);
			}
		}

		protected virtual Vector3 CalcAttackerCamStartPos
		{
			get
			{
				return new Vector3(this._listBattleShips.get_Item(0).spPointOfGaze.x, this._listBattleShips.get_Item(0).spPointOfGaze.y, 0f);
			}
		}

		protected virtual Vector3 CalcDefenderCamStartPos
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
				return (!this.isSkipAttack) ? new Vector3(battleFieldCamera.get_transform().get_position().x, this._listBattleShips.get_Item(1).spPointOfGaze.y, battleFieldCamera.get_transform().get_position().z) : new Vector3(this._listBattleShips.get_Item(1).spPointOfGaze.x, this._listBattleShips.get_Item(1).spPointOfGaze.y, 0f);
			}
		}

		public BaseProdAttackShelling()
		{
			this._nCurrentAttackCnt = 0;
			this._isFinished = false;
			this._isPlaying = false;
			this._clsHougekiModel = null;
			this._listBattleShips = new List<UIBattleShip>();
			this._dicAttackFleet = new Dictionary<FleetType, bool>();
			this._dicAttackFleet.Add(FleetType.Friend, false);
			this._dicAttackFleet.Add(FleetType.Enemy, false);
			this._clsState = new StatementMachine();
			this._actOnFinished = null;
		}

		public void Dispose()
		{
			Mem.Del<int>(ref this._nCurrentAttackCnt);
			Mem.Del<bool>(ref this._isFinished);
			Mem.Del<bool>(ref this._isPlaying);
			Mem.Del<Action>(ref this._actOnFinished);
			Mem.Del<HougekiModel>(ref this._clsHougekiModel);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			Mem.DelListSafe<UIBattleShip>(ref this._listBattleShips);
			Mem.DelDictionarySafe<FleetType, bool>(ref this._dicAttackFleet);
			this.OnDispose();
		}

		public virtual void Clear()
		{
			this._clsHougekiModel = null;
			this._actOnFinished = null;
			if (this._listBattleShips != null)
			{
				this._listBattleShips.Clear();
			}
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
		}

		public bool Reset()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				BattleTaskManager.GetPrefabFile().circleHPGauge.get_transform().localScaleZero();
			});
			return true;
		}

		protected virtual void OnDispose()
		{
		}

		protected virtual void OnCameraRotateStart()
		{
		}

		public virtual void PlayAttack(HougekiModel model, int nCurrentShellingCnt, bool isNextAttack, bool isSkipAttack, Action callback)
		{
			if (model == null)
			{
				Dlg.Call(ref callback);
			}
			BattleTaskManager.GetTorpedoHpGauges().Hide();
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			this.hougekiModel = model;
			this._actOnFinished = callback;
			this._isNextAttack = isNextAttack;
			this._isSkipAttack = isSkipAttack;
			this.SetDirectionSubjects(this.hougekiModel);
			this._nCurrentAttackCnt = nCurrentShellingCnt;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.ResetFleetAnchorPosition();
			this.CorFleetAnchorDifPosition();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SetVerticalSplitCameras(false);
			BattleShips battleShips2 = BattleTaskManager.GetBattleShips();
			battleShips2.SetBollboardTarget(true, battleCameras.fieldCameras.get_Item(0).get_transform());
			battleShips2.SetBollboardTarget(false, battleCameras.fieldCameras.get_Item(1).get_transform());
			battleShips2.SetTorpedoSalvoWakeAngle(false);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 0f;
			}
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			battleFieldCamera.clearFlags = 1;
			battleFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			battleFieldCamera.eyePosition = this.CalcAttackerCamStartPos;
			battleCameras.SwitchMainCamera(FleetType.Friend);
			BattleFieldCamera battleFieldCamera2 = battleCameras.fieldCameras.get_Item(1);
			battleFieldCamera2.eyePosition = new Vector3(0f, 4f, 0f);
			battleFieldCamera2.eyeRotation = Quaternion.get_identity();
			battleFieldCamera2.fieldOfView = 30f;
			this.SetFieldCamera(true, this.CalcCamPos(true, false), this._listBattleShips.get_Item(0).spPointOfGaze);
			this.SetDimCamera(true, battleFieldCamera.get_transform());
			this.subjectShipLayerFmAnD = Generics.Layers.FocusDim;
			this.subjectStandingPosFmAnD = StandingPositionType.Advance;
			BattleTaskManager.GetPrefabFile().circleHPGauge.get_transform().localScaleZero();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitAttackerFocus), new StatementMachine.StatementMachineUpdate(this.UpdateAttackerFocus));
		}

		protected virtual void SetDirectionSubjects(HougekiModel model)
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			this._listBattleShips.Add((!model.Attacker.IsFriend()) ? battleShips.dicEnemyBattleShips.get_Item(model.Attacker.Index) : battleShips.dicFriendBattleShips.get_Item(model.Attacker.Index));
			if (this.isProtect)
			{
				this._listBattleShips.Add((!model.Defender.IsFriend()) ? battleShips.flagShipEnemy : battleShips.flagShipFriend);
				this._listBattleShips.Add((!model.Defender.IsFriend()) ? battleShips.dicEnemyBattleShips.get_Item(model.Defender.Index) : battleShips.dicFriendBattleShips.get_Item(model.Defender.Index));
			}
			else
			{
				this._listBattleShips.Add((!model.Defender.IsFriend()) ? battleShips.dicEnemyBattleShips.get_Item(model.Defender.Index) : battleShips.dicFriendBattleShips.get_Item(model.Defender.Index));
			}
			this.subjectStandingPosFmAnD = StandingPositionType.Advance;
		}

		protected void CorFleetAnchorDifPosition()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			Vector3 to = (!this._listBattleShips.get_Item(0).shipModel.IsFriend()) ? this._listBattleShips.get_Item(1).get_transform().get_position() : this._listBattleShips.get_Item(0).get_transform().get_position();
			Vector3 to2 = this._listBattleShips.get_Item(0).shipModel.IsFriend() ? this._listBattleShips.get_Item(1).get_transform().get_position() : this._listBattleShips.get_Item(0).get_transform().get_position();
			Vector3 vector = Mathe.Direction(Vector3.get_zero(), to);
			Vector3 vector2 = Mathe.Direction(Vector3.get_zero(), to2);
			battleField.dicFleetAnchor.get_Item(FleetType.Friend).get_transform().AddPosX(-vector.x);
			battleField.dicFleetAnchor.get_Item(FleetType.Enemy).get_transform().AddPosX(-vector2.x);
		}

		protected void SetProtecterLayer()
		{
			this._listBattleShips.get_Item(2).layer = Generics.Layers.FocusDim;
		}

		public virtual bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return true;
		}

		protected virtual bool InitAttackerFocus(object data)
		{
			return false;
		}

		protected virtual bool UpdateAttackerFocus(object data)
		{
			return true;
		}

		protected virtual bool InitRotateFocus(object data)
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

		protected virtual bool UpdateRotateFocus(object data)
		{
			return true;
		}

		protected virtual bool InitDefenderFocus(object data)
		{
			return false;
		}

		protected virtual bool UpdateDefenderFocus(object data)
		{
			return true;
		}

		protected virtual bool InitDefenderFocusErx(object data)
		{
			return false;
		}

		protected virtual bool UpdateDefenderFocusErx(object data)
		{
			return true;
		}

		protected virtual Vector3 CalcCamPos(bool isAttacker, bool isPointOfGaze)
		{
			int num = (!isAttacker) ? 1 : 0;
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? this._listBattleShips.get_Item(num).spPointOfGaze : this._listBattleShips.get_Item(num).pointOfGaze, battleFieldCamera.eyePosition) * 50f;
			return (!isPointOfGaze) ? new Vector3(this._listBattleShips.get_Item(num).spPointOfGaze.x + vector.x, this._listBattleShips.get_Item(num).spPointOfGaze.y, this._listBattleShips.get_Item(num).spPointOfGaze.z + vector.z) : new Vector3(this._listBattleShips.get_Item(num).pointOfGaze.x + vector.x, this._listBattleShips.get_Item(num).pointOfGaze.y, this._listBattleShips.get_Item(num).pointOfGaze.z + vector.z);
		}

		protected virtual Vector3 CalcCamTargetPos(bool isAttacker, bool isPointOfGaze)
		{
			int num = (!isAttacker) ? 1 : 0;
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? this._listBattleShips.get_Item(num).spPointOfGaze : this._listBattleShips.get_Item(num).pointOfGaze, battleFieldCamera.eyePosition) * 10f;
			return (!isPointOfGaze) ? new Vector3(this._listBattleShips.get_Item(num).spPointOfGaze.x + vector.x, this._listBattleShips.get_Item(num).spPointOfGaze.y, this._listBattleShips.get_Item(num).spPointOfGaze.z + vector.z) : new Vector3(this._listBattleShips.get_Item(num).pointOfGaze.x + vector.x, this._listBattleShips.get_Item(num).pointOfGaze.y, this._listBattleShips.get_Item(num).pointOfGaze.z + vector.z);
		}

		protected virtual List<Vector3> CalcCloseUpCamPos(Vector3 from, Vector3 to, bool isProtect)
		{
			List<Vector3> result;
			if (!isProtect)
			{
				List<Vector3> list = new List<Vector3>();
				list.Add(Vector3.Lerp(from, to, 0.98f));
				list.Add(to);
				result = list;
			}
			else
			{
				Vector3 vector = Vector3.Lerp(from, to, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(0));
				Vector3 vector2 = Vector3.Lerp(from, to, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(1));
				vector.y = this._listBattleShips.get_Item(2).spPointOfGaze.y;
				vector2.y = this._listBattleShips.get_Item(2).spPointOfGaze.y;
				float num = this._listBattleShips.get_Item(1).get_transform().get_position().x - this._listBattleShips.get_Item(1).spPointOfGaze.x - (this._listBattleShips.get_Item(2).get_transform().get_position().x - this._listBattleShips.get_Item(2).spPointOfGaze.x);
				vector.x += num;
				vector2.x += num;
				List<Vector3> list = new List<Vector3>();
				list.Add(Vector3.Lerp(from, to, 0.98f));
				list.Add(to);
				list.Add(vector);
				list.Add(vector2);
				result = list;
			}
			return result;
		}

		protected virtual Vector3 CalcProtecterPos(Vector3 close4)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			Vector3 vector = Vector3.Lerp(this._listBattleShips.get_Item(1).spPointOfGaze, close4, 0.58f);
			Vector3 position = this._listBattleShips.get_Item(1).get_transform().get_position();
			position.y = battleField.seaLevelPos.y;
			position.z = vector.z;
			return position;
		}

		protected virtual void SetFieldCamera(bool isAttacker, Vector3 camPos, Vector3 lookPos)
		{
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			if (isAttacker)
			{
				battleFieldCamera.motionBlur.set_enabled(false);
				battleFieldCamera.motionBlur.blurAmount = 0.65f;
				battleFieldCamera.get_transform().set_position(camPos);
				battleFieldCamera.LookAt(lookPos);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
				battleFieldCamera.cullingMask = (Generics.Layers.FocusDim | Generics.Layers.UnRefrectEffects);
				battleFieldCamera.clearFlags = 3;
			}
			else
			{
				battleFieldCamera.motionBlur.set_enabled(false);
				battleFieldCamera.get_transform().set_position(camPos);
				battleFieldCamera.LookAt(lookPos);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			}
		}

		protected virtual void RotateFocusTowardsTarget2RotateFieldCam(Vector3 target)
		{
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
			{
				BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
				float num = (!this._listBattleShips.get_Item(0).shipModel.IsFriend()) ? -180f : 180f;
				Vector3 vector = new Vector3(cam.eyeRotation.x, num, cam.eyeRotation.z);
				cam.get_transform().LTRotateAround(Vector3.get_up(), num, 0.666f).setEase(LeanTweenType.easeInQuad).setOnComplete(delegate
				{
					cam.LookAt(target);
					cam.ReqViewMode(CameraActor.ViewMode.FixChasing);
				});
			});
		}

		protected void RotateFocusTowardTarget2MoveFieldCam_version2(Vector3 target, Action callback)
		{
		}

		protected virtual void RotateFocusTowardTarget2MoveFieldCam(Vector3 target, Action callback)
		{
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			Vector3 vector = Vector3.Lerp(cam.eyePosition, target, 0.2f);
			vector.x = target.x;
			vector.y = target.y;
			cam.get_transform().LTMoveX(vector.x, 0.666f).setOnStart(delegate
			{
				this.OnCameraRotateStart();
			}).setEase(LeanTweenType.easeInQuad).setOnUpdate(delegate(float x)
			{
				cam.get_transform().positionX(x);
			});
			cam.get_transform().LTMoveY(vector.y, 0.666f).setEase(LeanTweenType.easeInQuad).setOnUpdate(delegate(float x)
			{
				cam.get_transform().positionY(x);
			});
			cam.get_transform().LTMoveZ(vector.z, 1.1655f).setEase(LeanTweenType.easeInQuad).setOnUpdate(delegate(float x)
			{
				cam.get_transform().positionZ(x);
			}).setOnComplete(delegate
			{
				Dlg.Call(ref callback);
			});
		}

		protected virtual void SetDimCamera(bool isAttacker, Transform syncTarget)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldDimCamera fieldDimCamera = battleCameras.fieldDimCamera;
			if (isAttacker)
			{
				fieldDimCamera.syncTarget = syncTarget;
				fieldDimCamera.SyncTransform();
				fieldDimCamera.cullingMask = battleCameras.GetDefaultLayers();
				fieldDimCamera.maskAlpha = 0f;
				fieldDimCamera.isCulling = true;
				fieldDimCamera.isSync = true;
			}
		}

		protected virtual void GraAddDimCameraMaskAlpha(float time)
		{
			BattleFieldDimCamera fieldDimCamera = BattleTaskManager.GetBattleCameras().fieldDimCamera;
			if (time == 0f)
			{
				fieldDimCamera.maskAlpha = 0.75f;
			}
			else
			{
				fieldDimCamera.SetMaskPlaneAlpha(0f, 0.75f, time);
			}
		}

		protected virtual void GraSubDimCameraMaskAlpha(float time)
		{
			BattleFieldDimCamera fieldDimCamera = BattleTaskManager.GetBattleCameras().fieldDimCamera;
			fieldDimCamera.SetMaskPlaneAlpha(0.75f, 0f, time);
		}

		protected virtual void PlayShipAnimation(UIBattleShip ship, UIBattleShip.AnimationName iName, float delay)
		{
			Observable.Timer(TimeSpan.FromSeconds((double)delay)).Subscribe(delegate(long _)
			{
				SoundUtils.PlayShellingSE(this._listBattleShips.get_Item(0).shipModel);
				ShipUtils.PlayShellingVoive(ship.shipModel);
				ship.PlayShipAnimation(iName);
			});
		}

		protected virtual void PlayShellingSlot(SlotitemModel_Battle model, BaseProdLine.AnimationName iName, bool isFriend, float delay)
		{
			if (model == null)
			{
				return;
			}
			Observable.Timer(TimeSpan.FromSeconds((double)delay)).Subscribe(delegate(long _)
			{
				ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
				prodShellingSlotLine.SetSlotData(model, isFriend);
				prodShellingSlotLine.Play(iName, isFriend, null);
			});
		}

		protected virtual void PlayShellingSlot(SlotitemModel_Battle[] models, BaseProdLine.AnimationName iName, bool isFriend, float delay)
		{
			if (models == null)
			{
				return;
			}
			Observable.Timer(TimeSpan.FromSeconds((double)delay)).Subscribe(delegate(long _)
			{
				ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
				prodShellingSlotLine.SetSlotData(new List<SlotitemModel_Battle>(models), isFriend);
				prodShellingSlotLine.Play(iName, isFriend, null);
			});
		}

		protected virtual void PlayProtectDefender(List<Vector3> camTargetPos)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.fieldCameras.get_Item(0);
			fieldCam.get_transform().LTMove(camTargetPos.get_Item(1), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(1));
			Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate(long _)
			{
				fieldCam.get_transform().LTCancel();
				this.SetProtecterLayer();
				Vector3 to = this.CalcProtecterPos(camTargetPos.get_Item(3));
				this._listBattleShips.get_Item(2).get_transform().positionZ(to.z);
				this._listBattleShips.get_Item(2).get_transform().LTMove(to, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0) * 1.2f).setEase(LeanTweenType.easeOutSine);
				fieldCam.get_transform().LTMove(camTargetPos.get_Item(2), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(0)).setOnComplete(delegate
				{
					this.PlayDefenderEffect(this._listBattleShips.get_Item(2), this._listBattleShips.get_Item(2).pointOfGaze, fieldCam, 0.5f);
					this.ChkDamagedStateFmAnticipating(camTargetPos.get_Item(3));
				});
			});
		}

		protected void PlayDefenderEffect(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera, float delay)
		{
			Observable.Timer(TimeSpan.FromSeconds((double)delay)).Subscribe(delegate(long _)
			{
				switch (this.hitState)
				{
				case HitState.Miss:
					this.PlayDefenderMiss(ship, defenderPos, fieldCamera);
					break;
				case HitState.Gard:
					this.PlayDefenderGard(ship, defenderPos, fieldCamera);
					break;
				case HitState.NomalDamage:
					this.PlayDefenderNormal(ship, defenderPos, fieldCamera);
					break;
				case HitState.CriticalDamage:
					this.PlayDefenderCritical(ship, defenderPos, fieldCamera);
					break;
				}
			});
		}

		protected virtual void PlayDefenderCritical(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			SoundUtils.PlayDamageSE(HitState.CriticalDamage, false);
			ParticleSystem explosionB = BattleTaskManager.GetParticleFile().explosionB2;
			explosionB.get_transform().set_parent(this.particleParent);
			explosionB.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			explosionB.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 0.9f));
			explosionB.SetActive(true);
			explosionB.Play();
			this.PlayHpGaugeDamage(ship, this.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
			this.PlayDamageVoice(ship, this._clsHougekiModel.Defender.DamageEventAfter);
		}

		protected virtual void PlayDefenderNormal(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem explosionB = BattleTaskManager.GetParticleFile().explosionB2;
			explosionB.get_transform().set_parent(this.particleParent);
			explosionB.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			explosionB.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 0.9f));
			explosionB.SetActive(true);
			explosionB.Play();
			SoundUtils.PlayDamageSE(HitState.NomalDamage, false);
			this.PlayDamageVoice(ship, this._clsHougekiModel.Defender.DamageEventAfter);
			this.PlayHpGaugeDamage(ship, this.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected virtual void PlayDefenderGard(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			ParticleSystem explosionB3WhiteSmoke = BattleTaskManager.GetParticleFile().explosionB3WhiteSmoke;
			explosionB3WhiteSmoke.get_transform().set_parent(this.particleParent);
			explosionB3WhiteSmoke.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			explosionB3WhiteSmoke.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 0.9f));
			explosionB3WhiteSmoke.SetActive(true);
			explosionB3WhiteSmoke.Play();
			SoundUtils.PlayDamageSE(HitState.Gard, false);
			this.PlayDamageVoice(ship, this._clsHougekiModel.Defender.DamageEventAfter);
			this.PlayHpGaugeDamage(ship, this.hitState);
			fieldCamera.cameraShake.ShakeRot(null);
		}

		protected virtual void PlayDefenderMiss(UIBattleShip ship, Vector3 defenderPos, BattleFieldCamera fieldCamera)
		{
			SoundUtils.PlayDamageSE(HitState.Miss, false);
			ParticleSystem splashMiss = BattleTaskManager.GetParticleFile().splashMiss;
			splashMiss.get_transform().set_parent(this.particleParent);
			splashMiss.SetLayer(Generics.Layers.UnRefrectEffects.IntLayer(), true);
			splashMiss.get_transform().set_position(Vector3.Lerp(fieldCamera.get_transform().get_position(), defenderPos, 1f));
			splashMiss.get_transform().positionY(0f);
			splashMiss.get_transform().set_localPosition(new Vector3(splashMiss.get_transform().get_localPosition().x, splashMiss.get_transform().get_localPosition().y, (!ship.shipModel.IsFriend()) ? -15f : 15f));
			splashMiss.SetActive(true);
			splashMiss.Play();
			this.PlayHpGaugeDamage(ship, this.hitState);
		}

		protected void PlayDamageVoice(UIBattleShip ship, DamagedStates iStates)
		{
			if (iStates != DamagedStates.Tyuuha && iStates != DamagedStates.Taiha)
			{
				ShipUtils.PlayMildCaseVoice(this._clsHougekiModel.Defender);
			}
		}

		protected virtual void PlayHpGaugeDamage(UIBattleShip ship, HitState iState)
		{
			BattleHitStatus status = BattleHitStatus.Normal;
			switch (iState)
			{
			case HitState.Miss:
				status = BattleHitStatus.Miss;
				break;
			case HitState.CriticalDamage:
				status = BattleHitStatus.Clitical;
				break;
			}
			if (this._clsHougekiModel != null)
			{
				UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
				circleHPGauge.SetHPGauge(this._clsHougekiModel.Defender.MaxHp, this._clsHougekiModel.Defender.HpBefore, this._clsHougekiModel.Defender.HpAfter, this._clsHougekiModel.GetDamage(), status, this._clsHougekiModel.Defender.IsFriend());
				Vector3 damagePosition = (!this._clsHougekiModel.Defender.IsFriend()) ? new Vector3(280f, -125f, 0f) : new Vector3(-500f, -125f, 0f);
				circleHPGauge.SetDamagePosition(damagePosition);
				circleHPGauge.Play(delegate
				{
				});
			}
		}

		protected virtual void ChkAttackCntForNextPhase()
		{
			if (this.isSkipAttack)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDefenderFocus), new StatementMachine.StatementMachineUpdate(this.UpdateDefenderFocus));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitRotateFocus), new StatementMachine.StatementMachineUpdate(this.UpdateRotateFocus));
			}
		}

		protected virtual void ChkDamagedStateFmAnticipating(Vector3 closeUpPos)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			switch (this._clsHougekiModel.Defender.DamageEventAfter)
			{
			case DamagedStates.None:
			case DamagedStates.Shouha:
				battleFieldCamera.get_transform().LTMove(closeUpPos, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(1)).setOnComplete(delegate
				{
					this.OnFinished();
				});
				break;
			case DamagedStates.Tyuuha:
			case DamagedStates.Taiha:
				battleFieldCamera.get_transform().LTMove(closeUpPos, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(1)).setOnComplete(delegate
				{
					ShellingProdSubject shellingProdSubject2 = (!this.isProtect) ? ShellingProdSubject.Defender : ShellingProdSubject.Protector;
					if (this._listBattleShips.get_Item((int)shellingProdSubject2).shipModel.IsFriend())
					{
						DamagedStates damageEventAfter = this._clsHougekiModel.Defender.DamageEventAfter;
						ProdDamageCutIn.DamageCutInType iType = (damageEventAfter != DamagedStates.Taiha) ? ProdDamageCutIn.DamageCutInType.Moderate : ProdDamageCutIn.DamageCutInType.Heavy;
						ProdDamageCutIn prodDamageCutIn = BattleTaskManager.GetPrefabFile().prodDamageCutIn;
						ProdDamageCutIn arg_76_0 = prodDamageCutIn;
						List<ShipModel_Defender> list = new List<ShipModel_Defender>();
						list.Add(this._clsHougekiModel.Defender);
						arg_76_0.SetShipData(list, iType);
						prodDamageCutIn.Play(iType, delegate
						{
							BattleTaskManager.GetPrefabFile().circleHPGauge.get_transform().set_localScale(Vector3.get_zero());
						}, delegate
						{
							BattleTaskManager.GetBattleShips().UpdateDamageAll(this._clsHougekiModel);
							this.OnFinished();
						});
					}
					else
					{
						this.OnFinished();
					}
				});
				break;
			case DamagedStates.Gekichin:
			case DamagedStates.Youin:
			case DamagedStates.Megami:
			{
				bool isFriend = this._listBattleShips.get_Item(1).shipModel.IsFriend();
				ShellingProdSubject shellingProdSubject = (!this.isProtect) ? ShellingProdSubject.Defender : ShellingProdSubject.Protector;
				this._listBattleShips.get_Item((int)shellingProdSubject).PlayProdSinking(null);
				battleFieldCamera.get_transform().LTMove(closeUpPos, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(2)).setEase(BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE.get_Item(1)).setOnComplete(delegate
				{
					if (!isFriend)
					{
						this.OnFinished();
					}
				});
				if (isFriend)
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
					{
						ProdSinking prodSinking = BattleTaskManager.GetPrefabFile().prodSinking;
						prodSinking.SetSinkingData(this._clsHougekiModel.Defender);
						prodSinking.Play(delegate
						{
							BattleTaskManager.GetPrefabFile().circleHPGauge.get_transform().set_localScale(Vector3.get_zero());
						}, delegate
						{
						}, delegate
						{
							this.OnFinished();
						});
						BattleTaskManager.GetPrefabFile().circleHPGauge.get_transform().set_localScale(Vector3.get_zero());
					});
				}
				break;
			}
			}
		}

		protected virtual void OnFinished()
		{
			if (this._clsHougekiModel.Defender.DamageEventAfter == DamagedStates.Megami || this._clsHougekiModel.Defender.DamageEventAfter == DamagedStates.Youin)
			{
				BattleTaskManager.GetBattleShips().Restored(this._clsHougekiModel.Defender);
			}
			else
			{
				BattleTaskManager.GetBattleShips().UpdateDamageAll(this._clsHougekiModel);
			}
			this.Reset();
			this._isFinished = true;
			Dlg.Call(ref this._actOnFinished);
		}
	}
}
