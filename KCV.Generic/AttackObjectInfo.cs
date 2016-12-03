using System;
using UnityEngine;

namespace KCV.Generic
{
	[Serializable]
	public struct AttackObjectInfo
	{
		public Transform prefab;

		public Transform parent;

		public AttackObjectInfo(Transform prefab, Transform parent)
		{
			this.prefab = prefab;
			this.parent = parent;
		}

		public override string ToString()
		{
			string empty = string.Empty;
			return empty + string.Format("prefab:{0}|parent;{1}", this.prefab, this.parent);
		}
	}
}
