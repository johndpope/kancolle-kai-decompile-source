using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRebellionSelectorShip : MonoBehaviour
	{
		[SerializeField]
		private UIWidget _uiWiget;

		[SerializeField]
		private UITexture _uiShipTexture;

		private ShipModel _clsShipModel;

		public ShipModel shipModel
		{
			get
			{
				return this._clsShipModel;
			}
		}

		public float textureAlpha
		{
			get
			{
				return this._uiShipTexture.alpha;
			}
			set
			{
				this._uiShipTexture.alpha = value;
			}
		}

		public static UIRebellionSelectorShip Instantiate(UIRebellionSelectorShip prefab, Transform parent, Vector3 pos, ShipModel model)
		{
			UIRebellionSelectorShip uIRebellionSelectorShip = Object.Instantiate<UIRebellionSelectorShip>(prefab);
			uIRebellionSelectorShip.get_transform().set_parent(parent);
			uIRebellionSelectorShip.get_transform().localScaleOne();
			uIRebellionSelectorShip.get_transform().set_localPosition(pos);
			uIRebellionSelectorShip.SetShipTexture(model);
			return uIRebellionSelectorShip;
		}

		private void Awake()
		{
			if (this._uiWiget == null)
			{
				this._uiWiget = base.GetComponent<UIWidget>();
			}
			if (this._uiShipTexture == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiShipTexture, base.get_transform(), "ShipTexture");
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UIWidget>(ref this._uiWiget);
			Mem.Del<UITexture>(ref this._uiShipTexture);
			Mem.Del<ShipModel>(ref this._clsShipModel);
		}

		public bool Init(ShipModel model)
		{
			this.SetShipTexture(model);
			return true;
		}

		private void SetShipTexture(ShipModel model)
		{
			this._clsShipModel = model;
			if (model == null)
			{
				this._uiShipTexture.mainTexture = null;
				this._uiShipTexture.get_transform().localPositionZero();
				return;
			}
			this._uiShipTexture.mainTexture = ShipUtils.LoadTexture(model);
			this._uiShipTexture.MakePixelPerfect();
			this._uiShipTexture.get_transform().set_localPosition(Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(model.IsDamaged())));
		}
	}
}
