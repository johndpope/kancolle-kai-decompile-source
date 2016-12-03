using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Practice.Deck
{
	public class UIDeckPracticeTrainingGround : MonoBehaviour
	{
		public enum SeaType
		{
			A,
			B,
			C
		}

		public enum Direction
		{
			LEFT,
			RIGHT
		}

		[SerializeField]
		private UITexture mTexture_Sea;

		[SerializeField]
		private UITexture mTexture_SeaOverlay;

		[SerializeField]
		private UITexture mTexture_Mountain;

		[SerializeField]
		private UITexture mTexture_Sky;

		private UIDeckPracticeTrainingGround.SeaType mSeaType;

		private UIDeckPracticeTrainingGround.Direction mSeaDirection;

		private UIDeckPracticeTrainingGround.SeaType mSeaOverlayType;

		private UIDeckPracticeTrainingGround.Direction mSeaOverlayDirection;

		private float mSeaMoveSpeed;

		private float mSeaOverlayMoveSpeed;

		private IEnumerator mSeaAnimationCoroutine;

		private IEnumerator mSeaOverlayAnimationCoroutine;

		public void InitializeSea(UIDeckPracticeTrainingGround.Direction direction, UIDeckPracticeTrainingGround.SeaType seaType, float moveSpeed)
		{
			this.mSeaType = seaType;
			this.mSeaDirection = direction;
			this.mSeaMoveSpeed = moveSpeed;
		}

		public void InitializeSeaOverlay(UIDeckPracticeTrainingGround.Direction direction, UIDeckPracticeTrainingGround.SeaType seaType, float moveSpeed)
		{
			this.mSeaType = seaType;
			this.mSeaOverlayDirection = direction;
			this.mSeaOverlayMoveSpeed = moveSpeed;
		}

		public void PlaySea()
		{
			if (this.mTexture_Sea != null)
			{
				if (this.mSeaAnimationCoroutine != null)
				{
					base.StopCoroutine(this.mSeaAnimationCoroutine);
					this.mSeaAnimationCoroutine = null;
				}
				this.mSeaAnimationCoroutine = this.SeaAnimationCoroutine(this.mSeaDirection, this.mSeaMoveSpeed);
				base.StartCoroutine(this.mSeaAnimationCoroutine);
			}
			if (this.mTexture_SeaOverlay != null)
			{
				if (this.mSeaOverlayAnimationCoroutine != null)
				{
					base.StopCoroutine(this.mSeaOverlayAnimationCoroutine);
					this.mSeaOverlayAnimationCoroutine = null;
				}
				this.mSeaOverlayAnimationCoroutine = this.SeaOverlayAnimationCoroutine(this.mSeaOverlayDirection, this.mSeaOverlayMoveSpeed);
				base.StartCoroutine(this.mSeaOverlayAnimationCoroutine);
			}
		}

		[DebuggerHidden]
		private IEnumerator SeaAnimationCoroutine(UIDeckPracticeTrainingGround.Direction direction, float waveSpeed)
		{
			UIDeckPracticeTrainingGround.<SeaAnimationCoroutine>c__Iterator152 <SeaAnimationCoroutine>c__Iterator = new UIDeckPracticeTrainingGround.<SeaAnimationCoroutine>c__Iterator152();
			<SeaAnimationCoroutine>c__Iterator.direction = direction;
			<SeaAnimationCoroutine>c__Iterator.waveSpeed = waveSpeed;
			<SeaAnimationCoroutine>c__Iterator.<$>direction = direction;
			<SeaAnimationCoroutine>c__Iterator.<$>waveSpeed = waveSpeed;
			<SeaAnimationCoroutine>c__Iterator.<>f__this = this;
			return <SeaAnimationCoroutine>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator SeaOverlayAnimationCoroutine(UIDeckPracticeTrainingGround.Direction direction, float waveSpeed)
		{
			UIDeckPracticeTrainingGround.<SeaOverlayAnimationCoroutine>c__Iterator153 <SeaOverlayAnimationCoroutine>c__Iterator = new UIDeckPracticeTrainingGround.<SeaOverlayAnimationCoroutine>c__Iterator153();
			<SeaOverlayAnimationCoroutine>c__Iterator.direction = direction;
			<SeaOverlayAnimationCoroutine>c__Iterator.waveSpeed = waveSpeed;
			<SeaOverlayAnimationCoroutine>c__Iterator.<$>direction = direction;
			<SeaOverlayAnimationCoroutine>c__Iterator.<$>waveSpeed = waveSpeed;
			<SeaOverlayAnimationCoroutine>c__Iterator.<>f__this = this;
			return <SeaOverlayAnimationCoroutine>c__Iterator;
		}

		private void OnDisable()
		{
			if (this.mSeaAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mSeaAnimationCoroutine);
			}
		}
	}
}
