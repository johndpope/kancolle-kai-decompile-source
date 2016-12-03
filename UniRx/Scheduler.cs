using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using UniRx.InternalUtil;

namespace UniRx
{
	public static class Scheduler
	{
		private class CurrentThreadScheduler : IScheduler
		{
			private static class Trampoline
			{
				public static void Run(SchedulerQueue queue)
				{
					while (queue.Count > 0)
					{
						ScheduledItem scheduledItem = queue.Dequeue();
						if (!scheduledItem.IsCanceled)
						{
							TimeSpan timeSpan = scheduledItem.DueTime - Scheduler.CurrentThreadScheduler.Time;
							if (timeSpan.get_Ticks() > 0L)
							{
								Thread.Sleep(timeSpan);
							}
							if (!scheduledItem.IsCanceled)
							{
								scheduledItem.Invoke();
							}
						}
					}
				}
			}

			[ThreadStatic]
			private static SchedulerQueue s_threadLocalQueue;

			[ThreadStatic]
			private static Stopwatch s_clock;

			private static TimeSpan Time
			{
				get
				{
					if (Scheduler.CurrentThreadScheduler.s_clock == null)
					{
						Scheduler.CurrentThreadScheduler.s_clock = Stopwatch.StartNew();
					}
					return Scheduler.CurrentThreadScheduler.s_clock.get_Elapsed();
				}
			}

