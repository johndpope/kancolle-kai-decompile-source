using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[ExecuteInEditMode, RequireComponent(typeof(Animation))]
	public class BaseProdBuffer : MonoBehaviour
	{
		protected enum AnimationList
		{
			ProdBufferAntiSubmarine,
			ProdBufferAssault,
			ProdBufferAviation,
			ProdBufferAvoidance,
			ProdBufferClose,
			ProdBufferShelling,
			ProdBufferTorpedoSalvo,
			ProdBufferUnifiedFire,
			ProdBufferWithdrawal
		}

		protected Vector3 _vCameraPos = Vector3.get_zero();

		protected Vector3 _vFleetPos = Vector3.get_zero();

		protected Bezier _clsCameraBezier;

		protected Bezier _clsFleetBezier;

		protected Transform _traFleetAnchorFriend;

		protected Transform _traFleetAnchorEnemy;

		protected Action _actOnPlayBufferEffect;

		protected Action _actOnCalcInitLineRotation;

		protected Action _actOnPlayLineAnimation;

		protected Action _actOnNextFocusShipAnimation;

		[SerializeField]
		protected Animation _anim;

		[Range(0f, 4f), SerializeField]
		protected int _nBufferCnt;

		[Range(0f, 1f), SerializeField]
		protected float _fProgress;

		[Header("[Main Camera Properties]"), SerializeField]
		protected Vector3 _vStartCameraPivot = Vector3.get_zero();

		[SerializeField]
		protected Vector3 _vMidCameraPivot = Vector3.get_zero();

		[SerializeField]
		protected List<Vector3> _listEndCameraPivot;

		[SerializeField]
		protected Vector3 _vCameraPog = Vector3.get_zero();

		[SerializeField]
		protected float _fFov = 30f;

		[Header("[Friend Fleet Properties]"), SerializeField]
		protected Vector3 _vStartFleetPivot = Vector3.get_zero();

		[SerializeField]
		protected Vector3 _vMidFleetPivot = Vector3.get_zero();

		[SerializeField]
		protected Vector3 _vEndFleetPivot = Vector3.get_zero();

		[SerializeField]
		protected float _fFleetRotation;

		[Range(0f, 1f), SerializeField]
		protected float _fFleetMoveProgress;

		[Header("[Enemy Fleet Properties]"), SerializeField]
		protected List<Vector3> _listEnemyFleetPivot;

		protected Animation animation
		{
			get
			{
				return this.GetComponentThis(ref this._anim);
			}
		}

		protected virtual void OnDestroy()
		{
			Mem.Del<Vector3>(ref this._vCameraPos);
			Mem.Del<Vector3>(ref this._vFleetPos);
			Mem.Del<Bezier>(ref this._clsCameraBezier);
			Mem.Del<Bezier>(ref this._clsFleetBezier);
			Mem.Del<Transform>(ref this._traFleetAnchorFriend);
			Mem.Del<Transform>(ref this._traFleetAnchorEnemy);
			Mem.Del<Action>(ref this._actOnPlayBufferEffect);
			Mem.Del<Action>(ref this._actOnCalcInitLineRotation);
			Mem.Del<Action>(ref this._actOnPlayLineAnimation);
			Mem.Del<Action>(ref this._actOnNextFocusShipAnimation);
			Mem.Del<int>(ref this._nBufferCnt);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<float>(ref this._fProgress);
			Mem.Del<Vector3>(ref this._vStartCameraPivot);
			Mem.Del<Vector3>(ref this._vMidCameraPivot);
			Mem.DelListSafe<Vector3>(ref this._listEndCameraPivot);
			Mem.Del<Vector3>(ref this._vCameraPog);
			Mem.Del<float>(ref this._fFov);
			Mem.Del<Vector3>(ref this._vStartFleetPivot);
			Mem.Del<Vector3>(ref this._vMidFleetPivot);
			Mem.Del<Vector3>(ref this._vEndFleetPivot);
			Mem.Del<float>(ref this._fFleetRotation);
			Mem.Del<float>(ref this._fFleetMoveProgress);
			Mem.DelListSafe<Vector3>(ref this._listEnemyFleetPivot);
		}

		protected virtual void LateUpdate()
		{
			if (this.animation.get_isPlaying())
			{
				this._vCameraPos = this._clsCameraBezier.Interpolate(this._fProgress);
				this._vFleetPos = this._clsFleetBezier.Interpolate(this._fProgress);
				if (Application.get_isPlaying())
				{
					BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(1);
					battleFieldCamera.get_transform().set_position(this._vCameraPos);
					battleFieldCamera.fieldOfView = this._fFov;
					this._vCameraPog = Vector3.Lerp(this._vFleetPos, this._listEnemyFleetPivot.get_Item(this._nBufferCnt), 0.5f);
					battleFieldCamera.pointOfGaze = this._vCameraPog;
					this._traFleetAnchorFriend.set_position(this._vFleetPos);
					this._traFleetAnchorEnemy.set_position(this._listEnemyFleetPivot.get_Item(this._nBufferCnt));
					this._traFleetAnchorFriend.set_rotation(Quaternion.Euler(new Vector3(0f, this._fFleetRotation, 0f)));
				}
			}
		}

		public virtual IObservable<bool> Play(Action onPlayBufferEffect, Action onCalcInitLineRotation, Action onPlayLineAnimation, Action onNextFocusShipAnimation, int nBufferCnt)
		{
			this._nBufferCnt = Mathe.MinMax2(nBufferCnt, 0, 4);
			this._clsCameraBezier = new Bezier(Bezier.BezierType.Quadratic, this._vStartCameraPivot, this._listEndCameraPivot.get_Item(this._nBufferCnt), this._vMidCameraPivot, Vector3.get_zero());
			this._clsFleetBezier = new Bezier(Bezier.BezierType.Quadratic, this._vStartFleetPivot, this._vEndFleetPivot, this._vMidFleetPivot, Vector3.get_zero());
			BattleField battleField = BattleTaskManager.GetBattleField();
			this._traFleetAnchorFriend = battleField.dicFleetAnchor.get_Item(FleetType.Friend);
			this._traFleetAnchorEnemy = battleField.dicFleetAnchor.get_Item(FleetType.Enemy);
			this._traFleetAnchorEnemy.get_transform().set_localScale(Vector3.get_one() * 0.8f);
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SetVerticalSplitCameras(false);
			battleCameras.fieldDimCamera.maskAlpha = 0f;
			battleCameras.SwitchMainCamera(FleetType.Enemy);
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(1);
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.FixChasing);
			battleFieldCamera.eyePosition = this._clsCameraBezier.Interpolate(0f);
			battleFieldCamera.pointOfGaze = Vector3.Lerp(this._clsFleetBezier.Interpolate(0f), this._listEnemyFleetPivot.get_Item(this._nBufferCnt), 0.5f);
			battleFieldCamera.get_transform().LookAt(battleFieldCamera.pointOfGaze);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(battleFieldCamera.get_transform());
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 0f;
			}
			this._actOnPlayBufferEffect = onPlayBufferEffect;
			this._actOnNextFocusShipAnimation = onNextFocusShipAnimation;
			Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit x)
			{
				Dlg.Call(ref onCalcInitLineRotation);
				Dlg.Call(ref onPlayLineAnimation);
			});
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.AnimationObserver(observer));
		}

		[DebuggerHidden]
		protected virtual IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			BaseProdBuffer.<AnimationObserver>c__IteratorCB <AnimationObserver>c__IteratorCB = new BaseProdBuffer.<AnimationObserver>c__IteratorCB();
			<AnimationObserver>c__IteratorCB.observer = observer;
			<AnimationObserver>c__IteratorCB.<$>observer = observer;
			return <AnimationObserver>c__IteratorCB;
		}

		protected virtual void PlayBufferEffect()
		{
			Dlg.Call(ref this._actOnPlayBufferEffect);
		}

		protected virtual void PlayNextFocusShipAnimation()
		{
			Dlg.Call(ref this._actOnNextFocusShipAnimation);
		}
	}
}
