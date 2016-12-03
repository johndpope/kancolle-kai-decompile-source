using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class CommonShipBanner : BaseShipBanner
	{
		public UISprite UiConditionIcon;

		public UISprite UiConditionMask;

		public GameObject Kira;

		public GameObject Smoke;

		public GameObject Ring;

		public int sizeX;

		private Vector2 defaultLocalPos;

		private Vector2 blingIconLocalPos;

		public bool isUseKira;

		public bool isUseSmoke;

		private BannerSmokes bannerSmokes;

		private Transform ParticlePanel;

		private bool isAwake;

		protected override void Awake()
		{
			if (this.isAwake)
			{
				return;
			}
			base.Awake();
			this.UiConditionMask.alpha = 0f;
			this.UiConditionIcon.alpha = 0f;
			this._uiDamageIcon.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			this.isUseKira = true;
			this.isUseSmoke = true;
			this.ParticlePanel = base.get_transform().FindChild("ParticlePanel");
			if (this.ParticlePanel == null)
			{
				this.ParticlePanel = this._uiShipTex.get_transform().FindChild("ParticlePanel");
				if (this.ParticlePanel == null)
				{
					this._uiShipTex.get_transform().AddChild("ParticlePanel");
					this.ParticlePanel = this._uiShipTex.get_transform().FindChild("ParticlePanel");
				}
			}
			if (this.ParticlePanel != null)
			{
				this.ParticlePanel.SetParent(this._uiShipTex.get_transform());
				this.ParticlePanel.get_transform().set_localScale(Vector3.get_one());
				this.ParticlePanel.get_transform().set_localPosition(new Vector3(-128f, 32f, 0f));
			}
			if (this._uiDamageIcon != null)
			{
				this._uiShipTex.pivot = UIWidget.Pivot.Center;
				this._uiDamageIcon.pivot = UIWidget.Pivot.Center;
				this._uiDamageIcon.get_transform().SetParent(this._uiShipTex.get_transform());
			}
			this.isAwake = true;
		}

		public UITexture GetUITexture()
		{
			return this._uiShipTex;
		}

		public virtual void SetShipData(ShipModel model)
		{
			if (model == null)
			{
				this._clsShipModel = null;
				return;
			}
			if (!this.isAwake)
			{
				this.Awake();
			}
			if (this.UiConditionIcon == null)
			{
				this.UiConditionIcon = base.get_transform().FindChild("ConditionIcon").GetComponent<UISprite>();
			}
			if (this.UiConditionMask == null)
			{
				this.UiConditionMask = base.get_transform().FindChild("ConditionMask").GetComponent<UISprite>();
			}
			this._clsShipModel = model;
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			this._uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
			this.SetSmoke(model);
			this.SetStateIcon(model);
			this.InitMarriageRing(model);
			this.UpdateCondition(model.ConditionState);
		}

		public void SetShipDataWithDisableParticle(ShipModel model)
		{
			this.StopParticle();
			if (model == null)
			{
				this._clsShipModel = null;
				return;
			}
			if (!this.isAwake)
			{
				this.Awake();
			}
			if (this.UiConditionIcon == null)
			{
				this.UiConditionIcon = base.get_transform().FindChild("ConditionIcon").GetComponent<UISprite>();
			}
			if (this.UiConditionMask == null)
			{
				this.UiConditionMask = base.get_transform().FindChild("ConditionMask").GetComponent<UISprite>();
			}
			this._clsShipModel = model;
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			this._uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
			this.UpdateCondition(model.ConditionState);
		}

		public virtual void SetShipData(ShipModel_Practice model)
		{
			if (model == null)
			{
				return;
			}
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			this._uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
			this._uiDamageMask.alpha = 0f;
			this._uiDamageIcon.alpha = 0f;
			this.UiConditionMask.alpha = 0f;
			this.UiConditionIcon.alpha = 0f;
		}

		public static void stopBannerParticle()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("CommonShipBanner");
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject = array2[i];
				gameObject.SendMessage("StopParticle");
			}
		}

		public static void startBannerParticle()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("CommonShipBanner");
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject = array2[i];
				gameObject.SendMessage("StartParticle");
			}
		}

		private void UpdateCondition(FatigueState state)
		{
			switch (state)
			{
			case FatigueState.Exaltation:
				this.UiConditionMask.alpha = 0f;
				this.UiConditionIcon.alpha = 0f;
				this.SetKiraPar();
				break;
			case FatigueState.Normal:
				this.UiConditionMask.alpha = 0f;
				this.UiConditionIcon.alpha = 0f;
				if (this.Kira != null)
				{
					this.Kira.SetActive(false);
				}
				break;
			case FatigueState.Light:
				this.UiConditionMask.alpha = 1f;
				this.UiConditionIcon.alpha = 1f;
				this.UiConditionMask.spriteName = "card-ss_fatigue_1";
				this.UiConditionIcon.spriteName = "icon_fatigue_1";
				if (this.Kira != null)
				{
					this.Kira.SetActive(false);
				}
				break;
			case FatigueState.Distress:
				this.UiConditionMask.alpha = 1f;
				this.UiConditionIcon.alpha = 1f;
				this.UiConditionMask.spriteName = "card-ss_fatigue_2";
				this.UiConditionIcon.spriteName = "icon_fatigue_2";
				if (this.Kira != null)
				{
					this.Kira.SetActive(false);
				}
				break;
			}
		}

		public void StartParticle()
		{
			if (this.ParticlePanel != null)
			{
				this.ParticlePanel.SetActive(true);
				UIPanel component = this.ParticlePanel.GetComponent<UIPanel>();
				if (component != null)
				{
					component.alpha = 0f;
					TweenAlpha.Begin(component.get_gameObject(), 1f, 1f);
				}
			}
			if (this.Smoke != null && this.isNeedSmoke())
			{
				this.Smoke.SetActive(true);
			}
			if (this.Kira != null && this.isNeedKira())
			{
				this.Kira.SetActive(true);
			}
		}

		public void StopParticle()
		{
			if (this.ParticlePanel != null)
			{
				this.ParticlePanel.SetActive(false);
			}
			if (this.Smoke != null)
			{
				this.Smoke.SetActive(false);
			}
			if (this.Kira != null)
			{
				this.Kira.SetActive(false);
			}
		}

		private void SetStateIcon(ShipModel model)
		{
			if (model.IsInRepair())
			{
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageIcon.spriteName = "icon-ss_syufuku";
				this.setStateIconSize(false);
			}
			else if (model.IsInMission())
			{
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageIcon.spriteName = "icon-ss_ensei";
				this.setStateIconSize(false);
			}
			else if (model.IsTettaiBling() && model.IsInDeck() != -1)
			{
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageIcon.spriteName = "shipicon_withdraw";
				this.setStateIconSize(true);
			}
			else if (model.IsBling())
			{
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageIcon.spriteName = "icon_kaikou";
				this.setStateIconSize(true);
			}
			else
			{
				this.UpdateDamage(model.DamageStatus);
				this.setStateIconSize(false);
			}
		}

		private void setStateIconSize(bool isBling)
		{
			if (isBling)
			{
				this._uiDamageIcon.get_transform().localPositionX(100f);
				this._uiDamageIcon.get_transform().localPositionY(0f);
				this._uiDamageIcon.width = 48;
				this._uiDamageIcon.height = 64;
				this._uiDamageIcon.get_transform().set_localScale(Vector3.get_one());
			}
			else
			{
				this._uiDamageIcon.get_transform().localPositionX(0f);
				this._uiDamageIcon.get_transform().localPositionY(0f);
				this._uiDamageIcon.width = 256;
				this._uiDamageIcon.height = 64;
				this._uiDamageIcon.get_transform().set_localScale(Vector3.get_one());
			}
		}

		private void SetSmoke(ShipModel model)
		{
			if (!this.isUseSmoke)
			{
				return;
			}
			if (model.DamageStatus != DamageState.Normal && !model.IsInRepair())
			{
				if (this.Smoke == null)
				{
					this.CreateSmoke(model.DamageStatus);
				}
				else
				{
					this.ParticlePanel.SetActive(true);
					this.Smoke.SetActive(true);
				}
			}
			else if (this.Smoke != null)
			{
				this.Smoke.SetActive(false);
			}
		}

		private void CreateSmoke(DamageState state)
		{
			if (this.ParticlePanel == null)
			{
				this.ParticlePanel = base.get_transform().FindChild("ParticlePanel");
			}
			this.Smoke = Util.Instantiate(Resources.Load("Prefabs/Common/BannerSmokes") as GameObject, this.ParticlePanel.get_gameObject(), false, false);
			float num = this.getMagnification() / base.get_transform().get_localScale().x;
			this.Smoke.get_transform().localScaleX(num);
			this.Smoke.get_transform().localScaleY(num);
			this.bannerSmokes = this.Smoke.GetComponent<BannerSmokes>();
			if (!this.ParticlePanel.get_gameObject().get_activeSelf())
			{
				this.ParticlePanel.get_gameObject().SetActive(true);
			}
		}

		private void SetKiraPar()
		{
			if (!this.isUseKira)
			{
				return;
			}
			this.ParticlePanel.SetActive(true);
			if (this.Kira == null)
			{
				this.CreateKiraPar();
			}
			else
			{
				this.ParticlePanel.SetActive(true);
				this.Kira.SetActive(true);
			}
		}

		private void CreateKiraPar()
		{
			if (this.ParticlePanel == null)
			{
				this.ParticlePanel = base.get_transform().FindChild("ParticlePanel");
			}
			this.Kira = Util.Instantiate(Resources.Load("Prefabs/Common/KiraPar") as GameObject, this.ParticlePanel.get_gameObject(), false, false);
			float num = this.getMagnification() / base.get_transform().get_localScale().x;
			this.Kira.get_transform().localScaleX(num);
			this.Kira.get_transform().localScaleY(num);
			if (!this.ParticlePanel.get_gameObject().get_activeSelf())
			{
				this.ParticlePanel.get_gameObject().SetActive(true);
			}
		}

		private void InitMarriageRing(ShipModel shipModel)
		{
			if (shipModel != null && shipModel.IsMarriage())
			{
				if (this.Ring == null)
				{
					this.Ring = Util.Instantiate(Resources.Load("Prefabs/Common/MarriagedRing") as GameObject, this._uiShipTex.get_gameObject(), false, false);
				}
			}
			else if (this.Ring != null)
			{
				Object.Destroy(this.Ring);
				this.Ring = null;
			}
		}

		private bool isNeedKira()
		{
			return this._clsShipModel.ConditionState == FatigueState.Exaltation;
		}

		private bool isNeedSmoke()
		{
			return this._clsShipModel.IsDamaged();
		}

		private void OnValidate()
		{
			if (this._uiShipTex != null && this._uiShipTex.mainTexture != null)
			{
				float magnification = this.getMagnification();
				base.get_transform().localScaleX(magnification);
				base.get_transform().localScaleY(magnification);
			}
		}

		private float getMagnification()
		{
			float num = (float)this._uiShipTex.mainTexture.get_width();
			float num2 = (float)this._uiShipTex.mainTexture.get_height();
			Vector2 texelSize = this._uiShipTex.mainTexture.get_texelSize();
			return (float)this.sizeX / num;
		}

		public void ReleaseShipBannerTexture(bool unloadAsset = false)
		{
			if (this._uiShipTex != null)
			{
				if (this._uiShipTex.mainTexture != null && unloadAsset)
				{
					Resources.UnloadAsset(this._uiShipTex.mainTexture);
				}
				this._uiShipTex.mainTexture = null;
			}
		}
	}
}
