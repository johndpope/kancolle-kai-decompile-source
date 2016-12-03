using System;
using System.Collections;
using UniRx.InternalUtil;
using UnityEngine;

namespace UniRx
{
	public sealed class MainThreadDispatcher : MonoBehaviour
	{
		public enum CullingMode
		{
			Disabled,
			Self,
			All
		}

		public static MainThreadDispatcher.CullingMode cullingMode = MainThreadDispatcher.CullingMode.Self;

		private ThreadSafeQueueWorker queueWorker = new ThreadSafeQueueWorker();

		private Action<Exception> unhandledExceptionCallback = delegate(Exception ex)
		{
			Debug.LogException(ex);
		};

		private static MainThreadDispatcher instance;

		private static bool initialized;

		private static bool isQuitting;

		[ThreadStatic]
		private static object mainThreadToken;

		private Subject<bool> onApplicationFocus;

		private Subject<bool> onApplicationPause;

		private Subject<Unit> onApplicationQuit;

		public static string InstanceName
		{
			get
			{
				if (MainThreadDispatcher.instance == null)
				{
					throw new NullReferenceException("MainThreadDispatcher is not initialized.");
				}
				return MainThreadDispatcher.instance.get_name();
			}
		}

		public static bool IsInitialized
		{
			get
			{
				return MainThreadDispatcher.initialized && MainThreadDispatcher.instance != null;
			}
		}

		private static MainThreadDispatcher Instance
		{
			get
			{
				MainThreadDispatcher.Initialize();
				return MainThreadDispatcher.instance;
			}
		}

		public static void Post(Action action)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (mainThreadDispatcher != null)
			{
				mainThreadDispatcher.queueWorker.Enqueue(action);
			}
		}

		public static void Send(Action action)
		{
			if (MainThreadDispatcher.mainThreadToken != null)
			{
				try
				{
					action.Invoke();
				}
				catch (Exception ex)
				{
					MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
					if (mainThreadDispatcher != null)
					{
						mainThreadDispatcher.unhandledExceptionCallback.Invoke(ex);
					}
				}
			}
			else
			{
				MainThreadDispatcher.Post(action);
			}
		}

		public static void UnsafeSend(Action action)
		{
			try
			{
				action.Invoke();
			}
			catch (Exception ex)
			{
				MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
				if (mainThreadDispatcher != null)
				{
					mainThreadDispatcher.unhandledExceptionCallback.Invoke(ex);
				}
			}
		}

