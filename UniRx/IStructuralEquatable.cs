using System;
using System.Collections;

namespace UniRx
{
	public interface IStructuralEquatable
	{
		bool Equals(object other, IEqualityComparer comparer);

		int GetHashCode(IEqualityComparer comparer);
	}
}
