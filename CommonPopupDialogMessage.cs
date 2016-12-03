using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class CommonPopupDialogMessage : MonoBehaviour
{
	public enum PlayType
	{
		Short,
		Long
	}

	[Button("StartPopup", "Start", new object[]
	{
		"Test",
		0
	})]
	public int button1;

	[SerializeField]
	private UILabel Message;

	private UIPanel myPanel;

	private TweenAlpha ta;

	public float moveY;

	public iTween.EaseType ease;

	public float duration;

	private float hideDelay;

	public Vector3 defaultPos;

	private Hashtable tweenHash;

	private void Awake()
	{
		this.myPanel = base.GetComponent<UIPanel>();
		this.defaultPos = base.get_transform().get_localPosition();
		this.moveY = 120f;
		this.ease = iTween.EaseType.easeOutSine;
		this.duration = 0.3f;
		this.hideDelay = 1f;
		this.Message.alignment = NGUIText.Alignment.Left;
		this.myPanel.widgetsAreStatic = true;
		this.tweenHash = new Hashtable();
	}

	private void OnDestroy()
	{
		Mem.Del<UILabel>(ref this.Message);
		Mem.Del<UIPanel>(ref this.myPanel);
		Mem.Del<TweenAlpha>(ref this.ta);
		Mem.Del<Vector3>(ref this.defaultPos);
		Mem.DelHashtableSafe(ref this.tweenHash);
	}

	public void StartPopup(string mes, int messageNo, CommonPopupDialogMessage.PlayType type = CommonPopupDialogMessage.PlayType.Long)
	{
		if (this.Message == null)
		{
			this.Message = base.GetComponentInChildren<UILabel>();
		}
		this.Message.text = mes;
		this.hideDelay = (float)((type != CommonPopupDialogMessage.PlayType.Short) ? 1 : 0);
		base.StartCoroutine(this.StartPopupCor(messageNo));
	}

	[DebuggerHidden]
	private IEnumerator StartPopupCor(int messageNo)
	{
		CommonPopupDialogMessage.<StartPopupCor>c__Iterator46 <StartPopupCor>c__Iterator = new CommonPopupDialogMessage.<StartPopupCor>c__Iterator46();
		<StartPopupCor>c__Iterator.messageNo = messageNo;
		<StartPopupCor>c__Iterator.<$>messageNo = messageNo;
		<StartPopupCor>c__Iterator.<>f__this = this;
		return <StartPopupCor>c__Iterator;
	}

	private void OnComplete()
	{
		this.ta.onFinished.Clear();
		this.ta = TweenAlpha.Begin(base.get_gameObject(), 0.5f, 0f);
		this.ta.SetOnFinished(delegate
		{
			base.get_transform().set_localPosition(this.defaultPos);
			this.myPanel.widgetsAreStatic = true;
		});
		this.ta.delay = this.hideDelay;
	}
}
