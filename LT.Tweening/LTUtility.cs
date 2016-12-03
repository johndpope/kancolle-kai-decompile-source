using System;
using UnityEngine;

namespace LT.Tweening
{
	public class LTUtility
	{
		public static Vector3[] reverse(Vector3[] arr)
		{
			int num = arr.Length - 1;
			for (int i = 0; i <= arr.Length / 2; i++)
			{
				Vector3 vector = arr[i];
				arr[i] = arr[num];
				arr[num] = vector;
				num--;
			}
			return arr;
		}
	}
}
