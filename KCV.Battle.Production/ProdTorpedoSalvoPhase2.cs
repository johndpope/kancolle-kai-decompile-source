using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoSalvoPhase2
	{
		private enum StateType
		{
			None,
			Line,
			TorpedoShot,
			TorpedoMove,
			End
		}

		private bool[] _isTC;

		private bool _isPlaying;

		private float _fPhaseTime;

		private float _straightTargetGazeY;

		private Vector3 _straightBegin;

		private Vector3 _straightTarget;

		private Action _actCallback;

		private Dictionary<FleetType, bool> _dicIsAttack;

		private UITexture _centerLine;

		private BattleFieldCamera _fieldCam;

		private PSTorpedoWake _torpedoWake;

		private RaigekiModel _clsTorpedo;

		private TorpedoStraightController _straightController;

		private PSTorpedoWake _onesTorpedoWake;

		private List<PSTorpedoWake> _listTorpedoWake;

		private ProdTorpedoSalvoPhase2.StateType _stateType;

		public Transform transform;

		public ProdTorpedoSalvoPhase2(Transform obj, TorpedoStraightController torpedoStraightController)
		{
			this.transform = obj;
			this._straightController = torpedoStraightController;
		}

		public bool Initialize(RaigekiModel model, PSTorpedoWake psTorpedo, UITexture line)
		{
			this._fieldCam = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._isPlaying = false;
			this._isTC = new bool[2];
			this._clsTorpedo = model;
			this._torpedoWake = psTorpedo;
			this._centerLine = line;
			this._fPhaseTime = 0f;
			this._stateType = ProdTorpedoSalvoPhase2.StateType.None;
			this._dicIsAttack = new Dictionary<FleetType, bool>();
			this._dicIsAttack.Add(FleetType.Friend, false);
			this._dicIsAttack.Add(FleetType.Enemy, false);
			return true;
		}

		public void OnSetDestroy()
		{
			if (this._straightController != null)
			{
				Object.Destroy(this._straightController.get_gameObject());
			}
			Mem.Del<ProdTorpedoSalvoPhase2.StateType>(ref this._stateType);
			Mem.Del<Vector3>(ref this._straightBegin);
			Mem.Del<Vector3>(ref this._straightTarget);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<PSTorpedoWake>(ref this._torpedoWake);
			Mem.Del<TorpedoStraightController>(ref this._straightController);
			Mem.Del<PSTorpedoWake>(ref this._onesTorpedoWake);
			Mem.DelListSafe<PSTorpedoWake>(ref this._listTorpedoWake);
			Mem.DelDictionarySafe<FleetType, bool>(ref this._dicIsAttack);
			this._clsTorpedo = null;
			if (this.transform != null)
			{
				Object.Destroy(this.transform.get_gameObject());
			}
			Mem.Del<Transform>(ref this.transform);
		}

		public bool Run()
		{
			if (this._isPlaying)
			{
				if (this._stateType == ProdTorpedoSalvoPhase2.StateType.TorpedoShot)
				{
					this._fPhaseTime += Time.get_deltaTime();
					if (this._fPhaseTime > 2f)
					{
						this._centerLine.alpha = 0f;
						this._startZoomTorpedo();
						this._setState(ProdTorpedoSalvoPhase2.StateType.TorpedoMove);
						BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(false);
						this._straightController.ReferenceCameraTransform = this._fieldCam.get_transform();
						this._straightController.BeginPivot = this._straightBegin;
						this._straightController.TargetPivot = this._straightTarget;
						this._straightController._params.gazeHeight = this._straightTargetGazeY;
						this._straightController.FlyingFinish2F.Take(1).Subscribe(delegate(int x)
						{
							this._straightController._isAnimating = false;
							this._setState(ProdTorpedoSalvoPhase2.StateType.End);
						});
						this._straightController.PlayAnimation().Subscribe(delegate(int x)
						{
						}).AddTo(this._straightController.get_gameObject());
					}
				}
				else if (this._stateType == ProdTorpedoSalvoPhase2.StateType.End)
				{
					this._onTorpedoAttackFinished();
					this._stateType = ProdTorpedoSalvoPhase2.StateType.None;
					return true;
				}
			}
			return false;
		}

		private void _setState(ProdTorpedoSalvoPhase2.StateType state)
		{
			this._stateType = state;
			this._fPhaseTime = 0f;
		}

		public void Play(Action callBack)
		{
			this._isPlaying = true;
			this._actCallback = callBack;
			this._stateType = ProdTorpedoSalvoPhase2.StateType.TorpedoShot;
			this._fieldCam.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			this._setAttack();
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this._createTorpedoWake(observer)).Subscribe(delegate(bool _)
			{
				this.injectionTorpedo();
			});
		}

		private void _startZoomTorpedo()
		{
			if (this._listTorpedoWake != null)
			{
				using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSTorpedoWake current = enumerator.get_Current();
						current.SetActive(false);
						Object.Destroy(current);
					}
				}
				this._listTorpedoWake.Clear();
			}
			this._listTorpedoWake = null;
			BattleTaskManager.GetBattleCameras().SetVerticalSplitCameras(false);
			if (this._dicIsAttack.get_Item(FleetType.Friend))
			{
				this._setTorpedoWakeZoom(true);
			}
			else if (this._dicIsAttack.get_Item(FleetType.Enemy))
			{
				this._setTorpedoWakeZoom(false);
			}
		}

		private void _setCameraPosition(float _x)
		{
			if (this._dicIsAttack.get_Item(FleetType.Friend))
			{
				this._fieldCam.get_transform().set_position(new Vector3(_x, 20f, 92f));
				this._fieldCam.get_transform().set_rotation(Quaternion.Euler(new Vector3(90f, -180f, 0f)));
			}
			else if (this._dicIsAttack.get_Item(FleetType.Enemy))
			{
				this._fieldCam.get_transform().set_position(new Vector3(_x, 20f, -92f));
				this._fieldCam.get_transform().set_rotation(Quaternion.Euler(new Vector3(90f, 0f, 0f)));
			}
			this._straightBegin = this._fieldCam.get_transform().get_position();
			BattleTaskManager.GetBattleCameras().fieldDimCamera.maskAlpha = 0f;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(true, this._fieldCam.get_transform());
			battleShips.SetBollboardTarget(false, this._fieldCam.get_transform());
		}

		private void _setAttack()
		{
			this._dicIsAttack.set_Item(FleetType.Friend, false);
			this._dicIsAttack.set_Item(FleetType.Enemy, false);
			List<ShipModel_Attacker> attackers = this._clsTorpedo.GetAttackers(true);
			if (attackers != null && attackers.get_Count() > 0)
			{
				this._dicIsAttack.set_Item(FleetType.Friend, true);
			}
			List<ShipModel_Attacker> attackers2 = this._clsTorpedo.GetAttackers(false);
			if (attackers2 != null && attackers2.get_Count() > 0)
			{
				this._dicIsAttack.set_Item(FleetType.Enemy, true);
			}
		}

		[DebuggerHidden]
		private IEnumerator _createTorpedoWake(IObserver<bool> observer)
		{
			ProdTorpedoSalvoPhase2.<_createTorpedoWake>c__IteratorF2 <_createTorpedoWake>c__IteratorF = new ProdTorpedoSalvoPhase2.<_createTorpedoWake>c__IteratorF2();
			<_createTorpedoWake>c__IteratorF.observer = observer;
			<_createTorpedoWake>c__IteratorF.<$>observer = observer;
			<_createTorpedoWake>c__IteratorF.<>f__this = this;
			return <_createTorpedoWake>c__IteratorF;
		}

		private void _createTorpedo(FleetType type, Dictionary<int, UIBattleShip> dicAttackShip, Dictionary<int, UIBattleShip> dicDefenceShip)
		{
			if (!this._dicIsAttack.get_Item(type))
			{
				return;
			}
			for (int i = 0; i < dicAttackShip.get_Count(); i++)
			{
				ShipModel_Battle shipModel = dicAttackShip.get_Item(i).shipModel;
				ShipModel_Battle attackTo = this._clsTorpedo.GetAttackTo(shipModel);
				if (shipModel != null && attackTo != null)
				{
					if (shipModel.ShipType == 4)
					{
						this._isTC[(int)type] = true;
					}
					if (this._listTorpedoWake != null)
					{
						this._listTorpedoWake.Add(this._instantiateTorpedo(dicAttackShip.get_Item(i).get_transform().get_position(), dicDefenceShip.get_Item(attackTo.Index).get_transform().get_position(), i, 8f, false, false, false));
					}
				}
			}
		}

		private void injectionTorpedo()
		{
			this._straightController.DelayAction(0.5f, delegate
			{
				if (this._listTorpedoWake != null)
				{
					using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PSTorpedoWake current = enumerator.get_Current();
							current.Injection(iTween.EaseType.linear, false, false, null);
						}
					}
					if (this._listTorpedoWake.get_Count() > 0)
					{
						KCV.Utils.SoundUtils.PlaySE((!this._isTC[0] && !this._isTC[1]) ? SEFIleInfos.SE_904 : SEFIleInfos.SE_905);
					}
				}
			});
		}

		private void _setTorpedoWakeZoom(bool isFriend)
		{
			bool flag = this._createTorpedoWakeOnes(isFriend);
			if (flag)
			{
				this._onesTorpedoWake.Injection(iTween.EaseType.linear, false, false, delegate
				{
				});
			}
		}

		private bool _createTorpedoWakeOnes(bool isFriend)
		{
			FleetType fleetType = (!isFriend) ? FleetType.Enemy : FleetType.Friend;
			Dictionary<int, UIBattleShip> dictionary = (!isFriend) ? BattleTaskManager.GetBattleShips().dicEnemyBattleShips : BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dictionary2 = (!isFriend) ? BattleTaskManager.GetBattleShips().dicFriendBattleShips : BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			if (this._dicIsAttack.get_Item(fleetType))
			{
				for (int i = 0; i < dictionary.get_Count(); i++)
				{
					ShipModel_Battle shipModel = dictionary.get_Item(i).shipModel;
					ShipModel_Battle attackTo = this._clsTorpedo.GetAttackTo(shipModel);
					if (shipModel != null && attackTo != null)
					{
						Vector3 position = dictionary.get_Item(i).get_transform().get_position();
						Vector3 injection = (!isFriend) ? new Vector3(dictionary2.get_Item(attackTo.Index).get_transform().get_position().x, position.y, position.z + 1f) : new Vector3(dictionary2.get_Item(attackTo.Index).get_transform().get_position().x, position.y, position.z - 1f);
						RaigekiDamageModel attackDamage = this._clsTorpedo.GetAttackDamage(attackTo.Index, !isFriend);
						int num = (!attackDamage.GetProtectEffect(shipModel.TmpId)) ? attackTo.Index : 0;
						float num2 = (!isFriend) ? -20f : 20f;
						float num3 = (attackDamage.GetHitState(shipModel.TmpId) != BattleHitStatus.Miss) ? 0f : -3f;
						Vector3 position2 = dictionary2.get_Item(num).get_transform().get_position();
						Vector3 target = new Vector3(position2.x + num3, position2.y, position2.z + num2);
						this._onesTorpedoWake = this._instantiateTorpedo(injection, target, i, 2.65f, false, false, true);
						this._setCameraPosition(injection.x);
						this._straightTarget = position2;
						this._straightTargetGazeY = dictionary2.get_Item(num).pointOfGaze.y;
						break;
					}
				}
			}
			return this._onesTorpedoWake != null;
		}

		private PSTorpedoWake _instantiateTorpedo(Vector3 injection, Vector3 target, int index, float _time, bool isDet, bool isMiss, bool isD)
		{
			Vector3 injectionVec = new Vector3(injection.x, injection.y + 1f, injection.z);
			Vector3 target2 = new Vector3(target.x, target.y + 1f, target.z);
			return PSTorpedoWake.Instantiate((!isD) ? this._torpedoWake : ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD), BattleTaskManager.GetBattleField().get_transform(), injectionVec, target2, index, _time, isDet, isMiss);
		}

		public void deleteTorpedoWake()
		{
			if (this._listTorpedoWake == null)
			{
				return;
			}
			using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PSTorpedoWake current = enumerator.get_Current();
					Object.Destroy(current);
				}
			}
			this._listTorpedoWake.Clear();
			this._listTorpedoWake = null;
		}

		private void _onTorpedoAttackFinished()
		{
			this.deleteTorpedoWake();
			if (this._onesTorpedoWake != null)
			{
				Object.Destroy(this._onesTorpedoWake);
			}
			this._onesTorpedoWake = null;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
