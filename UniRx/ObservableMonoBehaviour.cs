using System;
using UnityEngine;

namespace UniRx
{
	public class ObservableMonoBehaviour : TypedMonoBehaviour
	{
		private bool calledAwake;

		private Subject<Unit> awake;

		private Subject<Unit> fixedUpdate;

		private Subject<Unit> lateUpdate;

		private Subject<int> onAnimatorIK;

		private Subject<Unit> onAnimatorMove;

		private Subject<bool> onApplicationFocus;

		private Subject<bool> onApplicationPause;

		private Subject<Unit> onApplicationQuit;

		private Subject<Tuple<float[], int>> onAudioFilterRead;

		private Subject<Unit> onBecameInvisible;

		private Subject<Unit> onBecameVisible;

		private Subject<Collision> onCollisionEnter;

		private Subject<Collision2D> onCollisionEnter2D;

		private Subject<Collision> onCollisionExit;

		private Subject<Collision2D> onCollisionExit2D;

		private Subject<Collision> onCollisionStay;

		private Subject<Collision2D> onCollisionStay2D;

		private Subject<Unit> onConnectedToServer;

		private Subject<ControllerColliderHit> onControllerColliderHit;

		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private Subject<Unit> onDisable;

		private Subject<Unit> onDrawGizmos;

		private Subject<Unit> onDrawGizmosSelected;

		private Subject<Unit> onEnable;

		private Subject<float> onJointBreak;

		private Subject<int> onLevelWasLoaded;

		private Subject<Unit> onMouseDown;

		private Subject<Unit> onMouseDrag;

		private Subject<Unit> onMouseEnter;

		private Subject<Unit> onMouseExit;

		private Subject<Unit> onMouseOver;

		private Subject<Unit> onMouseUp;

		private Subject<Unit> onMouseUpAsButton;

		private Subject<GameObject> onParticleCollision;

		private Subject<Unit> onPostRender;

		private Subject<Unit> onPreCull;

		private Subject<Unit> onPreRender;

		private Subject<Tuple<RenderTexture, RenderTexture>> onRenderImage;

		private Subject<Unit> onRenderObject;

		private Subject<Unit> onServerInitialized;

		private Subject<Collider> onTriggerEnter;

		private Subject<Collider2D> onTriggerEnter2D;

		private Subject<Collider> onTriggerExit;

		private Subject<Collider2D> onTriggerExit2D;

		private Subject<Collider> onTriggerStay;

		private Subject<Collider2D> onTriggerStay2D;

		private Subject<Unit> onValidate;

		private Subject<Unit> onWillRenderObject;

		private Subject<Unit> reset;

		private bool calledStart;

		private Subject<Unit> start;

		private Subject<Unit> update;

		private Subject<NetworkDisconnection> onDisconnectedFromServer;

		private Subject<NetworkConnectionError> onFailedToConnect;

		private Subject<NetworkConnectionError> onFailedToConnectToMasterServer;

		private Subject<MasterServerEvent> onMasterServerEvent;

		private Subject<NetworkMessageInfo> onNetworkInstantiate;

		private Subject<NetworkPlayer> onPlayerConnected;

		private Subject<NetworkPlayer> onPlayerDisconnected;

		private Subject<Tuple<BitStream, NetworkMessageInfo>> onSerializeNetworkView;

		public override void Awake()
		{
			this.calledAwake = true;
			if (this.awake != null)
			{
				this.awake.OnNext(Unit.Default);
				this.awake.OnCompleted();
			}
		}

		public IObservable<Unit> AwakeAsObservable()
		{
			if (this.calledAwake)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_31_0;
			if ((arg_31_0 = this.awake) == null)
			{
				arg_31_0 = (this.awake = new Subject<Unit>());
			}
			return arg_31_0;
		}

