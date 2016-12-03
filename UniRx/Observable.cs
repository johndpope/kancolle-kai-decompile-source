using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UniRx.Triggers;
using UnityEngine;

namespace UniRx
{
	public static class Observable
	{
		private enum AmbState
		{
			Left,
			Right,
			Neither
		}

		private class AnonymousObservable<T> : IObservable<T>
		{
			private readonly Func<IObserver<T>, IDisposable> subscribe;

			public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
			{
				this.subscribe = subscribe;
			}

			public IDisposable Subscribe(IObserver<T> observer)
			{
				SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
				IObserver<T> safeObserver = Observer.Create<T>(new Action<T>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted), subscription);
				if (Scheduler.IsCurrentThreadSchedulerScheduleRequired)
				{
					Scheduler.CurrentThread.Schedule(delegate
					{
						subscription.Disposable = this.subscribe.Invoke(safeObserver);
					});
				}
				else
				{
					subscription.Disposable = this.subscribe.Invoke(safeObserver);
				}
				return subscription;
			}
		}

		private class ConnectableObservable<T> : IObservable<T>, IConnectableObservable<T>
		{
			private readonly IObservable<T> source;

			private readonly ISubject<T> subject;

			public ConnectableObservable(IObservable<T> source, ISubject<T> subject)
			{
				this.source = source;
				this.subject = subject;
			}

			public IDisposable Connect()
			{
				return this.source.Subscribe(this.subject);
			}

			public IDisposable Subscribe(IObserver<T> observer)
			{
				return this.subject.Subscribe(observer);
			}
		}

