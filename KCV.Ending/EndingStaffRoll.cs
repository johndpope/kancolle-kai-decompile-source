using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Ending
{
	public class EndingStaffRoll : MonoBehaviour
	{
		public UILabel StaffRollLabel;

		private Coroutine cor;

		public int RollSize;

		public float Speed = 60f;

		public bool isFinishRoll;

		public void StartStaffRoll()
		{
			this.cor = base.StartCoroutine(this.UpdateRoll());
		}

		[DebuggerHidden]
		private IEnumerator UpdateRoll()
		{
			EndingStaffRoll.<UpdateRoll>c__Iterator59 <UpdateRoll>c__Iterator = new EndingStaffRoll.<UpdateRoll>c__Iterator59();
			<UpdateRoll>c__Iterator.<>f__this = this;
			return <UpdateRoll>c__Iterator;
		}

		private void OnDestroy()
		{
			if (this.cor != null)
			{
				base.StopCoroutine(this.cor);
				this.cor = null;
			}
		}
	}
}
