using Common.Enum;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdTranscendenceCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			ProdTAMainBatteryx3,
			ProdTATorpedox2,
			ProdTAMainBatteryNTorpedo
		}

		private ProdTranscendenceCutIn.AnimationList _iList;

		private ShipModel_Attacker _clsAttacker;

		private List<UITexture> _listShipTexture;

		private ProdTranscendenceSlots _prodTranscendenceSlots;

		private UIPanel _uiPanel;

		public ProdTranscendenceCutIn.AnimationList playAnimation
		{
			get
			{
				return this._iList;
			}
		}

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdTranscendenceCutIn Instantiate(ProdTranscendenceCutIn prefab, Transform parent)
		{
			ProdTranscendenceCutIn prodTranscendenceCutIn = Object.Instantiate<ProdTranscendenceCutIn>(prefab);
			prodTranscendenceCutIn.get_transform().set_parent(parent);
			prodTranscendenceCutIn.get_transform().set_localScale(Vector3.get_zero());
			prodTranscendenceCutIn.get_transform().set_localPosition(Vector3.get_zero());
			return prodTranscendenceCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			this._prodTranscendenceSlots = base.get_transform().GetComponentInChildren<ProdTranscendenceSlots>();
			this._prodTranscendenceSlots.Init();
			Transform transform = base.get_transform().FindChild("ShipAnchor").get_transform();
			this._listShipTexture = new List<UITexture>();
			this._listShipTexture.Add(transform.get_transform().FindChild("Ship").GetComponent<UITexture>());
			this._listShipTexture.Add(transform.get_transform().FindChild("Mask").GetComponent<UITexture>());
			this.panel.widgetsAreStatic = true;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<ProdTranscendenceCutIn.AnimationList>(ref this._iList);
			Mem.Del<ShipModel_Attacker>(ref this._clsAttacker);
			Mem.DelListSafe<UITexture>(ref this._listShipTexture);
			Mem.Del<ProdTranscendenceSlots>(ref this._prodTranscendenceSlots);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		public void SetShellingData(HougekiModel model)
		{
			this._iList = this.getAnimation(model.AttackType);
			this._clsAttacker = model.Attacker;
			this.setShipsTexture(model.Attacker);
			this.setSlotItems(model.GetSlotitems());
		}

		private void setShipsTexture(ShipModel_Attacker model)
		{
			Texture2D mainTexture = ShipUtils.LoadTexture(model.GetGraphicsMstId(), model.IsFriend() && model.DamagedFlg);
			Vector2 vector = ShipUtils.GetShipOffsPos(model, model.DamagedFlg, this.GetGraphColumn(this._iList));
			using (List<UITexture>.Enumerator enumerator = this._listShipTexture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UITexture current = enumerator.get_Current();
					current.mainTexture = mainTexture;
					current.MakePixelPerfect();
					current.get_transform().set_localPosition(vector);
				}
			}
		}

		private void setSlotItems(SlotitemModel_Battle[] models)
		{
			ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
			prodShellingSlotLine.SetSlotData(models, this._iList);
			this.setHexBtn(models);
		}

		private void setHexBtn(SlotitemModel_Battle[] models)
		{
			switch (this._iList)
			{
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3:
			{
				int num = 0;
				using (List<UISlotItemHexButton>.Enumerator enumerator = this._prodTranscendenceSlots.hexButtonList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UISlotItemHexButton current = enumerator.get_Current();
						if (current != null)
						{
							current.SetSlotItem(models[num]);
						}
						current.SetActive(false);
						num++;
					}
				}
				break;
			}
			case ProdTranscendenceCutIn.AnimationList.ProdTATorpedox2:
			{
				int num2 = 0;
				using (List<UISlotItemHexButton>.Enumerator enumerator2 = this._prodTranscendenceSlots.hexButtonList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UISlotItemHexButton current2 = enumerator2.get_Current();
						if (num2 == 0)
						{
							current2.SetSlotItem(models[0]);
						}
						else if (num2 > 0)
						{
							current2.SetSlotItem(models[1]);
						}
						num2++;
					}
				}
				break;
			}
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryNTorpedo:
				this._prodTranscendenceSlots.hexButtonList.get_Item(0).SetSlotItem(models[0]);
				this._prodTranscendenceSlots.hexButtonList.get_Item(1).SetSlotItem(models[1]);
				this._prodTranscendenceSlots.hexButtonList.get_Item(0).SetActive(false);
				this._prodTranscendenceSlots.hexButtonList.get_Item(1).SetActive(false);
				break;
			}
		}

		public override void Play(Action callback)
		{
			this.panel.widgetsAreStatic = false;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			this.Init();
			this.setGlowEffects();
			base.get_transform().set_localScale(Vector3.get_one());
			base.Play(this._iList, callback);
		}

		public void Play(ProdTranscendenceCutIn.AnimationList iList, Action callback)
		{
			this.Init();
			this._actCallback = callback;
			base.get_transform().set_localPosition(Vector3.get_zero());
			this._iList = iList;
			this.setGlowEffects();
			base.get_transform().set_localScale(Vector3.get_one());
			this._animAnimation.Play(iList.ToString());
		}

		private void PlayShellingVoice()
		{
			ShipUtils.PlayShellingVoive(this._clsAttacker);
		}

		private void playShellingSE()
		{
			SoundUtils.PlayShellingSE(this._clsAttacker);
		}

		private void playSlotItemSuccessiveLine()
		{
			ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
			prodShellingSlotLine.PlayTranscendenceLine(BaseProdLine.AnimationName.ProdSuccessiveLine, this._clsAttacker.IsFriend(), null);
		}

		private void playSlotItemTripleLine()
		{
			ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
			prodShellingSlotLine.PlayTranscendenceLine(BaseProdLine.AnimationName.ProdTripleLine, this._clsAttacker.IsFriend(), null);
		}

		private void playSlotItem(int nSlotNum)
		{
			iTween.ValueTo(base.get_gameObject(), this.getGlowTIntHash(0.3f));
		}

		private void playSlotItems()
		{
			this._prodTranscendenceSlots.Play(this._iList);
		}

		protected override void onAnimationFinished()
		{
			this.panel.widgetsAreStatic = true;
			base.onAnimationFinished();
		}

		private void PlayGlow()
		{
			base.playGlowEffect();
		}

		private ProdTranscendenceCutIn.AnimationList getAnimation(BattleAttackKind iKind)
		{
			switch (iKind)
			{
			case BattleAttackKind.Syu_Rai:
				return ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryNTorpedo;
			case BattleAttackKind.Rai_Rai:
				return ProdTranscendenceCutIn.AnimationList.ProdTATorpedox2;
			case BattleAttackKind.Syu_Syu_Fuku:
				return ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3;
			case BattleAttackKind.Syu_Syu_Syu:
				return ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3;
			default:
				return ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3;
			}
		}

		private MstShipGraphColumn GetGraphColumn(ProdTranscendenceCutIn.AnimationList iList)
		{
			switch (iList)
			{
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3:
				return MstShipGraphColumn.CutIn;
			case ProdTranscendenceCutIn.AnimationList.ProdTATorpedox2:
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryNTorpedo:
				return MstShipGraphColumn.CutInSp1;
			default:
				return MstShipGraphColumn.CutIn;
			}
		}
	}
}
