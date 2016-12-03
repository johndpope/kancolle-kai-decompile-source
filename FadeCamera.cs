using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FadeCamera : SingletonMonoBehaviour<FadeCamera>
{
	public enum TransitionRule
	{
		NONE,
		Transition1,
		Transition2
	}

	public MeshRenderer targetRender;

	private float goalTime;

	private float time;

	private Material material;

	private List<Action> action;

	private static readonly string cutoff = "_Cutoff";

	private static readonly string mainTex = "_MainTex";

	private static readonly string maskTex = "_MaskTex";

	public bool isCutoff;

	public bool isDrawNowLoading;

	private bool isWithOutNowLoading = true;

	private string[] TransitionFilePath = new string[]
	{
		"Textures/Common/Mask/Overlay",
		"Textures/rule/101",
		"Textures/rule/160"
	};

	private FadeCamera.TransitionRule nowRule = FadeCamera.TransitionRule.Transition1;

	public bool isFadeOut
	{
		get;
		private set;
	}

	public bool fading
	{
		get
		{
			return this.goalTime > Time.get_time();
		}
	}

	public void SetWithOutNowLoading(bool isWithOut)
	{
		this.isWithOutNowLoading = isWithOut;
	}

	protected override void Awake()
	{
		base.Awake();
		this.material = this.targetRender.get_material();
		bool enabled = this.isCutoff;
		float num = (!this.isCutoff) ? 1f : -1f;
		base.GetComponent<Camera>().set_enabled(enabled);
		this.material.SetFloat(FadeCamera.cutoff, num);
		this.action = new List<Action>();
		this.isFadeOut = false;
		this.isDrawNowLoading = false;
	}

	private void OnEnable()
	{
		base.GetComponent<Camera>().set_enabled(true);
	}

	private void OnDisable()
	{
		if (!this.isFadeOut)
		{
			base.GetComponent<Camera>().set_enabled(false);
		}
	}

	public void UpdateTexture(Texture texture)
	{
		this.material.SetTexture(FadeCamera.mainTex, texture);
	}

	public void UpdateMaskTexture(Texture texture)
	{
		this.material.SetTexture(FadeCamera.maskTex, texture);
	}

	public void FadeOut(float requestTime, Action act)
	{
		this.isFadeOut = true;
		this.TimerSetup(requestTime, act);
	}

	public void FadeOutNotNowLoading(float requestTime, Action act)
	{
		this.isWithOutNowLoading = true;
		this.FadeOut(requestTime, act);
	}

	public void FadeIn(float requestTime, Action act)
	{
		if (!this.isWithOutNowLoading)
		{
			SingletonMonoBehaviour<NowLoadingAnimation>.Instance.EndAnimation();
		}
		this.isFadeOut = false;
		this.TimerSetup(requestTime, act);
	}

	private void TimerSetup(float requestTime, Action act)
	{
		this.targetRender.get_gameObject().SetActive(true);
		this.action.Clear();
		if (act != null)
		{
			this.action.Add(act);
		}
		this.time = requestTime;
		this.goalTime = Time.get_time() + this.time;
		base.set_enabled(true);
		float num = -1.2f;
		float num2 = 1.2f;
		iTween.Stop(base.get_gameObject());
		Hashtable hashtable = new Hashtable();
		float @float = this.material.GetFloat(FadeCamera.cutoff);
		if (this.isFadeOut)
		{
			hashtable.Add("from", @float);
			hashtable.Add("to", num);
			float num3 = (1f - @float) / 2f;
			requestTime *= 1f - num3;
		}
		else
		{
			hashtable.Add("from", @float);
			hashtable.Add("to", num2);
			float num4 = (1f + @float) / 2f;
			requestTime *= 1f - num4;
		}
		if (requestTime < 0f)
		{
			requestTime = 0f;
		}
		hashtable.Add("time", requestTime);
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("onupdate", "UpdateHandler");
		hashtable.Add("oncomplete", "OnCompleteHandler");
		iTween.ValueTo(base.get_gameObject(), hashtable);
	}

	private void UpdateHandler(float value)
	{
		this.material.SetFloat(FadeCamera.cutoff, value);
	}

	private void OnCompleteHandler()
	{
		if (this.action.get_Count() != 0)
		{
			for (int i = 0; i < this.action.get_Count(); i++)
			{
				this.action.get_Item(i).Invoke();
				this.action.set_Item(i, null);
			}
		}
		this.action.Clear();
		if (this.isFadeOut && this.isDrawNowLoading && !this.isWithOutNowLoading && !SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isYouseiExist)
		{
			SingletonMonoBehaviour<NowLoadingAnimation>.Instance.StartAnimation(Random.Range(1, 10));
		}
		this.isWithOutNowLoading = false;
		this.targetRender.get_gameObject().SetActive(this.isFadeOut);
		base.set_enabled(false);
	}

	public void SetTransitionRule(FadeCamera.TransitionRule rule)
	{
	}

	private void OnDestroy()
	{
		this.targetRender = null;
	}
}
