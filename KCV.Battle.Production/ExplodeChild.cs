using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ExplodeChild : MonoBehaviour
	{
		private struct Fragment : IDisposable
		{
			public Transform transform;

			public Vector3 initialPosition;

			public Quaternion initialRotation;

			public Vector3 velocity;

			public Quaternion angularVelocity;

			public void Dispose()
			{
				Mem.DelMeshSafe(ref this.transform);
				Mem.Del<Transform>(ref this.transform);
				Mem.Del<Vector3>(ref this.initialPosition);
				Mem.Del<Quaternion>(ref this.initialRotation);
				Mem.Del<Vector3>(ref this.velocity);
				Mem.Del<Quaternion>(ref this.angularVelocity);
			}
		}

		[SerializeField, Tooltip("破片回転速度")]
		private float _anglarSpeed = 18f;

		[SerializeField, Tooltip("爆発初速")]
		private float _explodePower = 4.5f;

		[SerializeField, Tooltip("初速力方向ランダム度数")]
		private float _explodeDirectionRandomize = 45f;

		[SerializeField, Tooltip("重力加速度")]
		private float _gravity = 15.5f;

		[SerializeField, Tooltip("上方向へのふっとび補正")]
		private float _upPowerPlus = 1.3f;

		[SerializeField, Tooltip("デバッグ用にループ起動するか")]
		private bool _startLoop;

		private List<ExplodeChild.Fragment> _fragments = new List<ExplodeChild.Fragment>();

		private bool _isAnimating;

		public float anglarSpeed
		{
			get
			{
				return this._anglarSpeed;
			}
			set
			{
				if (value != this._anglarSpeed)
				{
					this._anglarSpeed = value;
				}
			}
		}

		public float explodePower
		{
			get
			{
				return this._explodePower;
			}
			set
			{
				if (value != this._explodePower)
				{
					this._explodePower = value;
				}
			}
		}

		public float explodeDirectionRandomize
		{
			get
			{
				return this._explodeDirectionRandomize;
			}
			set
			{
				if (value != this._explodeDirectionRandomize)
				{
					this._explodeDirectionRandomize = value;
				}
			}
		}

		public float gravity
		{
			get
			{
				return this._gravity;
			}
			set
			{
				if (value != this._gravity)
				{
					this._gravity = value;
				}
			}
		}

		public float upPowerPlus
		{
			get
			{
				return this._upPowerPlus;
			}
			set
			{
				if (value != this._upPowerPlus)
				{
					this._upPowerPlus = value;
				}
			}
		}

		public bool isStartLoop
		{
			get
			{
				return this._startLoop;
			}
			set
			{
				if (value != this._startLoop)
				{
					this._startLoop = value;
				}
			}
		}

		public bool isPlaying
		{
			get
			{
				return this._isAnimating;
			}
		}

		private void Start()
		{
			this.SetupFragment();
			if (this._startLoop)
			{
				this.PlayAnimation().DelayFrame(1, FrameCountType.Update).Do(delegate(int _)
				{
					this.ResetFragment();
				}).Repeat<int>().Subscribe(delegate(int _)
				{
				});
			}
		}

		private void OnDestroy()
		{
			Mem.Del<float>(ref this._anglarSpeed);
			Mem.Del<float>(ref this._explodePower);
			Mem.Del<float>(ref this._explodeDirectionRandomize);
			Mem.Del<float>(ref this._gravity);
			Mem.Del<float>(ref this._upPowerPlus);
			Mem.Del<bool>(ref this._startLoop);
			if (this._fragments != null)
			{
				this._fragments.ForEach(delegate(ExplodeChild.Fragment x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe<ExplodeChild.Fragment>(ref this._fragments);
			Mem.Del<bool>(ref this._isAnimating);
		}

		private void SetupFragment()
		{
			Transform transform = base.get_transform();
			if (this._fragments.get_Count() != transform.get_childCount())
			{
				this._fragments.set_Capacity(transform.get_childCount());
				for (int i = 0; i < transform.get_childCount(); i++)
				{
					ExplodeChild.Fragment fragment = default(ExplodeChild.Fragment);
					fragment.transform = transform.GetChild(i);
					this._fragments.Add(fragment);
				}
			}
			for (int j = 0; j < this._fragments.get_Count(); j++)
			{
				ExplodeChild.Fragment fragment2 = this._fragments.get_Item(j);
				fragment2.initialPosition = fragment2.transform.get_localPosition();
				fragment2.initialRotation = fragment2.transform.get_localRotation();
				Vector3 vector = Quaternion.AngleAxis(Random.Range(-this._explodeDirectionRandomize, this._explodeDirectionRandomize), Random.get_onUnitSphere()) * fragment2.initialPosition.get_normalized();
				if (vector.y < 0f)
				{
					vector.y *= -1f;
				}
				vector.y *= this._upPowerPlus;
				fragment2.velocity = vector * this._explodePower * Random.Range(0.5f, 1.5f);
				fragment2.angularVelocity = Quaternion.AngleAxis(Random.Range(90f, 180f), Random.get_onUnitSphere());
				this._fragments.set_Item(j, fragment2);
			}
		}

		public void ResetFragment()
		{
			for (int i = 0; i < this._fragments.get_Count(); i++)
			{
				ExplodeChild.Fragment fragment = this._fragments.get_Item(i);
				fragment.transform.set_localPosition(fragment.initialPosition);
				fragment.transform.set_localRotation(fragment.initialRotation);
			}
		}

		public bool LateRun()
		{
			if (!this._isAnimating)
			{
				return false;
			}
			for (int i = 0; i < this._fragments.get_Count(); i++)
			{
				ExplodeChild.Fragment fragment = this._fragments.get_Item(i);
				Transform transform = fragment.transform;
				Vector3 vector = transform.get_transform().get_localPosition();
				Quaternion quaternion = this._fragments.get_Item(i).transform.get_localRotation();
				vector += this._fragments.get_Item(i).velocity * Time.get_deltaTime();
				quaternion *= Quaternion.Slerp(Quaternion.get_identity(), this._fragments.get_Item(i).angularVelocity, Time.get_deltaTime() * this._anglarSpeed);
				transform.set_localPosition(vector);
				transform.set_localRotation(quaternion);
				fragment.velocity += Vector3.get_down() * this._gravity * Time.get_deltaTime();
				this._fragments.set_Item(i, fragment);
			}
			return true;
		}

		public IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			ExplodeChild.<AnimationCoroutine>c__IteratorDF <AnimationCoroutine>c__IteratorDF = new ExplodeChild.<AnimationCoroutine>c__IteratorDF();
			<AnimationCoroutine>c__IteratorDF.observer = observer;
			<AnimationCoroutine>c__IteratorDF.<$>observer = observer;
			<AnimationCoroutine>c__IteratorDF.<>f__this = this;
			return <AnimationCoroutine>c__IteratorDF;
		}
	}
}
