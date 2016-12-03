using KCV.Utils;
using local.models;
using local.utils;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Production
{
	public class ProdBattleRewardItem : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _uiPar;

		[SerializeField]
		private ParticleSystem _uiParticleComp;

		[SerializeField]
		private ParticleSystem _uiParticleStar1;

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem;

		[SerializeField]
		private UISprite _uiShipGet;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private Animation _getAnim;

		[SerializeField]
		private Animation _anim;

		private Generics.Message _clsShipMessage;

		private List<IReward> _iRewardList;

		private Animation _anime;

		private Animation _gearAnime;

		private Action _actCallback;

		private int _rewardCount;

		private bool _isInput;

		private KeyControl _clsInput;

		private int debugIndex;

		public void Init()
		{
			Util.FindParentToChild<ParticleSystem>(ref this._uiPar, base.get_transform(), "Particle");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticleComp, base.get_transform(), "ParticleComp");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticleStar1, base.get_transform(), "ParticleStar1");
			Util.FindParentToChild<UITexture>(ref this._uiRareBG, base.get_transform(), "RareBG");
			Util.FindParentToChild<UITexture>(ref this._uiItem, base.get_transform(), "Item");
			Util.FindParentToChild<UISprite>(ref this._uiShipGet, base.get_transform(), "MessageWindow/Get");
			Util.FindParentToChild<UISprite>(ref this._uiGear, base.get_transform(), "MessageWindow/NextBtn");
			Util.FindParentToChild<Animation>(ref this._getAnim, base.get_transform(), "MessageWindow/Get");
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			if (this._gearAnime == null)
			{
				this._gearAnime = this._uiGear.GetComponent<Animation>();
			}
			this._clsShipMessage = new Generics.Message(base.get_transform(), "MessageWindow/ShipMessage");
			this._uiParticleComp.SetActive(false);
			this._uiParticleStar1.SetActive(false);
			this._uiItem.alpha = 0f;
			this.debugIndex = 0;
			this._rewardCount = 0;
			this._uiParticleComp.SetActive(false);
			this._uiParticleStar1.SetActive(false);
			this._uiItem.alpha = 0f;
			this._setRewardItem();
			this._uiRareBG.alpha = 0f;
			this._uiPar.Stop();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiPar);
			Mem.Del(ref this._uiParticleComp);
			Mem.Del(ref this._uiParticleStar1);
			Mem.Del<UITexture>(ref this._uiRareBG);
			Mem.Del<UITexture>(ref this._uiItem);
			Mem.Del(ref this._uiShipGet);
			Mem.Del(ref this._uiGear);
			Mem.Del<Animation>(ref this._getAnim);
			Mem.Del<Animation>(ref this._anim);
			this._clsShipMessage.UnInit();
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<Animation>(ref this._gearAnime);
			Mem.Del<Action>(ref this._actCallback);
			Mem.DelList<IReward>(ref this._iRewardList);
			this._clsInput = null;
		}

		private void Update()
		{
			this._clsShipMessage.Update();
			if (this._isInput && this._clsInput.keyState.get_Item(1).down)
			{
				this.fadeOutExtinguish();
				this._isInput = false;
			}
		}

		public static ProdBattleRewardItem Instantiate(ProdBattleRewardItem prefab, Transform parent, List<IReward> iReward, int nDepth, KeyControl input)
		{
			ProdBattleRewardItem prodBattleRewardItem = Object.Instantiate<ProdBattleRewardItem>(prefab);
			prodBattleRewardItem.get_transform().set_parent(parent);
			prodBattleRewardItem.get_transform().set_localScale(Vector3.get_one());
			prodBattleRewardItem.get_transform().set_localPosition(Vector3.get_zero());
			prodBattleRewardItem.Init();
			prodBattleRewardItem._clsInput = input;
			prodBattleRewardItem.GetComponent<UIPanel>().depth = nDepth;
			prodBattleRewardItem._iRewardList = iReward;
			prodBattleRewardItem._setRewardItem();
			prodBattleRewardItem._uiRareBG.alpha = 0f;
			prodBattleRewardItem._uiPar.Stop();
			return prodBattleRewardItem;
		}

		private void _setRewardItem()
		{
			int slotItemID = 0;
			int num = 0;
			int num2 = 0;
			string message = string.Empty;
			if (this._iRewardList.get_Item(this._rewardCount) is IReward_Materials)
			{
				IReward_Materials reward_Materials = (IReward_Materials)this._iRewardList.get_Item(this._rewardCount);
				num = 1;
				message = string.Concat(new object[]
				{
					"「",
					reward_Materials.Rewards[0].Name,
					"」を",
					reward_Materials.Rewards[0].Count,
					"入手しました。"
				});
			}
			if (this._iRewardList.get_Item(this._rewardCount) is IReward_Slotitem)
			{
				IReward_Slotitem reward_Slotitem = (IReward_Slotitem)this._iRewardList.get_Item(this._rewardCount);
				slotItemID = reward_Slotitem.Id;
				switch (reward_Slotitem.Rare)
				{
				case 0:
					num = 1;
					num2 = 1;
					break;
				case 1:
					num = 2;
					num2 = 1;
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
				message = reward_Slotitem.Type3Name + "「" + reward_Slotitem.Name + "」を入手しました。";
			}
			if (this._iRewardList.get_Item(this._rewardCount) is IReward_Useitem)
			{
				IReward_Useitem reward_Useitem = (IReward_Useitem)this._iRewardList.get_Item(this._rewardCount);
				slotItemID = reward_Useitem.Id;
				num = 1;
				message = "「" + reward_Useitem.Name + "」を入手しました。";
			}
			this._uiItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotItemID, 1);
			this._uiItem.MakePixelPerfect();
			this._uiShipGet.alpha = 0f;
			this._clsShipMessage.Init(message, 0.04f, null);
			string text = (num2 != 0) ? "i_rare" : "s_rare";
			this._uiRareBG.mainTexture = (Resources.Load(string.Format("Textures/Common/RareBG/" + text + "{0}", num)) as Texture2D);
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
				IReward_Slotitem reward_Slotitem = new Reward_Slotitem(this.debugIndex);
				this._setRewardItem();
			}
			this._anime.Stop();
			this._anime.Play("comp_GetSlotItem");
		}

		public void Play(Action callback)
		{
			this._anime.Play("start_GetSlotItem");
			this._uiPar.Play();
			this._actCallback = callback;
			this._rewardCount++;
			SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void NextPlay(Action callback)
		{
			this._anime.Stop();
			this._anime.Play("start_GetSlotItem");
			this._uiPar.Play();
			this._actCallback = callback;
			SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		private void onClick()
		{
			this.compStartAnimation();
		}

		private void compStartAnimation()
		{
			this._uiPar.Stop();
			this._uiPar.set_time(0f);
			this._uiPar.get_gameObject().SetActive(false);
			this._uiRareBG.alpha = 1f;
			this._uiItem.alpha = 1f;
			this._uiParticleComp.SetActive(true);
			this._uiParticleComp.Play();
			this._uiParticleStar1.SetActive(true);
			this._uiParticleStar1.Play();
			this._anime.Stop();
			this._anime.Play("comp_GetSlotItem");
			TrophyUtil.Unlock_AlbumSlotNum();
		}

		private void startGearIcon()
		{
			this._gearAnime.Stop();
			this._gearAnime.Play();
			this._clsShipMessage.Play();
			this._isInput = true;
		}

		private void startGetIcon()
		{
			this._uiShipGet.alpha = 1f;
			this._getAnim.get_gameObject().SetActive(true);
			this._getAnim.Stop();
			this._getAnim.Play();
		}

		private void fadeOutExtinguish()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.StopSE();
			this._uiParticleComp.Stop();
			this._uiParticleComp.SetActive(false);
			this._uiParticleStar1.Stop();
			this._uiParticleStar1.SetActive(false);
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
			if (!this._isInput)
			{
				return;
			}
			this._isInput = false;
			this.fadeOutExtinguish();
		}
	}
}
