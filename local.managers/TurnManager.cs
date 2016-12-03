using Common.Enum;
using local.models;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class TurnManager : ManagerBase, ITurnOperator
	{
		public TurnManager()
		{
		}

		public virtual UserPreActionPhaseResultModel GetResult_UserPreActionPhase()
		{
			bool arg_10_1 = false;
			List<TurnState> list = new List<TurnState>();
			list.Add(TurnState.TURN_START);
			TurnWorkResult turnWorkResult = this._PhaseEnd(arg_10_1, list);
			if (turnWorkResult == null)
			{
				return null;
			}
			return new UserPreActionPhaseResultModel(turnWorkResult, this);
		}

		public virtual UserActionPhaseResultModel GetResult_UserActionPhase()
		{
			bool arg_17_1 = true;
			List<TurnState> list = new List<TurnState>();
			list.Add(TurnState.CONTINOUS);
			list.Add(TurnState.OWN_END);
			TurnWorkResult turnWorkResult = this._PhaseEnd(arg_17_1, list);
			return (turnWorkResult == null) ? null : new UserActionPhaseResultModel(turnWorkResult);
		}

		public EnemyPreActionPhaseResultModel GetResult_EnemyPreActionPhase()
		{
			bool arg_10_1 = false;
			List<TurnState> list = new List<TurnState>();
			list.Add(TurnState.ENEMY_START);
			TurnWorkResult turnWorkResult = this._PhaseEnd(arg_10_1, list);
			return (turnWorkResult == null) ? null : new EnemyPreActionPhaseResultModel(turnWorkResult);
		}

		public EnemyActionPhaseResultModel GetResult_EnemyActionPhase()
		{
			bool arg_10_1 = false;
			List<TurnState> list = new List<TurnState>();
			list.Add(TurnState.ENEMY_END);
			TurnWorkResult turnWorkResult = this._PhaseEnd(arg_10_1, list);
			return (turnWorkResult == null) ? null : new EnemyActionPhaseResultModel(turnWorkResult);
		}

		public TurnResultModel GetResult_Turn()
		{
			bool arg_13_1 = false;
			List<TurnState> list = new List<TurnState>();
			list.Add(TurnState.TURN_END);
			TurnWorkResult turnWorkResult = this._PhaseEnd(arg_13_1, list);
			if (turnWorkResult == null)
			{
				return null;
			}
			TurnResultModel turnResultModel = new TurnResultModel(turnWorkResult);
			if (turnResultModel.RadingResult == null)
			{
				return turnResultModel;
			}
			for (int i = 0; i < turnResultModel.RadingResult.get_Count(); i++)
			{
				RadingResultData radingResultData = turnResultModel.RadingResult.get_Item(i);
				if (radingResultData.RadingDamage != null)
				{
					List<int> list2 = radingResultData.RadingDamage.FindAll((RadingDamageData item) => item.DamageState == DamagedStates.Gekichin).ConvertAll<int>((RadingDamageData item) => item.Rid);
					if (list2.get_Count() > 0)
					{
						base.UserInfo.__RemoveGekichinShips__(list2);
						base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
						break;
					}
				}
			}
			return turnResultModel;
		}

		public TurnWorkResult ExecTurnStateChange()
		{
			return null;
		}

		public List<PhaseResultModel> DebugTurnEnd()
		{
			List<PhaseResultModel> list = new List<PhaseResultModel>();
			return this._DebugTurnEnd(list);
		}

		private List<PhaseResultModel> _DebugTurnEnd(List<PhaseResultModel> list)
		{
			PhaseResultModel phaseResultModel = null;
			if (base.TurnState == TurnState.TURN_START)
			{
				phaseResultModel = this.GetResult_UserPreActionPhase();
				if (phaseResultModel != null)
				{
					list.Add(phaseResultModel);
				}
				return list;
			}
			if (base.TurnState == TurnState.CONTINOUS || base.TurnState == TurnState.OWN_END)
			{
				phaseResultModel = this.GetResult_UserActionPhase();
			}
			else if (base.TurnState == TurnState.ENEMY_START)
			{
				phaseResultModel = this.GetResult_EnemyPreActionPhase();
			}
			else if (base.TurnState == TurnState.ENEMY_END)
			{
				phaseResultModel = this.GetResult_EnemyActionPhase();
			}
			else if (base.TurnState == TurnState.TURN_END)
			{
				phaseResultModel = this.GetResult_Turn();
			}
			if (phaseResultModel == null)
			{
				return list;
			}
			list.Add(phaseResultModel);
			return this._DebugTurnEnd(list);
		}

		private TurnWorkResult _PhaseEnd(bool force, List<TurnState> now_states)
		{
			if (!now_states.Contains(base.TurnState))
			{
				return null;
			}
			TurnWorkResult turnWorkResult = new Api_TurnOperator().ExecTurnStateChange(this, force, ManagerBase._turn_state);
			if (turnWorkResult != null)
			{
				ManagerBase._turn_state = turnWorkResult.ChangeState;
				return turnWorkResult;
			}
			return null;
		}

		public override string ToString()
		{
			string text = string.Format("{0}\n{1}\n", base.UserInfo, base.Material);
			text += string.Format("総ターン数:{0}\t日時:{1}", base.Turn, base.Datetime);
			text += string.Format("({0}年{1} {2}日 {3})\n", new object[]
			{
				base.DatetimeString.Year,
				base.DatetimeString.Month,
				base.DatetimeString.Day,
				base.DatetimeString.DayOfWeek
			});
			Mem_trophy user_trophy = Comm_UserDatas.Instance.User_trophy;
			text += string.Format("累計データ:[出撃-{0}, S勝利-{1}, 応急修理-{2}, 改修工廠-{3}\n", new object[]
			{
				user_trophy.Start_map_count,
				user_trophy.Win_S_count,
				user_trophy.Use_recovery_item_count,
				user_trophy.Revamp_count
			});
			return text + string.Format("{0}", base.Settings);
		}
	}
}
