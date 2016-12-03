using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor"), ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
	public enum Side
	{
		BottomLeft,
		Left,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		Center
	}

	public Camera uiCamera;

	public GameObject container;

	public UIAnchor.Side side = UIAnchor.Side.Center;

	public bool runOnlyOnce = true;

	public Vector2 relativeOffset = Vector2.get_zero();

	public Vector2 pixelOffset = Vector2.get_zero();

	[HideInInspector, SerializeField]
	private UIWidget widgetContainer;

	private Transform mTrans;

	private Animation mAnim;

	private Rect mRect = default(Rect);

	private UIRoot mRoot;

	private bool mStarted;

	private void Awake()
	{
		this.mTrans = base.get_transform();
		this.mAnim = base.GetComponent<Animation>();
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
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.get_gameObject());
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.get_gameObject().get_layer());
		}
		this.Update();
		this.mStarted = true;
	}

	private void Update()
	{
		if (this.mAnim != null && this.mAnim.get_enabled() && this.mAnim.get_isPlaying())
		{
			return;
		}
		bool flag = false;
		UIWidget uIWidget = (!(this.container == null)) ? this.container.GetComponent<UIWidget>() : null;
		UIPanel uIPanel = (!(this.container == null) || !(uIWidget == null)) ? this.container.GetComponent<UIPanel>() : null;
		if (uIWidget != null)
		{
			Bounds bounds = uIWidget.CalculateBounds(this.container.get_transform().get_parent());
			this.mRect.set_x(bounds.get_min().x);
			this.mRect.set_y(bounds.get_min().y);
			this.mRect.set_width(bounds.get_size().x);
			this.mRect.set_height(bounds.get_size().y);
		}
		else if (uIPanel != null)
		{
			if (uIPanel.clipping == UIDrawCall.Clipping.None)
			{
				float num = (!(this.mRoot != null)) ? 0.5f : ((float)this.mRoot.activeHeight / (float)Screen.get_height() * 0.5f);
				this.mRect.set_xMin((float)(-(float)Screen.get_width()) * num);
				this.mRect.set_yMin((float)(-(float)Screen.get_height()) * num);
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
			Transform parent = this.container.get_transform().get_parent();
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
			flag = true;
			this.mRect = this.uiCamera.get_pixelRect();
		}
		float num2 = (this.mRect.get_xMin() + this.mRect.get_xMax()) * 0.5f;
		float num3 = (this.mRect.get_yMin() + this.mRect.get_yMax()) * 0.5f;
		Vector3 vector = new Vector3(num2, num3, 0f);
		if (this.side != UIAnchor.Side.Center)
		{
			if (this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight)
			{
				vector.x = this.mRect.get_xMax();
			}
			else if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom)
			{
				vector.x = num2;
			}
			else
			{
				vector.x = this.mRect.get_xMin();
			}
			if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft)
			{
				vector.y = this.mRect.get_yMax();
			}
			else if (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right)
			{
				vector.y = num3;
			}
			else
			{
				vector.y = this.mRect.get_yMin();
			}
		}
		float width = this.mRect.get_width();
		float height = this.mRect.get_height();
		vector.x += this.pixelOffset.x + this.relativeOffset.x * width;
		vector.y += this.pixelOffset.y + this.relativeOffset.y * height;
		if (flag)
		{
			if (this.uiCamera.get_orthographic())
			{
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
			}
			vector.z = this.uiCamera.WorldToScreenPoint(this.mTrans.get_position()).z;
			vector = this.uiCamera.ScreenToWorldPoint(vector);
		}
		else
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			if (uIPanel != null)
			{
				vector = uIPanel.cachedTransform.TransformPoint(vector);
			}
			else if (this.container != null)
			{
				Transform parent2 = this.container.get_transform().get_parent();
				if (parent2 != null)
				{
					vector = parent2.TransformPoint(vector);
				}
			}
			vector.z = this.mTrans.get_position().z;
		}
		if (this.mTrans.get_position() != vector)
		{
			this.mTrans.set_position(vector);
		}
		if (this.runOnlyOnce && Application.get_isPlaying())
		{
			base.set_enabled(false);
		}
	}
}
