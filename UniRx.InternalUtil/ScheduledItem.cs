using System;

namespace UniRx.InternalUtil
{
	public class ScheduledItem : IComparable<ScheduledItem>
	{
		private readonly BooleanDisposable _disposable = new BooleanDisposable();

		private readonly TimeSpan _dueTime;

		private readonly Action _action;

		public TimeSpan DueTime
		{
			get
			{
				return this._dueTime;
			}
		}

		public IDisposable Cancellation
		{
			get
			{
				return this._disposable;
			}
		}

		public bool IsCanceled
		{
			get
			{
				return this._disposable.IsDisposed;
			}
		}

		public ScheduledItem(Action action, TimeSpan dueTime)
		{
			this._dueTime = dueTime;
			this._action = action;
		}

		public void Invoke()
		{
			if (!this._disposable.IsDisposed)
			{
				this._action.Invoke();
			}
		}

		public int CompareTo(ScheduledItem other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return 1;
			}
			return this.DueTime.CompareTo(other.DueTime);
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator <(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool operator >(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator >=(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool operator ==(ScheduledItem left, ScheduledItem right)
		{
			return object.ReferenceEquals(left, right);
		}

		public static bool operator !=(ScheduledItem left, ScheduledItem right)
		{
			return !(left == right);
		}
	}
}
