using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			None,
			SupportCutIn
		}

		[SerializeField]
		private UITexture _shipTex;

		[SerializeField]
		private UITexture _shipShadow;

		[SerializeField]
		private ParticleSystem _firePar;

		private ShipModel_Attacker _flagShip;

		private IShienModel _clsSupport;

		public static ProdSupportCutIn Instantiate(ProdSupportCutIn prefab, IShienModel model, Transform parent)
		{
			ProdSupportCutIn prodSupportCutIn = Object.Instantiate<ProdSupportCutIn>(prefab);
			prodSupportCutIn._clsSupport = model;
			prodSupportCutIn.get_transform().set_parent(parent);
			prodSupportCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodSupportCutIn.get_transform().set_localScale(Vector3.get_one());
			return prodSupportCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			Util.FindParentToChild<UITexture>(ref this._shipTex, base.get_transform(), "ShipObj/Anchor/Object2D");
			Util.FindParentToChild<UITexture>(ref this._shipShadow, base.get_transform(), "ShipObj/Anchor/ObjectShadow");
			Util.FindParentToChild<ParticleSystem>(ref this._firePar, base.get_transform(), "Fire");
			this._firePar.SetActive(false);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._shipTex);
			Mem.Del<UITexture>(ref this._shipShadow);
			Mem.Del(ref this._firePar);
			Mem.Del<ShipModel_Attacker>(ref this._flagShip);
			Mem.Del<IShienModel>(ref this._clsSupport);
		}

		private void _setShipInfo()
		{
			this._flagShip = this._clsSupport.ShienShips[0];
			this._shipTex.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._flagShip);
			this._shipTex.MakePixelPerfect();
			this._shipTex.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._flagShip.GetGraphicsMstId()).GetFace(this._flagShip.DamagedFlg)));
			this._shipShadow.mainTexture = this._shipTex.mainTexture;
			this._shipShadow.MakePixelPerfect();
			this._shipShadow.get_transform().set_localPosition(this._shipTex.get_transform().get_localPosition());
		}

		private void _debugShipInfo()
		{
			this._shipTex.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(82, false);
			this._shipTex.MakePixelPerfect();
			this._shipTex.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(82).GetFace(false)));
			this._shipShadow.mainTexture = this._shipTex.mainTexture;
			this._shipShadow.MakePixelPerfect();
			this._shipShadow.get_transform().set_localPosition(this._shipTex.get_transform().get_localPosition());
		}

		private void _setCameraRotation()
		{
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			friendFieldCamera.ReqViewMode(CameraActor.ViewMode.RotateAroundObject);
			friendFieldCamera.SetRotateAroundObjectCamera(Vector3.get_zero(), new Vector3(0f, 19f, 220f), 1f);
		}

		public override void Play(Action callback)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			this._actCallback = callback;
			this._setCameraRotation();
			this._setShipInfo();
			this.setGlowEffects();
			this._firePar.SetActive(true);
			this._firePar.get_transform().set_localScale(new Vector3(100f, 1f, 1f));
			this._firePar.Stop();
			this._firePar.Play();
			base.Play(ProdSupportCutIn.AnimationList.SupportCutIn, callback);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_057);
		}

		private ProdSupportCutIn.AnimationList getAnimationList()
		{
			return ProdSupportCutIn.AnimationList.SupportCutIn;
		}

		private void startMotionBlur()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(true);
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
				SEFIleInfos info = SEFIleInfos.SE_057;
				base._playSE(info);
			}
		}

		private void _playShipVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlaySupportingFireVoice(this._flagShip);
		}

		private void stopParticle()
		{
			this._firePar.Stop();
		}

		private void _onFinishedAnimation()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
