using System;
using UnityEngine;

public class AutoScaleQuad : MonoBehaviour
{
	public enum ScaleType
	{
		Fit,
		Box
	}

	public Transform targetQuad;

	public AutoScaleQuad.ScaleType scaleType = AutoScaleQuad.ScaleType.Box;

	public bool scalableMask;

	private void Update()
	{
		this.UpdateScale();
	}

	[ContextMenu("execute")]
	private void UpdateScale()
	{
		float num = base.GetComponent<Camera>().get_orthographicSize() * 2f;
		float num2 = num * base.GetComponent<Camera>().get_aspect();
		if (this.scaleType == AutoScaleQuad.ScaleType.Box)
		{
			float num3 = Mathf.Max(num2, num);
			this.targetQuad.get_transform().set_localScale(new Vector3(num3, num3, 0f));
		}
		else
		{
			this.targetQuad.get_transform().set_localScale(new Vector3(num2, num, 0f));
		}
		this.targetQuad.get_transform().set_localPosition(Vector3.get_zero() + base.get_transform().get_forward());
		if (this.scalableMask)
		{
			float num4 = num / num2;
			this.targetQuad.GetComponent<Renderer>().get_material().SetTextureScale("_MaskTex", new Vector2(1f, num4));
			this.targetQuad.GetComponent<Renderer>().get_material().SetTextureOffset("_MaskTex", new Vector2(0f, (1f - num4) / 2f));
		}
		base.set_enabled(false);
	}
}
