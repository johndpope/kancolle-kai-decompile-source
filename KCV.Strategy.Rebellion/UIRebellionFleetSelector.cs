using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionFleetSelector : MonoBehaviour, RouletteSelectorHandler
	{
		[Serializable]
		private class FleetInfos
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UILabel _uiFleetName;

			[SerializeField]
			private UITexture _uiFleetNum;

			[SerializeField]
			private List<UIButton> _listBtns;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public List<UIButton> buttons
			{
				get
				{
					return this._listBtns;
				}
			}

			public bool Init(MonoBehaviour mono, string methodName)
			{
				int cnt = 0;
				this._listBtns.ForEach(delegate(UIButton x)
				{
					x.onClick = Util.CreateEventDelegateList(mono, methodName, (UIRebellionFleetSelector.ArrowType)cnt);
					cnt++;
				});
				return true;
			}

			public bool UnInit()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UITexture>(ref this._uiBackground);
				Mem.Del<UILabel>(ref this._uiFleetName);
				Mem.Del<UITexture>(ref this._uiFleetNum);
				Mem.DelListSafe<UIButton>(ref this._listBtns);
				return true;
			}

			public void SetFleetInfos(string fleetName, int fleetID)
			{
				this._uiFleetName.text = fleetName;
				this._uiFleetName.supportEncoding = false;
				this._uiFleetNum.mainTexture = Resources.Load<Texture2D>(string.Format("Textures/Common/DeckFlag/icon_deck{0}", fleetID));
				this._uiFleetNum.MakePixelPerfect();
			}
		}

		public enum ArrowType
		{
			Left,
			Right
		}

		[SerializeField]
		private Transform _prefabSelectorShip;

		[SerializeField]
		private RouletteSelector _clsRouletteSelector;

		[SerializeField]
		private Vector3 _vOriginPos = new Vector3(240f, -160f, 0f);

		[SerializeField]
		private UIRebellionFleetSelector.FleetInfos _clsFleetInfos;

		[SerializeField]
		private List<Vector3> _listFleetInfosPos;

		[SerializeField]
		private List<Vector3> _listSelectorPos;

		private int _nSelectedIndex;

		private List<DeckModel> _listDeckModels;

		private List<UIRebellionSelectorShip> _listSelectorShips;

		private List<BoxCollider2D> _listCollider;

		private DelDecideRebellionOrganizeFleetSelector _delDecideRebellionOrganizeFleetSelector;

		public int selectedIndex
		{
			get
			{
				return this._nSelectedIndex;
			}
		}

		public int fleetCnt
		{
			get
			{
				if (this._listDeckModels == null)
				{
					return -1;
				}
				return this._listDeckModels.get_Count();
			}
		}

		public DeckModel nowSelectedDeck
		{
			get
			{
				return this._listDeckModels.get_Item(this._nSelectedIndex);
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				if (this._listCollider == null)
				{
					this._listCollider = new List<BoxCollider2D>();
					this._listCollider.AddRange(base.GetComponentsInChildren<BoxCollider2D>());
				}
				return this._listCollider.get_Item(0).get_enabled();
			}
			set
			{
				if (this.isColliderEnabled != value)
				{
					this._listCollider.ForEach(delegate(BoxCollider2D x)
					{
						x.set_enabled(value);
					});
				}
			}
		}

		public RouletteSelector rouletteSelector
		{
			get
			{
				return this._clsRouletteSelector;
			}
		}

		public UIPanel panel
		{
			get
			{
				return base.GetComponent<UIPanel>();
			}
		}

		public UIRebellionFleetSelector()
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(new Vector3(3f, -11f, 0f));
			list.Add(new Vector3(-494f, -22f, 0f));
			this._listFleetInfosPos = list;
			list = new List<Vector3>();
			list.Add(new Vector3(0f, 231f, 0f));
			list.Add(new Vector3(-525f, 242f, 0f));
			this._listSelectorPos = list;
			base..ctor();
		}

		bool RouletteSelectorHandler.IsSelectable(int index)
		{
			DebugUtils.Log("UIRebellionFleetSelector", "index:" + index);
			return true;
		}

		void RouletteSelectorHandler.OnUpdateIndex(int index, Transform transform)
		{
			DebugUtils.Log("UIRebellionFleetSelector", string.Format("index:{0} transform:{1}", index, transform.get_name()));
			this.ChangeFleet(index);
		}

		void RouletteSelectorHandler.OnSelect(int index, Transform transform)
		{
			DebugUtils.Log("UIRebellionFleetSelector", string.Format("index:{0} transform:{1}", index, transform.get_name()));
			this._nSelectedIndex = index;
			DebugUtils.Log(string.Format("[{0}({1})]{2}", this.nowSelectedDeck.Id, this.nowSelectedDeck.Name, (this.nowSelectedDeck.GetFlagShip() == null) ? string.Empty : this.nowSelectedDeck.GetFlagShip().Name));
			if (this._delDecideRebellionOrganizeFleetSelector != null)
			{
				this._delDecideRebellionOrganizeFleetSelector(this.nowSelectedDeck);
			}
		}

		public static UIRebellionFleetSelector Instantiate(UIRebellionFleetSelector prefab, Transform parent)
		{
			UIRebellionFleetSelector uIRebellionFleetSelector = Object.Instantiate<UIRebellionFleetSelector>(prefab);
			uIRebellionFleetSelector.get_transform().set_parent(parent);
			uIRebellionFleetSelector.get_transform().localScaleOne();
			uIRebellionFleetSelector.get_transform().set_localPosition(uIRebellionFleetSelector._vOriginPos);
			return uIRebellionFleetSelector;
		}

		private void Awake()
		{
			if (this._clsRouletteSelector == null)
			{
				Util.FindParentToChild<RouletteSelector>(ref this._clsRouletteSelector, base.get_transform(), "ShipRoletteSelector");
			}
			this._nSelectedIndex = 0;
			this._listDeckModels = new List<DeckModel>();
			this._listSelectorShips = new List<UIRebellionSelectorShip>();
			this._clsFleetInfos.Init(this, "DecideSelectorArrow");
			this._clsFleetInfos.transform.set_localPosition(this._listFleetInfosPos.get_Item(0));
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabSelectorShip);
			Mem.Del<RouletteSelector>(ref this._clsRouletteSelector);
			Mem.DelListSafe<Vector3>(ref this._listSelectorPos);
			Mem.Del<DelDecideRebellionOrganizeFleetSelector>(ref this._delDecideRebellionOrganizeFleetSelector);
			Mem.Del<int>(ref this._nSelectedIndex);
			Mem.DelListSafe<DeckModel>(ref this._listDeckModels);
			Mem.DelListSafe<UIRebellionSelectorShip>(ref this._listSelectorShips);
			Mem.DelListSafe<BoxCollider2D>(ref this._listCollider);
		}

		public bool Init(List<DeckModel> models, int initIndex, DelDecideRebellionOrganizeFleetSelector decideDelegate)
		{
			DebugUtils.Log("UIRebellionFleetSelector", string.Empty);
			this._listDeckModels = models;
			this._nSelectedIndex = initIndex;
			this._delDecideRebellionOrganizeFleetSelector = decideDelegate;
			this.SetFleetInfos(initIndex);
			using (IEnumerator enumerator = this._clsRouletteSelector.GetContainer().get_transform().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					Object.Destroy(transform.get_gameObject());
				}
			}
			int num = 0;
			using (List<DeckModel>.Enumerator enumerator2 = models.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DeckModel current = enumerator2.get_Current();
					this._listSelectorShips.Add(UIRebellionSelectorShip.Instantiate(this._prefabSelectorShip.GetComponent<UIRebellionSelectorShip>(), this._clsRouletteSelector.get_transform(), Vector3.get_zero(), current.GetFlagShip()));
					this._listSelectorShips.get_Item(num).get_transform().set_name("SelectorShips" + num);
					num++;
				}
			}
			this._clsRouletteSelector.Init(this);
			this._clsRouletteSelector.SetKeyController(StrategyTaskManager.GetStrategyRebellion().keycontrol);
			this._clsRouletteSelector.ScaleForce(0.3f, 1f);
			return true;
		}

		public void ReqMode(CtrlRebellionOrganize.RebellionOrganizeMode iMode, float time, LeanTweenType easeType)
		{
			if (iMode != CtrlRebellionOrganize.RebellionOrganizeMode.Main)
			{
				if (iMode == CtrlRebellionOrganize.RebellionOrganizeMode.Detail)
				{
					this._clsFleetInfos.buttons.ForEach(delegate(UIButton x)
					{
						x.SetActive(false);
					});
					this._clsFleetInfos.transform.LTMoveLocal(this._listFleetInfosPos.get_Item((int)iMode), time).setEase(easeType);
					this._listSelectorShips.ForEach(delegate(UIRebellionSelectorShip x)
					{
						if (x.shipModel != this.nowSelectedDeck.GetFlagShip())
						{
							x.get_transform().LTValue(1f, 0f, time).setEase(easeType).setOnUpdate(delegate(float y)
							{
								x.textureAlpha = y;
							});
						}
					});
					this._clsRouletteSelector.get_transform().LTMoveLocal(this._listSelectorPos.get_Item(1), time).setEase(easeType);
				}
			}
			else
			{
				this._clsFleetInfos.buttons.ForEach(delegate(UIButton x)
				{
					x.SetActive(true);
				});
				this._clsFleetInfos.transform.LTMoveLocal(this._listFleetInfosPos.get_Item((int)iMode), time).setEase(easeType);
				this._listSelectorShips.ForEach(delegate(UIRebellionSelectorShip x)
				{
					if (x.shipModel != this.nowSelectedDeck.GetFlagShip())
					{
						x.get_transform().LTValue(0f, 1f, time).setEase(easeType).setOnUpdate(delegate(float y)
						{
							x.textureAlpha = y;
						});
					}
				});
				this._clsRouletteSelector.get_transform().LTMoveLocal(this._listSelectorPos.get_Item(0), time).setEase(easeType);
			}
		}

		private void ChangeFleet(int nIndex)
		{
			this._nSelectedIndex = nIndex;
			this.SetFleetInfos(nIndex);
			ShipUtils.PlayShipVoice(this._listDeckModels.get_Item(nIndex).GetFlagShip(), 13);
		}

		private void SetFleetInfos(int nFleetNum)
		{
			if (nFleetNum >= this.fleetCnt)
			{
				return;
			}
			this._clsFleetInfos.SetFleetInfos(this._listDeckModels.get_Item(nFleetNum).Name, this._listDeckModels.get_Item(nFleetNum).Id);
		}

		private void DecideSelectorArrow(UIRebellionFleetSelector.ArrowType iType)
		{
			this._clsFleetInfos.buttons.get_Item((int)iType).state = UIButtonColor.State.Normal;
			if (iType != UIRebellionFleetSelector.ArrowType.Left)
			{
				if (iType == UIRebellionFleetSelector.ArrowType.Right)
				{
					this._clsRouletteSelector.MoveNext();
				}
			}
			else
			{
				this._clsRouletteSelector.MovePrev();
			}
		}
	}
}
