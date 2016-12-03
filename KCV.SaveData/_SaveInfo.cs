using System;
using UnityEngine;

namespace KCV.SaveData
{
	public class _SaveInfo : MonoBehaviour
	{
		[Serializable]
		private class DisplayInfo
		{
			private GameObject _objDisplayInfo;

			private UITexture _uiBackground;

			private UILabel _uiName;

			private UILabel _uiLv;

			private UILabel _uiFleetNum;

			private UILabel _uiShipNum;

			private UILabel _uiCheatsArea;

			private UILabel _uiArea;

			private UILabel _uiTurn;

			private UILabel _uiDifficlty;

			private UILabel _uiTactics;

			public DisplayInfo(Transform parent, string objName)
			{
				Util.FindParentToChild(ref this._objDisplayInfo, parent, objName);
				Util.FindParentToChild<UITexture>(ref this._uiBackground, this._objDisplayInfo.get_transform(), "bg");
				Util.FindParentToChild<UILabel>(ref this._uiName, this._objDisplayInfo.get_transform(), "Name");
				Util.FindParentToChild<UILabel>(ref this._uiLv, this._objDisplayInfo.get_transform(), "Lv");
				Util.FindParentToChild<UILabel>(ref this._uiFleetNum, this._objDisplayInfo.get_transform(), "FleetNum");
				Util.FindParentToChild<UILabel>(ref this._uiShipNum, this._objDisplayInfo.get_transform(), "ShipNum");
				Util.FindParentToChild<UILabel>(ref this._uiCheatsArea, this._objDisplayInfo.get_transform(), "CheatsArea");
				Util.FindParentToChild<UILabel>(ref this._uiArea, this._objDisplayInfo.get_transform(), "Area");
				Util.FindParentToChild<UILabel>(ref this._uiTurn, this._objDisplayInfo.get_transform(), "Turn");
				Util.FindParentToChild<UILabel>(ref this._uiDifficlty, this._objDisplayInfo.get_transform(), "Difficlty");
				Util.FindParentToChild<UILabel>(ref this._uiTactics, this._objDisplayInfo.get_transform(), "Tactics");
			}

			public void SetDisplayData()
			{
				this._uiName.text = string.Format("{0}", "だみはらだみこだみはらだ");
				this._uiLv.text = string.Format("司令部LV：{0}", 999);
				this._uiFleetNum.text = string.Format("保有艦隊数：{0}", 999);
				this._uiShipNum.text = string.Format("艦娘保有数：{0}", 999);
				this._uiCheatsArea.text = string.Format("攻略海域数：{0}", 999);
				this._uiArea.text = string.Format("現\u3000在\u3000地：{0}", "鎮守府海域");
				this._uiTurn.text = string.Format("現在ターン：{0}", "零の年 睦月17日");
				this._uiTactics.text = string.Format("遂行中作戦：{0}", "第17海域あ号作戦");
				this._uiDifficlty.text = string.Format("難\u3000易\u3000度：{0}", "ナイトメアモード");
			}

			public void Release()
			{
				this._objDisplayInfo = null;
				this._uiBackground = null;
				this._uiName = null;
				this._uiLv = null;
				this._uiFleetNum = null;
				this._uiShipNum = null;
				this._uiCheatsArea = null;
				this._uiArea = null;
				this._uiDifficlty = null;
				this._uiTactics = null;
			}
		}

		private UITexture _uiThumb;

		private _SaveInfo.DisplayInfo _uiDisplayInfo;

		private void Awake()
		{
			Util.FindParentToChild<UITexture>(ref this._uiThumb, base.get_transform(), "Thumbnail/Thumb");
			this._uiDisplayInfo = new _SaveInfo.DisplayInfo(base.get_transform(), "DisplayInfo");
			this._uiDisplayInfo.SetDisplayData();
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDisable()
		{
			this._uiThumb = null;
			this._uiDisplayInfo = null;
		}

		public void SetSaveData()
		{
			this._uiDisplayInfo.SetDisplayData();
		}
	}
}
