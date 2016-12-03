using System;

namespace UniRx
{
	public class BooleanDisposable : IDisposable, ICancelable
	{
		public bool IsDisposed
		{
			get;
			private set;
		}

		public BooleanDisposable()
		{
		}

		internal BooleanDisposable(bool isDisposed)
		{
			this.IsDisposed = isDisposed;
		}

		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				this.IsDisposed = true;
			}
		}
	}
}
