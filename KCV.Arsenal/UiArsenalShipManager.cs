using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalShipManager : MonoBehaviour
	{
		private const int NORMAL_SHIP_COUNT = 5;

		private const int LARGE_SHIP_COUNT = 5;

		private const int TANKER_SHIP_COUNT = 5;

		[SerializeField]
		private GameObject _mini1Obj;

		[SerializeField]
		private GameObject _mini2Obj;

		[SerializeField]
		private GameObject _mini3Obj;

		[SerializeField]
		private UISprite[] _ship1;

		[SerializeField]
		private UISprite[] _ship2;

		[SerializeField]
		private UISprite[] _ship3;

		private int _nowBuildCount;

		private int _fullCount;

		private ShipModelMst _ship;

		private BuildDockModel _dock;

		public void init(int num)
		{
			this._mini1Obj = base.get_transform().FindChild("ShipType1").get_gameObject();
			this._mini2Obj = base.get_transform().FindChild("ShipType2").get_gameObject();
			this._mini3Obj = base.get_transform().FindChild("ShipType3").get_gameObject();
			this._ship1 = new UISprite[5];
			this._ship2 = new UISprite[5];
			this._ship3 = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				this._ship1[i] = this._mini1Obj.get_transform().FindChild("Ship" + (i + 1)).GetComponent<UISprite>();
				this._ship1[i].alpha = 1f;
				this._ship1[i].get_transform().SetActive(false);
			}
			for (int j = 0; j < 5; j++)
			{
				this._ship2[j] = this._mini2Obj.get_transform().FindChild("Ship" + (j + 1)).GetComponent<UISprite>();
				this._ship2[j].get_transform().SetActive(false);
			}
			for (int k = 0; k < 5; k++)
			{
				this._ship3[k] = this._mini3Obj.get_transform().FindChild("Ship" + (k + 1)).GetComponent<UISprite>();
				this._ship3[k].get_transform().SetActive(false);
			}
		}

		public void set(ShipModelMst ship, BuildDockModel dock, bool isHight)
		{
			this._ship = ship;
			this._dock = dock;
			this._fullCount = this._dock.CompleteTurn - this._dock.StartTurn;
			this._nowBuildCount = this._fullCount - this._dock.GetTurn();
			if (this._dock.State == KdockStates.COMPLETE)
			{
				if (isHight)
				{
					this.SetFirstShip();
				}
				else
				{
					this.SetShipCmp();
				}
			}
			else
			{
				this.SetShip();
			}
		}

		private void SetFirstShip()
		{
			int num = 0;
			int[] array = new int[5];
			int num2;
			int num3;
			UISprite[] array2;
			if (this._dock.IsTunker())
			{
				num2 = (int)Math.Ceiling((double)this._fullCount / 5.0);
				num3 = 5;
				array2 = this._ship3;
			}
			else
			{
				num2 = (int)Math.Ceiling((double)this._fullCount / (double)this._ship.BuildStep);
				num3 = this._ship.BuildStep;
				array2 = this._ship1;
			}
			if (num2 == 0)
			{
				num = this._nowBuildCount;
				if (num > num3)
				{
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					array[i] = i * num2;
				}
				for (int j = 0; j < 5; j++)
				{
					if (j != 0 && array[j] <= this._nowBuildCount)
					{
						num++;
					}
					if (num > num3)
					{
						break;
					}
				}
			}
			array2[0].get_transform().SetActive(true);
		}

		private void SetShip()
		{
			int num = 0;
			int[] array = new int[5];
			int num2;
			int num3;
			UISprite[] array2;
			if (this._dock.IsTunker())
			{
				num2 = 5;
				num3 = (int)Math.Ceiling((double)this._fullCount / (double)num2);
				array2 = this._ship3;
			}
			else
			{
				num3 = (int)Math.Ceiling((double)this._fullCount / (double)this._ship.BuildStep);
				num2 = this._ship.BuildStep;
				array2 = this._ship1;
			}
			if (num3 == 0)
			{
				num = this._nowBuildCount;
				if (num > num2)
				{
					num = num2;
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					array[i] = i * num3;
				}
				for (int j = 0; j < 5; j++)
				{
					if (j != 0 && array[j] <= this._nowBuildCount)
					{
						num++;
					}
					if (num > num2)
					{
						num = num2;
						break;
					}
				}
			}
			for (int k = 0; k < 5; k++)
			{
				if (num >= k)
				{
					array2[k].get_transform().SetActive(true);
				}
			}
		}

		private void SetShipCmp()
		{
			UISprite[] array;
			int num;
			if (this._dock.IsTunker())
			{
				array = this._ship3;
				num = 4;
			}
			else
			{
				array = this._ship1;
				num = this._ship.BuildStep;
			}
			for (int i = 0; i < 5; i++)
			{
				if (num >= i)
				{
					array[i].get_transform().SetActive(true);
				}
			}
		}

		private void OnDestroy()
		{
			this._mini1Obj = null;
			this._mini2Obj = null;
			this._mini3Obj = null;
			this._ship1 = null;
			this._ship2 = null;
			this._ship3 = null;
			this._ship = null;
			this._dock = null;
		}
	}
}
