using KCV.Battle.Utils;
using KCV.BattleCut;
using KCV.Utils;
using local.models;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdEscortShipEvacuation : BaseAnimation
	{
		public enum HexType
		{
			None = -1,
			Back,
			Next
		}

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UITexture[] _uiShip;

		[SerializeField]
		private List<UIHexButton> _listHexBtns;

		[SerializeField]
		private UILabel _uiLabel1;

		[SerializeField]
		private UILabel _uiLabel2;

		[SerializeField]
		private ParticleSystem _uiSmoke;

		private int _debugIndex;

		private int _btnIndex;

		private bool _isDecide;

		private bool _isInputPossible;

		private bool _isBattleCut;

		private List<bool> _listIsBtn;

		private KeyControl _clsInput;

		private ShipModel[] _shipModels;

		private DelDecideAdvancingWithdrawalButton _delDecideAdvancingWithdrawalButton;

		public static ProdEscortShipEvacuation Instantiate(ProdEscortShipEvacuation prefab, Transform parent, KeyControl input, ShipModel[] escapeCandidate, bool isBattleCut)
		{
			ProdEscortShipEvacuation prodEscortShipEvacuation = Object.Instantiate<ProdEscortShipEvacuation>(prefab);
			prodEscortShipEvacuation.get_transform().set_parent(parent);
			prodEscortShipEvacuation.get_transform().localScaleOne();
			prodEscortShipEvacuation.get_transform().localPositionZero();
			prodEscortShipEvacuation._clsInput = input;
			prodEscortShipEvacuation._shipModels = escapeCandidate;
			prodEscortShipEvacuation._isBattleCut = isBattleCut;
			return prodEscortShipEvacuation;
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelArySafe<UITexture>(ref this._uiShip);
			Mem.Del<UILabel>(ref this._uiLabel1);
			Mem.Del<UILabel>(ref this._uiLabel2);
			Mem.Del(ref this._uiSmoke);
			Mem.DelListSafe<UIHexButton>(ref this._listHexBtns);
			Mem.DelListSafe<bool>(ref this._listIsBtn);
			Mem.Del<KeyControl>(ref this._clsInput);
			Mem.DelArySafe<ShipModel>(ref this._shipModels);
			Mem.Del<DelDecideAdvancingWithdrawalButton>(ref this._delDecideAdvancingWithdrawalButton);
		}

		public void Init()
		{
			this._debugIndex = 0;
			this._btnIndex = 0;
			this._isDecide = false;
			this._isInputPossible = false;
			if (this._uiPanel == null)
			{
				this._uiPanel = base.GetComponent<UIPanel>();
			}
			Util.FindParentToChild<UILabel>(ref this._uiLabel1, base.get_transform(), "Label1");
			Util.FindParentToChild<UILabel>(ref this._uiLabel2, base.get_transform(), "Label2");
			Util.FindParentToChild<ParticleSystem>(ref this._uiSmoke, base.get_transform(), "Smoke");
			this._uiPanel.depth = 70;
			this._uiSmoke.SetActive(false);
			this._uiShip = new UITexture[2];
			for (int i = 0; i < 2; i++)
			{
				Util.FindParentToChild<UITexture>(ref this._uiShip[i], base.get_transform(), "ShipObj" + (i + 1) + "/Ship");
			}
			this._listHexBtns = new List<UIHexButton>();
			this._listIsBtn = new List<bool>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(ProdEscortShipEvacuation.HexType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProdEscortShipEvacuation.HexType hexType = (ProdEscortShipEvacuation.HexType)((int)enumerator.get_Current());
					if (hexType != ProdEscortShipEvacuation.HexType.None)
					{
						this._listIsBtn.Add(false);
						this._listHexBtns.Add(base.get_transform().FindChild(string.Format("{0}Btn", hexType.ToString())).GetComponent<UIHexButton>());
						this._listHexBtns.get_Item((int)hexType).Init();
						this._listHexBtns.get_Item((int)hexType).SetIndex((int)hexType);
						this._listHexBtns.get_Item((int)hexType).uiButton.onClick = Util.CreateEventDelegateList(this, "DecideAdvancingWithDrawalBtn", this._listHexBtns.get_Item((int)hexType));
						this._listHexBtns.get_Item((int)hexType).isColliderEnabled = true;
					}
				}
			}
		}

		public void Play(DelDecideAdvancingWithdrawalButton decideCallback)
		{
			this._delDecideAdvancingWithdrawalButton = decideCallback;
			base.Init();
			this._setShipTexture();
			this._setLabel();
			this._btnIndex = 0;
			this._listIsBtn.set_Item(0, true);
			this._uiSmoke.SetActive(true);
			this._uiSmoke.Play();
			this._listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.SetActive(true);
				x.Play(UIHexButton.AnimationList.HexButtonShow, delegate
				{
					if (this._isBattleCut)
					{
						UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
						navigation.SetNavigationInEscortShipEvacuation();
						navigation.Show(0.2f, null);
					}
					else
					{
						UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
						battleNavigation.SetNavigationInEscortShipEvacuation();
						battleNavigation.Show(0.2f, null);
					}
					this._isInputPossible = true;
					this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				});
			});
		}

		public bool Run()
		{
			if (!this._isDecide && this._isInputPossible)
			{
				using (List<UIHexButton>.Enumerator enumerator = this._listHexBtns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIHexButton current = enumerator.get_Current();
						current.Run();
					}
				}
				if (this._clsInput.keyState.get_Item(14).down)
				{
					this._btnIndex = this._setButtonIndex(true);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				}
				else if (this._clsInput.keyState.get_Item(10).down)
				{
					this._btnIndex = this._setButtonIndex(false);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				}
				else if (this._clsInput.GetDown(KeyControl.KeyName.MARU))
				{
					this.DecideAdvancingWithDrawalBtn(this._listHexBtns.get_Item(this._btnIndex));
				}
			}
			return !this._isDecide;
		}

		private void _setShipTexture()
		{
			for (int i = 0; i < 2; i++)
			{
				int num = (i != 0) ? 0 : 1;
				bool flag = i == 0;
				if (this._shipModels[num] != null)
				{
					this._uiShip[i].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._shipModels[num].GetGraphicsMstId(), flag);
					this._uiShip[i].MakePixelPerfect();
					this._uiShip[i].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._shipModels[num].GetGraphicsMstId()).GetShipDisplayCenter(flag)));
				}
			}
		}

		private void _setLabel()
		{
			if (this._shipModels[0] == null || this._shipModels[1] == null)
			{
				return;
			}
			this._uiLabel1.text = string.Concat(new object[]
			{
				this._shipModels[1].Name,
				" Lv",
				this._shipModels[1].Level,
				"が大きく損傷しています。"
			});
			this._uiLabel2.text = "随伴艦の" + this._shipModels[0].Name + "を護衛につけて戦場から退避させますか？";
			this._uiLabel1.MakePixelPerfect();
			this._uiLabel2.MakePixelPerfect();
		}

		private void _debugShipTexture()
		{
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(this._debugIndex))
			{
				ShipModelMst shipModelMst = new ShipModelMst(this._debugIndex);
				this._uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._debugIndex, true);
				this._uiShip[0].MakePixelPerfect();
				this._uiShip[0].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._debugIndex).GetShipDisplayCenter(true)));
				this._uiShip[1].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._debugIndex, false);
				this._uiShip[1].MakePixelPerfect();
				this._uiShip[1].get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._debugIndex).GetShipDisplayCenter(false)));
				this._uiLabel1.text = string.Concat(new object[]
				{
					shipModelMst.Name,
					" Lv",
					100,
					"が大きく損傷しています。"
				});
				this._uiLabel2.text = "随伴艦の" + shipModelMst.Name + "を護衛につけて戦場から退避させますか？";
				this._uiLabel1.MakePixelPerfect();
				this._uiLabel2.MakePixelPerfect();
			}
		}

		private int _setButtonIndex(bool isUp)
		{
			return (!isUp) ? 1 : 0;
		}

		private void setTextEnabled()
		{
			UISprite component = this._listHexBtns.get_Item(0).get_transform().FindChild("Label/Text").GetComponent<UISprite>();
			component.spriteName = ((!this._listHexBtns.get_Item(0).isFocus) ? "txt_shelter_off" : "txt_shelter_on");
			UISprite component2 = this._listHexBtns.get_Item(1).get_transform().FindChild("Label/Text").GetComponent<UISprite>();
			component2.spriteName = ((!this._listHexBtns.get_Item(1).isFocus) ? "txt_continue_off" : "txt_continue_on");
		}

		private void SetAdvancingWithdrawalBtnState(int nIndex)
		{
			this._listHexBtns.ForEach(delegate(UIHexButton x)
			{
				if (x.index == nIndex)
				{
					x.isFocus = true;
				}
				else
				{
					x.isFocus = false;
				}
				x.SetFocusAnimation();
			});
			this.setTextEnabled();
		}

		private void DecideAdvancingWithDrawalBtn(UIHexButton btn)
		{
			if (this._isDecide)
			{
				return;
			}
			this._isDecide = true;
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
			this._listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.isColliderEnabled = false;
			});
			this.SetAdvancingWithdrawalBtnState(btn.index);
			if (this._isBattleCut)
			{
				UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
				navigation.Hide(0.2f, null);
			}
			else
			{
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.Hide(0.2f, null);
			}
			if (this._delDecideAdvancingWithdrawalButton != null)
			{
				this._delDecideAdvancingWithdrawalButton(btn);
			}
		}
	}
}
