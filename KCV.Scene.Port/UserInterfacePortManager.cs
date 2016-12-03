using DG.Tweening;
using KCV.Arsenal;
using KCV.Furniture.JukeBox;
using KCV.PopupString;
using KCV.Scene.Marriage;
using KCV.Scene.Others;
using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UserInterfacePortManager : MonoBehaviour
	{
		private enum State
		{
			NONE,
			Menu,
			FirstOpenMenu,
			JukeBox,
			PortViewer,
			MarriageConfirm,
			MarriageProduction,
			Option,
			ArsenalTypeSelect
		}

		private enum ShipDepth
		{
			Default,
			HigherPortFrame
		}

		private enum EngageValidation
		{
			NoYubiwa,
			InRepair
		}

		public class StateManager<State>
		{
			private Stack<State> mStateStack;

			private State mEmptyState;

			public Action<State> OnPush
			{
				private get;
				set;
			}

			public Action<State> OnPop
			{
				private get;
				set;
			}

			public Action<State> OnResume
			{
				private get;
				set;
			}

			public Action<State> OnSwitch
			{
				private get;
				set;
			}

			public State CurrentState
			{
				get
				{
					if (0 < this.mStateStack.get_Count())
					{
						return this.mStateStack.Peek();
					}
					return this.mEmptyState;
				}
			}

			public StateManager(State emptyState)
			{
				this.mEmptyState = emptyState;
				this.mStateStack = new Stack<State>();
			}

			public void PushState(State state)
			{
				this.mStateStack.Push(state);
				this.Notify(this.OnPush, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}

			public void ReplaceState(State state)
			{
				if (0 < this.mStateStack.get_Count())
				{
					this.PopState();
				}
				this.mStateStack.Push(state);
				this.Notify(this.OnPush, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}

			public void PopState()
			{
				if (0 < this.mStateStack.get_Count())
				{
					State state = this.mStateStack.Pop();
					this.Notify(this.OnPop, state);
				}
			}

			public void ResumeState()
			{
				if (0 < this.mStateStack.get_Count())
				{
					this.Notify(this.OnResume, this.mStateStack.Peek());
					this.Notify(this.OnSwitch, this.mStateStack.Peek());
				}
			}

			public override string ToString()
			{
				this.mStateStack.ToArray();
				string text = string.Empty;
				using (Stack<State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						State current = enumerator.get_Current();
						text = current + " > " + text;
					}
				}
				return text;
			}

			private void Notify(Action<State> target, State state)
			{
				if (target != null)
				{
					target.Invoke(state);
				}
			}
		}

		public static class ReleaseUtils
		{
			public static void Release(ref UITexture uiTexture, bool unloadUnUsedAsset = false)
			{
				if (uiTexture != null)
				{
					if (uiTexture.mainTexture != null)
					{
						if (unloadUnUsedAsset)
						{
							Resources.UnloadAsset(uiTexture.mainTexture);
						}
						uiTexture.mainTexture = null;
					}
					uiTexture.RemoveFromPanel();
				}
				uiTexture = null;
			}

			public static void Release(ref UILabel uiLabel)
			{
				if (uiLabel != null)
				{
					uiLabel.text = string.Empty;
					uiLabel.RemoveFromPanel();
				}
				uiLabel = null;
			}

			public static void Release(ref Texture texture, bool unloadUnUsedAsset = false)
			{
				if (texture != null && unloadUnUsedAsset)
				{
					Resources.UnloadAsset(texture);
				}
				texture = null;
			}

			internal static void Release(ref UISprite uiSprite)
			{
				if (uiSprite != null)
				{
					uiSprite.spriteName = string.Empty;
					uiSprite.atlas = null;
					uiSprite.RemoveFromPanel();
				}
				uiSprite = null;
			}

			internal static void Release(ref UIPanel uiPanel)
			{
				uiPanel = null;
			}

			internal static void Release(ref UIWidget uiWidget)
			{
				if (uiWidget != null)
				{
					uiWidget.RemoveFromPanel();
				}
				uiWidget = null;
			}

			internal static void Releases(ref UISprite[] uiSprites)
			{
				if (uiSprites != null)
				{
					for (int i = 0; i < uiSprites.Length; i++)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref uiSprites[i]);
					}
				}
				uiSprites = null;
			}

			internal static void Releases(ref UIButton[] uiButtons)
			{
				if (uiButtons != null)
				{
					for (int i = 0; i < uiButtons.Length; i++)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref uiButtons[i]);
					}
				}
				uiButtons = null;
			}

			internal static void Release(ref UIButton uiButton)
			{
				if (uiButton != null)
				{
					uiButton.Release();
				}
				uiButton = null;
			}

			internal static void Release(ref UILabel[] uiLabels)
			{
				if (uiLabels != null)
				{
					for (int i = 0; i < uiLabels.Length; i++)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref uiLabels[i]);
					}
				}
			}

			internal static void Releases(ref Texture[] textures, bool unloadUnUsedAsset = false)
			{
				if (textures != null)
				{
					for (int i = 0; i < textures.Length; i++)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref textures[i], unloadUnUsedAsset);
					}
				}
				textures = null;
			}

			public static void Releases(ref GameObject[] gameObjects)
			{
				if (gameObjects != null)
				{
					for (int i = 0; i < gameObjects.Length; i++)
					{
						gameObjects[i] = null;
					}
				}
				gameObjects = null;
			}

			internal static void Release(ref AudioClip audioClip, bool unloadUnUsedAsset = false)
			{
				if (audioClip != null && unloadUnUsedAsset)
				{
					Resources.UnloadAsset(audioClip);
				}
				audioClip = null;
			}

			internal static void Releases(ref ParticleSystem[] particleSystems)
			{
				if (particleSystems != null)
				{
					for (int i = 0; i < particleSystems.Length; i++)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref particleSystems[i]);
					}
				}
				particleSystems = null;
			}

			internal static void Release(ref ParticleSystem particleSystem)
			{
				if (particleSystem != null)
				{
					Renderer component = particleSystem.GetComponent<Renderer>();
					if (component != null)
					{
						Material[] materials = component.get_materials();
						if (materials != null)
						{
							for (int i = 0; i < materials.Length; i++)
							{
								materials[i] = null;
							}
						}
					}
					Object.DestroyImmediate(particleSystem);
				}
				particleSystem = null;
			}

			public static void Release(ref Material material, bool immidiate = false)
			{
				material = null;
			}

			internal static void OverwriteCheck()
			{
			}

			internal static void Release(ref CommonShipBanner commonShipBanner, bool unloadAsset = false)
			{
				commonShipBanner.ReleaseShipBannerTexture(unloadAsset);
			}
		}

		private const string PREFAB_PATH_JUKEBOX_MANAGER = "Prefabs/JukeBox/UserInterfaceJukeBoxManager";

		private const string PREFAB_PATH_OPTION_MANAGER = "Prefabs/Others/Option";

		private const string PREFAB_PATH_COMMON_DIALOG_PORT = "Prefabs/Others/CommonDialogPort";

		private const string PREFAB_PATH_MARRIAGE_CUT = "Prefabs/PortTop/MarriageCut";

		private UserInterfacePortManager.StateManager<UserInterfacePortManager.State> mStateManager;

		[SerializeField]
		private StrategyShipCharacter mUIShipCharacter;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		[SerializeField]
		private UserInterfacePortMenuManager mUserInterfacePortMenuManager;

		[SerializeField]
		private Blur mBlur_Camera;

		[SerializeField]
		private Transform mTransform_LayerOverlay;

		[SerializeField]
		private Transform mTransform_LayerPort;

		[SerializeField]
		private UIInteriorFurniturePreviewWaiter mUIInteriorFurniturePreviewWaiter;

		[SerializeField]
		private UIPortCameraControlMode mUIPortCameraControlMode;

		[SerializeField]
		private Camera mCamera_Overlay;

		[SerializeField]
		private Camera mCamera_MenuCamera;

		private Option mUserInterfaceOptionManager;

		private CommonDialog mCommonDialog;

		private UIMarriageConfirm mUIMarriageConfirm;

		private UserInterfaceJukeBoxManager mUserInterfaceJukeBoxManager;

		private PortManager mPortManager;

		private DeckModel mDeckModel;

		private KeyControl mKeyController;

		private ParticleSystem mParticleSystem_MarriagePetal;

		private IEnumerator TutorialInstantiate;

		private UserInterfacePortManager.ShipDepth mShipDepth;

		private void Awake()
		{
			try
			{
				UICamera.mainCamera.GetComponent<UICamera>().allowMultiTouch = false;
			}
			catch (Exception)
			{
				Debug.Log("Not Found UICamera, Need Arrow MultiTouch = false (Xp)");
			}
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UserInterfacePortManager.<Start>c__IteratorA4 <Start>c__IteratorA = new UserInterfacePortManager.<Start>c__IteratorA4();
			<Start>c__IteratorA.<>f__this = this;
			return <Start>c__IteratorA;
		}

		private void OnFirstOpendListener()
		{
			IEnumerator enumerator = this.OnFirstOpendListenerCoroutine();
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator OnFirstOpendListenerCoroutine()
		{
			UserInterfacePortManager.<OnFirstOpendListenerCoroutine>c__IteratorA5 <OnFirstOpendListenerCoroutine>c__IteratorA = new UserInterfacePortManager.<OnFirstOpendListenerCoroutine>c__IteratorA5();
			<OnFirstOpendListenerCoroutine>c__IteratorA.<>f__this = this;
			return <OnFirstOpendListenerCoroutine>c__IteratorA;
		}

		private void OnCloseTutorialDialog()
		{
			this.mKeyController.IsRun = true;
			SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
			this.mUIShipCharacter.SetEnableBackTouch(true);
		}

		private void OnBackJukeBox()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfacePortManager.State.JukeBox;
			if (flag)
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnRequestJukeBoxEvent()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfacePortManager.State.Menu;
			if (flag)
			{
				bool flag2 = this.mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.MainMenu;
				flag2 |= (this.mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.SubMenu);
				flag &= flag2;
			}
			bool flag3 = flag || this.mStateManager.CurrentState == UserInterfacePortManager.State.PortViewer;
			if (flag3)
			{
				if (this.mStateManager.CurrentState == UserInterfacePortManager.State.Menu)
				{
					this.mUserInterfacePortMenuManager.StartWaitingState();
					this.mUserInterfacePortMenuManager.SetKeyController(null);
				}
				else if (this.mStateManager.CurrentState == UserInterfacePortManager.State.PortViewer)
				{
					this.mUIInteriorFurniturePreviewWaiter.StopWait();
					this.mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
					this.mUIPortCameraControlMode.SetKeyController(null);
				}
				this.mStateManager.PushState(UserInterfacePortManager.State.JukeBox);
			}
		}

		private void OnFinishedFurniturePreview()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIInteriorFurniturePreviewWaiter.StopWait();
			this.mUIInteriorFurniturePreviewWaiter.get_gameObject().SetActive(false);
			this.mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
			this.mUIPortCameraControlMode.ExitMode();
			this.mUIPortCameraControlMode.SetKeyController(null);
		}

		private void OnFinishedOfficeModeListener()
		{
			this.mUIInteriorFurniturePreviewWaiter.StopWait();
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(0f, 1f, 0.1f, delegate(float alpha)
			{
				this.mUserInterfacePortMenuManager.alpha = alpha;
				SingletonMonoBehaviour<UIPortFrame>.Instance.alpha = alpha;
			}), this);
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
			if (this.mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.MainMenu && SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
			}
		}

		private void OnArsenalSelectedListener(UIArsenalSelector.SelectType selectedType)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfacePortManager.State.ArsenalTypeSelect;
			if (flag)
			{
				if (selectedType != UIArsenalSelector.SelectType.Arsenal)
				{
					if (selectedType == UIArsenalSelector.SelectType.Revamp)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.ImprovementArsenal);
					}
				}
				else
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Arsenal, true);
				}
				this.mStateManager.PushState(UserInterfacePortManager.State.NONE);
			}
		}

		private void OnSelectedSceneListener(Generics.Scene selectedScene)
		{
			if (this.mStateManager.CurrentState != UserInterfacePortManager.State.Menu)
			{
				return;
			}
			this.ChangeShipDepth(UserInterfacePortManager.ShipDepth.Default);
			if (selectedScene != Generics.Scene.Option)
			{
				if (selectedScene != Generics.Scene.Marriage)
				{
					if (SingletonMonoBehaviour<UIPortFrame>.exist())
					{
						SingletonMonoBehaviour<UIPortFrame>.Instance.ReqFrame(false);
					}
				}
			}
			switch (selectedScene)
			{
			case Generics.Scene.Arsenal:
			{
				bool flag = this.mDeckModel.GetFlagShip().ShipType == 19;
				if (flag)
				{
					this.mStateManager.PushState(UserInterfacePortManager.State.ArsenalTypeSelect);
				}
				else
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(selectedScene, false);
				}
				return;
			}
			case Generics.Scene.ImprovementArsenal:
			case Generics.Scene.Duty:
			case Generics.Scene.Record:
			case Generics.Scene.InheritSave:
			case Generics.Scene.InheritLoad:
				IL_98:
				if (selectedScene != Generics.Scene.Marriage)
				{
					bool flag2 = PortObjectManager.isPrefabSecene(selectedScene);
					if (flag2)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(selectedScene, false);
					}
					else
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(selectedScene);
					}
					return;
				}
				this.mStateManager.PushState(UserInterfacePortManager.State.MarriageConfirm);
				return;
			case Generics.Scene.Item:
			case Generics.Scene.Interior:
			case Generics.Scene.Album:
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(selectedScene);
				return;
			case Generics.Scene.Option:
				this.mStateManager.PushState(UserInterfacePortManager.State.Option);
				return;
			case Generics.Scene.SaveLoad:
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rootType", Generics.Scene.Port);
				RetentionData.SetData(hashtable);
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(selectedScene);
				return;
			}
			case Generics.Scene.Strategy:
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.3f, delegate
				{
					this.UnLoadUnUsedAssets(delegate
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Strategy, false);
					});
				});
				return;
			}
			goto IL_98;
		}

		private void UnLoadUnUsedAssets(Action onFinished)
		{
			IEnumerator enumerator = this.UnLoadUnUsedAssetsCoroutine(onFinished);
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator UnLoadUnUsedAssetsCoroutine(Action onFinished)
		{
			UserInterfacePortManager.<UnLoadUnUsedAssetsCoroutine>c__IteratorA6 <UnLoadUnUsedAssetsCoroutine>c__IteratorA = new UserInterfacePortManager.<UnLoadUnUsedAssetsCoroutine>c__IteratorA6();
			<UnLoadUnUsedAssetsCoroutine>c__IteratorA.onFinished = onFinished;
			<UnLoadUnUsedAssetsCoroutine>c__IteratorA.<$>onFinished = onFinished;
			return <UnLoadUnUsedAssetsCoroutine>c__IteratorA;
		}

		private void OnPushMarriageConfirmState()
		{
			base.StartCoroutine(this.OnPushMarriageConfirmStateCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushMarriageConfirmStateCoroutine()
		{
			UserInterfacePortManager.<OnPushMarriageConfirmStateCoroutine>c__IteratorA7 <OnPushMarriageConfirmStateCoroutine>c__IteratorA = new UserInterfacePortManager.<OnPushMarriageConfirmStateCoroutine>c__IteratorA7();
			<OnPushMarriageConfirmStateCoroutine>c__IteratorA.<>f__this = this;
			return <OnPushMarriageConfirmStateCoroutine>c__IteratorA;
		}

		private void OnCancelMarriageConfirm()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUserInterfacePortMenuManager.ResumeState();
			this.mCommonDialog.CloseDialog();
			this.mStateManager.PopState();
		}

		private void OnStartMarriageConfirm()
		{
			this.mPortManager.Marriage(this.mDeckModel.GetFlagShip().MemId);
			this.mCommonDialog.CloseDialog();
			this.mStateManager.PopState();
			this.mStateManager.PushState(UserInterfacePortManager.State.MarriageProduction);
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				if (this.mKeyController.keyState.get_Item(3).down)
				{
					bool flag = this.mStateManager.CurrentState == UserInterfacePortManager.State.Menu;
					if (flag)
					{
						this.mKeyController.ClearKeyAll();
						this.mKeyController.firstUpdate = true;
						this.RequestPortViewrMode();
					}
				}
				else if (this.mKeyController.keyState.get_Item(2).down)
				{
					bool flag2 = this.mUserInterfacePortInteriorManager.IsConfigureJukeBox();
					if (flag2)
					{
						this.OnRequestJukeBoxEvent();
					}
				}
				else if (this.mKeyController.keyState.get_Item(2).down)
				{
				}
			}
		}

		private void OnPlayMarriageProduction()
		{
		}

		private void OnCancelMarriage()
		{
		}

		private void OnPopState(UserInterfacePortManager.State state)
		{
			switch (state)
			{
			case UserInterfacePortManager.State.JukeBox:
				this.mUserInterfaceJukeBoxManager.SetKeyController(null);
				this.mUserInterfaceJukeBoxManager.CloseState();
				break;
			case UserInterfacePortManager.State.PortViewer:
				this.mUIPortCameraControlMode.SetKeyController(null);
				break;
			}
		}

		private void OnResumeState(UserInterfacePortManager.State state)
		{
			switch (state)
			{
			case UserInterfacePortManager.State.Menu:
				this.mUserInterfacePortMenuManager.SetKeyController(this.mKeyController);
				this.mUserInterfacePortMenuManager.ResumeState();
				break;
			case UserInterfacePortManager.State.PortViewer:
				this.mUserInterfacePortMenuManager.alpha = 0f;
				SingletonMonoBehaviour<UIPortFrame>.Instance.alpha = 0f;
				this.mUIInteriorFurniturePreviewWaiter.get_gameObject().SetActive(true);
				this.mUIInteriorFurniturePreviewWaiter.SetKeyController(this.mKeyController);
				this.mUIPortCameraControlMode.SetKeyController(this.mKeyController);
				this.mUIInteriorFurniturePreviewWaiter.ResumeWait();
				break;
			}
		}

		private List<UserInterfacePortManager.EngageValidation> EngegeCheck(PortManager portManager, ShipModel shipModel)
		{
			List<UserInterfacePortManager.EngageValidation> list = new List<UserInterfacePortManager.EngageValidation>();
			if (this.mPortManager.YubiwaNum <= 0)
			{
				list.Add(UserInterfacePortManager.EngageValidation.NoYubiwa);
			}
			if (shipModel.IsInRepair())
			{
				list.Add(UserInterfacePortManager.EngageValidation.InRepair);
			}
			return list;
		}

		private void OnPushState(UserInterfacePortManager.State state)
		{
			switch (state)
			{
			case UserInterfacePortManager.State.Menu:
				this.OnPushMenuState();
				break;
			case UserInterfacePortManager.State.FirstOpenMenu:
				this.OnPushFirstOpenMenuState();
				break;
			case UserInterfacePortManager.State.JukeBox:
				this.OnPushJukeBoxState();
				break;
			case UserInterfacePortManager.State.PortViewer:
				this.OnPushPortViewerState();
				break;
			case UserInterfacePortManager.State.MarriageConfirm:
			{
				bool flag = this.mPortManager.IsValidMarriage(this.mDeckModel.GetFlagShip().MemId);
				if (flag)
				{
					List<UserInterfacePortManager.EngageValidation> list = this.EngegeCheck(this.mPortManager, this.mDeckModel.GetFlagShip());
					if (list.get_Count() <= 0)
					{
						this.OnPushMarriageConfirmState();
					}
					else
					{
						UserInterfacePortManager.EngageValidation engageValidation = list.get_Item(0);
						if (engageValidation != UserInterfacePortManager.EngageValidation.NoYubiwa)
						{
							if (engageValidation == UserInterfacePortManager.EngageValidation.InRepair)
							{
								CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NowRepairing));
							}
						}
						else
						{
							CommonPopupDialog.Instance.StartPopup("ケッコン指輪が必要です");
						}
						this.mKeyController.ClearKeyAll();
						this.mKeyController.firstUpdate = true;
						this.mStateManager.PopState();
						this.mStateManager.ResumeState();
					}
				}
				break;
			}
			case UserInterfacePortManager.State.MarriageProduction:
				this.OnPushMarriageProductionState();
				break;
			case UserInterfacePortManager.State.Option:
				this.OnPushOptionState();
				break;
			case UserInterfacePortManager.State.ArsenalTypeSelect:
				this.OnPushArsenalTypeSelectState();
				break;
			}
		}

		private void OnPushFirstOpenMenuState()
		{
			base.StartCoroutine(this.OnPushFirstOpenMenuStateCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushFirstOpenMenuStateCoroutine()
		{
			UserInterfacePortManager.<OnPushFirstOpenMenuStateCoroutine>c__IteratorA8 <OnPushFirstOpenMenuStateCoroutine>c__IteratorA = new UserInterfacePortManager.<OnPushFirstOpenMenuStateCoroutine>c__IteratorA8();
			<OnPushFirstOpenMenuStateCoroutine>c__IteratorA.<>f__this = this;
			return <OnPushFirstOpenMenuStateCoroutine>c__IteratorA;
		}

		private void OnPushJukeBoxState()
		{
			base.StartCoroutine(this.OnPushJukeBoxStateCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushJukeBoxStateCoroutine()
		{
			UserInterfacePortManager.<OnPushJukeBoxStateCoroutine>c__IteratorA9 <OnPushJukeBoxStateCoroutine>c__IteratorA = new UserInterfacePortManager.<OnPushJukeBoxStateCoroutine>c__IteratorA9();
			<OnPushJukeBoxStateCoroutine>c__IteratorA.<>f__this = this;
			return <OnPushJukeBoxStateCoroutine>c__IteratorA;
		}

		private void OnPushMenuState()
		{
			if (this.mDeckModel.GetFlagShip().IsMarriage())
			{
				if (this.mParticleSystem_MarriagePetal == null)
				{
					GameObject original = Resources.Load("Prefabs/Others/MarriagePetal") as GameObject;
					this.mParticleSystem_MarriagePetal = Util.Instantiate(original, this.mTransform_LayerPort.get_gameObject(), false, false).GetComponent<ParticleSystem>();
				}
				this.mParticleSystem_MarriagePetal.Stop();
				this.mParticleSystem_MarriagePetal.Play();
			}
			this.mUserInterfacePortMenuManager.SetKeyController(this.mKeyController);
		}

		private void OnPushArsenalTypeSelectState()
		{
			IEnumerator enumerator = this.OnPushArsenalTypeSelectStateCoroutine();
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator OnPushArsenalTypeSelectStateCoroutine()
		{
			UserInterfacePortManager.<OnPushArsenalTypeSelectStateCoroutine>c__IteratorAA <OnPushArsenalTypeSelectStateCoroutine>c__IteratorAA = new UserInterfacePortManager.<OnPushArsenalTypeSelectStateCoroutine>c__IteratorAA();
			<OnPushArsenalTypeSelectStateCoroutine>c__IteratorAA.<>f__this = this;
			return <OnPushArsenalTypeSelectStateCoroutine>c__IteratorAA;
		}

		private void OnPushOptionState()
		{
			base.StartCoroutine(this.OnPushOptionStateCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushOptionStateCoroutine()
		{
			UserInterfacePortManager.<OnPushOptionStateCoroutine>c__IteratorAB <OnPushOptionStateCoroutine>c__IteratorAB = new UserInterfacePortManager.<OnPushOptionStateCoroutine>c__IteratorAB();
			<OnPushOptionStateCoroutine>c__IteratorAB.<>f__this = this;
			return <OnPushOptionStateCoroutine>c__IteratorAB;
		}

		private void OnPushMarriageProductionState()
		{
			base.StartCoroutine(this.OnPushMarriageProductionStateCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushMarriageProductionStateCoroutine()
		{
			UserInterfacePortManager.<OnPushMarriageProductionStateCoroutine>c__IteratorAC <OnPushMarriageProductionStateCoroutine>c__IteratorAC = new UserInterfacePortManager.<OnPushMarriageProductionStateCoroutine>c__IteratorAC();
			<OnPushMarriageProductionStateCoroutine>c__IteratorAC.<>f__this = this;
			return <OnPushMarriageProductionStateCoroutine>c__IteratorAC;
		}

		private void _compMarriageAnimation()
		{
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(1f, delegate
			{
				TrophyUtil.Unlock_At_Marriage();
				Application.LoadLevel(Generics.Scene.PortTop.ToString());
			});
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchHideMenu()
		{
			if (this.mStateManager.CurrentState == UserInterfacePortManager.State.Menu)
			{
				this.RequestPortViewrMode();
			}
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchFrontDepthChange()
		{
			bool flag = this.mUserInterfacePortMenuManager.GetCurrentState() != UserInterfacePortMenuManager.State.CallingNextScene;
			if (flag)
			{
				UserInterfacePortManager.ShipDepth shipDepth = this.mShipDepth;
				if (shipDepth != UserInterfacePortManager.ShipDepth.Default)
				{
					if (shipDepth == UserInterfacePortManager.ShipDepth.HigherPortFrame)
					{
						this.ChangeShipDepth(UserInterfacePortManager.ShipDepth.Default);
					}
				}
				else
				{
					this.ChangeShipDepth(UserInterfacePortManager.ShipDepth.HigherPortFrame);
				}
			}
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchShowMenu()
		{
		}

		private void RequestPortViewrMode()
		{
			bool flag = this.mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.MainMenu;
			flag |= (this.mUserInterfacePortMenuManager.GetCurrentState() == UserInterfacePortMenuManager.State.SubMenu);
			if (flag)
			{
				this.mStateManager.PushState(UserInterfacePortManager.State.PortViewer);
			}
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
			}
		}

		private void OnPushPortViewerState()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.mUserInterfacePortMenuManager.alpha = 0f;
			SingletonMonoBehaviour<UIPortFrame>.Instance.alpha = 0f;
			this.mUIInteriorFurniturePreviewWaiter.get_gameObject().SetActive(true);
			this.mUserInterfacePortMenuManager.StartWaitingState();
			this.mUserInterfacePortMenuManager.SetKeyController(null);
			this.mUIInteriorFurniturePreviewWaiter.SetKeyController(this.mKeyController);
			this.mUIInteriorFurniturePreviewWaiter.StartWait();
			this.mUIPortCameraControlMode.Init();
			this.mUIPortCameraControlMode.SetKeyController(this.mKeyController);
		}

		private void OnBackOptionState()
		{
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this.mUserInterfaceOptionManager.get_gameObject(), 0.01f, 0f);
			this.mUserInterfaceOptionManager.SetKeyController(null);
			this.mUserInterfaceOptionManager.get_gameObject().SetActive(false);
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void PlayPortInVoice(ShipModelMst shipModel)
		{
			if (App.rand.Next(0, 2) == 0)
			{
				int voiceNum = App.rand.Next(2, 4);
				ShipUtils.PlayShipVoice(shipModel, voiceNum);
			}
		}

		private IEnumerator TutorialCheck(Action OnFinish)
		{
			TutorialModel tutorial = this.mPortManager.UserInfo.Tutorial;
			bool[] array = new bool[]
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.PortTopText),
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.RepairInfo),
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.SupplyInfo)
			};
			if (array[0] || array[1] || array[2])
			{
				return this.StepTutorialInstantiateForWaitFirstTutorial(tutorial, OnFinish);
			}
			this.StepTutorialInstantiate(tutorial);
			if (OnFinish != null)
			{
				OnFinish.Invoke();
			}
			return null;
		}

		private void ChangeShipDepth(UserInterfacePortManager.ShipDepth shipDepth)
		{
			this.mShipDepth = shipDepth;
			UserInterfacePortManager.ShipDepth shipDepth2 = this.mShipDepth;
			if (shipDepth2 != UserInterfacePortManager.ShipDepth.Default)
			{
				if (shipDepth2 == UserInterfacePortManager.ShipDepth.HigherPortFrame)
				{
					this.mCamera_MenuCamera.set_depth(2f);
				}
			}
			else
			{
				this.mCamera_MenuCamera.set_depth(1.2f);
			}
		}

		[DebuggerHidden]
		private IEnumerator StepTutorialInstantiateForWaitFirstTutorial(TutorialModel model, Action OnFinish)
		{
			UserInterfacePortManager.<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD <StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD = new UserInterfacePortManager.<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD();
			<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD.model = model;
			<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD.OnFinish = OnFinish;
			<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD.<$>model = model;
			<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD.<$>OnFinish = OnFinish;
			<StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD.<>f__this = this;
			return <StepTutorialInstantiateForWaitFirstTutorial>c__IteratorAD;
		}

		private bool StepTutorialInstantiate(TutorialModel model)
		{
			if (model.GetStep() == 0 && !model.GetStepTutorialFlg(1))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide2"), base.get_gameObject(), false, false);
				return false;
			}
			if (model.GetStep() == 1 && !model.GetStepTutorialFlg(2))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide3"), base.get_gameObject(), false, false);
				return false;
			}
			if (model.GetStep() == 2 && !model.GetStepTutorialFlg(3))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide3_2"), base.get_gameObject(), false, false);
				return false;
			}
			if (model.GetStep() == 3 && !model.GetStepTutorialFlg(4))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide4"), base.get_gameObject(), false, false);
				return false;
			}
			if (model.GetStep() == 4 && !model.GetStepTutorialFlg(5))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide5"), base.get_gameObject(), false, false);
				return false;
			}
			if (model.GetStep() == 6 && !model.GetStepTutorialFlg(7))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide7_2"), base.get_gameObject(), false, false);
				return false;
			}
			if (model.GetStep() == 7 && !model.GetStepTutorialFlg(8))
			{
				Util.Instantiate(Resources.Load("Prefabs/TutorialGuide/TutorialGuide8_port"), base.get_gameObject(), false, false);
				return false;
			}
			return true;
		}

		private void OnDestroy()
		{
			this.mStateManager = null;
			this.mUIShipCharacter = null;
			this.mUserInterfacePortInteriorManager = null;
			this.mUserInterfacePortMenuManager = null;
			this.mBlur_Camera = null;
			if (this.mParticleSystem_MarriagePetal != null)
			{
				this.mParticleSystem_MarriagePetal.Stop();
			}
			this.mParticleSystem_MarriagePetal = null;
			this.mTransform_LayerPort = null;
			this.mTransform_LayerOverlay = null;
			this.mUIInteriorFurniturePreviewWaiter = null;
			this.mUIPortCameraControlMode = null;
			this.mCamera_Overlay = null;
			this.mCamera_MenuCamera = null;
			this.mUserInterfaceOptionManager = null;
			this.mCommonDialog = null;
			this.mUIMarriageConfirm = null;
			this.mUserInterfaceJukeBoxManager = null;
			this.mPortManager = null;
			this.mDeckModel = null;
			this.mKeyController = null;
			this.TutorialInstantiate = null;
		}
	}
}
