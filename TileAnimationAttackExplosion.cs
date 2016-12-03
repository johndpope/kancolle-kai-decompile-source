using System;
using UnityEngine;

public class TileAnimationAttackExplosion : MonoBehaviour
{
	private UITexture tex;

	public void Awake()
	{
		this.tex = base.GetComponent<UITexture>();
		if (this.tex == null)
		{
			Debug.Log("Warning: UITexture not attached");
		}
		this.tex.alpha = 0f;
		base.get_transform().set_localScale(0.01f * Vector3.get_one());
	}

	public void Initialize()
	{
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"from",
			0,
			"to",
			1,
			"time",
			0.1f,
			"onupdate",
			"Alpha",
			"onupdatetarget",
			base.get_gameObject()
		}));
		iTween.ScaleTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"scale",
			2f * Vector3.get_one(),
			"islocal",
			true,
			"time",
			0.5f,
			"easeType",
			iTween.EaseType.easeOutQuad
		}));
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"from",
			1,
			"to",
			0,
			"time",
			0.1f,
			"delay",
			0.4f,
			"onupdate",
			"Alpha",
			"onupdatetarget",
			base.get_gameObject()
		}));
	}

	public void Alpha(float f)
	{
		this.tex.alpha = f;
	}
}