			[EditorBrowsable]
			public static bool IsScheduleRequired
			{
				get
				{
					return Scheduler.CurrentThreadScheduler.GetQueue() == null;
				}
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			private static SchedulerQueue GetQueue()
			{
				return Scheduler.CurrentThreadScheduler.s_threadLocalQueue;
			}

			private static void SetQueue(SchedulerQueue newQueue)
			{
				Scheduler.CurrentThreadScheduler.s_threadLocalQueue = newQueue;
			}

			public IDisposable Schedule(Action action)
			{
				return this.Schedule(TimeSpan.Zero, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				TimeSpan dueTime2 = Scheduler.CurrentThreadScheduler.Time + Scheduler.Normalize(dueTime);
				ScheduledItem scheduledItem = new ScheduledItem(action, dueTime2);
				SchedulerQueue schedulerQueue = Scheduler.CurrentThreadScheduler.GetQueue();
				if (schedulerQueue == null)
				{
					schedulerQueue = new SchedulerQueue(4);
					schedulerQueue.Enqueue(scheduledItem);
					Scheduler.CurrentThreadScheduler.SetQueue(schedulerQueue);
					try
					{
						Scheduler.CurrentThreadScheduler.Trampoline.Run(schedulerQueue);
					}
					finally
					{
						Scheduler.CurrentThreadScheduler.SetQueue(null);
					}
				}
				else
				{
					schedulerQueue.Enqueue(scheduledItem);
				}
				return scheduledItem.Cancellation;
			}
		}

		private class ImmediateScheduler : IScheduler
		{
			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IDisposable Schedule(Action action)
			{
				action.Invoke();
				return Disposable.Empty;
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				TimeSpan timeSpan = Scheduler.Normalize(dueTime);
				if (timeSpan.get_Ticks() > 0L)
				{
					Thread.Sleep(timeSpan);
				}
				action.Invoke();
				return Disposable.Empty;
			}
		}

		public static class DefaultSchedulers
		{
			private static IScheduler constantTime;

			private static IScheduler tailRecursion;

			private static IScheduler iteration;

			private static IScheduler timeBasedOperations;

			private static IScheduler asyncConversions;

			public static IScheduler ConstantTimeOperations
			{
				get
				{
					IScheduler arg_17_0;
					if ((arg_17_0 = Scheduler.DefaultSchedulers.constantTime) == null)
					{
						arg_17_0 = (Scheduler.DefaultSchedulers.constantTime = Scheduler.Immediate);
					}
					return arg_17_0;
				}
				set
				{
					Scheduler.DefaultSchedulers.constantTime = value;
				}
			}

			public static IScheduler TailRecursion
			{
				get
				{
					IScheduler arg_17_0;
					if ((arg_17_0 = Scheduler.DefaultSchedulers.tailRecursion) == null)
					{
						arg_17_0 = (Scheduler.DefaultSchedulers.tailRecursion = Scheduler.Immediate);
					}
					return arg_17_0;
				}
				set
				{
					Scheduler.DefaultSchedulers.tailRecursion = value;
				}
			}

			public static IScheduler Iteration
			{
				get
				{
					IScheduler arg_17_0;
					if ((arg_17_0 = Scheduler.DefaultSchedulers.iteration) == null)
					{
						arg_17_0 = (Scheduler.DefaultSchedulers.iteration = Scheduler.CurrentThread);
					}
					return arg_17_0;
				}
				set
				{
					Scheduler.DefaultSchedulers.iteration = value;
				}
			}

			public static IScheduler TimeBasedOperations
			{
				get
				{
					IScheduler arg_17_0;
					if ((arg_17_0 = Scheduler.DefaultSchedulers.timeBasedOperations) == null)
					{
						arg_17_0 = (Scheduler.DefaultSchedulers.timeBasedOperations = Scheduler.MainThread);
					}
					return arg_17_0;
				}
				set
				{
					Scheduler.DefaultSchedulers.timeBasedOperations = value;
				}
			}

			public static IScheduler AsyncConversions
			{
				get
				{
					IScheduler arg_17_0;
					if ((arg_17_0 = Scheduler.DefaultSchedulers.asyncConversions) == null)
					{
						arg_17_0 = (Scheduler.DefaultSchedulers.asyncConversions = Scheduler.ThreadPool);
					}
					return arg_17_0;
				}
				set
				{
					Scheduler.DefaultSchedulers.asyncConversions = value;
				}
			}

			public static void SetDefaultForUnity()
			{
				Scheduler.DefaultSchedulers.ConstantTimeOperations = Scheduler.Immediate;
				Scheduler.DefaultSchedulers.TailRecursion = Scheduler.Immediate;
				Scheduler.DefaultSchedulers.Iteration = Scheduler.CurrentThread;
				Scheduler.DefaultSchedulers.TimeBasedOperations = Scheduler.MainThread;
				Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.ThreadPool;
			}

			public static void SetDotNetCompatible()
			{
				Scheduler.DefaultSchedulers.ConstantTimeOperations = Scheduler.Immediate;
				Scheduler.DefaultSchedulers.TailRecursion = Scheduler.Immediate;
				Scheduler.DefaultSchedulers.Iteration = Scheduler.CurrentThread;
				Scheduler.DefaultSchedulers.TimeBasedOperations = Scheduler.ThreadPool;
				Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.ThreadPool;
			}
		}

		private class ThreadPoolScheduler : IScheduler
		{
			private sealed class Timer : IDisposable
			{
				private static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();

				private readonly SingleAssignmentDisposable _disposable;

				private Action _action;

				private System.Threading.Timer _timer;

				private bool _hasAdded;

				private bool _hasRemoved;

				public Timer(TimeSpan dueTime, Action action)
				{
					this._disposable = new SingleAssignmentDisposable();
					this._disposable.Disposable = Disposable.Create(new Action(this.Unroot));
					this._action = action;
					this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), null, dueTime, TimeSpan.FromMilliseconds(-1.0));
					HashSet<System.Threading.Timer> hashSet = Scheduler.ThreadPoolScheduler.Timer.s_timers;
					lock (hashSet)
					{
						if (!this._hasRemoved)
						{
							Scheduler.ThreadPoolScheduler.Timer.s_timers.Add(this._timer);
							this._hasAdded = true;
						}
					}
				}

				private void Tick(object state)
				{
					try
					{
						if (!this._disposable.IsDisposed)
						{
							this._action.Invoke();
						}
					}
					finally
					{
						this.Unroot();
					}
				}

				private void Unroot()
				{
					this._action = delegate
					{
					};
					System.Threading.Timer timer = null;
					HashSet<System.Threading.Timer> hashSet = Scheduler.ThreadPoolScheduler.Timer.s_timers;
					lock (hashSet)
					{
						if (!this._hasRemoved)
						{
							timer = this._timer;
							this._timer = null;
							if (this._hasAdded && timer != null)
							{
								Scheduler.ThreadPoolScheduler.Timer.s_timers.Remove(timer);
							}
							this._hasRemoved = true;
						}
					}
					if (timer != null)
					{
						timer.Dispose();
					}
				}

				public void Dispose()
				{
					this._disposable.Dispose();
				}
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				System.Threading.ThreadPool.QueueUserWorkItem(delegate(object _)
				{
					if (!d.IsDisposed)
					{
						action.Invoke();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				return new Scheduler.ThreadPoolScheduler.Timer(dueTime, action);
			}
		}

		private class MainThreadScheduler : IScheduler
		{
			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public MainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
			}

			[DebuggerHidden]
			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				Scheduler.MainThreadScheduler.<DelayAction>c__Iterator17 <DelayAction>c__Iterator = new Scheduler.MainThreadScheduler.<DelayAction>c__Iterator17();
				<DelayAction>c__Iterator.dueTime = dueTime;
				<DelayAction>c__Iterator.cancellation = cancellation;
				<DelayAction>c__Iterator.action = action;
				<DelayAction>c__Iterator.<$>dueTime = dueTime;
				<DelayAction>c__Iterator.<$>cancellation = cancellation;
				<DelayAction>c__Iterator.<$>action = action;
				return <DelayAction>c__Iterator;
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				MainThreadDispatcher.Post(delegate
				{
					if (!d.IsDisposed)
					{
						action.Invoke();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				MainThreadDispatcher.SendStartCoroutine(this.DelayAction(dueTime2, delegate
				{
					if (!d.IsDisposed)
					{
						action.Invoke();
					}
				}, d));
				return d;
			}
		}

		private class IgnoreTimeScaleMainThreadScheduler : IScheduler
		{
			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IgnoreTimeScaleMainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
			}

			[DebuggerHidden]
			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				Scheduler.IgnoreTimeScaleMainThreadScheduler.<DelayAction>c__Iterator18 <DelayAction>c__Iterator = new Scheduler.IgnoreTimeScaleMainThreadScheduler.<DelayAction>c__Iterator18();
				<DelayAction>c__Iterator.dueTime = dueTime;
				<DelayAction>c__Iterator.cancellation = cancellation;
				<DelayAction>c__Iterator.action = action;
				<DelayAction>c__Iterator.<$>dueTime = dueTime;
				<DelayAction>c__Iterator.<$>cancellation = cancellation;
				<DelayAction>c__Iterator.<$>action = action;
				return <DelayAction>c__Iterator;
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				MainThreadDispatcher.Post(delegate
				{
					if (!d.IsDisposed)
					{
						action.Invoke();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				MainThreadDispatcher.SendStartCoroutine(this.DelayAction(dueTime2, delegate
				{
					if (!d.IsDisposed)
					{
						action.Invoke();
					}
				}, d));
				return d;
			}
		}

		public static readonly IScheduler CurrentThread = new Scheduler.CurrentThreadScheduler();

		public static readonly IScheduler Immediate = new Scheduler.ImmediateScheduler();

		public static readonly IScheduler ThreadPool = new Scheduler.ThreadPoolScheduler();

		private static IScheduler mainThread;

		private static IScheduler mainThreadIgnoreTimeScale;

		public static bool IsCurrentThreadSchedulerScheduleRequired
		{
			get
			{
				return Scheduler.CurrentThreadScheduler.IsScheduleRequired;
			}
		}

		public static DateTimeOffset Now
		{
			get
			{
				return DateTimeOffset.get_UtcNow();
			}
		}

		public static IScheduler MainThread
		{
			get
			{
				IScheduler arg_17_0;
				if ((arg_17_0 = Scheduler.mainThread) == null)
				{
					arg_17_0 = (Scheduler.mainThread = new Scheduler.MainThreadScheduler());
				}
				return arg_17_0;
			}
		}

		public static IScheduler MainThreadIgnoreTimeScale
		{
			get
			{
				IScheduler arg_17_0;
				if ((arg_17_0 = Scheduler.mainThreadIgnoreTimeScale) == null)
				{
					arg_17_0 = (Scheduler.mainThreadIgnoreTimeScale = new Scheduler.IgnoreTimeScaleMainThreadScheduler());
				}
				return arg_17_0;
			}
		}

		public static TimeSpan Normalize(TimeSpan timeSpan)
		{
			return (!(timeSpan >= TimeSpan.Zero)) ? TimeSpan.Zero : timeSpan;
		}

		public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
		{
			return scheduler.Schedule(dueTime - scheduler.Now, action);
		}

		public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate
			{
				action.Invoke(delegate
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					object gate;
					d = scheduler.Schedule(delegate
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction.Invoke();
					});
					gate = gate;
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(recursiveAction));
			return group;
		}

		public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate
			{
				action.Invoke(delegate(TimeSpan dt)
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					object gate;
					d = scheduler.Schedule(dt, delegate
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction.Invoke();
					});
					gate = gate;
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(dueTime, recursiveAction));
			return group;
		}

		public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate
			{
				action.Invoke(delegate(DateTimeOffset dt)
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					object gate;
					d = scheduler.Schedule(dt, delegate
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction.Invoke();
					});
					gate = gate;
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(dueTime, recursiveAction));
			return group;
		}
	}
}
