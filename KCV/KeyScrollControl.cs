using System;
using UnityEngine;

namespace KCV
{
	public class KeyScrollControl
	{
		public int index;

		public int topIndex;

		public int underIndex;

		public int viewRange;

		public int objCount;

		private UIScrollBar _scrollBar;

		public KeyScrollControl(int range, int count, UIScrollBar scrollBar)
		{
			this.index = 1;
			this.viewRange = range;
			this.topIndex = 1;
			this.underIndex = range;
			this.objCount = count;
			this._scrollBar = scrollBar;
		}

		private void Start()
		{
		}

		public void SetKeyScroll(KeyControl.KeyName key)
		{
			if (key == KeyControl.KeyName.DOWN)
			{
				if (this.index == this.objCount)
				{
					return;
				}
				if (this.CheckScrollBar())
				{
					this.index = this.topIndex;
				}
				else
				{
					if (this.index < this.objCount)
					{
						this.index++;
					}
					if (this.underIndex < this.index)
					{
						this.MoveScrollBar(key);
						this.topIndex++;
						this.underIndex++;
						Debug.Log(string.Concat(new object[]
						{
							"True top",
							this.topIndex,
							" under:",
							this.underIndex,
							"  index:",
							this.index
						}));
					}
				}
			}
			else if (key == KeyControl.KeyName.UP)
			{
				if (this.index == 1)
				{
					return;
				}
				if (this.CheckScrollBar())
				{
					this.index = this.topIndex;
				}
				else
				{
					if (this.index > 1)
					{
						this.index--;
					}
					if (this.topIndex > this.index)
					{
						this.MoveScrollBar(key);
						this.topIndex--;
						this.underIndex--;
						Debug.Log(string.Concat(new object[]
						{
							"True top",
							this.topIndex,
							" under:",
							this.underIndex,
							"  index:",
							this.index
						}));
					}
				}
			}
		}

		public void MoveScrollBar(KeyControl.KeyName key)
		{
			float num = (float)(this.objCount - this.viewRange);
			if (num <= 0f)
			{
				return;
			}
			float num2 = 1f / num;
			Debug.Log(key);
			if (key == KeyControl.KeyName.DOWN)
			{
				if (this._scrollBar.value < 1f)
				{
					this._scrollBar.value += num2;
				}
			}
			else if (key == KeyControl.KeyName.UP)
			{
				if (this._scrollBar.value > 0f)
				{
					this._scrollBar.value -= num2;
				}
			}
		}

		public bool CheckScrollBar()
		{
			float num = (float)(this.objCount - (this.viewRange - 1));
			float num2 = 1f / num;
			int num3 = 0;
			int num4 = 0;
			while ((float)num4 < num)
			{
				if (this._scrollBar.value < num2 * (float)(num4 + 1))
				{
					break;
				}
				num3++;
				if (1f <= num2 * (float)(num4 + 1))
				{
					num3 = (int)num;
				}
				num4++;
			}
			if (this.index == this.objCount && this._scrollBar.value >= 1f)
			{
				return false;
			}
			this.topIndex = num3 + 1;
			this.underIndex = this.topIndex + (this.viewRange - 1);
			Debug.Log(string.Concat(new object[]
			{
				"CheckScrollBar  Top:",
				this.topIndex,
				" Und:",
				this.underIndex,
				" List:",
				this.index
			}));
			return this.index < this.topIndex || this.index > this.underIndex;
		}

		public void ChangeObjAllCount(int cnt)
		{
			this.objCount = cnt;
		}

		private void Update()
		{
		}
	}
}
