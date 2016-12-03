using System;
using UnityEngine;

namespace KCV
{
	public class RouletteSelector : MonoBehaviour
	{
		[SerializeField]
		private GameObject container;

		[SerializeField]
		private float horizontalRadius;

		[SerializeField]
		private float verticalRadius;

		[SerializeField]
		private float interval;

		[SerializeField]
		private float maxScale;

		[SerializeField]
		private float minScale;

		[SerializeField]
		private float maxAlpha;

		[SerializeField]
		private float minAlpha;

		[SerializeField]
		private int maxDepth;

		[SerializeField]
		private int minDepth;

		private int currentIdx;

		private bool moving;

		private float eachRadian;

		private float elapsedTime;

		private float moveRadian;

		private RouletteSelectorHandler handler;

		private KeyControl keyController;

		private bool dirty;

		private float PIx2 = 6.28318548f;

		private float OFFSET_PI = 1.57079637f;

		private float tmpRadian;

		private bool radiusScaling;

		private float scaleInterval;

		private float startRadiusScale;

		private float goalRadiusScale;

		private float radiusScaleElapsedTime;

		private float radiusScale = 1f;

		private int itemCount
		{
			get
			{
				return this.container.get_transform().get_childCount();
			}
		}

		public bool controllable
		{
			get;
			set;
		}

		public float intervalTime
		{
			get
			{
				return this.interval;
			}
			set
			{
				if (value != this.interval)
				{
					this.interval = value;
				}
			}
		}

		public float scalaMax
		{
			get
			{
				return this.maxScale;
			}
			set
			{
				if (value != this.maxScale)
				{
					this.maxScale = value;
				}
			}
		}

		public float scalaMin
		{
			get
			{
				return this.minScale;
			}
			set
			{
				if (value != this.minScale)
				{
					this.minScale = value;
				}
			}
		}

		public float alphaMax
		{
			get
			{
				return this.maxAlpha;
			}
			set
			{
				if (value != this.maxAlpha)
				{
					this.maxAlpha = value;
				}
			}
		}

		public float alphaMin
		{
			get
			{
				return this.minAlpha;
			}
			set
			{
				if (value != this.minAlpha)
				{
					this.minAlpha = value;
				}
			}
		}

		public int depthMax
		{
			get
			{
				return this.maxDepth;
			}
			set
			{
				if (value != this.maxDepth)
				{
					this.maxDepth = value;
				}
			}
		}

		public int depthMin
		{
			get
			{
				return this.minDepth;
			}
			set
			{
				if (value != this.minDepth)
				{
					this.minDepth = value;
				}
			}
		}

		public void Init(RouletteSelectorHandler handler)
		{
			this.handler = handler;
			this.eachRadian = this.PIx2 / (float)this.itemCount;
			this.goalRadiusScale = 1f;
			this.CancelMoving();
			this.CancelRadiusScaling();
			this.controllable = true;
			for (int i = 0; i < this.itemCount; i++)
			{
				if (handler.IsSelectable(i))
				{
					this.SetCurrent(i);
					break;
				}
			}
			this.Reposition();
		}

		public void SetCurrent(int idx)
		{
			this.currentIdx = idx;
			this.handler.OnUpdateIndex(this.currentIdx, this.container.get_transform().GetChild(this.currentIdx));
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.keyController = keyController;
		}

		public void SetHorizontalRadius(int value)
		{
			this.horizontalRadius = (float)value;
		}

		public void SetVerticalRadius(int value)
		{
			this.verticalRadius = (float)value;
		}

		public void Scale(float interval, float scale)
		{
			if (this.radiusScaling)
			{
				return;
			}
			this.startRadiusScale = this.radiusScale;
			this.goalRadiusScale = scale;
			this.radiusScaleElapsedTime = 0f;
			this.scaleInterval = interval;
			this.radiusScaling = true;
		}

		public void ScaleForce(float interval, float scale)
		{
			this.CancelRadiusScaling();
			this.Scale(interval, scale);
		}

		public void CancelMoving()
		{
			this.moving = false;
			this.dirty = true;
		}

		public void CancelRadiusScaling()
		{
			this.radiusScaling = false;
			this.radiusScale = this.goalRadiusScale;
			this.dirty = true;
		}

