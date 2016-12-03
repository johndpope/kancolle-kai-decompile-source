using DG.Tweening;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Strategy
{
	public class UIStageConfirm : MonoBehaviour
	{
		[SerializeField]
		private UIButton mButton_GoStage;

		[SerializeField]
		private Transform mTransfrom_GoStageDisabled;

		[SerializeField]
		private UILabel mLabel_OperationTitle;

		[SerializeField]
		private UILabel mLabel_OperationDetail;

		[SerializeField]
		private UISprite[] mSprites_Reward;

		[SerializeField]
		private Vector3 mVector3_ShowPosition;

		[SerializeField]
		private Vector3 mVector3_HidePosition;

		[SerializeField]
		private GoConditionInfo ConditionInfo;

		private Tween mTweenShowHide;

		public MapModel mMapModel
		{
			get;
			private set;
		}

		public bool Shown
		{
			get;
			private set;
		}

		public void Initialize(MapModel mapModel)
		{
			this.mMapModel = mapModel;
			this.mLabel_OperationTitle.text = this.mMapModel.Opetext;
			this.mLabel_OperationDetail.text = this.mMapModel.Infotext;
			if (this.mMapModel.Map_Possible)
			{
				this.mButton_GoStage.get_transform().SetActive(true);
				this.mTransfrom_GoStageDisabled.SetActive(false);
			}
			else
			{
				this.mButton_GoStage.get_transform().SetActive(false);
				this.mTransfrom_GoStageDisabled.SetActive(true);
			}
			int[] rewardItemIds = mapModel.GetRewardItemIds();
			UISprite[] array = this.mSprites_Reward;
			for (int i = 0; i < array.Length; i++)
			{
				UISprite component = array[i];
				component.SetActive(false);
			}
			for (int j = 0; j < rewardItemIds.Length; j++)
			{
				this.mSprites_Reward[j].SetActive(true);
				this.mSprites_Reward[j].spriteName = string.Format("item_{0}", rewardItemIds[j]);
			}
		}

		public void Show()
		{
			this.Shown = true;
			base.get_transform().SetActive(true);
			if (this.mTweenShowHide != null)
			{
				TweenExtensions.Kill(this.mTweenShowHide, false);
				this.mTweenShowHide = null;
			}
			this.ConditionInfo.Initialize(this.mMapModel);
			this.DelayActionFrame(1, delegate
			{
				this.mTweenShowHide = TweenSettingsExtensions.SetId<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions.DOLocalMove(base.get_transform(), this.mVector3_ShowPosition, 0.3f, false)), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mButton_GoStage.get_transform(), new Vector3(1f, 1f), 0.2f), 27)), this);
			});
		}

		public void Hide()
		{
			this.Shown = false;
			if (this.mTweenShowHide != null)
			{
				TweenExtensions.Kill(this.mTweenShowHide, false);
				this.mTweenShowHide = null;
			}
			this.mTweenShowHide = TweenSettingsExtensions.SetId<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMove(base.get_transform(), this.mVector3_HidePosition, 0.2f, false), delegate
			{
				base.get_transform().SetActive(false);
			})), ShortcutExtensions.DOScale(this.mButton_GoStage.get_transform(), new Vector3(0.1f, 0.1f), 0.3f)), this);
		}

		public void ClickAnimation(Action onFinished)
		{
			TweenExtensions.PlayForward(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mButton_GoStage.get_transform(), new Vector3(0.8f, 0.8f, 0f), 0.15f), 20), delegate
			{
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mButton_GoStage.get_transform(), new Vector3(1f, 1f, 0f), 0.15f), 21), delegate
				{
					if (onFinished != null)
					{
						onFinished.Invoke();
					}
				});
			}));
		}

		private void OnDestroy()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.mButton_GoStage = null;
			this.mTransfrom_GoStageDisabled = null;
			this.mLabel_OperationTitle = null;
			this.mLabel_OperationDetail = null;
			this.mSprites_Reward = null;
			this.ConditionInfo = null;
		}
	}
}
