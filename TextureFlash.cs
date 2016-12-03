using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TextureFlash : MonoBehaviour
{
	private UIBasicSprite parentTex;

	private UIBasicSprite maskTex;

	private int frameCount;

	private bool isUpdate;

	[Button("flash", "フラッシュ", new object[]
	{
		5,
		0.05f
	})]
	public int Flash;

	[Button("MaskFadeExpanding", "マスクエフェクト", new object[]
	{
		2,
		0.5f
	})]
	public int MaskEffect;

	private void Start()
	{
		this.parentTex = base.get_transform().get_parent().GetComponent<UIBasicSprite>();
		if (this.parentTex.get_gameObject().GetComponent<UITexture>())
		{
			this.maskTex = base.get_transform().AddComponent<UITexture>();
			this.maskTex.mainTexture = this.parentTex.mainTexture;
			this.maskTex.width = this.parentTex.width;
			this.maskTex.height = this.parentTex.height;
			this.maskTex.shader = Shader.Find("GUI/Text Shader");
		}
		else
		{
			this.maskTex = base.get_transform().AddComponent<UISprite>();
			((UISprite)this.maskTex).atlas = ((UISprite)this.parentTex).atlas;
			((UISprite)this.maskTex).spriteName = ((UISprite)this.parentTex).spriteName;
			this.maskTex.width = this.parentTex.width;
			this.maskTex.height = this.parentTex.height;
		}
		this.maskTex.set_enabled(false);
		this.maskTex.depth = 100;
	}

	private void Update()
	{
	}

	public void MaskFadeExpanding(float size, float time, bool isWhite = true)
	{
		this.init();
		this.maskTex.set_enabled(true);
		if (!isWhite)
		{
			this.maskTex.color = new Color(this.parentTex.color.r, this.parentTex.color.g, this.parentTex.color.b, 1f);
		}
		else
		{
			this.maskTex.color = Color.get_white();
		}
		TweenScale.Begin(base.get_gameObject(), time, new Vector3(size, size, 1f));
		TweenAlpha.Begin(base.get_gameObject(), time, 0f);
	}

	public void flash(int count, float interval)
	{
		this.init();
		this.maskTex.set_enabled(true);
		base.StartCoroutine(this.flashAction(count, interval));
	}

	[DebuggerHidden]
	private IEnumerator flashAction(int count, float interval)
	{
		TextureFlash.<flashAction>c__Iterator34 <flashAction>c__Iterator = new TextureFlash.<flashAction>c__Iterator34();
		<flashAction>c__Iterator.count = count;
		<flashAction>c__Iterator.interval = interval;
		<flashAction>c__Iterator.<$>count = count;
		<flashAction>c__Iterator.<$>interval = interval;
		<flashAction>c__Iterator.<>f__this = this;
		return <flashAction>c__Iterator;
	}

	public void SetTexFillAmount(float fillAmount)
	{
		if (this.maskTex.type != this.parentTex.type)
		{
			this.maskTex.type = this.parentTex.type;
			this.maskTex.fillDirection = this.parentTex.fillDirection;
		}
		this.maskTex.fillAmount = fillAmount;
	}

	private void init()
	{
		this.maskTex.get_gameObject().get_transform().set_localScale(new Vector3(1f, 1f, 1f));
	}
}
