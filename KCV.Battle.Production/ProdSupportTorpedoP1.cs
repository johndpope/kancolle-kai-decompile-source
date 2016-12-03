using Common.Enum;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportTorpedoP1
	{
		public enum StateType
		{
			None,
			TorpedoShot,
			TorpedoMove,
			End
		}

		public ProdSupportTorpedoP1.StateType _stateType;

		private Action _actCallback;

		private ShienModel_Rai _clsTorpedo;

		private PSTorpedoWake _torpedoWake;

		private BattleFieldCamera _fieldCam;

		private List<PSTorpedoWake> _listTorpedoWake;

		private TorpedoStraightController _straightController;

		private bool _isPlaying;

		private float _fPhaseTime;

		private float _fStraightTargetGazeHeight;

		private Vector3 _vecStraightBegin;

		private Vector3 _vecStraightTarget;

		public Transform transform;

		public ProdSupportTorpedoP1(Transform obj, TorpedoStraightController torpedoStraightController)
		{
			this.transform = obj;
			this._straightController = torpedoStraightController;
		}

		public bool Initialize(ShienModel_Rai model, PSTorpedoWake trupedoWake)
		{
			this._fieldCam = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._fieldCam.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			this._clsTorpedo = model;
			this._torpedoWake = trupedoWake;
			this._fPhaseTime = 0f;
			this._stateType = ProdSupportTorpedoP1.StateType.None;
			return true;
		}

		public void OnSetDestroy()
		{
			if (this._straightController != null)
			{
				Object.Destroy(this._straightController.get_gameObject());
			}
			Mem.Del<TorpedoStraightController>(ref this._straightController);
			Mem.Del<ProdSupportTorpedoP1.StateType>(ref this._stateType);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<ShienModel_Rai>(ref this._clsTorpedo);
			Mem.Del<PSTorpedoWake>(ref this._torpedoWake);
			Mem.Del<BattleFieldCamera>(ref this._fieldCam);
			Mem.Del<Vector3>(ref this._vecStraightBegin);
			Mem.Del<Vector3>(ref this._vecStraightTarget);
			Mem.DelListSafe<PSTorpedoWake>(ref this._listTorpedoWake);
			if (this.transform != null)
			{
				Object.Destroy(this.transform.get_gameObject());
			}
			Mem.Del<Transform>(ref this.transform);
		}

		public bool Update()
		{
			if (this._isPlaying && this._stateType == ProdSupportTorpedoP1.StateType.End)
			{
				this._onTorpedoAttackFinished();
				this._stateType = ProdSupportTorpedoP1.StateType.None;
				return true;
			}
			return false;
		}

		private void _setState(ProdSupportTorpedoP1.StateType state)
		{
			this._stateType = state;
			this._fPhaseTime = 0f;
		}

		public void Play(Action callBack)
		{
			this._isPlaying = true;
			this._actCallback = callBack;
			this._playOnesTorpedo();
			this._setState(ProdSupportTorpedoP1.StateType.TorpedoMove);
			BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(false);
			this._straightController.ReferenceCameraTransform = this._fieldCam.get_transform();
			this._straightController.BeginPivot = this._vecStraightBegin;
			this._straightController.TargetPivot = this._vecStraightTarget;
			this._straightController._params.gazeHeight = this._fStraightTargetGazeHeight;
			this._straightController.FlyingFinish2F.Take(1).Subscribe(delegate(int x)
			{
				this._straightController._isAnimating = false;
				this._setState(ProdSupportTorpedoP1.StateType.End);
			});
			this._straightController.PlayAnimation().Subscribe(delegate(int x)
			{
			}).AddTo(this._straightController.get_gameObject());
		}

		private void _playOnesTorpedo()
		{
			this._fieldCam.motionBlur.set_enabled(true);
			BattleTaskManager.GetBattleCameras().SetVerticalSplitCameras(false);
			this._createTorpedoWakeOnes();
			using (Dictionary<int, UIBattleShip>.ValueCollection.Enumerator enumerator = BattleTaskManager.GetBattleShips().dicEnemyBattleShips.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIBattleShip current = enumerator.get_Current();
					current.billboard.billboardTarget = this._fieldCam.get_transform();
				}
			}
		}

		private void _setTopCamera(float _x)
		{
			this._fieldCam.get_transform().set_position(new Vector3(_x, 20f, 95f));
			this._fieldCam.get_transform().set_rotation(Quaternion.Euler(new Vector3(90f, -180f, 0f)));
			this._vecStraightBegin = this._fieldCam.get_transform().get_position();
		}

		private void _createTorpedoWakeOnes()
		{
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			bool isTC = false;
			for (int i = 0; i < this._clsTorpedo.ShienShips.Length; i++)
			{
				if (this._clsTorpedo.ShienShips[i] != null)
				{
					if (this._clsTorpedo.ShienShips[i].ShipType == 4)
					{
						isTC = true;
					}
				}
			}
			this._listTorpedoWake = new List<PSTorpedoWake>();
			List<ShipModel_Defender> defenders = this._clsTorpedo.GetDefenders(false);
			for (int j = 0; j < defenders.get_Count(); j++)
			{
				DamageModel attackDamage = this._clsTorpedo.GetAttackDamage(defenders.get_Item(j).TmpId);
				Vector3 vecStraightTarget = (!attackDamage.GetProtectEffect()) ? dicEnemyBattleShips.get_Item(j).get_transform().get_position() : dicEnemyBattleShips.get_Item(0).get_transform().get_position();
				Vector3 injection = new Vector3(vecStraightTarget.x, vecStraightTarget.y, dicFriendBattleShips.get_Item(0).get_transform().get_position().z - 1f);
				Vector3 target = (attackDamage.GetHitState() != BattleHitStatus.Miss) ? new Vector3(vecStraightTarget.x, vecStraightTarget.y, vecStraightTarget.z + 20f) : new Vector3(vecStraightTarget.x - 3f, vecStraightTarget.y, vecStraightTarget.z + 20f);
				this._listTorpedoWake.Add(this._createTorpedo(true, injection, target, 2.65f, false, false));
				this._setTopCamera(injection.x);
				this._vecStraightTarget = vecStraightTarget;
				this._fStraightTargetGazeHeight = dicEnemyBattleShips.get_Item(j).pointOfGaze.y;
			}
			using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PSTorpedoWake current = enumerator.get_Current();
					current.Injection(iTween.EaseType.linear, true, isTC, delegate
					{
					});
				}
			}
		}

		private PSTorpedoWake _createTorpedo(bool isOnes, Vector3 injection, Vector3 target, float _time, bool isDet, bool isMiss)
		{
			return PSTorpedoWake.Instantiate((!isOnes) ? this._torpedoWake : ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD), BattleTaskManager.GetBattleField().get_transform(), new Vector3(injection.x, injection.y + 1f, injection.z), new Vector3(target.x, target.y + 1f, target.z), 0, _time, isDet, isMiss);
		}

		public void deleteTorpedoWake()
		{
			if (this._listTorpedoWake != null)
			{
				using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSTorpedoWake current = enumerator.get_Current();
						Object.Destroy(current);
					}
				}
				this._listTorpedoWake.Clear();
			}
			this._listTorpedoWake = null;
		}

		private void _onTorpedoAttackFinished()
		{
			this.deleteTorpedoWake();
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
