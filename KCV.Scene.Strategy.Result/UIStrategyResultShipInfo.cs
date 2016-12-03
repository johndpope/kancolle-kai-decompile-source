using DG.Tweening;
using KCV.Utils;
using local.models;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Strategy.Result
{
	public class UIStrategyResultShipInfo : MonoBehaviour
	{
		private UIWidget mWidget_Banner;

		[SerializeField]
		private UIWidget mWidget_Status;

		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UITexture mTexture_GaugeExp;

		[SerializeField]
		private UITexture mTexture_LevelUp;

		private ShipModel mShipModel;

		private ShipExpModel mShipExpModel;

		private int mSlotIndex;

		private void Awake()
		{
			this.mWidget_Banner = base.GetComponent<UIWidget>();
			this.mWidget_Banner.alpha = 0f;
		}

		private void Start()
		{
			this.mTexture_LevelUp.SetActive(false);
			this.mWidget_Status.alpha = 0f;
		}

		public void Initialize(int slotIndex, ShipModel shipModel, ShipExpModel shipExpModel)
		{
			this.mSlotIndex = slotIndex;
			this.mShipModel = shipModel;
			this.mShipExpModel = shipExpModel;
			this.mLabel_Level.text = shipExpModel.LevelBefore.ToString();
			this.mCommonShipBanner.SetShipData(this.mShipModel);
		}

		public void PlayShowBannerAnimation(Action onFinished)
		{
			Vector3 localPosition = base.get_transform().get_localPosition();
			Vector3 localPosition2 = new Vector3(base.get_transform().get_localPosition().x, base.get_transform().get_localPosition().y - 20f);
			base.get_transform().set_localPosition(localPosition2);
			float num = (float)this.mSlotIndex * 0.1f;
			float num2 = 0.5f;
			TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMove(base.get_transform(), localPosition, num2, false), num);
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(0f, 1f, num2, delegate(float alpha)
			{
				this.mWidget_Banner.alpha = alpha;
			}), num), delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			});
		}

		public void PlayShowStatusAnimation(Action onFinished)
		{
			Vector3 localPosition = this.mWidget_Status.get_transform().get_localPosition();
			Vector3 localPosition2 = new Vector3(localPosition.x + 20f, localPosition.y);
			this.mWidget_Status.get_transform().set_localPosition(localPosition2);
			float num = (float)this.mSlotIndex * 0.1f;
			float num2 = 0.5f;
			TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMove(this.mWidget_Status.get_transform(), localPosition, num2, false), num);
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(0f, 1f, num2, delegate(float alpha)
			{
				this.mWidget_Status.alpha = alpha;
			}), num), delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			});
		}

		public void PlayExpAnimation(Action onFinished)
		{
			float num = 1.3f;
			float num2 = 0.4f;
			float maxGaugeTextureWidth = 90f;
			this.mTexture_GaugeExp.width = (int)(maxGaugeTextureWidth * ((float)this.mShipExpModel.ExpRateBefore * 0.01f));
			int num3 = Enumerable.Sum(this.mShipExpModel.ExpRateAfter);
			int currentLevel = this.mShipExpModel.LevelBefore;
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float((float)this.mShipExpModel.ExpRateBefore, (float)num3, (float)(num3 / 90) * num2, delegate(float exp)
			{
				this.mTexture_GaugeExp.width = (int)(maxGaugeTextureWidth * (exp % 100f * 0.01f));
				int num4 = (int)(exp / 100f);
				if (currentLevel != num4 + this.mShipExpModel.LevelBefore)
				{
					currentLevel = num4 + this.mShipExpModel.LevelBefore;
					this.mLabel_Level.text = currentLevel.ToString();
				}
			}), num), delegate
			{
				if (this.mShipExpModel.LevelBefore != this.mShipExpModel.LevelAfter)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_058);
					this.PlayLevelUpAnimation();
				}
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			});
		}

		private void PlayLevelUpAnimation()
		{
			this.mTexture_LevelUp.SetActive(true);
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_LevelUp.get_transform().set_localPosition(new Vector3(225f, -15f));
			};
			Tween tween = ShortcutExtensions.DOLocalMove(this.mTexture_LevelUp.get_transform(), new Vector3(225f, -10f), 0.15f, false);
			Tween tween2 = ShortcutExtensions.DOLocalMove(this.mTexture_LevelUp.get_transform(), new Vector3(225f, -15f), 0.15f, false);
			Tween tween3 = TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float(1f, 0f, 0.5f, delegate(float alpha)
			{
				this.mTexture_LevelUp.alpha = alpha;
			}), delegate
			{
				this.mTexture_LevelUp.SetActive(false);
			});
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
			TweenSettingsExtensions.Append(sequence, tween3);
		}

		private void OnDestroy()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.mWidget_Banner = null;
			this.mWidget_Status = null;
			this.mCommonShipBanner = null;
			this.mLabel_Level = null;
			this.mTexture_GaugeExp = null;
			this.mTexture_LevelUp = null;
			this.mShipModel = null;
			this.mShipExpModel = null;
		}
	}
}
