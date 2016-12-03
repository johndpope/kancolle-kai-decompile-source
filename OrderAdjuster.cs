using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class OrderAdjuster : MonoBehaviour
{
	public enum OrderAdjusterMode
	{
		ZAxisOrder,
		DepthOrder
	}

	public OrderAdjuster.OrderAdjusterMode AjusterMode = OrderAdjuster.OrderAdjusterMode.DepthOrder;

	public int OderDepthOffs = 1;

	public bool isAdjust;

	private void Update()
	{
	}

	private void AdjustZ(Transform tf)
	{
		if (this.isAdjust)
		{
			using (IEnumerator enumerator = tf.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					UIWidget component = transform.GetComponent<UIWidget>();
					UIPanel component2 = transform.GetComponent<UIPanel>();
					OrderAdjuster.OrderAdjusterMode ajusterMode = this.AjusterMode;
					if (ajusterMode != OrderAdjuster.OrderAdjusterMode.ZAxisOrder)
					{
						if (ajusterMode == OrderAdjuster.OrderAdjusterMode.DepthOrder)
						{
							if (component)
							{
								transform.set_localPosition(new Vector3(transform.get_localPosition().x, transform.get_localPosition().y, -(float)component.depth * (float)this.OderDepthOffs));
							}
							else
							{
								this.AdjustZ(transform);
							}
							if (component2)
							{
								transform.set_localPosition(new Vector3(transform.get_localPosition().x, transform.get_localPosition().y, -(float)component2.depth * (float)this.OderDepthOffs));
							}
							else
							{
								this.AdjustZ(transform);
							}
						}
					}
					else
					{
						if (component)
						{
							component.depth = -(int)transform.get_localPosition().z * this.OderDepthOffs;
						}
						else
						{
							this.AdjustZ(transform);
						}
						if (component2)
						{
							component2.depth = -(int)transform.get_localPosition().z * this.OderDepthOffs;
						}
						else
						{
							this.AdjustZ(transform);
						}
					}
				}
			}
		}
	}
}
