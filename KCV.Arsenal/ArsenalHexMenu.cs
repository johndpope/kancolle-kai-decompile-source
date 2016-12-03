using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalHexMenu : MonoBehaviour
	{
		[SerializeField]
		private UIButton KenzoButton;

		[SerializeField]
		private UIButtonManager MenuButtonManager;

		[SerializeField]
		private GameObject TopMoveMenuButtons;

		[SerializeField]
		private GameObject SubMoveMenuButtons;

		private TaskMainArsenalManager.State[] MenuStateString;

		private bool isSubMenuEnter;

		public int GetIndex()
		{
			return this.MenuButtonManager.nowForcusIndex;
		}

		private void Awake()
		{
			this.MenuStateString = new TaskMainArsenalManager.State[]
			{
				TaskMainArsenalManager.State.KENZOU,
				TaskMainArsenalManager.State.KAIHATSU,
				TaskMainArsenalManager.State.KAITAI,
				TaskMainArsenalManager.State.HAIKI,
				TaskMainArsenalManager.State.KENZOU,
				TaskMainArsenalManager.State.KENZOU_BIG,
				TaskMainArsenalManager.State.YUSOUSEN
			};
			this.MenuButtonManager.setFocus(0);
			this.ChangeControlStateTopMenu(0);
		}

		private void OnDestroy()
		{
			this.KenzoButton = null;
			this.MenuButtonManager = null;
			this.TopMoveMenuButtons = null;
			this.SubMoveMenuButtons = null;
			this.MenuStateString = null;
		}

		private void Start()
		{
			this.MenuButtonManager.IsFocusButtonAlwaysHover = true;
			this.MenuButtonManager.IndexChangeAct = delegate
			{
				this.OnPushTopMenuButton();
			};
		}

		public bool Init()
		{
			this.MenuButtonManager.setAllButtonEnable(true);
			return true;
		}

		public void UpdateButtonForcus()
		{
			this.MenuButtonManager.setAllButtonEnable(true);
			this.MenuButtonManager.setFocus(this.MenuButtonManager.nowForcusIndex);
		}

		public void NextButtonForcus()
		{
			if (this.MenuButtonManager.moveNextButton())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void BackButtonForcus()
		{
			if (this.MenuButtonManager.movePrevButton())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void OnPushTopMenuButton()
		{
			int nowForcusIndex = this.MenuButtonManager.nowForcusIndex;
			this.ChangeControlStateTopMenu(nowForcusIndex);
		}

		private void ChangeControlStateTopMenu(int nowStateNo)
		{
			TaskMainArsenalManager.StateType = this.MenuStateString[nowStateNo];
		}

		public void AllButtonEnable(bool enabled)
		{
			this.MenuButtonManager.setAllButtonEnable(enabled);
		}

		public void unsetFocus()
		{
			this.MenuButtonManager.setFocus(0);
			this.ChangeControlStateTopMenu(0);
			this.MenuButtonManager.unsetFocus();
		}
	}
}
