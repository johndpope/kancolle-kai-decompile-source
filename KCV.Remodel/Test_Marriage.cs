using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	public class Test_Marriage : MonoBehaviour
	{
		private GameObject go;

		public void Start()
		{
			base.StartCoroutine(this.Initialize());
		}

		[DebuggerHidden]
		public IEnumerator Initialize()
		{
			Test_Marriage.<Initialize>c__IteratorB0 <Initialize>c__IteratorB = new Test_Marriage.<Initialize>c__IteratorB0();
			<Initialize>c__IteratorB.<>f__this = this;
			return <Initialize>c__IteratorB;
		}

		public void DeleteGO()
		{
			Object.Destroy(this.go);
		}
	}
}
