using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeShutter : MonoBehaviour
	{
		[SerializeField]
		private Animation mAnimation_Shutter;

		[SerializeField]
		private Transform mTransform_ShutterTop;

		[SerializeField]
		private Transform mTransform_ShutterBotton;

		private Vector3 mVector3_LocalPositionCloseShutterTop;

		private Vector3 mVector3_LocalPositionCloseShutterBottom;

		private Action mOnFinishedOpenShutterAnimationListener;

		private Action mOnFinishedCloseShutterAnimationListener;

		private void Awake()
		{
			this.mVector3_LocalPositionCloseShutterTop = this.mTransform_ShutterTop.get_localPosition();
			this.mVector3_LocalPositionCloseShutterBottom = this.mTransform_ShutterBotton.get_localPosition();
			this.OpenWithNonAnimation();
		}

		public void SetOnFinishedOpenShutterAnimationListener(Action onFinished)
		{
			this.mOnFinishedOpenShutterAnimationListener = onFinished;
		}

		public void SetOnFinishedCloseShutterAnimationListener(Action onFinished)
		{
			this.mOnFinishedCloseShutterAnimationListener = onFinished;
		}

		public void OpenWithNonAnimation()
		{
			this.mTransform_ShutterTop.localPositionY(230f);
			this.mTransform_ShutterBotton.localPositionY(-230f);
		}

		public void CloseWithNonAnimation()
		{
			this.mTransform_ShutterTop.localPositionY(this.mVector3_LocalPositionCloseShutterTop.y);
			this.mTransform_ShutterBotton.localPositionY(this.mVector3_LocalPositionCloseShutterBottom.y);
		}

		public void OpenShutter()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_034);
			this.mAnimation_Shutter.Play("Anim_OpenShutter");
		}

		public void CloseShutter()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_034);
			this.mAnimation_Shutter.Play("Anim_CloseShutter");
		}

		public void OnFinishedOpenShutterAnimation()
		{
			if (this.mOnFinishedOpenShutterAnimationListener != null)
			{
				this.mOnFinishedOpenShutterAnimationListener.Invoke();
			}
		}

		public void OnFinishedCloseShutterAnimation()
		{
			if (this.mOnFinishedCloseShutterAnimationListener != null)
			{
				this.mOnFinishedCloseShutterAnimationListener.Invoke();
			}
		}

		private void OnDestroy()
		{
			this.mOnFinishedOpenShutterAnimationListener = null;
			this.mOnFinishedCloseShutterAnimationListener = null;
			if (this.mAnimation_Shutter != null)
			{
				this.mAnimation_Shutter.Stop();
			}
			this.mAnimation_Shutter = null;
			this.mTransform_ShutterTop = null;
			this.mTransform_ShutterBotton = null;
		}
	}
}
