using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIPanel))]
	public class BaseShutter : MonoBehaviour
	{
		public enum ShutterMode
		{
			None = -1,
			Close,
			Open
		}

		protected const float SHUTTER_OPENCLOSE_TIME = 0.25f;

		[SerializeField]
		protected UIPanel _uiPanel;

		[SerializeField]
		protected Transform _traShutter;

		[SerializeField]
		protected UISprite _uiTop;

		[SerializeField]
		protected UISprite _uiBtm;

		[SerializeField]
		protected BoxCollider2D _colBox2D;

		protected bool _isTween;

		protected Vector3[] _vTopPos = new Vector3[]
		{
			new Vector3(0f, 272f, 0f),
			new Vector3(0f, 575f, 0f)
		};

		protected Vector3[] _vBtnPos = new Vector3[]
		{
			new Vector3(0f, -272f, 0f),
			new Vector3(0f, -575f, 0f)
		};

		protected BaseShutter.ShutterMode _iShutterMode;

		protected Action _actCallback;

		public int panelDepth
		{
			get
			{
				return this._uiPanel.depth;
			}
			set
			{
				if (this._uiPanel.depth != value)
				{
					this._uiPanel.depth = value;
				}
			}
		}

		protected virtual void Awake()
		{
			if (this._uiPanel == null)
			{
				this._uiPanel = base.GetComponent<UIPanel>();
			}
			if (this._traShutter == null)
			{
				Util.FindParentToChild<Transform>(ref this._traShutter, this._uiPanel.get_transform(), "Shutter");
			}
			if (this._uiBtm == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiBtm, this._traShutter, "Btm");
			}
			if (this._uiTop == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiTop, this._traShutter, "Top");
			}
			this._actCallback = null;
			this._iShutterMode = BaseShutter.ShutterMode.Open;
			this._uiTop.get_transform().set_localPosition(this._vTopPos[1]);
			this._uiBtm.get_transform().set_localPosition(this._vBtnPos[1]);
		}

		private void OnDestroy()
		{
			this.UnInit();
		}

		public virtual bool Init(BaseShutter.ShutterMode iMode)
		{
			this._uiTop.get_transform().set_localPosition(this._vTopPos[(int)iMode]);
			this._uiBtm.get_transform().set_localPosition(this._vBtnPos[(int)iMode]);
			this._iShutterMode = iMode;
			return true;
		}

		public virtual bool UnInit()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Transform>(ref this._traShutter);
			if (this._uiTop != null)
			{
				this._uiTop.Clear();
			}
			Mem.Del(ref this._uiTop);
			if (this._uiBtm != null)
			{
				this._uiBtm.Clear();
			}
			Mem.Del(ref this._uiBtm);
			Mem.Del<bool>(ref this._isTween);
			Mem.DelArySafe<Vector3>(ref this._vTopPos);
			Mem.DelArySafe<Vector3>(ref this._vBtnPos);
			Mem.Del<BaseShutter.ShutterMode>(ref this._iShutterMode);
			Mem.Del<Action>(ref this._actCallback);
			return true;
		}

		public virtual void SetLayer(int nPanelDepth)
		{
			this._uiPanel.depth = nPanelDepth;
		}

		public virtual void ReqMode(BaseShutter.ShutterMode iMode, Action callback)
		{
			if (iMode == BaseShutter.ShutterMode.None)
			{
				return;
			}
			if (this._iShutterMode == iMode)
			{
				return;
			}
			this._actCallback = callback;
			if (!this._isTween)
			{
				if (iMode == BaseShutter.ShutterMode.Close)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_921);
				}
				Hashtable hashtable = new Hashtable();
				hashtable.Add("time", 0.25f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easetype", iTween.EaseType.easeOutBounce);
				hashtable.Add("oncompletetarget", base.get_gameObject());
				hashtable.Add("oncomplete", "OnShutterActionComplate");
				hashtable.Add("position", this._vTopPos[(int)iMode]);
				iTween.MoveTo(this._uiTop.get_gameObject(), hashtable);
				hashtable.Remove("position");
				hashtable.Remove("oncompletetarget");
				hashtable.Remove("oncomplete");
				hashtable.Add("position", this._vBtnPos[(int)iMode]);
				iTween.MoveTo(this._uiBtm.get_gameObject(), hashtable);
			}
			this._iShutterMode = iMode;
		}

		protected virtual void OnShutterActionComplate()
		{
			this._isTween = false;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
