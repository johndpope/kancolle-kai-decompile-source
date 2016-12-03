using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Popup List"), ExecuteInEditMode]
public class UIPopupList : UIWidgetContainer
{
	public enum Position
	{
		Auto,
		Above,
		Below
	}

	public enum OpenOn
	{
		ClickOrTap,
		RightClick,
		DoubleClick,
		Manual
	}

	public delegate void LegacyEvent(string val);

	private const float animSpeed = 0.15f;

	public static UIPopupList current;

	public UIAtlas atlas;

	public UIFont bitmapFont;

	public Font trueTypeFont;

	public int fontSize = 16;

	public FontStyle fontStyle;

	public string backgroundSprite;

	public string highlightSprite;

	public UIPopupList.Position position;

	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	public List<string> items = new List<string>();

	public List<object> itemData = new List<object>();

	public Vector2 padding = new Vector3(4f, 4f);

	public Color textColor = Color.get_white();

	public Color backgroundColor = Color.get_white();

	public Color highlightColor = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);

	public bool isAnimated = true;

	public bool isLocalized;

	public UIPopupList.OpenOn openOn;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	[HideInInspector, SerializeField]
	private string mSelectedItem;

	[HideInInspector, SerializeField]
	private UIPanel mPanel;

	[HideInInspector, SerializeField]
	private GameObject mChild;

	[HideInInspector, SerializeField]
	private UISprite mBackground;

	[HideInInspector, SerializeField]
	private UISprite mHighlight;

	[HideInInspector, SerializeField]
	private UILabel mHighlightedLabel;

	[HideInInspector, SerializeField]
	private List<UILabel> mLabelList = new List<UILabel>();

	[HideInInspector, SerializeField]
	private float mBgBorder;

	[NonSerialized]
	private GameObject mSelection;

	[HideInInspector, SerializeField]
	private GameObject eventReceiver;

	[HideInInspector, SerializeField]
	private string functionName = "OnSelectionChange";

	[HideInInspector, SerializeField]
	private float textScale;

	[HideInInspector, SerializeField]
	private UIFont font;

	[HideInInspector, SerializeField]
	private UILabel textLabel;

	private UIPopupList.LegacyEvent mLegacyEvent;

	private bool mUseDynamicFont;

	private bool mTweening;

	public Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	public bool isOpen
	{
		get
		{
			return this.mChild != null;
		}
	}

	public string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	public object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			return (num <= -1 || num >= this.itemData.get_Count()) ? null : this.itemData.get_Item(num);
		}
	}

	[Obsolete("Use 'value' instead")]
	public string selection
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	private bool handleEvents
	{
		get
		{
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			return component == null || !component.get_enabled();
		}
		set
		{
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				component.set_enabled(!value);
			}
		}
	}

	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	private int activeFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? this.bitmapFont.defaultSize : this.fontSize;
		}
	}

	private float activeFontScale
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? ((float)this.fontSize / (float)this.bitmapFont.defaultSize) : 1f;
		}
	}

	public void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	public void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	public void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	public void RemoveItem(string text)
	{
		int num = this.items.IndexOf(text);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	public void RemoveItemByData(object data)
	{
		int num = this.itemData.IndexOf(data);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	protected void TriggerCallbacks()
	{
		if (UIPopupList.current != this)
		{
			UIPopupList uIPopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, 1);
			}
			UIPopupList.current = uIPopupList;
		}
	}

	private void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((!(this.bitmapFont != null)) ? 16 : Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale));
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	private void OnValidate()
	{
		Font font = this.trueTypeFont;
		UIFont uIFont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (font != null && (uIFont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
		else if (uIFont != null)
		{
			if (uIFont.isDynamic)
			{
				this.trueTypeFont = uIFont.dynamicFont;
				this.fontStyle = uIFont.dynamicFontStyle;
				this.fontSize = uIFont.defaultSize;
				this.mUseDynamicFont = true;
			}
			else
			{
				this.bitmapFont = uIFont;
				this.mUseDynamicFont = false;
			}
		}
		else
		{
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
	}

	private void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.get_isPlaying())
		{
			if (string.IsNullOrEmpty(this.mSelectedItem))
			{
				if (this.items.get_Count() > 0)
				{
					this.value = this.items.get_Item(0);
				}
			}
			else
			{
				string value = this.mSelectedItem;
				this.mSelectedItem = null;
				this.value = value;
			}
		}
	}

	private void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	private void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			if (this.mHighlight.GetAtlasSprite() == null)
			{
				return;
			}
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (instant || !this.isAnimated)
			{
				this.mHighlight.cachedTransform.set_localPosition(highlightPosition);
			}
			else
			{
				TweenPosition.Begin(this.mHighlight.get_gameObject(), 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine("UpdateTweenPosition");
				}
			}
		}
	}

	private Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.get_zero();
		}
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return Vector3.get_zero();
		}
		float pixelSize = this.atlas.pixelSize;
		float num = (float)atlasSprite.borderLeft * pixelSize;
		float num2 = (float)atlasSprite.borderTop * pixelSize;
		return this.mHighlightedLabel.cachedTransform.get_localPosition() + new Vector3(-num, num2, 1f);
	}

	[DebuggerHidden]
	private IEnumerator UpdateTweenPosition()
	{
		UIPopupList.<UpdateTweenPosition>c__Iterator3 <UpdateTweenPosition>c__Iterator = new UIPopupList.<UpdateTweenPosition>c__Iterator3();
		<UpdateTweenPosition>c__Iterator.<>f__this = this;
		return <UpdateTweenPosition>c__Iterator;
	}

	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
		UIEventListener component = lbl.get_gameObject().GetComponent<UIEventListener>();
		this.value = (component.parameter as string);
		UIPlaySound[] components = base.GetComponents<UIPlaySound>();
		int i = 0;
		int num = components.Length;
		while (i < num)
		{
			UIPlaySound uIPlaySound = components[i];
			if (uIPlaySound.trigger == UIPlaySound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uIPlaySound.audioClip, uIPlaySound.volume, 1f);
			}
			i++;
		}
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
		}
	}

	private void OnItemClick(GameObject go)
	{
		this.Close();
	}

	private void OnKey(KeyCode key)
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.handleEvents)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == 273)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList.get_Item(num - 1), false);
				}
			}
			else if (key == 274)
			{
				if (num + 1 < this.mLabelList.get_Count())
				{
					this.Select(this.mLabelList.get_Item(num + 1), false);
				}
			}
			else if (key == 27)
			{
				this.OnSelect(false);
			}
		}
	}

	private void OnDisable()
	{
		this.Close();
	}

	private void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.Close();
		}
	}

	public void Close()
	{
		base.StopCoroutine("CloseIfUnselected");
		this.mSelection = null;
		if (this.mChild != null)
		{
			this.mLabelList.Clear();
			this.handleEvents = false;
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uIWidget = componentsInChildren[i];
					Color color = uIWidget.color;
					color.a = 0f;
					TweenColor.Begin(uIWidget.get_gameObject(), 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = this.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].set_enabled(false);
					j++;
				}
				Object.Destroy(this.mChild, 0.15f);
			}
			else
			{
				Object.Destroy(this.mChild);
			}
			this.mBackground = null;
			this.mHighlight = null;
			this.mChild = null;
		}
	}

	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.get_gameObject(), 0.15f, color).method = UITweener.Method.EaseOut;
	}

	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.get_localPosition();
		Vector3 localPosition2 = (!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
		widget.cachedTransform.set_localPosition(localPosition2);
		GameObject gameObject = widget.get_gameObject();
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.get_gameObject();
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.set_localScale(new Vector3(1f, num / (float)widget.height, 1f));
		TweenScale.Begin(gameObject, 0.15f, Vector3.get_one()).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.get_localPosition();
			cachedTransform.set_localPosition(new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z));
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	private void OnClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
		{
			return;
		}
		if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
		{
			return;
		}
		this.Show();
	}

	private void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	[DebuggerHidden]
	private IEnumerator CloseIfUnselected()
	{
		UIPopupList.<CloseIfUnselected>c__Iterator4 <CloseIfUnselected>c__Iterator = new UIPopupList.<CloseIfUnselected>c__Iterator4();
		<CloseIfUnselected>c__Iterator.<>f__this = this;
		return <CloseIfUnselected>c__Iterator;
	}

	public void Show()
	{
		if (base.get_enabled() && NGUITools.GetActive(base.get_gameObject()) && this.mChild == null && this.atlas != null && this.isValid && this.items.get_Count() > 0)
		{
			this.mLabelList.Clear();
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.get_transform());
				if (this.mPanel == null)
				{
					return;
				}
			}
			this.handleEvents = true;
			Transform transform = base.get_transform();
			this.mChild = new GameObject("Drop-down List");
			this.mChild.set_layer(base.get_gameObject().get_layer());
			Transform transform2 = this.mChild.get_transform();
			transform2.set_parent(transform.get_parent());
			base.StopCoroutine("CloseIfUnselected");
			if (UICamera.selectedObject == null)
			{
				this.mSelection = base.get_gameObject();
				UICamera.selectedObject = this.mSelection;
			}
			else
			{
				this.mSelection = UICamera.selectedObject;
			}
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (this.openOn == UIPopupList.OpenOn.Manual && this.mSelection != base.get_gameObject())
			{
				vector = transform2.get_parent().InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(UICamera.lastTouchPosition));
				vector2 = vector;
				transform2.set_localPosition(vector);
				vector3 = transform2.get_position();
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.get_parent(), transform, false, false);
				vector = bounds.get_min();
				vector2 = bounds.get_max();
				transform2.set_localPosition(vector);
				vector3 = transform.get_position();
			}
			base.StartCoroutine("CloseIfUnselected");
			transform2.set_localRotation(Quaternion.get_identity());
			transform2.set_localScale(Vector3.get_one());
			this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.get_gameObject());
			this.mBackground.color = this.backgroundColor;
			Vector4 border = this.mBackground.border;
			this.mBgBorder = border.y;
			this.mBackground.cachedTransform.set_localPosition(new Vector3(0f, border.y, 0f));
			this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float num = (float)atlasSprite.borderTop;
			float num2 = (float)this.activeFontSize;
			float activeFontScale = this.activeFontScale;
			float num3 = num2 * activeFontScale;
			float num4 = 0f;
			float num5 = -this.padding.y;
			List<UILabel> list = new List<UILabel>();
			if (!this.items.Contains(this.mSelectedItem))
			{
				this.mSelectedItem = null;
			}
			int i = 0;
			int count = this.items.get_Count();
			while (i < count)
			{
				string text = this.items.get_Item(i);
				UILabel uILabel = NGUITools.AddWidget<UILabel>(this.mChild);
				uILabel.set_name(i.ToString());
				uILabel.pivot = UIWidget.Pivot.TopLeft;
				uILabel.bitmapFont = this.bitmapFont;
				uILabel.trueTypeFont = this.trueTypeFont;
				uILabel.fontSize = this.fontSize;
				uILabel.fontStyle = this.fontStyle;
				uILabel.text = ((!this.isLocalized) ? text : Localization.Get(text));
				uILabel.color = this.textColor;
				uILabel.cachedTransform.set_localPosition(new Vector3(border.x + this.padding.x - uILabel.pivotOffset.x, num5, -1f));
				uILabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				uILabel.alignment = this.alignment;
				list.Add(uILabel);
				num5 -= num3;
				num5 -= this.padding.y;
				num4 = Mathf.Max(num4, uILabel.printedSize.x);
				UIEventListener uIEventListener = UIEventListener.Get(uILabel.get_gameObject());
				uIEventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
				uIEventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
				uIEventListener.onClick = new UIEventListener.VoidDelegate(this.OnItemClick);
				uIEventListener.parameter = text;
				if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
				{
					this.Highlight(uILabel, true);
				}
				this.mLabelList.Add(uILabel);
				i++;
			}
			num4 = Mathf.Max(num4, (vector2.x - vector.x) * activeFontScale - (border.x + this.padding.x) * 2f);
			float num6 = num4;
			Vector3 vector4 = new Vector3(num6 * 0.5f, -num3 * 0.5f, 0f);
			Vector3 vector5 = new Vector3(num6, num3 + this.padding.y, 1f);
			int j = 0;
			int count2 = list.get_Count();
			while (j < count2)
			{
				UILabel uILabel2 = list.get_Item(j);
				NGUITools.AddWidgetCollider(uILabel2.get_gameObject());
				uILabel2.autoResizeBoxCollider = false;
				BoxCollider component = uILabel2.GetComponent<BoxCollider>();
				if (component != null)
				{
					vector4.z = component.get_center().z;
					component.set_center(vector4);
					component.set_size(vector5);
				}
				else
				{
					BoxCollider2D component2 = uILabel2.GetComponent<BoxCollider2D>();
					component2.set_offset(vector4);
					component2.set_size(vector5);
				}
				j++;
			}
			int width = Mathf.RoundToInt(num4);
			num4 += (border.x + this.padding.x) * 2f;
			num5 -= border.y;
			this.mBackground.width = Mathf.RoundToInt(num4);
			this.mBackground.height = Mathf.RoundToInt(-num5 + border.y);
			int k = 0;
			int count3 = list.get_Count();
			while (k < count3)
			{
				UILabel uILabel3 = list.get_Item(k);
				uILabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
				uILabel3.width = width;
				k++;
			}
			float num7 = 2f * this.atlas.pixelSize;
			float num8 = num4 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num7;
			float num9 = num3 + num * num7;
			this.mHighlight.width = Mathf.RoundToInt(num8);
			this.mHighlight.height = Mathf.RoundToInt(num9);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(this.mSelection.get_layer());
				if (uICamera != null)
				{
					flag = (uICamera.cachedCamera.WorldToViewportPoint(vector3).y < 0.5f);
				}
			}
			if (this.isAnimated)
			{
				float bottom = num5 + num3;
				this.Animate(this.mHighlight, flag, bottom);
				int l = 0;
				int count4 = list.get_Count();
				while (l < count4)
				{
					this.Animate(list.get_Item(l), flag, bottom);
					l++;
				}
				this.AnimateColor(this.mBackground);
				this.AnimateScale(this.mBackground, flag, bottom);
			}
			if (flag)
			{
				transform2.set_localPosition(new Vector3(vector.x, vector2.y - num5 - border.y, vector.z));
			}
			vector = transform2.get_localPosition();
			vector2.x = vector.x + (float)this.mBackground.width;
			vector2.y = vector.y - (float)this.mBackground.height;
			vector2.z = vector.z;
			Vector3 vector6 = this.mPanel.CalculateConstrainOffset(vector, vector2);
			Transform expr_862 = transform2;
			expr_862.set_localPosition(expr_862.get_localPosition() + vector6);
		}
		else
		{
			this.OnSelect(false);
		}
	}
}
