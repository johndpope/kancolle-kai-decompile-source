using System;
using UnityEngine;

public class NowLoadingText : MonoBehaviour
{
	private UIPanel Panel;

	[SerializeField]
	private UITexture textTexture;

	private void Awake()
	{
		this.Panel = base.GetComponent<UIPanel>();
	}

	private void Start()
	{
		this.HideText();
	}

	public void StopAnimation()
	{
		iTween.Stop(base.get_gameObject());
		if (this.Panel == null)
		{
			this.Panel = base.GetComponent<UIPanel>();
		}
		this.Panel.clipOffset = new Vector2(-180f, 0f);
		this.textTexture.color = Color.get_white();
	}

	public void StartAnimation()
	{
		this.textTexture.color = Color.get_gray();
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"from",
			-180,
			"to",
			180,
			"time",
			1.2f,
			"onupdate",
			"UpdateHandler",
			"looptype",
			iTween.LoopType.loop
		}));
	}

	private void UpdateHandler(float value)
	{
		this.Panel.clipOffset = new Vector2(value, 0f);
	}

	public void HideText()
	{
		this.Panel.SetActive(false);
		this.textTexture.SetActive(false);
	}
}
