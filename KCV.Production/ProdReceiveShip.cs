using KCV.Utils;
using local.models;
using local.utils;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdReceiveShip : BaseReceiveShip
	{
		[SerializeField]
		private ParticleSystem _uiParComp;

		[SerializeField]
		private ParticleSystem _uiParticle;

		private bool _isNeedBGM;

		private bool _isPlayPhase1;

		private int timer;

		protected override void init()
		{
			base.GetComponent<UIPanel>().alpha = 0f;
			base.init();
			Util.FindParentToChild<ParticleSystem>(ref this._uiParComp, base.get_transform(), "ParticleComp");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticle, base.get_transform(), "Particle");
			this._uiShip.alpha = 0f;
			this._uiParticle.SetActive(false);
			this._uiParComp.SetActive(false);
			this.timer = 0;
			this._isPlayPhase1 = false;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref this._uiParComp);
			Mem.Del(ref this._uiParticle);
		}

		private void Update()
		{
			base.Run();
			if (this.timer <= 1)
			{
				this.timer++;
				return;
			}
			if (this._isInput && this._clsInput.keyState.get_Item(1).down)
			{
				if (this._isPlayPhase1)
				{
					this.compStartAnimation();
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.FadeOutExtinguish();
					this._isInput = false;
				}
			}
		}

		public static ProdReceiveShip Instantiate(ProdReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input)
		{
			return ProdReceiveShip.Instantiate(prefab, parent, rewardShip, nPanelDepth, input, true);
		}

		public static ProdReceiveShip Instantiate(ProdReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input, bool needBGM)
		{
			ProdReceiveShip prodReceiveShip = Object.Instantiate<ProdReceiveShip>(prefab);
			prodReceiveShip.get_transform().set_parent(parent);
			prodReceiveShip.get_transform().set_localScale(Vector3.get_one());
			prodReceiveShip.get_transform().set_localPosition(Vector3.get_zero());
			prodReceiveShip.init();
			prodReceiveShip._clsRewardShip = rewardShip;
			prodReceiveShip.GetComponent<UIPanel>().depth = nPanelDepth;
			prodReceiveShip._clsInput = input;
			prodReceiveShip._isNeedBGM = needBGM;
			prodReceiveShip._anim.Stop();
			return prodReceiveShip;
		}

		public void Play(Action callback)
		{
			base._setRewardShip();
			this._actCallback = callback;
			base.GetComponent<UIPanel>().alpha = 1f;
			this._anim.Play("start_GetShip");
			this._uiParticle.SetActive(true);
			this._uiParticle.Play();
			this._Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
			this._isPlayPhase1 = true;
			this._isInput = true;
		}

		public void onScreenTap()
		{
			this.compStartAnimation();
		}

		private void compStartAnimation()
		{
			if (!this._isNeedBGM)
			{
				this._uiParComp.SetActive(true);
				this._uiParComp.Play();
			}
			this._uiParticle.Stop();
			this._uiParticle.SetActive(false);
			this._uiShip.alpha = 1f;
			this._uiBg.mainTexture = TextureFile.LoadRareBG(this._clsRewardShip.Ship.Rare);
			this._getIconAnim.get_gameObject().SetActive(true);
			this._getIconAnim.Stop();
			this._getIconAnim.Play();
			this._isPlayPhase1 = false;
			this._isInput = false;
			this._anim.Stop();
			this._anim.Play("comp_GetShip");
			TrophyUtil.Unlock_At_BuildShip(this._clsRewardShip.Ship.MstId);
			TrophyUtil.Unlock_GetShip(this._clsRewardShip.Ship.MstId);
		}

		public void showMessage()
		{
			this._clsShipName.SetActive(true);
			this._clsSType.SetActive(true);
			this._clsShipName.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._clsShipName.get_gameObject(), string.Empty);
			this._clsSType.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._clsSType.get_gameObject(), string.Empty);
		}

		private void startMessage()
		{
			ShipUtils.PlayShipVoice(this._clsRewardShip.Ship, 1);
			this._clsShipMessage.Play();
			this.showMessage();
			this._uiGear.GetComponent<Collider2D>().set_enabled(true);
			this._isInput = true;
			this._gearAnim.Stop();
			this._gearAnim.Play();
		}

		private void FadeOutExtinguish()
		{
			if (this._isNeedBGM)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			}
			SoundUtils.StopSE(0.5f, new AudioSource[]
			{
				this._Se
			});
			this._uiParComp.Stop();
			this._uiParComp.SetActive(false);
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			base.Discard();
		}

		private void prodReceiveShipEL(GameObject obj)
		{
			if (!this._isInput)
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.FadeOutExtinguish();
			this._isInput = false;
		}

		private void backgroundEL(GameObject obj)
		{
			if (this._isPlayPhase1)
			{
				this.compStartAnimation();
			}
		}

		public void ReleaseShipTextureAndBackgroundTexture()
		{
			this.ReleaseBackgroundTexture();
			this.ReleaseShipTexture();
			this._clsShipName = null;
			this._clsSType = null;
			if (this._getIconAnim != null)
			{
				this._getIconAnim.Stop();
			}
			this._getIconAnim = null;
			this._uiGear = null;
			if (this._anim != null)
			{
				this._anim.Stop();
			}
			this._anim = null;
			if (this._gearAnim != null)
			{
				this._gearAnim.Stop();
			}
			this._gearAnim = null;
			this._Se = null;
			this._clsRewardShip = null;
			this._actCallback = null;
			this._clsInput = null;
		}

		private void ReleaseShipTexture()
		{
			if (this._uiShip != null)
			{
				if (this._uiShip.mainTexture != null)
				{
					Resources.UnloadAsset(this._uiShip.mainTexture);
				}
				this._uiShip.mainTexture = null;
			}
		}

		private void ReleaseBackgroundTexture()
		{
			if (this._uiBg != null)
			{
				if (this._uiBg.mainTexture != null)
				{
					Resources.UnloadAsset(this._uiBg.mainTexture);
				}
				this._uiBg.mainTexture = null;
			}
		}
	}
}
