using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation)), RequireComponent(typeof(UIPanel))]
	public class ProdNightRadarDeployment : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			[Range(0f, 1f)]
			public float progress;

			[Header("[Camera Properties]")]
			public Vector3 cameraStartPivot;

			public Vector3 cameraMidPivot;

			public Vector3 cameraEndPivot;

			[Range(0f, 1f)]
			public float cameraMoveProgress;

			[Header("[Fleet Properties]")]
			public Vector3 friendFleetPivot;

			public Vector3 enemyFleetPivot;

			public void Dispose()
			{
				Mem.Del<float>(ref this.progress);
				Mem.Del<Vector3>(ref this.cameraStartPivot);
				Mem.Del<Vector3>(ref this.cameraMidPivot);
				Mem.Del<Vector3>(ref this.cameraEndPivot);
				Mem.Del<float>(ref this.cameraMoveProgress);
				Mem.Del<Vector3>(ref this.friendFleetPivot);
				Mem.Del<Vector3>(ref this.enemyFleetPivot);
			}
		}

		[SerializeField]
		private Transform _prefabProdNightMessage;

		[SerializeField]
		private UITexture _uiOverlay;

		[Header("[Animation Properties]"), SerializeField]
		private ProdNightRadarDeployment.Params _strParams = default(ProdNightRadarDeployment.Params);

		private Bezier _clsCameraMoveBezier;

		private Vector3 _vCameraPos;

		private Animation _anim;

		private UIPanel _uiPanel;

		private Subject<bool> _subMessage = new Subject<bool>();

		private Transform _traFriendFleet;

		private Transform _traEnemyFleet;

		private ProdNightMessage _prodNightMessage;

		private bool _isRadarDeployment;

		private Animation animation
		{
			get
			{
				return this.GetComponentThis(ref this._anim);
			}
		}

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdNightRadarDeployment Instantiate(ProdNightRadarDeployment prefab, Transform parent)
		{
			ProdNightRadarDeployment prodNightRadarDeployment = Object.Instantiate<ProdNightRadarDeployment>(prefab);
			prodNightRadarDeployment.get_transform().set_parent(parent);
			prodNightRadarDeployment.get_transform().localScaleOne();
			prodNightRadarDeployment.get_transform().localPositionZero();
			prodNightRadarDeployment.Init();
			return prodNightRadarDeployment;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabProdNightMessage);
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.DelIDisposableSafe<ProdNightRadarDeployment.Params>(ref this._strParams);
			Mem.Del<Bezier>(ref this._clsCameraMoveBezier);
			Mem.Del<Vector3>(ref this._vCameraPos);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<UIPanel>(ref this._uiPanel);
			this._subMessage.OnCompleted();
			Mem.Del<Subject<bool>>(ref this._subMessage);
			Mem.Del<Transform>(ref this._traFriendFleet);
			Mem.Del<Transform>(ref this._traEnemyFleet);
			Mem.DelComponentSafe<ProdNightMessage>(ref this._prodNightMessage);
			Mem.Del<bool>(ref this._isRadarDeployment);
		}

		private void LateUpdate()
		{
			if (this.animation.get_isPlaying())
			{
				this._traEnemyFleet.set_position(this._strParams.enemyFleetPivot);
				this._traFriendFleet.set_position(this._strParams.friendFleetPivot);
				this._vCameraPos = Vector3.Lerp(this._strParams.cameraStartPivot, this._strParams.cameraEndPivot, Mathe.MinMax2F01(this._strParams.cameraMoveProgress));
				BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
				battleFieldCamera.get_transform().set_position(this._vCameraPos);
			}
		}

		private bool Init()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			this._prodNightMessage = ProdNightMessage.Instantiate(this._prefabProdNightMessage.GetComponent<ProdNightMessage>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
			this._prodNightMessage.panel.widgetsAreStatic = true;
			this._subMessage.Take(1).Subscribe(delegate(bool _)
			{
				this._prodNightMessage.panel.widgetsAreStatic = false;
				this._prodNightMessage.Play(null);
			}).AddTo(base.get_gameObject());
			this.panel.depth = this._prodNightMessage.panel.depth - 1;
			this._traFriendFleet = battleField.dicFleetAnchor.get_Item(FleetType.Friend);
			this._traEnemyFleet = battleField.dicFleetAnchor.get_Item(FleetType.Enemy);
			this._clsCameraMoveBezier = new Bezier(Bezier.BezierType.Quadratic, this._strParams.cameraStartPivot, this._strParams.cameraEndPivot, this._strParams.cameraMidPivot, Vector3.get_zero());
			this.RadarDeployment();
			return true;
		}

		private void RadarDeployment()
		{
			if (this._isRadarDeployment)
			{
				return;
			}
			this._isRadarDeployment = true;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(true);
		}

		public void RadarObjectConvergence()
		{
			BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0).glowEffect.set_enabled(false);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(false);
		}

		public IObservable<bool> Play()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetStandingPosition(StandingPositionType.CommandBuffer);
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SetVerticalSplitCameras(false);
			battleCameras.fieldDimCamera.maskAlpha = 0f;
			battleCameras.SwitchMainCamera(FleetType.Friend);
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.FixChasing);
			battleFieldCamera.pointOfGaze = Vector3.Lerp(BattleTaskManager.GetBattleField().dicFleetAnchor.get_Item(FleetType.Friend).get_position(), BattleTaskManager.GetBattleField().dicFleetAnchor.get_Item(FleetType.Enemy).get_position(), 0.8f);
			battleFieldCamera.vignetting.set_enabled(true);
			battleFieldCamera.glowEffect.set_enabled(true);
			battleShips.SetBollboardTarget(null);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 0f;
			}
			Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit _)
			{
				this.CalcInitLineRotation();
				this.PlayLineAnimation();
			});
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.AnimationObserver(observer));
		}

		private void CalcInitLineRotation()
		{
			BattleField field = BattleTaskManager.GetBattleField();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor.get_Item(FleetType.Enemy));
			});
			battleShips.bufferShipCirlce.get_Item(1).ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor.get_Item(FleetType.Friend));
			});
		}

		private void PlayLineAnimation()
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
			battleShips.bufferShipCirlce.get_Item(1).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
		}

		[DebuggerHidden]
		private IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdNightRadarDeployment.<AnimationObserver>c__IteratorE5 <AnimationObserver>c__IteratorE = new ProdNightRadarDeployment.<AnimationObserver>c__IteratorE5();
			<AnimationObserver>c__IteratorE.observer = observer;
			<AnimationObserver>c__IteratorE.<$>observer = observer;
			<AnimationObserver>c__IteratorE.<>f__this = this;
			return <AnimationObserver>c__IteratorE;
		}

		private void PlayNightMessage()
		{
			this._subMessage.OnNext(true);
		}
	}
}
