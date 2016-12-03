using System;

namespace UniRx
{
	public class SingleAssignmentDisposable : IDisposable, ICancelable
	{
		private readonly object gate = new object();

		private IDisposable current;

		private bool disposed;

		public bool IsDisposed
		{
			get
			{
				object obj = this.gate;
				bool result;
				lock (obj)
				{
					result = this.disposed;
				}
				return result;
			}
		}

		public IDisposable Disposable
		{
			get
			{
				return this.current;
			}
			set
			{
				IDisposable disposable = null;
				object obj = this.gate;
				bool flag;
				lock (obj)
				{
					flag = this.disposed;
					disposable = this.current;
					if (!flag)
					{
						if (value == null)
						{
							return;
						}
						this.current = value;
					}
				}
				if (flag && value != null)
				{
					value.Dispose();
					return;
				}
				if (disposable != null)
				{
					throw new InvalidOperationException("Disposable is already set");
				}
			}
		}

		public void Dispose()
		{
			IDisposable disposable = null;
			object obj = this.gate;
			lock (obj)
			{
				if (!this.disposed)
				{
					this.disposed = true;
					disposable = this.current;
					this.current = null;
				}
			}
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
