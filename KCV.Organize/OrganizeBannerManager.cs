using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeBannerManager : UIDragDropItem
	{
		private struct DragDropParams
		{
			public Vector3 defaultPos;

			public int defaultShipFrameDepth;

			public int defaultShutterPanelDepth;

			public DragDropParams(Vector3 defaultPos, int defaultFrameDepth, int defaultShutterDepth)
			{
				this.defaultPos = defaultPos;
				this.defaultShipFrameDepth = defaultFrameDepth;
				this.defaultShutterPanelDepth = defaultShutterDepth;
			}
		}

		private const float SHUTTER_OPEN_DURATION_SEC = 0.3f;

		private const float SHUTTER_OPEN_DELAY_SEC = 0.2f;

		private const float SHUTTER_CLOSE_DURATION_SEC = 0.3f;

		private const float SCALE_ANIMATION_DURATION_SEC = 0.15f;

		private static readonly Vector2 SHUTTER_OPEN_POS_L = new Vector2(-290f, 3f);

		private static readonly Vector2 SHUTTER_OPEN_POS_R = new Vector2(290f, 3f);

		private static readonly Vector2 SHUTTER_CLOSE_POS_L = new Vector2(-83f, 3f);

		private static readonly Vector2 SHUTTER_CLOSE_POS_R = new Vector2(78f, 3f);

		[SerializeField]
		private GameObject _shipFrame;

		[SerializeField]
		private UIPanel _shutterPanel;

		[SerializeField]
		private UIPanel _bannerPanel;

		[SerializeField]
		private UISprite _gauge;

		[SerializeField]
		private UILabel _labelNumber;

		[SerializeField]
		private UILabel _labelLevel;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UILabel _labelTaikyu;

		[SerializeField]
		private GameObject _starManager;

		[SerializeField]
		private GameObject _shutterObj;

		[SerializeField]
		private UISprite _shutterL;

		[SerializeField]
		private UISprite _shutterR;

		[SerializeField]
		private UISprite _shutterLShadow;

		[SerializeField]
		private UISprite _shutterRShadow;

		[SerializeField]
		private Animation _animation;

		[SerializeField]
		public CommonShipBanner shipBanner;

		[SerializeField]
		public UISprite _frame;

		private int gaugeMaxWidth;

		private bool isShow;

		private Action _callback;

		private Action _actOnDragDropEnd;

		private ShipModel _ship;

		private OrganizeBannerManager.DragDropParams _strDragDropParams;

		private Predicate<OrganizeBannerManager> _preOnCheckDragDropTarget;

		private Action<OrganizeBannerManager> _actOnDragDropStart;

		private Predicate<OrganizeBannerManager> _preOnDragDropRelease;

		public bool IsSet;

		public bool _isDeckChange;

		public UiStarManager StarManager;

		private bool isCloseing;

		public int number
		{
			get;
			private set;
		}

		public ShipModel ship
		{
			get
			{
				return this._ship;
			}
		}

		public ShipBannerDragDrop bannerDragDrop
		{
			get;
			private set;
		}

		private void Awake()
		{
			if (this._shipFrame == null)
			{
				this._shipFrame = base.get_transform().FindChild("ShipFrame").get_gameObject();
			}
			Util.FindParentToChild<CommonShipBanner>(ref this.shipBanner, this._shipFrame.get_transform(), "CommonShipBanner2");
			Util.FindParentToChild<UIPanel>(ref this._bannerPanel, base.get_transform(), "ShipFrame");
			Util.FindParentToChild<UIPanel>(ref this._shutterPanel, base.get_transform(), "ShutterPanel");
			Util.FindParentToChild<UISprite>(ref this._frame, this._shipFrame.get_transform(), "Frame");
			Util.FindParentToChild<UISprite>(ref this._gauge, this._shipFrame.get_transform(), "Gauge");
			Util.FindParentToChild<UILabel>(ref this._labelNumber, this._shipFrame.get_transform(), "LabelNumber");
			Util.FindParentToChild<UILabel>(ref this._labelLevel, this._shipFrame.get_transform(), "LabelLevel");
			Util.FindParentToChild<UILabel>(ref this._labelName, this._shipFrame.get_transform(), "LabelName");
			Util.FindParentToChild<UILabel>(ref this._labelTaikyu, this._shipFrame.get_transform(), "LabelTaikyu");
			if (this._shutterObj == null)
			{
				this._shutterObj = this._shutterPanel.get_transform().FindChild("ShutterObj").get_gameObject();
			}
			Util.FindParentToChild<UISprite>(ref this._shutterL, this._shutterPanel.get_transform(), "ShutterObj/ShutterL");
			Util.FindParentToChild<UISprite>(ref this._shutterR, this._shutterPanel.get_transform(), "ShutterObj/ShutterR");
			Util.FindParentToChild<UISprite>(ref this._shutterLShadow, this._shutterL.get_transform(), "ShutterL_Shadow");
			Util.FindParentToChild<UISprite>(ref this._shutterRShadow, this._shutterR.get_transform(), "ShutterR_Shadow");
			Util.FindParentToChild<UiStarManager>(ref this.StarManager, this._shipFrame.get_transform(), "StarManager");
			this.bannerDragDrop = base.GetComponent<ShipBannerDragDrop>();
			this._strDragDropParams = new OrganizeBannerManager.DragDropParams(Vector3.get_zero(), this._bannerPanel.depth, this._shutterPanel.depth);
		}

		private void OnDestroy()
		{
			Mem.Del<GameObject>(ref this._shipFrame);
			Mem.Del<UIPanel>(ref this._shutterPanel);
			Mem.Del<UIPanel>(ref this._bannerPanel);
			Mem.Del<CommonShipBanner>(ref this.shipBanner);
			Mem.Del(ref this._frame);
			Mem.Del(ref this._gauge);
			Mem.Del<UILabel>(ref this._labelNumber);
			Mem.Del<UILabel>(ref this._labelLevel);
			Mem.Del<UILabel>(ref this._labelName);
			Mem.Del<UILabel>(ref this._labelTaikyu);
			Mem.Del<GameObject>(ref this._starManager);
			Mem.Del<GameObject>(ref this._shutterObj);
			Mem.Del(ref this._shutterL);
			Mem.Del(ref this._shutterR);
			Mem.Del(ref this._shutterLShadow);
			Mem.Del(ref this._shutterRShadow);
			Mem.Del<Animation>(ref this._animation);
			Mem.Del<Action>(ref this._callback);
			Mem.Del<ShipModel>(ref this._ship);
			Mem.Del<UiStarManager>(ref this.StarManager);
			Mem.Del<Predicate<OrganizeBannerManager>>(ref this._preOnCheckDragDropTarget);
			Mem.Del<Action<OrganizeBannerManager>>(ref this._actOnDragDropStart);
			Mem.Del<Action>(ref this._actOnDragDropEnd);
		}

		public void init(int number, Predicate<OrganizeBannerManager> onCheckDragDropTarget, Action<OrganizeBannerManager> onDragDropStart, Predicate<OrganizeBannerManager> onDragDropRelease, Action onDragDropEnd, bool isInitPos = true)
		{
			this._preOnCheckDragDropTarget = onCheckDragDropTarget;
			this._actOnDragDropStart = onDragDropStart;
			this._preOnDragDropRelease = onDragDropRelease;
			this._actOnDragDropEnd = onDragDropEnd;
			this.IsSet = false;
			this.number = number;
			this._isDeckChange = false;
			this._callback = null;
			if (isInitPos)
			{
				int num = 0;
				if (this.number == 1 || this.number == 2)
				{
					num = 0;
				}
				else if (this.number == 3 || this.number == 4)
				{
					num = 1;
				}
				else if (this.number == 5 || this.number == 6)
				{
					num = 2;
				}
				base.get_gameObject().get_transform().set_localPosition((this.number % 2 != 1) ? new Vector3(680f, 66f - (float)(124 * num)) : new Vector3(-670f, 66f - (float)(124 * num)));
			}
			if (this._animation == null)
			{
				this._animation = base.get_gameObject().GetComponent<Animation>();
			}
			this._animation.Stop();
			UIButtonMessage component = base.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "DetailEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			this.InitBanner(false);
			this._labelNumber.textInt = number;
			UISprite component2 = this._shipFrame.get_transform().FindChild("GaugeFrame").GetComponent<UISprite>();
			this.gaugeMaxWidth = component2.width;
		}

		public void InitBanner(bool closeAnimation)
		{
			if (this.IsSetShip())
			{
				this.shipBanner.StopParticle();
			}
			this.IsSet = false;
			this._callback = null;
			this.shipBanner.get_transform().get_gameObject().SetActive(false);
			this._labelNumber.alpha = 0f;
			this._gauge.alpha = 0f;
			this._labelName.alpha = 0f;
			this._labelLevel.alpha = 0f;
			this._labelTaikyu.alpha = 0f;
			this._shutterL.alpha = 1f;
			this._shutterR.alpha = 1f;
			this.CloseBanner(closeAnimation);
			if (!closeAnimation)
			{
				this.SetShipFrameActive(false);
			}
		}

		public void InitChangeBanner(bool closeAnimation)
		{
			if (this.IsSetShip())
			{
				this.shipBanner.StopParticle();
			}
			this.IsSet = false;
			this.shipBanner.get_transform().get_gameObject().SetActive(false);
			this._labelNumber.alpha = 0f;
			this._gauge.alpha = 0f;
			this._labelName.alpha = 0f;
			this._labelLevel.alpha = 0f;
			this._labelTaikyu.alpha = 0f;
			this._shutterL.alpha = 1f;
			this._shutterR.alpha = 1f;
			if (!closeAnimation)
			{
				this.CloseBanner(closeAnimation);
				this.SetShipFrameActive(false);
			}
		}

		public void SetShipFrameActive(bool active)
		{
			this._shipFrame.SetActive(active);
			this._shutterLShadow.SetActive(active);
			this._shutterRShadow.SetActive(active);
		}

		public void CloseBanner(bool animation)
		{
			if (this.isCloseing)
			{
				return;
			}
			if (animation)
			{
				this.isCloseing = true;
				this._shutterPanel.alpha = 1f;
				this.MoveTo(this._shutterL.get_gameObject(), OrganizeBannerManager.SHUTTER_CLOSE_POS_L, 0.3f, 0f, "compCloseAnimation");
				this.MoveTo(this._shutterR.get_gameObject(), OrganizeBannerManager.SHUTTER_CLOSE_POS_R, 0.3f);
			}
			else
			{
				this._shutterPanel.alpha = 1f;
				this._shutterL.get_transform().set_localPosition(OrganizeBannerManager.SHUTTER_CLOSE_POS_L);
				this._shutterR.get_transform().set_localPosition(OrganizeBannerManager.SHUTTER_CLOSE_POS_R);
			}
		}

		public void OpenBanner(bool animation)
		{
			this.isCloseing = false;
			if (animation)
			{
				this.shipBanner.StopParticle();
				this.MoveTo(this._shutterL.get_gameObject(), OrganizeBannerManager.SHUTTER_OPEN_POS_L, 0.3f, 0.2f, "compOpenAnimation");
				this.MoveTo(this._shutterR.get_gameObject(), OrganizeBannerManager.SHUTTER_OPEN_POS_R, 0.3f, 0.2f, string.Empty);
			}
			else
			{
				this._shutterL.get_transform().set_localPosition(OrganizeBannerManager.SHUTTER_OPEN_POS_L);
				this._shutterR.get_transform().set_localPosition(OrganizeBannerManager.SHUTTER_OPEN_POS_R);
				this._shutterPanel.alpha = 0f;
			}
		}

		public void setBanner(ShipModel ship, bool openAnimation, Action callback, bool isShutterHide = false)
		{
			this.IsSet = true;
			this._ship = ship;
			this._callback = callback;
			this.updateBannerWhenShipExist(openAnimation, isShutterHide);
		}

		public void ChangeBanner(ShipModel ship)
		{
			this.IsSet = true;
			this._ship = ship;
			this.updateBannerWhenShipExist(false, false);
			this.OpenBanner(false);
		}

		public void setShip(ShipModel ship)
		{
			this._ship = ship;
		}

		public void updateBannerWhenShipExist(bool openAnimation, bool isShutterHide = false)
		{
			if (!this.IsSet)
			{
				this._shipFrame.SetActive(false);
				return;
			}
			this.SetShipFrameActive(true);
			this.StarManager.init(this._ship.Srate);
			this.shipBanner.get_transform().get_gameObject().SetActive(true);
			this.shipBanner.SetShipData(this._ship);
			this._labelNumber.alpha = 1f;
			this._gauge.alpha = 1f;
			this.SetHpGauge();
			this._labelLevel.alpha = 1f;
			this._labelLevel.textInt = this._ship.Level;
			this._labelName.alpha = 1f;
			this._labelName.text = this._ship.Name;
			this._labelName.color = ((!this._ship.IsMarriage()) ? new Color(0f, 0f, 0f) : new Color(1f, 0.7f, 0f));
			this._labelName.effectColor = ((!this._ship.IsMarriage()) ? new Color(1f, 1f, 1f) : new Color(0f, 0f, 0f));
			this._labelTaikyu.alpha = 1f;
			this._labelTaikyu.text = this._ship.NowHp + "/" + this._ship.MaxHp;
			if (openAnimation)
			{
				this.CloseBanner(false);
				this.OpenBanner(!isShutterHide);
			}
		}

		public bool IsSetShip()
		{
			return this.IsSet;
		}

		private void SetHpGauge()
		{
			float num = (float)this._ship.NowHp / (float)this._ship.MaxHp;
			float num2 = (float)this.gaugeMaxWidth * num;
			this._gauge.width = (int)num2;
			this._gauge.alpha = 1f;
			this._gauge.color = Util.HpGaugeColor2(this._ship.MaxHp, this._ship.NowHp);
		}

		public bool CheckBtnEnabled()
		{
			return !OrganizeTaskManager.Instance.GetDetailTask().isEnabled && !OrganizeTaskManager.Instance.GetListTask().isEnabled && !OrganizeTaskManager.Instance.GetTopTask().isTenderAnimation();
		}

		public void UpdateBanner(bool enabled)
		{
			if (enabled)
			{
				UISelectedObject.SelectedOneObjectBlink(this._frame.get_gameObject(), true);
				this._shutterPanel.baseClipRegion = new Vector4(-2.5f, 4f, 345f, 122f);
				if (this._shipFrame.get_activeSelf())
				{
					this.setScaleAnimation(this._shipFrame.get_gameObject(), new Vector3(1f, 1f, 1f), 0.15f, 0f);
				}
				this.setScaleAnimation(this._shutterObj.get_gameObject(), new Vector3(1f, 1f, 1f), 0.15f, 0f);
				if (!this.IsSet)
				{
					this.CloseBanner(!this._isDeckChange);
					UISelectedObject.SelectedOneObjectBlink(this._shutterR.get_gameObject(), true);
					UISelectedObject.SelectedOneObjectBlink(this._shutterL.get_gameObject(), true);
				}
			}
			else
			{
				this._frame.color = new Color(1f, 1f, 1f);
				UISelectedObject.SelectedOneObjectBlink(this._frame.get_gameObject(), false);
				UISelectedObject.SelectedOneObjectBlink(this._shutterR.get_gameObject(), false);
				UISelectedObject.SelectedOneObjectBlink(this._shutterL.get_gameObject(), false);
				if (this._shipFrame.get_activeSelf())
				{
					this.setScaleAnimation(this._shipFrame.get_gameObject(), new Vector3(0.9f, 0.9f, 0.9f), 0.15f, 0f);
				}
				this.setScaleAnimation(this._shutterObj.get_gameObject(), new Vector3(0.9f, 0.9f, 0.9f), 0.15f, 0f);
				if (!this.IsSet)
				{
					this.CloseBanner(!this._isDeckChange);
				}
			}
		}

		public void setScaleAnimation(GameObject obj, Vector3 _scale, float _time, float _delay)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("scale", _scale);
			hashtable.Add("time", _time);
			hashtable.Add("delay", _delay);
			hashtable.Add("isLocal", true);
			hashtable.Add("easeType", iTween.EaseType.linear);
			obj.ScaleTo(hashtable);
		}

		public void CompUpdateMove()
		{
			this._shutterPanel.baseClipRegion = new Vector4(-2.5f, 4f, 340f, 120f);
		}

		public void UpdateBannerFatigue()
		{
			this.shipBanner.SetShipData(this._ship);
		}

		public void UpdateChangeBanner(bool enabled)
		{
			this._frame.color = new Color(1f, 1f, 1f);
			UIWidget[] componentsInChildren = base.get_transform().GetComponentsInChildren<UIWidget>();
			UIWidget[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UIWidget uIWidget = array[i];
				uIWidget.depth = uIWidget.depth;
			}
			base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
		}

		public void Show(int num)
		{
			float num4;
			float num5;
			if (num % 2 == 0)
			{
				if (num == 0)
				{
					float num2 = (this._frame.localSize.x * 1f - this._frame.localSize.x) * 0.5f;
					float num3 = this._frame.localSize.y * 1f - this._frame.localSize.y;
					num4 = -71f - num2 + 1.5f;
					num5 = 69f - num3 + 1.5f;
				}
				else
				{
					int num6 = num / 2;
					num4 = -71f;
					num5 = 69f - (float)num6 * 124f;
				}
			}
			else
			{
				int num7 = num / 2;
				num4 = 270f;
				num5 = 69f - (float)num7 * 124f;
			}
			this.MoveTo(base.get_gameObject(), new Vector3(num4, num5, 0f), 0.3f, 0f, "showSelect");
			if (this.bannerDragDrop != null)
			{
				this.bannerDragDrop.setDefaultPosition(new Vector2(num4, num5));
			}
			this.isShow = true;
		}

		private void showSelect()
		{
			if (this.number == 1)
			{
				this.UpdateBanner(true);
			}
			this._strDragDropParams.defaultPos = base.get_transform().get_localPosition();
		}

		public void DeckChangeAnimetion(bool isLeft)
		{
			if (this.IsSetShip())
			{
				this.shipBanner.StopParticle();
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				if (i % 2 == 0 && i != 0)
				{
					num++;
				}
				if (i == this.number - 1)
				{
					break;
				}
			}
			if (this._isDeckChange)
			{
				OrganizeTaskManager.Instance.GetTopTask().UpdateChangeBanner(this.number);
				if (!this.IsSet)
				{
					this.CloseBanner(false);
				}
				else
				{
					this.OpenBanner(false);
				}
			}
			string text = (!isLeft) ? ("OutBannerRight" + (num + 1)) : ("OutBannerLeft" + (num + 1));
			this._animation.Stop();
			this._animation.Play(text);
			this._isDeckChange = true;
		}

		public void CompChangeRightAnimate()
		{
			OrganizeTaskManager.Instance.GetTopTask().ChangeDeckAnimate(this.number);
			this._animation.Stop();
			this._animation.Play("InBannerLeft");
			if (!this.IsSet)
			{
				this.CloseBanner(false);
			}
			else
			{
				this.OpenBanner(false);
			}
			this._isDeckChange = false;
		}

		public void CompChangeLeftAnimate()
		{
			OrganizeTaskManager.Instance.GetTopTask().ChangeDeckAnimate(this.number);
			this._animation.Stop();
			this._animation.Play("InBannerRight");
			if (!this.IsSet)
			{
				this.CloseBanner(false);
			}
			else
			{
				this.OpenBanner(false);
			}
			this._isDeckChange = false;
		}

		public void EndChangeAnimate()
		{
			if (this.number == 0)
			{
				this.UpdateBanner(true);
			}
		}

		private void compOpenAnimation()
		{
			this._shutterPanel.alpha = 0f;
			if (this._callback != null)
			{
				this._callback.Invoke();
			}
			this.shipBanner.StartParticle();
		}

		private void compCloseAnimation()
		{
			this.isCloseing = false;
			if (this._callback != null)
			{
				this._callback.Invoke();
			}
		}

		private void MoveTo(GameObject o, Vector3 to, float duration)
		{
			this.MoveTo(o, to, duration, 0f, string.Empty);
		}

		private void MoveTo(GameObject obj, Vector3 to, float duration, float delay, string comp)
		{
			iTween.Stop(obj);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", to);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", delay);
			hashtable.Add("time", duration);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", comp);
			hashtable.Add("oncompletetarget", base.get_gameObject());
			obj.MoveTo(hashtable);
		}

		public void DetailEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetListTask().isRun)
			{
				return;
			}
			if (this.CheckBtnEnabled())
			{
				TaskOrganizeTop.BannerIndex = this.number;
				if (this.IsSet)
				{
					OrganizeTaskManager.Instance.GetTopTask().setChangePhase("detail");
					OrganizeTaskManager.Instance.GetDetailTask().Show(this._ship);
					if (this._ship != null)
					{
						ShipUtils.PlayShipVoice(this._ship, App.rand.Next(2, 4));
					}
				}
				else
				{
					OrganizeTaskManager.Instance.GetTopTask().setChangePhase("list");
					OrganizeTaskManager.Instance.GetListTask().Show(this.IsSet);
				}
				SoundUtils.PlaySE(SEFIleInfos.SE_002);
				OrganizeTaskManager.Instance.GetTopTask().UpdateAllSelectBanner();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
		}

		protected override void OnDragStart()
		{
			if (!this.IsSet)
			{
				return;
			}
			if (this._ship.IsInActionEndDeck())
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(IsGoCondition.ActionEndDeck));
				return;
			}
			if (this._ship.IsTettaiBling())
			{
				return;
			}
			if (!this._preOnCheckDragDropTarget.Invoke(this))
			{
				return;
			}
			Dlg.Call<OrganizeBannerManager>(ref this._actOnDragDropStart, this);
			this._bannerPanel.depth += 5;
			this._shutterPanel.depth += 5;
			OrganizeTaskManager._clsTop.deckSwitchManager.keyControlEnable = false;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			base.OnDragStart();
		}

		protected override void OnDragDropRelease(GameObject surface)
		{
			if (surface != null)
			{
				OrganizeBannerManager component = surface.GetComponent<OrganizeBannerManager>();
				if (component != null && component.IsSet)
				{
					ShipModel ship = component.ship;
					if (!this._preOnDragDropRelease.Invoke(component))
					{
						Dlg.Call(ref this._actOnDragDropEnd);
					}
				}
				else
				{
					Dlg.Call(ref this._actOnDragDropEnd);
				}
			}
			else
			{
				Dlg.Call(ref this._actOnDragDropEnd);
			}
			OrganizeTaskManager._clsTop.deckSwitchManager.keyControlEnable = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			base.OnDragDropRelease(surface);
		}

		protected override void OnDragDropEnd()
		{
			this.mTrans.set_localPosition(this._strDragDropParams.defaultPos);
			this._bannerPanel.depth = this._strDragDropParams.defaultShipFrameDepth;
			this._shutterPanel.depth = this._strDragDropParams.defaultShutterPanelDepth;
		}
	}
}
