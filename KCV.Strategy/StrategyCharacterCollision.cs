using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyCharacterCollision : MonoBehaviour
	{
		[SerializeField]
		private UITexture liveTex;

		[SerializeField]
		private UIFlagShip UIflagShip;

		private bool failed;

		[SerializeField]
		private ParticleSystem heartUpPar;

		[SerializeField]
		private ParticleSystem heartDownPar;

		[SerializeField]
		private Camera camera;

		private void Start()
		{
			if (this.UIflagShip != null)
			{
				this.UIflagShip.SetOnBackTouchCallBack(new Action<bool, bool, bool>(this.HeartAction));
			}
		}

		public void SetCollisionHight(int height)
		{
			BoxCollider2D component = base.GetComponent<BoxCollider2D>();
			component.set_size(new Vector2(component.get_size().x, (float)height));
		}

		public void OnClick()
		{
			StrategyShipCharacter component = this.UIflagShip.GetComponent<StrategyShipCharacter>();
			if (component == null || component.shipModel == null)
			{
				return;
			}
			int num = this.UIflagShip.TouchedPartnerShip(component.shipModel);
			ShipUtils.PlayShipVoice(component.shipModel, num);
			int lov = component.shipModel.Lov;
			component.shipModel.LovAction(0, num);
			bool isLovUp = component.shipModel.Lov - lov > 0;
			bool isLovDown = component.shipModel.Lov - lov < 0;
			this.PlayMotion(component, isLovUp, isLovDown);
			SingletonMonoBehaviour<Live2DModel>.Instance.Play();
		}

		public void ResetTouchCount()
		{
			this.UIflagShip.ResetClickedCount();
		}

		private void PlayMotion(StrategyShipCharacter character, bool isLovUp, bool isLovDown)
		{
			this.HeartAction(isLovUp, isLovDown, false);
			if (this.UIflagShip.getClickedCount() == 0)
			{
				if (character.shipModel.IsMarriage())
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Secret);
				}
				else
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Port);
				}
			}
			else if (this.UIflagShip.getClickedCount() < 4)
			{
				if (character.shipModel.Lov >= 700)
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion((Live2DModel.MotionType)Random.Range(6, 8));
				}
				else if (character.shipModel.Lov >= 500)
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Love1);
				}
				else
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Port);
				}
			}
			else if (character.shipModel.Lov >= 25 || character.shipModel.IsMarriage())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Port);
			}
			else if (character.shipModel.Lov <= 10)
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion((Live2DModel.MotionType)Random.Range(4, 6));
			}
			else
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Dislike2);
			}
		}

		public void HeartAction(bool isLovUp, bool isLovDown, bool isBackTouch)
		{
			if (isLovUp)
			{
				if (isBackTouch)
				{
					float y = base.get_transform().get_parent().get_parent().get_localPosition().y;
					this.heartUpPar.get_transform().set_localPosition(new Vector3(0f, -y, 0f));
				}
				else if (this.camera != null)
				{
					this.heartUpPar.get_transform().set_position(this.camera.ScreenToWorldPoint(Input.get_mousePosition()));
				}
				this.heartUpPar.Stop();
				this.heartUpPar.Clear();
				this.heartUpPar.Play();
			}
			else if (isLovDown)
			{
				if (isBackTouch)
				{
					float y2 = base.get_transform().get_parent().get_parent().get_localPosition().y;
					this.heartDownPar.get_transform().set_localPosition(new Vector3(0f, -y2, 0f));
				}
				else if (this.camera != null)
				{
					this.heartDownPar.get_transform().set_position(this.camera.ScreenToWorldPoint(Input.get_mousePosition()));
				}
				this.heartDownPar.Stop();
				this.heartDownPar.Clear();
				this.heartDownPar.Play();
			}
		}

		public void SetEnableBackTouch(bool isEnable)
		{
			this.UIflagShip.isEnableBackTouch = isEnable;
		}

		private void OnDestroy()
		{
			this.liveTex = null;
			this.UIflagShip = null;
			this.heartUpPar = null;
			this.heartDownPar = null;
			this.camera = null;
		}
	}
}
