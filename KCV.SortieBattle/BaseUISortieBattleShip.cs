using local.models;
using System;
using UnityEngine;

namespace KCV.SortieBattle
{
	[RequireComponent(typeof(UIPanel))]
	public class BaseUISortieBattleShip<ShipModelType> : BaseShipTexture where ShipModelType : IMemShip
	{
		protected UIPanel _uiPanel;

		[Header("Lov Parameter"), SerializeField]
		protected Vector3 originPos = Vector3.get_zero();

		[SerializeField]
		protected Vector3 lovMaxPos = Vector3.get_zero();

		public virtual ShipModelType shipModel
		{
			get
			{
				return (ShipModelType)((object)this._clsIShipModel);
			}
		}

		public virtual UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		protected virtual void SetLovOffset(ShipModelType model)
		{
		}

		protected override void OnUnInit()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
		}
	}
}
