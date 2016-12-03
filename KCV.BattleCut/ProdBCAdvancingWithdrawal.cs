using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using local.managers;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBCAdvancingWithdrawal : MonoBehaviour
	{
		[SerializeField]
		private List<Vector3> _listAWPos4Sortie;

		[SerializeField]
		private List<Vector3> _listAWPos4Rebellion;

		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private Generics.BattleRootType _iRootType;

		private AdvancingWithdrawalType _iSelectType;

		private Action<AdvancingWithdrawalType> _actCallback;

		private List<bool> _listEnabledBtn;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private int maxIndex
		{
			get
			{
				int result = 0;
				Generics.BattleRootType iRootType = this._iRootType;
				if (iRootType != Generics.BattleRootType.SortieMap)
				{
					if (iRootType == Generics.BattleRootType.Rebellion)
					{
						BattleManager battleManager = BattleCutManager.GetBattleManager();
						result = ((!battleManager.ChangeableDeck) ? 1 : 2);
					}
				}
				else
				{
					result = 1;
				}
				return result;
			}
		}

		public static ProdBCAdvancingWithdrawal Instantiate(ProdBCAdvancingWithdrawal prefab, Transform parent, Generics.BattleRootType iType)
		{
			ProdBCAdvancingWithdrawal prodBCAdvancingWithdrawal = Object.Instantiate<ProdBCAdvancingWithdrawal>(prefab);
			prodBCAdvancingWithdrawal.get_transform().set_parent(parent);
			prodBCAdvancingWithdrawal.get_transform().localPositionZero();
			prodBCAdvancingWithdrawal.get_transform().localScaleOne();
			prodBCAdvancingWithdrawal.Init(iType);
			return prodBCAdvancingWithdrawal;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<bool>(ref this._listEnabledBtn);
			Mem.DelListSafe<UILabelButton>(ref this._listLabelButton);
			Mem.DelListSafe<Vector3>(ref this._listAWPos4Sortie);
			Mem.DelListSafe<Vector3>(ref this._listAWPos4Rebellion);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Generics.BattleRootType>(ref this._iRootType);
			Mem.Del<AdvancingWithdrawalType>(ref this._iSelectType);
			Mem.Del<Action<AdvancingWithdrawalType>>(ref this._actCallback);
		}

		private bool Init(Generics.BattleRootType iType)
		{
			this._isInputPossible = false;
			this.panel.alpha = 0f;
			this._iRootType = iType;
			this._iSelectType = AdvancingWithdrawalType.Withdrawal;
			this.SetEnabledBtns(iType);
			this.SetLabelPos(iType);
			int cnt = 0;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.Init(cnt, this._listEnabledBtn.get_Item(cnt), KCVColor.ConvertColor(110f, 110f, 110f, 255f), KCVColor.ConvertColor(110f, 110f, 110f, 128f));
				x.isFocus = false;
				x.toggle.group = 1;
				x.toggle.set_enabled(false);
				x.toggle.onDecide = delegate
				{
					this.DecideAdvancingWithDrawal();
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (AdvancingWithdrawalType)x.index);
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

		private void SetEnabledBtns(Generics.BattleRootType iType)
		{
			this._listEnabledBtn = new List<bool>();
			if (iType != Generics.BattleRootType.SortieMap)
			{
				if (iType == Generics.BattleRootType.Rebellion)
				{
					this._listEnabledBtn.Add(true);
					this._listEnabledBtn.Add(BattleCutManager.GetBattleManager().Ships_f[0].DmgStateEnd != DamageState_Battle.Taiha);
					this._listEnabledBtn.Add(BattleCutManager.GetBattleManager().ChangeableDeck);
				}
			}
			else
			{
				this._listEnabledBtn.Add(true);
				this._listEnabledBtn.Add(true);
				this._listEnabledBtn.Add(false);
			}
		}

		public void Play(Action<AdvancingWithdrawalType> onDecideCallback)
		{
			this._actCallback = onDecideCallback;
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
				this.DecideAdvancingWithDrawal();
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			AdvancingWithdrawalType iSelectType = this._iSelectType;
			this._iSelectType = (AdvancingWithdrawalType)Mathe.NextElement((int)this._iSelectType, 0, this.maxIndex, isFoward, (int x) => this._listLabelButton.get_Item(x).isValid);
			if (iSelectType != this._iSelectType)
			{
				this.ChangeFocus(this._iSelectType);
			}
		}

		private void ChangeFocus(AdvancingWithdrawalType iType)
		{
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = (x.index == (int)iType);
			});
		}

		private LTDescr Show()
		{
			return this.panel.get_transform().LTValue(0f, 1f, Defines.PHASE_FADE_TIME).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			return this.panel.get_transform().LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private void OnActive(AdvancingWithdrawalType nIndex)
		{
			if (this._iSelectType != nIndex)
			{
				this._iSelectType = nIndex;
				this.ChangeFocus(this._iSelectType);
			}
		}

		private void DecideAdvancingWithDrawal()
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			this._isInputPossible = false;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.set_enabled(false);
			});
			this.Hide().setOnComplete(delegate
			{
				Dlg.Call<AdvancingWithdrawalType>(ref this._actCallback, this._iSelectType);
			});
		}
	}
}
