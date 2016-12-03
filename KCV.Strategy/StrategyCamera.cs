using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyCamera : MonoBehaviour
	{
		private const int WIDTH = 960;

		private const int HEIGHT = 544;

		private Vector2 mapLeftTop;

		private Vector2 mapRightBottom;

		private Vector2 cameraLeftTop;

		private Vector2 cameraRightBottom;

		public Camera myCamera;

		private float mapX1;

		private float mapX2;

		private float mapY1;

		private float mapY2;

		private float cameraX1;

		private float cameraX2;

		private float cameraY1;

		private float cameraY2;

		private Blur blur;

		public bool isMoving
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.myCamera = base.get_gameObject().GetComponent<Camera>();
			this.blur = base.GetComponent<Blur>();
		}

		private void Start()
		{
			GameObject gameObject = GameObject.Find("Map_BG_Riku");
			Vector3 localPosition = gameObject.get_transform().get_localPosition();
			UITexture component = gameObject.GetComponent<UITexture>();
			float num = (float)component.width;
			float num2 = (float)component.height;
			base.get_transform().set_localPosition(StrategyTopTaskManager.Instance.TileManager.Tiles[1].getDefaultPosition());
			base.get_transform().localPositionZ(-500f);
			this.mapX1 = localPosition.x - num / 2f;
			this.mapX2 = localPosition.x + num / 2f;
			this.mapY1 = localPosition.y - num2 / 2f;
			this.mapY2 = localPosition.y + num2 / 2f;
			this.cameraX1 = this.myCamera.get_transform().get_position().x - 480f * this.myCamera.get_orthographicSize();
			this.cameraX2 = this.myCamera.get_transform().get_position().x + 480f * this.myCamera.get_orthographicSize();
			this.cameraY1 = this.myCamera.get_transform().get_position().y - 272f * this.myCamera.get_orthographicSize();
			this.cameraY2 = this.myCamera.get_transform().get_position().y + 272f * this.myCamera.get_orthographicSize();
		}

		public Vector3 FixPosition(Vector3 vec, float nextSize)
		{
			if (nextSize == 0f)
			{
				nextSize = this.myCamera.get_orthographicSize();
			}
			if ((double)nextSize < 0.6)
			{
				nextSize = 0.6f;
			}
			float num = vec.x;
			float num2 = vec.y;
			float z = this.myCamera.get_transform().get_localPosition().z;
			this.cameraX1 = num - 480f * nextSize;
			this.cameraX2 = num + 480f * nextSize;
			this.cameraY1 = num2 - 272f * nextSize;
			this.cameraY2 = num2 + 272f * nextSize;
			if (this.mapX1 > this.cameraX1)
			{
				num = this.mapX1 + 480f * nextSize;
			}
			if (this.mapX2 < this.cameraX2)
			{
				num = this.mapX2 - 480f * nextSize;
			}
			if (this.mapY1 > this.cameraY1)
			{
				num2 = this.mapY1 + 272f * nextSize;
			}
			if (this.mapY2 < this.cameraY2)
			{
				num2 = this.mapY2 - 272f * nextSize;
			}
			if (this.mapX1 <= this.cameraX1 && this.mapX2 >= this.cameraX2 && this.mapY1 <= this.cameraY1 && this.mapY2 >= this.cameraY2)
			{
				this.myCamera.set_orthographicSize(nextSize);
			}
			return new Vector3(num, num2, z);
		}

		public void FixSize(Vector3 targetTilePos, float nextSize)
		{
			if ((double)nextSize < 0.6 || 1.4 < (double)nextSize)
			{
				return;
			}
			this.myCamera.get_transform().set_position(targetTilePos);
			float num = this.myCamera.get_transform().get_localPosition().x;
			float y = this.myCamera.get_transform().get_localPosition().y;
			float z = this.myCamera.get_transform().get_localPosition().z;
			float num2 = (this.myCamera.get_orthographicSize() - nextSize) * 960f;
			float num3 = (this.myCamera.get_orthographicSize() - nextSize) * 544f;
			this.cameraX1 = num - 480f * nextSize;
			this.cameraX2 = num + 480f * nextSize;
			this.cameraY1 = y - 272f * nextSize;
			this.cameraY2 = y + 272f * nextSize;
			if (this.mapX1 > this.cameraX1)
			{
				num += 960f * (nextSize - this.myCamera.get_orthographicSize());
			}
			if (this.mapX2 < this.cameraX2)
			{
				num += 960f * (nextSize - this.myCamera.get_orthographicSize());
			}
			Debug.Log(this.mapX1);
			Debug.Log(this.cameraX1);
			if (this.mapX1 > this.cameraX1)
			{
				Debug.Log("fixsize");
				this.myCamera.get_transform().AddLocalPositionX(this.mapX1 - this.cameraX1);
			}
			if (this.mapX2 < this.cameraX2)
			{
				this.myCamera.get_transform().AddLocalPositionX(this.mapX2 - this.cameraX2);
			}
			if (this.mapY1 > this.cameraY1)
			{
				this.myCamera.get_transform().AddLocalPositionY(this.mapY1 - this.cameraY1);
			}
			if (this.mapY2 < this.cameraY2)
			{
				this.myCamera.get_transform().AddLocalPositionY(this.mapY2 - this.cameraY2);
			}
			this.myCamera.set_orthographicSize(nextSize);
		}

		public Coroutine MoveToTargetTile(int TargetAreaNo, bool immediate = false)
		{
			float time = (!immediate) ? 0.5f : 0f;
			return base.StartCoroutine(this.MoveToTargetTile(TargetAreaNo, time));
		}

		[DebuggerHidden]
		public IEnumerator MoveToTargetTile(int TargetAreaNo, float time)
		{
			StrategyCamera.<MoveToTargetTile>c__Iterator182 <MoveToTargetTile>c__Iterator = new StrategyCamera.<MoveToTargetTile>c__Iterator182();
			<MoveToTargetTile>c__Iterator.TargetAreaNo = TargetAreaNo;
			<MoveToTargetTile>c__Iterator.time = time;
			<MoveToTargetTile>c__Iterator.<$>TargetAreaNo = TargetAreaNo;
			<MoveToTargetTile>c__Iterator.<$>time = time;
			<MoveToTargetTile>c__Iterator.<>f__this = this;
			return <MoveToTargetTile>c__Iterator;
		}

		public void InitPositionTargetTile(int TargetAreaNo)
		{
			Vector3 defaultPosition = StrategyTopTaskManager.Instance.TileManager.Tiles[TargetAreaNo].getDefaultPosition();
			Vector3 localPosition = this.FixPosition(defaultPosition, 0f);
			base.get_transform().set_localPosition(localPosition);
		}

		public void setBlurEnable(bool enable)
		{
			this.blur.set_enabled(enable);
			if (enable)
			{
				this.blur.blurSize = 0f;
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					0,
					"to",
					2.5f,
					"time",
					0.5f,
					"onupdate",
					"OnBlurUpdate"
				}));
			}
		}

		private void OnBlurUpdate(float value)
		{
			this.blur.blurSize = value;
		}
	}
}
