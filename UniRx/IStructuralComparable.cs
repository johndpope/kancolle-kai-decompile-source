using System;
using System.Collections;

namespace UniRx
{
	public interface IStructuralComparable
	{
		int CompareTo(object other, IComparer comparer);
	}
}
