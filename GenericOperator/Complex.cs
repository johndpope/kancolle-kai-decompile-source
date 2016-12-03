using System;

namespace GenericOperator
{
	public struct Complex<T> where T : IComparable<T>
	{
		private T re;

		private T im;

		public T Re
		{
			get
			{
				return this.re;
			}
			set
			{
				this.re = value;
			}
		}

		public T Im
		{
			get
			{
				return this.im;
			}
			set
			{
				this.im = value;
			}
		}

		public Complex(T re, T im)
		{
			this.re = re;
			this.im = im;
		}

		private static T Add(T x, T y)
		{
			return Operator<T>.Add.Invoke(x, y);
		}

		private static T Sub(T x, T y)
		{
			return Operator<T>.Subtract.Invoke(x, y);
		}

		private static T Mul(T x, T y)
		{
			return Operator<T>.Multiply.Invoke(x, y);
		}

		private static T Div(T x, T y)
		{
			return Operator<T>.Divide.Invoke(x, y);
		}

		private static T Neg(T x)
		{
			return Operator<T>.Negate.Invoke(x);
		}

		private static T Acc(T x, T y, T z, T w)
		{
			return Operator<T>.ProductSum.Invoke(x, y, z, w);
		}

		private static T Det(T x, T y, T z, T w)
		{
			return Operator<T>.ProductDifference.Invoke(x, y, z, w);
		}

		private static T Norm(T x, T y)
		{
			return Operator<T>.ProductSum.Invoke(x, y, x, y);
		}

		public Complex<T> Inverse()
		{
			T y = Complex<T>.Norm(this.re, this.im);
			T t = Complex<T>.Div(this.re, y);
			T t2 = Complex<T>.Neg(Complex<T>.Div(this.im, y));
			return new Complex<T>(t, t2);
		}

		public override string ToString()
		{
			if (this.im.CompareTo(default(T)) < 0)
			{
				return string.Format("{0} - i{1}", this.re, Complex<T>.Neg(this.im));
			}
			return string.Format("{0} + i{1}", this.re, this.im);
		}

		public static Complex<T>operator +(Complex<T> x, Complex<T> y)
		{
			return new Complex<T>(Complex<T>.Add(x.re, y.re), Complex<T>.Add(x.im, y.im));
		}

		public static Complex<T>operator +(T x, Complex<T> y)
		{
			return new Complex<T>(Complex<T>.Add(x, y.re), y.im);
		}

		public static Complex<T>operator +(Complex<T> x, T y)
		{
			return new Complex<T>(Complex<T>.Add(x.re, y), x.im);
		}

		public static Complex<T>operator -(Complex<T> x)
		{
			return new Complex<T>(Complex<T>.Neg(x.re), Complex<T>.Neg(x.im));
		}

		public static Complex<T>operator -(Complex<T> x, Complex<T> y)
		{
			return new Complex<T>(Complex<T>.Sub(x.re, y.re), Complex<T>.Sub(x.im, y.im));
		}

		public static Complex<T>operator -(T x, Complex<T> y)
		{
			return new Complex<T>(Complex<T>.Sub(x, y.re), y.im);
		}

		public static Complex<T>operator -(Complex<T> x, T y)
		{
			return new Complex<T>(Complex<T>.Sub(x.re, y), x.im);
		}

		public static Complex<T>operator *(Complex<T> x, Complex<T> y)
		{
			return new Complex<T>(Complex<T>.Det(x.re, y.re, x.im, y.im), Complex<T>.Acc(x.re, y.im, x.im, y.re));
		}

		public static Complex<T>operator *(T x, Complex<T> y)
		{
			return new Complex<T>(Complex<T>.Mul(x, y.re), Complex<T>.Mul(x, y.im));
		}

		public static Complex<T>operator *(Complex<T> x, T y)
		{
			return new Complex<T>(Complex<T>.Mul(x.re, y), Complex<T>.Mul(x.im, y));
		}

		public static Complex<T>operator /(Complex<T> x, Complex<T> y)
		{
			return x * y.Inverse();
		}

		public static Complex<T>operator /(T x, Complex<T> y)
		{
			return x * y.Inverse();
		}

		public static Complex<T>operator /(Complex<T> x, T y)
		{
			return new Complex<T>(Complex<T>.Div(x.re, y), Complex<T>.Div(x.im, y));
		}
	}
}
