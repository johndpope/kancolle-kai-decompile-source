using SpicyPixel.Threading.Tasks;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SpicyPixel.Threading
{
	public class ConcurrencyKitSample : ConcurrentBehaviour
	{
		private void Start()
		{
			this.Log("Starting concurrent tasks ...");
			Task.WhenAll(new Task[]
			{
				this.RunWithConcurrentBehaviourScheduler(),
				this.RunWithSharedScheduler()
			}).ContinueWith(delegate(Task t)
			{
				this.Log("Finished all concurrent tasks.");
			});
		}

		private Task RunWithConcurrentBehaviourScheduler()
		{
			return base.get_taskFactory().StartNew(delegate
			{
				this.Log("Starting a new task on the main Unity thread ...");
			}).ContinueWith(delegate(Task antecedent)
			{
				this.Log("Waiting 2s on a thread pool thread ...");
				Thread.Sleep(2000);
			}).ContinueWith(delegate(Task antecedent)
			{
				this.Log("Continued with another task on the main Unity thread after 2 seconds.");
			}, base.get_taskScheduler()).ContinueWith(delegate(Task antecedent)
			{
				this.Log("Waiting 2s on a thread pool thread ...");
				Thread.Sleep(2000);
			}).ContinueWith(delegate(Task antecedent)
			{
				this.Log("Continued with another task on the main Unity thread after 2 more seconds.");
			}, base.get_taskScheduler());
		}

		private Task RunWithSharedScheduler()
		{
			return FiberTaskExtensions.StartNew(UnityTaskFactory.get_Default(), this.ExampleCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator ExampleCoroutine()
		{
			ConcurrencyKitSample.<ExampleCoroutine>c__IteratorA <ExampleCoroutine>c__IteratorA = new ConcurrencyKitSample.<ExampleCoroutine>c__IteratorA();
			<ExampleCoroutine>c__IteratorA.<>f__this = this;
			return <ExampleCoroutine>c__IteratorA;
		}

		private void OnGUI()
		{
			GUI.Label(new Rect((float)(Screen.get_width() / 2 - 200), (float)(Screen.get_height() / 2 - 50), 400f, 100f), "This example outputs information to the console. It demonstrates how tasks can be chained together to run in sequence and how a blocking call like Thread.Sleep() can be scheduled so that it does not block the main Unity thread.");
		}

		private void Log(string message)
		{
			Debug.Log(DateTime.get_Now().ToString("HH:mm:ss.fff") + " " + message);
		}
	}
}