		public override void FixedUpdate()
		{
			if (this.fixedUpdate != null)
			{
				this.fixedUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> FixedUpdateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.fixedUpdate) == null)
			{
				arg_1B_0 = (this.fixedUpdate = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void LateUpdate()
		{
			if (this.lateUpdate != null)
			{
				this.lateUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> LateUpdateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.lateUpdate) == null)
			{
				arg_1B_0 = (this.lateUpdate = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnAnimatorIK(int layerIndex)
		{
			if (this.onAnimatorIK != null)
			{
				this.onAnimatorIK.OnNext(layerIndex);
			}
		}

		public IObservable<int> OnAnimatorIKAsObservable()
		{
			Subject<int> arg_1B_0;
			if ((arg_1B_0 = this.onAnimatorIK) == null)
			{
				arg_1B_0 = (this.onAnimatorIK = new Subject<int>());
			}
			return arg_1B_0;
		}

		public override void OnAnimatorMove()
		{
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnAnimatorMoveAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onAnimatorMove) == null)
			{
				arg_1B_0 = (this.onAnimatorMove = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnApplicationFocus(bool focus)
		{
			if (this.onApplicationFocus != null)
			{
				this.onApplicationFocus.OnNext(focus);
			}
		}

		public IObservable<bool> OnApplicationFocusAsObservable()
		{
			Subject<bool> arg_1B_0;
			if ((arg_1B_0 = this.onApplicationFocus) == null)
			{
				arg_1B_0 = (this.onApplicationFocus = new Subject<bool>());
			}
			return arg_1B_0;
		}

		public override void OnApplicationPause(bool pause)
		{
			if (this.onApplicationPause != null)
			{
				this.onApplicationPause.OnNext(pause);
			}
		}

		public IObservable<bool> OnApplicationPauseAsObservable()
		{
			Subject<bool> arg_1B_0;
			if ((arg_1B_0 = this.onApplicationPause) == null)
			{
				arg_1B_0 = (this.onApplicationPause = new Subject<bool>());
			}
			return arg_1B_0;
		}

		public override void OnApplicationQuit()
		{
			if (this.onApplicationQuit != null)
			{
				this.onApplicationQuit.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnApplicationQuitAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onApplicationQuit) == null)
			{
				arg_1B_0 = (this.onApplicationQuit = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnAudioFilterRead(float[] data, int channels)
		{
			if (this.onAudioFilterRead != null)
			{
				this.onAudioFilterRead.OnNext(Tuple.Create<float[], int>(data, channels));
			}
		}

		public IObservable<Tuple<float[], int>> OnAudioFilterReadAsObservable()
		{
			Subject<Tuple<float[], int>> arg_1B_0;
			if ((arg_1B_0 = this.onAudioFilterRead) == null)
			{
				arg_1B_0 = (this.onAudioFilterRead = new Subject<Tuple<float[], int>>());
			}
			return arg_1B_0;
		}

		public override void OnBecameInvisible()
		{
			if (this.onBecameInvisible != null)
			{
				this.onBecameInvisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameInvisibleAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onBecameInvisible) == null)
			{
				arg_1B_0 = (this.onBecameInvisible = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnBecameVisible()
		{
			if (this.onBecameVisible != null)
			{
				this.onBecameVisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameVisibleAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onBecameVisible) == null)
			{
				arg_1B_0 = (this.onBecameVisible = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnCollisionEnter(Collision collision)
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.OnNext(collision);
			}
		}

		public IObservable<Collision> OnCollisionEnterAsObservable()
		{
			Subject<Collision> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionEnter) == null)
			{
				arg_1B_0 = (this.onCollisionEnter = new Subject<Collision>());
			}
			return arg_1B_0;
		}

		public override void OnCollisionEnter2D(Collision2D coll)
		{
			if (this.onCollisionEnter2D != null)
			{
				this.onCollisionEnter2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionEnter2DAsObservable()
		{
			Subject<Collision2D> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionEnter2D) == null)
			{
				arg_1B_0 = (this.onCollisionEnter2D = new Subject<Collision2D>());
			}
			return arg_1B_0;
		}

		public override void OnCollisionExit(Collision collisionInfo)
		{
			if (this.onCollisionExit != null)
			{
				this.onCollisionExit.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionExitAsObservable()
		{
			Subject<Collision> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionExit) == null)
			{
				arg_1B_0 = (this.onCollisionExit = new Subject<Collision>());
			}
			return arg_1B_0;
		}

		public override void OnCollisionExit2D(Collision2D coll)
		{
			if (this.onCollisionExit2D != null)
			{
				this.onCollisionExit2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionExit2DAsObservable()
		{
			Subject<Collision2D> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionExit2D) == null)
			{
				arg_1B_0 = (this.onCollisionExit2D = new Subject<Collision2D>());
			}
			return arg_1B_0;
		}

		public override void OnCollisionStay(Collision collisionInfo)
		{
			if (this.onCollisionStay != null)
			{
				this.onCollisionStay.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionStayAsObservable()
		{
			Subject<Collision> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionStay) == null)
			{
				arg_1B_0 = (this.onCollisionStay = new Subject<Collision>());
			}
			return arg_1B_0;
		}

		public override void OnCollisionStay2D(Collision2D coll)
		{
			if (this.onCollisionStay2D != null)
			{
				this.onCollisionStay2D.OnNext(coll);
			}
		}

		public IObservable<Collision2D> OnCollisionStay2DAsObservable()
		{
			Subject<Collision2D> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionStay2D) == null)
			{
				arg_1B_0 = (this.onCollisionStay2D = new Subject<Collision2D>());
			}
			return arg_1B_0;
		}

		public override void OnConnectedToServer()
		{
			if (this.onConnectedToServer != null)
			{
				this.onConnectedToServer.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnConnectedToServerAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onConnectedToServer) == null)
			{
				arg_1B_0 = (this.onConnectedToServer = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (this.onControllerColliderHit != null)
			{
				this.onControllerColliderHit.OnNext(hit);
			}
		}

		public IObservable<ControllerColliderHit> OnControllerColliderHitAsObservable()
		{
			Subject<ControllerColliderHit> arg_1B_0;
			if ((arg_1B_0 = this.onControllerColliderHit) == null)
			{
				arg_1B_0 = (this.onControllerColliderHit = new Subject<ControllerColliderHit>());
			}
			return arg_1B_0;
		}

		public override void OnDestroy()
		{
			this.calledDestroy = true;
			if (this.onDestroy != null)
			{
				this.onDestroy.OnNext(Unit.Default);
				this.onDestroy.OnCompleted();
			}
		}

		public IObservable<Unit> OnDestroyAsObservable()
		{
			if (this == null)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			if (this.calledDestroy)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_48_0;
			if ((arg_48_0 = this.onDestroy) == null)
			{
				arg_48_0 = (this.onDestroy = new Subject<Unit>());
			}
			return arg_48_0;
		}

		public override void OnDisable()
		{
			if (this.onDisable != null)
			{
				this.onDisable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDisableAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onDisable) == null)
			{
				arg_1B_0 = (this.onDisable = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnDrawGizmos()
		{
			if (this.onDrawGizmos != null)
			{
				this.onDrawGizmos.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDrawGizmosAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onDrawGizmos) == null)
			{
				arg_1B_0 = (this.onDrawGizmos = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnDrawGizmosSelected()
		{
			if (this.onDrawGizmosSelected != null)
			{
				this.onDrawGizmosSelected.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDrawGizmosSelectedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onDrawGizmosSelected) == null)
			{
				arg_1B_0 = (this.onDrawGizmosSelected = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnEnable()
		{
			if (this.onEnable != null)
			{
				this.onEnable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnEnableAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onEnable) == null)
			{
				arg_1B_0 = (this.onEnable = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnJointBreak(float breakForce)
		{
			if (this.onJointBreak != null)
			{
				this.onJointBreak.OnNext(breakForce);
			}
		}

		public IObservable<float> OnJointBreakAsObservable()
		{
			Subject<float> arg_1B_0;
			if ((arg_1B_0 = this.onJointBreak) == null)
			{
				arg_1B_0 = (this.onJointBreak = new Subject<float>());
			}
			return arg_1B_0;
		}

		public override void OnLevelWasLoaded(int level)
		{
			if (this.onLevelWasLoaded != null)
			{
				this.onLevelWasLoaded.OnNext(level);
			}
		}

		public IObservable<int> OnLevelWasLoadedAsObservable()
		{
			Subject<int> arg_1B_0;
			if ((arg_1B_0 = this.onLevelWasLoaded) == null)
			{
				arg_1B_0 = (this.onLevelWasLoaded = new Subject<int>());
			}
			return arg_1B_0;
		}

		public override void OnMouseDown()
		{
			if (this.onMouseDown != null)
			{
				this.onMouseDown.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDownAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseDown) == null)
			{
				arg_1B_0 = (this.onMouseDown = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnMouseDrag()
		{
			if (this.onMouseDrag != null)
			{
				this.onMouseDrag.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDragAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseDrag) == null)
			{
				arg_1B_0 = (this.onMouseDrag = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnMouseEnter()
		{
			if (this.onMouseEnter != null)
			{
				this.onMouseEnter.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseEnterAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseEnter) == null)
			{
				arg_1B_0 = (this.onMouseEnter = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnMouseExit()
		{
			if (this.onMouseExit != null)
			{
				this.onMouseExit.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseExitAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseExit) == null)
			{
				arg_1B_0 = (this.onMouseExit = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnMouseOver()
		{
			if (this.onMouseOver != null)
			{
				this.onMouseOver.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseOverAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseOver) == null)
			{
				arg_1B_0 = (this.onMouseOver = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnMouseUp()
		{
			if (this.onMouseUp != null)
			{
				this.onMouseUp.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseUp) == null)
			{
				arg_1B_0 = (this.onMouseUp = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnMouseUpAsButton()
		{
			if (this.onMouseUpAsButton != null)
			{
				this.onMouseUpAsButton.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsButtonAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseUpAsButton) == null)
			{
				arg_1B_0 = (this.onMouseUpAsButton = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnParticleCollision(GameObject other)
		{
			if (this.onParticleCollision != null)
			{
				this.onParticleCollision.OnNext(other);
			}
		}

		public IObservable<GameObject> OnParticleCollisionAsObservable()
		{
			Subject<GameObject> arg_1B_0;
			if ((arg_1B_0 = this.onParticleCollision) == null)
			{
				arg_1B_0 = (this.onParticleCollision = new Subject<GameObject>());
			}
			return arg_1B_0;
		}

		public override void OnPostRender()
		{
			if (this.onPostRender != null)
			{
				this.onPostRender.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnPostRenderAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onPostRender) == null)
			{
				arg_1B_0 = (this.onPostRender = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnPreCull()
		{
			if (this.onPreCull != null)
			{
				this.onPreCull.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnPreCullAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onPreCull) == null)
			{
				arg_1B_0 = (this.onPreCull = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnPreRender()
		{
			if (this.onPreRender != null)
			{
				this.onPreRender.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnPreRenderAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onPreRender) == null)
			{
				arg_1B_0 = (this.onPreRender = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (this.onRenderImage != null)
			{
				this.onRenderImage.OnNext(Tuple.Create<RenderTexture, RenderTexture>(src, dest));
			}
		}

		public IObservable<Tuple<RenderTexture, RenderTexture>> OnRenderImageAsObservable()
		{
			Subject<Tuple<RenderTexture, RenderTexture>> arg_1B_0;
			if ((arg_1B_0 = this.onRenderImage) == null)
			{
				arg_1B_0 = (this.onRenderImage = new Subject<Tuple<RenderTexture, RenderTexture>>());
			}
			return arg_1B_0;
		}

		public override void OnRenderObject()
		{
			if (this.onRenderObject != null)
			{
				this.onRenderObject.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnRenderObjectAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onRenderObject) == null)
			{
				arg_1B_0 = (this.onRenderObject = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnServerInitialized()
		{
			if (this.onServerInitialized != null)
			{
				this.onServerInitialized.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnServerInitializedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onServerInitialized) == null)
			{
				arg_1B_0 = (this.onServerInitialized = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnTriggerEnter(Collider other)
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerEnterAsObservable()
		{
			Subject<Collider> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerEnter) == null)
			{
				arg_1B_0 = (this.onTriggerEnter = new Subject<Collider>());
			}
			return arg_1B_0;
		}

		public override void OnTriggerEnter2D(Collider2D other)
		{
			if (this.onTriggerEnter2D != null)
			{
				this.onTriggerEnter2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerEnter2DAsObservable()
		{
			Subject<Collider2D> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerEnter2D) == null)
			{
				arg_1B_0 = (this.onTriggerEnter2D = new Subject<Collider2D>());
			}
			return arg_1B_0;
		}

		public override void OnTriggerExit(Collider other)
		{
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerExitAsObservable()
		{
			Subject<Collider> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerExit) == null)
			{
				arg_1B_0 = (this.onTriggerExit = new Subject<Collider>());
			}
			return arg_1B_0;
		}

		public override void OnTriggerExit2D(Collider2D other)
		{
			if (this.onTriggerExit2D != null)
			{
				this.onTriggerExit2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerExit2DAsObservable()
		{
			Subject<Collider2D> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerExit2D) == null)
			{
				arg_1B_0 = (this.onTriggerExit2D = new Subject<Collider2D>());
			}
			return arg_1B_0;
		}

		public override void OnTriggerStay(Collider other)
		{
			if (this.onTriggerStay != null)
			{
				this.onTriggerStay.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerStayAsObservable()
		{
			Subject<Collider> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerStay) == null)
			{
				arg_1B_0 = (this.onTriggerStay = new Subject<Collider>());
			}
			return arg_1B_0;
		}

		public override void OnTriggerStay2D(Collider2D other)
		{
			if (this.onTriggerStay2D != null)
			{
				this.onTriggerStay2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerStay2DAsObservable()
		{
			Subject<Collider2D> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerStay2D) == null)
			{
				arg_1B_0 = (this.onTriggerStay2D = new Subject<Collider2D>());
			}
			return arg_1B_0;
		}

		public override void OnValidate()
		{
			if (this.onValidate != null)
			{
				this.onValidate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnValidateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onValidate) == null)
			{
				arg_1B_0 = (this.onValidate = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnWillRenderObject()
		{
			if (this.onWillRenderObject != null)
			{
				this.onWillRenderObject.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnWillRenderObjectAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onWillRenderObject) == null)
			{
				arg_1B_0 = (this.onWillRenderObject = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void Reset()
		{
			if (this.reset != null)
			{
				this.reset.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> ResetAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.reset) == null)
			{
				arg_1B_0 = (this.reset = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void Start()
		{
			this.calledStart = true;
			if (this.start != null)
			{
				this.start.OnNext(Unit.Default);
				this.start.OnCompleted();
			}
		}

		public IObservable<Unit> StartAsObservable()
		{
			if (this.calledStart)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_31_0;
			if ((arg_31_0 = this.start) == null)
			{
				arg_31_0 = (this.start = new Subject<Unit>());
			}
			return arg_31_0;
		}

		public override void Update()
		{
			if (this.update != null)
			{
				this.update.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> UpdateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.update) == null)
			{
				arg_1B_0 = (this.update = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public override void OnDisconnectedFromServer(NetworkDisconnection info)
		{
			if (this.onDisconnectedFromServer != null)
			{
				this.onDisconnectedFromServer.OnNext(info);
			}
		}

		public IObservable<NetworkDisconnection> OnDisconnectedFromServerAsObservable()
		{
			Subject<NetworkDisconnection> arg_1B_0;
			if ((arg_1B_0 = this.onDisconnectedFromServer) == null)
			{
				arg_1B_0 = (this.onDisconnectedFromServer = new Subject<NetworkDisconnection>());
			}
			return arg_1B_0;
		}

		public override void OnFailedToConnect(NetworkConnectionError error)
		{
			if (this.onFailedToConnect != null)
			{
				this.onFailedToConnect.OnNext(error);
			}
		}

		public IObservable<NetworkConnectionError> OnFailedToConnectAsObservable()
		{
			Subject<NetworkConnectionError> arg_1B_0;
			if ((arg_1B_0 = this.onFailedToConnect) == null)
			{
				arg_1B_0 = (this.onFailedToConnect = new Subject<NetworkConnectionError>());
			}
			return arg_1B_0;
		}

		public override void OnFailedToConnectToMasterServer(NetworkConnectionError info)
		{
			if (this.onFailedToConnectToMasterServer != null)
			{
				this.onFailedToConnectToMasterServer.OnNext(info);
			}
		}

		public IObservable<NetworkConnectionError> OnFailedToConnectToMasterServerAsObservable()
		{
			Subject<NetworkConnectionError> arg_1B_0;
			if ((arg_1B_0 = this.onFailedToConnectToMasterServer) == null)
			{
				arg_1B_0 = (this.onFailedToConnectToMasterServer = new Subject<NetworkConnectionError>());
			}
			return arg_1B_0;
		}

		public override void OnMasterServerEvent(MasterServerEvent msEvent)
		{
			if (this.onMasterServerEvent != null)
			{
				this.onMasterServerEvent.OnNext(msEvent);
			}
		}

		public IObservable<MasterServerEvent> OnMasterServerEventAsObservable()
		{
			Subject<MasterServerEvent> arg_1B_0;
			if ((arg_1B_0 = this.onMasterServerEvent) == null)
			{
				arg_1B_0 = (this.onMasterServerEvent = new Subject<MasterServerEvent>());
			}
			return arg_1B_0;
		}

		public override void OnNetworkInstantiate(NetworkMessageInfo info)
		{
			if (this.onNetworkInstantiate != null)
			{
				this.onNetworkInstantiate.OnNext(info);
			}
		}

		public IObservable<NetworkMessageInfo> OnNetworkInstantiateAsObservable()
		{
			Subject<NetworkMessageInfo> arg_1B_0;
			if ((arg_1B_0 = this.onNetworkInstantiate) == null)
			{
				arg_1B_0 = (this.onNetworkInstantiate = new Subject<NetworkMessageInfo>());
			}
			return arg_1B_0;
		}

		public override void OnPlayerConnected(NetworkPlayer player)
		{
			if (this.onPlayerConnected != null)
			{
				this.onPlayerConnected.OnNext(player);
			}
		}

		public IObservable<NetworkPlayer> OnPlayerConnectedAsObservable()
		{
			Subject<NetworkPlayer> arg_1B_0;
			if ((arg_1B_0 = this.onPlayerConnected) == null)
			{
				arg_1B_0 = (this.onPlayerConnected = new Subject<NetworkPlayer>());
			}
			return arg_1B_0;
		}

		public override void OnPlayerDisconnected(NetworkPlayer player)
		{
			if (this.onPlayerDisconnected != null)
			{
				this.onPlayerDisconnected.OnNext(player);
			}
		}

		public IObservable<NetworkPlayer> OnPlayerDisconnectedAsObservable()
		{
			Subject<NetworkPlayer> arg_1B_0;
			if ((arg_1B_0 = this.onPlayerDisconnected) == null)
			{
				arg_1B_0 = (this.onPlayerDisconnected = new Subject<NetworkPlayer>());
			}
			return arg_1B_0;
		}

		public override void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
		{
			if (this.onSerializeNetworkView != null)
			{
				this.onSerializeNetworkView.OnNext(Tuple.Create<BitStream, NetworkMessageInfo>(stream, info));
			}
		}

		public IObservable<Tuple<BitStream, NetworkMessageInfo>> OnSerializeNetworkViewAsObservable()
		{
			Subject<Tuple<BitStream, NetworkMessageInfo>> arg_1B_0;
			if ((arg_1B_0 = this.onSerializeNetworkView) == null)
			{
				arg_1B_0 = (this.onSerializeNetworkView = new Subject<Tuple<BitStream, NetworkMessageInfo>>());
			}
			return arg_1B_0;
		}
	}
}
