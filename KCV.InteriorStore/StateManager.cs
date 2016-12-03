using System;
using System.Collections.Generic;

namespace KCV.InteriorStore
{
	internal class StateManager<State>
	{
		private Stack<State> mStateStack;

		private State mEmptyState;

		public Action<State> OnPush
		{
			private get;
			set;
		}

		public Action<State> OnPop
		{
			private get;
			set;
		}

		public Action<State> OnResume
		{
			private get;
			set;
		}

		public Action<State> OnSwitch
		{
			private get;
			set;
		}

		public State CurrentState
		{
			get
			{
				if (0 < this.mStateStack.get_Count())
				{
					return this.mStateStack.Peek();
				}
				return this.mEmptyState;
			}
		}

		public StateManager(State emptyState)
		{
			this.mEmptyState = emptyState;
			this.mStateStack = new Stack<State>();
		}

		public void PushState(State state)
		{
			this.mStateStack.Push(state);
			this.Notify(this.OnPush, this.mStateStack.Peek());
			this.Notify(this.OnSwitch, this.mStateStack.Peek());
		}

		public void ReplaceState(State state)
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.PopState();
			}
			this.mStateStack.Push(state);
			this.Notify(this.OnPush, this.mStateStack.Peek());
			this.Notify(this.OnSwitch, this.mStateStack.Peek());
		}

		public void PopState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				State state = this.mStateStack.Pop();
				this.Notify(this.OnPop, state);
			}
		}

		public void ResumeState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.Notify(this.OnResume, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}
		}

		public override string ToString()
		{
			this.mStateStack.ToArray();
			string text = string.Empty;
			using (Stack<State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					State current = enumerator.get_Current();
					text = current + " > " + text;
				}
			}
			return text;
		}

		private void Notify(Action<State> target, State state)
		{
			if (target != null)
			{
				target.Invoke(state);
			}
		}
	}
}
