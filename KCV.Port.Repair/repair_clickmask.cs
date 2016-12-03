using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class repair_clickmask : MonoBehaviour
	{
		public void set_clickable(bool setting)
		{
			base.GetComponent<Collider2D>().set_enabled(setting);
		}

		public void unclickable_onesec()
		{
			base.GetComponent<Animation>().Play();
		}

		public void unclickable_sec(float time)
		{
			base.StartCoroutine(this._unclickable_sec(time));
		}

		[DebuggerHidden]
		private IEnumerator _unclickable_sec(float time)
		{
			repair_clickmask.<_unclickable_sec>c__IteratorBE <_unclickable_sec>c__IteratorBE = new repair_clickmask.<_unclickable_sec>c__IteratorBE();
			<_unclickable_sec>c__IteratorBE.time = time;
			<_unclickable_sec>c__IteratorBE.<$>time = time;
			<_unclickable_sec>c__IteratorBE.<>f__this = this;
			return <_unclickable_sec>c__IteratorBE;
		}

		public bool get_clickable()
		{
			return base.GetComponent<Collider2D>().get_enabled();
		}
	}
}
