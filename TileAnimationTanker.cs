using System;
using UnityEngine;

public class TileAnimationTanker : MonoBehaviour
{
	private UITexture tex;

	private Vector3 start;

	private Vector3 finish;

	private float birth;

	private bool on;

	public void Awake()
	{
		this.tex = base.GetComponent<UITexture>();
		if (this.tex == null)
		{
			Debug.Log("Warning: UITexture not attached");
		}
		this.start = Vector3.get_zero();
		this.finish = Vector3.get_zero();
		this.birth = 0f;
		this.on = false;
	}

	public void Initialize(Vector3 s, Vector3 f)
	{
		this.start = s;
		this.finish = f;
		this.birth = Time.get_time();
		this.on = true;
	}

	public void Update()
	{
		if (this.on)
		{
			Transform expr_11 = base.get_transform();
			expr_11.set_localPosition(expr_11.get_localPosition() + Vector3.Normalize(this.finish - this.start) * 50f * Time.get_deltaTime());
			if (Vector3.Distance(base.get_transform().get_localPosition(), this.start) >= Vector3.Distance(this.start, this.finish))
			{
				base.get_transform().set_localPosition(this.start);
			}
			if (Time.get_time() > this.birth + 6f)
			{
				Object.Destroy(base.get_gameObject());
			}
			else if (Time.get_time() > this.birth + 5f || Vector3.Distance(base.get_transform().get_localPosition(), this.finish) < 50f)
			{
				this.tex.alpha -= Mathf.Min(this.tex.alpha, 50f * Time.get_deltaTime());
			}
			else
			{
				this.tex.alpha = Mathf.Max(0f, Mathf.Sin(4f * Time.get_time()));
			}
		}
	}
}
