using KCV;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
	public interface UIButtonManagement
	{
		UIButton GetButton();
	}

	[SerializeField]
	private UIButton[] Buttons;

	private Collider2D[] ButtonColliders;

	public UIButton nowForcusButton;

	public int nowForcusIndex;

	public Action IndexChangeAct;

	public bool IsFocusButtonAlwaysHover;

	public bool isLoopIndex;

	public bool isPlaySE;

	public bool isDisableFocus;

	private void Awake()
	{
		this.ButtonColliders = new Collider2D[this.Buttons.Length];
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			if (this.Buttons[i] != null)
			{
				this.Buttons[i].callBack = new UIButton.CallBackSetState(this.ChangeAllButtonState);
				this.ButtonColliders[i] = this.Buttons[i].GetComponent<Collider2D>();
			}
		}
	}

	public void UpdateButtons(UIButtonManager.UIButtonManagement[] managetargets)
	{
		List<UIButton> list = new List<UIButton>();
		for (int i = 0; i < managetargets.Length; i++)
		{
			UIButtonManager.UIButtonManagement uIButtonManagement = managetargets[i];
			list.Add(uIButtonManagement.GetButton());
		}
		this.UpdateButtons(list.ToArray());
	}

	public void UpdateButtons(UIButton[] buttons)
	{
		this.Buttons = buttons;
		this.ButtonColliders = new Collider2D[this.Buttons.Length];
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			this.Buttons[i].callBack = new UIButton.CallBackSetState(this.ChangeAllButtonState);
			this.ButtonColliders[i] = this.Buttons[i].GetComponent<Collider2D>();
		}
	}

	private void Update()
	{
		if (this.IsFocusButtonAlwaysHover && this.nowForcusButton != null && this.nowForcusButton.state != UIButtonColor.State.Hover)
		{
			this.nowForcusButton.SetStateNonCallBack(UIButtonColor.State.Hover);
		}
	}

	public void Decide()
	{
		if (this.nowForcusButton == null)
		{
			return;
		}
		for (int i = 0; i < this.nowForcusButton.onClick.get_Count(); i++)
		{
			this.nowForcusButton.onClick.get_Item(i).Execute();
		}
	}

	public void setAllButtonEnable(bool enable)
	{
		Collider2D[] buttonColliders = this.ButtonColliders;
		for (int i = 0; i < buttonColliders.Length; i++)
		{
			Collider2D collider2D = buttonColliders[i];
			if (collider2D != null)
			{
				collider2D.set_enabled(enable);
			}
		}
		if (enable)
		{
			UIButton[] buttons = this.Buttons;
			for (int j = 0; j < buttons.Length; j++)
			{
				UIButton uIButton = buttons[j];
				if (uIButton != null)
				{
					uIButton.state = UIButtonColor.State.Normal;
				}
			}
		}
	}

	public void setAllButtonActive()
	{
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			this.Buttons[i].SetActive(true);
		}
	}

	public void setDisableButtons(List<int> ButtonIndex)
	{
		this.setAllButtonEnable(true);
		for (int i = 0; i < ButtonIndex.get_Count(); i++)
		{
			this.Buttons[ButtonIndex.get_Item(i)].state = UIButtonColor.State.Disabled;
			if (!this.isDisableFocus)
			{
				this.ButtonColliders[ButtonIndex.get_Item(i)].set_enabled(false);
			}
		}
	}

	public bool moveNextButton()
	{
		return this.movebutton(1);
	}

	public bool movePrevButton()
	{
		return this.movebutton(-1);
	}

	public void setFocus(int index)
	{
		if (index == -1)
		{
			index = 0;
		}
		this.unsetFocus();
		if (this.isButtonFocusAble(this.Buttons[index]))
		{
			if (this.Buttons[index].state == UIButtonColor.State.Disabled)
			{
				this.setDisableFocus(this.Buttons[index], index);
			}
			else
			{
				this.nowForcusButton = this.Buttons[index];
				this.nowForcusIndex = index;
				this.Buttons[index].SetState(UIButtonColor.State.Hover, false);
			}
		}
		else
		{
			this.nowForcusIndex = index;
			this.moveNextButton();
		}
	}

	public void unsetFocus()
	{
		this.nowForcusButton = null;
		this.nowForcusIndex = -1;
		UIButton[] buttons = this.Buttons;
		for (int i = 0; i < buttons.Length; i++)
		{
			UIButton uIButton = buttons[i];
			if (uIButton.state != UIButtonColor.State.Disabled)
			{
				uIButton.state = UIButtonColor.State.Normal;
			}
		}
	}

	public void setButtonDelegate(EventDelegate eventDel)
	{
		UIButton[] buttons = this.Buttons;
		for (int i = 0; i < buttons.Length; i++)
		{
			UIButton uIButton = buttons[i];
			if (uIButton != null)
			{
				uIButton.onClick.Add(eventDel);
			}
		}
	}

	public UIButton[] GetFocusableButtons()
	{
		return this.Buttons;
	}

	private void ChangeAllButtonState(UIButton button)
	{
		if (this.nowForcusButton == button)
		{
			return;
		}
		this.nowForcusButton = button;
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			if (!(this.Buttons[i] == null))
			{
				if (this.Buttons[i].state == UIButtonColor.State.Disabled)
				{
					if (this.isDisableFocus && this.Buttons[i] == button)
					{
						this.Buttons[i].SetState(UIButtonColor.State.Disabled, false);
						this.nowForcusIndex = i;
						this.nowForcusButton = this.Buttons[i];
						if (this.isPlaySE)
						{
							SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
						}
					}
				}
				else if (this.Buttons[i] == button)
				{
					this.nowForcusIndex = i;
					if (this.isPlaySE)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					}
				}
				else
				{
					this.Buttons[i].SetStateNonCallBack(UIButtonColor.State.Normal);
				}
			}
		}
		if (this.IndexChangeAct != null)
		{
			this.IndexChangeAct.Invoke();
		}
	}

	private bool movebutton(int SarchDirection)
	{
		int num = this.nowForcusIndex;
		int num2 = this.nowForcusIndex;
		int arg_20_0 = (!this.isLoopIndex) ? 0 : 1;
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			if (this.isLoopIndex)
			{
				num2 = (int)Util.LoopValue(this.nowForcusIndex + SarchDirection * (i + 1), 0f, (float)(this.Buttons.Length - 1));
			}
			else
			{
				num2 = (int)Util.RangeValue(this.nowForcusIndex + SarchDirection * (i + 1), 0f, (float)(this.Buttons.Length - 1));
			}
			if (this.isButtonFocusAble(this.Buttons[num2]))
			{
				if (this.Buttons[num2].state != UIButtonColor.State.Disabled)
				{
					this.Buttons[num2].SetState(UIButtonColor.State.Hover, false);
				}
				else
				{
					this.setDisableFocus(this.Buttons[num2], num2);
				}
				return num != this.nowForcusIndex;
			}
		}
		return false;
	}

	private void setDisableFocus(UIButton btn, int Index)
	{
		if (this.nowForcusButton != null && this.nowForcusButton.state != UIButtonColor.State.Disabled)
		{
			this.nowForcusButton.SetState(UIButtonColor.State.Normal, false);
		}
		this.nowForcusButton = btn;
		this.nowForcusIndex = Index;
		if (this.IndexChangeAct != null)
		{
			this.IndexChangeAct.Invoke();
		}
	}

	private bool isButtonFocusAble(UIButton btn)
	{
		return (btn.state != UIButtonColor.State.Disabled || this.isDisableFocus) && btn.get_gameObject().get_activeInHierarchy();
	}

	private void OnDestroy()
	{
		this.Buttons = null;
		this.ButtonColliders = null;
		this.nowForcusButton = null;
		this.IndexChangeAct = null;
	}
}
