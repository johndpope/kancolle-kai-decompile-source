using System;
using System.Collections;
using UnityEngine;

public class DialogAnimation : MonoBehaviour
{
	public enum AnimType
	{
		POPUP,
		FEAD,
		SLIDE
	}

	private const float popTime = 0.3f;

	[SerializeField]
	private GameObject Black;

	private UIWidget BlackWidget;

	[Button("PopUpIn", "ポップアップIN", new object[]
	{

	})]
	public bool inspecterButton1;

	[Button("FadeIn", "フェードIN", new object[]
	{

	})]
	public bool inspecterButton2;

	[Button("PopUpOut", "ポップアップOUT", new object[]
	{

	})]
	public bool inspecterButton3;

	[Button("FadeOut", "フェードOUT", new object[]
	{

	})]
	public bool inspecterButton4;

	private bool isOpen;

	private bool isFinished;

	private Vector3 defaultPosition;

	private Quaternion defaultRotate;

	private Vector3 defaultScale;

	private float defaultAlpha;

	private UIPanel panel;

	private UIWidget DialogTexture;

	public Action OpenAction;

	public Action CloseAction;

	public float fadeTime = 0.2f;

	public bool IsOpen
	{
		get
		{
			return this.isOpen;
		}
	}

	public bool IsFinished
	{
		get
		{
			return this.isFinished;
		}
	}

	public void Awake()
	{
		this.defaultPosition = base.get_transform().get_position();
		this.defaultRotate = base.get_transform().get_localRotation();
		this.defaultScale = base.get_transform().get_localScale();
		this.panel = base.GetComponent<UIPanel>();
		if (this.panel != null)
		{
			this.defaultAlpha = this.panel.alpha;
		}
		this.DialogTexture = base.GetComponent<UIWidget>();
		if (this.DialogTexture != null)
		{
			this.DialogTexture.alpha = 0f;
		}
		if (this.Black != null)
		{
			this.BlackWidget = this.Black.GetComponent<UIWidget>();
			if (this.BlackWidget != null)
			{
				this.BlackWidget.alpha = 0f;
			}
		}
	}

	public void StartAnim(DialogAnimation.AnimType animType, bool isOpen)
	{
		if (!Application.get_isPlaying())
		{
			return;
		}
		this.isOpen = isOpen;
		TweenAlpha component = base.GetComponent<TweenAlpha>();
		if (component != null)
		{
			component.onFinished.Clear();
		}
		iTween.Stop(base.get_gameObject());
		switch (animType)
		{
		case DialogAnimation.AnimType.POPUP:
			if (this.IsOpen)
			{
				this.PopUpIn();
			}
			else
			{
				this.PopUpOut();
			}
			break;
		case DialogAnimation.AnimType.FEAD:
			if (this.IsOpen)
			{
				this.FadeIn();
			}
			else
			{
				this.FadeOut();
			}
			break;
		case DialogAnimation.AnimType.SLIDE:
			if (this.IsOpen)
			{
				this.SlideIn(0, 0.5f);
			}
			else
			{
				this.SlideOut(0, 0.5f);
			}
			break;
		}
		if (this.Black != null)
		{
			float alpha = (!isOpen) ? 0f : 0.5f;
			TweenAlpha.Begin(this.Black, this.fadeTime, alpha);
		}
		this.isFinished = false;
	}

	private void OpenAnimEnd()
	{
		if (this.OpenAction != null)
		{
			this.OpenAction.Invoke();
		}
		this.OpenAction = null;
		this.isFinished = true;
	}

	private void CloseAnimEnd()
	{
		if (this.CloseAction != null)
		{
			this.CloseAction.Invoke();
		}
		this.CloseAction = null;
		this.isFinished = true;
	}

	public void PopUpIn()
	{
		if (this.panel != null)
		{
			TweenAlpha.Begin(base.get_gameObject(), this.fadeTime, 1f);
		}
		else if (this.DialogTexture != null)
		{
			TweenAlpha.Begin(this.DialogTexture.get_gameObject(), this.fadeTime, 1f);
			TweenAlpha.Begin(this.BlackWidget.get_gameObject(), this.fadeTime, 0.5f);
		}
		base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
		Hashtable hashtable = new Hashtable();
		hashtable.Add("x", 0);
		hashtable.Add("y", 0);
		hashtable.Add("time", 0.3f);
		hashtable.Add("easetype", iTween.EaseType.easeOutBack);
		hashtable.Add("oncomplete", "OpenAnimEnd");
		iTween.ScaleFrom(base.get_gameObject(), hashtable);
	}

	public void PopUpOut()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("x", 0);
		hashtable.Add("y", 0);
		hashtable.Add("time", 0.3f);
		hashtable.Add("easetype", iTween.EaseType.easeOutQuad);
		hashtable.Add("oncomplete", "CloseAnimEnd");
		iTween.ScaleTo(base.get_gameObject(), hashtable);
	}

	public void FadeIn()
	{
		base.get_transform().set_localScale(Vector3.get_one());
		TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), this.fadeTime, 1f);
		tweenAlpha.SetOnFinished(new EventDelegate.Callback(this.OpenAnimEnd));
	}

	public void FadeIn(float FadeTime)
	{
		base.get_transform().set_localScale(Vector3.get_one());
		TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), FadeTime, 1f);
		tweenAlpha.SetOnFinished(new EventDelegate.Callback(this.OpenAnimEnd));
	}

	public void SlideIn(int PosX, float time)
	{
		if (this.panel != null)
		{
			this.panel.alpha = 1f;
		}
		Vector3 target = new Vector3((float)PosX, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z);
		base.get_transform().MoveTo(target, time, new Action(this.OpenAnimEnd));
	}

	public void SlideOut(int PosX, float time)
	{
		Vector3 target = new Vector3((float)PosX, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z);
		base.get_transform().MoveTo(target, time, new Action(this.CloseAnimEnd));
	}

	public void FadeOut()
	{
		TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), this.fadeTime, 0f);
		tweenAlpha.SetOnFinished(new EventDelegate.Callback(this.CloseAnimEnd));
	}

	private void OnDestroy()
	{
		this.Black = null;
		this.BlackWidget = null;
		this.panel = null;
		this.DialogTexture = null;
		this.OpenAction = null;
		this.CloseAction = null;
	}
}
