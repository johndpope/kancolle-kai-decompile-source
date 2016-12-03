using System;

namespace UniRx
{
	[Serializable]
	public class Unit : IEquatable<Unit>
	{
		private static readonly Unit @default = new Unit();

		public static Unit Default
		{
			get
			{
				return Unit.@default;
			}
		}

		public bool Equals(Unit other)
		{
			return true;
		}

		public override bool Equals(object obj)
		{
			return obj is Unit;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			return "()";
		}

		public static bool operator ==(Unit first, Unit second)
		{
			return true;
		}

		public static bool operator !=(Unit first, Unit second)
		{
			return false;
		}
	}
}
