using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyOhyodo : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Ohyodo;

		[SerializeField]
		private Texture[] mTextureOhyodo;

		[SerializeField]
		private Vector3 mVector3_Ohyodo_ShowLocalPosition;

		[SerializeField]
		private Vector3 mVector3_Ohyodo_GoBackLocalPosition;

		[SerializeField]
		private Vector3 mVector3_Ohyodo_WaitingLocalPosition;

		[SerializeField]
		private UIButton mButton_TouchBackArea;

		private void Start()
		{
			this.mTexture_Ohyodo.get_transform().set_localPosition(this.mVector3_Ohyodo_WaitingLocalPosition);
			this.mTexture_Ohyodo.mainTexture = this.mTextureOhyodo[0];
			this.EnableTouchBackArea(false);
		}

		public void Show(Action onFinishedAnimation)
		{
			ShipUtils.PlayPortVoice(1);
			TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMove(this.mTexture_Ohyodo.get_transform(), this.mVector3_Ohyodo_ShowLocalPosition, 0.4f, false), delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation.Invoke();
				}
			}), 3);
		}

		public void Hide(Action onFinishedAnimation)
		{
			this.mTexture_Ohyodo.mainTexture = this.mTextureOhyodo[1];
			TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMove(this.mTexture_Ohyodo.get_transform(), this.mVector3_Ohyodo_GoBackLocalPosition, 0.4f, false), 0.5f), delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation.Invoke();
				}
			}), 3);
		}

		public void EnableTouchBackArea(bool enabled)
		{
			this.mButton_TouchBackArea.set_enabled(enabled);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextureOhyodo, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_TouchBackArea);
		}
	}
}
