using KCV.Production;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDockShipManager : MonoBehaviour
	{
		private ProdReceiveShip _prodReceiveShip;

		private BuildDockModel dock;

		[SerializeField]
		private GameObject _type1Obj;

		[SerializeField]
		private GameObject _type2Obj;

		[SerializeField]
		private UISprite[] _ship1;

		[SerializeField]
		private UISprite[] _ship2;

		private int stateIndex;

		public void init(BuildDockModel _dock)
		{
			this.dock = _dock;
			this._type1Obj = base.get_transform().FindChild("ShipType1").get_gameObject();
			this._type2Obj = base.get_transform().FindChild("ShipType2").get_gameObject();
			this._ship1 = new UISprite[6];
			for (int i = 0; i < 6; i++)
			{
				this._ship1[i] = this._type1Obj.get_transform().FindChild("Ship" + (i + 1)).GetComponent<UISprite>();
			}
			this.showInit();
		}

		public void showInit()
		{
			for (int i = 0; i < 6; i++)
			{
				this._ship1[i].alpha = 0f;
			}
		}

		public void set(int num, int limit)
		{
			int num2 = (this.dock.CompleteTurn - this.dock.StartTurn) / num;
			Debug.Log("CNT:" + num2);
			int num3 = 0;
			for (int i = 0; i < 6; i++)
			{
				if (limit >= num2 * i)
				{
					num3 = i;
				}
			}
			Debug.Log("__CNT:" + num3);
		}

		public void end()
		{
		}
	}
}
