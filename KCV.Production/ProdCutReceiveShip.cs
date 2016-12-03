using KCV.Utils;
using local.models;
using local.utils;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdCutReceiveShip : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private UILabel _clsShipName;

		[SerializeField]
		private UILabel _clsSType;

		[SerializeField]
		private UITexture _uiMessageBG;

		[SerializeField]
		private UIButtonMessage _uiGearBtn;

		[SerializeField]
		private ParticleSystem _uiParStar1;

		[SerializeField]
		private ParticleSystem _uiParStar2;

		[SerializeField]
		private Animation _anim;

		[SerializeField]
		private Animation _getIconAnim;

		[SerializeField]
		private Animation _gearAnim;

		private Generics.Message _clsShipMessage;

		private AudioSource _Se;

		private IReward_Ship _clsRewardShip;

		private Action _actCallback;

		private bool _isFinished;

		private bool _isInput;

		private bool _isNeedBGM;

		private bool _isBGMove;

		private KeyControl _clsInput;

		public bool IsFinished
		{
			get
			{
				return this._isFinished;
			}
		}

		public void Init()
		{
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			Util.FindParentToChild<UITexture>(ref this._uiRareBG, base.get_transform(), "RareBG");
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "ShipLayoutOffset/Ship");
			Util.FindParentToChild<UILabel>(ref this._clsShipName, base.get_transform(), "MessageWindow/ShipName");
			Util.FindParentToChild<UILabel>(ref this._clsSType, base.get_transform(), "MessageWindow/ShipType");
			Util.FindParentToChild<UITexture>(ref this._uiMessageBG, base.get_transform(), "MessageWindow/MessageBG");
			Util.FindParentToChild<UIButtonMessage>(ref this._uiGearBtn, base.get_transform(), "MessageWindow/NextBtn");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParStar1, base.get_transform(), "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParStar2, base.get_transform(), "ParticleStar2");
			Util.FindParentToChild<Animation>(ref this._getIconAnim, base.get_transform(), "MessageWindow/Get");
			if (this._gearAnim == null)
			{
				this._gearAnim = this._uiGearBtn.GetComponent<Animation>();
			}
			this._clsShipMessage = new Generics.Message(base.get_transform(), "MessageWindow/ShipMessage");
			this._uiShip.alpha = 0f;
			this._isFinished = false;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiRareBG);
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.Del<UILabel>(ref this._clsShipName);
			Mem.Del<UILabel>(ref this._clsSType);
			Mem.Del<UITexture>(ref this._uiMessageBG);
			Mem.Del<UIButtonMessage>(ref this._uiGearBtn);
			Mem.Del(ref this._uiParStar1);
			Mem.Del(ref this._uiParStar2);
			Mem.Del<Animation>(ref this._getIconAnim);
			Mem.Del<Animation>(ref this._gearAnim);
			this._clsShipMessage.UnInit();
			Mem.Del<AudioSource>(ref this._Se);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<KeyControl>(ref this._clsInput);
		}

		private void Update()
		{
			this._clsShipMessage.Update();
			if (this._isInput && this._clsInput.keyState.get_Item(1).down)
			{
				this.FadeOutExtinguish();
				this._isInput = false;
			}
		}

		public static ProdCutReceiveShip Instantiate(ProdCutReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input)
		{
			return ProdCutReceiveShip.Instantiate(prefab, parent, rewardShip, nPanelDepth, input, true);
		}

		public static ProdCutReceiveShip Instantiate(ProdCutReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input, bool needBGM)
		{
			ProdCutReceiveShip prodCutReceiveShip = Object.Instantiate<ProdCutReceiveShip>(prefab);
			prodCutReceiveShip.get_transform().set_parent(parent);
			prodCutReceiveShip.get_transform().set_localScale(Vector3.get_one());
			prodCutReceiveShip.get_transform().set_localPosition(Vector3.get_zero());
			prodCutReceiveShip.Init();
			prodCutReceiveShip._clsRewardShip = rewardShip;
			prodCutReceiveShip._setRewardShip();
			prodCutReceiveShip.GetComponent<UIPanel>().depth = nPanelDepth;
			prodCutReceiveShip._clsInput = input;
			prodCutReceiveShip._isNeedBGM = needBGM;
			prodCutReceiveShip._anim.Stop();
			return prodCutReceiveShip;
		}

		private void _setRewardShip()
		{
			this._uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this._clsRewardShip.Ship.GetGraphicsMstId(), 9);
			this._uiShip.MakePixelPerfect();
			this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._clsRewardShip.Ship.GetGraphicsMstId()).GetShipDisplayCenter(false)));
			UIButtonMessage component = this._uiGearBtn.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "ProdReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			this._clsShipMessage.Init(this._clsRewardShip.GreetingText, 0.08f, null);
			this._clsShipName.text = this._clsRewardShip.Ship.Name;
			this._clsSType.text = this._clsRewardShip.Ship.ShipTypeName;
			this._clsShipName.SetActive(false);
			this._clsSType.SetActive(false);
			this._getIconAnim.get_gameObject().SetActive(false);
			this._isBGMove = false;
		}

		public void Play(Action callback)
		{
			this._anim.Play("comp_GetShip");
			this._actCallback = callback;
			this._uiParStar1.Play();
			this._uiParStar2.Play();
			this._uiShip.alpha = 1f;
			this._uiRareBG.mainTexture = TextureFile.LoadRareBG(this._clsRewardShip.Ship.Rare);
			this._uiMessageBG.alpha = 0.75f;
			this._Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void showMessage()
		{
			this._clsShipName.SetActive(true);
			this._clsSType.SetActive(true);
			this._clsShipName.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._clsShipName.get_gameObject(), string.Empty);
			this._clsSType.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._clsSType.get_gameObject(), string.Empty);
			TrophyUtil.Unlock_GetShip(this._clsRewardShip.Ship.MstId);
		}

		private void startMessage()
		{
			this.StartGetAnim();
			this._clsShipMessage.Play();
			this.showMessage();
			ShipUtils.PlayShipVoice(this._clsRewardShip.Ship, 1);
			this._gearAnim.Stop();
			this._gearAnim.Play();
			this._isInput = true;
		}

		public void StartGetAnim()
		{
			this._getIconAnim.get_gameObject().SetActive(true);
			this._getIconAnim.Stop();
			this._getIconAnim.Play();
		}

		private void FadeOutExtinguish()
		{
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "_onFadeOutExtinguishFinished");
		}

		private void ProdReceiveShipEL(GameObject obj)
		{
			if (!this._isInput)
			{
				return;
			}
			this._isInput = false;
			this.FadeOutExtinguish();
			SoundUtils.StopSE(0.5f, new AudioSource[]
			{
				this._Se
			});
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			Object.Destroy(base.get_gameObject(), 0.1f);
		}
	}
}