		private static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1);

		private static readonly HashSet<Type> YieldInstructionTypes;

		static Observable()
		{
			// Note: this type is marked as 'beforefieldinit'.
			HashSet<Type> hashSet = new HashSet<Type>();
			hashSet.Add(typeof(WWW));
			hashSet.Add(typeof(WaitForEndOfFrame));
			hashSet.Add(typeof(WaitForFixedUpdate));
			hashSet.Add(typeof(WaitForSeconds));
			hashSet.Add(typeof(Coroutine));
			Observable.YieldInstructionTypes = hashSet;
		}

		private static IObservable<T> AddRef<T>(IObservable<T> xs, RefCountDisposable r)
		{
			return Observable.Create<T>((IObserver<T> observer) => new CompositeDisposable(new IDisposable[]
			{
				r.GetDisposable(),
				xs.Subscribe(observer)
			}));
		}

		public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			return Observable.Create<TSource>(delegate(IObserver<TSource> observer)
			{
				bool isFirst = true;
				TSource prev = default(TSource);
				return source.Subscribe(delegate(TSource x)
				{
					if (isFirst)
					{
						isFirst = false;
						prev = x;
						observer.OnNext(x);
					}
					else
					{
						try
						{
							prev = func.Invoke(prev, x);
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						observer.OnNext(prev);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			return Observable.Create<TAccumulate>(delegate(IObserver<TAccumulate> observer)
			{
				TAccumulate prev = seed;
				observer.OnNext(seed);
				return source.Subscribe(delegate(TSource x)
				{
					try
					{
						prev = func.Invoke(prev, x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					observer.OnNext(prev);
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IConnectableObservable<T> Multicast<T>(this IObservable<T> source, ISubject<T> subject)
		{
			return new Observable.ConnectableObservable<T>(source, subject);
		}

		public static IConnectableObservable<T> Publish<T>(this IObservable<T> source)
		{
			return source.Multicast(new Subject<T>());
		}

		public static IConnectableObservable<T> Publish<T>(this IObservable<T> source, T initialValue)
		{
			return source.Multicast(new BehaviorSubject<T>(initialValue));
		}

		public static IConnectableObservable<T> PublishLast<T>(this IObservable<T> source)
		{
			return source.Multicast(new AsyncSubject<T>());
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source)
		{
			return source.Multicast(new ReplaySubject<T>());
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize, scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window)
		{
			return source.Multicast(new ReplaySubject<T>(window));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(window, scheduler));
		}

		public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, TimeSpan window, IScheduler scheduler)
		{
			return source.Multicast(new ReplaySubject<T>(bufferSize, window, scheduler));
		}

		public static IObservable<T> RefCount<T>(this IConnectableObservable<T> source)
		{
			IDisposable connection = null;
			object gate = new object();
			int refCount = 0;
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				IDisposable subscription = source.Subscribe(observer);
				object gate = gate;
				lock (gate)
				{
					if (++refCount == 1)
					{
						connection = source.Connect();
					}
				}
				return Disposable.Create(delegate
				{
					subscription.Dispose();
					object gate2 = gate;
					lock (gate2)
					{
						if (--refCount == 0)
						{
							connection.Dispose();
						}
					}
				});
			});
		}

		public static T Wait<T>(this IObservable<T> source)
		{
			return Observable.WaitCore<T>(source, true, Observable.InfiniteTimeSpan);
		}

		public static T Wait<T>(this IObservable<T> source, TimeSpan timeout)
		{
			return Observable.WaitCore<T>(source, true, timeout);
		}

		private static T WaitCore<T>(IObservable<T> source, bool throwOnEmpty, TimeSpan timeout)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			ManualResetEvent semaphore = new ManualResetEvent(false);
			bool seenValue = false;
			T value = default(T);
			Exception ex = null;
			using (source.Subscribe(delegate(T x)
			{
				seenValue = true;
				value = x;
			}, delegate(Exception x)
			{
				ex = x;
				semaphore.Set();
			}, delegate
			{
				semaphore.Set();
			}))
			{
				if (!((!(timeout == Observable.InfiniteTimeSpan)) ? semaphore.WaitOne(timeout) : semaphore.WaitOne()))
				{
					throw new TimeoutException("OnCompleted not fired.");
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			if (throwOnEmpty && !seenValue)
			{
				throw new InvalidOperationException("No Elements.");
			}
			return value;
		}

		public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return Observable.ConcatCore<TSource>(sources);
		}

		public static IObservable<TSource> Concat<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return Observable.ConcatCore<TSource>(sources);
		}

		public static IObservable<TSource> Concat<TSource>(this IObservable<IObservable<TSource>> sources)
		{
			return sources.Merge(1);
		}

		public static IObservable<TSource> Concat<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				throw new ArgumentNullException("second");
			}
			return Observable.ConcatCore<TSource>(new IObservable<TSource>[]
			{
				first,
				second
			});
		}

		private static IObservable<T> ConcatCore<T>(IEnumerable<IObservable<T>> sources)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				bool isDisposed = false;
				IEnumerator<IObservable<T>> e = sources.AsSafeEnumerable<IObservable<T>>().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				object gate = new object();
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(delegate(Action self)
				{
					object gate = gate;
					lock (gate)
					{
						if (!isDisposed)
						{
							bool flag = false;
							Exception ex = null;
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									if (e.get_Current() == null)
									{
										throw new InvalidOperationException("sequence is null.");
									}
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								observer.OnError(ex);
							}
							else if (!flag)
							{
								observer.OnCompleted();
							}
							else
							{
								IObservable<T> current = e.get_Current();
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								singleAssignmentDisposable.Disposable = current.Subscribe(new Action<T>(observer.OnNext), new Action<Exception>(observer.OnError), self);
							}
						}
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					subscription,
					Disposable.Create(delegate
					{
						object gate = gate;
						lock (gate)
						{
							isDisposed = true;
							e.Dispose();
						}
					})
				});
			});
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			return sources.Merge(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
		{
			return sources.ToObservable(scheduler).Merge<TSource>();
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
		{
			return sources.Merge(maxConcurrent, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
		{
			return sources.ToObservable(scheduler).Merge(maxConcurrent);
		}

		public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
		{
			return Observable.Merge<TSource>(Scheduler.DefaultSchedulers.ConstantTimeOperations, sources);
		}

		public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
		{
			return sources.ToObservable(scheduler).Merge<TSource>();
		}

		public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second)
		{
			return Observable.Merge<T>(new IObservable<T>[]
			{
				first,
				second
			});
		}

		public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second, IScheduler scheduler)
		{
			return Observable.Merge<T>(scheduler, new IObservable<T>[]
			{
				first,
				second
			});
		}

		public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				bool isStopped = false;
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				CompositeDisposable group = new CompositeDisposable
				{
					singleAssignmentDisposable
				};
				singleAssignmentDisposable.Disposable = sources.Subscribe(delegate(IObservable<T> innerSource)
				{
					SingleAssignmentDisposable innerSubscription = new SingleAssignmentDisposable();
					group.Add(innerSubscription);
					innerSubscription.Disposable = innerSource.Subscribe(delegate(T x)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnNext(x);
						}
					}, delegate(Exception exception)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnError(exception);
						}
					}, delegate
					{
						group.Remove(innerSubscription);
						if (isStopped && group.Count == 1)
						{
							object gate = gate;
							lock (gate)
							{
								observer.OnCompleted();
							}
						}
					});
				}, delegate(Exception exception)
				{
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					isStopped = true;
					if (group.Count == 1)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnCompleted();
						}
					}
				});
				return group;
			});
		}

		public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources, int maxConcurrent)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				Queue<IObservable<T>> q = new Queue<IObservable<T>>();
				bool isStopped = false;
				CompositeDisposable group = new CompositeDisposable();
				int activeCount = 0;
				Action<IObservable<T>> subscribe = null;
				subscribe = delegate(IObservable<T> xs)
				{
					SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
					group.Add(subscription);
					subscription.Disposable = xs.Subscribe(delegate(T x)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnNext(x);
						}
					}, delegate(Exception exception)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnError(exception);
						}
					}, delegate
					{
						group.Remove(subscription);
						object gate = gate;
						lock (gate)
						{
							if (q.get_Count() > 0)
							{
								IObservable<T> observable = q.Dequeue();
								subscribe.Invoke(observable);
							}
							else
							{
								activeCount--;
								if (isStopped && activeCount == 0)
								{
									observer.OnCompleted();
								}
							}
						}
					});
				};
				group.Add(sources.Subscribe(delegate(IObservable<T> innerSource)
				{
					object gate = gate;
					lock (gate)
					{
						if (activeCount < maxConcurrent)
						{
							activeCount++;
							subscribe.Invoke(innerSource);
						}
						else
						{
							q.Enqueue(innerSource);
						}
					}
				}, delegate(Exception exception)
				{
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						isStopped = true;
						if (activeCount == 0)
						{
							observer.OnCompleted();
						}
					}
				}));
				return group;
			});
		}

		public static IObservable<TResult> Zip<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return Observable.Create<TResult>(delegate(IObserver<TResult> observer)
			{
				object gate = new object();
				Queue<TLeft> leftQ = new Queue<TLeft>();
				bool leftCompleted = false;
				Queue<TRight> rightQ = new Queue<TRight>();
				bool rightCompleted = false;
				Action dequeue = delegate
				{
					if (leftQ.get_Count() != 0 && rightQ.get_Count() != 0)
					{
						TLeft tLeft = leftQ.Dequeue();
						TRight tRight = rightQ.Dequeue();
						TResult value;
						try
						{
							value = selector.Invoke(tLeft, tRight);
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						observer.OnNext(value);
						return;
					}
					if (leftCompleted || rightCompleted)
					{
						observer.OnCompleted();
						return;
					}
				};
				IDisposable item = left.Synchronize(gate).Subscribe(delegate(TLeft x)
				{
					leftQ.Enqueue(x);
					dequeue.Invoke();
				}, new Action<Exception>(observer.OnError), delegate
				{
					leftCompleted = true;
					if (rightCompleted)
					{
						observer.OnCompleted();
					}
				});
				IDisposable item2 = right.Synchronize(gate).Subscribe(delegate(TRight x)
				{
					rightQ.Enqueue(x);
					dequeue.Invoke();
				}, new Action<Exception>(observer.OnError), delegate
				{
					rightCompleted = true;
					if (leftCompleted)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					item,
					item2,
					Disposable.Create(delegate
					{
						object gate = gate;
						lock (gate)
						{
							leftQ.Clear();
							rightQ.Clear();
						}
					})
				};
			});
		}

		public static IObservable<IList<T>> Zip<T>(this IEnumerable<IObservable<T>> sources)
		{
			return Observable.Zip<T>(Enumerable.ToArray<IObservable<T>>(sources));
		}

		public static IObservable<IList<T>> Zip<T>(params IObservable<T>[] sources)
		{
			return Observable.Create<IList<T>>(delegate(IObserver<IList<T>> observer)
			{
				object gate = new object();
				int num = sources.Length;
				Queue<T>[] queues = new Queue<T>[num];
				int i;
				for (i = 0; i < num; i++)
				{
					queues[i] = new Queue<T>();
				}
				bool[] isDone = new bool[num];
				Action<int> dequeue = delegate(int index)
				{
					object gate = gate;
					lock (gate)
					{
						if (Enumerable.All<Queue<T>>(queues, (Queue<T> x) => x.get_Count() > 0))
						{
							List<T> value = Enumerable.ToList<T>(Enumerable.Select<Queue<T>, T>(queues, (Queue<T> x) => x.Dequeue()));
							observer.OnNext(value);
						}
						else if (Enumerable.All<bool>(Enumerable.Where<bool>(isDone, (bool x, int i) => i != index), (bool x) => x))
						{
							observer.OnCompleted();
						}
					}
				};
				SingleAssignmentDisposable[] disposables = Enumerable.ToArray<SingleAssignmentDisposable>(Enumerable.Select<IObservable<T>, SingleAssignmentDisposable>(sources, delegate(IObservable<T> source, int index)
				{
					SingleAssignmentDisposable d = new SingleAssignmentDisposable();
					d.Disposable = source.Subscribe(delegate(T x)
					{
						object gate = gate;
						lock (gate)
						{
							queues[index].Enqueue(x);
							dequeue.Invoke(index);
						}
					}, delegate(Exception ex)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnError(ex);
						}
					}, delegate
					{
						object gate = gate;
						lock (gate)
						{
							isDone[index] = true;
							if (Enumerable.All<bool>(isDone, (bool x) => x))
							{
								observer.OnCompleted();
							}
							else
							{
								d.Dispose();
							}
						}
					});
					return d;
				}));
				return new CompositeDisposable(disposables)
				{
					Disposable.Create(delegate
					{
						object gate = gate;
						lock (gate)
						{
							Queue<T>[] queues = queues;
							for (int i = 0; i < queues.Length; i++)
							{
								Queue<T> queue = queues[i];
								queue.Clear();
							}
						}
					})
				};
			});
		}

		public static IObservable<TResult> CombineLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector)
		{
			return Observable.Create<TResult>(delegate(IObserver<TResult> observer)
			{
				object gate = new object();
				TLeft leftValue = default(TLeft);
				bool leftStarted = false;
				bool leftCompleted = false;
				TRight rightValue = default(TRight);
				bool rightStarted = false;
				bool rightCompleted = false;
				Action run = delegate
				{
					if ((leftCompleted && !leftStarted) || (rightCompleted && !rightStarted))
					{
						observer.OnCompleted();
						return;
					}
					if (!leftStarted || !rightStarted)
					{
						return;
					}
					TResult value;
					try
					{
						value = selector.Invoke(leftValue, rightValue);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					observer.OnNext(value);
				};
				IDisposable item = left.Synchronize(gate).Subscribe(delegate(TLeft x)
				{
					leftStarted = true;
					leftValue = x;
					run.Invoke();
				}, new Action<Exception>(observer.OnError), delegate
				{
					leftCompleted = true;
					if (rightCompleted)
					{
						observer.OnCompleted();
					}
				});
				IDisposable item2 = right.Synchronize(gate).Subscribe(delegate(TRight x)
				{
					rightStarted = true;
					rightValue = x;
					run.Invoke();
				}, new Action<Exception>(observer.OnError), delegate
				{
					rightCompleted = true;
					if (leftCompleted)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					item,
					item2
				};
			});
		}

		public static IObservable<IList<T>> CombineLatest<T>(this IEnumerable<IObservable<T>> sources)
		{
			return Observable.CombineLatest<T>(Enumerable.ToArray<IObservable<T>>(sources));
		}

		public static IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
		{
			return Observable.Create<IList<TSource>>(delegate(IObserver<IList<TSource>> observer)
			{
				IObservable<TSource>[] array = Enumerable.ToArray<IObservable<TSource>>(sources);
				int num = array.Length;
				bool[] hasValue = new bool[num];
				bool hasValueAll = false;
				List<TSource> values = new List<TSource>(num);
				for (int k = 0; k < num; k++)
				{
					values.Add(default(TSource));
				}
				bool[] isDone = new bool[num];
				int i;
				Action<int> next = delegate(int i)
				{
					hasValue[i] = true;
					if (!hasValueAll)
					{
						if (!(hasValueAll = Enumerable.All<bool>(hasValue, (bool x) => x)))
						{
							if (Enumerable.All<bool>(Enumerable.Where<bool>(isDone, (bool x, int j) => i != i), (bool x) => x))
							{
								observer.OnCompleted();
								return;
							}
							return;
						}
					}
					List<TSource> value = Enumerable.ToList<TSource>(values);
					observer.OnNext(value);
				};
				Action<int> done = delegate(int i)
				{
					isDone[i] = true;
					if (Enumerable.All<bool>(isDone, (bool x) => x))
					{
						observer.OnCompleted();
						return;
					}
				};
				SingleAssignmentDisposable[] array2 = new SingleAssignmentDisposable[num];
				object gate = new object();
				for (i = 0; i < num; i++)
				{
					int j = i;
					array2[j] = new SingleAssignmentDisposable
					{
						Disposable = array[j].Synchronize(gate).Subscribe(delegate(TSource x)
						{
							values.set_Item(j, x);
							next.Invoke(j);
						}, new Action<Exception>(observer.OnError), delegate
						{
							done.Invoke(j);
						})
					};
				}
				return new CompositeDisposable(array2);
			});
		}

		public static IObservable<T> Switch<T>(this IObservable<IObservable<T>> sources)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				SerialDisposable innerSubscription = new SerialDisposable();
				bool isStopped = false;
				ulong latest = 0uL;
				bool hasLatest = false;
				IDisposable disposable = sources.Subscribe(delegate(IObservable<T> innerSource)
				{
					ulong id = 0uL;
					object gate = gate;
					lock (gate)
					{
						id = (latest += 1uL);
						hasLatest = true;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					innerSubscription.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = innerSource.Subscribe(delegate(T x)
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (latest == id)
							{
								observer.OnNext(x);
							}
						}
					}, delegate(Exception exception)
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (latest == id)
							{
								observer.OnError(exception);
							}
						}
					}, delegate
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (latest == id)
							{
								hasLatest = false;
								if (isStopped)
								{
									observer.OnCompleted();
								}
							}
						}
					});
				}, delegate(Exception exception)
				{
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						isStopped = true;
						if (!hasLatest)
						{
							observer.OnCompleted();
						}
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					innerSubscription
				});
			});
		}

		public static IObservable<T[]> WhenAll<T>(params IObservable<T>[] sources)
		{
			if (sources.Length == 0)
			{
				return Observable.Return<T[]>(new T[0]);
			}
			return Observable.Create<T[]>(delegate(IObserver<T[]> observer)
			{
				object gate = new object();
				int length = sources.Length;
				int completedCount = 0;
				T[] values = new T[length];
				IDisposable[] array = new IDisposable[length];
				for (int i = 0; i < length; i++)
				{
					IObservable<T> source = sources[i];
					int capturedIndex = i;
					array[i] = new SingleAssignmentDisposable
					{
						Disposable = source.Subscribe(delegate(T x)
						{
							object gate = gate;
							lock (gate)
							{
								values[capturedIndex] = x;
							}
						}, delegate(Exception ex)
						{
							object gate = gate;
							lock (gate)
							{
								observer.OnError(ex);
							}
						}, delegate
						{
							object gate = gate;
							lock (gate)
							{
								completedCount++;
								if (completedCount == length)
								{
									observer.OnNext(values);
									observer.OnCompleted();
								}
							}
						})
					};
				}
				return new CompositeDisposable(array);
			});
		}

		public static IObservable<T[]> WhenAll<T>(this IEnumerable<IObservable<T>> sources)
		{
			IObservable<T>[] array = sources as IObservable<T>[];
			if (array != null)
			{
				return Observable.WhenAll<T>(array);
			}
			return Observable.Create<T[]>(delegate(IObserver<T[]> observer)
			{
				IList<IObservable<T>> list = sources as IList<IObservable<T>>;
				if (list == null)
				{
					list = new List<IObservable<T>>();
					using (IEnumerator<IObservable<T>> enumerator = sources.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IObservable<T> current = enumerator.get_Current();
							list.Add(current);
						}
					}
				}
				object gate = new object();
				int length = list.get_Count();
				int completedCount = 0;
				T[] values = new T[length];
				if (length == 0)
				{
					observer.OnNext(values);
					observer.OnCompleted();
					return Disposable.Empty;
				}
				IDisposable[] array2 = new IDisposable[length];
				for (int i = 0; i < length; i++)
				{
					IObservable<T> source = list.get_Item(i);
					int capturedIndex = i;
					array2[i] = new SingleAssignmentDisposable
					{
						Disposable = source.Subscribe(delegate(T x)
						{
							object gate = gate;
							lock (gate)
							{
								values[capturedIndex] = x;
							}
						}, delegate(Exception ex)
						{
							object gate = gate;
							lock (gate)
							{
								observer.OnError(ex);
							}
						}, delegate
						{
							object gate = gate;
							lock (gate)
							{
								completedCount++;
								if (completedCount == length)
								{
									observer.OnNext(values);
									observer.OnCompleted();
								}
							}
						})
					};
				}
				return new CompositeDisposable(array2);
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, T value)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				observer.OnNext(value);
				return source.Subscribe(observer);
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, Func<T> valueFactory)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				T value;
				try
				{
					value = valueFactory.Invoke();
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return Disposable.Empty;
				}
				observer.OnNext(value);
				return source.Subscribe(observer);
			});
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, params T[] values)
		{
			return source.StartWith(Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IEnumerable<T> values)
		{
			return source.StartWith(Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, T value)
		{
			return Observable.Return<T>(value, scheduler).Concat(source);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, IEnumerable<T> values)
		{
			T[] array = values as T[];
			if (array == null)
			{
				array = Enumerable.ToArray<T>(values);
			}
			return source.StartWith(scheduler, array);
		}

		public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, params T[] values)
		{
			return values.ToObservable(scheduler).Concat(source);
		}

		public static IObservable<T> Synchronize<T>(this IObservable<T> source)
		{
			return source.Synchronize(new object());
		}

		public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate)
		{
			return Observable.Create<T>((IObserver<T> observer) => source.Subscribe(delegate(T x)
			{
				object gate2 = gate;
				lock (gate2)
				{
					observer.OnNext(x);
				}
			}, delegate(Exception x)
			{
				object gate2 = gate;
				lock (gate2)
				{
					observer.OnError(x);
				}
			}, delegate
			{
				object gate2 = gate;
				lock (gate2)
				{
					observer.OnCompleted();
				}
			}));
		}

		public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				CompositeDisposable group = new CompositeDisposable();
				IDisposable item = source.Subscribe(delegate(T x)
				{
					IDisposable item2 = scheduler.Schedule(delegate
					{
						observer.OnNext(x);
					});
					group.Add(item2);
				}, delegate(Exception ex)
				{
					IDisposable item2 = scheduler.Schedule(delegate
					{
						observer.OnError(ex);
					});
					group.Add(item2);
				}, delegate
				{
					IDisposable item2 = scheduler.Schedule(delegate
					{
						observer.OnCompleted();
					});
					group.Add(item2);
				});
				group.Add(item);
				return group;
			});
		}

		public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				SerialDisposable d = new SerialDisposable();
				d.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = scheduler.Schedule(delegate
				{
					d.Disposable = new ScheduledDisposable(scheduler, source.Subscribe(observer));
				});
				return d;
			});
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.DelaySubscription(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				d.Disposable = scheduler.Schedule(dueTime2, delegate
				{
					d.Disposable = source.Subscribe(observer);
				});
				return d;
			});
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime)
		{
			return source.DelaySubscription(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				d.Disposable = scheduler.Schedule(dueTime, delegate
				{
					d.Disposable = source.Subscribe(observer);
				});
				return d;
			});
		}

		public static IObservable<T> Amb<T>(params IObservable<T>[] sources)
		{
			return Observable.Amb<T>(sources);
		}

		public static IObservable<T> Amb<T>(IEnumerable<IObservable<T>> sources)
		{
			IObservable<T> observable = Observable.Never<T>();
			using (IEnumerator<IObservable<T>> enumerator = sources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IObservable<T> current = enumerator.get_Current();
					IObservable<T> second = current;
					observable = observable.Amb(second);
				}
			}
			return observable;
		}

		public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				Observable.AmbState choice = Observable.AmbState.Neither;
				object gate = new object();
				SingleAssignmentDisposable leftSubscription = new SingleAssignmentDisposable();
				SingleAssignmentDisposable rightSubscription = new SingleAssignmentDisposable();
				leftSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					object gate = gate;
					lock (gate)
					{
						if (choice == Observable.AmbState.Neither)
						{
							choice = Observable.AmbState.Left;
							rightSubscription.Dispose();
						}
					}
					if (choice == Observable.AmbState.Left)
					{
						observer.OnNext(x);
					}
				}, delegate(Exception ex)
				{
					object gate = gate;
					lock (gate)
					{
						if (choice == Observable.AmbState.Neither)
						{
							choice = Observable.AmbState.Left;
							rightSubscription.Dispose();
						}
					}
					if (choice == Observable.AmbState.Left)
					{
						observer.OnError(ex);
					}
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						if (choice == Observable.AmbState.Neither)
						{
							choice = Observable.AmbState.Left;
							rightSubscription.Dispose();
						}
					}
					if (choice == Observable.AmbState.Left)
					{
						observer.OnCompleted();
					}
				});
				rightSubscription.Disposable = second.Subscribe(delegate(T x)
				{
					object gate = gate;
					lock (gate)
					{
						if (choice == Observable.AmbState.Neither)
						{
							choice = Observable.AmbState.Right;
							leftSubscription.Dispose();
						}
					}
					if (choice == Observable.AmbState.Right)
					{
						observer.OnNext(x);
					}
				}, delegate(Exception ex)
				{
					object gate = gate;
					lock (gate)
					{
						if (choice == Observable.AmbState.Neither)
						{
							choice = Observable.AmbState.Right;
							leftSubscription.Dispose();
						}
					}
					if (choice == Observable.AmbState.Right)
					{
						observer.OnError(ex);
					}
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						if (choice == Observable.AmbState.Neither)
						{
							choice = Observable.AmbState.Right;
							leftSubscription.Dispose();
						}
					}
					if (choice == Observable.AmbState.Right)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					leftSubscription,
					rightSubscription
				};
			});
		}

		public static IObservable<T> AsObservable<T>(this IObservable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Observable.Create<T>((IObserver<T> observer) => source.Subscribe(observer));
		}

		public static IObservable<T> ToObservable<T>(this IEnumerable<T> source)
		{
			return source.ToObservable(Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> ToObservable<T>(this IEnumerable<T> source, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				IEnumerator<T> e;
				try
				{
					e = source.AsSafeEnumerable<T>().GetEnumerator();
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return Disposable.Empty;
				}
				SingleAssignmentDisposable flag = new SingleAssignmentDisposable();
				flag.Disposable = scheduler.Schedule(delegate(Action self)
				{
					bool flag;
					if (flag.IsDisposed)
					{
						e.Dispose();
						return;
					}
					T value = default(T);
					try
					{
						flag = e.MoveNext();
						if (flag)
						{
							value = e.get_Current();
						}
					}
					catch (Exception error2)
					{
						e.Dispose();
						observer.OnError(error2);
						return;
					}
					if (flag)
					{
						observer.OnNext(value);
						self.Invoke();
					}
					else
					{
						e.Dispose();
						observer.OnCompleted();
					}
				});
				return flag;
			});
		}

		public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source)
		{
			return from x in source
			select (TResult)((object)x);
		}

		public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source, TResult witness)
		{
			return from x in source
			select (TResult)((object)x);
		}

		public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source)
		{
			return from x in source
			where x is TResult
			select (TResult)((object)x);
		}

		public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source, TResult witness)
		{
			return from x in source
			where x is TResult
			select (TResult)((object)x);
		}

		public static IObservable<Unit> AsUnitObservable<T>(this IObservable<T> source)
		{
			return Observable.Create<Unit>((IObserver<Unit> observer) => source.Subscribe(Observer.Create<T>(delegate(T _)
			{
				observer.OnNext(Unit.Default);
			}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted))));
		}

		public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
		{
			if (subscribe == null)
			{
				throw new ArgumentNullException("subscribe");
			}
			return new Observable.AnonymousObservable<T>(subscribe);
		}

		public static IObservable<T> Empty<T>()
		{
			return Observable.Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Empty<T>(IScheduler scheduler)
		{
			return Observable.Create<T>((IObserver<T> observer) => scheduler.Schedule(new Action(observer.OnCompleted)));
		}

		public static IObservable<T> Empty<T>(T witness)
		{
			return Observable.Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Empty<T>(IScheduler scheduler, T witness)
		{
			return Observable.Empty<T>(scheduler);
		}

		public static IObservable<T> Never<T>()
		{
			return Observable.Create<T>((IObserver<T> observer) => Disposable.Empty);
		}

		public static IObservable<T> Never<T>(T witness)
		{
			return Observable.Never<T>();
		}

		public static IObservable<T> Return<T>(T value)
		{
			return Observable.Return<T>(value, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Return<T>(T value, IScheduler scheduler)
		{
			return Observable.Create<T>((IObserver<T> observer) => scheduler.Schedule(delegate
			{
				observer.OnNext(value);
				observer.OnCompleted();
			}));
		}

		public static IObservable<T> Throw<T>(Exception error)
		{
			return Observable.Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Throw<T>(Exception error, T witness)
		{
			return Observable.Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
		}

		public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler)
		{
			return Observable.Create<T>((IObserver<T> observer) => scheduler.Schedule(delegate
			{
				observer.OnError(error);
			}));
		}

		public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler, T witness)
		{
			return Observable.Throw<T>(error, scheduler);
		}

		public static IObservable<int> Range(int start, int count)
		{
			return Observable.Range(start, count, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<int> Range(int start, int count, IScheduler scheduler)
		{
			return Observable.Create<int>(delegate(IObserver<int> observer)
			{
				int i = 0;
				return scheduler.Schedule(delegate(Action self)
				{
					if (i < count)
					{
						int value = start + i;
						observer.OnNext(value);
						Interlocked.Increment(ref i);
						self.Invoke();
					}
					else
					{
						observer.OnCompleted();
					}
				});
			});
		}

		public static IObservable<T> Repeat<T>(T value)
		{
			return Observable.Repeat<T>(value, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> Repeat<T>(T value, IScheduler scheduler)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return Observable.Create<T>((IObserver<T> observer) => scheduler.Schedule(delegate(Action self)
			{
				observer.OnNext(value);
				self.Invoke();
			}));
		}

		public static IObservable<T> Repeat<T>(T value, int repeatCount)
		{
			return Observable.Repeat<T>(value, repeatCount, Scheduler.DefaultSchedulers.Iteration);
		}

		public static IObservable<T> Repeat<T>(T value, int repeatCount, IScheduler scheduler)
		{
			if (repeatCount < 0)
			{
				throw new ArgumentOutOfRangeException("repeatCount");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				int currentCount = repeatCount;
				return scheduler.Schedule(delegate(Action self)
				{
					if (currentCount > 0)
					{
						observer.OnNext(value);
						currentCount--;
					}
					if (currentCount == 0)
					{
						observer.OnCompleted();
						return;
					}
					self.Invoke();
				});
			});
		}

		public static IObservable<T> Repeat<T>(this IObservable<T> source)
		{
			return Observable.RepeatInfinite<T>(source).Concat<T>();
		}

		[DebuggerHidden]
		private static IEnumerable<IObservable<T>> RepeatInfinite<T>(IObservable<T> source)
		{
			Observable.<RepeatInfinite>c__IteratorB<T> <RepeatInfinite>c__IteratorB = new Observable.<RepeatInfinite>c__IteratorB<T>();
			<RepeatInfinite>c__IteratorB.source = source;
			<RepeatInfinite>c__IteratorB.<$>source = source;
			Observable.<RepeatInfinite>c__IteratorB<T> expr_15 = <RepeatInfinite>c__IteratorB;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static IObservable<T> RepeatSafe<T>(this IObservable<T> source)
		{
			return Observable.RepeatSafeCore<T>(Observable.RepeatInfinite<T>(source));
		}

		private static IObservable<T> RepeatSafeCore<T>(IEnumerable<IObservable<T>> sources)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				bool isDisposed = false;
				bool isRunNext = false;
				IEnumerator<IObservable<T>> e = sources.AsSafeEnumerable<IObservable<T>>().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				object gate = new object();
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(delegate(Action self)
				{
					object gate = gate;
					lock (gate)
					{
						if (!isDisposed)
						{
							bool flag = false;
							Exception ex = null;
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									if (e.get_Current() == null)
									{
										throw new InvalidOperationException("sequence is null.");
									}
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								observer.OnError(ex);
							}
							else if (!flag)
							{
								observer.OnCompleted();
							}
							else
							{
								IObservable<T> current = e.get_Current();
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								singleAssignmentDisposable.Disposable = current.Subscribe(delegate(T x)
								{
									isRunNext = true;
									observer.OnNext(x);
								}, new Action<Exception>(observer.OnError), delegate
								{
									if (isRunNext && !isDisposed)
									{
										isRunNext = false;
										self.Invoke();
									}
									else
									{
										e.Dispose();
										if (!isDisposed)
										{
											observer.OnCompleted();
										}
									}
								});
							}
						}
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					subscription,
					Disposable.Create(delegate
					{
						object gate = gate;
						lock (gate)
						{
							isDisposed = true;
							e.Dispose();
						}
					})
				});
			});
		}

		public static IObservable<T> Defer<T>(Func<IObservable<T>> observableFactory)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				IObservable<T> observable;
				try
				{
					observable = observableFactory.Invoke();
				}
				catch (Exception error)
				{
					observable = Observable.Throw<T>(error);
				}
				return observable.Subscribe(observer);
			});
		}

		public static IObservable<T> Start<T>(Func<T> function)
		{
			return Observable.Start<T>(function, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<T> Start<T>(Func<T> function, IScheduler scheduler)
		{
			return Observable.ToAsync<T>(function, scheduler).Invoke();
		}

		public static IObservable<Unit> Start(Action action)
		{
			return Observable.Start(action, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static IObservable<Unit> Start(Action action, IScheduler scheduler)
		{
			return Observable.ToAsync(action, scheduler).Invoke();
		}

		public static Func<IObservable<T>> ToAsync<T>(Func<T> function)
		{
			return Observable.ToAsync<T>(function, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static Func<IObservable<T>> ToAsync<T>(Func<T> function, IScheduler scheduler)
		{
			return delegate
			{
				AsyncSubject<T> subject = new AsyncSubject<T>();
				scheduler.Schedule(delegate
				{
					T value = default(T);
					try
					{
						value = function.Invoke();
					}
					catch (Exception error)
					{
						subject.OnError(error);
						return;
					}
					subject.OnNext(value);
					subject.OnCompleted();
				});
				return subject.AsObservable<T>();
			};
		}

		public static Func<IObservable<Unit>> ToAsync(Action action)
		{
			return Observable.ToAsync(action, Scheduler.DefaultSchedulers.AsyncConversions);
		}

		public static Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler)
		{
			return delegate
			{
				AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
				scheduler.Schedule(delegate
				{
					try
					{
						action.Invoke();
					}
					catch (Exception error)
					{
						subject.OnError(error);
						return;
					}
					subject.OnNext(Unit.Default);
					subject.OnCompleted();
				});
				return subject.AsObservable<Unit>();
			};
		}

		public static IObservable<T> Finally<T>(this IObservable<T> source, Action finallyAction)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				IDisposable subscription;
				try
				{
					subscription = source.Subscribe(observer);
				}
				catch
				{
					finallyAction.Invoke();
					throw;
				}
				return Disposable.Create(delegate
				{
					try
					{
						subscription.Dispose();
					}
					finally
					{
						finallyAction.Invoke();
					}
				});
			});
		}

		public static IObservable<T> Catch<T, TException>(this IObservable<T> source, Func<TException, IObservable<T>> errorHandler) where TException : Exception
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				SerialDisposable serialDisposable = new SerialDisposable();
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				serialDisposable.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = source.Subscribe(new Action<T>(observer.OnNext), delegate(Exception exception)
				{
					TException ex = exception as TException;
					if (ex != null)
					{
						IObservable<T> observable;
						try
						{
							if (errorHandler == new Func<TException, IObservable<T>>(Stubs.CatchIgnore<T>))
							{
								observable = Observable.Empty<T>();
							}
							else
							{
								observable = errorHandler.Invoke(ex);
							}
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
						SingleAssignmentDisposable singleAssignmentDisposable2 = new SingleAssignmentDisposable();
						serialDisposable.Disposable = singleAssignmentDisposable2;
						singleAssignmentDisposable2.Disposable = observable.Subscribe(observer);
					}
					else
					{
						observer.OnError(exception);
					}
				}, new Action(observer.OnCompleted));
				return serialDisposable;
			});
		}

		public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources)
		{
			return Observable.Create<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				bool isDisposed = false;
				IEnumerator<IObservable<TSource>> e = sources.AsSafeEnumerable<IObservable<TSource>>().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				Exception lastException = null;
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(delegate(Action self)
				{
					object gate = gate;
					lock (gate)
					{
						IObservable<TSource> source = null;
						bool flag = false;
						Exception ex = null;
						if (!isDisposed)
						{
							try
							{
								flag = e.MoveNext();
								if (flag)
								{
									source = e.get_Current();
								}
								else
								{
									e.Dispose();
								}
							}
							catch (Exception ex2)
							{
								ex = ex2;
								e.Dispose();
							}
							if (ex != null)
							{
								observer.OnError(ex);
							}
							else if (!flag)
							{
								if (lastException != null)
								{
									observer.OnError(lastException);
								}
								else
								{
									observer.OnCompleted();
								}
							}
							else
							{
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								subscription.Disposable = singleAssignmentDisposable;
								singleAssignmentDisposable.Disposable = source.Subscribe(new Action<TSource>(observer.OnNext), delegate(Exception exception)
								{
									lastException = exception;
									self.Invoke();
								}, new Action(observer.OnCompleted));
							}
						}
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					subscription,
					disposable,
					Disposable.Create(delegate
					{
						object gate = gate;
						lock (gate)
						{
							e.Dispose();
							isDisposed = true;
						}
					})
				});
			});
		}

		public static IObservable<TSource> CatchIgnore<TSource>(this IObservable<TSource> source)
		{
			return source.Catch(new Func<Exception, IObservable<TSource>>(Stubs.CatchIgnore<TSource>));
		}

		public static IObservable<TSource> CatchIgnore<TSource, TException>(this IObservable<TSource> source, Action<TException> errorAction) where TException : Exception
		{
			return source.Catch(delegate(TException ex)
			{
				errorAction.Invoke(ex);
				return Observable.Empty<TSource>();
			});
		}

		public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source)
		{
			return Observable.RepeatInfinite<TSource>(source).Catch<TSource>();
		}

		public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount)
		{
			return Enumerable.Repeat<IObservable<TSource>>(source, retryCount).Catch<TSource>();
		}

		public static IObservable<TSource> OnErrorRetry<TSource>(this IObservable<TSource> source)
		{
			return source.Retry<TSource>();
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError) where TException : Exception
		{
			return source.OnErrorRetry(onError, TimeSpan.Zero);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, TimeSpan delay) where TException : Exception
		{
			return source.OnErrorRetry(onError, 2147483647, delay);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount) where TException : Exception
		{
			return source.OnErrorRetry(onError, retryCount, TimeSpan.Zero);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay) where TException : Exception
		{
			return source.OnErrorRetry(onError, retryCount, delay, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> OnErrorRetry<TSource, TException>(this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay, IScheduler delayScheduler) where TException : Exception
		{
			return Observable.Defer<TSource>(delegate
			{
				TimeSpan dueTime = (delay.get_Ticks() >= 0L) ? delay : TimeSpan.Zero;
				int count = 0;
				IObservable<TSource> self = null;
				self = source.Catch(delegate(TException ex)
				{
					onError.Invoke(ex);
					IObservable<TSource> arg_96_0;
					if (++count < retryCount)
					{
						IObservable<TSource> observable;
						IObservable<TSource> arg_84_0;
						if (dueTime == TimeSpan.Zero)
						{
							observable = self.SubscribeOn(Scheduler.CurrentThread);
							arg_84_0 = observable;
						}
						else
						{
							arg_84_0 = self.DelaySubscription(dueTime, delayScheduler).SubscribeOn(Scheduler.CurrentThread);
						}
						observable = arg_84_0;
						arg_96_0 = observable;
					}
					else
					{
						arg_96_0 = Observable.Throw<TSource>(ex);
					}
					return arg_96_0;
				});
				return self;
			});
		}

		public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) where TEventArgs : EventArgs
		{
			return Observable.Create<EventPattern<TEventArgs>>(delegate(IObserver<EventPattern<TEventArgs>> observer)
			{
				TDelegate handler = conversion.Invoke(delegate(object sender, TEventArgs eventArgs)
				{
					observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs));
				});
				addHandler.Invoke(handler);
				return Disposable.Create(delegate
				{
					removeHandler.Invoke(handler);
				});
			});
		}

		public static IObservable<Unit> FromEvent<TDelegate>(Func<Action, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
		{
			return Observable.Create<Unit>(delegate(IObserver<Unit> observer)
			{
				TDelegate handler = conversion.Invoke(delegate
				{
					observer.OnNext(Unit.Default);
				});
				addHandler.Invoke(handler);
				return Disposable.Create(delegate
				{
					removeHandler.Invoke(handler);
				});
			});
		}

		public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
		{
			return Observable.Create<TEventArgs>(delegate(IObserver<TEventArgs> observer)
			{
				TDelegate handler = conversion.Invoke(new Action<TEventArgs>(observer.OnNext));
				addHandler.Invoke(handler);
				return Disposable.Create(delegate
				{
					removeHandler.Invoke(handler);
				});
			});
		}

		public static IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler)
		{
			return Observable.Create<Unit>(delegate(IObserver<Unit> observer)
			{
				Action handler = delegate
				{
					observer.OnNext(Unit.Default);
				};
				addHandler.Invoke(handler);
				return Disposable.Create(delegate
				{
					removeHandler.Invoke(handler);
				});
			});
		}

		public static IObservable<T> FromEvent<T>(Action<Action<T>> addHandler, Action<Action<T>> removeHandler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				Action<T> handler = delegate(T x)
				{
					observer.OnNext(x);
				};
				addHandler.Invoke(handler);
				return Disposable.Create(delegate
				{
					removeHandler.Invoke(handler);
				});
			});
		}

		public static Func<IObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate
			{
				AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
				try
				{
					begin.Invoke(delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end.Invoke(iar);
						}
						catch (Exception error2)
						{
							subject.OnError(error2);
							return;
						}
						subject.OnNext(value);
						subject.OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Observable.Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return subject.AsObservable<TResult>();
			};
		}

		public static Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate(T1 x)
			{
				AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
				try
				{
					begin.Invoke(x, delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end.Invoke(iar);
						}
						catch (Exception error2)
						{
							subject.OnError(error2);
							return;
						}
						subject.OnNext(value);
						subject.OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Observable.Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return subject.AsObservable<TResult>();
			};
		}

		public static Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end)
		{
			return delegate(T1 x, T2 y)
			{
				AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
				try
				{
					begin.Invoke(x, y, delegate(IAsyncResult iar)
					{
						TResult value;
						try
						{
							value = end.Invoke(iar);
						}
						catch (Exception error2)
						{
							subject.OnError(error2);
							return;
						}
						subject.OnNext(value);
						subject.OnCompleted();
					}, null);
				}
				catch (Exception error)
				{
					return Observable.Throw<TResult>(error, Scheduler.DefaultSchedulers.AsyncConversions);
				}
				return subject.AsObservable<TResult>();
			};
		}

		public static Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return Observable.FromAsyncPattern<Unit>(begin, delegate(IAsyncResult iar)
			{
				end.Invoke(iar);
				return Unit.Default;
			});
		}

		public static Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return Observable.FromAsyncPattern<T1, Unit>(begin, delegate(IAsyncResult iar)
			{
				end.Invoke(iar);
				return Unit.Default;
			});
		}

		public static Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
		{
			return Observable.FromAsyncPattern<T1, T2, Unit>(begin, delegate(IAsyncResult iar)
			{
				end.Invoke(iar);
				return Unit.Default;
			});
		}

		public static IObservable<T> Take<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				return Observable.Empty<T>();
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				int rest = count;
				return source.Subscribe(delegate(T x)
				{
					if (rest > 0)
					{
						rest--;
						observer.OnNext(x);
						if (rest == 0)
						{
							observer.OnCompleted();
						}
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.TakeWhile((T x, int i) => predicate.Invoke(x));
		}

		public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				int i = 0;
				bool running = true;
				return source.Subscribe(delegate(T x)
				{
					try
					{
						running = predicate.Invoke(x, i++);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (running)
					{
						observer.OnNext(x);
					}
					else
					{
						observer.OnCompleted();
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> TakeUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				IDisposable disposable = other.Synchronize(gate).Subscribe(delegate(TOther _)
				{
					observer.OnCompleted();
				}, new Action<Exception>(observer.OnError));
				IDisposable item = source.Synchronize(gate).Finally(new Action(disposable.Dispose)).Subscribe(observer);
				return new CompositeDisposable
				{
					disposable,
					item
				};
			});
		}

		public static IObservable<T> Skip<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				int index = 0;
				return source.Subscribe(delegate(T x)
				{
					if (index++ >= count)
					{
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.SkipWhile((T x, int i) => predicate.Invoke(x));
		}

		public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				int i = 0;
				bool skipEnd = false;
				return source.Subscribe(delegate(T x)
				{
					if (!skipEnd)
					{
						try
						{
							if (predicate.Invoke(x, i++))
							{
								return;
							}
							skipEnd = true;
						}
						catch (Exception error)
						{
							observer.OnError(error);
							return;
						}
					}
					observer.OnNext(x);
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> SkipUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				SingleAssignmentDisposable otherSubscription = new SingleAssignmentDisposable();
				bool open = false;
				object gate = new object();
				singleAssignmentDisposable.Disposable = source.Synchronize(gate).Subscribe(delegate(T x)
				{
					if (open)
					{
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), delegate
				{
					if (open)
					{
						observer.OnCompleted();
					}
				});
				otherSubscription.Disposable = other.Synchronize(gate).Subscribe(delegate(TOther x)
				{
					open = true;
					otherSubscription.Dispose();
				}, new Action<Exception>(observer.OnError));
				return new CompositeDisposable(new IDisposable[]
				{
					singleAssignmentDisposable,
					otherSubscription
				});
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			return Observable.Create<IList<T>>(delegate(IObserver<IList<T>> observer)
			{
				List<T> list = new List<T>();
				return source.Subscribe(delegate(T x)
				{
					list.Add(x);
					if (list.get_Count() == count)
					{
						observer.OnNext(list);
						list = new List<T>();
					}
				}, new Action<Exception>(observer.OnError), delegate
				{
					if (list.get_Count() > 0)
					{
						observer.OnNext(list);
					}
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count, int skip)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			if (skip <= 0)
			{
				throw new ArgumentOutOfRangeException("skip <= 0");
			}
			return Observable.Create<IList<T>>(delegate(IObserver<IList<T>> observer)
			{
				Queue<List<T>> q = new Queue<List<T>>();
				int index = -1;
				return source.Subscribe(delegate(T x)
				{
					index++;
					if (index % skip == 0)
					{
						q.Enqueue(new List<T>(count));
					}
					int count2 = q.get_Count();
					for (int i = 0; i < count2; i++)
					{
						List<T> list = q.Dequeue();
						list.Add(x);
						if (list.get_Count() == count)
						{
							observer.OnNext(list);
						}
						else
						{
							q.Enqueue(list);
						}
					}
				}, new Action<Exception>(observer.OnError), delegate
				{
					using (Queue<List<T>>.Enumerator enumerator = q.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							List<T> current = enumerator.get_Current();
							observer.OnNext(current);
						}
					}
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan)
		{
			return source.Buffer(timeSpan, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Observable.Create<IList<T>>(delegate(IObserver<IList<T>> observer)
			{
				List<T> list = new List<T>();
				object gate = new object();
				return new CompositeDisposable(2)
				{
					scheduler.Schedule(timeSpan, delegate(Action<TimeSpan> self)
					{
						object gate = gate;
						List<T> list;
						lock (gate)
						{
							list = list;
							list = new List<T>();
						}
						observer.OnNext(list);
						self.Invoke(timeSpan);
					}),
					source.Subscribe(delegate(T x)
					{
						object gate = gate;
						lock (gate)
						{
							list.Add(x);
						}
					}, new Action<Exception>(observer.OnError), delegate
					{
						List<T> list = list;
						observer.OnNext(list);
						observer.OnCompleted();
					})
				};
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count)
		{
			return source.Buffer(timeSpan, count, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count <= 0");
			}
			return Observable.Create<IList<T>>(delegate(IObserver<IList<T>> observer)
			{
				List<T> list = new List<T>();
				object gate = new object();
				long timerId = 0L;
				CompositeDisposable compositeDisposable = new CompositeDisposable(2);
				SerialDisposable timerD = new SerialDisposable();
				compositeDisposable.Add(timerD);
				Action createTimer = delegate
				{
					long currentTimerId = timerId;
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					timerD.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = scheduler.Schedule(timeSpan, delegate(Action<TimeSpan> self)
					{
						object gate = gate;
						List<T> list;
						lock (gate)
						{
							if (currentTimerId != timerId)
							{
								return;
							}
							list = list;
							if (list.get_Count() != 0)
							{
								list = new List<T>();
							}
						}
						if (list.get_Count() != 0)
						{
							observer.OnNext(list);
						}
						self.Invoke(timeSpan);
					});
				};
				createTimer.Invoke();
				compositeDisposable.Add(source.Subscribe(delegate(T x)
				{
					List<T> list = null;
					object gate = gate;
					lock (gate)
					{
						list.Add(x);
						if (list.get_Count() == count)
						{
							list = list;
							list = new List<T>();
							timerId += 1L;
							createTimer.Invoke();
						}
					}
					if (list != null)
					{
						observer.OnNext(list);
					}
				}, new Action<Exception>(observer.OnError), delegate
				{
					object gate = gate;
					lock (gate)
					{
						timerId += 1L;
					}
					List<T> list = list;
					observer.OnNext(list);
					observer.OnCompleted();
				}));
				return compositeDisposable;
			});
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift)
		{
			return source.Buffer(timeSpan, timeShift, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Observable.Create<IList<T>>(delegate(IObserver<IList<T>> observer)
			{
				TimeSpan totalTime = TimeSpan.Zero;
				TimeSpan nextShift = timeShift;
				TimeSpan nextSpan = timeSpan;
				object gate = new object();
				Queue<IList<T>> q = new Queue<IList<T>>();
				SerialDisposable timerD = new SerialDisposable();
				Action createTimer = null;
				createTimer = delegate
				{
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					timerD.Disposable = singleAssignmentDisposable;
					bool isSpan = false;
					bool isShift = false;
					if (nextSpan == nextShift)
					{
						isSpan = true;
						isShift = true;
					}
					else if (nextSpan < nextShift)
					{
						isSpan = true;
					}
					else
					{
						isShift = true;
					}
					TimeSpan timeSpan2 = (!isSpan) ? nextShift : nextSpan;
					TimeSpan dueTime = timeSpan2 - totalTime;
					totalTime = timeSpan2;
					if (isSpan)
					{
						nextSpan += timeShift;
					}
					if (isShift)
					{
						nextShift += timeShift;
					}
					singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate
					{
						object gate = gate;
						lock (gate)
						{
							if (isShift)
							{
								List<T> list = new List<T>();
								q.Enqueue(list);
							}
							if (isSpan)
							{
								IList<T> value = q.Dequeue();
								observer.OnNext(value);
							}
						}
						createTimer.Invoke();
					});
				};
				q.Enqueue(new List<T>());
				createTimer.Invoke();
				return source.Subscribe(delegate(T x)
				{
					object gate = gate;
					lock (gate)
					{
						using (Queue<IList<T>>.Enumerator enumerator = q.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								IList<T> current = enumerator.get_Current();
								current.Add(x);
							}
						}
					}
				}, new Action<Exception>(observer.OnError), delegate
				{
					object gate = gate;
					lock (gate)
					{
						using (Queue<IList<T>>.Enumerator enumerator = q.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								IList<T> current = enumerator.get_Current();
								observer.OnNext(current);
							}
						}
						observer.OnCompleted();
					}
				});
			});
		}

		public static IObservable<IList<TSource>> Buffer<TSource, TWindowBoundary>(this IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
		{
			return Observable.Create<IList<TSource>>(delegate(IObserver<IList<TSource>> observer)
			{
				List<TSource> list = new List<TSource>();
				object gate = new object();
				return new CompositeDisposable(2)
				{
					source.Subscribe(Observer.Create<TSource>(delegate(TSource x)
					{
						object gate = gate;
						lock (gate)
						{
							list.Add(x);
						}
					}, delegate(Exception ex)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnError(ex);
						}
					}, delegate
					{
						object gate = gate;
						lock (gate)
						{
							List<TSource> list = list;
							list = new List<TSource>();
							observer.OnNext(list);
							observer.OnCompleted();
						}
					})),
					windowBoundaries.Subscribe(Observer.Create<TWindowBoundary>(delegate(TWindowBoundary w)
					{
						object gate = gate;
						List<TSource> list;
						lock (gate)
						{
							list = list;
							if (list.get_Count() != 0)
							{
								list = new List<TSource>();
							}
						}
						if (list.get_Count() != 0)
						{
							observer.OnNext(list);
						}
					}, delegate(Exception ex)
					{
						object gate = gate;
						lock (gate)
						{
							observer.OnError(ex);
						}
					}, delegate
					{
						object gate = gate;
						lock (gate)
						{
							List<TSource> list = list;
							list = new List<TSource>();
							observer.OnNext(list);
							observer.OnCompleted();
						}
					}))
				};
			});
		}

		public static IObservable<TR> Pairwise<T, TR>(this IObservable<T> source, Func<T, T, TR> selector)
		{
			return Observable.Create<TR>(delegate(IObserver<TR> observer)
			{
				T prev = default(T);
				bool isFirst = true;
				return source.Subscribe(delegate(T x)
				{
					if (isFirst)
					{
						isFirst = false;
						prev = x;
						return;
					}
					TR value;
					try
					{
						value = selector.Invoke(prev, x);
						prev = x;
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					observer.OnNext(value);
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> Last<T>(this IObservable<T> source)
		{
			return source.LastCore(false);
		}

		public static IObservable<T> Last<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).LastCore(false);
		}

		public static IObservable<T> LastOrDefault<T>(this IObservable<T> source)
		{
			return source.LastCore(true);
		}

		public static IObservable<T> LastOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).LastCore(true);
		}

		private static IObservable<T> LastCore<T>(this IObservable<T> source, bool useDefault)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				T value = default(T);
				bool hasValue = false;
				return source.Subscribe(delegate(T x)
				{
					value = x;
					hasValue = true;
				}, new Action<Exception>(observer.OnError), delegate
				{
					if (hasValue)
					{
						observer.OnNext(value);
						observer.OnCompleted();
					}
					else if (useDefault)
					{
						observer.OnNext(default(T));
						observer.OnCompleted();
					}
					else
					{
						observer.OnError(new InvalidOperationException("sequence is empty"));
					}
				});
			});
		}

		public static IObservable<T> First<T>(this IObservable<T> source)
		{
			return source.FirstCore(false);
		}

		public static IObservable<T> First<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).FirstCore(false);
		}

		public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source)
		{
			return source.FirstCore(true);
		}

		public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).FirstCore(true);
		}

		private static IObservable<T> FirstCore<T>(this IObservable<T> source, bool useDefault)
		{
			return Observable.Create<T>((IObserver<T> observer) => source.Subscribe(delegate(T x)
			{
				observer.OnNext(x);
				observer.OnCompleted();
			}, new Action<Exception>(observer.OnError), delegate
			{
				if (useDefault)
				{
					observer.OnNext(default(T));
					observer.OnCompleted();
				}
				else
				{
					observer.OnError(new InvalidOperationException("sequence is empty"));
				}
			}));
		}

		public static IObservable<T> Single<T>(this IObservable<T> source)
		{
			return source.SingleCore(false);
		}

		public static IObservable<T> Single<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).SingleCore(false);
		}

		public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source)
		{
			return source.SingleCore(true);
		}

		public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where(predicate).SingleCore(true);
		}

		private static IObservable<T> SingleCore<T>(this IObservable<T> source, bool useDefault)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				T value = default(T);
				bool seenValue = false;
				return source.Subscribe(delegate(T x)
				{
					if (seenValue)
					{
						observer.OnError(new InvalidOperationException("sequence is not single"));
					}
					value = x;
					seenValue = true;
				}, new Action<Exception>(observer.OnError), delegate
				{
					if (seenValue)
					{
						observer.OnNext(value);
						observer.OnCompleted();
					}
					else if (useDefault)
					{
						observer.OnNext(default(T));
						observer.OnCompleted();
					}
					else
					{
						observer.OnError(new InvalidOperationException("sequence is empty"));
					}
				});
			});
		}

		public static IObservable<long> Interval(TimeSpan period)
		{
			return Observable.TimerCore(period, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
		{
			return Observable.TimerCore(period, period, scheduler);
		}

		public static IObservable<long> Timer(TimeSpan dueTime)
		{
			return Observable.TimerCore(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime)
		{
			return Observable.TimerCore(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
		{
			return Observable.TimerCore(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
		{
			return Observable.TimerCore(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
		{
			return Observable.TimerCore(dueTime, scheduler);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Observable.TimerCore(dueTime, scheduler);
		}

		public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			return Observable.TimerCore(dueTime, period, scheduler);
		}

		public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			return Observable.TimerCore(dueTime, period, scheduler);
		}

		private static IObservable<long> TimerCore(TimeSpan dueTime, IScheduler scheduler)
		{
			TimeSpan time = Scheduler.Normalize(dueTime);
			return Observable.Create<long>((IObserver<long> observer) => scheduler.Schedule(time, delegate(Action<TimeSpan> self)
			{
				observer.OnNext(0L);
				observer.OnCompleted();
			}));
		}

		private static IObservable<long> TimerCore(DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Observable.Create<long>((IObserver<long> observer) => scheduler.Schedule(dueTime, delegate(Action<DateTimeOffset> self)
			{
				observer.OnNext(0L);
				observer.OnCompleted();
			}));
		}

		private static IObservable<long> TimerCore(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
		{
			TimeSpan timeD = Scheduler.Normalize(dueTime);
			TimeSpan timeP = Scheduler.Normalize(period);
			return Observable.Create<long>(delegate(IObserver<long> observer)
			{
				int count = 0;
				return scheduler.Schedule(timeD, delegate(Action<TimeSpan> self)
				{
					observer.OnNext((long)count);
					count++;
					self.Invoke(timeP);
				});
			});
		}

		private static IObservable<long> TimerCore(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
		{
			TimeSpan timeP = Scheduler.Normalize(period);
			return Observable.Create<long>(delegate(IObserver<long> observer)
			{
				DateTimeOffset nextTime = dueTime;
				long count = 0L;
				return scheduler.Schedule(nextTime, delegate(Action<DateTimeOffset> self)
				{
					if (timeP > TimeSpan.Zero)
					{
						nextTime += period;
						DateTimeOffset now = scheduler.Now;
						if (nextTime <= now)
						{
							nextTime = now + period;
						}
					}
					observer.OnNext(count);
					count += 1L;
					self.Invoke(nextTime);
				});
			});
		}

		public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source)
		{
			return source.Timestamp(Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source, IScheduler scheduler)
		{
			return from x in source
			select new Timestamped<TSource>(x, scheduler.Now);
		}

		public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source)
		{
			return source.TimeInterval(Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source, IScheduler scheduler)
		{
			return Observable.Defer<TimeInterval<TSource>>(delegate
			{
				DateTimeOffset last = scheduler.Now;
				return source.Select(delegate(TSource x)
				{
					DateTimeOffset now = scheduler.Now;
					TimeSpan interval = now.Subtract(last);
					last = now;
					return new TimeInterval<TSource>(x, interval);
				});
			});
		}

		public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.Delay(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return Observable.Create<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				Queue<Timestamped<Notification<TSource>>> q = new Queue<Timestamped<Notification<TSource>>>();
				bool active = false;
				bool running = false;
				SerialDisposable cancelable = new SerialDisposable();
				Exception exception = null;
				IDisposable disposable = source.Materialize<TSource>().Timestamp(scheduler).Subscribe(delegate(Timestamped<Notification<TSource>> notification)
				{
					bool flag = false;
					object gate = gate;
					lock (gate)
					{
						if (notification.Value.Kind == NotificationKind.OnError)
						{
							q.Clear();
							q.Enqueue(notification);
							exception = notification.Value.Exception;
							flag = !running;
						}
						else
						{
							q.Enqueue(new Timestamped<Notification<TSource>>(notification.Value, notification.Timestamp.Add(dueTime)));
							flag = !active;
							active = true;
						}
					}
					if (flag)
					{
						if (exception != null)
						{
							observer.OnError(exception);
						}
						else
						{
							SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
							cancelable.Disposable = singleAssignmentDisposable;
							singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate(Action<TimeSpan> self)
							{
								object gate2 = gate;
								lock (gate2)
								{
									if (exception != null)
									{
										return;
									}
									running = true;
								}
								Notification<TSource> notification;
								do
								{
									notification = null;
									object gate3 = gate;
									lock (gate3)
									{
										if (q.get_Count() > 0 && q.Peek().Timestamp.CompareTo(scheduler.Now) <= 0)
										{
											notification = q.Dequeue().Value;
										}
									}
									if (notification != null)
									{
										notification.Accept(observer);
									}
								}
								while (notification != null);
								bool flag2 = false;
								TimeSpan timeSpan = TimeSpan.Zero;
								Exception ex = null;
								object gate4 = gate;
								lock (gate4)
								{
									if (q.get_Count() > 0)
									{
										flag2 = true;
										timeSpan = TimeSpan.FromTicks(Math.Max(0L, q.Peek().Timestamp.Subtract(scheduler.Now).get_Ticks()));
									}
									else
									{
										active = false;
									}
									ex = exception;
									running = false;
								}
								if (ex != null)
								{
									observer.OnError(ex);
								}
								else if (flag2)
								{
									self.Invoke(timeSpan);
								}
							});
						}
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					cancelable
				});
			});
		}

		public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval)
		{
			return source.Sample(interval, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				T latestValue = default(T);
				bool isUpdated = false;
				bool isCompleted = false;
				object gate = new object();
				IDisposable item = scheduler.Schedule(interval, delegate(Action<TimeSpan> self)
				{
					object gate = gate;
					lock (gate)
					{
						if (isUpdated)
						{
							T latestValue = latestValue;
							isUpdated = false;
							observer.OnNext(latestValue);
						}
						if (isCompleted)
						{
							observer.OnCompleted();
						}
					}
					self.Invoke(interval);
				});
				SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
				sourceSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					object gate = gate;
					lock (gate)
					{
						latestValue = x;
						isUpdated = true;
					}
				}, delegate(Exception e)
				{
					object gate = gate;
					lock (gate)
					{
						observer.OnError(e);
					}
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						isCompleted = true;
						sourceSubscription.Dispose();
					}
				});
				return new CompositeDisposable
				{
					item,
					sourceSubscription
				};
			});
		}

		public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
		{
			return source.Throttle(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new Observable.AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				TSource value = default(TSource);
				bool hasValue = false;
				SerialDisposable cancelable = new SerialDisposable();
				ulong id = 0uL;
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					object gate = gate;
					ulong currentid;
					lock (gate)
					{
						hasValue = true;
						value = x;
						id += 1uL;
						currentid = id;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (hasValue && id == currentid)
							{
								observer.OnNext(value);
							}
							hasValue = false;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
						hasValue = false;
						id += 1uL;
					}
				}, delegate
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						if (hasValue)
						{
							observer.OnNext(value);
						}
						observer.OnCompleted();
						hasValue = false;
						id += 1uL;
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					cancelable
				});
			});
		}

		public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
		{
			return source.ThrottleFirst(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return new Observable.AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				bool open = true;
				SerialDisposable cancelable = new SerialDisposable();
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					object gate = gate;
					lock (gate)
					{
						if (!open)
						{
							return;
						}
						observer.OnNext(x);
						open = false;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = scheduler.Schedule(dueTime, delegate
					{
						object gate2 = gate;
						lock (gate2)
						{
							open = true;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					cancelable
				});
			});
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime)
		{
			return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				ulong objectId = 0uL;
				bool isTimeout = false;
				Func<ulong, IDisposable> runTimer = (ulong timerId) => scheduler.Schedule(dueTime, delegate
				{
					object gate = gate;
					lock (gate)
					{
						if (objectId == timerId)
						{
							isTimeout = true;
						}
					}
					if (isTimeout)
					{
						observer.OnError(new TimeoutException());
					}
				});
				SerialDisposable timerDisposable = new SerialDisposable();
				timerDisposable.Disposable = runTimer.Invoke(objectId);
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				singleAssignmentDisposable.Disposable = source.Subscribe(delegate(T x)
				{
					object gate = gate;
					bool isTimeout;
					lock (gate)
					{
						isTimeout = isTimeout;
						objectId += 1uL;
					}
					if (isTimeout)
					{
						return;
					}
					timerDisposable.Disposable = Disposable.Empty;
					observer.OnNext(x);
					timerDisposable.Disposable = runTimer.Invoke(objectId);
				}, delegate(Exception ex)
				{
					object gate = gate;
					bool isTimeout;
					lock (gate)
					{
						isTimeout = isTimeout;
						objectId += 1uL;
					}
					if (isTimeout)
					{
						return;
					}
					timerDisposable.Dispose();
					observer.OnError(ex);
				}, delegate
				{
					object gate = gate;
					bool isTimeout;
					lock (gate)
					{
						isTimeout = isTimeout;
						objectId += 1uL;
					}
					if (isTimeout)
					{
						return;
					}
					timerDisposable.Dispose();
					observer.OnCompleted();
				});
				return new CompositeDisposable
				{
					timerDisposable,
					singleAssignmentDisposable
				};
			});
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime)
		{
			return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
		}

		public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				bool isFinished = false;
				SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
				IDisposable timerD = scheduler.Schedule(dueTime, delegate
				{
					object gate = gate;
					lock (gate)
					{
						if (isFinished)
						{
							return;
						}
						isFinished = true;
					}
					sourceSubscription.Dispose();
					observer.OnError(new TimeoutException());
				});
				sourceSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					object gate = gate;
					lock (gate)
					{
						if (!isFinished)
						{
							observer.OnNext(x);
						}
					}
				}, delegate(Exception ex)
				{
					object gate = gate;
					lock (gate)
					{
						if (isFinished)
						{
							return;
						}
						isFinished = true;
					}
					observer.OnError(ex);
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						if (!isFinished)
						{
							isFinished = true;
							timerD.Dispose();
						}
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable
				{
					timerD,
					sourceSubscription
				};
			});
		}

		public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selector)
		{
			return source.Select((T x, int i) => selector.Invoke(x));
		}

		public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, int, TR> selector)
		{
			return Observable.Create<TR>(delegate(IObserver<TR> observer)
			{
				int index = 0;
				return source.Subscribe(Observer.Create<T>(delegate(T x)
				{
					TR value = default(TR);
					try
					{
						value = selector.Invoke(x, index++);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					observer.OnNext(value);
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
			});
		}

		public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
		{
			return source.Where((T x, int i) => predicate.Invoke(x));
		}

		public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, int, bool> predicate)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				int index = 0;
				return source.Subscribe(Observer.Create<T>(delegate(T x)
				{
					bool flag = false;
					try
					{
						flag = predicate.Invoke(x, index++);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (flag)
					{
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
			});
		}

		public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, IObservable<TR> other)
		{
			return source.SelectMany((T _) => other);
		}

		public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
		{
			return source.Select(selector).Merge<TR>();
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
		{
			return source.Select(selector).Merge<TResult>();
		}

		public static IObservable<TR> SelectMany<T, TC, TR>(this IObservable<T> source, Func<T, IObservable<TC>> collectionSelector, Func<T, TC, TR> resultSelector)
		{
			return source.SelectMany((T x) => from y in collectionSelector.Invoke(x)
			select resultSelector.Invoke(x, y));
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
		{
			return source.SelectMany((TSource x, int i) => collectionSelector.Invoke(x, i).Select((TCollection y, int i2) => resultSelector.Invoke(x, i, y, i2)));
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			return new Observable.AnonymousObservable<TResult>((IObserver<TResult> observer) => source.Subscribe(delegate(TSource x)
			{
				IEnumerable<TResult> source2 = null;
				try
				{
					source2 = selector.Invoke(x);
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return;
				}
				using (IEnumerator<TResult> enumerator = source2.AsSafeEnumerable<TResult>().GetEnumerator())
				{
					bool flag = true;
					while (flag)
					{
						flag = false;
						TResult value = default(TResult);
						try
						{
							flag = enumerator.MoveNext();
							if (flag)
							{
								value = enumerator.get_Current();
							}
						}
						catch (Exception error2)
						{
							observer.OnError(error2);
							break;
						}
						if (flag)
						{
							observer.OnNext(value);
						}
					}
				}
			}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
		}

		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			return Observable.Create<TResult>(delegate(IObserver<TResult> observer)
			{
				int index = 0;
				return source.Subscribe(delegate(TSource x)
				{
					IEnumerable<TResult> source2 = null;
					try
					{
						source2 = selector.Invoke(x, index++);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					using (IEnumerator<TResult> enumerator = source2.AsSafeEnumerable<TResult>().GetEnumerator())
					{
						bool flag = true;
						while (flag)
						{
							flag = false;
							TResult value = default(TResult);
							try
							{
								flag = enumerator.MoveNext();
								if (flag)
								{
									value = enumerator.get_Current();
								}
							}
							catch (Exception error2)
							{
								observer.OnError(error2);
								break;
							}
							if (flag)
							{
								observer.OnNext(value);
							}
						}
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			return new Observable.AnonymousObservable<TResult>((IObserver<TResult> observer) => source.Subscribe(delegate(TSource x)
			{
				IEnumerable<TCollection> source2 = null;
				try
				{
					source2 = collectionSelector.Invoke(x);
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return;
				}
				using (IEnumerator<TCollection> enumerator = source2.AsSafeEnumerable<TCollection>().GetEnumerator())
				{
					bool flag = true;
					while (flag)
					{
						flag = false;
						TResult value = default(TResult);
						try
						{
							flag = enumerator.MoveNext();
							if (flag)
							{
								value = resultSelector.Invoke(x, enumerator.get_Current());
							}
						}
						catch (Exception error2)
						{
							observer.OnError(error2);
							break;
						}
						if (flag)
						{
							observer.OnNext(value);
						}
					}
				}
			}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
		}

		public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
		{
			return Observable.Create<TResult>(delegate(IObserver<TResult> observer)
			{
				int index = 0;
				return source.Subscribe(delegate(TSource x)
				{
					IEnumerable<TCollection> source2 = null;
					try
					{
						source2 = collectionSelector.Invoke(x, index++);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					using (IEnumerator<TCollection> enumerator = source2.AsSafeEnumerable<TCollection>().GetEnumerator())
					{
						int num = 0;
						bool flag = true;
						while (flag)
						{
							flag = false;
							TResult value = default(TResult);
							try
							{
								flag = enumerator.MoveNext();
								if (flag)
								{
									value = resultSelector.Invoke(x, index, enumerator.get_Current(), checked(num++));
								}
							}
							catch (Exception error2)
							{
								observer.OnError(error2);
								break;
							}
							if (flag)
							{
								observer.OnNext(value);
							}
						}
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T[]> ToArray<T>(this IObservable<T> source)
		{
			return Observable.Create<T[]>(delegate(IObserver<T[]> observer)
			{
				List<T> list = new List<T>();
				return source.Subscribe(delegate(T x)
				{
					list.Add(x);
				}, new Action<Exception>(observer.OnError), delegate
				{
					observer.OnNext(list.ToArray());
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, IObserver<T> observer)
		{
			return source.Do(new Action<T>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext)
		{
			return source.Do(onNext, Stubs.Throw, Stubs.Nop);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
		{
			return source.Do(onNext, onError, Stubs.Nop);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
		{
			return source.Do(onNext, Stubs.Throw, onCompleted);
		}

		public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			return Observable.Create<T>((IObserver<T> observer) => source.Subscribe(delegate(T x)
			{
				try
				{
					if (onNext != new Action<T>(Stubs.Ignore<T>))
					{
						onNext.Invoke(x);
					}
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return;
				}
				observer.OnNext(x);
			}, delegate(Exception ex)
			{
				try
				{
					onError.Invoke(ex);
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return;
				}
				observer.OnError(ex);
			}, delegate
			{
				try
				{
					onCompleted.Invoke();
				}
				catch (Exception error)
				{
					observer.OnError(error);
					return;
				}
				observer.OnCompleted();
			}));
		}

		public static IObservable<Notification<T>> Materialize<T>(this IObservable<T> source)
		{
			return Observable.Create<Notification<T>>((IObserver<Notification<T>> observer) => source.Subscribe(delegate(T x)
			{
				observer.OnNext(Notification.CreateOnNext<T>(x));
			}, delegate(Exception x)
			{
				observer.OnNext(Notification.CreateOnError<T>(x));
				observer.OnCompleted();
			}, delegate
			{
				observer.OnNext(Notification.CreateOnCompleted<T>());
				observer.OnCompleted();
			}));
		}

		public static IObservable<T> Dematerialize<T>(this IObservable<Notification<T>> source)
		{
			return Observable.Create<T>((IObserver<T> observer) => source.Subscribe(delegate(Notification<T> x)
			{
				if (x.Kind == NotificationKind.OnNext)
				{
					observer.OnNext(x.Value);
				}
				else if (x.Kind == NotificationKind.OnError)
				{
					observer.OnError(x.Exception);
				}
				else if (x.Kind == NotificationKind.OnCompleted)
				{
					observer.OnCompleted();
				}
			}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
		}

		public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source)
		{
			return source.DefaultIfEmpty(default(T));
		}

		public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source, T defaultValue)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				bool hasValue = false;
				return source.Subscribe(delegate(T x)
				{
					hasValue = true;
					observer.OnNext(x);
				}, new Action<Exception>(observer.OnError), delegate
				{
					if (!hasValue)
					{
						observer.OnNext(defaultValue);
					}
					observer.OnCompleted();
				});
			});
		}

		public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
		{
			return source.Distinct(null);
		}

		public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			return Observable.Create<TSource>(delegate(IObserver<TSource> observer)
			{
				HashSet<TSource> hashSet = (comparer != null) ? new HashSet<TSource>(comparer) : new HashSet<TSource>();
				return source.Subscribe(delegate(TSource x)
				{
					TSource tSource = default(TSource);
					bool flag = false;
					try
					{
						flag = hashSet.Add(x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (flag)
					{
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.Distinct(keySelector, null);
		}

		public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			return Observable.Create<TSource>(delegate(IObserver<TSource> observer)
			{
				HashSet<TKey> hashSet = (comparer != null) ? new HashSet<TKey>(comparer) : new HashSet<TKey>();
				return source.Subscribe(delegate(TSource x)
				{
					TKey tKey = default(TKey);
					bool flag = false;
					try
					{
						TKey tKey2 = keySelector.Invoke(x);
						flag = hashSet.Add(tKey2);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					if (flag)
					{
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
		{
			return source.DistinctUntilChanged(null);
		}

		public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source, IEqualityComparer<T> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				bool isFirst = true;
				T prevKey = default(T);
				return source.Subscribe(delegate(T x)
				{
					T t;
					try
					{
						t = x;
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					bool flag = false;
					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						try
						{
							if (comparer == null)
							{
								if (t == null)
								{
									flag = (prevKey == null);
								}
								else
								{
									flag = t.Equals(prevKey);
								}
							}
							else
							{
								flag = comparer.Equals(t, prevKey);
							}
						}
						catch (Exception error2)
						{
							observer.OnError(error2);
							return;
						}
					}
					if (!flag)
					{
						prevKey = t;
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
		{
			return source.DistinctUntilChanged(keySelector, null);
		}

		public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				bool isFirst = true;
				TKey prevKey = default(TKey);
				return source.Subscribe(delegate(T x)
				{
					TKey tKey;
					try
					{
						tKey = keySelector.Invoke(x);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						return;
					}
					bool flag = false;
					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						try
						{
							flag = ((comparer != null) ? comparer.Equals(tKey, prevKey) : tKey.Equals(prevKey));
						}
						catch (Exception error2)
						{
							observer.OnError(error2);
							return;
						}
					}
					if (!flag)
					{
						prevKey = tKey;
						observer.OnNext(x);
					}
				}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
			});
		}

		public static IObservable<T> IgnoreElements<T>(this IObservable<T> source)
		{
			return Observable.Create<T>((IObserver<T> observer) => source.Subscribe(new Action<T>(Stubs.Ignore<T>), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)));
		}

		public static IObservable<Unit> FromCoroutine(Func<IEnumerator> coroutine, bool publishEveryYield = false)
		{
			return Observable.FromCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine.Invoke(), observer, cancellationToken, publishEveryYield));
		}

		[DebuggerHidden]
		private static IEnumerator WrapEnumerator(IEnumerator enumerator, IObserver<Unit> observer, CancellationToken cancellationToken, bool publishEveryYield)
		{
			Observable.<WrapEnumerator>c__IteratorC <WrapEnumerator>c__IteratorC = new Observable.<WrapEnumerator>c__IteratorC();
			<WrapEnumerator>c__IteratorC.enumerator = enumerator;
			<WrapEnumerator>c__IteratorC.observer = observer;
			<WrapEnumerator>c__IteratorC.publishEveryYield = publishEveryYield;
			<WrapEnumerator>c__IteratorC.cancellationToken = cancellationToken;
			<WrapEnumerator>c__IteratorC.<$>enumerator = enumerator;
			<WrapEnumerator>c__IteratorC.<$>observer = observer;
			<WrapEnumerator>c__IteratorC.<$>publishEveryYield = publishEveryYield;
			<WrapEnumerator>c__IteratorC.<$>cancellationToken = cancellationToken;
			return <WrapEnumerator>c__IteratorC;
		}

		public static IObservable<T> FromCoroutineValue<T>(Func<IEnumerator> coroutine, bool nullAsNextUpdate = true)
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken cancellationToken) => Observable.WrapEnumeratorYieldValue<T>(coroutine.Invoke(), observer, cancellationToken, nullAsNextUpdate));
		}

		[DebuggerHidden]
		private static IEnumerator WrapEnumeratorYieldValue<T>(IEnumerator enumerator, IObserver<T> observer, CancellationToken cancellationToken, bool nullAsNextUpdate)
		{
			Observable.<WrapEnumeratorYieldValue>c__IteratorD<T> <WrapEnumeratorYieldValue>c__IteratorD = new Observable.<WrapEnumeratorYieldValue>c__IteratorD<T>();
			<WrapEnumeratorYieldValue>c__IteratorD.enumerator = enumerator;
			<WrapEnumeratorYieldValue>c__IteratorD.observer = observer;
			<WrapEnumeratorYieldValue>c__IteratorD.nullAsNextUpdate = nullAsNextUpdate;
			<WrapEnumeratorYieldValue>c__IteratorD.cancellationToken = cancellationToken;
			<WrapEnumeratorYieldValue>c__IteratorD.<$>enumerator = enumerator;
			<WrapEnumeratorYieldValue>c__IteratorD.<$>observer = observer;
			<WrapEnumeratorYieldValue>c__IteratorD.<$>nullAsNextUpdate = nullAsNextUpdate;
			<WrapEnumeratorYieldValue>c__IteratorD.<$>cancellationToken = cancellationToken;
			return <WrapEnumeratorYieldValue>c__IteratorD;
		}

		public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, IEnumerator> coroutine)
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken _) => coroutine.Invoke(observer));
		}

		public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				MainThreadDispatcher.SendStartCoroutine(coroutine.Invoke(observer, new CancellationToken(booleanDisposable)));
				return booleanDisposable;
			});
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, IEnumerator coroutine, bool publishEveryYield = false)
		{
			return source.SelectMany(Observable.FromCoroutine(() => coroutine, publishEveryYield));
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<IEnumerator> selector, bool publishEveryYield = false)
		{
			return source.SelectMany(Observable.FromCoroutine(() => selector.Invoke(), publishEveryYield));
		}

		public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<T, IEnumerator> selector)
		{
			return source.SelectMany((T x) => Observable.FromCoroutine(() => selector.Invoke(x), false));
		}

		public static IObservable<Unit> ToObservable(this IEnumerator coroutine, bool publishEveryYield = false)
		{
			return Observable.FromCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellationToken) => Observable.WrapEnumerator(coroutine, observer, cancellationToken, publishEveryYield));
		}

		public static IObservable<long> EveryUpdate()
		{
			return Observable.FromCoroutine<long>((IObserver<long> observer, CancellationToken cancellationToken) => Observable.EveryUpdateCore(observer, cancellationToken));
		}

		[DebuggerHidden]
		private static IEnumerator EveryUpdateCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			Observable.<EveryUpdateCore>c__IteratorE <EveryUpdateCore>c__IteratorE = new Observable.<EveryUpdateCore>c__IteratorE();
			<EveryUpdateCore>c__IteratorE.cancellationToken = cancellationToken;
			<EveryUpdateCore>c__IteratorE.observer = observer;
			<EveryUpdateCore>c__IteratorE.<$>cancellationToken = cancellationToken;
			<EveryUpdateCore>c__IteratorE.<$>observer = observer;
			return <EveryUpdateCore>c__IteratorE;
		}

		public static IObservable<long> EveryFixedUpdate()
		{
			return Observable.FromCoroutine<long>((IObserver<long> observer, CancellationToken cancellationToken) => Observable.EveryFixedUpdateCore(observer, cancellationToken));
		}

		[DebuggerHidden]
		private static IEnumerator EveryFixedUpdateCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			Observable.<EveryFixedUpdateCore>c__IteratorF <EveryFixedUpdateCore>c__IteratorF = new Observable.<EveryFixedUpdateCore>c__IteratorF();
			<EveryFixedUpdateCore>c__IteratorF.cancellationToken = cancellationToken;
			<EveryFixedUpdateCore>c__IteratorF.observer = observer;
			<EveryFixedUpdateCore>c__IteratorF.<$>cancellationToken = cancellationToken;
			<EveryFixedUpdateCore>c__IteratorF.<$>observer = observer;
			return <EveryFixedUpdateCore>c__IteratorF;
		}

		public static IObservable<long> EveryEndOfFrame()
		{
			return Observable.FromCoroutine<long>((IObserver<long> observer, CancellationToken cancellationToken) => Observable.EveryEndOfFrameCore(observer, cancellationToken));
		}

		[DebuggerHidden]
		private static IEnumerator EveryEndOfFrameCore(IObserver<long> observer, CancellationToken cancellationToken)
		{
			Observable.<EveryEndOfFrameCore>c__Iterator10 <EveryEndOfFrameCore>c__Iterator = new Observable.<EveryEndOfFrameCore>c__Iterator10();
			<EveryEndOfFrameCore>c__Iterator.cancellationToken = cancellationToken;
			<EveryEndOfFrameCore>c__Iterator.observer = observer;
			<EveryEndOfFrameCore>c__Iterator.<$>cancellationToken = cancellationToken;
			<EveryEndOfFrameCore>c__Iterator.<$>observer = observer;
			return <EveryEndOfFrameCore>c__Iterator;
		}

		public static IObservable<Unit> NextFrame(FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancellation) => Observable.NextFrameCore(observer, frameCountType, cancellation));
		}

		[DebuggerHidden]
		private static IEnumerator NextFrameCore(IObserver<Unit> observer, FrameCountType frameCountType, CancellationToken cancellation)
		{
			Observable.<NextFrameCore>c__Iterator11 <NextFrameCore>c__Iterator = new Observable.<NextFrameCore>c__Iterator11();
			<NextFrameCore>c__Iterator.frameCountType = frameCountType;
			<NextFrameCore>c__Iterator.cancellation = cancellation;
			<NextFrameCore>c__Iterator.observer = observer;
			<NextFrameCore>c__Iterator.<$>frameCountType = frameCountType;
			<NextFrameCore>c__Iterator.<$>cancellation = cancellation;
			<NextFrameCore>c__Iterator.<$>observer = observer;
			return <NextFrameCore>c__Iterator;
		}

		public static IObservable<long> IntervalFrame(int intervalFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.TimerFrame(intervalFrameCount, intervalFrameCount, frameCountType);
		}

		public static IObservable<long> TimerFrame(int dueTimeFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromCoroutine<long>((IObserver<long> observer, CancellationToken cancellation) => Observable.TimerFrameCore(observer, dueTimeFrameCount, frameCountType, cancellation));
		}

		public static IObservable<long> TimerFrame(int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.FromCoroutine<long>((IObserver<long> observer, CancellationToken cancellation) => Observable.TimerFrameCore(observer, dueTimeFrameCount, periodFrameCount, frameCountType, cancellation));
		}

		[DebuggerHidden]
		private static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, FrameCountType frameCountType, CancellationToken cancel)
		{
			Observable.<TimerFrameCore>c__Iterator12 <TimerFrameCore>c__Iterator = new Observable.<TimerFrameCore>c__Iterator12();
			<TimerFrameCore>c__Iterator.dueTimeFrameCount = dueTimeFrameCount;
			<TimerFrameCore>c__Iterator.cancel = cancel;
			<TimerFrameCore>c__Iterator.observer = observer;
			<TimerFrameCore>c__Iterator.frameCountType = frameCountType;
			<TimerFrameCore>c__Iterator.<$>dueTimeFrameCount = dueTimeFrameCount;
			<TimerFrameCore>c__Iterator.<$>cancel = cancel;
			<TimerFrameCore>c__Iterator.<$>observer = observer;
			<TimerFrameCore>c__Iterator.<$>frameCountType = frameCountType;
			return <TimerFrameCore>c__Iterator;
		}

		[DebuggerHidden]
		private static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType, CancellationToken cancel)
		{
			Observable.<TimerFrameCore>c__Iterator13 <TimerFrameCore>c__Iterator = new Observable.<TimerFrameCore>c__Iterator13();
			<TimerFrameCore>c__Iterator.dueTimeFrameCount = dueTimeFrameCount;
			<TimerFrameCore>c__Iterator.periodFrameCount = periodFrameCount;
			<TimerFrameCore>c__Iterator.cancel = cancel;
			<TimerFrameCore>c__Iterator.observer = observer;
			<TimerFrameCore>c__Iterator.frameCountType = frameCountType;
			<TimerFrameCore>c__Iterator.<$>dueTimeFrameCount = dueTimeFrameCount;
			<TimerFrameCore>c__Iterator.<$>periodFrameCount = periodFrameCount;
			<TimerFrameCore>c__Iterator.<$>cancel = cancel;
			<TimerFrameCore>c__Iterator.<$>observer = observer;
			<TimerFrameCore>c__Iterator.<$>frameCountType = frameCountType;
			return <TimerFrameCore>c__Iterator;
		}

		public static IObservable<T> DelayFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			if (frameCount < 0)
			{
				throw new ArgumentOutOfRangeException("frameCount");
			}
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				BooleanDisposable cancel = new BooleanDisposable();
				source.Materialize<T>().Subscribe(delegate(Notification<T> x)
				{
					if (x.Kind == NotificationKind.OnError)
					{
						observer.OnError(x.Exception);
						cancel.Dispose();
						return;
					}
					MainThreadDispatcher.StartCoroutine(Observable.DelayFrameCore(delegate
					{
						x.Accept(observer);
					}, frameCount, frameCountType, cancel));
				});
				return cancel;
			});
		}

		[DebuggerHidden]
		private static IEnumerator DelayFrameCore(Action onNext, int frameCount, FrameCountType frameCountType, ICancelable cancel)
		{
			Observable.<DelayFrameCore>c__Iterator14 <DelayFrameCore>c__Iterator = new Observable.<DelayFrameCore>c__Iterator14();
			<DelayFrameCore>c__Iterator.cancel = cancel;
			<DelayFrameCore>c__Iterator.frameCount = frameCount;
			<DelayFrameCore>c__Iterator.frameCountType = frameCountType;
			<DelayFrameCore>c__Iterator.onNext = onNext;
			<DelayFrameCore>c__Iterator.<$>cancel = cancel;
			<DelayFrameCore>c__Iterator.<$>frameCount = frameCount;
			<DelayFrameCore>c__Iterator.<$>frameCountType = frameCountType;
			<DelayFrameCore>c__Iterator.<$>onNext = onNext;
			return <DelayFrameCore>c__Iterator;
		}

		public static IObservable<T> SampleFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				T latestValue = default(T);
				bool isUpdated = false;
				bool isCompleted = false;
				object gate = new object();
				SingleAssignmentDisposable scheduling = new SingleAssignmentDisposable();
				scheduling.Disposable = Observable.IntervalFrame(frameCount, frameCountType).Subscribe(delegate(long _)
				{
					object gate = gate;
					lock (gate)
					{
						if (isUpdated)
						{
							T latestValue = latestValue;
							isUpdated = false;
							try
							{
								observer.OnNext(latestValue);
							}
							catch
							{
								scheduling.Dispose();
							}
						}
						if (isCompleted)
						{
							observer.OnCompleted();
							scheduling.Dispose();
						}
					}
				});
				SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
				sourceSubscription.Disposable = source.Subscribe(delegate(T x)
				{
					object gate = gate;
					lock (gate)
					{
						latestValue = x;
						isUpdated = true;
					}
				}, delegate(Exception e)
				{
					object gate = gate;
					lock (gate)
					{
						observer.OnError(e);
						scheduling.Dispose();
					}
				}, delegate
				{
					object gate = gate;
					lock (gate)
					{
						isCompleted = true;
						sourceSubscription.Dispose();
					}
				});
				return new CompositeDisposable
				{
					scheduling,
					sourceSubscription
				};
			});
		}

		public static IObservable<TSource> ThrottleFrame<TSource>(this IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return new Observable.AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				TSource value = default(TSource);
				bool hasValue = false;
				SerialDisposable cancelable = new SerialDisposable();
				ulong id = 0uL;
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					object gate = gate;
					ulong currentid;
					lock (gate)
					{
						hasValue = true;
						value = x;
						id += 1uL;
						currentid = id;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = Observable.TimerFrame(frameCount, frameCountType).Subscribe(delegate(long _)
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (hasValue && id == currentid)
							{
								observer.OnNext(value);
							}
							hasValue = false;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
						hasValue = false;
						id += 1uL;
					}
				}, delegate
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						if (hasValue)
						{
							observer.OnNext(value);
						}
						observer.OnCompleted();
						hasValue = false;
						id += 1uL;
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					cancelable
				});
			});
		}

		public static IObservable<TSource> ThrottleFirstFrame<TSource>(this IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return new Observable.AnonymousObservable<TSource>(delegate(IObserver<TSource> observer)
			{
				object gate = new object();
				bool open = true;
				SerialDisposable cancelable = new SerialDisposable();
				IDisposable disposable = source.Subscribe(delegate(TSource x)
				{
					object gate = gate;
					lock (gate)
					{
						if (!open)
						{
							return;
						}
						observer.OnNext(x);
						open = false;
					}
					SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
					cancelable.Disposable = singleAssignmentDisposable;
					singleAssignmentDisposable.Disposable = Observable.TimerFrame(frameCount, frameCountType).Subscribe(delegate(long _)
					{
						object gate2 = gate;
						lock (gate2)
						{
							open = true;
						}
					});
				}, delegate(Exception exception)
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						observer.OnError(exception);
					}
				}, delegate
				{
					cancelable.Dispose();
					object gate = gate;
					lock (gate)
					{
						observer.OnCompleted();
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					disposable,
					cancelable
				});
			});
		}

		public static IObservable<T> TimeoutFrame<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				object gate = new object();
				ulong objectId = 0uL;
				bool isTimeout = false;
				Func<ulong, IDisposable> runTimer = (ulong timerId) => Observable.TimerFrame(frameCount, frameCountType).Subscribe(delegate(long _)
				{
					object gate = gate;
					lock (gate)
					{
						if (objectId == timerId)
						{
							isTimeout = true;
						}
					}
					if (isTimeout)
					{
						observer.OnError(new TimeoutException());
					}
				});
				SerialDisposable timerDisposable = new SerialDisposable();
				timerDisposable.Disposable = runTimer.Invoke(objectId);
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				singleAssignmentDisposable.Disposable = source.Subscribe(delegate(T x)
				{
					object gate = gate;
					bool isTimeout;
					lock (gate)
					{
						isTimeout = isTimeout;
						objectId += 1uL;
					}
					if (isTimeout)
					{
						return;
					}
					timerDisposable.Disposable = Disposable.Empty;
					observer.OnNext(x);
					timerDisposable.Disposable = runTimer.Invoke(objectId);
				}, delegate(Exception ex)
				{
					object gate = gate;
					bool isTimeout;
					lock (gate)
					{
						isTimeout = isTimeout;
						objectId += 1uL;
					}
					if (isTimeout)
					{
						return;
					}
					timerDisposable.Dispose();
					observer.OnError(ex);
				}, delegate
				{
					object gate = gate;
					bool isTimeout;
					lock (gate)
					{
						isTimeout = isTimeout;
						objectId += 1uL;
					}
					if (isTimeout)
					{
						return;
					}
					timerDisposable.Dispose();
					observer.OnCompleted();
				});
				return new CompositeDisposable
				{
					timerDisposable,
					singleAssignmentDisposable
				};
			});
		}

		public static IObservable<T> DelayFrameSubscription<T>(this IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				d.Disposable = Observable.TimerFrame(frameCount, frameCountType).Subscribe(delegate(long _)
				{
					d.Disposable = source.Subscribe(observer);
				});
				return d;
			});
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, CancellationToken cancel = null)
		{
			return source.ToAwaitableEnumerator(new Action<T>(Stubs.Ignore<T>), Stubs.Throw, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult, CancellationToken cancel = null)
		{
			return source.ToAwaitableEnumerator(onResult, Stubs.Throw, cancel);
		}

		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<Exception> onError, CancellationToken cancel = null)
		{
			return source.ToAwaitableEnumerator(new Action<T>(Stubs.Ignore<T>), onError, cancel);
		}

		[DebuggerHidden]
		public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = null)
		{
			Observable.<ToAwaitableEnumerator>c__Iterator15<T> <ToAwaitableEnumerator>c__Iterator = new Observable.<ToAwaitableEnumerator>c__Iterator15<T>();
			<ToAwaitableEnumerator>c__Iterator.cancel = cancel;
			<ToAwaitableEnumerator>c__Iterator.source = source;
			<ToAwaitableEnumerator>c__Iterator.onResult = onResult;
			<ToAwaitableEnumerator>c__Iterator.onError = onError;
			<ToAwaitableEnumerator>c__Iterator.<$>cancel = cancel;
			<ToAwaitableEnumerator>c__Iterator.<$>source = source;
			<ToAwaitableEnumerator>c__Iterator.<$>onResult = onResult;
			<ToAwaitableEnumerator>c__Iterator.<$>onError = onError;
			return <ToAwaitableEnumerator>c__Iterator;
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, CancellationToken cancel = null)
		{
			return source.StartAsCoroutine(new Action<T>(Stubs.Ignore<T>), Stubs.Throw, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult, CancellationToken cancel = null)
		{
			return source.StartAsCoroutine(onResult, Stubs.Throw, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<Exception> onError, CancellationToken cancel = null)
		{
			return source.StartAsCoroutine(new Action<T>(Stubs.Ignore<T>), onError, cancel);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = null)
		{
			return MainThreadDispatcher.StartCoroutine(source.ToAwaitableEnumerator(onResult, onError, cancel));
		}

		public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> source)
		{
			return source.ObserveOn(Scheduler.MainThread);
		}

		public static IObservable<T> SubscribeOnMainThread<T>(this IObservable<T> source)
		{
			return source.SubscribeOn(Scheduler.MainThread);
		}

		public static IObservable<bool> EveryApplicationPause()
		{
			return MainThreadDispatcher.OnApplicationPauseAsObservable().AsObservable<bool>();
		}

		public static IObservable<bool> EveryApplicationFocus()
		{
			return MainThreadDispatcher.OnApplicationFocusAsObservable().AsObservable<bool>();
		}

		public static IObservable<Unit> OnceApplicationQuit()
		{
			return MainThreadDispatcher.OnApplicationQuitAsObservable().Take(1);
		}

		public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, Component target)
		{
			return source.TakeUntil(target.OnDestroyAsObservable());
		}

		public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, GameObject target)
		{
			return source.TakeUntil(target.OnDestroyAsObservable());
		}

		public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, Component target)
		{
			return source.TakeUntil(target.OnDisableAsObservable());
		}

		public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, GameObject target)
		{
			return source.TakeUntil(target.OnDisableAsObservable());
		}

		public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, GameObject target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDestroyAsObservable(), target);
		}

		public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, Component target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDestroyAsObservable(), (!(target != null)) ? null : target.get_gameObject());
		}

		public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, GameObject target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDisableAsObservable(), target);
		}

		public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, Component target)
		{
			return Observable.RepeatInfinite<T>(source).RepeatUntilCore(target.OnDisableAsObservable(), (!(target != null)) ? null : target.get_gameObject());
		}

		private static IObservable<T> RepeatUntilCore<T>(this IEnumerable<IObservable<T>> sources, IObservable<Unit> trigger, GameObject lifeTimeChecker)
		{
			return Observable.Create<T>(delegate(IObserver<T> observer)
			{
				bool isFirstSubscribe = true;
				bool isDisposed = false;
				bool isStopped = false;
				IEnumerator<IObservable<T>> e = sources.AsSafeEnumerable<IObservable<T>>().GetEnumerator();
				SerialDisposable subscription = new SerialDisposable();
				SingleAssignmentDisposable schedule = new SingleAssignmentDisposable();
				object gate = new object();
				IDisposable stopper = trigger.Subscribe(delegate(Unit _)
				{
					object gate = gate;
					lock (gate)
					{
						isStopped = true;
						e.Dispose();
						subscription.Dispose();
						schedule.Dispose();
						observer.OnCompleted();
					}
				}, new Action<Exception>(observer.OnError));
				schedule.Disposable = Scheduler.CurrentThread.Schedule(delegate(Action self)
				{
					object gate = gate;
					lock (gate)
					{
						if (!isDisposed)
						{
							if (!isStopped)
							{
								bool flag = false;
								Exception ex = null;
								try
								{
									flag = e.MoveNext();
									if (flag)
									{
										if (e.get_Current() == null)
										{
											throw new InvalidOperationException("sequence is null.");
										}
									}
									else
									{
										e.Dispose();
									}
								}
								catch (Exception ex2)
								{
									ex = ex2;
									e.Dispose();
								}
								if (ex != null)
								{
									stopper.Dispose();
									IObserver<T> observer;
									observer.OnError(ex);
								}
								else if (!flag)
								{
									stopper.Dispose();
									IObserver<T> observer;
									observer.OnCompleted();
								}
								else
								{
									IObservable<T> current = e.get_Current();
									SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
									subscription.Disposable = singleAssignmentDisposable;
									IObserver<T> observer = Observer.Create<T>(new Action<T>(observer.OnNext), new Action<Exception>(observer.OnError), self);
									if (isFirstSubscribe)
									{
										isFirstSubscribe = false;
										singleAssignmentDisposable.Disposable = current.Subscribe(observer);
									}
									else
									{
										MainThreadDispatcher.SendStartCoroutine(Observable.SubscribeAfterEndOfFrame<T>(singleAssignmentDisposable, current, observer, lifeTimeChecker));
									}
								}
							}
						}
					}
				});
				return new CompositeDisposable(new IDisposable[]
				{
					schedule,
					subscription,
					stopper,
					Disposable.Create(delegate
					{
						object gate = gate;
						lock (gate)
						{
							isDisposed = true;
							e.Dispose();
						}
					})
				});
			});
		}

		[DebuggerHidden]
		private static IEnumerator SubscribeAfterEndOfFrame<T>(SingleAssignmentDisposable d, IObservable<T> source, IObserver<T> observer, GameObject lifeTimeChecker)
		{
			Observable.<SubscribeAfterEndOfFrame>c__Iterator16<T> <SubscribeAfterEndOfFrame>c__Iterator = new Observable.<SubscribeAfterEndOfFrame>c__Iterator16<T>();
			<SubscribeAfterEndOfFrame>c__Iterator.d = d;
			<SubscribeAfterEndOfFrame>c__Iterator.lifeTimeChecker = lifeTimeChecker;
			<SubscribeAfterEndOfFrame>c__Iterator.source = source;
			<SubscribeAfterEndOfFrame>c__Iterator.observer = observer;
			<SubscribeAfterEndOfFrame>c__Iterator.<$>d = d;
			<SubscribeAfterEndOfFrame>c__Iterator.<$>lifeTimeChecker = lifeTimeChecker;
			<SubscribeAfterEndOfFrame>c__Iterator.<$>source = source;
			<SubscribeAfterEndOfFrame>c__Iterator.<$>observer = observer;
			return <SubscribeAfterEndOfFrame>c__Iterator;
		}
	}
}
