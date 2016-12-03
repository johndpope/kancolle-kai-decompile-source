using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace UniRx
{
	public abstract class LazyTask
	{
		public enum TaskStatus
		{
			WaitingToRun,
			Running,
			Completed,
			Canceled,
			Faulted
		}

		protected readonly BooleanDisposable cancellation = new BooleanDisposable();

		public LazyTask.TaskStatus Status
		{
			get;
			protected set;
		}

		public abstract Coroutine Start();

		public void Cancel()
		{
			if (this.Status == LazyTask.TaskStatus.WaitingToRun || this.Status == LazyTask.TaskStatus.Running)
			{
				this.Status = LazyTask.TaskStatus.Canceled;
				this.cancellation.Dispose();
			}
		}

		public static LazyTask<T> FromResult<T>(T value)
		{
			return LazyTask<T>.FromResult(value);
		}

		public static Coroutine WhenAll(params LazyTask[] tasks)
		{
			return LazyTask.WhenAll(Enumerable.AsEnumerable<LazyTask>(tasks));
		}

		public static Coroutine WhenAll(IEnumerable<LazyTask> tasks)
		{
			Coroutine[] coroutines = Enumerable.ToArray<Coroutine>(Enumerable.Select<LazyTask, Coroutine>(tasks, (LazyTask x) => x.Start()));
			return MainThreadDispatcher.StartCoroutine(LazyTask.WhenAllCore(coroutines));
		}

		[DebuggerHidden]
		private static IEnumerator WhenAllCore(Coroutine[] coroutines)
		{
			LazyTask.<WhenAllCore>c__Iterator1C <WhenAllCore>c__Iterator1C = new LazyTask.<WhenAllCore>c__Iterator1C();
			<WhenAllCore>c__Iterator1C.coroutines = coroutines;
			<WhenAllCore>c__Iterator1C.<$>coroutines = coroutines;
			return <WhenAllCore>c__Iterator1C;
		}
	}
	public class LazyTask<T> : LazyTask
	{
		private readonly IObservable<T> source;

		private T result;

		public T Result
		{
			get
			{
				if (base.Status != LazyTask.TaskStatus.Completed)
				{
					throw new InvalidOperationException("Task is not completed");
				}
				return this.result;
			}
		}

		public Exception Exception
		{
			get;
			private set;
		}

		public LazyTask(IObservable<T> source)
		{
			this.source = source;
			base.Status = LazyTask.TaskStatus.WaitingToRun;
		}

		public override Coroutine Start()
		{
			if (base.Status != LazyTask.TaskStatus.WaitingToRun)
			{
				throw new InvalidOperationException("Task already started");
			}
			base.Status = LazyTask.TaskStatus.Running;
			return this.source.StartAsCoroutine(delegate(T x)
			{
				this.result = x;
				base.Status = LazyTask.TaskStatus.Completed;
			}, delegate(Exception ex)
			{
				this.Exception = ex;
				base.Status = LazyTask.TaskStatus.Faulted;
			}, new CancellationToken(this.cancellation));
		}

		public override string ToString()
		{
			switch (base.Status)
			{
			case LazyTask.TaskStatus.WaitingToRun:
				return "Status:WaitingToRun";
			case LazyTask.TaskStatus.Running:
				return "Status:Running";
			case LazyTask.TaskStatus.Completed:
			{
				string arg_4B_0 = "Status:Completed, Result:";
				T t = this.Result;
				return arg_4B_0 + t.ToString();
			}
			case LazyTask.TaskStatus.Canceled:
				return "Status:Canceled";
			case LazyTask.TaskStatus.Faulted:
			{
				string arg_70_0 = "Status:Faulted, Result:";
				T t2 = this.Result;
				return arg_70_0 + t2.ToString();
			}
			default:
				return string.Empty;
			}
		}

		public static LazyTask<T> FromResult(T value)
		{
			return new LazyTask<T>(null)
			{
				result = value,
				Status = LazyTask.TaskStatus.Completed
			};
		}
	}
}
