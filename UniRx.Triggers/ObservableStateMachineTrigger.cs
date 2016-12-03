using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableStateMachineTrigger : StateMachineBehaviour
	{
		public class OnStateInfo
		{
			public Animator Animator
			{
				get;
				private set;
			}

			public AnimatorStateInfo StateInfo
			{
				get;
				private set;
			}

			public int LayerIndex
			{
				get;
				private set;
			}

			public OnStateInfo(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			{
				this.Animator = animator;
				this.StateInfo = stateInfo;
				this.LayerIndex = layerIndex;
			}
		}

		public class OnStateMachineInfo
		{
			public Animator Animator
			{
				get;
				private set;
			}

			public int StateMachinePathHash
			{
				get;
				private set;
			}

			public OnStateMachineInfo(Animator animator, int stateMachinePathHash)
			{
				this.Animator = animator;
				this.StateMachinePathHash = stateMachinePathHash;
			}
		}

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateExit;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateEnter;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateIK;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateMove;

		private Subject<ObservableStateMachineTrigger.OnStateInfo> onStateUpdate;

		private Subject<ObservableStateMachineTrigger.OnStateMachineInfo> onStateMachineEnter;

		private Subject<ObservableStateMachineTrigger.OnStateMachineInfo> onStateMachineExit;

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateExit != null)
			{
				this.onStateExit.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateExitAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateExit) == null)
			{
				arg_1B_0 = (this.onStateExit = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return arg_1B_0;
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateEnter != null)
			{
				this.onStateEnter.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateEnterAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateEnter) == null)
			{
				arg_1B_0 = (this.onStateEnter = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return arg_1B_0;
		}

		public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateIK != null)
			{
				this.onStateIK.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateIKAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateIK) == null)
			{
				arg_1B_0 = (this.onStateIK = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return arg_1B_0;
		}

		public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateMove != null)
			{
				this.onStateMove.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateMoveAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateMove) == null)
			{
				arg_1B_0 = (this.onStateMove = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return arg_1B_0;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.onStateUpdate != null)
			{
				this.onStateUpdate.OnNext(new ObservableStateMachineTrigger.OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateInfo> OnStateUpdateAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateUpdate) == null)
			{
				arg_1B_0 = (this.onStateUpdate = new Subject<ObservableStateMachineTrigger.OnStateInfo>());
			}
			return arg_1B_0;
		}

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			if (this.onStateMachineEnter != null)
			{
				this.onStateMachineEnter.OnNext(new ObservableStateMachineTrigger.OnStateMachineInfo(animator, stateMachinePathHash));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateMachineInfo> OnStateMachineEnterAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateMachineInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateMachineEnter) == null)
			{
				arg_1B_0 = (this.onStateMachineEnter = new Subject<ObservableStateMachineTrigger.OnStateMachineInfo>());
			}
			return arg_1B_0;
		}

		public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
		{
			if (this.onStateMachineExit != null)
			{
				this.onStateMachineExit.OnNext(new ObservableStateMachineTrigger.OnStateMachineInfo(animator, stateMachinePathHash));
			}
		}

		public IObservable<ObservableStateMachineTrigger.OnStateMachineInfo> OnStateMachineExitAsObservable()
		{
			Subject<ObservableStateMachineTrigger.OnStateMachineInfo> arg_1B_0;
			if ((arg_1B_0 = this.onStateMachineExit) == null)
			{
				arg_1B_0 = (this.onStateMachineExit = new Subject<ObservableStateMachineTrigger.OnStateMachineInfo>());
			}
			return arg_1B_0;
		}
	}
}
