using System;
using System.Diagnostics.CodeAnalysis;

namespace UniRx
{
	public sealed class RefCountDisposable : IDisposable, ICancelable
	{
		private sealed class InnerDisposable : IDisposable
		{
			private RefCountDisposable _parent;

			private object parentLock = new object();

			public InnerDisposable(RefCountDisposable parent)
			{
				this._parent = parent;
			}

			public void Dispose()
			{
				object obj = this.parentLock;
				RefCountDisposable parent;
				lock (obj)
				{
					parent = this._parent;
					this._parent = null;
				}
				if (parent != null)
				{
					parent.Release();
				}
			}
		}

		private readonly object _gate = new object();

		private IDisposable _disposable;

		private bool _isPrimaryDisposed;

		private int _count;

		public bool IsDisposed
		{
			get
			{
				return this._disposable == null;
			}
		}

		public RefCountDisposable(IDisposable disposable)
		{
			if (disposable == null)
			{
				throw new ArgumentNullException("disposable");
			}
			this._disposable = disposable;
			this._isPrimaryDisposed = false;
			this._count = 0;
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Backward compat + non-trivial work for a property getter.")]
		public IDisposable GetDisposable()
		{
			object gate = this._gate;
			IDisposable result;
			lock (gate)
			{
				if (this._disposable == null)
				{
					result = Disposable.Empty;
				}
				else
				{
					this._count++;
					result = new RefCountDisposable.InnerDisposable(this);
				}
			}
			return result;
		}

		public void Dispose()
		{
			IDisposable disposable = null;
			object gate = this._gate;
			lock (gate)
			{
				if (this._disposable != null && !this._isPrimaryDisposed)
				{
					this._isPrimaryDisposed = true;
					if (this._count == 0)
					{
						disposable = this._disposable;
						this._disposable = null;
					}
				}
			}
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		private void Release()
		{
			IDisposable disposable = null;
			object gate = this._gate;
			lock (gate)
			{
				if (this._disposable != null)
				{
					this._count--;
					if (this._isPrimaryDisposed && this._count == 0)
					{
						disposable = this._disposable;
						this._disposable = null;
					}
				}
			}
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
