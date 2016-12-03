using Common.Enum;
using DG.Tweening;
using KCV.Dialog;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UserInterfaceRevampManager : MonoBehaviour
	{
		public class RevampContext
		{
			public class SlotItemInfo
			{
				public int MstId
				{
					get;
					private set;
				}

				public int MemId
				{
					get;
					private set;
				}

				public int Level
				{
					get;
					private set;
				}

				public string Name
				{
					get;
					private set;
				}

				public SlotItemInfo(SlotitemModel slotItemModel)
				{
					if (slotItemModel != null)
					{
						this.MstId = slotItemModel.MstId;
						this.Level = slotItemModel.Level;
						this.MemId = slotItemModel.MemId;
						this.Name = slotItemModel.Name;
					}
					else
					{
						this.MstId = -1;
						this.Level = -1;
					}
				}
			}

			public const int UNSET = -1;

			private UserInterfaceRevampManager.RevampContext.SlotItemInfo mBeforeSlotItemInfo;

			private UserInterfaceRevampManager.RevampContext.SlotItemInfo mAfterSlotItemInfo;

			private RevampRecipeModel mRevampRecipe;

			private ShipModel mConsortShip;

			private bool mIsDetermined;

			public RevampRecipeModel RevampRecipe
			{
				get
				{
					return this.mRevampRecipe;
				}
			}

			public ShipModel ConsortShip
			{
				get
				{
					return this.mConsortShip;
				}
			}

			public bool IsDetermined
			{
				get
				{
					return this.mIsDetermined;
				}
			}

			public bool Success
			{
				get;
				private set;
			}

			public void SetBeforeSlotItemInfo(SlotitemModel model)
			{
				this.mBeforeSlotItemInfo = new UserInterfaceRevampManager.RevampContext.SlotItemInfo(model);
			}

			public void SetAfterSlotItemInfo(SlotitemModel model)
			{
				this.mAfterSlotItemInfo = new UserInterfaceRevampManager.RevampContext.SlotItemInfo(model);
			}

			public void SetRevampRecipe(RevampRecipeModel revampRecipeModel)
			{
				this.mRevampRecipe = revampRecipeModel;
			}

			public void SetDetermined(bool isDetermined)
			{
				this.mIsDetermined = isDetermined;
			}

			public void SetConsortShip(ShipModel consortShipModel)
			{
				this.mConsortShip = consortShipModel;
			}

			public void SetSuccess(bool success)
			{
				this.Success = success;
			}

			public UserInterfaceRevampManager.RevampContext.SlotItemInfo GetAfterSlotItemInfo()
			{
				return this.mAfterSlotItemInfo;
			}

			public bool IsModelChange()
			{
				return this.mBeforeSlotItemInfo.MstId != this.mAfterSlotItemInfo.MstId;
			}

			public UserInterfaceRevampManager.RevampContext.SlotItemInfo GetBeforeSlotItemInfo()
			{
				return this.mBeforeSlotItemInfo;
			}
		}

		public class LocalUtils
		{
			public static string GenerateRevampSettingMessage(RevampValidationResult iResult, RevampRecipeDetailModel model)
			{
				if (model == null)
				{
					return null;
				}
				string text = "[000000]";
				switch (iResult)
				{
				case RevampValidationResult.OK:
					if (model.RequiredSlotitemId == 0)
					{
						text += string.Format("[329ad6]{0}[-]\n", model.Slotitem.Name);
						text += string.Format("を改修しますね！[-]", new object[0]);
					}
					else
					{
						SlotitemModel_Mst slotitemModel_Mst = new SlotitemModel_Mst(model.RequiredSlotitemId);
						text += string.Format("[329ad6]{0}[-]\n", model.Slotitem.Name);
						text += string.Format("を改修しますね！[-]", new object[0]);
						if (0 < model.RequiredSlotitemCount)
						{
							text += "\n";
							text += "[000000]この改修には、無改修の\n";
							text += string.Format("[329ad6]{0}×{1}[-]", slotitemModel_Mst.Name, model.RequiredSlotitemCount);
							text += "\n\nが必要です。[-]";
							text += "\n[666666](※改修で消費します)[-]";
						}
					}
					break;
				case RevampValidationResult.Max_Level:
					text += string.Format("[FF0000]現在、選択された装備[-]\n", new object[0]);
					text += string.Format("[329ad6]{0}[-]\n", model.Slotitem.Name);
					text += string.Format("[FF0000]は、これ以上の改修ができません。[-]", new object[0]);
					break;
				case RevampValidationResult.Lock:
					text += string.Format("[FF0000]この装備を改修するには\n\u3000同装備のロック解除が必要です。[-]", new object[0]);
					break;
				case RevampValidationResult.Less_Fuel:
				case RevampValidationResult.Less_Ammo:
				case RevampValidationResult.Less_Steel:
				case RevampValidationResult.Less_Baux:
				case RevampValidationResult.Less_Devkit:
				case RevampValidationResult.Less_Revkit:
					text += string.Format("[FF0000]\u3000資材が足りません。", new object[0]);
					break;
				case RevampValidationResult.Less_Slotitem:
					text += "[FF0000]この改修に必要となる\n(無改修)\n[-]";
					if (0 < model.RequiredSlotitemId)
					{
						text += string.Format("[329ad6]{0}×{1}[-]", new SlotitemModel_Mst(model.RequiredSlotitemId).Name, model.RequiredSlotitemCount);
					}
					else
					{
						text += string.Format("[329ad6]{0}×{1}[-]", model.Slotitem.Name, model.RequiredSlotitemCount);
					}
					text += "\n";
					text += "[FF0000]が足りません。[-]";
					break;
				case RevampValidationResult.Less_Slotitem_No_Lock:
					text += string.Format("[FF0000]この改修に必要となる\n(無改修)\n[-]", new object[0]);
					if (0 < model.RequiredSlotitemId)
					{
						text += string.Format("[329ad6]{0}x{1}[-]", new SlotitemModel_Mst(model.RequiredSlotitemId).Name, model.RequiredSlotitemCount);
					}
					else
					{
						text += string.Format("[329ad6]{0}x{1}[-]", model.Slotitem.Name, model.RequiredSlotitemCount);
					}
					text += string.Format("[FF0000]が足りません。[-]", new object[0]);
					break;
				}
				return text + "[-]";
			}
		}

		private const int MASTER_ID_AKASHI = 182;

		private const int MASTER_ID_AKASHI_KAI = 187;

		private const int NON_ASSISTANT_SHIP = 0;

		private RevampManager mRevampManager;

		private UserInterfaceRevampManager.RevampContext mRevampContext;

		[SerializeField]
		private Camera mCameraTouchEventCatch;

		[SerializeField]
		private Camera mCameraProduction;

		[SerializeField]
		private UIRevampSlotItemScrollListParentNew mUIRevampSlotItemScrollListParentNew;

		[SerializeField]
		private UIRevampRecipeScrollParentNew mRevampRecipeScrollParentNew;

		[SerializeField]
		private UIRevampSetting mPrefab_RevampSetting;

		[SerializeField]
		private UIRevampIcon mPrefab_RevampIcon;

		[SerializeField]
		private UIRevampMaterialsInfo mRevampMaterialsInfo;

		[SerializeField]
		private UIRevampBalloon mRevampInfoBalloon;

		[SerializeField]
		private UIRevampAkashi mRevampAkashi;

		[SerializeField]
		private ModalCamera mModalCamera;

		[SerializeField]
		private UITexture mTexture_AssistantShip;

		[SerializeField]
		private ParticleSystem mParticleSystem_SuccessStars;

		[SerializeField]
		private Transform mTransform_AssistantShipParent;

		[SerializeField]
		private Vector3 mVector3_AssistantShipShowLocalPosition;

		[SerializeField]
		private Vector3 mVector3_AssistantShipHideLocalPosition;

		private KeyControl mFocusKeyController;

		private int mDeckId;

		private int mAreaId;

		private UIButton _uiOverlayButton2;

		public bool _isTop;

		public bool _isAnimation;

		public bool _isSettingMode;

		private AudioClip mAudioClip_BGM;

		private AudioClip mAudioClip_SE_020;

		private AudioClip mAudioClip_SE_022;

		private AudioClip mAudioClip_SE_017;

		private AudioClip mAudioClip_SE_002;

		private AudioClip mAudioClip_SE_023;

		private AudioClip mAudioClip_SE_021;

		private AudioClip mAudioClip_303;

		private AudioClip mAudioClip_304;

		private AudioClip mAudioClip_305;

		private AudioClip mAudioClip_308;

		private AudioClip mAudioClip_309;

		private AudioClip mAudioClip_313;

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UserInterfaceRevampManager.<Start>c__IteratorC7 <Start>c__IteratorC = new UserInterfaceRevampManager.<Start>c__IteratorC7();
			<Start>c__IteratorC.<>f__this = this;
			return <Start>c__IteratorC;
		}

		private void InitializeAkashi()
		{
			if (this.mRevampManager.Deck.GetFlagShip().MstId == 187)
			{
				this.mRevampAkashi.Initialize(UIRevampAkashi.CharacterType.AkashiKai);
			}
			else
			{
				this.mRevampAkashi.Initialize(UIRevampAkashi.CharacterType.Akashi);
			}
			string message = this.mRevampInfoBalloon.GetMessageBuilder().AddMessage("提督、明石の工廠へようこそ！", true).AddMessage("どの装備の改修を試みますか？").Build();
			this.mRevampInfoBalloon.SayMessage(message);
		}

		private void Update()
		{
			if (this.mFocusKeyController != null)
			{
				this.mFocusKeyController.Update();
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable && this.mFocusKeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
					this.mFocusKeyController = null;
				}
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mParticleSystem_SuccessStars);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_BGM, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_020, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_022, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_017, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_002, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_023, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_021, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_303, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_304, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_305, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_308, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_309, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_313, false);
			this.mRevampManager = null;
			this.mRevampContext = null;
			this.mCameraTouchEventCatch = null;
			this.mCameraProduction = null;
			this.mUIRevampSlotItemScrollListParentNew = null;
			this.mRevampRecipeScrollParentNew = null;
			this.mPrefab_RevampSetting = null;
			this.mPrefab_RevampIcon = null;
			this.mRevampMaterialsInfo = null;
			this.mRevampInfoBalloon = null;
			this.mRevampAkashi = null;
			this.mModalCamera = null;
			this.mTexture_AssistantShip = null;
			this.mTransform_AssistantShipParent = null;
			this.mFocusKeyController = null;
		}

		private KeyControl ShowUIRevampRecipeList(int firstFocusIndex)
		{
			this.mRevampRecipeScrollParentNew.SetActive(false);
			this.mRevampRecipeScrollParentNew.SetActive(true);
			this._isTop = true;
			RevampRecipeModel[] recipes = this.mRevampManager.GetRecipes();
			this.mRevampContext = new UserInterfaceRevampManager.RevampContext();
			this.mRevampRecipeScrollParentNew.Initialize(this.mRevampManager);
			this.mRevampRecipeScrollParentNew.SetOnSelectedListener(new Action<UIRevampRecipeScrollChildNew>(this.OnSelectedRecipeListener));
			this.mRevampRecipeScrollParentNew.SetCamera(this.mCameraTouchEventCatch);
			KeyControl keyController = this.mRevampRecipeScrollParentNew.GetKeyController();
			this.mRevampRecipeScrollParentNew.PlaySlotInAnimation();
			this.mRevampRecipeScrollParentNew.SetOnFinishedSlotInAnimationListener(delegate
			{
				this.mRevampRecipeScrollParentNew.StartControl();
			});
			return keyController;
		}

		private void OnSelectedRecipeListener(UIRevampRecipeScrollChildNew child)
		{
			this.mRevampRecipeScrollParentNew.LockControl();
			int num = Random.Range(0, 100);
			if (30 < num)
			{
				this.PlayAkashiVoice(this.mAudioClip_303);
			}
			else
			{
				this.PlayAkashiVoice(this.mAudioClip_304);
			}
			RevampRecipeModel model = child.GetModel().Model;
			this.mRevampContext.SetRevampRecipe(model);
			SoundUtils.PlaySE(this.mAudioClip_SE_002);
			this.mRevampRecipeScrollParentNew.SetActive(false);
			KeyControl keyController = this.ShowUIRevampSlotItemGrid(this.mRevampContext);
			this.ChangeFocusKeyController(keyController);
		}

		private void PlayAkashiVoice(AudioClip audioClip)
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(audioClip);
		}

		private KeyControl ShowUIRevampSlotItemGrid(UserInterfaceRevampManager.RevampContext revampContext)
		{
			this._isTop = false;
			SlotitemModel[] slotitemList = this.mRevampManager.GetSlotitemList(this.mRevampContext.RevampRecipe.RecipeId);
			this.ChangeFocusKeyController(null);
			this.mUIRevampSlotItemScrollListParentNew.SetActive(false);
			this.mUIRevampSlotItemScrollListParentNew.SetActive(true);
			this._uiOverlayButton2 = this.mUIRevampSlotItemScrollListParentNew.GetOverlayBtn2();
			EventDelegate.Add(this._uiOverlayButton2.onClick, new EventDelegate.Callback(this._onClickOverlayButton2));
			this.mRevampInfoBalloon.alpha = 1E-10f;
			this.mUIRevampSlotItemScrollListParentNew.Initialize(slotitemList);
			this.mUIRevampSlotItemScrollListParentNew.SetCamera(this.mCameraTouchEventCatch);
			this.mUIRevampSlotItemScrollListParentNew.SetOnSelectedSlotItemListener(new Action<UIRevampSlotItemScrollListChildNew>(this.OnSelectedSlotItemListener));
			this.mUIRevampSlotItemScrollListParentNew.SetOnBackListener(new Action(this.OnBackSlotItemList));
			this.mUIRevampSlotItemScrollListParentNew.StartControl();
			return this.mUIRevampSlotItemScrollListParentNew.GetKeyController();
		}

		private void OnBackSlotItemList()
		{
			RevampRecipeModel[] recipes = this.mRevampManager.GetRecipes();
			SoundUtils.PlaySE(this.mAudioClip_SE_017);
			this.mRevampInfoBalloon.alpha = 1f;
			this.mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
			KeyControl keyController = this.ShowUIRevampRecipeList(0);
			this.ChangeFocusKeyController(keyController);
			this.mUIRevampSlotItemScrollListParentNew.SetActive(false);
		}

		private void _onClickOverlayButton2()
		{
			this._isTop = false;
			RevampRecipeModel[] recipes = this.mRevampManager.GetRecipes();
			int num = 0;
			this.mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
			KeyControl keyController;
			if (num <= recipes.Length)
			{
				keyController = this.ShowUIRevampRecipeList(num);
			}
			else
			{
				keyController = this.ShowUIRevampRecipeList(0);
			}
			this.ChangeFocusKeyController(keyController);
			this.mUIRevampSlotItemScrollListParentNew.SetActive(false);
		}

		private void OnSelectedSlotItemListener(UIRevampSlotItemScrollListChildNew selectedSlotItemView)
		{
			this.mRevampInfoBalloon.alpha = 1f;
			this.mRevampContext.SetBeforeSlotItemInfo(selectedSlotItemView.GetModel());
			RevampRecipeDetailModel detail = this.mRevampManager.GetDetail(this.mRevampContext.RevampRecipe.RecipeId, this.mRevampContext.GetBeforeSlotItemInfo().MemId);
			UIRevampSetting revampSetting = this.ShowUIRevampSetting(this.mRevampContext);
			int num = Random.Range(0, 100);
			if (40 < num)
			{
				this.PlayAkashiVoice(this.mAudioClip_305);
			}
			else
			{
				this.PlayAkashiVoice(this.mAudioClip_313);
			}
			this.mUIRevampSlotItemScrollListParentNew.SetActive(false);
			revampSetting.Show(delegate
			{
				this._isSettingMode = true;
				KeyControl keyController = revampSetting.GetKeyController();
				this.ChangeFocusKeyController(keyController);
			});
		}

		private UIRevampSetting ShowUIRevampSetting(UserInterfaceRevampManager.RevampContext revampContext)
		{
			RevampRecipeDetailModel detail = this.mRevampManager.GetDetail(revampContext.RevampRecipe.RecipeId, revampContext.GetBeforeSlotItemInfo().MemId);
			RevampValidationResult revampValidationResult = this.mRevampManager.IsValidRevamp(detail);
			UIRevampSetting component = Util.Instantiate(this.mPrefab_RevampSetting.get_gameObject(), base.get_gameObject(), false, false).GetComponent<UIRevampSetting>();
			component.SetOnRevampSettingActionCallBack(new UIRevampSetting.UIRevampSettingAction(this.UIRevampSettingActionCallBack));
			component.Initialize(detail, new UIRevampSetting.UIRevampSettingStateCheck(this.UIRevampRecipeSettingCheckDelegate), this.mCameraProduction);
			return component;
		}

		private RevampValidationResult UIRevampRecipeSettingCheckDelegate(RevampRecipeDetailModel targetModel)
		{
			RevampValidationResult revampValidationResult = this.mRevampManager.IsValidRevamp(targetModel);
			this.mRevampInfoBalloon.SayMessage(UserInterfaceRevampManager.LocalUtils.GenerateRevampSettingMessage(revampValidationResult, targetModel));
			return revampValidationResult;
		}

		private void UIRevampSettingActionCallBack(UIRevampSetting.ActionType actionType, UIRevampSetting calledObject)
		{
			this._isSettingMode = false;
			if (actionType != UIRevampSetting.ActionType.CancelRevamp)
			{
				if (actionType == UIRevampSetting.ActionType.StartRevamp)
				{
					this.OnStartRevamp(calledObject);
				}
			}
			else
			{
				this.OnCanelRevampSetting(calledObject);
			}
		}

		private void OnStartRevamp(UIRevampSetting calledObject)
		{
			base.StartCoroutine(this.OnStartRevampCoroutine(calledObject));
		}

		[DebuggerHidden]
		private IEnumerator OnStartRevampCoroutine(UIRevampSetting calledObject)
		{
			UserInterfaceRevampManager.<OnStartRevampCoroutine>c__IteratorC8 <OnStartRevampCoroutine>c__IteratorC = new UserInterfaceRevampManager.<OnStartRevampCoroutine>c__IteratorC8();
			<OnStartRevampCoroutine>c__IteratorC.calledObject = calledObject;
			<OnStartRevampCoroutine>c__IteratorC.<$>calledObject = calledObject;
			<OnStartRevampCoroutine>c__IteratorC.<>f__this = this;
			return <OnStartRevampCoroutine>c__IteratorC;
		}

		private void OnStartRevampAnimation(UserInterfaceRevampManager.RevampContext mRevampContext)
		{
			if (mRevampContext.Success)
			{
				this.OnStartSuccessRevampAnimation(mRevampContext);
			}
			else
			{
				this.OnStartFailRevampAnimation(mRevampContext);
			}
		}

		private void OnStartSuccessRevampAnimation(UserInterfaceRevampManager.RevampContext context)
		{
			this._isTop = true;
			UIRevampIcon revampIcon = Util.Instantiate(this.mPrefab_RevampIcon.get_gameObject(), base.get_gameObject(), false, false).GetComponent<UIRevampIcon>();
			revampIcon.Initialize(context.GetBeforeSlotItemInfo().MstId, context.GetBeforeSlotItemInfo().Level, this.mCameraProduction);
			revampIcon.StartRevamp(context.GetAfterSlotItemInfo().MstId, context.GetAfterSlotItemInfo().Level, context.GetAfterSlotItemInfo().Name, delegate
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mRevampAkashi.ChangeBodyTo(UIRevampAkashi.BodyType.Normal);
				this._isAnimation = false;
				string text = string.Empty;
				this.PlayAkashiVoice(this.mAudioClip_308);
				text += "[000000]改修成功しました。";
				text += "\n";
				text += string.Format("[329ad6]{0}[-]", this.mRevampContext.GetAfterSlotItemInfo().Name);
				this.mParticleSystem_SuccessStars.Play(false);
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
				}
				TrophyUtil.Unlock_AlbumSlotNum();
				if (this.mRevampContext.IsModelChange())
				{
					SoundUtils.PlaySE(this.mAudioClip_SE_023);
				}
				else
				{
					SoundUtils.PlaySE(this.mAudioClip_SE_021);
				}
				KeyControl keyController = this.mRevampInfoBalloon.SayMessage(text, delegate
				{
					if (this.mRevampContext.ConsortShip != null)
					{
						ShortcutExtensions.DOLocalMove(this.mTransform_AssistantShipParent, this.mVector3_AssistantShipHideLocalPosition, 0.6f, false);
					}
					this.mRevampInfoBalloon.alpha = 1f;
					Object.Destroy(revampIcon.get_gameObject());
					this.mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
					RevampRecipeDetailModel detail = this.mRevampManager.GetDetail(this.mRevampContext.RevampRecipe.RecipeId, this.mRevampContext.GetBeforeSlotItemInfo().MemId);
					RevampRecipeModel[] recipes = this.mRevampManager.GetRecipes();
					int num = 0;
					RevampRecipeModel[] array = recipes;
					for (int i = 0; i < array.Length; i++)
					{
						RevampRecipeModel revampRecipeModel = array[i];
						if (revampRecipeModel.RecipeId == this.mRevampContext.RevampRecipe.RecipeId)
						{
							break;
						}
						num++;
					}
					KeyControl keyController2;
					if (num <= recipes.Length)
					{
						keyController2 = this.ShowUIRevampRecipeList(num);
					}
					else
					{
						keyController2 = this.ShowUIRevampRecipeList(0);
					}
					this.ChangeFocusKeyController(keyController2);
				});
				this.ChangeFocusKeyController(keyController);
				ShortcutExtensions.DOLocalMove(this.mTransform_AssistantShipParent, this.mVector3_AssistantShipHideLocalPosition, 0.6f, false);
			});
		}

		private void OnStartFailRevampAnimation(UserInterfaceRevampManager.RevampContext context)
		{
			this._isTop = true;
			UIRevampIcon revampIcon = Util.Instantiate(this.mPrefab_RevampIcon.get_gameObject(), base.get_gameObject(), false, false).GetComponent<UIRevampIcon>();
			revampIcon.Initialize(context.GetBeforeSlotItemInfo().MstId, context.GetBeforeSlotItemInfo().Level, this.mCameraProduction);
			revampIcon.StartRevamp(context.GetBeforeSlotItemInfo().MstId, context.GetBeforeSlotItemInfo().Level, context.GetBeforeSlotItemInfo().Name, delegate
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mRevampAkashi.ChangeBodyTo(UIRevampAkashi.BodyType.Normal);
				string text = string.Empty;
				this._isAnimation = false;
				this.PlayAkashiVoice(this.mAudioClip_309);
				SoundUtils.PlaySE(this.mAudioClip_SE_022);
				text += "[000000]改修失敗しました。";
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
				}
				KeyControl keyController = this.mRevampInfoBalloon.SayMessage(text, delegate
				{
					if (this.mRevampContext.ConsortShip != null)
					{
						ShortcutExtensions.DOLocalMove(this.mTransform_AssistantShipParent, this.mVector3_AssistantShipHideLocalPosition, 0.6f, false);
					}
					Object.Destroy(revampIcon.get_gameObject());
					this.mRevampInfoBalloon.alpha = 1f;
					this.mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
					RevampRecipeDetailModel detail = this.mRevampManager.GetDetail(this.mRevampContext.RevampRecipe.RecipeId, this.mRevampContext.GetBeforeSlotItemInfo().MemId);
					RevampRecipeModel[] recipes = this.mRevampManager.GetRecipes();
					int num = 0;
					RevampRecipeModel[] array = recipes;
					for (int i = 0; i < array.Length; i++)
					{
						RevampRecipeModel revampRecipeModel = array[i];
						if (revampRecipeModel.RecipeId == this.mRevampContext.RevampRecipe.RecipeId)
						{
							break;
						}
						num++;
					}
					KeyControl keyController2;
					if (num <= recipes.Length)
					{
						keyController2 = this.ShowUIRevampRecipeList(num);
					}
					else
					{
						keyController2 = this.ShowUIRevampRecipeList(0);
					}
					this.ChangeFocusKeyController(keyController2);
				});
				this.ChangeFocusKeyController(keyController);
				ShortcutExtensions.DOLocalMove(this.mTransform_AssistantShipParent, this.mVector3_AssistantShipHideLocalPosition, 0.6f, false);
			});
		}

		private void OnCanelRevampSetting(UIRevampSetting calledObject)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SoundUtils.PlaySE(this.mAudioClip_SE_017);
			KeyControl nextFocusKeyController = null;
			calledObject.Hide(delegate
			{
				RevampRecipeModel[] recipes = this.mRevampManager.GetRecipes();
				RevampRecipeDetailModel detail = this.mRevampManager.GetDetail(this.mRevampContext.RevampRecipe.RecipeId, this.mRevampContext.GetBeforeSlotItemInfo().MemId);
				int num = 0;
				RevampRecipeModel[] array = recipes;
				for (int i = 0; i < array.Length; i++)
				{
					RevampRecipeModel revampRecipeModel = array[i];
					if (revampRecipeModel.RecipeId == detail.RecipeId)
					{
						break;
					}
					num++;
				}
				this.mRevampInfoBalloon.alpha = 1f;
				this.mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
				if (num <= recipes.Length)
				{
					nextFocusKeyController = this.ShowUIRevampRecipeList(num);
				}
				else
				{
					nextFocusKeyController = this.ShowUIRevampRecipeList(0);
				}
				this.ChangeFocusKeyController(nextFocusKeyController);
				Object.Destroy(calledObject.get_gameObject());
			});
		}

		private void ChangeFocusKeyController(KeyControl keyController)
		{
			if (this.mFocusKeyController != null)
			{
				this.mFocusKeyController.firstUpdate = true;
				this.mFocusKeyController.ClearKeyAll();
			}
			this.mFocusKeyController = keyController;
			if (this.mFocusKeyController != null)
			{
				this.mFocusKeyController.firstUpdate = true;
				this.mFocusKeyController.ClearKeyAll();
			}
		}

		private void UpdateInfo(ManagerBase manager)
		{
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(manager);
			}
			this.mRevampMaterialsInfo.UpdateInfo(manager);
		}
	}
}
