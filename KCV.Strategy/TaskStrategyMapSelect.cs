using Common.Enum;
using DG.Tweening;
using KCV.Display;
using KCV.Scene.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyMapSelect : SceneTaskMono, RouletteSelectorHandler
	{
		private const float ANIMATION_TIME = 0.3f;

		[SerializeField]
		private Camera mCamera;

		[SerializeField]
		private Transform mStageSelectRoot;

		[SerializeField]
		private UIStageCover mPrefab_UIStateCover;

		[SerializeField]
		private MapTransitionCutManager mPrefab_MapTransitionCutManager;

		[SerializeField]
		private UIStageConfirm mStageConfirm;

		[SerializeField]
		private Transform mTransform_StageCovers;

		[SerializeField]
		private RouletteSelector mRouletteSelector;

		[SerializeField]
		private UIDisplaySwipeEventRegion mDisplaySwipeRegion;

		[SerializeField]
		private UIGoSortieConfirm DeckInfoConfirm;

		[SerializeField]
		private CommonDialog commonDialog;

		public UnloadAtlas Unloader;

		private Transform mTransform_AnimationTile;

		[SerializeField]
		private UITexture mTexture_sallyBGsky;

		[SerializeField]
		private UITexture mTexture_sallyBGclouds;

		[SerializeField]
		private UITexture mTexture_sallyBGcloudRefl;

		[SerializeField]
		private UITexture mTexture_bgSea;

		[SerializeField]
		private UITexture mTexture_snow;

		private StrategyTopTaskManager mStrategyTopTaskManager;

		private Animation mAnimation_MapObjects;

		private AsyncOperation mAsyncOperation;

		private SortieManager mSortieManager;

		private KeyControl mKeyController;

		private MapModel[] mMapModels;

		private bool mIsFinishedAnimation;

		private int mAreaId;

		[SerializeField]
		private int _CenterIndex;

		private Tweener mTweener;

		private bool isAnimationStarted;

		private int moveEndCount;

		public int CenterIndex
		{
			get
			{
				return this._CenterIndex;
			}
			set
			{
				this._CenterIndex = value;
			}
		}

		bool RouletteSelectorHandler.IsSelectable(int index)
		{
			return true;
		}

		void RouletteSelectorHandler.OnUpdateIndex(int index, Transform transform)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Index:",
				index,
				" Model",
				this.mMapModels[index].Name
			}));
			this.mStageConfirm.Initialize(this.mMapModels[index]);
			if (!this.mAnimation_MapObjects.get_isPlaying())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			}
		}

		void RouletteSelectorHandler.OnSelect(int index, Transform transform)
		{
			Debug.Log("RouletteSelectorHandler.OnSelect index:" + index);
		}

		protected override bool Init()
		{
			this.isAnimationStarted = false;
			this.mDisplaySwipeRegion.SetEventCatchCamera(this.mCamera);
			this.mDisplaySwipeRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.OnSwipeAction));
			this.mStageSelectRoot.set_localPosition(new Vector3(this.mCamera.get_transform().get_localPosition().x, this.mCamera.get_transform().get_localPosition().y));
			this.mAreaId = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
			this.mStrategyTopTaskManager = StrategyTaskManager.GetStrategyTop();
			this.mIsFinishedAnimation = false;
			this.mMapModels = StrategyTopTaskManager.GetLogicManager().SelectArea(this.mAreaId).Maps;
			this.mSortieManager = StrategyTopTaskManager.GetLogicManager().SelectArea(this.mAreaId);
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.mKeyController.isLoopIndex = true;
			this.mKeyController.IsRun = false;
			TweenAlpha.Begin(GameObject.Find("Information Root"), 0.3f, 0f);
			TweenAlpha.Begin(GameObject.Find("Map_BG"), 0.3f, 0f);
			GameObject gameObject = StrategyTopTaskManager.Instance.TileManager.Tiles[this.mAreaId].getSprite().get_gameObject();
			this.mTransform_AnimationTile = Util.Instantiate(gameObject, GameObject.Find("Map Root").get_gameObject(), true, false).get_transform();
			this.mAnimation_MapObjects = this.mTransform_StageCovers.GetComponent<Animation>();
			base.StartCoroutine(this.StartSeaAnimationCoroutine());
			IEnumerator enumerator = this.StartSeaAnimationCoroutine();
			base.StartCoroutine(enumerator);
			this.mTransform_StageCovers.SetActive(true);
			this.mTransform_StageCovers.Find("UIStageCovers").GetComponent<UIWidget>().alpha = 0.001f;
			this.SelectedHexAnimation(delegate
			{
				base.StartCoroutine(this.InititalizeStageCovers(delegate
				{
					this.mTransform_StageCovers.GetComponent<Animation>().Play("SortieAnimation");
					this.ShowMaps(this.mMapModels);
				}));
			});
			if (this.mAreaId == 2 || this.mAreaId == 4 || this.mAreaId == 5 || this.mAreaId == 6 || this.mAreaId == 7 || this.mAreaId == 10 || this.mAreaId == 14)
			{
				this.mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_sky") as Texture);
				this.mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.height = 91;
				this.mTexture_sallyBGcloudRefl.alpha = 0.25f;
				this.mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea2_Sunny_sea") as Texture);
				this.mTexture_snow.mainTexture = null;
			}
			else if (this.mAreaId == 3 || this.mAreaId == 13)
			{
				this.mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_sky") as Texture);
				this.mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.height = 90;
				this.mTexture_sallyBGcloudRefl.alpha = 0.75f;
				this.mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea3_Sunny_sea") as Texture);
				this.mTexture_snow.mainTexture = (Resources.Load("Textures/Strategy/sea3_snow") as Texture);
			}
			else if (this.mAreaId == 15 || this.mAreaId == 16 || this.mAreaId == 17)
			{
				this.mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_sky2") as Texture);
				this.mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.height = 120;
				this.mTexture_sallyBGcloudRefl.alpha = 0.25f;
				this.mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea4_Sunny_sea") as Texture);
				this.mTexture_snow.mainTexture = null;
			}
			else
			{
				this.mTexture_sallyBGsky.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_sky") as Texture);
				this.mTexture_sallyBGclouds.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_clouds") as Texture);
				this.mTexture_sallyBGcloudRefl.height = 140;
				this.mTexture_sallyBGcloudRefl.alpha = 0.25f;
				this.mTexture_bgSea.mainTexture = (Resources.Load("Textures/Strategy/sea1_Sunny_sea") as Texture);
				this.mTexture_snow.mainTexture = null;
			}
			return true;
		}

		private void OnSwipeAction(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				if (!this.mStageConfirm.Shown)
				{
					if (40f < deltaX)
					{
						this.mRouletteSelector.MovePrev();
					}
					else if (deltaX < -40f)
					{
						this.mRouletteSelector.MoveNext();
					}
					else if (deltaY < -40f)
					{
					}
				}
			}
		}

		[DebuggerHidden]
		private IEnumerator StartSeaAnimationCoroutine()
		{
			TaskStrategyMapSelect.<StartSeaAnimationCoroutine>c__Iterator190 <StartSeaAnimationCoroutine>c__Iterator = new TaskStrategyMapSelect.<StartSeaAnimationCoroutine>c__Iterator190();
			<StartSeaAnimationCoroutine>c__Iterator.<>f__this = this;
			return <StartSeaAnimationCoroutine>c__Iterator;
		}

		private void SelectedHexAnimation(Action onFinishedCallBack)
		{
			TweenPosition tweenPosition = TweenPosition.Begin(this.mTransform_AnimationTile.get_gameObject(), 0.3f, this.mStrategyTopTaskManager.strategyCamera.get_transform().get_localPosition() + new Vector3(0f, -78f, 196f));
			TweenRotation tweenRotation = TweenRotation.Begin(this.mTransform_AnimationTile.get_gameObject(), 0.3f, Quaternion.AngleAxis(90f, Vector3.get_right()));
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this.mTransform_AnimationTile.get_gameObject(), 0.8f, 0f);
			tweenPosition.SetOnFinished(delegate
			{
				if (onFinishedCallBack != null)
				{
					onFinishedCallBack.Invoke();
				}
			});
		}

		[DebuggerHidden]
		private IEnumerator InititalizeStageCovers(Action onInitialized)
		{
			TaskStrategyMapSelect.<InititalizeStageCovers>c__Iterator191 <InititalizeStageCovers>c__Iterator = new TaskStrategyMapSelect.<InititalizeStageCovers>c__Iterator191();
			<InititalizeStageCovers>c__Iterator.onInitialized = onInitialized;
			<InititalizeStageCovers>c__Iterator.<$>onInitialized = onInitialized;
			<InititalizeStageCovers>c__Iterator.<>f__this = this;
			return <InititalizeStageCovers>c__Iterator;
		}

		private void ShowMaps(MapModel[] MapModels)
		{
			MapModel mapModel = Enumerable.LastOrDefault<MapModel>(MapModels, (MapModel x) => x.Map_Possible);
			int current = (mapModel == null) ? 0 : (mapModel.No - 1);
			this.mRouletteSelector.Init(this);
			this.mRouletteSelector.SetCurrent(current);
			this.mRouletteSelector.Reposition();
			this.mRouletteSelector.SetKeyController(this.mKeyController);
			this.mRouletteSelector.ScaleForce(0.3f, 1f);
			this.mRouletteSelector.controllable = true;
		}

		protected override bool UnInit()
		{
			base.StopCoroutine(this.StartSeaAnimationCoroutine());
			return true;
		}

		private void UpdateDescription(MapModel mapModel)
		{
			this.mStageConfirm.Initialize(mapModel);
		}

		private void OnSelectStageCover()
		{
			Transform transform = this.mTransform_StageCovers.Find("UIStageCovers");
			ShortcutExtensions.DOLocalMoveY(transform, 100f, 0.3f, false);
			Debug.Log("Shown:Show");
			this.mRouletteSelector.ScaleForce(0.3f, 0f);
			this.mRouletteSelector.controllable = false;
			this.mStageConfirm.Show();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void OnTouchSelectStageCover()
		{
			bool flag = !this.mStageConfirm.Shown && this.mKeyController.IsRun;
			if (flag)
			{
				this.OnSelectStageCover();
			}
		}

		private void OnBackSelectStageCover()
		{
			Transform transform = this.mTransform_StageCovers.Find("UIStageCovers");
			TweenExtensions.PlayForward(ShortcutExtensions.DOLocalMoveY(transform, 0f, 0.3f, false));
			this.mStageConfirm.Hide();
		}

		public void OnTouchBG()
		{
			if (!this.OnKeyControlCross() && this.mKeyController.IsRun)
			{
				base.Close();
			}
		}

		private bool OnKeyControlCircle()
		{
			if (this.mStageConfirm.Shown && this.mStageConfirm.mMapModel != null && this.mStageConfirm.mMapModel.Map_Possible)
			{
				List<IsGoCondition> list = this.mSortieManager.IsGoSortie(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID, this.mStageConfirm.mMapModel.MstId);
				if (list.get_Count() != 0)
				{
					CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(list.get_Item(0)));
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
					return true;
				}
				this.OpenDeckInfo();
			}
			else if (!this.mStageConfirm.Shown)
			{
				this.OnSelectStageCover();
			}
			return true;
		}

		private void OpenDeckInfo()
		{
			this.DeckInfoConfirm.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f));
			this.commonDialog.setOpenAction(delegate
			{
				this.DeckInfoConfirm.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f));
			});
			this.commonDialog.OpenDialog(2, DialogAnimation.AnimType.FEAD);
			this.DeckInfoConfirm.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck, true);
			this.DeckInfoConfirm.SetPushYesButton(delegate
			{
				this.mKeyController.IsRun = false;
				this.commonDialog.CloseDialog();
				this.OnStartSortieStage();
				base.Close();
			});
			this.DeckInfoConfirm.SetPushNoButton(delegate
			{
				this.commonDialog.CloseDialog();
			});
		}

		private bool OnStartSortieStage()
		{
			this.mKeyController.IsRun = false;
			this.mKeyController.ClearKeyAll();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SingletonMonoBehaviour<Live2DModel>.Instance.forceStop();
			SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
			SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation = true;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
			DebugUtils.SLog("OnStartSortieStage1");
			this.mStageConfirm.Hide();
			DebugUtils.SLog("OnStartSortieStage2");
			TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mRouletteSelector.get_transform(), 0f, 0.4f, false), 30)), ShortcutExtensions.DOScale(this.mRouletteSelector.get_transform(), new Vector3(1.6f, 1.6f, 1f), 0.3f));
			ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip(), App.rand.Next(13, 15));
			DebugUtils.SLog("OnStartSortieStage3");
			this.DelayAction(0.5f, delegate
			{
				DebugUtils.SLog("OnStartSortieStage mStageConfirm.ClickAnimation");
				MapModel mMapModel = this.mStageConfirm.mMapModel;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("sortieMapManager", this.mSortieManager.GoSortie(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id, mMapModel.MstId));
				hashtable.Add("rootType", 0);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
				hashtable.Add("escape", false);
				RetentionData.SetData(hashtable);
				Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.get_gameObject());
				Object.Destroy(GameObject.Find("Information Root"));
				Object.Destroy(GameObject.Find("OverView"));
				base.StartCoroutine(this.AsyncLoad());
				MapTransitionCutManager component = Util.Instantiate(this.mPrefab_MapTransitionCutManager.get_gameObject(), base.get_transform().get_root().Find("Map Root").get_gameObject(), false, false).GetComponent<MapTransitionCutManager>();
				component.get_transform().set_localPosition(this.mStrategyTopTaskManager.strategyCamera.get_transform().get_localPosition() + new Vector3(-26.4f, -43f, 496.4f));
				component.Initialize(mMapModel, this.mAsyncOperation);
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
				DebugUtils.SLog("OnStartSortieStage mStageConfirm.ClickAnimation END");
			});
			DebugUtils.SLog("OnStartSortieStage4");
			return false;
		}

		private bool OnKeyControlCross()
		{
			if (!this.mKeyController.IsRun)
			{
				return false;
			}
			if (this.mStageConfirm.Shown)
			{
				Debug.Log("Shown:Hide");
				this.mRouletteSelector.controllable = true;
				this.mRouletteSelector.ScaleForce(0.3f, 1f);
				this.OnBackSelectStageCover();
				return true;
			}
			Object.Destroy(this.mTransform_AnimationTile.get_gameObject());
			this.mTransform_StageCovers.SetActive(false);
			TweenAlpha.Begin(GameObject.Find("Information Root"), 0.3f, 1f);
			TweenAlpha.Begin(GameObject.Find("Map_BG"), 0.3f, 1f);
			Transform transform = this.mTransform_StageCovers.Find("UIStageCovers");
			using (IEnumerator enumerator = transform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform2 = (Transform)enumerator.get_Current();
					Object.Destroy(transform2.get_gameObject());
				}
			}
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(true, null);
			StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(true);
			List<int> openTileIDs = StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.CreateOpenTileIDs();
			StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.UpdateTileRouteState(openTileIDs);
			StrategyTopTaskManager.Instance.TileManager.SetVisible(true);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
			return false;
		}

		public void OnTouchStartSortie()
		{
			if (this.mRouletteSelector.controllable)
			{
				return;
			}
			if (this.commonDialog.isOpen)
			{
				return;
			}
			if (!this.OnKeyControlCircle())
			{
				base.Close();
			}
		}

		protected override bool Run()
		{
			if (this.mAnimation_MapObjects.IsPlaying("SortieAnimation"))
			{
				this.isAnimationStarted = true;
				return true;
			}
			if (!this.mIsFinishedAnimation)
			{
				if (!this.isAnimationStarted)
				{
					return true;
				}
				this.AnimFinished();
			}
			this.mKeyController.Update();
			if (Input.get_anyKey())
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					return this.OnKeyControlCircle();
				}
				if (this.mKeyController.keyState.get_Item(0).down)
				{
					return this.OnKeyControlCross();
				}
				if (!this.mKeyController.keyState.get_Item(3).down)
				{
					if (this.mKeyController.keyState.get_Item(5).down)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
					}
				}
			}
			return true;
		}

		private void moveEnd()
		{
			this.moveEndCount++;
		}

		[DebuggerHidden]
		private IEnumerator AsyncLoad()
		{
			TaskStrategyMapSelect.<AsyncLoad>c__Iterator192 <AsyncLoad>c__Iterator = new TaskStrategyMapSelect.<AsyncLoad>c__Iterator192();
			<AsyncLoad>c__Iterator.<>f__this = this;
			return <AsyncLoad>c__Iterator;
		}

		public void AnimFinished()
		{
			this.mIsFinishedAnimation = true;
			this.mKeyController.IsRun = true;
		}

		private void OnDestroy()
		{
			this.mPrefab_MapTransitionCutManager = null;
			this.mPrefab_UIStateCover = null;
			this.mTexture_sallyBGsky.mainTexture = null;
			this.mTexture_sallyBGclouds.mainTexture = null;
			this.mTexture_sallyBGcloudRefl.mainTexture = null;
			this.mTexture_bgSea.mainTexture = null;
			this.mTexture_snow.mainTexture = null;
		}
	}
}
