using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class SearchLightSceneController : MonoBehaviour
	{
		[Serializable]
		public struct SearchLightFocus
		{
			public bool isEval;

			public float z_progress;

			public float xy_progress;

			public float backprogress;
		}

		private Vector3 _friendFrontDirection = Vector3.get_back();

		public Vector3 _searchLightCameraStartPivot;

		public Vector3 _searchLightPivot;

		public Transform _referenceCameraTransform;

		public float _focusDistance = 5f;

		public float _backLength = 10f;

		private bool _isAnimating;

		public SearchLightSceneController.SearchLightFocus _searchLightFocus;

		public Transform _searchLight;

		private Animation _animation;

		public bool _launchLoop;

		public Vector3 SearchLightCameraStartPivot
		{
			get
			{
				return this._searchLightCameraStartPivot;
			}
			set
			{
				this._searchLightCameraStartPivot = value;
			}
		}

		public Vector3 SearchLightPivot
		{
			get
			{
				return this._searchLightPivot;
			}
			set
			{
				this._searchLightPivot = value;
			}
		}

		public Transform ReferenceCameraTransform
		{
			get
			{
				return this._referenceCameraTransform;
			}
			set
			{
				this._referenceCameraTransform = value;
			}
		}

		private Vector3 SearchLightFocusCameraPoint
		{
			get
			{
				return this._searchLightPivot + this._friendFrontDirection * this._focusDistance;
			}
		}

		private void OnDestroy()
		{
			Mem.Del<Vector3>(ref this._friendFrontDirection);
			Mem.Del<Vector3>(ref this._searchLightCameraStartPivot);
			Mem.Del<Vector3>(ref this._searchLightPivot);
			Mem.Del<Transform>(ref this._referenceCameraTransform);
			Mem.Del<float>(ref this._focusDistance);
			Mem.Del<float>(ref this._backLength);
			Mem.Del<bool>(ref this._isAnimating);
			Mem.Del<SearchLightSceneController.SearchLightFocus>(ref this._searchLightFocus);
			Mem.Del<Transform>(ref this._searchLight);
			Mem.Del<Animation>(ref this._animation);
			Mem.Del<bool>(ref this._launchLoop);
		}

		private void Start()
		{
			this._animation = base.GetComponent<Animation>();
			this._searchLightFocus = default(SearchLightSceneController.SearchLightFocus);
			if (this._launchLoop)
			{
				this.PlayAnimation().DelayFrame(10, FrameCountType.Update).Repeat<int>().Subscribe(delegate(int _)
				{
				});
			}
		}

		private void LateUpdate()
		{
			if (this._searchLightFocus.isEval && this._referenceCameraTransform != null)
			{
				Vector3 searchLightCameraStartPivot = this._searchLightCameraStartPivot;
				Vector3 searchLightFocusCameraPoint = this.SearchLightFocusCameraPoint;
				Vector2 vector = Vector2.Lerp(new Vector2(searchLightCameraStartPivot.x, searchLightCameraStartPivot.y), new Vector2(searchLightFocusCameraPoint.x, searchLightFocusCameraPoint.y), this._searchLightFocus.xy_progress);
				float num = Mathf.Lerp(searchLightCameraStartPivot.z, searchLightFocusCameraPoint.z, this._searchLightFocus.z_progress);
				Vector3 vector2 = new Vector3(vector.x, vector.y, num);
				this._referenceCameraTransform.set_position(vector2 + this._friendFrontDirection * this._backLength * this._searchLightFocus.backprogress);
			}
		}

		private void SearchLightOn()
		{
			this._searchLight.get_transform().set_position(this._searchLightPivot);
			SearchLightController component = this._searchLight.GetComponent<SearchLightController>();
			component.PlayAnimation().Subscribe(delegate(int _)
			{
			});
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			SearchLightSceneController.<AnimationCoroutine>c__IteratorE7 <AnimationCoroutine>c__IteratorE = new SearchLightSceneController.<AnimationCoroutine>c__IteratorE7();
			<AnimationCoroutine>c__IteratorE.observer = observer;
			<AnimationCoroutine>c__IteratorE.<$>observer = observer;
			<AnimationCoroutine>c__IteratorE.<>f__this = this;
			return <AnimationCoroutine>c__IteratorE;
		}

		public IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
		}
	}
}
