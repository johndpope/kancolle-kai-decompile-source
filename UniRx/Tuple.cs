using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public static class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8));
		}

		public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
		}

		public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}

		public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
		}

		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

		public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}

		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}

		public static Tuple<T1> Create<T1>(T1 item1)
		{
			return new Tuple<T1>(item1);
		}
	}
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		private T4 item4;

		private T5 item5;

		private T6 item6;

		private T7 item7;

		private TRest rest;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}

		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}

		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}

		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}

		public TRest Rest
		{
			get
			{
				return this.rest;
			}
		}

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
			this.rest = rest;
			if (!(rest is ITuple))
			{
				throw new ArgumentException("rest", "The last element of an eight element tuple must be a Tuple.");
			}
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item2, tuple.item2);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item3, tuple.item3);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item4, tuple.item4);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item5, tuple.item5);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item6, tuple.item6);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item7, tuple.item7);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.rest, tuple.rest);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
			return tuple != null && (comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3) && comparer.Equals(this.item4, tuple.item4) && comparer.Equals(this.item5, tuple.item5) && comparer.Equals(this.item6, tuple.item6) && comparer.Equals(this.item7, tuple.item7)) && comparer.Equals(this.rest, tuple.rest);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			int num2 = comparer.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item4));
			num = ((num << 5) + num ^ num2);
			num2 = comparer.GetHashCode(this.item5);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item6));
			int num3 = comparer.GetHashCode(this.item7);
			num3 = ((num3 << 5) + num3 ^ comparer.GetHashCode(this.rest));
			num2 = ((num2 << 5) + num2 ^ num3);
			return (num << 5) + num ^ num2;
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", new object[]
			{
				this.item1,
				this.item2,
				this.item3,
				this.item4,
				this.item5,
				this.item6,
				this.item7,
				((ITuple)((object)this.rest)).ToString()
			});
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public Tuple(T1 item1)
		{
			this.item1 = item1;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1> tuple = other as Tuple<T1>;
			if (tuple != null)
			{
				return comparer.Compare(this.item1, tuple.item1);
			}
			if (other == null)
			{
				return 1;
			}
			throw new ArgumentException("other");
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1> tuple = other as Tuple<T1>;
			return tuple != null && comparer.Equals(this.item1, tuple.item1);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.item1);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}", this.item1);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public Tuple(T1 item1, T2 item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2> tuple = other as Tuple<T1, T2>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.item2, tuple.item2);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2> tuple = other as Tuple<T1, T2>;
			return tuple != null && comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int hashCode = comparer.GetHashCode(this.item1);
			return (hashCode << 5) + hashCode ^ comparer.GetHashCode(this.item2);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}", this.item1, this.item2);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2, T3> tuple = other as Tuple<T1, T2, T3>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item2, tuple.item2);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.item3, tuple.item3);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2, T3> tuple = other as Tuple<T1, T2, T3>;
			return tuple != null && (comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2)) && comparer.Equals(this.item3, tuple.item3);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			return (num << 5) + num ^ comparer.GetHashCode(this.item3);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}", this.item1, this.item2, this.item3);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		private T4 item4;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2, T3, T4> tuple = other as Tuple<T1, T2, T3, T4>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item2, tuple.item2);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item3, tuple.item3);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.item4, tuple.item4);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2, T3, T4> tuple = other as Tuple<T1, T2, T3, T4>;
			return tuple != null && (comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3)) && comparer.Equals(this.item4, tuple.item4);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			int num2 = comparer.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item4));
			return (num << 5) + num ^ num2;
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}", new object[]
			{
				this.item1,
				this.item2,
				this.item3,
				this.item4
			});
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		private T4 item4;

		private T5 item5;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}

		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5> tuple = other as Tuple<T1, T2, T3, T4, T5>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item2, tuple.item2);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item3, tuple.item3);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item4, tuple.item4);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.item5, tuple.item5);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5> tuple = other as Tuple<T1, T2, T3, T4, T5>;
			return tuple != null && (comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3) && comparer.Equals(this.item4, tuple.item4)) && comparer.Equals(this.item5, tuple.item5);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			int num2 = comparer.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item4));
			num = ((num << 5) + num ^ num2);
			return (num << 5) + num ^ comparer.GetHashCode(this.item5);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}", new object[]
			{
				this.item1,
				this.item2,
				this.item3,
				this.item4,
				this.item5
			});
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		private T4 item4;

		private T5 item5;

		private T6 item6;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}

		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}

		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5, T6> tuple = other as Tuple<T1, T2, T3, T4, T5, T6>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item2, tuple.item2);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item3, tuple.item3);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item4, tuple.item4);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item5, tuple.item5);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.item6, tuple.item6);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5, T6> tuple = other as Tuple<T1, T2, T3, T4, T5, T6>;
			return tuple != null && (comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3) && comparer.Equals(this.item4, tuple.item4) && comparer.Equals(this.item5, tuple.item5)) && comparer.Equals(this.item6, tuple.item6);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			int num2 = comparer.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item4));
			num = ((num << 5) + num ^ num2);
			num2 = comparer.GetHashCode(this.item5);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item6));
			return (num << 5) + num ^ num2;
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}", new object[]
			{
				this.item1,
				this.item2,
				this.item3,
				this.item4,
				this.item5,
				this.item6
			});
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralEquatable, IStructuralComparable, ITuple, IComparable
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		private T4 item4;

		private T5 item5;

		private T6 item6;

		private T7 item7;

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}

		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}

		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}

		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.get_Default());
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;
			if (tuple == null)
			{
				if (other == null)
				{
					return 1;
				}
				throw new ArgumentException("other");
			}
			else
			{
				int num = comparer.Compare(this.item1, tuple.item1);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item2, tuple.item2);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item3, tuple.item3);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item4, tuple.item4);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item5, tuple.item5);
				if (num != 0)
				{
					return num;
				}
				num = comparer.Compare(this.item6, tuple.item6);
				if (num != 0)
				{
					return num;
				}
				return comparer.Compare(this.item7, tuple.item7);
			}
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;
			return tuple != null && (comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3) && comparer.Equals(this.item4, tuple.item4) && comparer.Equals(this.item5, tuple.item5) && comparer.Equals(this.item6, tuple.item6)) && comparer.Equals(this.item7, tuple.item7);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			int num2 = comparer.GetHashCode(this.item3);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item4));
			num = ((num << 5) + num ^ num2);
			num2 = comparer.GetHashCode(this.item5);
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item6));
			num2 = ((num2 << 5) + num2 ^ comparer.GetHashCode(this.item7));
			return (num << 5) + num ^ num2;
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", new object[]
			{
				this.item1,
				this.item2,
				this.item3,
				this.item4,
				this.item5,
				this.item6,
				this.item7
			});
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.get_Default());
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.get_Default());
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}
	}
}
