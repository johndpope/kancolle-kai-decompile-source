using System;
using System.Collections.Generic;
using System.Globalization;

namespace UniRx
{
	[Serializable]
	public class TimeInterval<T> : IEquatable<TimeInterval<T>>
	{
		private readonly TimeSpan _interval;

		private readonly T _value;

		public T Value
		{
			get
			{
				return this._value;
			}
		}

		public TimeSpan Interval
		{
			get
			{
				return this._interval;
			}
		}

		public TimeInterval(T value, TimeSpan interval)
		{
			this._interval = interval;
			this._value = value;
		}

		public bool Equals(TimeInterval<T> other)
		{
			return other.Interval.Equals(this.Interval) && EqualityComparer<T>.get_Default().Equals(this.Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TimeInterval<T>))
			{
				return false;
			}
			TimeInterval<T> other = (TimeInterval<T>)obj;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			int arg_2E_0;
			if (this.Value == null)
			{
				arg_2E_0 = 1963;
			}
			else
			{
				T value = this.Value;
				arg_2E_0 = value.GetHashCode();
			}
			int num = arg_2E_0;
			return this.Interval.GetHashCode() ^ num;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.get_CurrentCulture(), "{0}@{1}", new object[]
			{
				this.Value,
				this.Interval
			});
		}

		public static bool operator ==(TimeInterval<T> first, TimeInterval<T> second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(TimeInterval<T> first, TimeInterval<T> second)
		{
			return !first.Equals(second);
		}
	}
}
