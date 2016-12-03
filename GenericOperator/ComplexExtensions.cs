using System;

namespace GenericOperator
{
	public static class ComplexExtensions
	{
		public static Complex<T> I<T>(this T x) where T : IComparable<T>
		{
			return new Complex<T>(default(T), x);
		}

		public static Complex<T> I<T>(this T x, T y) where T : IComparable<T>
		{
			return new Complex<T>(x, y);
		}
	}
}
