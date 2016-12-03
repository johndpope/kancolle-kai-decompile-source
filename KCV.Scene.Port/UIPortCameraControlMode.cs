using KCV.Strategy;
using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortCameraControlMode : MonoBehaviour
	{
		[SerializeField]
		private Camera MenuCamera;

		[SerializeField]
		private Camera InteriorCamera;

		[SerializeField]
		private StrategyShipCharacter character;

		[SerializeField]
		private BoxCollider2D characterSlideCollider;

		private KeyControl key;

		private Action mOnFinishedModeListener;

		[Button("OnExitModeDebug", "OnExitMode", new object[]
		{

		})]
		public int Button1;

		private int WIDTH = 960;

		private int HEIGHT = 544;

		private float mapX1;

		private float mapX2;

		private float mapY1;

		private float mapY2;

		private float cameraX1;

		private float cameraX2;

		private float cameraY1;

		private float cameraY2;

		private void Start()
		{
			this.characterSlideCollider.set_enabled(false);
		}

		public void SetKeyController(KeyControl keyControl)
		{
			this.key = keyControl;
		}

		public void Init()
		{
			this.characterSlideCollider.set_enabled(true);
			Vector3 zero = Vector3.get_zero();
			this.mapX1 = zero.x - (float)(this.WIDTH / 2);
			this.mapX2 = zero.x + (float)(this.WIDTH / 2);
			this.mapY1 = zero.y - (float)(this.HEIGHT / 2);
			this.mapY2 = zero.y + (float)(this.HEIGHT / 2);
			this.cameraX1 = this.MenuCamera.get_transform().get_position().x - (float)(this.WIDTH / 2) * this.MenuCamera.get_orthographicSize();
			this.cameraX2 = this.MenuCamera.get_transform().get_position().x + (float)(this.WIDTH / 2) * this.MenuCamera.get_orthographicSize();
			this.cameraY1 = this.MenuCamera.get_transform().get_position().y - (float)(this.HEIGHT / 2) * this.MenuCamera.get_orthographicSize();
			this.cameraY2 = this.MenuCamera.get_transform().get_position().y + (float)(this.HEIGHT / 2) * this.MenuCamera.get_orthographicSize();
		}

		private void Update()
		{
			if (this.key != null)
			{
				float axisRaw = Input.GetAxisRaw("Left Stick Horizontal");
				float axisRaw2 = Input.GetAxisRaw("Left Stick Vertical");
				if (this.key.keyState.get_Item(16).press || this.key.keyState.get_Item(23).press || this.key.keyState.get_Item(17).press)
				{
					Camera expr_7B = this.MenuCamera;
					expr_7B.set_orthographicSize(expr_7B.get_orthographicSize() - 0.3f * Time.get_deltaTime());
					Camera expr_98 = this.InteriorCamera;
					expr_98.set_orthographicSize(expr_98.get_orthographicSize() - 0.3f * Time.get_deltaTime());
				}
				else if (this.key.keyState.get_Item(20).press || this.key.keyState.get_Item(21).press || this.key.keyState.get_Item(19).press)
				{
					Camera expr_10E = this.MenuCamera;
					expr_10E.set_orthographicSize(expr_10E.get_orthographicSize() + 0.3f * Time.get_deltaTime());
					Camera expr_12B = this.InteriorCamera;
					expr_12B.set_orthographicSize(expr_12B.get_orthographicSize() + 0.3f * Time.get_deltaTime());
				}
				else if (this.key.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else
				{
					if (this.key.IsSankakuDown())
					{
						this.key.ClearKeyAll();
						this.key.firstUpdate = true;
						this.ExitMode();
						return;
					}
					if (axisRaw == 0f && axisRaw2 == 0f)
					{
						if (this.key.keyState.get_Item(10).press)
						{
							this.character.get_transform().AddLocalPositionX(250f * Time.get_deltaTime());
							if (this.character.get_transform().get_localPosition().x > 400f)
							{
								this.character.get_transform().localPositionX(400f);
							}
						}
						else if (this.key.keyState.get_Item(14).press)
						{
							this.character.get_transform().AddLocalPositionX(-250f * Time.get_deltaTime());
							if (this.character.get_transform().get_localPosition().x < -400f)
							{
								this.character.get_transform().localPositionX(-400f);
							}
						}
					}
				}
				if (this.key.keyState.get_Item(8).press)
				{
					float num = -0.1f * Time.get_deltaTime();
					if (this.character.get_transform().get_localScale().x + num > 1.1f)
					{
						this.character.get_transform().AddLocalScale(num, num, num);
						this.character.get_transform().AddLocalPositionY((float)this.character.render.height * -num / 4f);
					}
				}
				else if (this.key.keyState.get_Item(12).press)
				{
					float num2 = 0.1f * Time.get_deltaTime();
					float num3 = 0.5f * (float)this.character.shipModel.Lov / 1000f;
					if (this.character.get_transform().get_localScale().x + num2 < 1.1f + num3)
					{
						this.character.get_transform().AddLocalScale(num2, num2, num2);
						this.character.get_transform().AddLocalPositionY((float)this.character.render.height * -num2 / 4f);
					}
				}
				this.MenuCamera.get_transform().AddPosX(axisRaw * Time.get_deltaTime());
				this.MenuCamera.get_transform().AddPosY(-axisRaw2 * Time.get_deltaTime());
				this.InteriorCamera.get_transform().AddPosX(axisRaw * Time.get_deltaTime());
				this.InteriorCamera.get_transform().AddPosY(-axisRaw2 * Time.get_deltaTime());
				this.FixSize(this.MenuCamera);
				this.FixSize(this.InteriorCamera);
				this.FixPosition(this.MenuCamera);
				this.FixPosition(this.InteriorCamera);
			}
		}

		private Vector3 FixPosition(Camera myCamera)
		{
			float orthographicSize = myCamera.get_orthographicSize();
			Vector3 localPosition = myCamera.get_transform().get_localPosition();
			float num = localPosition.x;
			float num2 = localPosition.y;
			float z = myCamera.get_transform().get_localPosition().z;
			this.cameraX1 = num - (float)(this.WIDTH / 2) * orthographicSize;
			this.cameraX2 = num + (float)(this.WIDTH / 2) * orthographicSize;
			this.cameraY1 = num2 - (float)(this.HEIGHT / 2) * orthographicSize;
			this.cameraY2 = num2 + (float)(this.HEIGHT / 2) * orthographicSize;
			if (this.mapX1 > this.cameraX1)
			{
				num = this.mapX1 + (float)(this.WIDTH / 2) * orthographicSize;
			}
			if (this.mapX2 < this.cameraX2)
			{
				num = this.mapX2 - (float)(this.WIDTH / 2) * orthographicSize;
			}
			if (this.mapY1 > this.cameraY1)
			{
				num2 = this.mapY1 + (float)(this.HEIGHT / 2) * orthographicSize;
			}
			if (this.mapY2 < this.cameraY2)
			{
				num2 = this.mapY2 - (float)(this.HEIGHT / 2) * orthographicSize;
			}
			return new Vector3(num, num2, z);
		}

		private void FixSize(Camera myCamera)
		{
			float num = myCamera.get_orthographicSize();
			if (num == 0f)
			{
				num = myCamera.get_orthographicSize();
			}
			if ((double)num < 0.5)
			{
				num = 0.5f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			float num2 = myCamera.get_transform().get_localPosition().x;
			float y = myCamera.get_transform().get_localPosition().y;
			float z = myCamera.get_transform().get_localPosition().z;
			float num3 = (myCamera.get_orthographicSize() - num) * (float)this.WIDTH;
			float num4 = (myCamera.get_orthographicSize() - num) * (float)this.HEIGHT;
			this.cameraX1 = num2 - (float)(this.WIDTH / 2) * num;
			this.cameraX2 = num2 + (float)(this.WIDTH / 2) * num;
			this.cameraY1 = y - (float)(this.HEIGHT / 2) * num;
			this.cameraY2 = y + (float)(this.HEIGHT / 2) * num;
			if (this.mapX1 > this.cameraX1)
			{
				num2 += (float)this.WIDTH * (num - myCamera.get_orthographicSize());
			}
			if (this.mapX2 < this.cameraX2)
			{
				num2 += (float)this.WIDTH * (num - myCamera.get_orthographicSize());
			}
			if (this.mapX1 > this.cameraX1)
			{
				myCamera.get_transform().AddLocalPositionX(this.mapX1 - this.cameraX1);
			}
			if (this.mapX2 < this.cameraX2)
			{
				myCamera.get_transform().AddLocalPositionX(this.mapX2 - this.cameraX2);
			}
			if (this.mapY1 > this.cameraY1)
			{
				myCamera.get_transform().AddLocalPositionY(this.mapY1 - this.cameraY1);
			}
			if (this.mapY2 < this.cameraY2)
			{
				myCamera.get_transform().AddLocalPositionY(this.mapY2 - this.cameraY2);
			}
			myCamera.set_orthographicSize(num);
		}

		public void SetOnFinishedModeListener(Action onFinishedModeListener)
		{
			this.mOnFinishedModeListener = onFinishedModeListener;
		}

		private void OnFinishedExitMode()
		{
			if (this.mOnFinishedModeListener != null)
			{
				this.mOnFinishedModeListener.Invoke();
			}
		}

		public void ExitMode()
		{
			this.key = null;
			this.characterSlideCollider.set_enabled(false);
			if (this.character.getEnterPosition().x == this.character.get_transform().get_localPosition().x)
			{
				bool flag = this.MenuCamera.get_gameObject().get_transform().get_localPosition() == Vector3.get_zero() && this.MenuCamera.get_orthographicSize() == 1f;
				bool flag2 = this.InteriorCamera.get_gameObject().get_transform().get_localPosition() == Vector3.get_zero() && this.InteriorCamera.get_orthographicSize() == 1f;
				if (flag && flag2)
				{
					this.OnFinishedExitMode();
					return;
				}
			}
			this.character.moveCharacterX(this.character.getEnterPosition().x, 0.5f, new Action(this.OnFinishedExitMode));
			Util.MoveTo(this.MenuCamera.get_gameObject(), 0.5f, Vector3.get_zero(), iTween.EaseType.easeOutQuad);
			Util.MoveTo(this.InteriorCamera.get_gameObject(), 0.5f, Vector3.get_zero(), iTween.EaseType.easeOutQuad);
			TweenScale.Begin(this.character.get_gameObject(), 0.5f, new Vector3(1.1f, 1.1f, 1.1f));
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"onupdate",
				"CameraResetUpdate",
				"time",
				0.5f,
				"from",
				this.MenuCamera.get_orthographicSize(),
				"to",
				1
			}));
		}

		private void CameraResetUpdate(float value)
		{
			this.MenuCamera.set_orthographicSize(value);
			this.InteriorCamera.set_orthographicSize(value);
		}

		private void OnDestroy()
		{
			this.MenuCamera = null;
			this.InteriorCamera = null;
			this.character = null;
			this.key = null;
			this.mOnFinishedModeListener = null;
		}

		private void OnExitModeDebug()
		{
			this.ExitMode();
		}
	}
}
