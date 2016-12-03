using System;
using UnityEngine;

namespace UniRx
{
	public abstract class PresenterBase : PresenterBase<Unit>
	{
		protected sealed override void BeforeInitialize(Unit argument)
		{
			this.BeforeInitialize();
		}

		protected abstract void BeforeInitialize();

		protected override void Initialize(Unit argument)
		{
			this.Initialize();
		}

		protected abstract void Initialize();
	}
	public abstract class PresenterBase<T> : MonoBehaviour, IPresenter
	{
		protected static readonly IPresenter[] EmptyChildren = new IPresenter[0];

		private int childrenCount;

		private int currentCalledCount;

		private bool isInitialized;

		private bool isStartedCapturePhase;

		private Subject<Unit> initializeSubject;

		private IPresenter[] children;

		private IPresenter parent;

		private T argument = default(T);

		IPresenter IPresenter.Parent
		{
			get
			{
				return this.parent;
			}
		}

		protected abstract IPresenter[] Children
		{
			get;
		}

		void IPresenter.StartCapturePhase()
		{
			this.isStartedCapturePhase = true;
			this.BeforeInitialize(this.argument);
			for (int i = 0; i < this.children.Length; i++)
			{
				IPresenter presenter = this.children[i];
				presenter.StartCapturePhase();
			}
			if (this.children.Length == 0)
			{
				this.Initialize(this.argument);
				this.isInitialized = true;
				if (this.initializeSubject != null)
				{
					this.initializeSubject.OnNext(Unit.Default);
					this.initializeSubject.OnCompleted();
				}
				if (this.parent != null)
				{
					this.parent.InitializeCore();
				}
			}
		}

		void IPresenter.RegisterParent(IPresenter parent)
		{
			if (this.parent != null)
			{
				throw new InvalidOperationException("PresenterBase can't register multiple parent. Name:" + base.get_name());
			}
			this.parent = parent;
		}

		void IPresenter.InitializeCore()
		{
			this.currentCalledCount++;
			if (this.childrenCount == this.currentCalledCount)
			{
				this.Initialize(this.argument);
				this.isInitialized = true;
				if (this.initializeSubject != null)
				{
					this.initializeSubject.OnNext(Unit.Default);
					this.initializeSubject.OnCompleted();
				}
				if (this.parent != null)
				{
					this.parent.InitializeCore();
				}
			}
		}

		public IObservable<Unit> InitializeAsObservable()
		{
			if (this.isInitialized)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_31_0;
			if ((arg_31_0 = this.initializeSubject) == null)
			{
				arg_31_0 = (this.initializeSubject = new Subject<Unit>());
			}
			return arg_31_0;
		}

		public void PropagateArgument(T argument)
		{
			this.argument = argument;
		}

		protected abstract void BeforeInitialize(T argument);

		protected abstract void Initialize(T argument);

		protected void Awake()
		{
			this.children = this.Children;
			this.childrenCount = this.children.Length;
			for (int i = 0; i < this.children.Length; i++)
			{
				IPresenter presenter = this.children[i];
				presenter.RegisterParent(this);
				if (!presenter.gameObject.get_activeSelf())
				{
					presenter.gameObject.SetActive(true);
					presenter.gameObject.SetActive(false);
				}
			}
			this.OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		protected void Start()
		{
			if (this.isStartedCapturePhase)
			{
				return;
			}
			IPresenter presenter = this.parent;
			if (presenter == null)
			{
				presenter = this;
			}
			else
			{
				while (presenter.Parent != null)
				{
					presenter = presenter.Parent;
				}
			}
			this.argument = default(T);
			presenter.StartCapturePhase();
		}

		virtual GameObject get_gameObject()
		{
			return base.get_gameObject();
		}
	}
}
