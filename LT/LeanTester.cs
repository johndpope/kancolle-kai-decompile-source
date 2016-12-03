using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace LT
{
	public class LeanTester : MonoBehaviour
	{
		public float timeout = 15f;

		public void Start()
		{
			base.StartCoroutine(this.timeoutCheck());
		}

		[DebuggerHidden]
		private IEnumerator timeoutCheck()
		{
			LeanTester.<timeoutCheck>c__Iterator1AD <timeoutCheck>c__Iterator1AD = new LeanTester.<timeoutCheck>c__Iterator1AD();
			<timeoutCheck>c__Iterator1AD.<>f__this = this;
			return <timeoutCheck>c__Iterator1AD;
		}
	}
}
