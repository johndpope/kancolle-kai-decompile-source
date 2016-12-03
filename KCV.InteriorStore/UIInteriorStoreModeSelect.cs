using KCV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIInteriorStoreModeSelect : MonoBehaviour
	{
		public enum SelectMode
		{
			Store,
			Interior
		}

		private Dictionary<UIInteriorStoreModeSelect.SelectMode, UIButton> _dicSelectBtn;

		private KeyControl _clsInput;

		public DelDecideSelectMode delDecideSelectMode
		{
			get;
			set;
		}

		private void Awake()
		{
			this._dicSelectBtn = new Dictionary<UIInteriorStoreModeSelect.SelectMode, UIButton>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(UIInteriorStoreModeSelect.SelectMode)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIInteriorStoreModeSelect.SelectMode selectMode = (UIInteriorStoreModeSelect.SelectMode)((int)enumerator.get_Current());
					this._dicSelectBtn.Add(selectMode, base.get_transform().FindChild(string.Format("Btns/{0}Btn", selectMode)).GetComponent<UIButton>());
					this._dicSelectBtn.get_Item(selectMode).onClick.Add(Util.CreateEventDelegate(this, "_decideSelectBtn", selectMode));
				}
			}
			base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (this._clsInput != null)
			{
				if (this._clsInput.IsChangeIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this._setSelectedBtn((UIInteriorStoreModeSelect.SelectMode)this._clsInput.Index);
				}
				if (this._clsInput.keyState.get_Item(1).down)
				{
					this._decideSelectBtn((UIInteriorStoreModeSelect.SelectMode)this._clsInput.Index);
				}
			}
		}

		public void SetInputEnable(KeyControl input)
		{
			new BaseDialogPopup().Open(base.get_gameObject(), 0f, 0f, 1f, 1f);
			this._clsInput = input;
			this._clsInput.Index = 0;
			this._clsInput.setMinMaxIndex(0, 1);
			this._clsInput.setChangeValue(-1f, 1f, 1f, -1f);
			this._setBtnEnabled(true);
			this._setSelectedBtn((UIInteriorStoreModeSelect.SelectMode)this._clsInput.Index);
		}

		private void _setSelectedBtn(UIInteriorStoreModeSelect.SelectMode nIndex)
		{
			using (Dictionary<UIInteriorStoreModeSelect.SelectMode, UIButton>.Enumerator enumerator = this._dicSelectBtn.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<UIInteriorStoreModeSelect.SelectMode, UIButton> current = enumerator.get_Current();
					if (current.get_Key() == nIndex)
					{
						current.get_Value().state = UIButtonColor.State.Pressed;
					}
					else
					{
						current.get_Value().state = UIButtonColor.State.Normal;
					}
				}
			}
		}

		private void _setBtnEnabled(bool isEnabled)
		{
			using (Dictionary<UIInteriorStoreModeSelect.SelectMode, UIButton>.Enumerator enumerator = this._dicSelectBtn.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<UIInteriorStoreModeSelect.SelectMode, UIButton> current = enumerator.get_Current();
					current.get_Value().isEnabled = isEnabled;
				}
			}
		}

		private void _decideSelectBtn(UIInteriorStoreModeSelect.SelectMode iMode)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this._setBtnEnabled(false);
			this._setSelectedBtn(iMode);
			this._clsInput = null;
			new BaseDialogPopup().Close(base.get_gameObject(), 1f, 1f, 0f, 0f);
			if (this.delDecideSelectMode != null)
			{
				this.delDecideSelectMode(iMode);
			}
		}
	}
}
