using System;
using UnityEngine;

namespace KCV
{
	public static class PrefabFile
	{
		public static GameObject Load(string path)
		{
			return Resources.Load(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, path)) as GameObject;
		}

		public static GameObject LoadAsync(string path)
		{
			return Resources.LoadAsync(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, path)).get_asset() as GameObject;
		}

		public static GameObject Load(PrefabFileInfos prefabInfos)
		{
			return Resources.Load(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, prefabInfos.PrefabPath())) as GameObject;
		}

		public static GameObject LoadAsync(PrefabFileInfos prefabInfos)
		{
			return Resources.LoadAsync(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, prefabInfos.PrefabPath())).get_asset() as GameObject;
		}

		public static T Load<T>(string path) where T : Component
		{
			return Resources.Load<T>(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, path));
		}

		public static T LoadAsync<T>(string path) where T : Component
		{
			return (T)((object)Resources.LoadAsync<T>(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, path)).get_asset());
		}

		public static T Load<T>(PrefabFileInfos prefabInfos) where T : Component
		{
			return Resources.Load<T>(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, prefabInfos.PrefabPath()));
		}

		public static T LoadAsync<T>(PrefabFileInfos prefabInfos) where T : Component
		{
			return (T)((object)Resources.LoadAsync<T>(string.Format("{0}/{1}", AppDataPath.PrefabFilePath, prefabInfos.PrefabPath())).get_asset());
		}

		public static GameObject Instantiate(string path, Transform parent = null)
		{
			GameObject gameObject = Object.Instantiate(PrefabFile.Load(path), Vector3.get_zero(), Quaternion.get_identity()) as GameObject;
			if (gameObject == null)
			{
				return null;
			}
			if (parent != null)
			{
				gameObject.get_transform().set_parent(parent);
			}
			gameObject.get_transform().set_localScale(Vector3.get_one());
			return gameObject;
		}

		public static T Instantiate<T>(string path, Transform parent = null) where T : Component
		{
			GameObject gameObject = Object.Instantiate(PrefabFile.Load(path), Vector3.get_zero(), Quaternion.get_identity()) as GameObject;
			if (gameObject == null)
			{
				return (T)((object)null);
			}
			if (parent != null)
			{
				gameObject.get_transform().set_parent(parent);
			}
			gameObject.get_transform().set_localScale(Vector3.get_one());
			return gameObject.SafeGetComponent<T>();
		}

		public static GameObject Instantiate(PrefabFileInfos prefabInfos, Transform parent = null)
		{
			return PrefabFile.Instantiate(prefabInfos.PrefabPath(), parent);
		}

		public static T Instantiate<T>(PrefabFileInfos prefabInfo, Transform parent = null) where T : Component
		{
			GameObject gameObject = Object.Instantiate(PrefabFile.Load(prefabInfo.PrefabPath()), Vector3.get_zero(), Quaternion.get_identity()) as GameObject;
			if (gameObject == null)
			{
				return (T)((object)null);
			}
			if (parent != null)
			{
				gameObject.get_transform().set_parent(parent);
			}
			gameObject.get_transform().set_localScale(Vector3.get_one());
			return gameObject.SafeGetComponent<T>();
		}

		public static T Instantiate<T>(PrefabFileInfos prefabInfo, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null) where T : Component
		{
			GameObject gameObject = Object.Instantiate(PrefabFile.Load(prefabInfo.PrefabPath()), Vector3.get_zero(), Quaternion.get_identity()) as GameObject;
			if (gameObject == null)
			{
				return (T)((object)null);
			}
			if (parent != null)
			{
				gameObject.get_transform().set_parent(parent);
			}
			gameObject.get_transform().set_localPosition(pos);
			gameObject.get_transform().set_localRotation(rot);
			gameObject.get_transform().set_localScale(scale);
			return gameObject.SafeGetComponent<T>();
		}
	}
}
