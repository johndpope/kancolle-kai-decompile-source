using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class DayAnimation : MonoBehaviour
	{
		private enum AnimType
		{
			Day,
			Week,
			Month,
			Choco,
			EndDay
		}

		private Animation anim;

		[SerializeField]
		private Transform[] AnimationTypes;

		[SerializeField]
		private StrategyMonthWeekBonus MonthBonus;

		[SerializeField]
		private StrategyMonthWeekBonus WeekBonus;

		[SerializeField]
		private UILabel dayText;

		[SerializeField]
		private UILabel EndDayText;

		private KeyControl key;

		private void Awake()
		{
			this.anim = base.GetComponent<Animation>();
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			DayAnimation.<Start>c__Iterator170 <Start>c__Iterator = new DayAnimation.<Start>c__Iterator170();
			<Start>c__Iterator.<>f__this = this;
			return <Start>c__Iterator;
		}

		public void StartWait(string AnimName)
		{
			base.StartCoroutine(this.WaitForItemView(AnimName));
		}

		[DebuggerHidden]
		public IEnumerator WaitForItemView(string AnimName)
		{
			DayAnimation.<WaitForItemView>c__Iterator171 <WaitForItemView>c__Iterator = new DayAnimation.<WaitForItemView>c__Iterator171();
			<WaitForItemView>c__Iterator.AnimName = AnimName;
			<WaitForItemView>c__Iterator.<$>AnimName = AnimName;
			<WaitForItemView>c__Iterator.<>f__this = this;
			return <WaitForItemView>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator StartDayAnimation(StrategyMapManager LogicMng, bool isDebug)
		{
			DayAnimation.<StartDayAnimation>c__Iterator172 <StartDayAnimation>c__Iterator = new DayAnimation.<StartDayAnimation>c__Iterator172();
			<StartDayAnimation>c__Iterator.LogicMng = LogicMng;
			<StartDayAnimation>c__Iterator.isDebug = isDebug;
			<StartDayAnimation>c__Iterator.<$>LogicMng = LogicMng;
			<StartDayAnimation>c__Iterator.<$>isDebug = isDebug;
			<StartDayAnimation>c__Iterator.<>f__this = this;
			return <StartDayAnimation>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator StartMonthAnimation(StrategyMapManager LogicMng, UserPreActionPhaseResultModel userPreAction, bool isDebug)
		{
			DayAnimation.<StartMonthAnimation>c__Iterator173 <StartMonthAnimation>c__Iterator = new DayAnimation.<StartMonthAnimation>c__Iterator173();
			<StartMonthAnimation>c__Iterator.userPreAction = userPreAction;
			<StartMonthAnimation>c__Iterator.LogicMng = LogicMng;
			<StartMonthAnimation>c__Iterator.isDebug = isDebug;
			<StartMonthAnimation>c__Iterator.<$>userPreAction = userPreAction;
			<StartMonthAnimation>c__Iterator.<$>LogicMng = LogicMng;
			<StartMonthAnimation>c__Iterator.<$>isDebug = isDebug;
			<StartMonthAnimation>c__Iterator.<>f__this = this;
			return <StartMonthAnimation>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator StartWeekAnimation(StrategyMapManager LogicMng, UserPreActionPhaseResultModel userPreAction, bool isDebug)
		{
			DayAnimation.<StartWeekAnimation>c__Iterator174 <StartWeekAnimation>c__Iterator = new DayAnimation.<StartWeekAnimation>c__Iterator174();
			<StartWeekAnimation>c__Iterator.userPreAction = userPreAction;
			<StartWeekAnimation>c__Iterator.isDebug = isDebug;
			<StartWeekAnimation>c__Iterator.<$>userPreAction = userPreAction;
			<StartWeekAnimation>c__Iterator.<$>isDebug = isDebug;
			<StartWeekAnimation>c__Iterator.<>f__this = this;
			return <StartWeekAnimation>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator StartSendChocoAnimation(StrategyMapManager LogicMng, UserPreActionPhaseResultModel userPreAction, bool isDebug)
		{
			DayAnimation.<StartSendChocoAnimation>c__Iterator175 <StartSendChocoAnimation>c__Iterator = new DayAnimation.<StartSendChocoAnimation>c__Iterator175();
			<StartSendChocoAnimation>c__Iterator.userPreAction = userPreAction;
			<StartSendChocoAnimation>c__Iterator.isDebug = isDebug;
			<StartSendChocoAnimation>c__Iterator.<$>userPreAction = userPreAction;
			<StartSendChocoAnimation>c__Iterator.<$>isDebug = isDebug;
			<StartSendChocoAnimation>c__Iterator.<>f__this = this;
			return <StartSendChocoAnimation>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator EndDayAnimation(StrategyMapManager LogicMng, bool isDebug)
		{
			DayAnimation.<EndDayAnimation>c__Iterator176 <EndDayAnimation>c__Iterator = new DayAnimation.<EndDayAnimation>c__Iterator176();
			<EndDayAnimation>c__Iterator.LogicMng = LogicMng;
			<EndDayAnimation>c__Iterator.isDebug = isDebug;
			<EndDayAnimation>c__Iterator.<$>LogicMng = LogicMng;
			<EndDayAnimation>c__Iterator.<$>isDebug = isDebug;
			<EndDayAnimation>c__Iterator.<>f__this = this;
			return <EndDayAnimation>c__Iterator;
		}

		private bool CheckEndDay(int leaveTurn)
		{
			return leaveTurn <= 100;
		}

		private void SetActiveAnimType(DayAnimation.AnimType type)
		{
			Transform[] animationTypes = this.AnimationTypes;
			for (int i = 0; i < animationTypes.Length; i++)
			{
				Transform component = animationTypes[i];
				component.SetActive(false);
			}
			this.AnimationTypes[(int)type].SetActive(true);
		}

		private void OnDestroy()
		{
			this.anim = null;
			this.AnimationTypes = null;
			this.MonthBonus = null;
			this.WeekBonus = null;
			this.dayText = null;
			this.key = null;
		}
	}
}
