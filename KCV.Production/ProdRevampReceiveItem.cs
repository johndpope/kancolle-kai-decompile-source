using KCV.Scene.Port;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Production
{
	public class ProdRevampReceiveItem : MonoBehaviour
	{
		public static Random rand = new Random((int)DateTime.get_Now().get_Ticks() & 65535);

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem1;

		[SerializeField]
		private UITexture _uiItem2;

		[SerializeField]
		private UITexture _uiMessageBG;

		[SerializeField]
		private Animation _getIconAnim;

		[SerializeField]
		private UISprite _uiGearBtn;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private Animation _anim;

		[SerializeField]
		private UIWidget _widgetJukurenParent;

		private Generics.Message _clsShipMessage;

		private SlotitemModel_Mst mSlotItemFrom;

		private SlotitemModel_Mst mSlotItemTo;

		private int _index;

		private Action _actCallback;

		private bool _isFinished;

		private bool _isInput;

		private bool _isNeedBGM;

		private bool _isBGMove;

		private bool _isUpdateMessage;

		private bool _isUpdateNextBtn;

		private KeyControl _clsInput;

		private bool _isUseJukuren;

		public bool IsFinished
		{
			get
			{
				return this._isFinished;
			}
		}

		private void Awake()
		{
			this._anim = base.GetComponent<Animation>();
			Util.FindParentToChild<UITexture>(ref this._uiRareBG, base.get_transform(), "RareBG");
			Util.FindParentToChild<UITexture>(ref this._uiItem1, base.get_transform(), "Item1");
			Util.FindParentToChild<UITexture>(ref this._uiItem2, base.get_transform(), "Item2");
			Util.FindParentToChild<UITexture>(ref this._uiMessageBG, base.get_transform(), "MessageWindow/MessageBG");
			Util.FindParentToChild<Animation>(ref this._getIconAnim, base.get_transform(), "MessageWindow/Get");
			Util.FindParentToChild<UISprite>(ref this._uiGearBtn, base.get_transform(), "MessageWindow/NextBtn");
			Util.FindParentToChild<UISprite>(ref this._uiGear, base.get_transform(), "MessageWindow/NextBtn/Gear");
			this._clsShipMessage = new Generics.Message(base.get_transform(), "MessageWindow/MessageBG/ShipMessage");
			this._uiItem1.alpha = 0f;
			this._uiItem2.alpha = 0f;
			this._uiItem1.depth = 4;
			this._uiItem2.depth = 3;
			this._isUpdateMessage = false;
			this._isFinished = false;
		}

		private void OnDestroy()
		{
			this._anim = null;
			this._uiRareBG = null;
			this._uiItem1 = null;
			this._clsShipMessage.UnInit();
			this._actCallback = null;
			this._isFinished = false;
			this._clsInput = null;
		}

		private void Update()
		{
			this._clsShipMessage.Update();
			if (this._isUpdateNextBtn)
			{
				this._uiGear.get_transform().Rotate(-50f * Time.get_deltaTime() * Vector3.get_forward());
			}
			if (this._isInput && this._clsInput != null)
			{
				if (this._clsInput != null)
				{
					this._clsInput.Update();
				}
				if (this._clsInput.keyState.get_Item(1).down)
				{
					this.FadeOutExtinguish();
					this._isInput = false;
				}
			}
		}

		public static ProdRevampReceiveItem Instantiate(ProdRevampReceiveItem prefab, Transform parent, SlotitemModel_Mst from, SlotitemModel_Mst to, int nPanelDepth, bool useJukuren, KeyControl input)
		{
			ProdRevampReceiveItem component = NGUITools.AddChild(parent.get_gameObject(), prefab.get_gameObject()).GetComponent<ProdRevampReceiveItem>();
			component.mSlotItemFrom = from;
			component.mSlotItemTo = to;
			component._isUseJukuren = useJukuren;
			component._setRewardItem();
			component._uiRareBG.alpha = 0f;
			component._uiRareBG.mainTexture = TextureFile.LoadRareBG(1);
			component.GetComponent<UIPanel>().depth = nPanelDepth;
			component._clsInput = input;
			component.SetJukuren(useJukuren);
			return component;
		}

		public static ProdRevampReceiveItem Instantiate(ProdRevampReceiveItem prefab, Transform parent, IReward_Slotitem from, IReward_Slotitem to, int nPanelDepth, bool useJukuren, KeyControl input)
		{
			return ProdRevampReceiveItem.Instantiate(prefab, parent, new SlotitemModel_Mst(from.Id), new SlotitemModel_Mst(to.Id), nPanelDepth, useJukuren, input);
		}

		private void SetJukuren(bool useJukuren)
		{
			this._widgetJukurenParent.alpha = (float)((!useJukuren) ? 0 : 1);
		}

		private void _setRewardItem()
		{
			try
			{
				this._uiItem1.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this.mSlotItemFrom.MstId, 1);
				this._uiItem1.MakePixelPerfect();
				this._uiItem2.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this.mSlotItemTo.MstId, 1);
				this._uiItem2.MakePixelPerfect();
			}
			catch (NullReferenceException)
			{
			}
			this._getIconAnim.get_gameObject().SetActive(false);
			this._uiGearBtn.alpha = 0f;
			this._isUpdateNextBtn = false;
		}

		public void Play(Action callback)
		{
			this._actCallback = callback;
			this._uiRareBG.alpha = 1f;
			this._uiItem1.alpha = 1f;
			this._uiItem2.alpha = 1f;
			this._anim.Stop();
			this._anim.Play("startRevampGetItem");
		}

		private void _startMessage()
		{
			if (this._isUseJukuren)
			{
				this._clsShipMessage.Init("部隊再編中…", 0.04f, null);
			}
			else
			{
				this._clsShipMessage.Init("装備改修中…", 0.04f, null);
			}
			this._clsShipMessage.Play();
		}

		private void _cmpAnimation()
		{
			string message = string.Empty;
			if (this._isUseJukuren)
			{
				message = string.Format("{0}に部隊再編完了！", this.mSlotItemTo.Name);
			}
			else
			{
				message = string.Format("{0}に装備が改修更新されました！", this.mSlotItemTo.Name);
			}
			this._clsShipMessage.Init(message, 0.04f, null);
			this._clsShipMessage.Play();
		}

		private void _changeDepth()
		{
			this._uiItem1.depth = 3;
			this._uiItem2.depth = 4;
		}

		private void strtMessageBox()
		{
			this._uiMessageBG.alpha = 1f;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.5f);
			hashtable.Add("y", -187f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			this._uiMessageBG.get_transform().MoveTo(hashtable);
		}

		private void strtGearIcon()
		{
			this._uiGear.alpha = 1f;
			this._isUpdateNextBtn = true;
			this._clsShipMessage.Play();
			this._isInput = true;
		}

		private void strtGetIcon()
		{
		}

		private void FadeOutExtinguish()
		{
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			this._isFinished = true;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this.Discard();
		}

		private void Discard()
		{
			Object.Destroy(base.get_gameObject(), 0.1f);
		}

		private void ProdReceiveShipEL(GameObject obj)
		{
			if (!this._isInput)
			{
				return;
			}
			this._isInput = false;
			this.FadeOutExtinguish();
		}

		private void OnDestriy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiRareBG, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiItem1, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiItem2, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiMessageBG, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiGearBtn);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uiGear);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._widgetJukurenParent);
			if (this._getIconAnim != null && this._getIconAnim.get_isPlaying())
			{
				this._getIconAnim.Stop();
			}
			this._getIconAnim = null;
			if (this._anim != null && this._anim.get_isPlaying())
			{
				this._anim.Stop();
			}
			this._anim = null;
		}
	}
}
