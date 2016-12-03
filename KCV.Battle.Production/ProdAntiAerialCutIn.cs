using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAntiAerialCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			None,
			AntiAerialCutIn2
		}

		[SerializeField]
		private GameObject _uiShipObj;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private UITexture _uiShipShadow;

		[SerializeField]
		private UITexture[] _uiSlotBg;

		[SerializeField]
		private UILabel[] _uiSlotLabel;

		private KoukuuModel _clsAerial;

		private ShipModel_Attacker _ship;

		private ProdAntiAerialCutIn.AnimationList _iList;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdAntiAerialCutIn Instantiate(ProdAntiAerialCutIn prefab, KoukuuModel model, Transform parent)
		{
			ProdAntiAerialCutIn prodAntiAerialCutIn = Object.Instantiate<ProdAntiAerialCutIn>(prefab);
			prodAntiAerialCutIn._clsAerial = model;
			prodAntiAerialCutIn.get_transform().set_parent(parent);
			prodAntiAerialCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodAntiAerialCutIn.get_transform().set_localScale(Vector3.get_one());
			prodAntiAerialCutIn._iList = ProdAntiAerialCutIn.AnimationList.None;
			prodAntiAerialCutIn.panel.widgetsAreStatic = true;
			return prodAntiAerialCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			this._iList = ProdAntiAerialCutIn.AnimationList.None;
			if (this._uiShipObj == null)
			{
				this._uiShipObj = base.get_transform().FindChild("ShipObj").get_gameObject();
			}
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "ShipObj/Anchor/Object2D");
			Util.FindParentToChild<UITexture>(ref this._uiShipShadow, base.get_transform(), "ShipObj/Anchor/ObjectShadow");
			this._uiSlotLabel = new UILabel[3];
			this._uiSlotBg = new UITexture[3];
			for (int i = 0; i < 3; i++)
			{
				Util.FindParentToChild<UILabel>(ref this._uiSlotLabel[i], base.get_transform(), "SlotLabel" + (i + 1));
				Util.FindParentToChild<UITexture>(ref this._uiSlotBg[i], base.get_transform(), "SlotBg" + (i + 1));
				this._uiSlotLabel[i].SetActive(false);
				this._uiSlotBg[i].SetActive(false);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<GameObject>(ref this._uiShipObj);
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.Del<UITexture>(ref this._uiShipShadow);
			Mem.DelArySafe<UITexture>(ref this._uiSlotBg);
			Mem.DelArySafe<UILabel>(ref this._uiSlotLabel);
			Mem.Del<KoukuuModel>(ref this._clsAerial);
			Mem.Del<ShipModel_Attacker>(ref this._ship);
			Mem.Del<ProdAntiAerialCutIn.AnimationList>(ref this._iList);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		private void _setShipInfo(bool isFriend)
		{
			this._ship = this._clsAerial.GetTaikuShip(isFriend);
			this._uiShip.mainTexture = ShipUtils.LoadTexture(this._ship);
			this._uiShip.MakePixelPerfect();
			this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._ship.GetGraphicsMstId()).GetShipDisplayCenter(this._ship.DamagedFlg)));
			this._uiShip.flip = ((!isFriend) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
			this._uiShipShadow.mainTexture = this._uiShip.mainTexture;
			this._uiShipShadow.MakePixelPerfect();
			this._uiShipShadow.flip = ((!isFriend) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
			this._uiShipObj.get_transform().set_localRotation((!isFriend) ? Quaternion.EulerAngles(new Vector3(0f, 180f, 0f)) : Quaternion.EulerAngles(Vector3.get_zero()));
		}

		private void _setSlotLabel(bool isFriend)
		{
			List<SlotitemModel_Battle> taikuSlotitems = this._clsAerial.GetTaikuSlotitems(isFriend);
			if (taikuSlotitems == null)
			{
				return;
			}
			for (int i = 0; i < taikuSlotitems.get_Count(); i++)
			{
				if (i >= 3)
				{
					break;
				}
				this._uiSlotBg[i].SetActive(true);
				this._uiSlotLabel[i].SetActive(true);
				this._uiSlotLabel[i].text = taikuSlotitems.get_Item(i).Name;
			}
		}

		public override void Play(Action callback)
		{
			base.Play(this._iList, callback);
		}

		public void Play(Action callback, bool isFriend)
		{
			this.panel.widgetsAreStatic = false;
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.depth = 5f;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			cutInEffectCamera.depth = 6f;
			base.get_transform().set_localScale(Vector3.get_one());
			this._actCallback = callback;
			this._iList = this.getAnimationList();
			if (this._iList == ProdAntiAerialCutIn.AnimationList.None)
			{
				this.onAnimationFinishedAfterDiscard();
				return;
			}
			this._setShipInfo(isFriend);
			this._setSlotLabel(isFriend);
			this.setGlowEffects();
			this.Play(callback);
		}

		private ProdAntiAerialCutIn.AnimationList getAnimationList()
		{
			return ProdAntiAerialCutIn.AnimationList.AntiAerialCutIn2;
		}

		private void startMotionBlur()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(true);
		}

		private void endMotionBlur()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(false);
		}

		private void _playExplosionParticle(int num)
		{
			if (num == 0)
			{
				ShipUtils.PlayAircraftCutInVoice(this._ship);
			}
		}

		private void onPlaySeAnime(int seNo)
		{
			if (seNo == 0)
			{
				SEFIleInfos info = SEFIleInfos.BattleAdmission;
				base._playSE(info);
			}
			else if (seNo == 1)
			{
				SEFIleInfos info = SEFIleInfos.BattleNightMessage;
				base._playSE(info);
			}
		}

		private void _onFinishedAnimation()
		{
			this.panel.widgetsAreStatic = true;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(false);
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
