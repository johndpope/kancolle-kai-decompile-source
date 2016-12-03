using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalParamBG : MonoBehaviour
	{
		private GameObject _uiOverlayBtn;

		private bool _BGtouched;

		public bool isEnable;

		private void Start()
		{
			this._uiOverlayBtn = base.get_gameObject();
			this._BGtouched = false;
		}

		public void _onClickOverlayButton()
		{
			this._BGtouched = true;
		}

		public void set_touch(bool value)
		{
			this._BGtouched = value;
		}

		public bool get_touch()
		{
			return this._BGtouched;
		}

		private void OnDestroy()
		{
			this._uiOverlayBtn = null;
		}
	}
}
