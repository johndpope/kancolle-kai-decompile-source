using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class NGUITools
{
	private static AudioListener mListener;

	private static bool mLoaded = false;

	private static float mGlobalVolume = 1f;

	private static float mLastTimestamp = 0f;

	private static AudioClip mLastClip;

	private static Vector3[] mSides = new Vector3[4];

	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	public static bool fileAccess
	{
		get
		{
			return Application.get_platform() != 5 && Application.get_platform() != 3;
		}
	}

	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.get_text();
		}
		set
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(value);
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	public static Vector2 screenSize
	{
		get
		{
			return new Vector2((float)Screen.get_width(), (float)Screen.get_height());
		}
	}

	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		float time = Time.get_time();
		if (NGUITools.mLastClip == clip && NGUITools.mLastTimestamp + 0.1f > time)
		{
			return null;
		}
		NGUITools.mLastClip = clip;
		NGUITools.mLastTimestamp = time;
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				AudioListener[] array = Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (NGUITools.GetActive(array[i]))
						{
							NGUITools.mListener = array[i];
							break;
						}
					}
				}
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.get_main();
					if (camera == null)
					{
						camera = (Object.FindObjectOfType(typeof(Camera)) as Camera);
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.get_gameObject().AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.get_enabled() && NGUITools.GetActive(NGUITools.mListener.get_gameObject()))
			{
				AudioSource audioSource = NGUITools.mListener.GetComponent<AudioSource>();
				if (audioSource == null)
				{
					audioSource = NGUITools.mListener.get_gameObject().AddComponent<AudioSource>();
				}
				audioSource.set_priority(50);
				audioSource.set_pitch(pitch);
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return Random.Range(min, max + 1);
	}

	public static string GetHierarchy(GameObject obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		string text = obj.get_name();
		while (obj.get_transform().get_parent() != null)
		{
			obj = obj.get_transform().get_parent().get_gameObject();
			text = obj.get_name() + "\\" + text;
		}
		return text;
	}

	public static T[] FindActive<T>() where T : Component
	{
		return Object.FindObjectsOfType(typeof(T)) as T[];
	}

	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		Camera camera;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			camera = UICamera.list.buffer[i].cachedCamera;
			if (camera && (camera.get_cullingMask() & num) != 0)
			{
				return camera;
			}
		}
		camera = Camera.get_main();
		if (camera && (camera.get_cullingMask() & num) != 0)
		{
			return camera;
		}
		Camera[] array = new Camera[Camera.get_allCamerasCount()];
		int allCameras = Camera.GetAllCameras(array);
		for (int j = 0; j < allCameras; j++)
		{
			camera = array[j];
			if (camera && camera.get_enabled() && (camera.get_cullingMask() & num) != 0)
			{
				return camera;
			}
		}
		return null;
	}

	public static void AddWidgetCollider(GameObject go)
	{
		NGUITools.AddWidgetCollider(go, false);
	}

	public static void AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
				return;
			}
			if (component != null)
			{
				return;
			}
			BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
			if (boxCollider2D != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			UICamera uICamera = UICamera.FindCameraForLayer(go.get_layer());
			if (uICamera != null && (uICamera.eventType == UICamera.EventType.World_2D || uICamera.eventType == UICamera.EventType.UI_2D))
			{
				boxCollider2D = go.AddComponent<BoxCollider2D>();
				boxCollider2D.set_isTrigger(true);
				UIWidget component2 = go.GetComponent<UIWidget>();
				if (component2 != null)
				{
					component2.autoResizeBoxCollider = true;
				}
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			boxCollider = go.AddComponent<BoxCollider>();
			boxCollider.set_isTrigger(true);
			UIWidget component3 = go.GetComponent<UIWidget>();
			if (component3 != null)
			{
				component3.autoResizeBoxCollider = true;
			}
			NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
		}
	}

	public static void UpdateWidgetCollider(GameObject go)
	{
		NGUITools.UpdateWidgetCollider(go, false);
	}

	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			BoxCollider component = go.GetComponent<BoxCollider>();
			if (component != null)
			{
				NGUITools.UpdateWidgetCollider(component, considerInactive);
				return;
			}
			BoxCollider2D component2 = go.GetComponent<BoxCollider2D>();
			if (component2 != null)
			{
				NGUITools.UpdateWidgetCollider(component2, considerInactive);
			}
		}
	}

	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.get_gameObject();
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector4 drawRegion = component.drawRegion;
				if (drawRegion.x != 0f || drawRegion.y != 0f || drawRegion.z != 1f || drawRegion.w != 1f)
				{
					Vector4 drawingDimensions = component.drawingDimensions;
					box.set_center(new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f));
					box.set_size(new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y));
				}
				else
				{
					Vector3[] localCorners = component.localCorners;
					box.set_center(Vector3.Lerp(localCorners[0], localCorners[2], 0.5f));
					box.set_size(localCorners[2] - localCorners[0]);
				}
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.get_transform(), considerInactive);
				box.set_center(bounds.get_center());
				box.set_size(new Vector3(bounds.get_size().x, bounds.get_size().y, 0f));
			}
		}
	}

	public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.get_gameObject();
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector3[] localCorners = component.localCorners;
				box.set_offset(Vector3.Lerp(localCorners[0], localCorners[2], 0.5f));
				box.set_size(localCorners[2] - localCorners[0]);
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.get_transform(), considerInactive);
				box.set_offset(bounds.get_center());
				box.set_size(new Vector2(bounds.get_size().x, bounds.get_size().y));
			}
		}
	}

	public static string GetTypeName<T>()
	{
		string text = typeof(T).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static string GetTypeName(Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static void RegisterUndo(Object obj, string name)
	{
	}

	public static void SetDirty(Object obj)
	{
	}

	public static GameObject AddChild(GameObject parent)
	{
		return NGUITools.AddChild(parent, true);
	}

	public static GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.get_transform();
			transform.set_parent(parent.get_transform());
			transform.set_localPosition(Vector3.get_zero());
			transform.set_localRotation(Quaternion.get_identity());
			transform.set_localScale(Vector3.get_one());
			gameObject.set_layer(parent.get_layer());
		}
		return gameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(prefab);
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.get_transform();
			transform.set_parent(parent.get_transform());
			transform.set_localPosition(Vector3.get_zero());
			transform.set_localRotation(Quaternion.get_identity());
			transform.set_localScale(Vector3.get_one());
			gameObject.set_layer(parent.get_layer());
		}
		return gameObject;
	}

	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = 2147483647;
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			if (componentsInChildren[i].get_enabled())
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
			i++;
		}
		return num;
	}

	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			num = Mathf.Max(num, componentsInChildren[i].depth);
			i++;
		}
		return num + 1;
	}

	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				UIWidget uIWidget = componentsInChildren[i];
				if (!(uIWidget.cachedGameObject != go) || (!(uIWidget.GetComponent<Collider>() != null) && !(uIWidget.GetComponent<Collider2D>() != null)))
				{
					num = Mathf.Max(num, uIWidget.depth);
				}
				i++;
			}
			return num + 1;
		}
		return NGUITools.CalculateNextDepth(go);
	}

	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (!(go != null))
		{
			return 0;
		}
		UIPanel uIPanel = go.GetComponent<UIPanel>();
		if (uIPanel != null)
		{
			UIPanel[] componentsInChildren = go.GetComponentsInChildren<UIPanel>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UIPanel uIPanel2 = componentsInChildren[i];
				uIPanel2.depth += adjustment;
			}
			return 1;
		}
		uIPanel = NGUITools.FindInParents<UIPanel>(go);
		if (uIPanel == null)
		{
			return 0;
		}
		UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(true);
		int j = 0;
		int num = componentsInChildren2.Length;
		while (j < num)
		{
			UIWidget uIWidget = componentsInChildren2[j];
			if (!(uIWidget.panel != uIPanel))
			{
				uIWidget.depth += adjustment;
			}
			j++;
		}
		return 2;
	}

	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	public static void NormalizeWidgetDepths()
	{
		NGUITools.NormalizeWidgetDepths(NGUITools.FindActive<UIWidget>());
	}

	public static void NormalizeWidgetDepths(GameObject go)
	{
		NGUITools.NormalizeWidgetDepths(go.GetComponentsInChildren<UIWidget>());
	}

	public static void NormalizeWidgetDepths(UIWidget[] list)
	{
		int num = list.Length;
		if (num > 0)
		{
			Array.Sort<UIWidget>(list, new Comparison<UIWidget>(UIWidget.FullCompareFunc));
			int num2 = 0;
			int depth = list[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIWidget uIWidget = list[i];
				if (uIWidget.depth == depth)
				{
					uIWidget.depth = num2;
				}
				else
				{
					depth = uIWidget.depth;
					num2 = (uIWidget.depth = num2 + 1);
				}
			}
		}
	}

	public static void NormalizePanelDepths()
	{
		UIPanel[] array = NGUITools.FindActive<UIPanel>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIPanel>(array, new Comparison<UIPanel>(UIPanel.CompareFunc));
			int num2 = 0;
			int depth = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIPanel uIPanel = array[i];
				if (uIPanel.depth == depth)
				{
					uIPanel.depth = num2;
				}
				else
				{
					depth = uIPanel.depth;
					num2 = (uIPanel.depth = num2 + 1);
				}
			}
		}
	}

	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uIRoot = (!(trans != null)) ? null : NGUITools.FindInParents<UIRoot>(trans.get_gameObject());
		if (uIRoot == null && UIRoot.list.get_Count() > 0)
		{
			using (List<UIRoot>.Enumerator enumerator = UIRoot.list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIRoot current = enumerator.get_Current();
					if (current.get_gameObject().get_layer() == layer)
					{
						uIRoot = current;
						break;
					}
				}
			}
		}
		if (uIRoot != null)
		{
			UICamera componentInChildren = uIRoot.GetComponentInChildren<UICamera>();
			if (componentInChildren != null && componentInChildren.GetComponent<Camera>().get_orthographic() == advanced3D)
			{
				trans = null;
				uIRoot = null;
			}
		}
		if (uIRoot == null)
		{
			GameObject gameObject = NGUITools.AddChild(null, false);
			uIRoot = gameObject.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject.set_layer(layer);
			if (advanced3D)
			{
				gameObject.set_name("UI Root (3D)");
				uIRoot.scalingStyle = UIRoot.Scaling.Constrained;
			}
			else
			{
				gameObject.set_name("UI Root");
				uIRoot.scalingStyle = UIRoot.Scaling.Flexible;
			}
		}
		UIPanel uIPanel = uIRoot.GetComponentInChildren<UIPanel>();
		if (uIPanel == null)
		{
			Camera[] array = NGUITools.FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uIRoot.get_gameObject().get_layer();
			for (int i = 0; i < array.Length; i++)
			{
				Camera camera = array[i];
				if (camera.get_clearFlags() == 2 || camera.get_clearFlags() == 1)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.get_depth());
				camera.set_cullingMask(camera.get_cullingMask() & ~num2);
			}
			Camera camera2 = NGUITools.AddChild<Camera>(uIRoot.get_gameObject(), false);
			camera2.get_gameObject().AddComponent<UICamera>();
			camera2.set_clearFlags((!flag) ? 2 : 3);
			camera2.set_backgroundColor(Color.get_grey());
			camera2.set_cullingMask(num2);
			camera2.set_depth(num + 1f);
			if (advanced3D)
			{
				camera2.set_nearClipPlane(0.1f);
				camera2.set_farClipPlane(4f);
				camera2.get_transform().set_localPosition(new Vector3(0f, 0f, -700f));
			}
			else
			{
				camera2.set_orthographic(true);
				camera2.set_orthographicSize(1f);
				camera2.set_nearClipPlane(-10f);
				camera2.set_farClipPlane(10f);
			}
			AudioListener[] array2 = NGUITools.FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.get_gameObject().AddComponent<AudioListener>();
			}
			uIPanel = uIRoot.get_gameObject().AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.get_parent() != null)
			{
				trans = trans.get_parent();
			}
			if (NGUITools.IsChild(trans, uIPanel.get_transform()))
			{
				uIPanel = trans.get_gameObject().AddComponent<UIPanel>();
			}
			else
			{
				trans.set_parent(uIPanel.get_transform());
				trans.set_localScale(Vector3.get_one());
				trans.set_localPosition(Vector3.get_zero());
				NGUITools.SetChildLayer(uIPanel.cachedTransform, uIPanel.cachedGameObject.get_layer());
			}
		}
		return uIPanel;
	}

	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.get_childCount(); i++)
		{
			Transform child = t.GetChild(i);
			child.get_gameObject().set_layer(layer);
			NGUITools.SetChildLayer(child, layer);
		}
	}

	public static T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent);
		gameObject.set_name(NGUITools.GetTypeName<T>());
		return gameObject.AddComponent<T>();
	}

	public static T AddChild<T>(GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent, undo);
		gameObject.set_name(NGUITools.GetTypeName<T>());
		return gameObject.AddComponent<T>();
	}

	public static T AddWidget<T>(GameObject go) where T : UIWidget
	{
		int depth = NGUITools.CalculateNextDepth(go);
		T result = NGUITools.AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	public static T AddWidget<T>(GameObject go, int depth) where T : UIWidget
	{
		T result = NGUITools.AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UISpriteData uISpriteData = (!(atlas != null)) ? null : atlas.GetSprite(spriteName);
		UISprite uISprite = NGUITools.AddWidget<UISprite>(go);
		uISprite.type = ((uISpriteData != null && uISpriteData.hasBorder) ? UIBasicSprite.Type.Sliced : UIBasicSprite.Type.Simple);
		uISprite.atlas = atlas;
		uISprite.spriteName = spriteName;
		return uISprite;
	}

	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.get_transform();
		while (true)
		{
			Transform parent = transform.get_parent();
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.get_gameObject();
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		T component = go.GetComponent<T>();
		if (component == null)
		{
			Transform parent = go.get_transform().get_parent();
			while (parent != null && component == null)
			{
				component = parent.get_gameObject().GetComponent<T>();
				parent = parent.get_parent();
			}
		}
		return component;
	}

	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return (T)((object)null);
		}
		return trans.GetComponentInParent<T>();
	}

	public static void Destroy(Object obj)
	{
		if (obj != null)
		{
			if (obj is Transform)
			{
				obj = (obj as Transform).get_gameObject();
			}
			if (Application.get_isPlaying())
			{
				if (obj is GameObject)
				{
					GameObject gameObject = obj as GameObject;
					gameObject.get_transform().set_parent(null);
				}
				Object.Destroy(obj);
			}
			else
			{
				Object.DestroyImmediate(obj);
			}
		}
	}

	public static void DestroyImmediate(Object obj)
	{
		if (obj != null)
		{
			if (Application.get_isEditor())
			{
				Object.DestroyImmediate(obj);
			}
			else
			{
				Object.Destroy(obj);
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] array = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, 1);
			i++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, 1);
			i++;
		}
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.get_parent();
		}
		return false;
	}

	private static void Activate(Transform t)
	{
		NGUITools.Activate(t, false);
	}

	private static void Activate(Transform t, bool compatibilityMode)
	{
		NGUITools.SetActiveSelf(t.get_gameObject(), true);
		if (compatibilityMode)
		{
			int i = 0;
			int childCount = t.get_childCount();
			while (i < childCount)
			{
				Transform child = t.GetChild(i);
				if (child.get_gameObject().get_activeSelf())
				{
					return;
				}
				i++;
			}
			int j = 0;
			int childCount2 = t.get_childCount();
			while (j < childCount2)
			{
				Transform child2 = t.GetChild(j);
				NGUITools.Activate(child2, true);
				j++;
			}
		}
	}

	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.get_gameObject(), false);
	}

	public static void SetActive(GameObject go, bool state)
	{
		NGUITools.SetActive(go, state, true);
	}

	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if (go)
		{
			if (state)
			{
				NGUITools.Activate(go.get_transform(), compatibilityMode);
				NGUITools.CallCreatePanel(go.get_transform());
			}
			else
			{
				NGUITools.Deactivate(go.get_transform());
			}
		}
	}

	[DebuggerHidden, DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		int childCount = t.get_childCount();
		while (i < childCount)
		{
			NGUITools.CallCreatePanel(t.GetChild(i));
			i++;
		}
	}

	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.get_transform();
		if (state)
		{
			int i = 0;
			int childCount = transform.get_childCount();
			while (i < childCount)
			{
				Transform child = transform.GetChild(i);
				NGUITools.Activate(child);
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount2 = transform.get_childCount();
			while (j < childCount2)
			{
				Transform child2 = transform.GetChild(j);
				NGUITools.Deactivate(child2);
				j++;
			}
		}
	}

	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.get_enabled() && mb.get_gameObject().get_activeInHierarchy();
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static bool GetActive(Behaviour mb)
	{
		return mb && mb.get_enabled() && mb.get_gameObject().get_activeInHierarchy();
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static bool GetActive(GameObject go)
	{
		return go && go.get_activeInHierarchy();
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	public static void SetLayer(GameObject go, int layer)
	{
		go.set_layer(layer);
		Transform transform = go.get_transform();
		int i = 0;
		int childCount = transform.get_childCount();
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			NGUITools.SetLayer(child.get_gameObject(), layer);
			i++;
		}
	}

	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.set_localPosition(NGUITools.Round(t.get_localPosition()));
			t.set_localScale(NGUITools.Round(t.get_localScale()));
		}
		int i = 0;
		int childCount = t.get_childCount();
		while (i < childCount)
		{
			NGUITools.MakePixelPerfect(t.GetChild(i));
			i++;
		}
	}

	public static bool Save(string fileName, byte[] bytes)
	{
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string text = Application.get_persistentDataPath() + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(text);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.get_Message());
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string text = Application.get_persistentDataPath() + "/" + fileName;
		if (File.Exists(text))
		{
			return File.ReadAllBytes(text);
		}
		return null;
	}

	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			componentsInChildren[i].ParentHasChanged();
			i++;
		}
	}

	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.get_nearClipPlane(), cam.get_farClipPlane(), 0.5f), null);
	}

	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.get_nearClipPlane(), cam.get_farClipPlane(), 0.5f), relativeTo);
	}

	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.get_orthographic())
		{
			float orthographicSize = cam.get_orthographicSize();
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float num3 = -orthographicSize;
			float num4 = orthographicSize;
			Rect rect = cam.get_rect();
			Vector2 screenSize = NGUITools.screenSize;
			float num5 = screenSize.x / screenSize.y;
			num5 *= rect.get_width() / rect.get_height();
			num *= num5;
			num2 *= num5;
			Transform transform = cam.get_transform();
			Quaternion rotation = transform.get_rotation();
			Vector3 position = transform.get_position();
			NGUITools.mSides[0] = rotation * new Vector3(num, 0f, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(0f, num4, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, 0f, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(0f, num3, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		float depth = Mathf.Lerp(cam.get_nearClipPlane(), cam.get_farClipPlane(), 0.5f);
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.get_nearClipPlane(), cam.get_farClipPlane(), 0.5f), relativeTo);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.get_orthographic())
		{
			float orthographicSize = cam.get_orthographicSize();
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float num3 = -orthographicSize;
			float num4 = orthographicSize;
			Rect rect = cam.get_rect();
			Vector2 screenSize = NGUITools.screenSize;
			float num5 = screenSize.x / screenSize.y;
			num5 *= rect.get_width() / rect.get_height();
			num *= num5;
			num2 *= num5;
			Transform transform = cam.get_transform();
			Quaternion rotation = transform.get_rotation();
			Vector3 position = transform.get_position();
			NGUITools.mSides[0] = rotation * new Vector3(num, num3, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(num, num4, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, num4, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(num2, num3, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static string GetFuncName(object obj, string method)
	{
		if (obj == null)
		{
			return "<null>";
		}
		string text = obj.GetType().ToString();
		int num = text.LastIndexOf('/');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		return (!string.IsNullOrEmpty(method)) ? (text + "/" + method) : text;
	}

	public static void Execute<T>(GameObject go, string funcName) where T : Component
	{
		T[] components = go.GetComponents<T>();
		T[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			T t = array[i];
			MethodInfo method = t.GetType().GetMethod(funcName, 52);
			if (method != null)
			{
				method.Invoke(t, null);
			}
		}
	}

	public static void ExecuteAll<T>(GameObject root, string funcName) where T : Component
	{
		NGUITools.Execute<T>(root, funcName);
		Transform transform = root.get_transform();
		int i = 0;
		int childCount = transform.get_childCount();
		while (i < childCount)
		{
			NGUITools.ExecuteAll<T>(transform.GetChild(i).get_gameObject(), funcName);
			i++;
		}
	}

	public static void ImmediatelyCreateDrawCalls(GameObject root)
	{
		NGUITools.ExecuteAll<UIWidget>(root, "Start");
		NGUITools.ExecuteAll<UIPanel>(root, "Start");
		NGUITools.ExecuteAll<UIWidget>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "LateUpdate");
	}
}
