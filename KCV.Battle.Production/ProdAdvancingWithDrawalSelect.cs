using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdAdvancingWithDrawalSelect : BaseAnimation
	{
		[SerializeField]
		private List<UIAdvancingWithDrawalButton> _listHexExBtns;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Sortie;

		[SerializeField]
		private List<Vector3> _listHexExBtnsPos4Rebellion;

		[SerializeField]
		private UIFleetInfos _uiFleetInfos;

		[SerializeField]
		private int _nToggleGroup = 10;

		[Button("SetHexBtnsPos4Sortie", "set hex buttons position for sortie battle.", new object[]
		{

		}), SerializeField]
		private int _nSetHexBtnsPos4Sortie;

		[Button("SetHexBtnsPos4Rebellion", "set hex buttons position for rebellion battle.", new object[]
		{

		}), SerializeField]
		private int _nSetHexBtnsPos4Rebellion;

		private UIPanel _uiPanel;

		private int _nIndex;

		private bool _isDecide;

		private bool _isInputPossible;

		private List<bool> _listEnabledBtn;

		private Generics.BattleRootType _iRootType;

		private DelDecideHexButtonEx _delDecideAdvancingWithdrawalButton;

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
			private set
			{
				this._uiPanel = value;
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
						BattleManager battleManager = BattleTaskManager.GetBattleManager();
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

		private bool isAdvancindPrimaryEnabled
		{
			get
			{
				return BattleTaskManager.GetBattleManager().ChangeableDeck;
			}
		}

		private bool isAdvancindEnabled
		{
			get
			{
				return BattleTaskManager.GetBattleManager().Ships_f[0].DmgStateEnd != DamageState_Battle.Taiha;
			}
		}

		public static ProdAdvancingWithDrawalSelect Instantiate(ProdAdvancingWithDrawalSelect prefab, Transform parent, Generics.BattleRootType iType)
		{
			ProdAdvancingWithDrawalSelect prodAdvancingWithDrawalSelect = Object.Instantiate<ProdAdvancingWithDrawalSelect>(prefab);
			prodAdvancingWithDrawalSelect.get_transform().set_parent(parent);
			prodAdvancingWithDrawalSelect.get_transform().localScaleZero();
			prodAdvancingWithDrawalSelect.get_transform().localPositionZero();
			prodAdvancingWithDrawalSelect._iRootType = iType;
			prodAdvancingWithDrawalSelect.Init();
			return prodAdvancingWithDrawalSelect;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelListSafe<UIAdvancingWithDrawalButton>(ref this._listHexExBtns);
			Mem.DelListSafe<Vector3>(ref this._listHexExBtnsPos4Sortie);
			Mem.DelListSafe<Vector3>(ref this._listHexExBtnsPos4Rebellion);
			Mem.Del<UIFleetInfos>(ref this._uiFleetInfos);
			Mem.Del<int>(ref this._nToggleGroup);
			Mem.Del<int>(ref this._nSetHexBtnsPos4Sortie);
			Mem.Del<int>(ref this._nSetHexBtnsPos4Rebellion);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<bool>(ref this._isDecide);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.DelListSafe<bool>(ref this._listEnabledBtn);
			Mem.Del<Generics.BattleRootType>(ref this._iRootType);
			Mem.Del<DelDecideHexButtonEx>(ref this._delDecideAdvancingWithdrawalButton);
		}

		private bool Init()
		{
			this.panel.depth = 70;
			this._nIndex = 0;
			this._listEnabledBtn = new List<bool>();
			if (this._listHexExBtns == null)
			{
				this._listHexExBtns = new List<UIAdvancingWithDrawalButton>();
			}
			int cnt = 0;
			this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.Init(cnt, false, 0, delegate
				{
					this.DecideAdvancingWithDrawalBtn(x);
				});
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", x.index);
				this._listEnabledBtn.Add(true);
				cnt++;
			});
			if (this._iRootType == Generics.BattleRootType.Rebellion)
			{
				this.SetHexBtnsPos4Rebellion();
			}
			else
			{
				this.SetHexBtnsPos4Sortie();
			}
			this._uiFleetInfos.Init(new List<ShipModel_BattleAll>(BattleTaskManager.GetBattleManager().Ships_f));
			this._uiFleetInfos.widget.alpha = 0f;
			return true;
		}

		private void SetHexBtnsPos4Sortie()
		{
			int cnt = 0;
			this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.get_transform().set_localPosition(this._listHexExBtnsPos4Sortie.get_Item(cnt));
				cnt++;
			});
		}

		private void SetHexBtnsPos4Rebellion()
		{
			int cnt = 0;
			this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.get_transform().set_localPosition(this._listHexExBtnsPos4Rebellion.get_Item(cnt));
				cnt++;
			});
		}

		public void Play(DelDecideHexButtonEx decideCallback)
		{
			base.Init();
			this._delDecideAdvancingWithdrawalButton = decideCallback;
			base.get_transform().localScaleOne();
			if (this._iRootType == Generics.BattleRootType.SortieMap)
			{
				this.ShowHexButtons2Sortie();
			}
			else
			{
				this.ShowHexButtons2Rebellion();
			}
			this._uiFleetInfos.Show();
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInAdvancingWithDrawal();
			battleNavigation.Show(0.2f, null);
			battleNavigation.panel.depth = this.panel.depth + 1;
		}

		private void ShowHexButtons2Sortie()
		{
			this._listEnabledBtn.set_Item(2, false);
			this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.SetActive(true);
				x.Play(delegate
				{
					if (x.index == 0)
					{
						this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton y)
						{
							y.toggle.group = 10;
						});
						KeyControl keyControl = BattleTaskManager.GetKeyControl();
						keyControl.reset(0, this.maxIndex, 0.4f, 0.1f);
						keyControl.setChangeValue(0f, -1f, 0f, 1f);
						keyControl.Index = 0;
						x.isFocus = true;
						keyControl.isLoopIndex = false;
						this._isInputPossible = true;
					}
					else
					{
						x.isFocus = false;
					}
					if (x.index == 2)
					{
						x.isColliderEnabled = this.isAdvancindPrimaryEnabled;
					}
					x.isColliderEnabled = true;
				});
			});
		}

		private void ShowHexButtons2Rebellion()
		{
			this._listEnabledBtn.set_Item(1, this.isAdvancindEnabled);
			this._listEnabledBtn.set_Item(2, this.isAdvancindPrimaryEnabled);
			this._listHexExBtns.get_Item(0).SetActive(true);
			this._listHexExBtns.get_Item(0).Play(null);
			this._listHexExBtns.get_Item(1).SetActive(true);
			this._listHexExBtns.get_Item(1).Play(delegate
			{
				this._listHexExBtns.get_Item(0).isFocus = true;
				this._listHexExBtns.get_Item(1).isFocus = false;
				this._listHexExBtns.get_Item(2).SetActive(true);
				this._listHexExBtns.get_Item(2).Play(delegate
				{
					this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
					{
						x.isFocus = false;
						x.toggle.group = 10;
					});
					KeyControl keyControl = BattleTaskManager.GetKeyControl();
					keyControl.reset(0, this.maxIndex, 0.4f, 0.1f);
					keyControl.setChangeValue(0f, -1f, 0f, 1f);
					keyControl.Index = 0;
					keyControl.isLoopIndex = false;
					this._isInputPossible = true;
					this._listHexExBtns.get_Item(0).isFocus = true;
					this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
					{
						x.isColliderEnabled = this._listEnabledBtn.get_Item(x.index);
					});
				});
			});
		}

		public bool Run()
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (!this._isDecide && this._isInputPossible)
			{
				if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					this.PreparaNext(false);
				}
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					this.PreparaNext(true);
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					this._listHexExBtns.get_Item(this._nIndex).OnDecide();
					return false;
				}
			}
			return !this._isDecide;
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = this._nIndex;
			this._nIndex = Mathe.NextElement(this._nIndex, 0, this.maxIndex, isFoward, (int x) => this._listEnabledBtn.get_Item(x));
			if (nIndex != this._nIndex)
			{
				this.ChangeFocus(this._nIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.isFocus = (x.index == nIndex);
			});
		}

		private void OnActive(int nIndex)
		{
			if (this._nIndex != nIndex)
			{
				this._nIndex = nIndex;
				this.ChangeFocus(this._nIndex);
			}
		}

		private void DecideAdvancingWithDrawalBtn(UIHexButtonEx btn)
		{
			if (this._isDecide)
			{
				return;
			}
			this._isDecide = true;
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			keyControl.Index = btn.index;
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
			this._listHexExBtns.ForEach(delegate(UIAdvancingWithDrawalButton x)
			{
				x.isColliderEnabled = false;
			});
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.Hide(0.2f, null);
			if (this._delDecideAdvancingWithdrawalButton != null)
			{
				this._delDecideAdvancingWithdrawalButton(btn);
			}
		}
	}
}
