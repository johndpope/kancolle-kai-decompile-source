using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISOverlayBtn2 : MonoBehaviour
	{
		private static readonly float BACKBTN_MOVE_TIME = 1f;

		private UIButton _uiOverlayBtn2;

		private void Awake()
		{
			this._uiOverlayBtn2 = base.get_transform().FindChild("OverlayBtn2").GetComponent<UIButton>();
			this._uiOverlayBtn2.GetComponent<Collider2D>().set_enabled(false);
		}

		public void SetBGButtonTouchable(bool value)
		{
			this._uiOverlayBtn2.GetComponent<Collider2D>().set_enabled(value);
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
