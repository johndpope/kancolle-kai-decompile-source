using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISBackBtn : MonoBehaviour
	{
		private static readonly float BACKBTN_MOVE_TIME = 1f;

		private UIButton _uiBackBtn;

		private Vector3[] _vPos = new Vector3[]
		{
			new Vector3(-580f, -248f, 0f),
			new Vector3(-385f, -248f, -0f)
		};

		private EventDelegate.Callback _actCallback;

		private void Awake()
		{
			Util.FindParentToChild<UIButton>(ref this._uiBackBtn, base.get_transform(), "BackBtn");
			base.get_transform().set_localPosition(this._vPos[0]);
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void ReqMode(bool isScreenIn)
		{
			Vector3 vector = (!isScreenIn) ? this._vPos[0] : this._vPos[1];
			this.ColliderEnabled(isScreenIn);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", vector);
			hashtable.Add("time", UIISBackBtn.BACKBTN_MOVE_TIME);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			hashtable.Add("isLocal", true);
			iTween.MoveTo(base.get_gameObject(), hashtable);
			hashtable.Clear();
		}

		public void ColliderEnabled(bool isEnabled)
		{
			this._uiBackBtn.isEnabled = isEnabled;
		}

		public void AddDelegade(EventDelegate.Callback callback)
		{
			this._actCallback = callback;
			this._uiBackBtn.onClick.Clear();
			EventDelegate.Add(this._uiBackBtn.onClick, delegate
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				this._actCallback();
				this.DelayAction(0.2f, delegate
				{
					this._uiBackBtn.state = UIButtonColor.State.Normal;
				});
			});
		}
	}
}
