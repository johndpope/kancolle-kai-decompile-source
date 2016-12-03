using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models;
using local.models.battle;
using local.utils;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdThalassocracy : BaseAnimation
	{
		[SerializeField]
		private ParticleSystem _uiParticle;

		[SerializeField]
		private UIPanel[] _uiShipPanel;

		[SerializeField]
		private UITexture[] _uiShip;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private Animation _anime;

		private bool _isDebugDamage;

		private int index;

		private bool _isRebelion;

		private bool _isControl;

		private KeyControl _keyControl;

		private BattleResultModel _model;

		private ShipModel_BattleAll[] _ships;

		private MapManager _clsMapManager;

		private void _init()
		{
			this.index = 0;
			this._isDebugDamage = false;
			this._isControl = false;
			this._isFinished = false;
			this._isRebelion = false;
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticle, base.get_transform(), "BgPanel/Particle");
			Util.FindParentToChild<UILabel>(ref this._uiLabel, base.get_transform(), "ShipPanel/SlotLabel");
			this._uiShip = new UITexture[6];
			this._uiShipPanel = new UIPanel[6];
			Util.FindParentToChild<UITexture>(ref this._uiShip[0], base.get_transform(), "ShipPanel/ShipObj/Ship");
			Util.FindParentToChild<UIPanel>(ref this._uiShipPanel[0], base.get_transform(), "ShipPanel");
			for (int i = 1; i < 6; i++)
			{
				Util.FindParentToChild<UITexture>(ref this._uiShip[i], base.get_transform(), "LeftPanel/ShipObj" + i + "/ShipObj/Ship");
				Util.FindParentToChild<UIPanel>(ref this._uiShipPanel[i], base.get_transform(), "LeftPanel/ShipObj" + i);
			}
			if (this._anime == null)
			{
				this._anime = base.GetComponent<Animation>();
			}
			this._uiParticle.SetActive(false);
			this._anime.Stop();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiParticle);
			Mem.Del<UIPanel[]>(ref this._uiShipPanel);
			Mem.Del<UILabel>(ref this._uiLabel);
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<UITexture[]>(ref this._uiShip);
			Mem.Del<KeyControl>(ref this._keyControl);
			Mem.Del<BattleResultModel>(ref this._model);
			Mem.Del<ShipModel_BattleAll[]>(ref this._ships);
			Mem.Del<MapManager>(ref this._clsMapManager);
		}

		public bool Run()
		{
			if (this._isControl && this._keyControl != null && this._keyControl.keyState.get_Item(1).down)
			{
				this.onAnimationComp();
			}
			return this._isFinished;
		}

		private void setShipView()
		{
			if (this._model == null || this._ships == null)
			{
				return;
			}
			int num = (this._model.MvpShip.MstId != this._ships[0].MstId) ? 2 : 1;
			this.setShipTexture(0, this._ships[0], false);
			for (int i = 1; i < 6; i++)
			{
				if (this._ships[i] == null)
				{
					this._uiShip[i].SetActive(false);
				}
				else if (this._ships[i].DmgStateEnd == DamageState_Battle.Gekichin)
				{
					this.setShipTexture(num, this._ships[i], true);
					num++;
				}
				else if (this._model.MvpShip.MstId == this._ships[i].MstId)
				{
					this.setShipTexture(1, this._ships[i], false);
				}
				else
				{
					this.setShipTexture(num, this._ships[i], false);
					num++;
				}
			}
		}

		private void setShipTexture(int index, ShipModel_BattleAll ship, bool isShink)
		{
			this._uiShip[index].SetActive(true);
			this._uiShip[index].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(ship.GetGraphicsMstId(), false);
			this._uiShip[index].MakePixelPerfect();
			this._uiShip[index].color = ((!isShink) ? new Color(1f, 1f, 1f, 0f) : new Color(0.3f, 0.3f, 0.3f, 0f));
			if (index == 0)
			{
				this._uiShip[0].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(ship.GetGraphicsMstId()).GetShipDisplayCenter(false)));
			}
			else
			{
				this._uiShip[index].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(ship.GetGraphicsMstId()).GetFace(false)));
			}
		}

		private void testShipView()
		{
			BattleResultModel battleResult = BattleTaskManager.GetBattleManager().GetBattleResult();
			ShipModel_BattleAll[] ships_f = BattleTaskManager.GetBattleManager().Ships_f;
			int arg_35_0 = (battleResult.MvpShip.MstId != ships_f[0].MstId) ? 2 : 1;
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(this.index))
			{
				this._uiShip[0].SetActive(true);
				this._uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this.index, this._isDebugDamage);
				this._uiShip[0].MakePixelPerfect();
				this._uiShip[0].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this.index).GetShipDisplayCenter(this._isDebugDamage)));
				for (int i = 1; i < 6; i++)
				{
					ShipModelMst shipModelMst = new ShipModelMst(1);
					this._uiShip[i].SetActive(true);
					this._uiShip[i].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this.index, this._isDebugDamage);
					this._uiShip[i].MakePixelPerfect();
					this._uiShip[i].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this.index).GetFace(this._isDebugDamage)));
				}
			}
		}

		public override void Play(Action callback)
		{
		}

		public void Play(Action callback, Generics.BattleRootType rootType, string mapName)
		{
			this._actCallback = callback;
			if (this._model == null || this._ships == null)
			{
				this.onAnimationComp();
			}
			this._animAnimation.Stop();
			if (rootType == Generics.BattleRootType.Rebellion)
			{
				this._isRebelion = true;
				this._uiLabel.text = "敵反攻作戦 迎撃成功!!";
				this._animAnimation.Play("Rebelion");
			}
			else
			{
				this._uiLabel.text = mapName + " 制海権確保!!";
				this._animAnimation.Play("Thalassocracy");
			}
			this._uiParticle.SetActive(true);
			this._uiParticle.Stop();
			this._uiParticle.Play();
		}

		private void startControl()
		{
			if (!this._isRebelion)
			{
				TrophyUtil.Unlock_At_AreaClear(this._clsMapManager.Map.AreaId);
			}
			this._isControl = true;
		}

		private void _playAnimationSE(int num)
		{
			if (num != 0)
			{
				if (num == 1)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleAdmission);
				}
			}
			else
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.ClearAA);
			}
		}

		private void onAnimationComp()
		{
			this._animAnimation.Stop();
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this._isFinished = true;
		}

		public static ProdThalassocracy Instantiate(ProdThalassocracy prefab, Transform parent, KeyControl keyControl, MapManager mapManager, BattleResultModel model, ShipModel_BattleAll[] ships, int nPanelDepth)
		{
			ProdThalassocracy prodThalassocracy = Object.Instantiate<ProdThalassocracy>(prefab);
			prodThalassocracy.get_transform().set_parent(parent);
			prodThalassocracy.get_transform().set_localScale(Vector3.get_one());
			prodThalassocracy.get_transform().set_localPosition(Vector3.get_zero());
			prodThalassocracy._init();
			prodThalassocracy._keyControl = keyControl;
			prodThalassocracy._model = model;
			prodThalassocracy._ships = ships;
			prodThalassocracy._clsMapManager = mapManager;
			prodThalassocracy.setShipView();
			return prodThalassocracy;
		}
	}
}
