using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesConvertShipManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite fade;

		[SerializeField]
		private Transform mTransform_Convert;

		[SerializeField]
		private Transform mTransform_ConvertComplete;

		[SerializeField]
		private UITexture bg;

		[SerializeField]
		private UITexture bg2;

		[SerializeField]
		private UIPanel bg2mask;

		[SerializeField]
		private UITexture card;

		[SerializeField]
		private Transform mShipOffsetFrame;

		[SerializeField]
		private UITexture ship;

		[SerializeField]
		private PortUpgradesConvertShipReturnButton retBtn;

		[SerializeField]
		private GameObject stripe;

		[SerializeField]
		private GameObject snowflakeInit;

		private GameObject[] snowflakes;

		private bool on;

		public bool finish;

		private float timer;

		public bool isFinished;

		private ShipModelMst mTargetShipModelMst;

		private bool enabledKey;

		private KeyControl mKeyController;

		public void Awake()
		{
			this.snowflakes = new GameObject[40];
			this.fade.alpha = 0f;
			this.bg.alpha = 0f;
			this.bg2.alpha = 0f;
			this.card.alpha = 0f;
			this.ship.alpha = 0f;
			UISprite[] componentsInChildren = this.stripe.GetComponentsInChildren<UISprite>();
			UISprite[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UISprite uISprite = array[i];
				uISprite.alpha = 0f;
			}
			this.stripe.get_transform().set_localScale(new Vector3(1f, 0.01f, 1f));
			this.on = false;
			this.finish = false;
			this.isFinished = false;
			this.timer = 0f;
		}

		public void Initialize(ShipModelMst targetShipMst, int bgID, int bg2ID, int startDepth)
		{
			this.mTargetShipModelMst = targetShipMst;
			Vector3 localPosition = Util.Poi2Vec(this.mTargetShipModelMst.Offsets.GetShipDisplayCenter(false));
			this.ship.get_transform().set_localPosition(localPosition);
			this.on = true;
			this.timer = Time.get_time();
			this.ship.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mTargetShipModelMst.GetGraphicsMstId(), 9);
			this.ship.GetComponent<UITexture>().MakePixelPerfect();
			this.card.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mTargetShipModelMst.MstId, 3);
			if (bgID == 0)
			{
				this.bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_1") as Texture);
			}
			else if (bgID == 1)
			{
				this.bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_2") as Texture);
			}
			else if (bgID == 2)
			{
				this.bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_3") as Texture);
			}
			else if (bgID == 3)
			{
				this.bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_1") as Texture);
			}
			else if (bgID == 4)
			{
				this.bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_2") as Texture);
			}
			else if (bgID == 5)
			{
				this.bg.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_horo_1") as Texture);
			}
			if (this.bg.mainTexture == null)
			{
				Debug.Log("Failed to load texture for ./BG/BG");
			}
			if (bg2ID == 0)
			{
				this.bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_1") as Texture);
			}
			else if (bg2ID == 1)
			{
				this.bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_2") as Texture);
			}
			else if (bg2ID == 2)
			{
				this.bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_common_3") as Texture);
			}
			else if (bg2ID == 3)
			{
				this.bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_1") as Texture);
			}
			else if (bg2ID == 4)
			{
				this.bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_rare_2") as Texture);
			}
			else if (bg2ID == 5)
			{
				this.bg2.mainTexture = (Resources.Load("Textures/ConvertShip/getBG_horo_1") as Texture);
			}
			base.StartCoroutine(this.EnableAlphas());
			base.StartCoroutine(this.StripeDisable());
			base.StartCoroutine(this.SnowflakeExplosion());
			base.StartCoroutine(this.CardToFront());
			base.StartCoroutine(this.EnableButton());
			base.get_transform().Find("BG").GetComponent<UIPanel>().depth = startDepth;
			base.get_transform().Find("BG2").GetComponent<UIPanel>().depth = startDepth + 1;
			base.get_transform().Find("Main").GetComponent<UIPanel>().depth = startDepth + 2;
		}

		public void Update()
		{
			if (this.mKeyController != null && this.enabledKey && this.mKeyController.keyState.get_Item(1).down)
			{
				this.finish = true;
			}
			if (this.on)
			{
				if (Time.get_time() - this.timer <= 1f)
				{
					this.fade.alpha += Mathf.Min(Time.get_deltaTime(), 1f - this.fade.alpha);
				}
				if (Time.get_time() - this.timer >= 1f && Time.get_time() - this.timer <= 2f)
				{
					this.fade.alpha -= Mathf.Min(Time.get_deltaTime(), this.fade.alpha);
				}
				if (Time.get_time() - this.timer >= 1f && Time.get_time() - this.timer <= 2.25f)
				{
					this.StripeExpand();
				}
				if (Time.get_time() - this.timer >= 1.5f && Time.get_time() - this.timer <= 2f)
				{
					this.ConvertCompleteSlide();
				}
				if (Time.get_time() - this.timer >= 3.25f && Time.get_time() - this.timer <= 4.5f)
				{
					this.StripeContract();
				}
				if (Time.get_time() - this.timer >= 4.5f && Time.get_time() - this.timer <= 5.5f)
				{
					this.BGExpand();
				}
				if (Time.get_time() - this.timer >= 4.75f && Time.get_time() - this.timer <= 6.25f)
				{
					this.ShipSlide();
				}
				if (Time.get_time() - this.timer >= 8f && Time.get_time() - this.timer <= 10f)
				{
					this.CardSwap();
				}
				if (this.finish)
				{
					this.finish = false;
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					TrophyUtil.Unlock_GetShip(this.mTargetShipModelMst.MstId);
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						this.isFinished = true;
					});
				}
			}
		}

		private void StripeExpand()
		{
			Transform expr_0B = this.stripe.get_transform();
			expr_0B.set_localScale(expr_0B.get_localScale() + new Vector3(0f, 0.8f * Time.get_deltaTime(), 0f));
			Transform expr_3B = this.mTransform_Convert;
			expr_3B.set_localPosition(expr_3B.get_localPosition() + new Vector3(700f * Time.get_deltaTime(), 0f, 0f));
		}

		private void ConvertCompleteSlide()
		{
			Transform expr_06 = this.mTransform_ConvertComplete;
			expr_06.set_localPosition(expr_06.get_localPosition() + new Vector3(1334f * Time.get_deltaTime(), 0f, 0f));
		}

		private void StripeContract()
		{
			Transform expr_0B = this.stripe.get_transform();
			expr_0B.set_localScale(expr_0B.get_localScale() - new Vector3(0f, 0.8f * Time.get_deltaTime(), 0f));
			Transform expr_3B = this.mTransform_Convert;
			expr_3B.set_localPosition(expr_3B.get_localPosition() + new Vector3(600f * Time.get_deltaTime(), 0f, 0f));
			Transform expr_6B = this.mTransform_ConvertComplete;
			expr_6B.set_localPosition(expr_6B.get_localPosition() + new Vector3(960f * Time.get_deltaTime(), 0f, 0f));
		}

		private void BGExpand()
		{
			this.bg2mask.baseClipRegion += new Vector4(0f, 0f, 0f, 544f * Time.get_deltaTime());
		}

		private void ShipSlide()
		{
			Transform expr_06 = this.mShipOffsetFrame;
			expr_06.set_localPosition(expr_06.get_localPosition() + new Vector3(0f, 300f * Time.get_deltaTime(), 0f));
			this.ship.alpha += Mathf.Min(Time.get_deltaTime(), 1f - this.ship.alpha);
		}

		private void CardSwap()
		{
			this.ship.alpha -= Mathf.Min(0.5f * Time.get_deltaTime(), this.ship.alpha);
			Transform expr_33 = this.mShipOffsetFrame;
			expr_33.set_localPosition(expr_33.get_localPosition() - new Vector3(300f * Time.get_deltaTime() * Mathf.Sin(3.14159274f * (Time.get_time() - this.timer)), 0f, 0f));
			Transform expr_7B = this.mShipOffsetFrame;
			expr_7B.set_localScale(expr_7B.get_localScale() - new Vector3(0.125f * Time.get_deltaTime(), 0.125f * Time.get_deltaTime(), 0f));
			this.card.alpha += Mathf.Min(0.5f * Time.get_deltaTime(), 1f - this.card.alpha);
			Transform expr_E9 = this.card.get_transform();
			expr_E9.set_localPosition(expr_E9.get_localPosition() + new Vector3(300f * Time.get_deltaTime() * Mathf.Sin(3.14159274f * (Time.get_time() - this.timer)), 0f, 0f));
			Transform expr_136 = this.card.get_transform();
			expr_136.set_localScale(expr_136.get_localScale() + new Vector3(0.125f * Time.get_deltaTime(), 0.125f * Time.get_deltaTime(), 0f));
		}

		[DebuggerHidden]
		public IEnumerator EnableAlphas()
		{
			PortUpgradesConvertShipManager.<EnableAlphas>c__Iterator1B2 <EnableAlphas>c__Iterator1B = new PortUpgradesConvertShipManager.<EnableAlphas>c__Iterator1B2();
			<EnableAlphas>c__Iterator1B.<>f__this = this;
			return <EnableAlphas>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator StripeDisable()
		{
			PortUpgradesConvertShipManager.<StripeDisable>c__Iterator1B3 <StripeDisable>c__Iterator1B = new PortUpgradesConvertShipManager.<StripeDisable>c__Iterator1B3();
			<StripeDisable>c__Iterator1B.<>f__this = this;
			return <StripeDisable>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator SnowflakeExplosion()
		{
			PortUpgradesConvertShipManager.<SnowflakeExplosion>c__Iterator1B4 <SnowflakeExplosion>c__Iterator1B = new PortUpgradesConvertShipManager.<SnowflakeExplosion>c__Iterator1B4();
			<SnowflakeExplosion>c__Iterator1B.<>f__this = this;
			return <SnowflakeExplosion>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator CardToFront()
		{
			PortUpgradesConvertShipManager.<CardToFront>c__Iterator1B5 <CardToFront>c__Iterator1B = new PortUpgradesConvertShipManager.<CardToFront>c__Iterator1B5();
			<CardToFront>c__Iterator1B.<>f__this = this;
			return <CardToFront>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator EnableButton()
		{
			PortUpgradesConvertShipManager.<EnableButton>c__Iterator1B6 <EnableButton>c__Iterator1B = new PortUpgradesConvertShipManager.<EnableButton>c__Iterator1B6();
			<EnableButton>c__Iterator1B.<>f__this = this;
			return <EnableButton>c__Iterator1B;
		}

		private void PlayVoice()
		{
			ShipUtils.PlayShipVoice(this.mTargetShipModelMst, 10);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void Finish()
		{
			this.finish = true;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.fade);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.bg, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.bg2, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.bg2mask);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.card, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.ship, false);
			this.mTransform_Convert = null;
			this.mTransform_ConvertComplete = null;
			this.mShipOffsetFrame = null;
			this.retBtn = null;
			this.stripe = null;
			this.snowflakeInit = null;
			if (this.snowflakes != null)
			{
				for (int i = 0; i < this.snowflakes.Length; i++)
				{
					this.snowflakes[i] = null;
				}
			}
			this.snowflakes = null;
			this.mTargetShipModelMst = null;
		}
	}
}
