using System;

namespace UniRx
{
	public class SerialDisposable : IDisposable, ICancelable
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
				bool flag = false;
				IDisposable disposable = null;
				object obj = this.gate;
				lock (obj)
				{
					flag = this.disposed;
					if (!flag)
					{
						disposable = this.current;
						this.current = value;
					}
				}
				if (disposable != null)
				{
					disposable.Dispose();
				}
				if (flag && value != null)
				{
					value.Dispose();
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
