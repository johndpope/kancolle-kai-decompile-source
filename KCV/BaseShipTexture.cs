using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class BaseShipTexture : MonoBehaviour
	{
		[SerializeField]
		protected UITexture _uiShipTex;

		protected IShipModel _clsIShipModel;

		protected virtual void Load(int shipID, int texNum)
		{
			this._uiShipTex.mainTexture = KCV.Utils.ShipUtils.LoadTexture(shipID, texNum);
			this._uiShipTex.MakePixelPerfect();
		}

		protected virtual void SetShipTexture(ShipModel model)
		{
			this._clsIShipModel = model;
			if (model == null)
			{
				this._uiShipTex.mainTexture = null;
				this._uiShipTex.get_transform().get_localPosition().Zero();
				return;
			}
			this._uiShipTex.mainTexture = KCV.Utils.ShipUtils.LoadTexture(model);
			this._uiShipTex.MakePixelPerfect();
		}

		protected virtual void SetShipTexture(ShipModel_BattleResult model)
		{
			this._clsIShipModel = model;
			if (model == null)
			{
				this._uiShipTex.mainTexture = null;
				this._uiShipTex.get_transform().get_localPosition().Zero();
				return;
			}
			this._uiShipTex.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(model);
			this._uiShipTex.MakePixelPerfect();
		}

		protected virtual void OnDestroy()
		{
			this._uiShipTex = null;
			this._clsIShipModel = null;
			this.OnUnInit();
		}

		public virtual void Discard()
		{
			Object.Destroy(base.get_gameObject());
		}

		protected virtual void OnUnInit()
		{
		}
	}
}
