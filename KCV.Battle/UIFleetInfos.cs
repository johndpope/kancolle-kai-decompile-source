using KCV.BattleCut;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIWidget))]
	public class UIFleetInfos : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabUIShortCutHPBar;

		[SerializeField]
		private Transform _traAnchor;

		private List<BtlCut_HPBar> _listHPBar;

		private UIWidget _uiWidget;

		public UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		public static UIFleetInfos Instantiate(UIFleetInfos prefab, Transform parent, Vector3 pos, List<ShipModel_BattleAll> ships)
		{
			UIFleetInfos uIFleetInfos = Object.Instantiate<UIFleetInfos>(prefab);
			uIFleetInfos.get_transform().set_parent(parent);
			uIFleetInfos.get_transform().localScaleOne();
			uIFleetInfos.get_transform().set_localPosition(pos);
			uIFleetInfos.Init(ships);
			return uIFleetInfos;
		}

		public bool Init(List<ShipModel_BattleAll> ships)
		{
			this._listHPBar = new List<BtlCut_HPBar>();
			ships.ForEach(delegate(ShipModel_BattleAll x)
			{
				if (x != null)
				{
					this._listHPBar.Add(BtlCut_HPBar.Instantiate(this._prefabUIShortCutHPBar.GetComponent<BtlCut_HPBar>(), this._traAnchor, x, true, BattleTaskManager.GetBattleManager()));
					this._listHPBar.get_Item(x.Index).SetHPLabelColor(Color.get_white());
				}
			});
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabUIShortCutHPBar);
			Mem.Del<Transform>(ref this._traAnchor);
			Mem.DelListSafe<BtlCut_HPBar>(ref this._listHPBar);
			Mem.Del<UIWidget>(ref this._uiWidget);
		}

		public LTDescr Show()
		{
			return this._uiWidget.get_transform().LTValue(this._uiWidget.alpha, 1f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiWidget.alpha = x;
			});
		}

		public LTDescr Hide()
		{
			return this._uiWidget.get_transform().LTValue(this._uiWidget.alpha, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiWidget.alpha = x;
			});
		}
	}
}
