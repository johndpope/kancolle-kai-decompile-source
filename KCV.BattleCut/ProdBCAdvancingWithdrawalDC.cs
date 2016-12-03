using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	public class ProdBCAdvancingWithdrawalDC : MonoBehaviour
	{
		[SerializeField]
		private List<Vector3> _listAWPos4Sortie;

		[SerializeField]
		private List<Vector3> _listAWPos4Rebellion;

		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		[Button("SetLabelPos", "set lavbels position to sortiemap battle.", new object[]
		{
			Generics.BattleRootType.SortieMap
		}), SerializeField]
		private int _nSetLabelPos2SortieMap;

		[Button("SetLabelPos", "set lavbels position to sortiemap battle.", new object[]
		{
			Generics.BattleRootType.Rebellion
		}), SerializeField]
		private int _nSetLabelPos2Rebellion;

		private UIPanel _uiPanel;

		private ShipModel_BattleAll _clsShipModel;

		private AdvancingWithdrawalDCType _iSelectType;

		private bool _isInputPossible;

		private Action<AdvancingWithdrawalDCType, ShipRecoveryType> _actOnDecide;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdBCAdvancingWithdrawalDC Instantiate(ProdBCAdvancingWithdrawalDC prefab, Transform parent, ShipModel_BattleAll flagShip, Generics.BattleRootType iRootType)
		{
			ProdBCAdvancingWithdrawalDC prodBCAdvancingWithdrawalDC = Object.Instantiate<ProdBCAdvancingWithdrawalDC>(prefab);
			prodBCAdvancingWithdrawalDC.get_transform().set_parent(parent);
			prodBCAdvancingWithdrawalDC.get_transform().localScaleOne();
			prodBCAdvancingWithdrawalDC.get_transform().localPositionZero();
			prodBCAdvancingWithdrawalDC.Init(flagShip, iRootType);
			return prodBCAdvancingWithdrawalDC;
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<ShipModel_BattleAll>(ref this._clsShipModel);
			Mem.DelListSafe<UILabelButton>(ref this._listLabelButton);
			Mem.Del<AdvancingWithdrawalDCType>(ref this._iSelectType);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<Action<AdvancingWithdrawalDCType, ShipRecoveryType>>(ref this._actOnDecide);
		}

		private bool Init(ShipModel_BattleAll flagShip, Generics.BattleRootType iRootType)
		{
			this._clsShipModel = flagShip;
			this.panel.alpha = 0f;
			this.SetLabelPos(iRootType);
			this._iSelectType = AdvancingWithdrawalDCType.Withdrawal;
			int cnt = 0;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				bool isValid = true;
				if (cnt == 2)
				{
					isValid = flagShip.HasRecoverMegami();
				}
				else if (cnt == 1)
				{
					isValid = flagShip.HasRecoverYouin();
				}
				else if (cnt == 3)
				{
					isValid = BattleCutManager.GetBattleManager().ChangeableDeck;
				}
				x.Init(cnt, isValid, KCVColor.ConvertColor(110f, 110f, 110f, 255f), KCVColor.ConvertColor(110f, 110f, 110f, 128f));
				x.isFocus = false;
				x.toggle.group = 20;
				x.toggle.set_enabled(false);
				x.toggle.onDecide = delegate
				{
					this.Decide();
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (AdvancingWithdrawalDCType)x.index);
				if (x.index == 0)
				{
					x.toggle.startsActive = true;
				}
				cnt++;
			});
			this.ChangeFocus(this._iSelectType);
			return true;
		}

		private void SetLabelPos(Generics.BattleRootType iType)
		{
			int cnt = 0;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.get_transform().set_localPosition((iType != Generics.BattleRootType.Rebellion) ? this._listAWPos4Sortie.get_Item(cnt) : this._listAWPos4Rebellion.get_Item(cnt));
				cnt++;
			});
		}

		public void Play(Action<AdvancingWithdrawalDCType, ShipRecoveryType> onDecide)
		{
			this._actOnDecide = onDecide;
			BattleCutManager.GetStateBattle().prodBCBattle.setResultHPModeAdvancingWithdrawal(-74.86f);
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInAdvancingWithdrawal();
			navigation.Show(Defines.PHASE_FADE_TIME, null);
			this.Show().setOnComplete(delegate
			{
				this._isInputPossible = true;
				this._listLabelButton.ForEach(delegate(UILabelButton x)
				{
					x.toggle.set_enabled(x.isValid);
				});
			});
		}

		public bool Run()
		{
			if (!this._isInputPossible)
			{
				return true;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				this.PreparaNext(true);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				this.PreparaNext(false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this.Decide();
			}
			return true;
		}

		private void ChangeFocus(AdvancingWithdrawalDCType iType)
		{
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = (x.index == (int)iType);
			});
		}

		private void PreparaNext(bool isFoward)
		{
			AdvancingWithdrawalDCType iSelectType = this._iSelectType;
			this._iSelectType = (AdvancingWithdrawalDCType)Mathe.NextElement((int)this._iSelectType, 0, 2, isFoward, (int x) => this._listLabelButton.get_Item(x).isValid);
			if (iSelectType != this._iSelectType)
			{
				this.ChangeFocus(this._iSelectType);
			}
		}

		private LTDescr Show()
		{
			return this.panel.get_transform().LTValue(this.panel.alpha, 1f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			return this.panel.get_transform().LTValue(this.panel.alpha, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private void OnActive(AdvancingWithdrawalDCType iType)
		{
			if (this._iSelectType != iType)
			{
				this._iSelectType = iType;
				this.ChangeFocus(this._iSelectType);
			}
		}

		public void Decide()
		{
			this._isInputPossible = false;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.set_enabled(false);
			});
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			ShipRecoveryType type = BattleUtils.GetShipRecoveryType(this._iSelectType);
			this.Hide().setOnComplete(delegate
			{
				Dlg.Call<AdvancingWithdrawalDCType, ShipRecoveryType>(ref this._actOnDecide, this._iSelectType, type);
			});
		}
	}
}
