using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root"), ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	public enum Scaling
	{
		Flexible,
		Constrained,
		ConstrainedOnMobiles
	}

	public enum Constraint
	{
		Fit,
		Fill,
		FitWidth,
		FitHeight
	}

	public static List<UIRoot> list = new List<UIRoot>();

	public UIRoot.Scaling scalingStyle;

	public int manualWidth = 1280;

	public int manualHeight = 720;

	public int minimumHeight = 320;

	public int maximumHeight = 1536;

	public bool fitWidth;

	public bool fitHeight = true;

	public bool adjustByDPI;

	public bool shrinkPortraitUI;

	private Transform mTrans;

	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			else
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.FitHeight;
				}
				return UIRoot.Constraint.Fill;
			}
		}
	}

	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Flexible;
			}
			return scaling;
		}
	}

	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 screenSize = NGUITools.screenSize;
				float num = screenSize.x / screenSize.y;
				if (screenSize.y < (float)this.minimumHeight)
				{
					screenSize.y = (float)this.minimumHeight;
					screenSize.x = screenSize.y * num;
				}
				else if (screenSize.y > (float)this.maximumHeight)
				{
					screenSize.y = (float)this.maximumHeight;
					screenSize.x = screenSize.y * num;
				}
				int num2 = Mathf.RoundToInt((!this.shrinkPortraitUI || screenSize.y <= screenSize.x) ? screenSize.y : (screenSize.y / num));
				return (!this.adjustByDPI) ? num2 : NGUIMath.AdjustByDPI((float)num2);
			}
			UIRoot.Constraint constraint = this.constraint;
			if (constraint == UIRoot.Constraint.FitHeight)
			{
				return this.manualHeight;
			}
			Vector2 screenSize2 = NGUITools.screenSize;
			float num3 = screenSize2.x / screenSize2.y;
			float num4 = (float)this.manualWidth / (float)this.manualHeight;
			switch (constraint)
			{
			case UIRoot.Constraint.Fit:
				return (num4 <= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			case UIRoot.Constraint.Fill:
				return (num4 >= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			case UIRoot.Constraint.FitWidth:
				return Mathf.RoundToInt((float)this.manualWidth / num3);
			default:
				return this.manualHeight;
			}
		}
	}

	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			return (num != -1) ? this.GetPixelSizeAdjustment(num) : 1f;
		}
	}

	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(go);
		return (!(uIRoot != null)) ? 1f : uIRoot.pixelSizeAdjustment;
	}

	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	protected virtual void Awake()
	{
		this.mTrans = base.get_transform();
	}

	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.get_gameObject().GetComponent<Camera>();
			componentInChildren.set_enabled(false);
			if (component != null)
			{
				component.set_orthographicSize(1f);
			}
		}
		else
		{
			this.Update();
		}
	}

	private void Update()
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.get_localScale();
				if (Mathf.Abs(localScale.x - num2) > 1.401298E-45f || Mathf.Abs(localScale.y - num2) > 1.401298E-45f || Mathf.Abs(localScale.z - num2) > 1.401298E-45f)
				{
					this.mTrans.set_localScale(new Vector3(num2, num2, num2));
				}
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.get_Count();
		while (i < count)
		{
			UIRoot uIRoot = UIRoot.list.get_Item(i);
			if (uIRoot != null)
			{
				uIRoot.BroadcastMessage(funcName, 1);
			}
			i++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
		else
		{
			int i = 0;
			int count = UIRoot.list.get_Count();
			while (i < count)
			{
				UIRoot uIRoot = UIRoot.list.get_Item(i);
				if (uIRoot != null)
				{
					uIRoot.BroadcastMessage(funcName, param, 1);
				}
				i++;
			}
		}
	}
}
