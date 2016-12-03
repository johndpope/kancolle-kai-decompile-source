using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation)), RequireComponent(typeof(UIPanel))]
	public class ProdObservedShellingCutIn : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiAircraft;

		[SerializeField]
		private NoiseMove _clsNoiseMove;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiSeparatorLine;

		[SerializeField]
		private List<UITexture> _listShipTextures;

		[SerializeField]
		private List<UISlotItemHexButton> _listHexBtns;

		[SerializeField]
		private List<UILabel> _listSlotLabels;

		[SerializeField]
		private List<UITexture> _listOverlays;

		[SerializeField]
		private UITexture _uiTelopOverlay;

		private UIPanel _uiPanel;

		private ShipModel_Attacker _clsAttacker;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdObservedShellingCutIn Instantiate(ProdObservedShellingCutIn prefab, Transform parent)
		{
			ProdObservedShellingCutIn prodObservedShellingCutIn = Object.Instantiate<ProdObservedShellingCutIn>(prefab);
			prodObservedShellingCutIn.get_transform().set_parent(parent);
			prodObservedShellingCutIn.get_transform().localScaleZero();
			prodObservedShellingCutIn.get_transform().localPositionZero();
			return prodObservedShellingCutIn;
		}

		protected override void Awake()
		{
			this.panel.widgetsAreStatic = true;
			base.Awake();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._uiAircraft);
			Mem.Del<NoiseMove>(ref this._clsNoiseMove);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiSeparatorLine);
			Mem.DelListSafe<UITexture>(ref this._listShipTextures);
			Mem.DelListSafe<UISlotItemHexButton>(ref this._listHexBtns);
			Mem.DelListSafe<UILabel>(ref this._listSlotLabels);
			Mem.DelListSafe<UITexture>(ref this._listOverlays);
			Mem.Del<UITexture>(ref this._uiTelopOverlay);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<ShipModel_Attacker>(ref this._clsAttacker);
		}

		public void SetObservedShelling(HougekiModel model)
		{
			this._clsAttacker = model.Attacker;
			Texture2D shipTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(model.Attacker);
			Vector3 offs = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(model.Attacker, model.Attacker.DamagedFlg, MstShipGraphColumn.CutIn);
			this._listShipTextures.ForEach(delegate(UITexture x)
			{
				x.mainTexture = shipTexture;
				x.MakePixelPerfect();
				x.get_transform().set_localPosition(offs);
			});
			List<SlotitemModel_Battle> list = new List<SlotitemModel_Battle>(model.GetSlotitems());
			this._listSlotLabels.get_Item(0).text = list.get_Item(1).Name;
			this._listSlotLabels.get_Item(1).text = list.get_Item(2).Name;
			this._uiAircraft.mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadUniDirTexture(list.get_Item(0));
			this._uiAircraft.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(6);
			this._listHexBtns.get_Item(0).SetSlotItem(list.get_Item(1));
			this._listHexBtns.get_Item(1).SetSlotItem(list.get_Item(2));
			Color col = (!this._clsAttacker.IsFriend()) ? new Color(1f, 0f, 0f, 0.50196f) : new Color(0f, 0.31875f, 1f, 0.50196f);
			this._uiTelopOverlay.color = col;
			this._listOverlays.ForEach(delegate(UITexture x)
			{
				x.color = col;
			});
		}

		public override void Play(Action callback)
		{
			this.panel.widgetsAreStatic = false;
			base.get_transform().localScaleOne();
			base.Play(callback);
		}

		private void PlayShellingVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlayShellingVoive(this._clsAttacker);
		}

		private void PlayHexSlot(int nNum)
		{
			this._listHexBtns.get_Item(nNum).SetActive(true);
			this._listHexBtns.get_Item(nNum).Play(UIHexButton.AnimationList.ProdTranscendenceAttackHex, null);
		}

		private void PlaySlotSE()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_939b);
		}

		private void PlayShellingSE()
		{
			KCV.Battle.Utils.SoundUtils.PlayShellingSE(this._clsAttacker);
		}

		protected override void onAnimationFinished()
		{
			this.panel.widgetsAreStatic = true;
			base.onAnimationFinished();
		}
	}
}
