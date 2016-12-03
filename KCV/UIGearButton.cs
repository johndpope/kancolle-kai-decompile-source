using System;
using UniRx;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIButton)), RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(UIWidget))]
	public class UIGearButton : MonoBehaviour
	{
		private const float GEAR_BUTTON_ALPHA_TIME = 0.2f;

		[SerializeField]
		private UISprite _uiCenter;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private bool _isRotate = true;

		private UIButton _uiButton;

		private UIWidget _uiWidget;

		private Action _actCallback;

		public bool isRotate
		{
			get
			{
				return this._isRotate;
			}
		}

		public UIButton button
		{
			get
			{
				if (this._uiButton == null)
				{
					this._uiButton = base.GetComponent<UIButton>();
				}
				return this._uiButton;
			}
		}

		public UIWidget widget
		{
			get
			{
				if (this._uiWidget == null)
				{
					this._uiWidget = base.GetComponent<UIWidget>();
				}
				return this._uiWidget;
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return base.GetComponent<BoxCollider2D>();
			}
			set
			{
				base.GetComponent<BoxCollider2D>().set_enabled(value);
			}
		}

		private void Awake()
		{
			if (this._uiCenter == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiCenter, base.get_transform(), "Center");
			}
			if (this._uiGear == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiGear, base.get_transform(), "Gear");
			}
			this._isRotate = true;
			this._actCallback = null;
		}

		private void OnDestroy()
		{
			this._uiCenter = null;
			this._uiGear = null;
			this._actCallback = null;
		}

		private void Update()
		{
			if (this._isRotate)
			{
				this._uiGear.get_transform().Rotate(new Vector3(0f, 0f, 30f) * -Time.get_deltaTime());
			}
		}

		public void Show(Action callback)
		{
			base.get_transform().ValueTo(0f, 1f, 0.2f, iTween.EaseType.easeInSine, delegate(object x)
			{
				this.widget.alpha = Convert.ToSingle(x);
			}, delegate
			{
				this.isColliderEnabled = true;
				if (callback != null)
				{
					callback.Invoke();
				}
			});
		}

		public void Hide(Action callback)
		{
			this.isColliderEnabled = false;
			base.get_transform().ValueTo(1f, 0f, 0.2f, iTween.EaseType.easeInSine, delegate(object x)
			{
				this.widget.alpha = Convert.ToSingle(x);
			}, delegate
			{
				this.isColliderEnabled = true;
				if (callback != null)
				{
					callback.Invoke();
				}
			});
		}

		public void ResetDecideAction()
		{
			this._actCallback = null;
		}

		public void SetDecideAction(Action callback)
		{
			this._actCallback = callback;
			this.button.onClick = Util.CreateEventDelegateList(this, "Decide", null);
		}

		public void Decide()
		{
			this.button.state = UIButtonColor.State.Pressed;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit _)
			{
				this.ResetDecideAction();
				this.button.state = UIButtonColor.State.Normal;
			});
		}
	}
}
