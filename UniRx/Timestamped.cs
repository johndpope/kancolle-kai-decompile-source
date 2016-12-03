using System;
using System.Collections.Generic;
using System.Globalization;

namespace UniRx
{
	[Serializable]
	public class Timestamped<T> : IEquatable<Timestamped<T>>
	{
		private readonly DateTimeOffset _timestamp;

		private readonly T _value;

		public T Value
		{
			get
			{
				return this._value;
			}
		}

		public DateTimeOffset Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		public Timestamped(T value, DateTimeOffset timestamp)
		{
			this._timestamp = timestamp;
			this._value = value;
		}

		public bool Equals(Timestamped<T> other)
		{
			return other.Timestamp.Equals(this.Timestamp) && EqualityComparer<T>.get_Default().Equals(this.Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Timestamped<T>))
			{
				return false;
			}
			Timestamped<T> other = (Timestamped<T>)obj;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			int arg_2E_0;
			if (this.Value == null)
			{
				arg_2E_0 = 1979;
			}
			else
			{
				T value = this.Value;
				arg_2E_0 = value.GetHashCode();
			}
			int num = arg_2E_0;
			return this._timestamp.GetHashCode() ^ num;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.get_CurrentCulture(), "{0}@{1}", new object[]
			{
				this.Value,
				this.Timestamp
			});
		}

		public static bool operator ==(Timestamped<T> first, Timestamped<T> second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(Timestamped<T> first, Timestamped<T> second)
		{
			return !first.Equals(second);
		}
	}
	public static class Timestamped
	{
		public static Timestamped<T> Create<T>(T value, DateTimeOffset timestamp)
		{
			return new Timestamped<T>(value, timestamp);
		}
	}
}
