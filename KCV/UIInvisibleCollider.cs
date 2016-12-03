using System;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(UIButton))]
	public class UIInvisibleCollider : MonoBehaviour
	{
		[SerializeField]
		private UIWidget _uiInvisibleObject;

		[SerializeField]
		private int _nDepth;

		private UIButton _uiButton;

		private BoxCollider2D _colBox2D;

		private Action _actOnTouch;

		public UIButton button
		{
			get
			{
				return this.GetComponentThis(ref this._uiButton);
			}
		}

		public BoxCollider2D collider2D
		{
			get
			{
				return this.GetComponentThis(ref this._colBox2D);
			}
		}

		public int depth
		{
			get
			{
				return this._nDepth;
			}
			set
			{
				if (this._uiInvisibleObject != null)
				{
					this._nDepth = value;
					this._uiInvisibleObject.depth = value;
				}
			}
		}

		private void Awake()
		{
			this.button.onClick = Util.CreateEventDelegateList(this, "OnTouch", null);
			if (this._uiInvisibleObject == null)
			{
				GameObject gameObject = new GameObject("InvisibleObject");
				gameObject.get_transform().set_parent(base.get_transform());
				gameObject.get_transform().AddComponent<UIWidget>();
				this._uiInvisibleObject = gameObject.GetComponent<UIWidget>();
				this._uiInvisibleObject.depth = this.depth;
			}
			this._uiInvisibleObject.depth = this._nDepth;
		}

		private void OnDestroy()
		{
			Mem.Del<UIWidget>(ref this._uiInvisibleObject);
			Mem.Del<UIButton>(ref this._uiButton);
			Mem.Del<BoxCollider2D>(ref this._colBox2D);
			Mem.Del<Action>(ref this._actOnTouch);
		}

		public void SetOnTouch(Action onTouch)
		{
			this._actOnTouch = onTouch;
		}

		private void OnTouch()
		{
			Dlg.Call(ref this._actOnTouch);
		}
	}
}
