using DG.Tweening;
using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIPracticeHeader : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Status;

		private void Awake()
		{
			this.mLabel_Status.alpha = 0f;
		}

		public void UpdateHeaderText(string text)
		{
			IEnumerator enumerator = this.UpdateHeaderTextCoroutine(text);
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator UpdateHeaderTextCoroutine(string text)
		{
			UIPracticeHeader.<UpdateHeaderTextCoroutine>c__Iterator155 <UpdateHeaderTextCoroutine>c__Iterator = new UIPracticeHeader.<UpdateHeaderTextCoroutine>c__Iterator155();
			<UpdateHeaderTextCoroutine>c__Iterator.text = text;
			<UpdateHeaderTextCoroutine>c__Iterator.<$>text = text;
			<UpdateHeaderTextCoroutine>c__Iterator.<>f__this = this;
			return <UpdateHeaderTextCoroutine>c__Iterator;
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Status);
		}
	}
}
