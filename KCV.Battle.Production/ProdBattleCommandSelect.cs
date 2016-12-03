using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBattleCommandSelect : MonoBehaviour
	{
		[SerializeField]
		private UICommandBox _uiCommandBox;

		[SerializeField]
		private UICommandUnitList _uiCommandUnitList;

		[SerializeField]
		private UIPanel _uiOverlay;

		[SerializeField]
		private UITexture _uiBlur;

		private Action _actOnFinished;

		private CommandPhaseModel _clsCommandModel;

		private InputMode _iInputMode;

		private BattleCommandMode _iCommandMode;

		private List<BattleCommand> _listInvalidCommands;

		private StatementMachine _clsState;

		public UICommandBox commandBox
		{
			get
			{
				return this._uiCommandBox;
			}
		}

		public UICommandUnitList commandUnitList
		{
			get
			{
				return this._uiCommandUnitList;
			}
		}

		public InputMode inputMode
		{
			get
			{
				return this._iInputMode;
			}
			private set
			{
				this._iInputMode = value;
			}
		}

		public BattleCommandMode commandMode
		{
			get
			{
				return this._iCommandMode;
			}
			private set
			{
				this._iCommandMode = value;
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.SetNavigationInCommand(this._iCommandMode);
			}
		}

		public static ProdBattleCommandSelect Instantiate(ProdBattleCommandSelect prefab, Transform parent, CommandPhaseModel model)
		{
			ProdBattleCommandSelect prodBattleCommandSelect = Object.Instantiate<ProdBattleCommandSelect>(prefab);
			prodBattleCommandSelect.get_transform().set_parent(parent);
			prodBattleCommandSelect.get_transform().localScaleOne();
			prodBattleCommandSelect.get_transform().localPositionZero();
			prodBattleCommandSelect.Init(model);
			return prodBattleCommandSelect;
		}

		private void OnDestroy()
		{
			Mem.Del<UICommandBox>(ref this._uiCommandBox);
			Mem.Del<UICommandUnitList>(ref this._uiCommandUnitList);
			Mem.Del<UIPanel>(ref this._uiOverlay);
			Mem.Del<UITexture>(ref this._uiBlur);
			Mem.Del<Action>(ref this._actOnFinished);
			Mem.Del<CommandPhaseModel>(ref this._clsCommandModel);
			Mem.Del<InputMode>(ref this._iInputMode);
			Mem.Del<BattleCommandMode>(ref this._iCommandMode);
			Mem.DelListSafe<BattleCommand>(ref this._listInvalidCommands);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
		}

		private bool Init(CommandPhaseModel model)
		{
			this._listInvalidCommands = this.GetInvalidCommands(model.GetSelectableCommands());
			this._clsCommandModel = model;
			this._iInputMode = InputMode.Key;
			this._uiCommandBox.Init(model, new Predicate<List<BattleCommand>>(this.OnStartBattle));
			this._uiCommandBox.isColliderEnabled = false;
			this._uiCommandUnitList.Init(model, new Action(this.OnUnitListDnDRelease));
			this._uiCommandUnitList.isColliderEnabled = false;
			this._uiOverlay.alpha = 0f;
			this._uiBlur.set_enabled(false);
			this.commandMode = BattleCommandMode.SurfaceBox;
			this._clsState = new StatementMachine();
			return true;
		}

		private void InitCommandBackground()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			BattleTaskManager.GetPrefabFile().DisposeProdCloud();
			battleField.dicFleetAnchor.get_Item(FleetType.Friend).set_position(Vector3.get_forward() * 100f);
			battleField.dicFleetAnchor.get_Item(FleetType.Enemy).set_position(Vector3.get_back() * 100f);
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.RotateAroundObject);
			battleFieldCamera.SetRotateAroundObjectCamera(battleField.fieldCenter.get_position(), 200f, -9.5f);
			battleFieldCamera.get_transform().LTMoveY(15.51957f, 0.01f).setEase(LeanTweenType.easeOutQuart);
			this._uiBlur.set_enabled(true);
			battleShips.RadarDeployment(true);
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

		[DebuggerHidden]
		public IEnumerator PlayShowAnimation(Action onFinished)
		{
			ProdBattleCommandSelect.<PlayShowAnimation>c__IteratorDE <PlayShowAnimation>c__IteratorDE = new ProdBattleCommandSelect.<PlayShowAnimation>c__IteratorDE();
			<PlayShowAnimation>c__IteratorDE.onFinished = onFinished;
			<PlayShowAnimation>c__IteratorDE.<$>onFinished = onFinished;
			<PlayShowAnimation>c__IteratorDE.<>f__this = this;
			return <PlayShowAnimation>c__IteratorDE;
		}

		public LTDescr DiscardAfterFadeIn()
		{
			this._uiCommandBox.panel.alpha = 0f;
			this._uiCommandUnitList.panel.alpha = 0f;
			return this.FadeInOverlay();
		}

		private LTDescr FadeInOverlay()
		{
			this._uiOverlay.get_transform().LTCancel();
			return this._uiOverlay.get_transform().LTValue(this._uiOverlay.alpha, 0f, 1.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				this._uiOverlay.alpha = x;
			});
		}

		private LTDescr FadeOutOverlay()
		{
			this._uiOverlay.get_transform().LTCancel();
			return this._uiOverlay.get_transform().LTValue(this._uiOverlay.alpha, 1f, 0.35f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				this._uiOverlay.alpha = x;
			});
		}

		public bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return true;
		}

		private bool InitKeyMode(object data)
		{
			this.inputMode = InputMode.Key;
			this.commandMode = BattleCommandMode.SurfaceBox;
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = Generics.Layers.CutIn;
			this._uiCommandBox.SetBattleStartButtonLayer();
			this._uiCommandBox.FocusSurfaceMagnify();
			return false;
		}

		private bool UpdateKeyMode(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			for (int i = 0; i < this._uiCommandUnitList.listCommandUnits.get_Count(); i++)
			{
				this._uiCommandUnitList.listCommandUnits.get_Item(i).ResetPosition();
			}
			if (Input.get_touchCount() != 0 || Input.GetMouseButtonDown(0))
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitTouchMode), new StatementMachine.StatementMachineUpdate(this.UpdateTouchMode));
				return true;
			}
			BattleCommandMode commandMode = this.commandMode;
			if (commandMode != BattleCommandMode.SurfaceBox)
			{
				if (commandMode == BattleCommandMode.UnitList)
				{
					if (keyControl.GetDown(KeyControl.KeyName.UP))
					{
						this._uiCommandUnitList.PrevColumn();
					}
					else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
					{
						this._uiCommandUnitList.NextColumn();
					}
					else if (keyControl.GetDown(KeyControl.KeyName.LEFT))
					{
						this._uiCommandUnitList.PrevLine();
					}
					else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
					{
						this._uiCommandUnitList.NextLine();
					}
					else
					{
						if (keyControl.GetDown(KeyControl.KeyName.MARU))
						{
							return this.OnKeyModeDecideUnit();
						}
						if (keyControl.GetDown(KeyControl.KeyName.BATU))
						{
							return this.OnKeyModeCancelUnit();
						}
					}
				}
			}
			else if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				this._uiCommandBox.Prev();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				this._uiCommandBox.Next();
			}
			else if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				this._uiCommandBox.RemoveCommandUnit2FocusSurface();
			}
			else
			{
				if (keyControl.GetDown(KeyControl.KeyName.SHIKAKU))
				{
					this._uiCommandBox.RemoveCommandUnitAll();
					return false;
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					return this.OnKeyModeDecideSurface();
				}
			}
			return false;
		}

		private bool OnKeyModeDecideSurface()
		{
			if (this._uiCommandBox.isSelectBattleStart)
			{
				return this._uiCommandBox.DecideStartBattle();
			}
			this._uiCommandUnitList.Active2FocusUnit2(this._uiCommandBox.focusSurface, this._listInvalidCommands);
			this.commandMode = BattleCommandMode.UnitList;
			return false;
		}

		private bool OnKeyModeDecideUnit()
		{
			if (!this._uiCommandUnitList.focusUnitIcon.isValid)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				return false;
			}
			this._uiCommandBox.AbsodedUnitIcon2FocusSurface();
			this._uiCommandBox.SetFocusUnitIcon2FocusSurface();
			this._uiCommandUnitList.Reset2Unit();
			this._uiCommandUnitList.ActiveAll2Unit(false);
			this._uiCommandBox.FocusSurfaceMagnify();
			this.commandMode = BattleCommandMode.SurfaceBox;
			return false;
		}

		private bool OnKeyModeCancelUnit()
		{
			this._uiCommandUnitList.ActiveAll2Unit(false);
			this.commandMode = BattleCommandMode.SurfaceBox;
			return false;
		}

		private bool InitTouchMode(object data)
		{
			this.inputMode = InputMode.Touch;
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			this._uiCommandBox.ReductionAll();
			this._uiCommandUnitList.ActiveAll2Unit(false);
			this._uiCommandUnitList.isColliderEnabled = true;
			return false;
		}

		private bool UpdateTouchMode(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			return keyControl.IsAnyKey && this.OnTouchModeFinished();
		}

		private bool OnTouchModeFinished()
		{
			this._uiCommandUnitList.Reset2Unit();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitKeyMode), new StatementMachine.StatementMachineUpdate(this.UpdateKeyMode));
			return true;
		}

		private void OnUnitListDnDRelease()
		{
			List<UICommandSurface> listCommandSurfaces = this._uiCommandBox.listCommandSurfaces;
			listCommandSurfaces.FindAll((UICommandSurface x) => !x.isAbsorded).ForEach(delegate(UICommandSurface x)
			{
				x.Reduction();
			});
			listCommandSurfaces.ForEach(delegate(UICommandSurface x)
			{
				x.isAbsorded = false;
			});
		}

		private bool OnStartBattle(List<BattleCommand> commands)
		{
			if (!this._clsCommandModel.SetCommand(commands))
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				return false;
			}
			this._clsState.Clear();
			this._uiCommandUnitList.isColliderEnabled = false;
			Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
			{
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.Hide();
				this.FadeOutOverlay().setOnComplete(delegate
				{
					this._uiBlur.set_enabled(false);
					Dlg.Call(ref this._actOnFinished);
				});
			});
			return true;
		}
	}
}
