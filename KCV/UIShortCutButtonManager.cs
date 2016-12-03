using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UIShortCutButtonManager : MonoBehaviour
	{
		public UIButtonManager ButtonManager;

		[SerializeField]
		private UIGrid grid;

		[SerializeField]
		private Transform ButtonFocus;

		private int[] SceneButtonIndex;

		private void Awake()
		{
			this.ButtonManager.IndexChangeAct = delegate
			{
				this.ChangeCursolPos();
			};
		}

		public void ChangeCursolPos()
		{
			this.ButtonFocus.set_localPosition(this.ButtonManager.nowForcusButton.get_transform().get_localPosition());
		}

		public void HideNowScene()
		{
			string nowScene = SingletonMonoBehaviour<PortObjectManager>.Instance.NowScene;
			UIButton button = SingletonMonoBehaviour<UIShortCutMenu>.Instance.getButton(nowScene);
			if (button != null)
			{
				button.SetActive(false);
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.getButton(Generics.Scene.SaveLoad.ToString()).SetActive(false);
			}
			this.grid.Reposition();
		}

		public void setDisableButton(List<int> DisableList)
		{
			this.ButtonManager.setDisableButtons(DisableList);
			UIButton[] focusableButtons = this.ButtonManager.GetFocusableButtons();
			int i;
			for (i = 0; i < focusableButtons.Length; i++)
			{
				if (DisableList.Exists((int x) => x == i))
				{
					focusableButtons[i].hoverSprite = focusableButtons[i].disabledSprite;
					focusableButtons[i].pressedSprite = focusableButtons[i].disabledSprite;
					focusableButtons[i].defaultColor = Color.get_gray();
					focusableButtons[i].disabledColor = Color.get_gray();
					focusableButtons[i].hover = Color.get_gray();
					focusableButtons[i].pressed = Color.get_gray();
					focusableButtons[i].UpdateColor(true);
				}
				else
				{
					focusableButtons[i].hoverSprite = focusableButtons[i].disabledSprite + "_on";
					focusableButtons[i].pressedSprite = focusableButtons[i].disabledSprite + "_on";
					focusableButtons[i].defaultColor = Color.get_white();
					focusableButtons[i].disabledColor = Color.get_white();
					focusableButtons[i].hover = Color.get_white();
					focusableButtons[i].pressed = Color.get_white();
					focusableButtons[i].UpdateColor(true);
				}
			}
		}

		public void setSelectedBtn(bool isDownKey)
		{
			if (isDownKey)
			{
				this.ButtonManager.moveNextButton();
			}
			else
			{
				this.ButtonManager.movePrevButton();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
	}
}
