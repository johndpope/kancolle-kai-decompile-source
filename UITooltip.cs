using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	protected static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public UISprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	protected GameObject mHover;

	protected Transform mTrans;

	protected float mTarget;

	protected float mCurrent;

	protected Vector3 mPos;

	protected Vector3 mSize = Vector3.get_zero();

	protected UIWidget[] mWidgets;

	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	protected virtual void Start()
	{
		this.mTrans = base.get_transform();
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.get_localPosition();
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.get_gameObject().get_layer());
		}
		this.SetAlpha(0f);
	}

	protected virtual void Update()
	{
		if (this.mHover != UICamera.hoveredObject)
		{
			this.mHover = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector = this.mSize * 0.25f;
				vector.y = -vector.y;
				Vector3 localScale = Vector3.get_one() * (1.5f - this.mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
				this.mTrans.set_localPosition(localPosition);
				this.mTrans.set_localScale(localScale);
			}
		}
	}

	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uIWidget = this.mWidgets[i];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
			i++;
		}
	}

	protected virtual void SetText(string tooltipText)
	{
		if (this.text != null && !string.IsNullOrEmpty(tooltipText))
		{
			this.mTarget = 1f;
			this.mHover = UICamera.hoveredObject;
			this.text.text = tooltipText;
			this.mPos = Input.get_mousePosition();
			Transform transform = this.text.get_transform();
			Vector3 localPosition = transform.get_localPosition();
			Vector3 localScale = transform.get_localScale();
			this.mSize = this.text.printedSize;
			this.mSize.x = this.mSize.x * localScale.x;
			this.mSize.y = this.mSize.y * localScale.y;
			if (this.background != null)
			{
				Vector4 border = this.background.border;
				this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
				this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
				this.background.width = Mathf.RoundToInt(this.mSize.x);
				this.background.height = Mathf.RoundToInt(this.mSize.y);
			}
			if (this.uiCamera != null)
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.get_width());
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.get_height());
				float num = this.uiCamera.get_orthographicSize() / this.mTrans.get_parent().get_lossyScale().y;
				float num2 = (float)Screen.get_height() * 0.5f / num;
				Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.get_width(), num2 * this.mSize.y / (float)Screen.get_height());
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
				this.mTrans.set_position(this.uiCamera.ViewportToWorldPoint(this.mPos));
				this.mPos = this.mTrans.get_localPosition();
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.set_localPosition(this.mPos);
			}
			else
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.get_width())
				{
					this.mPos.x = (float)Screen.get_width() - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				this.mPos.x = this.mPos.x - (float)Screen.get_width() * 0.5f;
				this.mPos.y = this.mPos.y - (float)Screen.get_height() * 0.5f;
			}
		}
		else
		{
			this.mHover = null;
			this.mTarget = 0f;
		}
	}

	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mHover = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}
}
