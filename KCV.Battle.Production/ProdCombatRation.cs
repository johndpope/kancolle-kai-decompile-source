using Common.Struct;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(Animation))]
	public class ProdCombatRation : MonoBehaviour
	{
		private const int PARTICIPANTS_NUM = 3;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		[SerializeField]
		private UITexture _uiRationIcon;

		[SerializeField]
		private UIWidget _uiShipsAnchor;

		[SerializeField]
		private List<UITexture> _listShipTexture;

		private UIPanel _uiPanel;

		private Animation _anim;

		private RationModel _clsRationModel;

		private Action _actOnStageReady;

		private Action _actOnFinished;

		private List<ShipModel_Eater> _listShips;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private Animation animation
		{
			get
			{
				return this.GetComponentThis(ref this._anim);
			}
		}

		public static ProdCombatRation Instantiate(ProdCombatRation prefab, Transform parent, RationModel model)
		{
			ProdCombatRation prodCombatRation = Object.Instantiate<ProdCombatRation>(prefab);
			prodCombatRation.get_transform().set_parent(parent);
			prodCombatRation.get_transform().localScaleZero();
			prodCombatRation.get_transform().localPositionZero();
			prodCombatRation.Init(model);
			return prodCombatRation;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiForeground);
			Mem.Del<UITexture>(ref this._uiRationIcon);
			Mem.Del<UIWidget>(ref this._uiShipsAnchor);
			Mem.DelListSafe<UITexture>(ref this._listShipTexture);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<RationModel>(ref this._clsRationModel);
			Mem.Del<Action>(ref this._actOnFinished);
			Mem.DelListSafe<ShipModel_Eater>(ref this._listShips);
		}

		private bool Init(RationModel model)
		{
			this._clsRationModel = model;
			this._uiShipsAnchor.alpha = 0.01f;
			this._listShips = new List<ShipModel_Eater>(3);
			this.SetShipsInfos(model.EatingShips, model.SharedShips);
			this.SetShipTexture(this._listShips);
			return true;
		}

		private void SetShipsInfos(List<ShipModel_Eater> eatingShips, List<ShipModel_Eater> sharedShips)
		{
			using (List<ShipModel_Eater>.Enumerator enumerator = eatingShips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipModel_Eater current = enumerator.get_Current();
					if (current != null && this._listShips.get_Count() < this._listShips.get_Capacity())
					{
						this._listShips.Add(current);
					}
				}
			}
			using (List<ShipModel_Eater>.Enumerator enumerator2 = sharedShips.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ShipModel_Eater current2 = enumerator2.get_Current();
					if (current2 != null && this._listShips.get_Count() < this._listShips.get_Capacity())
					{
						this._listShips.Add(current2);
					}
				}
			}
		}

		private void SetShipTexture(List<ShipModel_Eater> ships)
		{
			switch (ships.get_Count())
			{
			case 1:
			{
				ShipModel_Eater shipModel_Eater = ships.get_Item(0);
				Point cutinSp1_InBattle = shipModel_Eater.Offsets.GetCutinSp1_InBattle(shipModel_Eater.DamagedFlg);
				this._listShipTexture.get_Item(1).mainTexture = ShipUtils.LoadTexture(shipModel_Eater);
				this._listShipTexture.get_Item(1).MakePixelPerfect();
				this._listShipTexture.get_Item(1).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle.x, (float)cutinSp1_InBattle.y, 0f));
				break;
			}
			case 2:
			{
				ShipModel_Eater shipModel_Eater2 = ships.get_Item(0);
				Point cutinSp1_InBattle2 = shipModel_Eater2.Offsets.GetCutinSp1_InBattle(shipModel_Eater2.DamagedFlg);
				this._listShipTexture.get_Item(0).mainTexture = ShipUtils.LoadTexture(shipModel_Eater2);
				this._listShipTexture.get_Item(0).MakePixelPerfect();
				this._listShipTexture.get_Item(0).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle2.x, (float)cutinSp1_InBattle2.y, 0f));
				shipModel_Eater2 = ships.get_Item(1);
				cutinSp1_InBattle2 = shipModel_Eater2.Offsets.GetCutinSp1_InBattle(shipModel_Eater2.DamagedFlg);
				this._listShipTexture.get_Item(2).mainTexture = ShipUtils.LoadTexture(shipModel_Eater2);
				this._listShipTexture.get_Item(2).MakePixelPerfect();
				this._listShipTexture.get_Item(2).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle2.x, (float)cutinSp1_InBattle2.y, 0f));
				break;
			}
			case 3:
			{
				ShipModel_Eater shipModel_Eater3 = ships.get_Item(0);
				Point cutinSp1_InBattle3 = shipModel_Eater3.Offsets.GetCutinSp1_InBattle(shipModel_Eater3.DamagedFlg);
				this._listShipTexture.get_Item(1).mainTexture = ShipUtils.LoadTexture(shipModel_Eater3);
				this._listShipTexture.get_Item(1).MakePixelPerfect();
				this._listShipTexture.get_Item(1).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle3.x, (float)cutinSp1_InBattle3.y, 0f));
				shipModel_Eater3 = ships.get_Item(1);
				cutinSp1_InBattle3 = shipModel_Eater3.Offsets.GetCutinSp1_InBattle(shipModel_Eater3.DamagedFlg);
				this._listShipTexture.get_Item(0).mainTexture = ShipUtils.LoadTexture(shipModel_Eater3);
				this._listShipTexture.get_Item(0).MakePixelPerfect();
				this._listShipTexture.get_Item(0).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle3.x, (float)cutinSp1_InBattle3.y, 0f));
				shipModel_Eater3 = ships.get_Item(2);
				cutinSp1_InBattle3 = shipModel_Eater3.Offsets.GetCutinSp1_InBattle(shipModel_Eater3.DamagedFlg);
				this._listShipTexture.get_Item(2).mainTexture = ShipUtils.LoadTexture(shipModel_Eater3);
				this._listShipTexture.get_Item(2).MakePixelPerfect();
				this._listShipTexture.get_Item(2).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle3.x, (float)cutinSp1_InBattle3.y, 0f));
				break;
			}
			}
		}

		public ProdCombatRation SetOnStageReady(Action onStageReady)
		{
			this._actOnStageReady = onStageReady;
			return this;
		}

		public void Play(Action onFinished)
		{
			this._actOnFinished = onFinished;
			this.animation.Play();
			base.get_transform().localScaleOne();
		}

		private void OnStageReady()
		{
			Dlg.Call(ref this._actOnStageReady);
		}

		private void PlayEatingVoice()
		{
			ShipUtils.PlayEatingVoice(this._listShips.get_Item(0));
		}

		private void OnFinished()
		{
			Dlg.Call(ref this._actOnFinished);
		}
	}
}
