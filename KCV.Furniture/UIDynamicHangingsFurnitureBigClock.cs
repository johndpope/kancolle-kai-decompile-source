using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicHangingsFurnitureBigClock : UIDynamicFurniture
	{
		private const int OPEN_GATE_WIDTH = 43;

		private const int OPEN_GATE_HEIGHT = 16;

		private const int CLOSE_GATE_WIDTH = 26;

		private const int CLOSE_GATE_HEIGHT = 12;

		private const int OPEN_PEGION_WIDTH = 29;

		private const int OPEN_PEGION_HEIGHT = 16;

		private const int CLOSE_PEGION_WIDTH = 26;

		private const int CLOSE_PEGION_HEIGHT = 12;

		[Header("Pegion"), SerializeField]
		private Transform mPegion;

		[SerializeField]
		private Vector2 mVector2_ClosePegionLocalPosition;

		[SerializeField]
		private Vector2 mVector2_OpenPegionLocalPosition;

		[SerializeField]
		private UITexture mTexture_Pegion;

		[SerializeField]
		private Texture mTexture2d_Pegion_On;

		[SerializeField]
		private Texture mTexture2d_Pegion_Off;

		[Header("Gate"), SerializeField]
		private UITexture mTexture_Gate;

		[SerializeField]
		private Texture mTexture2d_OpenGate;

		[SerializeField]
		private Texture mTexture2d_CloseGate;

		[SerializeField]
		private AudioClip mAudioClip_Pegion;

		private ShipModel mShipModel;

		private int mLastPlayMinute = -1;

		private void Update()
		{
			if (this.mFurnitureModel != null)
			{
				bool flag = this.mFurnitureModel.GetDateTime().get_Minute() == 0 || this.mFurnitureModel.GetDateTime().get_Minute() % 15 == 0;
				if (flag)
				{
					bool flag2 = DOTween.IsTweening(this);
					bool flag3 = this.mLastPlayMinute == this.mFurnitureModel.GetDateTime().get_Minute();
					if (!flag2 && !flag3)
					{
						this.mLastPlayMinute = this.mFurnitureModel.GetDateTime().get_Minute();
						this.Animation();
					}
				}
			}
			if (Input.GetKeyDown(97))
			{
				this.Animation();
			}
		}

		protected override void OnAwake()
		{
			base.Initialize(new UIFurniture.UIFurnitureModel(null, null));
			this.mPegion.get_gameObject().SetActive(false);
			this.CloseGate();
			this.ClosePegion();
		}

		private void ClosePegion()
		{
			this.OffPegion();
		}

		protected override void OnCalledActionEvent()
		{
			base.OnCalledActionEvent();
			this.Animation();
		}

		private void Animation()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				return;
			}
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenCallback tweenCallback = delegate
			{
				this.OpenGate();
				this.mPegion.get_gameObject().SetActive(true);
				this.mPegion.get_transform().set_localPosition(this.mVector2_ClosePegionLocalPosition);
				SoundUtils.PlaySE(this.mAudioClip_Pegion);
				this.OffPegion();
			};
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMove(this.mPegion.get_transform(), this.mVector2_OpenPegionLocalPosition, 0.3f, false), this);
			Sequence sequence2 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenCallback tweenCallback2 = delegate
			{
				this.OnPegion();
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.OffPegion();
			};
			TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback2);
			TweenSettingsExtensions.AppendInterval(sequence2, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence2, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback2);
			TweenSettingsExtensions.AppendInterval(sequence2, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence2, 0.1f);
			Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMove(this.mPegion.get_transform(), this.mVector2_ClosePegionLocalPosition, 0.15f, false), this);
			TweenCallback tweenCallback4 = delegate
			{
				this.CloseGate();
				this.mPegion.get_gameObject().SetActive(false);
			};
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, sequence2);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback4);
		}

		private void OffPegion()
		{
			this.mTexture_Pegion.mainTexture = this.mTexture2d_Pegion_Off;
			this.mTexture_Pegion.width = 26;
			this.mTexture_Pegion.height = 12;
		}

		private void OnPegion()
		{
			this.mTexture_Pegion.mainTexture = this.mTexture2d_Pegion_On;
			this.mTexture_Pegion.width = 29;
			this.mTexture_Pegion.height = 16;
		}

		private void OpenGate()
		{
			this.mTexture_Gate.mainTexture = this.mTexture2d_OpenGate;
			this.mTexture_Gate.width = 43;
			this.mTexture_Gate.height = 16;
		}

		private void CloseGate()
		{
			this.mTexture_Gate.mainTexture = this.mTexture2d_CloseGate;
			this.mTexture_Gate.width = 26;
			this.mTexture_Gate.height = 12;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Pegion, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Pegion_On, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Pegion_Off, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Gate, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_OpenGate, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_CloseGate, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_Pegion, false);
			this.mPegion = null;
			this.mShipModel = null;
		}
	}
}
