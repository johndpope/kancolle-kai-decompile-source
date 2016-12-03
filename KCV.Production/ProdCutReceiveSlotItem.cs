using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdCutReceiveSlotItem : MonoBehaviour
	{
		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Animation _getAnime;

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem;

		[SerializeField]
		private UITexture _uiMessageBG;

		[SerializeField]
		private UISprite _uiShipGet;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private UISprite _uiInner;

		[SerializeField]
		private ParticleSystem _uiStarPar1;

		[SerializeField]
		private ParticleSystem _uiStarPar2;

		private Generics.Message _clsShipMessage;

		private AudioSource _Se;

		private bool _isStartAnim;

		private bool _isInput;

		private bool _isUpdateShipGet;

		private bool _isUpdateNextBtn;

		private KeyControl _clsInput;

		private Action _actCallback;

		private IReward_Slotitem _clsRewardItem;

		private void Awake()
		{
			Util.FindParentToChild<UITexture>(ref this._uiRareBG, base.get_transform(), "RareBG");
			Util.FindParentToChild<UITexture>(ref this._uiItem, base.get_transform(), "Item");
			Util.FindParentToChild<UITexture>(ref this._uiMessageBG, base.get_transform(), "MessageWindow/MessageBG");
			Util.FindParentToChild<UISprite>(ref this._uiShipGet, base.get_transform(), "MessageWindow/Get");
			Util.FindParentToChild<UISprite>(ref this._uiGear, base.get_transform(), "MessageWindow/NextBtn");
			Util.FindParentToChild<UISprite>(ref this._uiInner, base.get_transform(), "MessageWindow/NextBtn/Gear");
			Util.FindParentToChild<ParticleSystem>(ref this._uiStarPar1, base.get_transform(), "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref this._uiStarPar2, base.get_transform(), "ParticleStar2");
			this._clsShipMessage = new Generics.Message(base.get_transform(), "MessageWindow/ShipMessage");
			this._anime = base.GetComponent<Animation>();
			this._getAnime = this._uiShipGet.GetComponent<Animation>();
			this._uiItem.alpha = 0f;
			this._uiStarPar1.SetActive(false);
			this._uiStarPar2.SetActive(false);
		}

		private void OnDestroy()
		{
			this._anime = null;
			this._uiRareBG = null;
			this._uiItem = null;
			this._uiMessageBG = null;
			this._uiShipGet = null;
			this._uiGear = null;
			this._uiInner = null;
			this._clsShipMessage.UnInit();
			this._actCallback = null;
			this._clsInput = null;
		}

		private void Update()
		{
			if (this._isUpdateNextBtn)
			{
				this._clsShipMessage.Update();
			}
			if (this._isUpdateNextBtn)
			{
				this._uiInner.get_transform().Rotate(-50f * Time.get_deltaTime() * Vector3.get_forward());
			}
			if (this._isInput && this._clsInput.keyState.get_Item(1).down)
			{
				this._fadeOutExtinguish();
				this._isInput = false;
			}
		}

		public void Init(IReward_Slotitem rewardItem)
		{
			this._setRewardItem(rewardItem);
			this._uiRareBG.alpha = 0f;
			this._uiRareBG.mainTexture = TextureFile.LoadRareBG(1);
			this._uiItem.alpha = 0f;
			this._uiStarPar1.Stop();
			this._uiStarPar2.Stop();
			this._uiStarPar1.SetActive(false);
			this._uiStarPar2.SetActive(false);
		}

		public static ProdCutReceiveSlotItem Instantiate(ProdCutReceiveSlotItem prefab, Transform parent, IReward_Slotitem rewardItem, int nPanelDepth, KeyControl input)
		{
			ProdCutReceiveSlotItem prodCutReceiveSlotItem = Object.Instantiate<ProdCutReceiveSlotItem>(prefab);
			prodCutReceiveSlotItem.get_transform().set_parent(parent);
			prodCutReceiveSlotItem.get_transform().set_localScale(Vector3.get_one());
			prodCutReceiveSlotItem.get_transform().set_localPosition(Vector3.get_zero());
			prodCutReceiveSlotItem._setRewardItem(rewardItem);
			prodCutReceiveSlotItem._uiRareBG.alpha = 0f;
			prodCutReceiveSlotItem._uiRareBG.mainTexture = TextureFile.LoadRareBG(1);
			prodCutReceiveSlotItem.GetComponent<UIPanel>().depth = nPanelDepth;
			prodCutReceiveSlotItem._clsInput = input;
			return prodCutReceiveSlotItem;
		}

		private void _setRewardItem(IReward_Slotitem rewardItem)
		{
			this._clsRewardItem = rewardItem;
			this._uiItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this._clsRewardItem.Id, 1);
			this._uiItem.MakePixelPerfect();
			this._uiShipGet.alpha = 0f;
			this._uiGear.alpha = 0f;
			this._isUpdateShipGet = false;
			this._isUpdateNextBtn = false;
			this._clsShipMessage.Init(this._clsRewardItem.Type3Name + "「" + this._clsRewardItem.Name + "」を入手しました。", 0.04f, null);
			UIButtonMessage component = this._uiGear.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "_receiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		public void Play(Action callback)
		{
			this._isStartAnim = true;
			this._uiRareBG.alpha = 1f;
			this._uiItem.alpha = 1f;
			this._actCallback = callback;
			this._anime.Stop();
			this._anime.Play("start_GetSlotItemCut");
			this._uiStarPar1.SetActive(true);
			this._uiStarPar2.SetActive(true);
			this._uiStarPar1.Play();
			this._uiStarPar2.Play();
			this._Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		private void _startMessageBox()
		{
			this._uiMessageBG.alpha = 1f;
		}

		private void _startGearIcon()
		{
			this._uiGear.alpha = 1f;
			this._isUpdateNextBtn = true;
			this._getAnime.Stop();
			this._getAnime.Play();
			this._clsShipMessage.Play();
			this._isInput = true;
		}

		private void _startGetIcon()
		{
			this._uiShipGet.alpha = 1f;
			this._isUpdateShipGet = true;
		}

		private void _fadeOutExtinguish()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			SoundUtils.StopSE(0.5f, new AudioSource[]
			{
				this._Se
			});
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this._discard();
		}

		private void _discard()
		{
			Object.Destroy(base.get_gameObject(), 0.1f);
		}

		private void _receiveShipEL(GameObject obj)
		{
			if (!this._isInput)
			{
				return;
			}
			this._isInput = false;
			this._fadeOutExtinguish();
		}
	}
}
