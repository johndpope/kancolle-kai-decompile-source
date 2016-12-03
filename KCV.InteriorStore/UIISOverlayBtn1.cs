using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISOverlayBtn1 : MonoBehaviour
	{
		private static readonly float BACKBTN_MOVE_TIME = 1f;

		private UIButton _uiOverlayBtn1;

		private void Awake()
		{
			this._uiOverlayBtn1 = base.get_transform().FindChild("OverlayBtn1").GetComponent<UIButton>();
			this._uiOverlayBtn1.GetComponent<Collider2D>().set_enabled(false);
		}

		public void SetBGButtonTouchable(bool value)
		{
			this._uiOverlayBtn1.GetComponent<Collider2D>().set_enabled(value);
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
