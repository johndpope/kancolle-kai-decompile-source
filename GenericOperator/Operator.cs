using System;
using System.Linq.Expressions;

namespace GenericOperator
{
	public static class Operator<T>
	{
		private static readonly ParameterExpression x = Expression.Parameter(typeof(T), "x");

		private static readonly ParameterExpression y = Expression.Parameter(typeof(T), "y");

		private static readonly ParameterExpression z = Expression.Parameter(typeof(T), "z");

		private static readonly ParameterExpression w = Expression.Parameter(typeof(T), "w");

		public static readonly Func<T, T, T> Add = Operator<T>.Lambda(new Func<ParameterExpression, ParameterExpression, BinaryExpression>(Expression.Add));

		public static readonly Func<T, T, T> Subtract = Operator<T>.Lambda(new Func<ParameterExpression, ParameterExpression, BinaryExpression>(Expression.Subtract));

		public static readonly Func<T, T, T> Multiply = Operator<T>.Lambda(new Func<ParameterExpression, ParameterExpression, BinaryExpression>(Expression.Multiply));

		public static readonly Func<T, T, T> Divide = Operator<T>.Lambda(new Func<ParameterExpression, ParameterExpression, BinaryExpression>(Expression.Divide));

		public static readonly Func<T, T> Plus = Operator<T>.Lambda(new Func<ParameterExpression, UnaryExpression>(Expression.UnaryPlus));

		public static readonly Func<T, T> Negate = Operator<T>.Lambda(new Func<ParameterExpression, UnaryExpression>(Expression.Negate));

		public static readonly Func<T, T, T, T, T> ProductSum = Expression.Lambda<Func<T, T, T, T, T>>(Expression.Add(Expression.Multiply(Operator<T>.x, Operator<T>.y), Expression.Multiply(Operator<T>.z, Operator<T>.w)), new ParameterExpression[]
		{
			Operator<T>.x,
			Operator<T>.y,
			Operator<T>.z,
			Operator<T>.w
		}).Compile();

		public static readonly Func<T, T, T, T, T> ProductDifference = Expression.Lambda<Func<T, T, T, T, T>>(Expression.Subtract(Expression.Multiply(Operator<T>.x, Operator<T>.y), Expression.Multiply(Operator<T>.z, Operator<T>.w)), new ParameterExpression[]
		{
			Operator<T>.x,
			Operator<T>.y,
			Operator<T>.z,
			Operator<T>.w
		}).Compile();

		public static Func<T, T, T> Lambda(Func<ParameterExpression, ParameterExpression, BinaryExpression> op)
		{
			return Expression.Lambda<Func<T, T, T>>(op.Invoke(Operator<T>.x, Operator<T>.y), new ParameterExpression[]
			{
				Operator<T>.x,
				Operator<T>.y
			}).Compile();
		}

		public static Func<T, T> Lambda(Func<ParameterExpression, UnaryExpression> op)
		{
			return Expression.Lambda<Func<T, T>>(op.Invoke(Operator<T>.x), new ParameterExpression[]
			{
				Operator<T>.x
			}).Compile();
		}
	}
}
