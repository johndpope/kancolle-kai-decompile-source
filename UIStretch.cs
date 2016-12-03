using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Stretch"), ExecuteInEditMode]
public class UIStretch : MonoBehaviour
{
	public enum Style
	{
		None,
		Horizontal,
		Vertical,
		Both,
		BasedOnHeight,
		FillKeepingRatio,
		FitInternalKeepingRatio
	}

	public Camera uiCamera;

	public GameObject container;

	public UIStretch.Style style;

	public bool runOnlyOnce = true;

	public Vector2 relativeSize = Vector2.get_one();

	public Vector2 initialSize = Vector2.get_one();

	public Vector2 borderPadding = Vector2.get_zero();

	[HideInInspector, SerializeField]
	private UIWidget widgetContainer;

	private Transform mTrans;

	private UIWidget mWidget;

	private UISprite mSprite;

	private UIPanel mPanel;

	private UIRoot mRoot;

	private Animation mAnim;

	private Rect mRect;

	private bool mStarted;

	private void Awake()
	{
		this.mAnim = base.GetComponent<Animation>();
		this.mRect = default(Rect);
		this.mTrans = base.get_transform();
		this.mWidget = base.GetComponent<UIWidget>();
		this.mSprite = base.GetComponent<UISprite>();
		this.mPanel = base.GetComponent<UIPanel>();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.get_gameObject();
			this.widgetContainer = null;
		}
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.get_gameObject().get_layer());
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.get_gameObject());
		this.Update();
		this.mStarted = true;
	}

	private void Update()
	{
		if (this.mAnim != null && this.mAnim.get_isPlaying())
		{
			return;
		}
		if (this.style != UIStretch.Style.None)
		{
			UIWidget uIWidget = (!(this.container == null)) ? this.container.GetComponent<UIWidget>() : null;
			UIPanel uIPanel = (!(this.container == null) || !(uIWidget == null)) ? this.container.GetComponent<UIPanel>() : null;
			float num = 1f;
			if (uIWidget != null)
			{
				Bounds bounds = uIWidget.CalculateBounds(base.get_transform().get_parent());
				this.mRect.set_x(bounds.get_min().x);
				this.mRect.set_y(bounds.get_min().y);
				this.mRect.set_width(bounds.get_size().x);
				this.mRect.set_height(bounds.get_size().y);
			}
			else if (uIPanel != null)
			{
				if (uIPanel.clipping == UIDrawCall.Clipping.None)
				{
					float num2 = (!(this.mRoot != null)) ? 0.5f : ((float)this.mRoot.activeHeight / (float)Screen.get_height() * 0.5f);
					this.mRect.set_xMin((float)(-(float)Screen.get_width()) * num2);
					this.mRect.set_yMin((float)(-(float)Screen.get_height()) * num2);
					this.mRect.set_xMax(-this.mRect.get_xMin());
					this.mRect.set_yMax(-this.mRect.get_yMin());
				}
				else
				{
					Vector4 finalClipRegion = uIPanel.finalClipRegion;
					this.mRect.set_x(finalClipRegion.x - finalClipRegion.z * 0.5f);
					this.mRect.set_y(finalClipRegion.y - finalClipRegion.w * 0.5f);
					this.mRect.set_width(finalClipRegion.z);
					this.mRect.set_height(finalClipRegion.w);
				}
			}
			else if (this.container != null)
			{
				Transform parent = base.get_transform().get_parent();
				Bounds bounds2 = (!(parent != null)) ? NGUIMath.CalculateRelativeWidgetBounds(this.container.get_transform()) : NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.get_transform());
				this.mRect.set_x(bounds2.get_min().x);
				this.mRect.set_y(bounds2.get_min().y);
				this.mRect.set_width(bounds2.get_size().x);
				this.mRect.set_height(bounds2.get_size().y);
			}
			else
			{
				if (!(this.uiCamera != null))
				{
					return;
				}
				this.mRect = this.uiCamera.get_pixelRect();
				if (this.mRoot != null)
				{
					num = this.mRoot.pixelSizeAdjustment;
				}
			}
			float num3 = this.mRect.get_width();
			float num4 = this.mRect.get_height();
			if (num != 1f && num4 > 1f)
			{
				float num5 = (float)this.mRoot.activeHeight / num4;
				num3 *= num5;
				num4 *= num5;
			}
			Vector3 vector = (!(this.mWidget != null)) ? this.mTrans.get_localScale() : new Vector3((float)this.mWidget.width, (float)this.mWidget.height);
			if (this.style == UIStretch.Style.BasedOnHeight)
			{
				vector.x = this.relativeSize.x * num4;
				vector.y = this.relativeSize.y * num4;
			}
			else if (this.style == UIStretch.Style.FillKeepingRatio)
			{
				float num6 = num3 / num4;
				float num7 = this.initialSize.x / this.initialSize.y;
				if (num7 < num6)
				{
					float num8 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num8;
				}
				else
				{
					float num9 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num9;
					vector.y = num4;
				}
			}
			else if (this.style == UIStretch.Style.FitInternalKeepingRatio)
			{
				float num10 = num3 / num4;
				float num11 = this.initialSize.x / this.initialSize.y;
				if (num11 > num10)
				{
					float num12 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num12;
				}
				else
				{
					float num13 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num13;
					vector.y = num4;
				}
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x = this.relativeSize.x * num3;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y = this.relativeSize.y * num4;
				}
			}
			if (this.mSprite != null)
			{
				float num14 = (!(this.mSprite.atlas != null)) ? 1f : this.mSprite.atlas.pixelSize;
				vector.x -= this.borderPadding.x * num14;
				vector.y -= this.borderPadding.y * num14;
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mSprite.width = Mathf.RoundToInt(vector.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mSprite.height = Mathf.RoundToInt(vector.y);
				}
				vector = Vector3.get_one();
			}
			else if (this.mWidget != null)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mWidget.width = Mathf.RoundToInt(vector.x - this.borderPadding.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mWidget.height = Mathf.RoundToInt(vector.y - this.borderPadding.y);
				}
				vector = Vector3.get_one();
			}
			else if (this.mPanel != null)
			{
				Vector4 baseClipRegion = this.mPanel.baseClipRegion;
				if (this.style != UIStretch.Style.Vertical)
				{
					baseClipRegion.z = vector.x - this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					baseClipRegion.w = vector.y - this.borderPadding.y;
				}
				this.mPanel.baseClipRegion = baseClipRegion;
				vector = Vector3.get_one();
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x -= this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y -= this.borderPadding.y;
				}
			}
			if (this.mTrans.get_localScale() != vector)
			{
				this.mTrans.set_localScale(vector);
			}
			if (this.runOnlyOnce && Application.get_isPlaying())
			{
				base.set_enabled(false);
			}
		}
	}
}
