using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAdvancingWithDrawalDC : BaseAnimation
	{
		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private List<UIHexButton> _listHexBtns;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Sortie;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Rebellion;

		[SerializeField]
		private UIFleetInfos _uiFleetInfos;

		[Button("SetHexBtnsPos4Sortie", "set hex buttons position for sortie battle.", new object[]
		{

		}), SerializeField]
		private int _nSetHexBtnsPos4Sortie;

		[Button("SetHexBtnsPos4Rebellion", "set hex buttons position for rebellion battle.", new object[]
		{

		}), SerializeField]
		private int _nSetHexBtnsPos4Rebellion;

		private DelDecideAdvancingWithdrawalButton _delDecideAdvancingWithdrawalButton;

		private bool _isDecide;

		private bool _isInputPossible;

		private List<bool> _listIsBtn;

		private int _btnIndex;

		private Generics.BattleRootType _iType;

		public static ProdAdvancingWithDrawalDC Instantiate(ProdAdvancingWithDrawalDC prefab, Transform parent, Generics.BattleRootType iType)
		{
			ProdAdvancingWithDrawalDC prodAdvancingWithDrawalDC = Object.Instantiate<ProdAdvancingWithDrawalDC>(prefab);
			prodAdvancingWithDrawalDC.get_transform().set_parent(parent);
			prodAdvancingWithDrawalDC.get_transform().localScaleZero();
			prodAdvancingWithDrawalDC.get_transform().localPositionZero();
			prodAdvancingWithDrawalDC.Init(iType);
			return prodAdvancingWithDrawalDC;
		}

		private bool Init(Generics.BattleRootType type)
		{
			this._iType = type;
			this._btnIndex = 0;
			if (this._uiPanel == null)
			{
				this._uiPanel = base.GetComponent<UIPanel>();
			}
			this._uiPanel.depth = 70;
			this._listHexBtns = new List<UIHexButton>();
			this._listIsBtn = new List<bool>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(AdvancingWithdrawalDCType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AdvancingWithdrawalDCType advancingWithdrawalDCType = (AdvancingWithdrawalDCType)((int)enumerator.get_Current());
					if (advancingWithdrawalDCType != AdvancingWithdrawalDCType.None)
					{
						this._listIsBtn.Add(false);
						this._listHexBtns.Add(base.get_transform().FindChild(string.Format("{0}Btn", advancingWithdrawalDCType.ToString())).GetComponent<UIHexButton>());
						this._listHexBtns.get_Item((int)advancingWithdrawalDCType).Init();
						this._listHexBtns.get_Item((int)advancingWithdrawalDCType).SetIndex((int)advancingWithdrawalDCType);
						this._listHexBtns.get_Item((int)advancingWithdrawalDCType).uiButton.onClick = Util.CreateEventDelegateList(this, "DecideAdvancingWithDrawalBtn", this._listHexBtns.get_Item((int)advancingWithdrawalDCType));
						this._listHexBtns.get_Item((int)advancingWithdrawalDCType).isColliderEnabled = true;
					}
				}
			}
			this._uiFleetInfos.Init(new List<ShipModel_BattleAll>(BattleTaskManager.GetBattleManager().Ships_f));
			this._uiFleetInfos.widget.alpha = 0f;
			return true;
		}

		private void SetHexBtnsPos4Sortie()
		{
			int cnt = 0;
			this._listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.get_transform().set_localPosition(this._listHexExBtnsPos4Sortie.get_Item(cnt));
				cnt++;
			});
		}

		private void SetHexBtnsPos4Rebellion()
		{
			int cnt = 0;
			this._listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.get_transform().set_localPosition(this._listHexExBtnsPos4Rebellion.get_Item(cnt));
				cnt++;
			});
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelListSafe<UIHexButton>(ref this._listHexBtns);
			Mem.DelListSafe<Vector3>(ref this._listHexExBtnsPos4Sortie);
			Mem.DelListSafe<Vector3>(ref this._listHexExBtnsPos4Rebellion);
			Mem.Del<UIFleetInfos>(ref this._uiFleetInfos);
			Mem.Del<int>(ref this._nSetHexBtnsPos4Sortie);
			Mem.Del<int>(ref this._nSetHexBtnsPos4Rebellion);
			Mem.Del<DelDecideAdvancingWithdrawalButton>(ref this._delDecideAdvancingWithdrawalButton);
			Mem.Del<bool>(ref this._isDecide);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.DelListSafe<bool>(ref this._listIsBtn);
			Mem.Del<int>(ref this._btnIndex);
			Mem.Del<bool>(ref this._isInputPossible);
		}

		public void Play(DelDecideAdvancingWithdrawalButton decideCallback)
		{
			this._delDecideAdvancingWithdrawalButton = decideCallback;
			base.get_transform().localScaleOne();
			base.Init();
			if (this._iType == Generics.BattleRootType.Rebellion)
			{
				this.SetHexBtnsPos4Rebellion();
			}
			else
			{
				this.SetHexBtnsPos4Sortie();
			}
			ShipModel_BattleAll shipModel_BattleAll = BattleTaskManager.GetBattleManager().Ships_f[0];
			if (shipModel_BattleAll.HasRecoverYouin())
			{
				this._btnIndex = 1;
			}
			else if (shipModel_BattleAll.HasRecoverMegami())
			{
				this._btnIndex = 2;
			}
			else
			{
				this._btnIndex = 0;
			}
			if (shipModel_BattleAll.HasRecoverYouin())
			{
				this._listIsBtn.set_Item(1, true);
			}
			else
			{
				this._listHexBtns.get_Item(1).uiButton.defaultColor = new Color(0.2f, 0.2f, 0.2f);
			}
			if (shipModel_BattleAll.HasRecoverMegami())
			{
				this._listIsBtn.set_Item(2, true);
			}
			else
			{
				this._listHexBtns.get_Item(2).uiButton.defaultColor = new Color(0.2f, 0.2f, 0.2f);
			}
			if (BattleTaskManager.GetBattleManager().ChangeableDeck)
			{
				this._listIsBtn.set_Item(3, BattleTaskManager.GetBattleManager().ChangeableDeck);
			}
			else
			{
				this._listHexBtns.get_Item(3).uiButton.defaultColor = new Color(0.2f, 0.2f, 0.2f);
			}
			this._listIsBtn.set_Item(0, true);
			this._listHexBtns.ForEach(delegate(UIHexButton x)
			{
				x.SetActive(true);
				x.Play(UIHexButton.AnimationList.HexButtonShow, delegate
				{
					this._isInputPossible = true;
					this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				});
			});
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInAdvancingWithDrawal();
			battleNavigation.Show(0.2f, null);
			this._uiFleetInfos.Show();
		}

		public bool Run()
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
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
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					this._btnIndex = this._setButtonIndex(this._btnIndex, false);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				}
				if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					this._btnIndex = this._setButtonIndex(this._btnIndex, true);
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					this.DecideAdvancingWithDrawalBtn(this._listHexBtns.get_Item(this._btnIndex));
				}
			}
			return !this._isDecide;
		}

		private int _setButtonIndex(int nIndex, bool isUp)
		{
			int result = nIndex;
			if (!isUp)
			{
				for (int i = nIndex; i < 4; i++)
				{
					if (i != nIndex)
					{
						if (this._listIsBtn.get_Item(i))
						{
							result = i;
							break;
						}
					}
				}
			}
			else
			{
				for (int j = nIndex; j > -1; j--)
				{
					if (j != nIndex)
					{
						if (this._listIsBtn.get_Item(j))
						{
							result = j;
							break;
						}
					}
				}
			}
			return result;
		}

		private void setTextEnabled()
		{
			UISprite component = this._listHexBtns.get_Item(1).get_transform().FindChild("Label/Text").GetComponent<UISprite>();
			component.spriteName = ((!this._listHexBtns.get_Item(1).isFocus) ? "txt_yoin_off" : "txt_yoin_on");
			UISprite component2 = this._listHexBtns.get_Item(2).get_transform().FindChild("Label/Text").GetComponent<UISprite>();
			component2.spriteName = ((!this._listHexBtns.get_Item(2).isFocus) ? "txt_megami_off" : "txt_megami_on");
			UISprite component3 = this._listHexBtns.get_Item(0).get_transform().FindChild("Label/Text").GetComponent<UISprite>();
			component3.spriteName = ((!this._listHexBtns.get_Item(0).isFocus) ? "txt_escape_off" : "txt_escape_on");
			UISprite component4 = this._listHexBtns.get_Item(3).get_transform().FindChild("Label/Text1").GetComponent<UISprite>();
			UISprite component5 = this._listHexBtns.get_Item(3).get_transform().FindChild("Label/Text2").GetComponent<UISprite>();
			component4.spriteName = ((!this._listHexBtns.get_Item(3).isFocus) ? "txt_go_off" : "txt_go_on");
			component5.spriteName = ((!this._listHexBtns.get_Item(3).isFocus) ? "txt_kessen_off" : "txt_kessen_on");
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
			ShipModel_BattleAll shipModel_BattleAll = BattleTaskManager.GetBattleManager().Ships_f[0];
			if (btn.index == 1 && !shipModel_BattleAll.HasRecoverYouin())
			{
				return;
			}
			if (btn.index == 2 && !shipModel_BattleAll.HasRecoverMegami())
			{
				return;
			}
			if (btn.index == 1 && this._btnIndex != btn.index)
			{
				this._btnIndex = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				return;
			}
			if (btn.index == 2 && this._btnIndex != btn.index)
			{
				this._btnIndex = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				return;
			}
			if (btn.index == 0 && this._btnIndex != btn.index)
			{
				this._btnIndex = btn.index;
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.SetAdvancingWithdrawalBtnState(this._btnIndex);
				return;
			}
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
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.Hide(0.2f, null);
			if (this._delDecideAdvancingWithdrawalButton != null)
			{
				this._delDecideAdvancingWithdrawalButton(btn);
			}
		}
	}
}
