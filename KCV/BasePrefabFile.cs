using System;
using UnityEngine;

namespace KCV
{
	public class BasePrefabFile : IDisposable
	{
		private bool _isDisposed;

		public BasePrefabFile()
		{
			this._isDisposed = false;
		}

		public static Transform PassesPrefab(ref Transform prefab)
		{
			Transform result = prefab;
			prefab = null;
			return result;
		}

		public static T InstantiatePrefab<T>(ref T instance, ref Transform prefab, Transform parent)
		{
			return Util.Instantiate<T>(ref instance, ref prefab, parent);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				return;
			}
			if (disposing)
			{
			}
			this._isDisposed = true;
		}
	}
}
