using KCV;
using System;
using UnityEngine;

public class UIAlbumShipViewerCameraController : MonoBehaviour
{
	[SerializeField]
	private Camera MenuCamera;

	private KeyControl mKeyController;

	private int mWidth = 960;

	private int mHeight = 544;

	private float mMaximumOrthographicSize;

	private float mapX1;

	private float mapX2;

	private float mapY1;

	private float mapY2;

	private float cameraX1;

	private float cameraX2;

	private float cameraY1;

	private float cameraY2;

	private Action mOnFinishedModeListener;

	public void SetKeyController(KeyControl keyControl)
	{
		this.mKeyController = keyControl;
	}

	public void Initialize(int width, int height, float maximumOrthographicSize)
	{
		width = 2048;
		height = 2048;
		this.mWidth = width;
		this.mHeight = height;
		this.mMaximumOrthographicSize = maximumOrthographicSize;
		Vector3 zero = Vector3.get_zero();
		this.mapX1 = zero.x - (float)(this.mWidth / 2);
		this.mapX2 = zero.x + (float)(this.mWidth / 2);
		this.mapY1 = zero.y - (float)(this.mHeight / 2);
		this.mapY2 = zero.y + (float)(this.mHeight / 2);
		this.cameraX1 = this.MenuCamera.get_transform().get_position().x - (float)(this.mWidth / 2) * this.MenuCamera.get_orthographicSize();
		this.cameraX2 = this.MenuCamera.get_transform().get_position().x + (float)(this.mWidth / 2) * this.MenuCamera.get_orthographicSize();
		this.cameraY1 = this.MenuCamera.get_transform().get_position().y - (float)(this.mHeight / 2) * this.MenuCamera.get_orthographicSize();
		this.cameraY2 = this.MenuCamera.get_transform().get_position().y + (float)(this.mHeight / 2) * this.MenuCamera.get_orthographicSize();
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(16).press || this.mKeyController.keyState.get_Item(23).press || this.mKeyController.keyState.get_Item(17).press)
			{
				Camera expr_65 = this.MenuCamera;
				expr_65.set_orthographicSize(expr_65.get_orthographicSize() - 0.3f * Time.get_deltaTime());
			}
			else if (this.mKeyController.keyState.get_Item(20).press || this.mKeyController.keyState.get_Item(21).press || this.mKeyController.keyState.get_Item(19).press)
			{
				Camera expr_DB = this.MenuCamera;
				expr_DB.set_orthographicSize(expr_DB.get_orthographicSize() + 0.3f * Time.get_deltaTime());
			}
			float axisRaw = Input.GetAxisRaw("Left Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Left Stick Vertical");
			this.MenuCamera.get_transform().AddPosX(axisRaw * Time.get_deltaTime());
			this.MenuCamera.get_transform().AddPosY(-axisRaw2 * Time.get_deltaTime());
			this.FixSize(this.MenuCamera);
			this.FixPosition(this.MenuCamera);
		}
	}

	private Vector3 FixPosition(Camera myCamera)
	{
		float orthographicSize = myCamera.get_orthographicSize();
		Vector3 localPosition = myCamera.get_transform().get_localPosition();
		float num = localPosition.x;
		float num2 = localPosition.y;
		float z = myCamera.get_transform().get_localPosition().z;
		this.cameraX1 = num - (float)(this.mWidth / 2) * orthographicSize;
		this.cameraX2 = num + (float)(this.mWidth / 2) * orthographicSize;
		this.cameraY1 = num2 - (float)(this.mHeight / 2) * orthographicSize;
		this.cameraY2 = num2 + (float)(this.mHeight / 2) * orthographicSize;
		if (this.mapX1 > this.cameraX1)
		{
			num = this.mapX1 + (float)(this.mWidth / 2) * orthographicSize;
		}
		if (this.mapX2 < this.cameraX2)
		{
			num = this.mapX2 - (float)(this.mWidth / 2) * orthographicSize;
		}
		if (this.mapY1 > this.cameraY1)
		{
			num2 = this.mapY1 + (float)(this.mHeight / 2) * orthographicSize;
		}
		if (this.mapY2 < this.cameraY2)
		{
			num2 = this.mapY2 - (float)(this.mHeight / 2) * orthographicSize;
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
		if (num > this.mMaximumOrthographicSize)
		{
			num = this.mMaximumOrthographicSize;
		}
		float num2 = myCamera.get_transform().get_localPosition().x;
		float y = myCamera.get_transform().get_localPosition().y;
		float z = myCamera.get_transform().get_localPosition().z;
		float num3 = (myCamera.get_orthographicSize() - num) * (float)this.mWidth;
		float num4 = (myCamera.get_orthographicSize() - num) * (float)this.mHeight;
		this.cameraX1 = num2 - (float)(this.mWidth / 2) * num;
		this.cameraX2 = num2 + (float)(this.mWidth / 2) * num;
		this.cameraY1 = y - (float)(this.mHeight / 2) * num;
		this.cameraY2 = y + (float)(this.mHeight / 2) * num;
		if (this.mapX1 > this.cameraX1)
		{
			num2 += (float)this.mWidth * (num - myCamera.get_orthographicSize());
		}
		if (this.mapX2 < this.cameraX2)
		{
			num2 += (float)this.mWidth * (num - myCamera.get_orthographicSize());
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

	public void QuitState()
	{
	}

	private void OnDestroy()
	{
		this.MenuCamera = null;
		this.mKeyController = null;
	}
}
