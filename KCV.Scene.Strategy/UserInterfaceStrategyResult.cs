using DG.Tweening;
using KCV.Scene.Mission;
using KCV.Scene.Port;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Strategy
{
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfaceStrategyResult : MonoBehaviour
	{
		private UIPanel mPanelThis;

		[SerializeField]
		private UILabel mLabel_MissionName;

		[SerializeField]
		private UILabel mLabel_AdmiralName;

		[SerializeField]
		private UILabel mLabel_AdmiralLevel;

		[SerializeField]
		private UILabel mLabel_DeckName;

		[SerializeField]
		private UIMissionResultBonus mMissionResultBonus;

		[SerializeField]
		private UIMissionResultStatus mMissionResultStatus;

		[SerializeField]
		private UIMissionJudgeCutIn mMissionJudgeCutIn;

		[SerializeField]
		private UITexture mTexture_FlagShipGraphic;

		[SerializeField]
		private Transform mTransform_TouchControlArea;

		private StrategyMapManager mStrategyMapManager;

		private KeyControl mKeyController;

		private MissionResultModel mMissionResultModel;

		private Action mOnSelectNextAction;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
			}
		}

		public void Initialize(StrategyMapManager manager, MissionResultModel missionResultModel, KeyControl keyController, Action onSelectNextAction)
		{
			this.mMissionResultModel = missionResultModel;
			this.mTransform_TouchControlArea.SetActive(false);
			this.mStrategyMapManager = manager;
			this.mMissionJudgeCutIn.Initialize(this.mMissionResultModel.result);
			this.mMissionResultBonus.Inititalize(this.mMissionResultModel);
			this.mMissionResultStatus.Inititalize(this.mMissionResultModel);
			ShipModel shipModel = this.mMissionResultModel.Ships[0];
			this.mTexture_FlagShipGraphic.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), (!shipModel.IsDamaged()) ? 9 : 10);
			this.mTexture_FlagShipGraphic.MakePixelPerfect();
			this.mTexture_FlagShipGraphic.get_transform().set_localPosition(Util.Poi2Vec(shipModel.Offsets.GetShipDisplayCenter(shipModel.IsDamaged())));
			this.mLabel_AdmiralLevel.text = this.mStrategyMapManager.UserInfo.Level.ToString();
			this.mLabel_AdmiralName.text = this.mStrategyMapManager.UserInfo.Name;
			this.mLabel_AdmiralName.supportEncoding = false;
			this.mLabel_DeckName.text = this.mStrategyMapManager.UserInfo.GetDeck(this.mMissionResultModel.DeckID).Name;
			this.mLabel_DeckName.supportEncoding = false;
			this.mLabel_MissionName.text = this.mMissionResultModel.MissionName;
			this.mKeyController = keyController;
			this.mOnSelectNextAction = onSelectNextAction;
		}

		private void Start()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		}

		public void Play()
		{
			this.mMissionResultStatus.PlayShowBanners(delegate
			{
				this.mMissionJudgeCutIn.SetOnFinishedAnimationListener(delegate
				{
					TrophyUtil.Unlock_UserLevel();
					TrophyUtil.Unlock_Material();
					this.mKeyController.IsRun = true;
					base.StartCoroutine(this.WaitForKeyOrTouch(this.mKeyController, delegate
					{
						this.mKeyController.ClearKeyAll();
						this.mMissionJudgeCutIn.SetActive(false);
						this.mMissionResultStatus.PlayShowBannersExp(null);
						this.mMissionResultBonus.Play(delegate
						{
							this.mTransform_TouchControlArea.SetActive(true);
							this.mKeyController.IsRun = true;
							base.StartCoroutine(this.WaitForKeyOrTouch(this.mKeyController, delegate
							{
								this.mKeyController.IsRun = true;
								if (this.mOnSelectNextAction != null)
								{
									this.mOnSelectNextAction.Invoke();
								}
							}));
						});
					}));
				});
				this.mMissionJudgeCutIn.Play();
			});
		}

		[DebuggerHidden]
		private IEnumerator WaitForKeyOrTouch(KeyControl keyController, Action onKeyAction)
		{
			UserInterfaceStrategyResult.<WaitForKeyOrTouch>c__Iterator1C6 <WaitForKeyOrTouch>c__Iterator1C = new UserInterfaceStrategyResult.<WaitForKeyOrTouch>c__Iterator1C6();
			<WaitForKeyOrTouch>c__Iterator1C.keyController = keyController;
			<WaitForKeyOrTouch>c__Iterator1C.onKeyAction = onKeyAction;
			<WaitForKeyOrTouch>c__Iterator1C.<$>keyController = keyController;
			<WaitForKeyOrTouch>c__Iterator1C.<$>onKeyAction = onKeyAction;
			<WaitForKeyOrTouch>c__Iterator1C.<>f__this = this;
			return <WaitForKeyOrTouch>c__Iterator1C;
		}

		[Obsolete("インスペクタ上のUIButtonに紐付けて使用します。")]
		public void OnTouchNext()
		{
			this.OnCallNext();
		}

		private void OnCallNext()
		{
			if (this.mKeyController != null && this.mKeyController.IsRun)
			{
				this.mTransform_TouchControlArea.SetActive(false);
				this.mKeyController.IsRun = false;
			}
		}

		public void FadeOut(Action onFinished)
		{
			TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			});
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_MissionName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_AdmiralName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_AdmiralLevel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_DeckName);
			if (this.mMissionResultModel.Ships[0].IsDamaged())
			{
				UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_FlagShipGraphic, false);
			}
			else
			{
				UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_FlagShipGraphic, true);
			}
			this.mMissionResultModel = null;
			this.mMissionResultBonus = null;
			this.mMissionResultStatus = null;
			this.mMissionJudgeCutIn = null;
			this.mTransform_TouchControlArea = null;
			this.mStrategyMapManager = null;
			this.mKeyController = null;
		}
	}
}
