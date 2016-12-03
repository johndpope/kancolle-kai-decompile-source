using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalTankerDialog : MonoBehaviour
	{
		[SerializeField]
		private UILabel Message;

		[SerializeField]
		private UILabel TankerNum;

		private void Start()
		{
			this.Message = base.GetComponent<UILabel>();
		}

		public void setMessage(int CreateNum, int beforeNum, int afterNum)
		{
			this.Message.text = "輸送船を" + CreateNum + "隻入手しました";
			this.TankerNum.text = beforeNum + "  ▶  " + afterNum;
		}

		private void OnDestroy()
		{
			this.Message = null;
			this.TankerNum = null;
		}
	}
}