		public static void SendStartCoroutine(IEnumerator routine)
		{
			if (MainThreadDispatcher.mainThreadToken != null)
			{
				MainThreadDispatcher.StartCoroutine(routine);
			}
			else
			{
				MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
				if (mainThreadDispatcher != null)
				{
					mainThreadDispatcher.queueWorker.Enqueue(delegate
					{
						MainThreadDispatcher mainThreadDispatcher2 = MainThreadDispatcher.Instance;
						if (mainThreadDispatcher2 != null)
						{
							mainThreadDispatcher2.StartCoroutine_Auto(routine);
						}
					});
				}
			}
		}

		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (mainThreadDispatcher != null)
			{
				return mainThreadDispatcher.StartCoroutine_Auto(routine);
			}
			return null;
		}

		public static void RegisterUnhandledExceptionCallback(Action<Exception> exceptionCallback)
		{
			if (exceptionCallback == null)
			{
				MainThreadDispatcher.Instance.unhandledExceptionCallback = new Action<Exception>(Stubs.Ignore<Exception>);
			}
			else
			{
				MainThreadDispatcher.Instance.unhandledExceptionCallback = exceptionCallback;
			}
		}

		public static void Initialize()
		{
			if (!MainThreadDispatcher.initialized)
			{
				MainThreadDispatcher mainThreadDispatcher = null;
				try
				{
					mainThreadDispatcher = Object.FindObjectOfType<MainThreadDispatcher>();
				}
				catch
				{
					Exception ex = new Exception("UniRx requires a MainThreadDispatcher component created on the main thread. Make sure it is added to the scene before calling UniRx from a worker thread.");
					Debug.LogException(ex);
					throw ex;
				}
				if (MainThreadDispatcher.isQuitting)
				{
					return;
				}
				if (mainThreadDispatcher == null)
				{
					MainThreadDispatcher.instance = new GameObject("MainThreadDispatcher").AddComponent<MainThreadDispatcher>();
				}
				else
				{
					MainThreadDispatcher.instance = mainThreadDispatcher;
				}
				Object.DontDestroyOnLoad(MainThreadDispatcher.instance);
				MainThreadDispatcher.mainThreadToken = new object();
				MainThreadDispatcher.initialized = true;
			}
		}

		private void Awake()
		{
			if (MainThreadDispatcher.instance == null)
			{
				MainThreadDispatcher.instance = this;
				MainThreadDispatcher.mainThreadToken = new object();
				MainThreadDispatcher.initialized = true;
				Object.DontDestroyOnLoad(base.get_gameObject());
			}
			else if (MainThreadDispatcher.cullingMode == MainThreadDispatcher.CullingMode.Self)
			{
				Debug.LogWarning("There is already a MainThreadDispatcher in the scene. Removing myself...");
				MainThreadDispatcher.DestroyDispatcher(this);
			}
			else if (MainThreadDispatcher.cullingMode == MainThreadDispatcher.CullingMode.All)
			{
				Debug.LogWarning("There is already a MainThreadDispatcher in the scene. Cleaning up all excess dispatchers...");
				MainThreadDispatcher.CullAllExcessDispatchers();
			}
			else
			{
				Debug.LogWarning("There is already a MainThreadDispatcher in the scene.");
			}
		}

		private static void DestroyDispatcher(MainThreadDispatcher aDispatcher)
		{
			if (aDispatcher != MainThreadDispatcher.instance)
			{
				Component[] components = aDispatcher.get_gameObject().GetComponents<Component>();
				if (aDispatcher.get_gameObject().get_transform().get_childCount() == 0 && components.Length == 2)
				{
					if (components[0] is Transform && components[1] is MainThreadDispatcher)
					{
						Object.Destroy(aDispatcher.get_gameObject());
					}
				}
				else
				{
					Object.Destroy(aDispatcher);
				}
			}
		}

		public static void CullAllExcessDispatchers()
		{
			MainThreadDispatcher[] array = Object.FindObjectsOfType<MainThreadDispatcher>();
			for (int i = 0; i < array.Length; i++)
			{
				MainThreadDispatcher.DestroyDispatcher(array[i]);
			}
		}

		private void OnDestroy()
		{
			if (MainThreadDispatcher.instance == this)
			{
				MainThreadDispatcher.instance = Object.FindObjectOfType<MainThreadDispatcher>();
				MainThreadDispatcher.initialized = (MainThreadDispatcher.instance != null);
			}
		}

		private void Update()
		{
			this.queueWorker.ExecuteAll(this.unhandledExceptionCallback);
		}

		private void OnLevelWasLoaded(int level)
		{
		}

		private void OnApplicationFocus(bool focus)
		{
			if (this.onApplicationFocus != null)
			{
				this.onApplicationFocus.OnNext(focus);
			}
		}

		public static IObservable<bool> OnApplicationFocusAsObservable()
		{
			Subject<bool> arg_23_0;
			if ((arg_23_0 = MainThreadDispatcher.Instance.onApplicationFocus) == null)
			{
				arg_23_0 = (MainThreadDispatcher.Instance.onApplicationFocus = new Subject<bool>());
			}
			return arg_23_0;
		}

		private void OnApplicationPause(bool pause)
		{
			if (this.onApplicationPause != null)
			{
				this.onApplicationPause.OnNext(pause);
			}
		}

		public static IObservable<bool> OnApplicationPauseAsObservable()
		{
			Subject<bool> arg_23_0;
			if ((arg_23_0 = MainThreadDispatcher.Instance.onApplicationPause) == null)
			{
				arg_23_0 = (MainThreadDispatcher.Instance.onApplicationPause = new Subject<bool>());
			}
			return arg_23_0;
		}

		private void OnApplicationQuit()
		{
			MainThreadDispatcher.isQuitting = true;
			if (this.onApplicationQuit != null)
			{
				this.onApplicationQuit.OnNext(Unit.Default);
			}
		}

		public static IObservable<Unit> OnApplicationQuitAsObservable()
		{
			Subject<Unit> arg_23_0;
			if ((arg_23_0 = MainThreadDispatcher.Instance.onApplicationQuit) == null)
			{
				arg_23_0 = (MainThreadDispatcher.Instance.onApplicationQuit = new Subject<Unit>());
			}
			return arg_23_0;
		}
	}
}
