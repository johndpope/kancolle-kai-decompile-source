using Common.Enum;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyCatapultMenu : MonoBehaviour
	{
		private const int NormalWidth = 321;

		private const int ShowCatapultWidth = 750;

		private const int KaisyuModeLinePosX = 424;

		private const int ItemModeLinePosX = 560;

		[SerializeField]
		private UISprite Catapult;

		[SerializeField]
		private UISprite MenuBG;

		[SerializeField]
		private UISprite MenuLine;

		[SerializeField]
		private UILabel SPointNumLabel;

		[SerializeField]
		private UILabel NejiNumLabel;

		[SerializeField]
		private UIButton ItemShopBtn;

		[SerializeField]
		private UIButton ItemHouseBtn;

		[SerializeField]
		private UIButton ArsenalBtn;

		[SerializeField]
		private Transform ItemModeLabel;

		[SerializeField]
		private Transform KaisyuModeLabel;

		[SerializeField]
		private BoxCollider2D CancelTouch;

		[SerializeField]
		private UIButtonManager ButtonManager;

		private TweenPosition TweenPos;

		private Transform MenuBox;

		private bool isItemMode;

		private bool isShow;

		private KeyControl key;

		[SerializeField]
		private Camera camera;

		[Button("Initialize", "Initialize", new object[]
		{

		})]
		public int Button00;

		[Button("Show", "Show", new object[]
		{

		})]
		public int Button01;

		[Button("Hide", "Hide", new object[]
		{

		})]
		public int Button02;

		[Button("setItemMode", "setItemMode", new object[]
		{

		})]
		public int Button03;

		[Button("setKaisyuMode", "setKaisyuMode", new object[]
		{

		})]
		public int Button04;

		private void Awake()
		{
			this.TweenPos = base.GetComponent<TweenPosition>();
			this.MenuBox = base.get_transform().FindChild("MenuBox");
			this.isShow = false;
			this.CancelTouch.set_enabled(false);
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			StrategyCatapultMenu.<Start>c__Iterator16E <Start>c__Iterator16E = new StrategyCatapultMenu.<Start>c__Iterator16E();
			<Start>c__Iterator16E.<>f__this = this;
			return <Start>c__Iterator16E;
		}

		private void Update()
		{
			if (App.OnlyController == this.key)
			{
				this.key.Update();
				if (this.key.IsAnyKey)
				{
					this.key.ClearKeyAll();
					this.Hide();
				}
			}
		}

		public void OnTouch()
		{
			if (StrategyTopTaskManager.GetSailSelect().isRun)
			{
				ShipModel flagShip = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();
				if (flagShip == null)
				{
					CommonPopupDialog.Instance.StartPopup("艦隊が編成されていません");
					return;
				}
				this.Initialize(flagShip);
				if (this.isShow)
				{
					this.Hide();
				}
				else
				{
					this.Show();
				}
			}
		}

		public void Initialize(ShipModel FlagShip)
		{
			StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
			this.SPointNumLabel.textInt = logicManager.UserInfo.SPoint;
			this.NejiNumLabel.textInt = logicManager.Material.Revkit;
			if (FlagShip.ShipType == 19)
			{
				this.setKaisyuMode();
			}
			else
			{
				this.setItemMode();
			}
		}

		public void Show()
		{
			this.isShow = true;
			App.OnlyController = this.key;
			this.CancelTouch.set_enabled(true);
			this.camera.SetActive(true);
			this.MenuBox.SetActive(true);
			this.TweenPos.onFinished.Clear();
			this.TweenPos.PlayForward();
		}

		public void Hide()
		{
			this.isShow = false;
			App.OnlyController = null;
			this.CancelTouch.set_enabled(false);
			this.TweenPos.onFinished.Clear();
			this.TweenPos.SetOnFinished(new EventDelegate.Callback(this.OnHideFinished));
			this.TweenPos.PlayReverse();
		}

		private void OnHideFinished()
		{
			this.MenuBox.SetActive(false);
			this.camera.SetActive(false);
		}

		private void setItemMode()
		{
			this.isItemMode = true;
			this.ArsenalBtn.SetActive(false);
			this.KaisyuModeLabel.SetActive(false);
			this.ItemModeLabel.SetActive(true);
			this.MenuLine.get_transform().localPositionX(560f);
		}

		private void setKaisyuMode()
		{
			this.isItemMode = false;
			this.ArsenalBtn.SetActive(true);
			this.KaisyuModeLabel.SetActive(true);
			this.ItemModeLabel.SetActive(false);
			this.MenuLine.get_transform().localPositionX(424f);
		}

		public void OnClickStoreButton()
		{
			if (!this.isMoveable())
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add(UserInterfaceItemManager.SHARE_DATA_START_AT_KEY, UserInterfaceItemManager.SHARE_DATA_START_AT_VALUE_ITEMSTORE);
			RetentionData.SetData(hashtable);
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Item);
		}

		public void OnClickHouseButton()
		{
			if (!this.isMoveable())
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add(UserInterfaceItemManager.SHARE_DATA_START_AT_KEY, UserInterfaceItemManager.SHARE_DATA_START_AT_VALUE_ITEMLIST);
			RetentionData.SetData(hashtable);
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Item);
		}

		public void OnClickKaisyuButton()
		{
			if (!this.isMoveable())
			{
				return;
			}
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.ImprovementArsenal);
		}

		private bool isMoveable()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				CommonPopupDialog.Instance.StartPopup("撤退中の艦を含んでいます");
				return false;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != MissionStates.NONE)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(IsGoCondition.Mission));
				return false;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count == 0)
			{
				CommonPopupDialog.Instance.StartPopup("艦隊が編成されていません");
				return false;
			}
			return true;
		}

		private void OnDestroy()
		{
			this.Catapult = null;
			this.MenuBG = null;
			this.MenuLine = null;
			this.SPointNumLabel = null;
			this.NejiNumLabel = null;
			this.ItemShopBtn = null;
			this.ItemHouseBtn = null;
			this.ArsenalBtn = null;
			this.ItemModeLabel = null;
			this.KaisyuModeLabel = null;
			this.CancelTouch = null;
			this.ButtonManager = null;
			this.TweenPos = null;
			this.MenuBox = null;
			this.key = null;
			this.camera = null;
		}
	}
}
