using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	public enum ControlScheme
	{
		Mouse,
		Touch,
		Controller
	}

	public enum ClickNotification
	{
		None,
		Always,
		BasedOnDelta
	}

	public class MouseOrTouch
	{
		public Vector2 pos;

		public Vector2 lastPos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject last;

		public GameObject current;

		public GameObject pressed;

		public GameObject dragged;

		public float pressTime;

		public float clickTime;

		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		public bool touchBegan = true;

		public bool pressStarted;

		public bool dragStarted;

		public float deltaTime
		{
			get
			{
				return (!this.touchBegan) ? 0f : (RealTime.time - this.pressTime);
			}
		}

		public bool isOverUI
		{
			get
			{
				return this.current != null && this.current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(this.current) != null;
			}
		}
	}

	public enum EventType
	{
		World_3D,
		UI_3D,
		World_2D,
		UI_2D
	}

	private struct DepthEntry
	{
		public int depth;

		public RaycastHit hit;

		public Vector3 point;

		public GameObject go;
	}

	public class Touch
	{
		public int fingerId;

		public TouchPhase phase;

		public Vector2 position;

		public int tapCount;
	}

	public delegate bool GetKeyStateFunc(KeyCode key);

	public delegate float GetAxisFunc(string name);

	public delegate void OnScreenResize();

	public delegate void OnCustomInput();

	public delegate void MoveDelegate(Vector2 delta);

	public delegate void VoidDelegate(GameObject go);

	public delegate void BoolDelegate(GameObject go, bool state);

	public delegate void FloatDelegate(GameObject go, float delta);

	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	public delegate int GetTouchCountCallback();

	public delegate UICamera.Touch GetTouchCallback(int index);

	public static BetterList<UICamera> list = new BetterList<UICamera>();

	public static UICamera.GetKeyStateFunc GetKeyDown = new UICamera.GetKeyStateFunc(Input.GetKeyDown);

	public static UICamera.GetKeyStateFunc GetKeyUp = new UICamera.GetKeyStateFunc(Input.GetKeyUp);

	public static UICamera.GetKeyStateFunc GetKey = new UICamera.GetKeyStateFunc(Input.GetKey);

	public static UICamera.GetAxisFunc GetAxis = new UICamera.GetAxisFunc(Input.GetAxis);

	public static UICamera.OnScreenResize onScreenResize;

	public UICamera.EventType eventType = UICamera.EventType.UI_3D;

	public bool eventsGoToColliders;

	public LayerMask eventReceiverMask = -1;

	public bool debug;

	public bool useMouse = true;

	public bool useTouch = true;

	public bool allowMultiTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public bool stickyTooltip = true;

	public float tooltipDelay = 1f;

	public bool longPressTooltip;

	public float mouseDragThreshold = 4f;

	public float mouseClickThreshold = 10f;

	public float touchDragThreshold = 40f;

	public float touchClickThreshold = 40f;

	public float rangeDistance = -1f;

	public string scrollAxisName = "Mouse ScrollWheel";

	public string verticalAxisName = "Vertical";

	public string horizontalAxisName = "Horizontal";

	public bool commandClick = true;

	public KeyCode submitKey0 = 13;

	public KeyCode submitKey1 = 330;

	public KeyCode cancelKey0 = 27;

	public KeyCode cancelKey1 = 331;

	public static UICamera.OnCustomInput onCustomInput;

	public static bool showTooltips = true;

	public static Vector2 lastTouchPosition = Vector2.get_zero();

	public static Vector3 lastWorldPosition = Vector3.get_zero();

	public static RaycastHit lastHit;

	public static UICamera current = null;

	public static Camera currentCamera = null;

	public static UICamera.ControlScheme currentScheme = UICamera.ControlScheme.Controller;

	public static int currentTouchID = -100;

	public static KeyCode currentKey = 0;

	public static UICamera.MouseOrTouch currentTouch = null;

	public static bool inputHasFocus = false;

	private static GameObject mGenericHandler;

	public static GameObject fallThrough;

	public static UICamera.VoidDelegate onClick;

	public static UICamera.VoidDelegate onDoubleClick;

	public static UICamera.BoolDelegate onHover;

	public static UICamera.BoolDelegate onPress;

	public static UICamera.BoolDelegate onSelect;

	public static UICamera.FloatDelegate onScroll;

	public static UICamera.VectorDelegate onDrag;

	public static UICamera.VoidDelegate onDragStart;

	public static UICamera.ObjectDelegate onDragOver;

	public static UICamera.ObjectDelegate onDragOut;

	public static UICamera.VoidDelegate onDragEnd;

	public static UICamera.ObjectDelegate onDrop;

	public static UICamera.KeyCodeDelegate onKey;

	public static UICamera.BoolDelegate onTooltip;

	public static UICamera.MoveDelegate onMouseMove;

	private static GameObject mCurrentSelection = null;

	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	private static GameObject mHover;

	public static UICamera.MouseOrTouch controller = new UICamera.MouseOrTouch();

	private static float mNextEvent = 0f;

	private static Dictionary<int, UICamera.MouseOrTouch> mTouches = new Dictionary<int, UICamera.MouseOrTouch>();

	private static int mWidth = 0;

	private static int mHeight = 0;

	private GameObject mTooltip;

	private Camera mCam;

	private float mTooltipTime;

	private float mNextRaycast;

	public static bool isDragging = false;

	public static GameObject hoveredObject;

	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	private static Plane m2DPlane = new Plane(Vector3.get_back(), 0f);

	private static int mNotifying = 0;

	private static bool mUsingTouchEvents = true;

	public static UICamera.GetTouchCountCallback GetInputTouchCount;

	public static UICamera.GetTouchCallback GetInputTouch;

	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	public static Ray currentRay
	{
		get
		{
			return (!(UICamera.currentCamera != null) || UICamera.currentTouch == null) ? default(Ray) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	public static bool isOverUI
	{
		get
		{
			if (UICamera.currentTouch != null)
			{
				return UICamera.currentTouch.isOverUI;
			}
			return !(UICamera.hoveredObject == null) && !(UICamera.hoveredObject == UICamera.fallThrough) && NGUITools.FindInParents<UIRoot>(UICamera.hoveredObject) != null;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			if (UICamera.mCurrentSelection)
			{
				return UICamera.mCurrentSelection;
			}
			return null;
		}
		set
		{
			if (UICamera.mCurrentSelection == value)
			{
				return;
			}
			bool flag = false;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
			}
			UICamera.inputHasFocus = false;
			if (UICamera.onSelect != null)
			{
				UICamera.onSelect(UICamera.selectedObject, false);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", false);
			UICamera.mCurrentSelection = value;
			if (UICamera.mCurrentSelection != null)
			{
				if (flag)
				{
					UICamera uICamera = (!(UICamera.mCurrentSelection != null)) ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mCurrentSelection.get_layer());
					if (uICamera != null)
					{
						UICamera.current = uICamera;
						UICamera.currentCamera = uICamera.cachedCamera;
					}
				}
				UICamera.inputHasFocus = (UICamera.mCurrentSelection.get_activeInHierarchy() && UICamera.mCurrentSelection.GetComponent<UIInput>() != null);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mCurrentSelection, true);
				}
				UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", true);
			}
			if (flag)
			{
				UICamera.current = null;
				UICamera.currentCamera = null;
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	public static int touchCount
	{
		get
		{
			int num = 0;
			using (Dictionary<int, UICamera.MouseOrTouch>.Enumerator enumerator = UICamera.mTouches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair = enumerator.get_Current();
					if (keyValuePair.get_Value().pressed != null)
					{
						num++;
					}
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].pressed != null)
				{
					num++;
				}
			}
			if (UICamera.controller.pressed != null)
			{
				num++;
			}
			return num;
		}
	}

	public static int dragCount
	{
		get
		{
			int num = 0;
			using (Dictionary<int, UICamera.MouseOrTouch>.Enumerator enumerator = UICamera.mTouches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair = enumerator.get_Current();
					if (keyValuePair.get_Value().dragged != null)
					{
						num++;
					}
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			return (!(eventHandler != null)) ? null : eventHandler.cachedCamera;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uICamera = UICamera.list.buffer[i];
				if (!(uICamera == null) && uICamera.get_enabled() && NGUITools.GetActive(uICamera.get_gameObject()))
				{
					return uICamera;
				}
			}
			return null;
		}
	}

	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		using (Dictionary<int, UICamera.MouseOrTouch>.Enumerator enumerator = UICamera.mTouches.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair = enumerator.get_Current();
				if (keyValuePair.get_Value().pressed == go)
				{
					return true;
				}
			}
		}
		return UICamera.controller.pressed == go;
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.get_depth() < b.cachedCamera.get_depth())
		{
			return 1;
		}
		if (a.cachedCamera.get_depth() > b.cachedCamera.get_depth())
		{
			return -1;
		}
		return 0;
	}

	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody component = trans.GetComponent<Rigidbody>();
			if (component != null)
			{
				return component;
			}
			trans = trans.get_parent();
		}
		return null;
	}

	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				return component;
			}
			trans = trans.get_parent();
		}
		return null;
	}

	public static bool Raycast(Vector3 inPos)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uICamera = UICamera.list.buffer[i];
			if (uICamera.get_enabled() && NGUITools.GetActive(uICamera.get_gameObject()))
			{
				UICamera.currentCamera = uICamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y))
				{
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int num = UICamera.currentCamera.get_cullingMask() & uICamera.eventReceiverMask;
						float num2 = (uICamera.rangeDistance <= 0f) ? (UICamera.currentCamera.get_farClipPlane() - UICamera.currentCamera.get_nearClipPlane()) : uICamera.rangeDistance;
						if (uICamera.eventType == UICamera.EventType.World_3D)
						{
							if (Physics.Raycast(ray, ref UICamera.lastHit, num2, num))
							{
								UICamera.lastWorldPosition = UICamera.lastHit.get_point();
								UICamera.hoveredObject = UICamera.lastHit.get_collider().get_gameObject();
								if (!UICamera.list[0].eventsGoToColliders)
								{
									Rigidbody rigidbody = UICamera.FindRootRigidbody(UICamera.hoveredObject.get_transform());
									if (rigidbody != null)
									{
										UICamera.hoveredObject = rigidbody.get_gameObject();
									}
								}
								return true;
							}
						}
						else if (uICamera.eventType == UICamera.EventType.UI_3D)
						{
							RaycastHit[] array = Physics.RaycastAll(ray, num2, num);
							if (array.Length > 1)
							{
								int j = 0;
								while (j < array.Length)
								{
									GameObject gameObject = array[j].get_collider().get_gameObject();
									UIWidget component = gameObject.GetComponent<UIWidget>();
									if (component != null)
									{
										if (component.isVisible)
										{
											if (component.hitCheck == null || component.hitCheck(array[j].get_point()))
											{
												goto IL_260;
											}
										}
									}
									else
									{
										UIRect uIRect = NGUITools.FindInParents<UIRect>(gameObject);
										if (!(uIRect != null) || uIRect.finalAlpha >= 0.001f)
										{
											goto IL_260;
										}
									}
									IL_2E1:
									j++;
									continue;
									IL_260:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.hit = array[j];
										UICamera.mHit.point = array[j].get_point();
										UICamera.mHit.go = array[j].get_collider().get_gameObject();
										UICamera.mHits.Add(UICamera.mHit);
										goto IL_2E1;
									}
									goto IL_2E1;
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										UICamera.lastHit = UICamera.mHits[k].hit;
										UICamera.hoveredObject = UICamera.mHits[k].go;
										UICamera.lastWorldPosition = UICamera.mHits[k].point;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (array.Length == 1)
							{
								GameObject gameObject2 = array[0].get_collider().get_gameObject();
								UIWidget component2 = gameObject2.GetComponent<UIWidget>();
								if (component2 != null)
								{
									if (!component2.isVisible)
									{
										goto IL_7E2;
									}
									if (component2.hitCheck != null && !component2.hitCheck(array[0].get_point()))
									{
										goto IL_7E2;
									}
								}
								else
								{
									UIRect uIRect2 = NGUITools.FindInParents<UIRect>(gameObject2);
									if (uIRect2 != null && uIRect2.finalAlpha < 0.001f)
									{
										goto IL_7E2;
									}
								}
								if (UICamera.IsVisible(array[0].get_point(), array[0].get_collider().get_gameObject()))
								{
									UICamera.lastHit = array[0];
									UICamera.lastWorldPosition = array[0].get_point();
									UICamera.hoveredObject = UICamera.lastHit.get_collider().get_gameObject();
									return true;
								}
							}
						}
						else if (uICamera.eventType == UICamera.EventType.World_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, ref num2))
							{
								Vector3 point = ray.GetPoint(num2);
								Collider2D collider2D = Physics2D.OverlapPoint(point, num);
								if (collider2D)
								{
									UICamera.lastWorldPosition = point;
									UICamera.hoveredObject = collider2D.get_gameObject();
									if (!uICamera.eventsGoToColliders)
									{
										Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.hoveredObject.get_transform());
										if (rigidbody2D != null)
										{
											UICamera.hoveredObject = rigidbody2D.get_gameObject();
										}
									}
									return true;
								}
							}
						}
						else if (uICamera.eventType == UICamera.EventType.UI_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, ref num2))
							{
								UICamera.lastWorldPosition = ray.GetPoint(num2);
								Collider2D[] array2 = Physics2D.OverlapPointAll(UICamera.lastWorldPosition, num);
								if (array2.Length > 1)
								{
									int l = 0;
									while (l < array2.Length)
									{
										GameObject gameObject3 = array2[l].get_gameObject();
										UIWidget component3 = gameObject3.GetComponent<UIWidget>();
										if (component3 != null)
										{
											if (component3.isVisible)
											{
												if (component3.hitCheck == null || component3.hitCheck(UICamera.lastWorldPosition))
												{
													goto IL_639;
												}
											}
										}
										else
										{
											UIRect uIRect3 = NGUITools.FindInParents<UIRect>(gameObject3);
											if (!(uIRect3 != null) || uIRect3.finalAlpha >= 0.001f)
											{
												goto IL_639;
											}
										}
										IL_688:
										l++;
										continue;
										IL_639:
										UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject3);
										if (UICamera.mHit.depth != 2147483647)
										{
											UICamera.mHit.go = gameObject3;
											UICamera.mHit.point = UICamera.lastWorldPosition;
											UICamera.mHits.Add(UICamera.mHit);
											goto IL_688;
										}
										goto IL_688;
									}
									UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
									for (int m = 0; m < UICamera.mHits.size; m++)
									{
										if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
										{
											UICamera.hoveredObject = UICamera.mHits[m].go;
											UICamera.mHits.Clear();
											return true;
										}
									}
									UICamera.mHits.Clear();
								}
								else if (array2.Length == 1)
								{
									GameObject gameObject4 = array2[0].get_gameObject();
									UIWidget component4 = gameObject4.GetComponent<UIWidget>();
									if (component4 != null)
									{
										if (!component4.isVisible)
										{
											goto IL_7E2;
										}
										if (component4.hitCheck != null && !component4.hitCheck(UICamera.lastWorldPosition))
										{
											goto IL_7E2;
										}
									}
									else
									{
										UIRect uIRect4 = NGUITools.FindInParents<UIRect>(gameObject4);
										if (uIRect4 != null && uIRect4.finalAlpha < 0.001f)
										{
											goto IL_7E2;
										}
									}
									if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject4))
									{
										UICamera.hoveredObject = gameObject4;
										return true;
									}
								}
							}
						}
					}
				}
			}
			IL_7E2:;
		}
		return false;
	}

	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(go);
		while (uIPanel != null)
		{
			if (!uIPanel.IsVisible(worldPoint))
			{
				return false;
			}
			uIPanel = uIPanel.parentPanel;
		}
		return true;
	}

	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(de.go);
		while (uIPanel != null)
		{
			if (!uIPanel.IsVisible(de.point))
			{
				return false;
			}
			uIPanel = uIPanel.parentPanel;
		}
		return true;
	}

	public static bool IsHighlighted(GameObject go)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			return UICamera.hoveredObject == go;
		}
		return UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.selectedObject == go;
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uICamera = UICamera.list.buffer[i];
			Camera cachedCamera = uICamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.get_cullingMask() & num) != 0)
			{
				return uICamera;
			}
		}
		return null;
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			return 1;
		}
		if (UICamera.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0) || UICamera.GetKeyDown(up1))
		{
			return 1;
		}
		if (UICamera.GetKeyDown(down0) || UICamera.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time && !string.IsNullOrEmpty(axis))
		{
			float num = UICamera.GetAxis(axis);
			if (num > 0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (num < -0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying > 10)
		{
			return;
		}
		if (NGUITools.GetActive(go))
		{
			UICamera.mNotifying++;
			go.SendMessage(funcName, obj, 1);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, 1);
			}
			UICamera.mNotifying--;
		}
	}

	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	public static UICamera.MouseOrTouch GetTouch(int id)
	{
		UICamera.MouseOrTouch mouseOrTouch = null;
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		if (!UICamera.mTouches.TryGetValue(id, ref mouseOrTouch))
		{
			mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.pressTime = RealTime.time;
			mouseOrTouch.touchBegan = true;
			UICamera.mTouches.Add(id, mouseOrTouch);
		}
		return mouseOrTouch;
	}

	public static void RemoveTouch(int id)
	{
		UICamera.mTouches.Remove(id);
	}

	private void Awake()
	{
		UICamera.mWidth = Screen.get_width();
		UICamera.mHeight = Screen.get_height();
		if (Application.get_platform() == 11 || Application.get_platform() == 24 || Application.get_platform() == 8 || Application.get_platform() == 21 || Application.get_platform() == 22)
		{
			this.useTouch = true;
			this.useMouse = false;
			this.useKeyboard = false;
			this.useController = false;
		}
		else if (Application.get_platform() == 9 || Application.get_platform() == 10)
		{
			this.useMouse = false;
			this.useTouch = false;
			this.useKeyboard = false;
			this.useController = true;
		}
		UICamera.mMouse[0].pos = Input.get_mousePosition();
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
	}

	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	private void Start()
	{
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.get_transparencySortMode() != 2)
		{
			this.cachedCamera.set_transparencySortMode(2);
		}
		if (Application.get_isPlaying())
		{
			if (UICamera.fallThrough == null)
			{
				UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.get_gameObject());
				if (uIRoot != null)
				{
					UICamera.fallThrough = uIRoot.get_gameObject();
				}
				else
				{
					Transform transform = base.get_transform();
					UICamera.fallThrough = ((!(transform.get_parent() != null)) ? base.get_gameObject() : transform.get_parent().get_gameObject());
				}
			}
			this.cachedCamera.set_eventMask(0);
		}
		if (this.handlesEvents)
		{
			NGUIDebug.debugRaycast = this.debug;
		}
	}

	private void Update()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if (this.useMouse && UICamera.mCurrentSelection != null)
		{
			if (this.cancelKey0 != null && UICamera.GetKeyDown(this.cancelKey0))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey0;
				UICamera.selectedObject = null;
			}
			else if (this.cancelKey1 != null && UICamera.GetKeyDown(this.cancelKey1))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey1;
				UICamera.selectedObject = null;
			}
		}
		if (UICamera.mCurrentSelection == null)
		{
			UICamera.inputHasFocus = false;
		}
		else if (!UICamera.mCurrentSelection || !UICamera.mCurrentSelection.get_activeInHierarchy())
		{
			UICamera.inputHasFocus = false;
			UICamera.mCurrentSelection = null;
		}
		if ((this.useKeyboard || this.useController) && UICamera.mCurrentSelection != null)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float num = string.IsNullOrEmpty(this.scrollAxisName) ? 0f : UICamera.GetAxis(this.scrollAxisName);
			if (num != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, num);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", num);
			}
			if (UICamera.showTooltips && this.mTooltipTime != 0f && (this.mTooltipTime < RealTime.time || UICamera.GetKey(304) || UICamera.GetKey(303)))
			{
				this.mTooltip = UICamera.mHover;
				UICamera.currentTouch = UICamera.mMouse[0];
				UICamera.currentTouchID = -1;
				this.ShowTooltip(true);
			}
		}
		UICamera.current = null;
		UICamera.currentTouchID = -100;
	}

	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		int width = Screen.get_width();
		int height = Screen.get_height();
		if (width != UICamera.mWidth || height != UICamera.mHeight)
		{
			UICamera.mWidth = width;
			UICamera.mHeight = height;
			UIRoot.Broadcast("UpdateAnchors");
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
	}

	public void ProcessMouse()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag2 = true;
				flag = true;
			}
			else if (Input.GetMouseButton(i))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag = true;
			}
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Touch)
		{
			return;
		}
		Vector2 vector = Input.get_mousePosition();
		Vector2 delta = vector - UICamera.mMouse[0].pos;
		float sqrMagnitude = delta.get_sqrMagnitude();
		bool flag3 = false;
		if (UICamera.currentScheme != UICamera.ControlScheme.Mouse)
		{
			if (sqrMagnitude < 0.001f)
			{
				return;
			}
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			flag3 = true;
		}
		else if (sqrMagnitude > 0.001f)
		{
			flag3 = true;
		}
		UICamera.lastTouchPosition = vector;
		for (int j = 0; j < 3; j++)
		{
			UICamera.mMouse[j].pos = vector;
			UICamera.mMouse[j].delta = delta;
		}
		if (flag || flag3 || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			if (!UICamera.Raycast(Input.get_mousePosition()))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.hoveredObject;
			}
		}
		bool flag4 = UICamera.mMouse[0].last != UICamera.mMouse[0].current;
		if (flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
		}
		UICamera.currentTouch = UICamera.mMouse[0];
		UICamera.currentTouchID = -1;
		if (flag)
		{
			this.mTooltipTime = 0f;
		}
		else if (flag3 && (!this.stickyTooltip || flag4))
		{
			if (this.mTooltipTime != 0f)
			{
				this.mTooltipTime = RealTime.time + this.tooltipDelay;
			}
			else if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
		}
		if (flag3 && UICamera.onMouseMove != null)
		{
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if ((flag2 || !flag) && UICamera.mHover != null && flag4)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.onHover != null)
			{
				UICamera.onHover(UICamera.mHover, false);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", false);
			UICamera.mHover = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown || mouseButtonUp)
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = 323 + l;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			UICamera.currentKey = 0;
		}
		if (!flag && flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			this.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.mHover = UICamera.mMouse[0].current;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouchID = -1;
			if (UICamera.onHover != null)
			{
				UICamera.onHover(UICamera.mHover, true);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", true);
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	public void ProcessTouches()
	{
		int num = (UICamera.GetInputTouchCount != null) ? UICamera.GetInputTouchCount() : Input.get_touchCount();
		for (int i = 0; i < num; i++)
		{
			TouchPhase phase;
			int fingerId;
			Vector2 position;
			int tapCount;
			if (UICamera.GetInputTouch == null)
			{
				UnityEngine.Touch touch = Input.GetTouch(i);
				phase = touch.get_phase();
				fingerId = touch.get_fingerId();
				position = touch.get_position();
				tapCount = touch.get_tapCount();
			}
			else
			{
				UICamera.Touch touch2 = UICamera.GetInputTouch(i);
				phase = touch2.phase;
				fingerId = touch2.fingerId;
				position = touch2.position;
				tapCount = touch2.tapCount;
			}
			UICamera.currentTouchID = ((!this.allowMultiTouch) ? 1 : fingerId);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);
			bool flag = phase == null || UICamera.currentTouch.touchBegan;
			bool flag2 = phase == 4 || phase == 3;
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentScheme = UICamera.ControlScheme.Touch;
			UICamera.currentTouch.delta = ((!flag) ? (position - UICamera.currentTouch.pos) : Vector2.get_zero());
			UICamera.currentTouch.pos = position;
			if (!UICamera.Raycast(UICamera.currentTouch.pos))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (num == 0)
		{
			if (UICamera.mUsingTouchEvents)
			{
				UICamera.mUsingTouchEvents = false;
				return;
			}
			if (this.useMouse)
			{
				this.ProcessMouse();
			}
		}
		else
		{
			UICamera.mUsingTouchEvents = true;
		}
	}

	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			Vector2 vector = Input.get_mousePosition();
			UICamera.currentTouch.delta = ((!mouseButtonDown) ? (vector - UICamera.currentTouch.pos) : Vector2.get_zero());
			UICamera.currentTouch.pos = vector;
			if (!UICamera.Raycast(UICamera.currentTouch.pos))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != null && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey1 != null && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		if (this.submitKey0 != null && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (this.submitKey1 != null && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		if (flag)
		{
			UICamera.currentTouch.pressTime = RealTime.time;
		}
		if (flag || flag2)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.mCurrentSelection;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.last = null;
		}
		int num = 0;
		int num2 = 0;
		if (this.useKeyboard)
		{
			if (UICamera.inputHasFocus)
			{
				num += UICamera.GetDirection(273, 274);
				num2 += UICamera.GetDirection(275, 276);
			}
			else
			{
				num += UICamera.GetDirection(119, 273, 115, 274);
				num2 += UICamera.GetDirection(100, 275, 97, 276);
			}
		}
		if (this.useController)
		{
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				num += UICamera.GetDirection(this.verticalAxisName);
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				num2 += UICamera.GetDirection(this.horizontalAxisName);
			}
		}
		if (num != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			KeyCode keyCode = (num <= 0) ? 274 : 273;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, keyCode);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", keyCode);
		}
		if (num2 != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			KeyCode keyCode2 = (num2 <= 0) ? 276 : 275;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, keyCode2);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", keyCode2);
		}
		if (this.useKeyboard && UICamera.GetKeyDown(9))
		{
			UICamera.currentKey = 9;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, 9);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", 9);
		}
		if (this.cancelKey0 != null && UICamera.GetKeyDown(this.cancelKey0))
		{
			UICamera.currentKey = this.cancelKey0;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, 27);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", 27);
		}
		if (this.cancelKey1 != null && UICamera.GetKeyDown(this.cancelKey1))
		{
			UICamera.currentKey = this.cancelKey1;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, 27);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", 27);
		}
		UICamera.currentTouch = null;
		UICamera.currentKey = 0;
	}

	private void ProcessPress(bool pressed, float click, float drag)
	{
		if (pressed)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.get_zero();
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.selectedObject = UICamera.currentTouch.pressed;
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.get_sqrMagnitude() != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float sqrMagnitude = UICamera.currentTouch.totalDelta.get_sqrMagnitude();
			bool flag = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && drag < sqrMagnitude)
			{
				flag = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.isDragging = true;
				bool flag2 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag2)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
				else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && click < sqrMagnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
	}

	private void ProcessRelease(bool isMouse, float drag)
	{
		if (UICamera.currentTouch == null)
		{
			return;
		}
		UICamera.currentTouch.pressStarted = false;
		if (UICamera.currentTouch.pressed != null)
		{
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDragOut != null)
				{
					UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
				if (UICamera.onDragEnd != null)
				{
					UICamera.onDragEnd(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
			}
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (isMouse)
			{
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.currentTouch.current, true);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
			}
			UICamera.mHover = UICamera.currentTouch.current;
			if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.get_sqrMagnitude() < drag))
			{
				if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
				{
					float time = RealTime.time;
					if (UICamera.onClick != null)
					{
						UICamera.onClick(UICamera.currentTouch.pressed);
					}
					UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
					if (UICamera.currentTouch.clickTime + 0.35f > time)
					{
						if (UICamera.onDoubleClick != null)
						{
							UICamera.onDoubleClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
					}
					UICamera.currentTouch.clickTime = time;
				}
			}
			else if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDrop != null)
				{
					UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
			}
		}
		UICamera.currentTouch.dragStarted = false;
		UICamera.currentTouch.pressed = null;
		UICamera.currentTouch.dragged = null;
	}

	public void ProcessTouch(bool pressed, bool released)
	{
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = (!flag) ? this.touchDragThreshold : this.mouseDragThreshold;
		float num2 = (!flag) ? this.touchClickThreshold : this.mouseClickThreshold;
		num *= num;
		num2 *= num2;
		if (UICamera.currentTouch.pressed != null)
		{
			if (released)
			{
				this.ProcessRelease(flag, num);
			}
			this.ProcessPress(pressed, num2, num);
			if (this.longPressTooltip && UICamera.currentTouch.pressed == UICamera.currentTouch.current && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && !UICamera.currentTouch.dragStarted && UICamera.currentTouch.deltaTime > this.tooltipDelay)
			{
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				this.mTooltip = UICamera.currentTouch.pressed;
				this.ShowTooltip(true);
			}
		}
		else if (flag || pressed || released)
		{
			this.ProcessPress(pressed, num2, num);
			if (released)
			{
				this.ProcessRelease(flag, num);
			}
		}
	}

	public void ShowTooltip(bool val)
	{
		this.mTooltipTime = 0f;
		if (UICamera.onTooltip != null)
		{
			UICamera.onTooltip(this.mTooltip, val);
		}
		UICamera.Notify(this.mTooltip, "OnTooltip", val);
		if (!val)
		{
			this.mTooltip = null;
		}
	}
}
