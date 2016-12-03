using System;
using UnityEngine;

namespace KCV.Supply
{
	public class SupplyStorage : MonoBehaviour
	{
		private int STORAGE_LENGTH = 6;

		[SerializeField]
		private UISprite _uiStorage;

		private string[] _spriteName;

		private int _fromIndex;

		private int _toIndex;

		private float _animeTimer;

		private bool _isAnimate;

		public float ChangeTime;

		public void init(int num, float changeTime)
		{
			this._fromIndex = num;
			this._toIndex = 0;
			this._animeTimer = 0f;
			this._isAnimate = false;
			this.ChangeTime = changeTime;
			this._spriteName = new string[this.STORAGE_LENGTH];
			for (int i = 0; i < this.STORAGE_LENGTH; i++)
			{
				this._spriteName[i] = "sizai_gauge" + i;
			}
			Util.FindParentToChild<UISprite>(ref this._uiStorage, base.get_transform(), "Storage");
			this._uiStorage.spriteName = this._spriteName[this._fromIndex];
		}

		private void Update()
		{
			if (this._isAnimate)
			{
				if (this._fromIndex < this._toIndex)
				{
					this._animeTimer += Time.get_deltaTime();
					if (this._animeTimer >= this.ChangeTime)
					{
						this._fromIndex++;
						this._animeTimer = 0f;
						this._uiStorage.spriteName = this._spriteName[this._fromIndex];
					}
				}
				else
				{
					this._animeTimer = 0f;
					this._isAnimate = false;
				}
			}
		}

		public void UpdateStorage(int num)
		{
			for (int i = 0; i < this.STORAGE_LENGTH; i++)
			{
				if (num == i)
				{
					this._toIndex = i;
					this._isAnimate = true;
					break;
				}
			}
		}

		public void Stop()
		{
			this._isAnimate = false;
		}

		public void End()
		{
			this._isAnimate = false;
			this._fromIndex = this._toIndex;
			this._uiStorage.spriteName = this._spriteName[this._fromIndex];
		}
	}
}
