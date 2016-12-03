using Common.Enum;
using KCV.PopupString;
using KCV.Tutorial.Guide;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV
{
	public class PortObjectManager : SingletonMonoBehaviour<PortObjectManager>
	{
		[Button("Unload", "Unload", new object[]
		{

		})]
		public int unload;

		[SerializeField]
		private Transform SceneChanger;

		[SerializeField]
		private GameObject PortFramePrefab;

		public PortTransitionManager PortTransition;

		private IEnumerator OnSceneChangeCoroutine;

		private int SceneMoveCount;

		private string nowScene;

		private bool isLoadLevelScene;

		public static Action SceneChangeAct;

		[SerializeField]
		private GameObject[] ScenePrefabs;

		[SerializeField]
		private GameObject[] SceneObjects;

		public GameObject SceneObject;

		private Vector3 furnitureScale;

		private Vector3 furniturePosition;

		public bool isHidePortObject;

		private KeyControl portKeyControl;

		private Dictionary<string, int> SceneNo;

		private TutorialGuide NowTutorialGuide;

		private KeyControl dummyKey;

		private bool[,] ReleaseSet = new bool[,]
		{
			{
				true,
				false,
				false,
				true,
				false,
				true,
				true,
				false
			},
			{
				false,
				false,
				true,
				false,
				false,
				true,
				true,
				true
			},
			{
				false,
				true,
				false,
				false,
				true,
				true,
				true,
				false
			},
			{
				true,
				true,
				true,
				true,
				true,
				true,
				true,
				true
			}
		};

		public string NowScene
		{
			get
			{
				return this.nowScene;
			}
		}

		public bool IsLoadLevelScene
		{
			get
			{
				return this.isLoadLevelScene;
			}
		}

		public void setOnSceneChangeCoroutine(IEnumerator cor)
		{
			this.OnSceneChangeCoroutine = cor;
		}

		public void UnloadFlagOn()
		{
			this.SceneMoveCount = 6;
		}

		private void Awake()
		{
			base.Awake();
			this.SceneMoveCount = 0;
		}

		private void Start()
		{
			this.setNowScene(Generics.Scene.Strategy.ToString(), false);
			this.SceneNo = new Dictionary<string, int>(StringComparer.get_OrdinalIgnoreCase());
			this.SceneNo.Add(Generics.Scene.Organize.ToString(), 0);
			this.SceneNo.Add(Generics.Scene.Remodel.ToString(), 1);
			this.SceneNo.Add(Generics.Scene.Arsenal.ToString(), 2);
			this.SceneNo.Add(Generics.Scene.Supply.ToString(), 3);
			this.SceneNo.Add(Generics.Scene.Duty.ToString(), 4);
			this.SceneNo.Add(Generics.Scene.PortTop.ToString(), 5);
			this.SceneNo.Add(Generics.Scene.Strategy.ToString(), 6);
			this.SceneNo.Add(Generics.Scene.Repair.ToString(), 7);
			this.SceneNo.Add(Generics.Scene.ArsenalSelector.ToString(), 8);
			this.dummyKey = new KeyControl(0, 0, 0.4f, 0.1f);
		}

		private void Update()
		{
			if (this.isHidePortObject && (this.portKeyControl.IsAnyKey || Input.GetMouseButtonDown(0)))
			{
				this.HidePortObject(null);
			}
		}

		public void EnterStrategy()
		{
			if (SingletonMonoBehaviour<UIPortFrame>.Instance != null)
			{
				Object.Destroy(SingletonMonoBehaviour<UIPortFrame>.Instance.get_gameObject());
			}
		}

		private void setLive2D(Generics.Scene NextScene)
		{
			if (NextScene == Generics.Scene.PortTop || NextScene == Generics.Scene.Strategy)
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			}
			else
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
		}

		public void HidePortObject(KeyControl portKeyControl)
		{
			this.portKeyControl = portKeyControl;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			if (this.isHidePortObject)
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.SetActive(true);
				this.isHidePortObject = false;
				base.set_enabled(false);
			}
			else
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.SetActive(false);
				this.isHidePortObject = true;
				base.set_enabled(true);
			}
		}

		public void InstantiateScene(Generics.Scene NextScene, bool isForceFadeOut = false)
		{
			App.OnlyController = new KeyControl(0, 0, 0.4f, 0.1f);
			if (this.NowTutorialGuide != null)
			{
				this.NowTutorialGuide.Hide();
			}
			if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsOpen)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.CloseMenu();
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(true);
			}
			this.PortTransition.isTransitionNow = true;
			if (this.isUseCrossFade(NextScene) && !isForceFadeOut)
			{
				this.PortTransition.StartTransition(NextScene, true, delegate
				{
					this.InstantiateSceneChange(NextScene);
				});
			}
			else
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					this.InstantiateSceneChange(NextScene);
				});
			}
		}

		private bool isUseCrossFade(Generics.Scene NextScene)
		{
			return NextScene != Generics.Scene.PortTop && NextScene != Generics.Scene.Strategy && NextScene != Generics.Scene.Interior && this.NowScene != Generics.Scene.Strategy.ToString();
		}

		private void InstantiateSceneChange(Generics.Scene NextScene)
		{
			base.StartCoroutine(this.InstantiateSceneChange(NextScene, true));
		}

		[DebuggerHidden]
		private IEnumerator InstantiateSceneChange(Generics.Scene NextScene, bool destroyPrevScene)
		{
			PortObjectManager.<InstantiateSceneChange>c__IteratorA2 <InstantiateSceneChange>c__IteratorA = new PortObjectManager.<InstantiateSceneChange>c__IteratorA2();
			<InstantiateSceneChange>c__IteratorA.destroyPrevScene = destroyPrevScene;
			<InstantiateSceneChange>c__IteratorA.NextScene = NextScene;
			<InstantiateSceneChange>c__IteratorA.<$>destroyPrevScene = destroyPrevScene;
			<InstantiateSceneChange>c__IteratorA.<$>NextScene = NextScene;
			<InstantiateSceneChange>c__IteratorA.<>f__this = this;
			return <InstantiateSceneChange>c__IteratorA;
		}

		public static bool isPrefabSecene(Generics.Scene scene)
		{
			return scene == Generics.Scene.Organize || scene == Generics.Scene.Remodel || scene == Generics.Scene.Supply || scene == Generics.Scene.Arsenal || scene == Generics.Scene.Duty || scene == Generics.Scene.PortTop || scene == Generics.Scene.Strategy || scene == Generics.Scene.Repair;
		}

		public bool isLoadSecene()
		{
			string text = this.nowScene.ToLower();
			return text == Generics.Scene.Record.ToString().ToLower() || text == Generics.Scene.Album.ToString().ToLower() || text == Generics.Scene.Item.ToString().ToLower() || text == Generics.Scene.Interior.ToString().ToLower() || text == Generics.Scene.SaveLoad.ToString().ToLower() || text == Generics.Scene.ImprovementArsenal.ToString().ToLower();
		}

		public void DestroyScene()
		{
			Object.DestroyImmediate(this.SceneObject);
			this.SceneObject = null;
		}

		private void OnLevelWasLoaded()
		{
			this.setNowScene(Application.get_loadedLevelName(), true);
			this.SceneMoveCount = 0;
			if (this.SceneNo != null && this.SceneNo.ContainsKey(Application.get_loadedLevelName()))
			{
				this.SceneObject = GameObject.Find(this.ScenePrefabs[this.SceneNo.get_Item(Application.get_loadedLevelName())].get_name());
			}
			else
			{
				this.SceneObject = null;
			}
		}

		private void CheckMemory()
		{
			DebugUtils.Log(Profiler.GetTotalReservedMemory().ToString());
			DebugUtils.Log(Profiler.GetTotalAllocatedMemory().ToString());
			DebugUtils.Log(Profiler.GetTotalUnusedReservedMemory().ToString());
		}

		private void SceneAdjustNow(Generics.Scene NextScene)
		{
			switch (NextScene)
			{
			case Generics.Scene.Remodel:
				if (!SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					Util.Instantiate(this.PortFramePrefab, null, false, false);
				}
				goto IL_97;
			case Generics.Scene.Repair:
			case Generics.Scene.Arsenal:
				IL_1A:
				if (NextScene != Generics.Scene.Item)
				{
					goto IL_97;
				}
				if (!SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					Util.Instantiate(this.PortFramePrefab, null, false, false);
				}
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(true);
				goto IL_97;
			case Generics.Scene.ImprovementArsenal:
				if (!SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					Util.Instantiate(this.PortFramePrefab, null, false, false);
				}
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(true);
				goto IL_97;
			}
			goto IL_1A;
			IL_97:
			if (this.NowScene.ToLower() == Generics.Scene.Remodel.ToString().ToLower())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.setVisibleHeader(true);
			}
			else if (this.NowScene.ToLower() == Generics.Scene.Strategy.ToString().ToLower())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.DestroyCache();
			}
		}

		private void SceneAdjustNext(Generics.Scene NextScene)
		{
			if (NextScene == Generics.Scene.Strategy)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
			}
			else if (!SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				Util.Instantiate(this.PortFramePrefab, null, false, false);
			}
			if (NextScene == Generics.Scene.PortTop)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
			}
		}

		public void setNowScene(string NowScene, bool isLoadLevel)
		{
			this.nowScene = NowScene;
			this.isLoadLevelScene = isLoadLevel;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.disableButtonList.Clear();
		}

		public void SceneLoad(Generics.Scene NextScene)
		{
			App.OnlyController = this.dummyKey;
			App.isFirstUpdate = true;
			if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsOpen)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.CloseMenu();
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(true);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			}
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				this.SceneAdjustNow(NextScene);
				if (PortObjectManager.SceneChangeAct != null)
				{
					PortObjectManager.SceneChangeAct.Invoke();
					PortObjectManager.SceneChangeAct = null;
				}
				this.StartCoroutine(this.GotoNextScene(NextScene));
			});
		}

		[DebuggerHidden]
		private IEnumerator GotoNextScene(Generics.Scene NextScene)
		{
			PortObjectManager.<GotoNextScene>c__IteratorA3 <GotoNextScene>c__IteratorA = new PortObjectManager.<GotoNextScene>c__IteratorA3();
			<GotoNextScene>c__IteratorA.NextScene = NextScene;
			<GotoNextScene>c__IteratorA.<$>NextScene = NextScene;
			<GotoNextScene>c__IteratorA.<>f__this = this;
			return <GotoNextScene>c__IteratorA;
		}

		private bool hasSceneObject()
		{
			return this.SceneObject != null;
		}

		private GameObject GetSceneObject()
		{
			return this.SceneObject;
		}

		private void SetSceneObject(GameObject nextSceneObject)
		{
			this.SceneObject = nextSceneObject;
		}

		public void OverwriteSceneObject(GameObject sceneObject)
		{
			bool flag = this.hasSceneObject();
			if (flag)
			{
				GameObject sceneObject2 = this.GetSceneObject();
				Object.Destroy(sceneObject2);
			}
			this.SetSceneObject(sceneObject);
		}

		public void SetTutorialGuide(TutorialGuide guide)
		{
			this.NowTutorialGuide = guide;
		}

		public TutorialGuide GetTutorialGuide()
		{
			return this.NowTutorialGuide;
		}

		private void OnDestroy()
		{
			this.Release();
		}

		public void ManualRelease()
		{
			this.Release();
		}

		private void Release()
		{
			int num = (!(SingletonMonoBehaviour<AppInformation>.Instance != null)) ? 0 : SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo;
			if (this.ScenePrefabs != null)
			{
				for (int i = 0; i < this.ReleaseSet.GetLength(1); i++)
				{
					if (this.ReleaseSet[num, i])
					{
						this.ScenePrefabs[i] = null;
					}
				}
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance != null)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo = (int)Util.LoopValue(SingletonMonoBehaviour<AppInformation>.Instance.ReleaseSetNo + 1, 0f, 3f);
			}
			this.SceneChanger = null;
			this.PortFramePrefab = null;
			this.NowTutorialGuide = null;
			this.PortTransition = null;
			PortObjectManager.SceneChangeAct = null;
			this.SceneObjects = null;
			this.SceneObject = null;
			this.portKeyControl = null;
			if (this.SceneNo != null)
			{
				this.SceneNo.Clear();
			}
			this.SceneNo = null;
			this.NowTutorialGuide = null;
		}

		private void Unload()
		{
			Resources.UnloadUnusedAssets();
		}

		public bool BackToActiveScene()
		{
			bool flag = this.IsGoPortCurrentDeck();
			if (flag)
			{
				return this.BackToPort();
			}
			return this.BackToStrategy();
		}

		public bool BackToPort()
		{
			if (this.isLoadLevelScene)
			{
				this.SceneLoad(Generics.Scene.PortTop);
			}
			else
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.PortTop, false);
			}
			return true;
		}

		public bool BackToPortOrOrganize()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != MissionStates.NONE)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.InMissionShip));
				return false;
			}
			bool flag = this.IsGoPortCurrentDeck();
			if (flag)
			{
				return this.BackToPort();
			}
			return this.BackToOrganize();
		}

		private bool BackToOrganize()
		{
			this.SceneLoad(Generics.Scene.Organize);
			return true;
		}

		public bool BackToStrategy()
		{
			if (this.isLoadLevelScene)
			{
				this.SceneLoad(Generics.Scene.Strategy);
			}
			else
			{
				this.InstantiateScene(Generics.Scene.Strategy, false);
			}
			return true;
		}

		private bool IsGoPortCurrentDeck()
		{
			return this.IsGoPort(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
		}

		public bool IsGoPort(DeckModel deckModel)
		{
			bool flag = deckModel.GetFlagShip() != null;
			bool flag2 = !deckModel.HasBling();
			return flag && flag2;
		}
	}
}
