using Common.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InheritDifficultySelecter : MonoBehaviour
{
	public enum Difficulty
	{
		SHI,
		KOU,
		OTU,
		HEI,
		TYOU
	}

	public UIButtonManager btnMng;

	public UIWidget uiWidget;

	private UIGrid grid;

	[SerializeField]
	private Transform DifficultyCursol;

	private readonly int[] exchange = new int[]
	{
		5,
		4,
		3,
		2,
		1
	};

	public void Init(int EnableNum, MonoBehaviour LoadSelect)
	{
		this.btnMng.setFocus(3);
		UIButton[] focusableButtons = this.btnMng.GetFocusableButtons();
		int num = focusableButtons.Length - EnableNum;
		List<int> list = new List<int>();
		for (int i = focusableButtons.Length - 1; i >= 0; i--)
		{
			bool flag = i >= num;
			focusableButtons[i].SetActive(flag);
			if (!flag)
			{
				list.Add(i);
			}
		}
		this.btnMng.setDisableButtons(list);
		this.grid = this.btnMng.GetComponent<UIGrid>();
		this.grid.Reposition();
		this.MoveCursol();
		this.btnMng.setButtonDelegate(Util.CreateEventDelegate(LoadSelect, "OnDesideDifficulty", null));
		this.btnMng.IndexChangeAct = new Action(this.MoveCursol);
	}

	public void MoveNextButton()
	{
		this.btnMng.moveNextButton();
	}

	public void MovePrevButton()
	{
		this.btnMng.movePrevButton();
	}

	private void MoveCursol()
	{
		this.DifficultyCursol.MoveTo(this.btnMng.nowForcusButton.get_transform().get_position(), 0.2f, iTween.EaseType.easeOutQuint, null);
	}

	public DifficultKind getDifficultKind()
	{
		return (DifficultKind)this.exchange[this.btnMng.nowForcusIndex];
	}
}
