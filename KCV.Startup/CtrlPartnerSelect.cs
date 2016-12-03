using KCV.Display;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlPartnerSelect : MonoBehaviour
	{
		public enum ButtonIndex
		{
			L,
			R,
			Back,
			Deside
		}

		public enum ShipPartsIndex
		{
			Girl,
			Background,
			Info
		}

		private Dictionary<int, Vector2> _shipLocate;

		private UIPanel _uiPanel;

		private List<List<ShipModelMst>> _listStarterShips;

		private Dictionary<CtrlPartnerSelect.ButtonIndex, UIButton> _dicButtons;

		private Dictionary<CtrlPartnerSelect.ShipPartsIndex, UITexture> _dicShipParts;

		private CtrlStarterSelect.StarterType _iStarterType;

		private int _nSelectedId;

		private Action _actOnCancel;

		private Action<ShipModelMst> _actOnDecidePartnerShip;

		private bool _isMove;

		private bool _isDecide;

		private UIDisplaySwipeEventRegion _clsSwipeEvent;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public Transform partnerShip
		{
			get
			{
				return this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform();
			}
		}

		public CtrlPartnerSelect()
		{
			Dictionary<int, Vector2> dictionary = new Dictionary<int, Vector2>();
			dictionary.Add(54, new Vector2(247f, -190f));
			dictionary.Add(55, new Vector2(284f, -185f));
			dictionary.Add(56, new Vector2(314f, -202f));
			dictionary.Add(9, new Vector2(250f, -177f));
			dictionary.Add(33, new Vector2(180f, -135f));
			dictionary.Add(37, new Vector2(243f, -134f));
			dictionary.Add(46, new Vector2(248f, -228f));
			dictionary.Add(94, new Vector2(324f, -190f));
			dictionary.Add(1, new Vector2(312f, -160f));
			dictionary.Add(43, new Vector2(327f, -160f));
			dictionary.Add(96, new Vector2(280f, -206f));
			this._shipLocate = dictionary;
			base..ctor();
		}

		public static CtrlPartnerSelect Instantiate(CtrlPartnerSelect prefab, Transform parent)
		{
			return Object.Instantiate<CtrlPartnerSelect>(prefab);
		}

		private void Awake()
		{
			this._isMove = false;
			this._dicButtons = new Dictionary<CtrlPartnerSelect.ButtonIndex, UIButton>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(CtrlPartnerSelect.ButtonIndex)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CtrlPartnerSelect.ButtonIndex buttonIndex = (CtrlPartnerSelect.ButtonIndex)((int)enumerator.get_Current());
					this._dicButtons.Add(buttonIndex, base.get_transform().FindChild("Button_" + buttonIndex.ToString()).GetComponent<UIButton>());
					this._dicButtons.get_Item(buttonIndex).onClick = Util.CreateEventDelegateList(this, "press_Button", buttonIndex);
				}
			}
			this._dicShipParts = new Dictionary<CtrlPartnerSelect.ShipPartsIndex, UITexture>();
			using (IEnumerator enumerator2 = Enum.GetValues(typeof(CtrlPartnerSelect.ShipPartsIndex)).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CtrlPartnerSelect.ShipPartsIndex shipPartsIndex = (CtrlPartnerSelect.ShipPartsIndex)((int)enumerator2.get_Current());
					this._dicShipParts.Add(shipPartsIndex, base.get_transform().FindChild("Ship_" + shipPartsIndex.ToString()).GetComponent<UITexture>());
				}
			}
			this._iStarterType = CtrlStarterSelect.StarterType.Ex;
			this._nSelectedId = 0;
			this._clsSwipeEvent = GameObject.Find("EventArea").GetComponent<UIDisplaySwipeEventRegion>();
			this._clsSwipeEvent.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.OnSwipe));
			this._clsSwipeEvent.SetEventCatchCamera(StartupTaskManager.GetPSVitaMovie().GetComponent<Camera>());
			this._isDecide = false;
			this.panel.widgetsAreStatic = true;
		}

		private void Start()
		{
			this._listStarterShips = new List<List<ShipModelMst>>();
			using (List<List<int>>.Enumerator enumerator = Defines.STARTER_PARTNER_SHIPS_ID.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<int> current = enumerator.get_Current();
					List<ShipModelMst> starterShips = new List<ShipModelMst>();
					current.ForEach(delegate(int x)
					{
						starterShips.Add(new ShipModelMst(x));
					});
					this._listStarterShips.Add(starterShips);
				}
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelListSafe<List<ShipModelMst>>(ref this._listStarterShips);
			Mem.DelDictionarySafe<CtrlPartnerSelect.ButtonIndex, UIButton>(ref this._dicButtons);
			Mem.DelDictionarySafe<CtrlPartnerSelect.ShipPartsIndex, UITexture>(ref this._dicShipParts);
			Mem.Del<CtrlStarterSelect.StarterType>(ref this._iStarterType);
			Mem.Del<int>(ref this._nSelectedId);
			Mem.Del<Action>(ref this._actOnCancel);
			Mem.Del<Action<ShipModelMst>>(ref this._actOnDecidePartnerShip);
			Mem.Del<UIDisplaySwipeEventRegion>(ref this._clsSwipeEvent);
			base.get_transform().GetComponentsInChildren<UIWidget>().ForEach(delegate(UIWidget x)
			{
				if (x is UISprite)
				{
					((UISprite)x).Clear();
				}
				Mem.Del<UIWidget>(ref x);
			});
		}

		public bool Init(Action<ShipModelMst> onDecidePartnerShip, Action onCancel)
		{
			this._actOnCancel = onCancel;
			this._actOnDecidePartnerShip = onDecidePartnerShip;
			this.panel.widgetsAreStatic = false;
			base.get_transform().localScaleOne();
			this.ChangePartnerShip(0);
			this._isDecide = false;
			using (IEnumerator enumerator = base.get_transform().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					if (!transform.get_gameObject().get_activeInHierarchy())
					{
						transform.SetActive(true);
					}
				}
			}
			return true;
		}

		public void SetStarter(CtrlStarterSelect.StarterType iType)
		{
			this._iStarterType = iType;
			this._nSelectedId = 0;
		}

		public ShipModelMst getSelectedShip()
		{
			return this._listStarterShips.get_Item((int)this._iStarterType).get_Item(this._nSelectedId);
		}

		public void PreparaNext(bool isFoward)
		{
			if (this._isMove)
			{
				return;
			}
			this._nSelectedId = Mathe.NextElementRev(this._nSelectedId, 0, this._listStarterShips.get_Item((int)this._iStarterType).get_Count() - 1, isFoward);
			this.ChangePartnerShip(this._nSelectedId);
		}

		public void press_Button(CtrlPartnerSelect.ButtonIndex iIndex)
		{
			if (this._isMove)
			{
				return;
			}
			switch (iIndex)
			{
			case CtrlPartnerSelect.ButtonIndex.L:
				this.PreparaNext(false);
				break;
			case CtrlPartnerSelect.ButtonIndex.R:
				this.PreparaNext(true);
				break;
			case CtrlPartnerSelect.ButtonIndex.Deside:
				this.OnDecidePartnerShip();
				break;
			}
		}

		public void Show()
		{
			this.panel.widgetsAreStatic = false;
			base.get_transform().localScaleOne();
		}

		public void Hide()
		{
			using (IEnumerator enumerator = base.get_transform().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					if (!(transform == this.partnerShip.get_transform()))
					{
						transform.SetActive(false);
					}
				}
			}
		}

		public bool OnCancel()
		{
			if (this._isMove)
			{
				return false;
			}
			Dlg.Call(ref this._actOnCancel);
			return true;
		}

		public bool OnDecidePartnerShip()
		{
			if (this._isMove)
			{
				return false;
			}
			if (this._isDecide)
			{
				return false;
			}
			this._isDecide = true;
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().LTCancel();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			Dlg.Call<ShipModelMst>(ref this._actOnDecidePartnerShip, this.getSelectedShip());
			return true;
		}

		private void OnSwipe(UIDisplaySwipeEventRegion.ActionType iType, float dX, float dY, float mpX, float mpY, float et)
		{
			if (this._isMove)
			{
				return;
			}
			if (iType == UIDisplaySwipeEventRegion.ActionType.Moving)
			{
				if (mpX >= 0.15f)
				{
					this.PreparaNext(true);
				}
				else if (mpX <= -0.15f)
				{
					this.PreparaNext(false);
				}
			}
		}

		private void ChangePartnerShip(int selectedShipCursor)
		{
			string text = string.Format("{0:000}", this._listStarterShips.get_Item((int)this._iStarterType).get_Item(selectedShipCursor).GetGraphicsMstId());
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Background).mainTexture = (Resources.Load("Textures/Startup/PartnerShip/startup_c" + text + "_txtArea") as Texture);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Background).localSize = new Vector2(740f, 382f);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().localPositionX(this._shipLocate.get_Item(this._listStarterShips.get_Item((int)this._iStarterType).get_Item(selectedShipCursor).MstId).x);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().localPositionY(this._shipLocate.get_Item(this._listStarterShips.get_Item((int)this._iStarterType).get_Item(selectedShipCursor).MstId).y);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().LTCancel();
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().localScaleOne();
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().LTScale(Vector3.get_one() * 1.05f, 5f).setEase(LeanTweenType.easeOutSine).setLoopPingPong();
			Vector3 localPosition = this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().get_localPosition();
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().localPositionX(1000f);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().LTMoveLocal(localPosition, 0.2f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate
			{
				this._isMove = false;
			});
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this._listStarterShips.get_Item((int)this._iStarterType).get_Item(selectedShipCursor).GetGraphicsMstId(), 9);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).MakePixelPerfect();
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Girl).get_transform().set_localScale(Vector3.get_one());
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).get_transform().localPositionY(155f);
			Vector3 localPosition2 = this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).get_transform().get_localPosition();
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).get_transform().localPositionY(135f);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).get_transform().LTMoveLocal(localPosition2, 0.2f).setEase(LeanTweenType.easeOutSine);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).mainTexture = (Resources.Load("Textures/Startup/PartnerShip/startup_c" + text + "_txt") as Texture);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).localSize = Defines.STARTER_PARTNER_TEXT_SIZE.get_Item((int)this._iStarterType).get_Item(selectedShipCursor);
			this._dicShipParts.get_Item(CtrlPartnerSelect.ShipPartsIndex.Info).get_transform().set_localScale(Vector3.get_one() * 0.8f);
			ShipUtils.PlayShipVoice(this._listStarterShips.get_Item((int)this._iStarterType).get_Item(selectedShipCursor), 26);
			this._isMove = true;
		}

		public void cachePreLoad()
		{
		}
	}
}