		public void Update()
		{
			if (base.get_enabled() && this.controllable && !this.moving && this.keyController != null)
			{
				if (this.keyController.IsLeftDown())
				{
					this.MovePrev();
				}
				else if (this.keyController.IsRightDown())
				{
					this.MoveNext();
				}
				else if (this.keyController.IsMaruDown())
				{
					this.Determine();
				}
			}
			if (this.moving)
			{
				this.Move();
			}
			if (this.radiusScaling)
			{
				this.DoScale();
			}
			if (this.dirty)
			{
				this.Reposition();
			}
		}

		private void Move()
		{
			this.dirty = true;
			this.elapsedTime += Time.get_deltaTime();
			if (this.elapsedTime >= this.interval)
			{
				this.moving = false;
				this.tmpRadian = 0f;
			}
			else
			{
				float num = this.elapsedTime / this.interval;
				this.tmpRadian = this.moveRadian * (1f - num);
			}
		}

		private void DoScale()
		{
			this.dirty = true;
			this.radiusScaleElapsedTime += Time.get_deltaTime();
			if (this.radiusScaleElapsedTime >= this.scaleInterval)
			{
				this.radiusScaling = false;
				this.radiusScale = this.goalRadiusScale;
			}
			else
			{
				float num = this.radiusScaleElapsedTime / this.scaleInterval;
				this.radiusScale = this.startRadiusScale * (1f - num) + this.goalRadiusScale * num;
			}
		}

		public void Reposition()
		{
			for (int i = 0; i < this.itemCount; i++)
			{
				float num = this.OFFSET_PI + this.eachRadian * (float)(this.currentIdx - i);
				if (this.moving)
				{
					num += this.tmpRadian;
				}
				float num2 = (float)Math.Sin((double)num);
				Transform child = this.container.get_transform().GetChild(i);
				child.localPositionX((float)((double)this.horizontalRadius * Math.Cos((double)num)) * this.radiusScale);
				child.localPositionY(-(this.verticalRadius * num2) * this.radiusScale);
				float num3 = (1f + num2) * 0.5f;
				float num4 = this.minScale + (this.maxScale - this.minScale) * num3;
				child.localScaleX(num4);
				child.localScaleY(num4);
				float alpha = this.minAlpha + (this.maxAlpha - this.minAlpha) * num3;
				UISprite component = child.GetComponent<UISprite>();
				if (component != null)
				{
					component.alpha = alpha;
				}
				UITexture component2 = child.GetComponent<UITexture>();
				if (component2 != null)
				{
					component2.alpha = alpha;
				}
				UIPanel component3 = child.GetComponent<UIPanel>();
				if (component3 != null)
				{
					component3.alpha = alpha;
				}
				UIWidget component4 = child.GetComponent<UIWidget>();
				if (component4 != null)
				{
					component4.alpha = alpha;
				}
				int depth = this.minDepth + (int)((float)(this.maxDepth - this.minDepth) * num3);
				if (component4 != null)
				{
					component4.depth = depth;
				}
				if (component3 != null)
				{
					component3.depth = depth;
				}
			}
		}

		public void MovePrev()
		{
			this.PrepareMove(false);
		}

		public void MoveNext()
		{
			this.PrepareMove(true);
		}

		public void Determine()
		{
			if (this.handler.IsSelectable(this.currentIdx))
			{
				Debug.Log("currentIdx:" + this.currentIdx);
				this.handler.OnSelect(this.currentIdx, this.container.get_transform().GetChild(this.currentIdx));
			}
		}

		private void PrepareMove(bool forward)
		{
			if (this.moving)
			{
				return;
			}
			int num = this.currentIdx;
			int num2 = 0;
			int num3 = (!forward) ? -1 : 1;
			while (true)
			{
				num += num3;
				if (num < 0)
				{
					num = this.itemCount - 1;
				}
				else if (num >= this.itemCount)
				{
					num = 0;
				}
				num2++;
				if (num2 >= this.itemCount)
				{
					break;
				}
				if (this.handler.IsSelectable(num))
				{
					goto Block_6;
				}
			}
			throw new Exception("選択可能な項目がありません。");
			Block_6:
			this.SetCurrent(num);
			this.elapsedTime = 0f;
			this.moving = true;
			this.moveRadian = this.eachRadian * (float)num2;
			if (forward)
			{
				this.moveRadian = -this.moveRadian;
			}
		}

		public GameObject GetContainer()
		{
			return this.container;
		}
	}
}
