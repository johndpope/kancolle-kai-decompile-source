using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class mess_done : MonoBehaviour
	{
		private repair rep;

		private void Start()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
		}

		private void OnDestroy()
		{
			Mem.Del<repair>(ref this.rep);
		}

		public void rep_reset(int mode)
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.set_mode(mode);
		}
	}
}
