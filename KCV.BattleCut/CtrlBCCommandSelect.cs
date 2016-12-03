using Common.Enum;
using local.managers;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlBCCommandSelect : MonoBehaviour
	{
		public enum CtrlMode
		{
			Surface,
			Command
		}

		[SerializeField]
		private Transform _prefabUICommandUnitSelect;

		[SerializeField]
		private UICommandSurfaceList _uiCommandSurfaceList;

		[SerializeField]
		private ProdBCBattle.FleetInfos _uiEnemyFleetInfos;

		private bool _isInputPossible;

		private CommandPhaseModel _clsCommandModel;

		private UICommandUnitSelect _uiCommandUnitSelect;

		private UIPanel _uiPanel;

		private CtrlBCCommandSelect.CtrlMode _iCtrlModel;

		private StatementMachine _clsState;

		private List<BattleCommand> _listInvalidCommands;

		private Action _actOnFinished;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static CtrlBCCommandSelect Instantiate(CtrlBCCommandSelect prefab, Transform parent, CommandPhaseModel model)
		{
			CtrlBCCommandSelect ctrlBCCommandSelect = Object.Instantiate<CtrlBCCommandSelect>(prefab);
			ctrlBCCommandSelect.get_transform().set_parent(parent);
			ctrlBCCommandSelect.get_transform().localPositionZero();
			ctrlBCCommandSelect.get_transform().localScaleOne();
			ctrlBCCommandSelect.Init(model);
			return ctrlBCCommandSelect;
		}

		private void OnDestroy()
		{
			Mem.Del<UICommandSurfaceList>(ref this._uiCommandSurfaceList);
			Mem.Del<CommandPhaseModel>(ref this._clsCommandModel);
			Mem.Del<Action>(ref this._actOnFinished);
		}

		private bool Init(CommandPhaseModel model)
		{
			this._clsState = new StatementMachine();
			this._listInvalidCommands = this.GetInvalidCommands(model.GetSelectableCommands());
			this._clsCommandModel = model;
			this._iCtrlModel = CtrlBCCommandSelect.CtrlMode.Surface;
			this._isInputPossible = false;
			this._uiCommandSurfaceList.Init(model.GetPresetCommand(), new Action<UICommandLabelButton>(this.OnSelectedSurface), new Predicate<List<BattleCommand>>(this.OnStartBattle));
			this._uiCommandSurfaceList.isColliderEnabled = false;
			this._uiCommandUnitSelect = UICommandUnitSelect.Instantiate(this._prefabUICommandUnitSelect.GetComponent<UICommandUnitSelect>(), base.get_transform(), model.GetSelectableCommands(), new Action<BattleCommand>(this.OnDecideUnitSelect), new Action(this.OnCancelUnitSelect));
			this.InitEnemyFleetInfos();
			this.panel.alpha = 0f;
			this.panel.widgetsAreStatic = true;
			return true;
		}

		private void InitEnemyFleetInfos()
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			BattleData battleData = new BattleData();
			IEnumerable<ShipModel_BattleAll> enumerable = Enumerable.Where<ShipModel_BattleAll>(battleManager.Ships_e, (ShipModel_BattleAll x) => x != null);
			int maxHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable, (ShipModel_BattleAll x) => x.HpStart));
			int num = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable, (ShipModel_BattleAll x) => x.HpPhaseStart));
			this._uiEnemyFleetInfos.circleGauge.SetHPGauge(maxHP, num, num);
			for (int i = 0; i < 6; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = battleManager.Ships_e[i];
				this._uiEnemyFleetInfos.hpBars.Add(this._uiEnemyFleetInfos.shipHPBarAnchor.Find("EnemyHPBar" + (i + 1)).GetComponent<BtlCut_HPBar>());
				if (shipModel_BattleAll != null)
				{
					this._uiEnemyFleetInfos.hpBars.get_Item(i).SetHpBar(shipModel_BattleAll);
				}
				else
				{
					this._uiEnemyFleetInfos.hpBars.get_Item(i).Hide();
				}
			}
		}

		private List<BattleCommand> GetInvalidCommands(HashSet<BattleCommand> validCommands)
		{
			List<BattleCommand> list = new List<BattleCommand>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(BattleCommand)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleCommand battleCommand = (BattleCommand)((int)enumerator.get_Current());
					if (battleCommand != BattleCommand.None)
					{
						if (!validCommands.Contains(battleCommand))
						{
							list.Add(battleCommand);
						}
					}
				}
			}
			return list;
		}

		public void Play(Action onFinished)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInCommand(this._iCtrlModel);
			navigation.Show(Defines.PHASE_FADE_TIME, null);
			this._actOnFinished = onFinished;
			BattleCutManager.SetTitleText(BattleCutPhase.Command);
			this.Show(delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSurfaceSelect), new StatementMachine.StatementMachineUpdate(this.UpdateSurfaceSelect));
			});
		}

		public bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return false;
		}

		private bool InitSurfaceSelect(object data)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInCommand(CtrlBCCommandSelect.CtrlMode.Surface);
			this._isInputPossible = true;
			this._uiCommandSurfaceList.isColliderEnabled = true;
			return false;
		}

		private bool UpdateSurfaceSelect(object data)
		{
			if (!this._isInputPossible)
			{
				return false;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.UP))
			{
				this._uiCommandSurfaceList.Prev();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
			{
				this._uiCommandSurfaceList.Next();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				this._uiCommandSurfaceList.RemoveUnit();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.SHIKAKU))
			{
				this._uiCommandSurfaceList.RemoveUnitAll();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this._uiCommandSurfaceList.OnSelectSurface();
			}
			return false;
		}

		private bool InitUnitSelect(object data)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInCommand(CtrlBCCommandSelect.CtrlMode.Command);
			this._isInputPossible = true;
			return false;
		}

		private bool UpdateUnitSelect(object data)
		{
			if (!this._isInputPossible)
			{
				return false;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.UP))
			{
				this._uiCommandUnitSelect.Prev();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
			{
				this._uiCommandUnitSelect.Next();
			}
			else
			{
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					return this._uiCommandUnitSelect.OnDecide();
				}
				if (keyControl.GetDown(KeyControl.KeyName.BATU))
				{
					return this._uiCommandUnitSelect.OnCancel();
				}
			}
			return false;
		}

		private void Show(Action onFinished)
		{
			this.panel.widgetsAreStatic = false;
			this.panel.get_transform().LTCancel();
			this.panel.get_transform().LTValue(this.panel.alpha, 1f, Defines.PHASE_FADE_TIME).setDelay(0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void Hide(Action onFinished)
		{
			this.panel.get_transform().LTCancel();
			this.panel.get_transform().LTValue(this.panel.alpha, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				this.panel.widgetsAreStatic = true;
				Dlg.Call(ref onFinished);
			});
		}

		private void OnSelectedSurface(UICommandLabelButton selectedButton)
		{
			this._clsState.Clear();
			this._isInputPossible = false;
			BattleCommand iCommand = (!this._listInvalidCommands.Contains(selectedButton.battleCommand)) ? selectedButton.battleCommand : BattleCommand.Sekkin;
			this._uiCommandUnitSelect.Show(iCommand, delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitUnitSelect), new StatementMachine.StatementMachineUpdate(this.UpdateUnitSelect));
			});
		}

		private void OnDecideUnitSelect(BattleCommand iCommand)
		{
			this._uiCommandSurfaceList.selectedSurface.SetCommand(iCommand);
			this._clsState.Clear();
			this._uiCommandUnitSelect.Hide(delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSurfaceSelect), new StatementMachine.StatementMachineUpdate(this.UpdateSurfaceSelect));
			});
		}

		private void OnCancelUnitSelect()
		{
			this._clsState.Clear();
			this._uiCommandUnitSelect.Hide(delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSurfaceSelect), new StatementMachine.StatementMachineUpdate(this.UpdateSurfaceSelect));
			});
		}

		private bool OnStartBattle(List<BattleCommand> commands)
		{
			if (!this._clsCommandModel.SetCommand(commands))
			{
				return false;
			}
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			this._clsState.Clear();
			this._isInputPossible = false;
			this.Hide(delegate
			{
				Dlg.Call(ref this._actOnFinished);
			});
			return true;
		}
	}
}
