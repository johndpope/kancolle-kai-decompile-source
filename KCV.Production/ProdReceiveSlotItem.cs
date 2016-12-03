using KCV.Utils;
using local.models;
using local.utils;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdReceiveSlotItem : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _uiPar;

		[SerializeField]
		private ParticleSystem _uiParticleComp;

		[SerializeField]
		private ParticleSystem _uiParticleStar1;

		[SerializeField]
		private ParticleSystem _uiParticleStar2;

		[SerializeField]
		private ParticleSystem _uiParticleMiss;

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem;

		[SerializeField]
		private GameObject _missObj;

		[SerializeField]
		private UISprite _uiShipGet;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private UIButton _uiBG;

		[SerializeField]
		protected Animation _getIconAnim;

		private Generics.Message _clsShipMessage;

		protected AudioSource _Se;

		private IReward_Slotitem _clsRewardItem;

		private Animation _anime;

		private Animation _gearAnime;

		private Action _actCallback;

		private int _rewardCount;

		private bool _isStartAnim;

		private bool _isEnabled;

		private bool _isExtinguish;

		private bool _isInput;

		private bool _isArsenal;

		private bool _isPlayPhase1;

		private bool _isUpdateNextBtn;

		private int timer;

		private KeyControl _clsInput;

		private int debugIndex;

		private bool _gearTouchableflag;

		private void Awake()
		{
			Util.FindParentToChild<ParticleSystem>(ref this._uiPar, base.get_transform(), "Particle");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticleComp, base.get_transform(), "ParticleComp");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticleMiss, base.get_transform(), "Miss/Reaf/MissPar");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticleStar1, base.get_transform(), "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticleStar2, base.get_transform(), "ParticleStar2");
			Util.FindParentToChild<UITexture>(ref this._uiRareBG, base.get_transform(), "RareBG");
			Util.FindParentToChild<UIButton>(ref this._uiBG, base.get_transform(), "BG");
			Util.FindParentToChild<UITexture>(ref this._uiItem, base.get_transform(), "Item");
			Util.FindParentToChild<UISprite>(ref this._uiShipGet, base.get_transform(), "MessageWindow/Get");
			Util.FindParentToChild<UISprite>(ref this._uiGear, base.get_transform(), "MessageWindow/NextBtn");
			Util.FindParentToChild<Animation>(ref this._getIconAnim, base.get_transform(), "MessageWindow/Get");
			this._anime = base.GetComponent<Animation>();
			this._gearAnime = this._uiGear.GetComponent<Animation>();
			this._missObj = base.get_transform().FindChild("Miss").get_gameObject();
			this._clsShipMessage = new Generics.Message(base.get_transform(), "MessageWindow/ShipMessage");
			this._uiParticleComp.SetActive(false);
			this._uiParticleStar1.SetActive(false);
			this._uiParticleStar2.SetActive(false);
			this._uiItem.alpha = 0f;
			this._isExtinguish = false;
			this.debugIndex = 0;
			this._rewardCount = 0;
		}

		public void Init()
		{
			this._uiParticleComp.SetActive(false);
			this._uiParticleStar1.SetActive(false);
			this._uiParticleStar2.SetActive(false);
			this._uiItem.alpha = 0f;
			this._isExtinguish = false;
			this._uiRareBG.alpha = 0f;
			this._uiPar.Stop();
			this._isStartAnim = false;
			this._isPlayPhase1 = false;
			this._isEnabled = base.get_enabled();
			this._uiBG.set_enabled(true);
			this._gearTouchableflag = false;
		}

		private void OnDestroy()
		{
			this._anime = null;
			this._uiPar = null;
			this._uiRareBG = null;
			this._uiItem = null;
			this._clsShipMessage.UnInit();
			this._actCallback = null;
			this._isStartAnim = false;
			this._clsInput = null;
			Mem.Del<AudioSource>(ref this._Se);
			Mem.Del<Animation>(ref this._getIconAnim);
		}

		private void Update()
		{
			if (this._isUpdateNextBtn)
			{
				this._clsShipMessage.Update();
			}
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
					this.fadeOutExtinguish();
					this._isInput = false;
				}
			}
		}

		public static ProdReceiveSlotItem Instantiate(ProdReceiveSlotItem prefab, Transform parent, IReward_Slotitem rewardItem, int nPanelDepth, KeyControl input, bool enabled, bool arsenal)
		{
			ProdReceiveSlotItem prodReceiveSlotItem = Object.Instantiate<ProdReceiveSlotItem>(prefab);
			prodReceiveSlotItem.get_transform().set_parent(parent);
			prodReceiveSlotItem.get_transform().set_localScale(Vector3.get_one());
			prodReceiveSlotItem.get_transform().set_localPosition(Vector3.get_zero());
			prodReceiveSlotItem._isStartAnim = false;
			prodReceiveSlotItem._isPlayPhase1 = false;
			input.keyState.get_Item(1).down = false;
			prodReceiveSlotItem._clsInput = input;
			prodReceiveSlotItem.GetComponent<UIPanel>().depth = nPanelDepth;
			if (enabled)
			{
				prodReceiveSlotItem._setRewardItem(rewardItem);
				prodReceiveSlotItem._uiRareBG.alpha = 0f;
			}
			else
			{
				prodReceiveSlotItem._setRewardMiss();
			}
			prodReceiveSlotItem._missObj.SetActive(false);
			if (parent.get_transform().get_parent().get_name() == "TaskArsenalMain" || parent.get_transform().get_parent().get_name() == "DescriptionCamera")
			{
				prodReceiveSlotItem._isArsenal = true;
			}
			prodReceiveSlotItem._uiPar.Stop();
			prodReceiveSlotItem._isStartAnim = true;
			prodReceiveSlotItem._isEnabled = enabled;
			prodReceiveSlotItem._isArsenal = arsenal;
			return prodReceiveSlotItem;
		}

		private void _setRewardItem(IReward_Slotitem rewardItem)
		{
			this._clsRewardItem = rewardItem;
			this._uiItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this._clsRewardItem.Id, 1);
			this._uiItem.MakePixelPerfect();
			this._uiShipGet.alpha = 0f;
			this._isUpdateNextBtn = false;
			this._clsShipMessage.Init(this._clsRewardItem.Type3Name + "「" + this._clsRewardItem.Name + "」を入手しました。", 0.04f, null);
			int num = 0;
			switch (this._clsRewardItem.Rare)
			{
			case 0:
				num = 1;
				break;
			case 1:
				num = 2;
				break;
			case 2:
				num = 6;
				break;
			case 3:
				num = 6;
				break;
			case 4:
				num = 6;
				break;
			case 5:
				num = 7;
				break;
			}
			string text = (this._clsRewardItem.Rare < 2) ? "i_rare" : "s_rare_";
			this._uiRareBG.mainTexture = (Resources.Load(string.Format("Textures/Common/RareBG/" + text + "{0}", num)) as Texture2D);
			Debug.Log(string.Format("Textures/Common/RareBG/" + text + "{0}", num));
			UIButtonMessage component = this._uiGear.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "prodReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		private void _debugRewardItem()
		{
			Debug.Log("ItemID:" + this.debugIndex);
			if (Mst_DataManager.Instance.Mst_Slotitem.ContainsKey(this.debugIndex))
			{
				IReward_Slotitem rewardItem = new Reward_Slotitem(this.debugIndex);
				this._setRewardItem(rewardItem);
			}
			this._anime.Stop();
			this._anime.Play("comp_GetSlotItem");
		}

		private void _setRewardMiss()
		{
			this._uiShipGet.alpha = 0f;
			this._isUpdateNextBtn = false;
			this._clsShipMessage.Init(" 装備の開発に失敗しました。\n\n『開発資材』は消費しませんでした。", 0.04f, null);
			UIButtonMessage component = this._uiGear.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "prodReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		public void Play(Action callback)
		{
			this._anime.Play("start_GetSlotItem");
			this._uiPar.Play();
			this._isStartAnim = true;
			this._isInput = true;
			this._isPlayPhase1 = true;
			this.timer = 0;
			this._actCallback = callback;
			this._rewardCount++;
			this._Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void NextPlay(Action callback)
		{
			this._anime.Stop();
			this._anime.Play("start_GetSlotItem");
			this._uiPar.Play();
			this._isStartAnim = true;
			this._actCallback = callback;
			this._Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void onClick()
		{
			this.compStartAnimation();
		}

		public void onScreenTap()
		{
			this.compStartAnimation();
		}

		private void compStartAnimation()
		{
			this._uiBG.set_enabled(false);
			this._isInput = false;
			this._isPlayPhase1 = false;
			this._uiPar.Stop();
			this._uiPar.set_time(0f);
			this._uiPar.get_gameObject().SetActive(false);
			if (this._isEnabled)
			{
				this._uiRareBG.alpha = 1f;
				this._uiItem.alpha = 1f;
				if (this._isArsenal)
				{
					this._uiParticleComp.SetActive(true);
					this._uiParticleComp.Play();
				}
				else
				{
					this._uiParticleStar1.SetActive(true);
					this._uiParticleStar2.SetActive(true);
					this._uiParticleStar1.Play();
					this._uiParticleStar2.Play();
				}
				this._anime.Stop();
				this._anime.Play("comp_GetSlotItem");
				TrophyUtil.Unlock_AlbumSlotNum();
			}
			else
			{
				this._anime.Stop();
				this._anime.Play("miss_GetSlotItem");
				this._missObj.SetActive(true);
				this._uiParticleMiss.Play();
			}
		}

		private void Success_Voice()
		{
			if (this._isEnabled && base.get_transform().get_parent().get_parent().get_name() == "TaskArsenalMain")
			{
				ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip(), 26);
			}
		}

		private void Success_SE()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_010);
		}

		private void Failure_SE()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_011);
		}

		private void compMissAnimation()
		{
			this._uiParticleMiss.Stop();
		}

		private void startGearIcon()
		{
			this._isUpdateNextBtn = true;
			this._gearAnime.Stop();
			this._gearAnime.Play();
			this._clsShipMessage.Play();
			this._isInput = true;
			this._gearTouchableflag = true;
		}

		private void startGetIcon()
		{
			this._uiShipGet.alpha = 1f;
			this._getIconAnim.Stop();
			this._getIconAnim.Play();
		}

		private void fadeOutExtinguish()
		{
			if (this._isArsenal)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopSE();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			SoundUtils.StopSE(0.5f, new AudioSource[]
			{
				this._Se
			});
			this._uiParticleComp.Stop();
			this._uiParticleComp.SetActive(false);
			this._uiParticleMiss.Stop();
			this._uiParticleMiss.SetActive(false);
			this._uiParticleStar1.Stop();
			this._uiParticleStar1.SetActive(false);
			this._uiParticleStar2.Stop();
			this._uiParticleStar2.SetActive(false);
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this.discard();
		}

		private void discard()
		{
			Object.Destroy(base.get_gameObject(), 0.1f);
		}

		private void prodReceiveShipEL(GameObject obj)
		{
			if (!this._gearTouchableflag)
			{
				return;
			}
			if (!this._isInput)
			{
				return;
			}
			this._isInput = false;
			this.fadeOutExtinguish();
		}

		public void ReleaseTextures()
		{
			this._uiPar = null;
			if (this._uiParticleComp != null)
			{
				this._uiParticleComp.Stop();
			}
			this._uiParticleComp = null;
			if (this._uiParticleStar1 != null)
			{
				this._uiParticleStar1.Stop();
			}
			this._uiParticleStar1 = null;
			if (this._uiParticleStar2 != null)
			{
				this._uiParticleStar2.Stop();
			}
			this._uiParticleStar2 = null;
			if (this._uiParticleMiss != null)
			{
				this._uiParticleMiss.Stop();
			}
			this._uiParticleMiss = null;
			if (this._uiRareBG != null)
			{
				if (this._uiRareBG.mainTexture != null)
				{
					Resources.UnloadAsset(this._uiRareBG.mainTexture);
				}
				this._uiRareBG.mainTexture = null;
			}
			this._uiRareBG = null;
			if (this._uiItem != null)
			{
				if (this._uiItem.mainTexture != null)
				{
					Resources.UnloadAsset(this._uiItem.mainTexture);
				}
				this._uiItem.mainTexture = null;
			}
			this._uiItem = null;
			this._missObj = null;
			this._uiShipGet = null;
			this._uiGear = null;
			this._uiBG = null;
			if (this._getIconAnim != null)
			{
				this._getIconAnim.Stop();
			}
			this._getIconAnim = null;
			this._Se = null;
			this._clsRewardItem = null;
			if (this._anime != null)
			{
				this._anime.Stop();
			}
			this._anime = null;
			if (this._gearAnime != null)
			{
				this._gearAnime.Stop();
			}
			this._gearAnime = null;
			this._actCallback = null;
			this._clsInput = null;
		}
	}
}
