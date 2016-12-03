using Common.Enum;
using KCV.Generic;
using KCV.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	public class UICommandSurfaceList : MonoBehaviour
	{
		[Serializable]
		private struct Frame : IDisposable
		{
			public UILabelButton battleStart;

			public UITexture bottomLine;

			public UITexture topLine;

			public UILabel label;

			public UILabel detectionResult;

			public void Dispose()
			{
				Mem.Del<UILabelButton>(ref this.battleStart);
				Mem.Del<UITexture>(ref this.bottomLine);
				Mem.Del<UITexture>(ref this.topLine);
				Mem.Del<UILabel>(ref this.label);
				Mem.Del<UILabel>(ref this.detectionResult);
			}
		}

		[SerializeField]
		private Transform _prefabUILabelButton;

		[SerializeField]
		private Transform _uiSurfaceAnchor;

		[SerializeField]
		private UICommandSurfaceList.Frame _strFrame;

		private List<ISelectedObject<int>> _listISelectSurface;

		private int _nIndex;

		private UIWidget _uiWidget;

		private Action<UICommandLabelButton> _actOnSelectedSurface;

		private Predicate<List<BattleCommand>> _preOnDecideCommand;

		public int index
		{
			get
			{
				return this._nIndex;
			}
			private set
			{
				this._nIndex = value;
			}
		}

		public UICommandLabelButton selectedSurface
		{
			get
			{
				if (this._listISelectSurface.get_Item(this.index) is UICommandLabelButton)
				{
					return this._listISelectSurface.get_Item(this.index) as UICommandLabelButton;
				}
				return null;
			}
		}

		public bool isColliderEnabled
		{
			set
			{
				this._listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
				{
					x.toggle.set_enabled(x.isValid && value);
				});
			}
		}

		public bool Init(List<BattleCommand> commands, Action<UICommandLabelButton> onSelectedSurface, Predicate<List<BattleCommand>> onDecideCommand)
		{
			this.SetDetectionResult(BattleCutManager.GetBattleManager().GetSakutekiData().value_f);
			this.index = 0;
			this._actOnSelectedSurface = onSelectedSurface;
			this._preOnDecideCommand = onDecideCommand;
			this._listISelectSurface = new List<ISelectedObject<int>>();
			this.CreateCommandLabel(commands);
			int firstFocusIndex = (!this.CheckBattleStartState()) ? 0 : commands.get_Count();
			this.index = firstFocusIndex;
			this._listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				x.isFocus = (x.index == firstFocusIndex);
			});
			return true;
		}

		private void CreateCommandLabel(List<BattleCommand> presetList)
		{
			int num = 0;
			using (List<BattleCommand>.Enumerator enumerator = presetList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleCommand current = enumerator.get_Current();
					this._listISelectSurface.Add(UICommandLabelButton.Instantiate(this._prefabUILabelButton.GetComponent<UICommandLabelButton>(), this._uiSurfaceAnchor.get_transform(), Vector3.get_zero(), num, current, new Func<bool>(this.CheckBattleStartState)));
					this._listISelectSurface.get_Item(num).toggle.get_transform().set_localPosition(Vector3.get_down() * (float)(50 * num));
					num++;
				}
			}
			UILabelButton battleStart = this._strFrame.battleStart;
			battleStart.Init(presetList.get_Count(), false, KCVColor.ConvertColor(170f, 170f, 170f, 255f), KCVColor.ConvertColor(170f, 170f, 170f, 128f));
			this._listISelectSurface.Add(battleStart);
			this._listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				x.toggle.group = 15;
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", x.index);
				x.toggle.onDecide = delegate
				{
					if (x is UICommandLabelButton)
					{
						this.OnSelectSurface(x as UICommandLabelButton);
					}
					else
					{
						this.OnSelectBattleStart();
					}
				};
			});
		}

		private void SetDetectionResult(BattleSearchValues iValues)
		{
			string text = string.Empty;
			switch (iValues)
			{
			case BattleSearchValues.Success:
			case BattleSearchValues.Success_Lost:
			case BattleSearchValues.Found:
				text = "成功";
				break;
			case BattleSearchValues.Lost:
			case BattleSearchValues.Faile:
			case BattleSearchValues.NotFound:
				text = "失敗";
				break;
			}
			this._strFrame.detectionResult.text = string.Format("索敵結果[{0}]", text);
		}

		private void PreparaNext(bool isFoward)
		{
			int index = this.index;
			this.index = Mathe.NextElement(this.index, 0, this._listISelectSurface.get_Count() - 1, isFoward, (int x) => this._listISelectSurface.get_Item(x).isValid);
			if (index != this.index)
			{
				this.ChangeFocus(this.index);
			}
		}

		public void Prev()
		{
			this.PreparaNext(false);
		}

		public void Next()
		{
			this.PreparaNext(true);
		}

		private void ChangeFocus(int nIndex)
		{
			this._listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				x.isFocus = (x.index == nIndex);
			});
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
		}

		public void RemoveUnit()
		{
			if (this.selectedSurface == null)
			{
				return;
			}
			this.selectedSurface.SetCommand(BattleCommand.None);
			this.CheckBattleStartState();
		}

		public void RemoveUnitAll()
		{
			this._listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				if (x is UICommandLabelButton)
				{
					((UICommandLabelButton)x).SetCommand(BattleCommand.None);
				}
			});
			this.CheckBattleStartState();
			this._nIndex = 0;
			this.ChangeFocus(this._nIndex);
		}

		private bool CheckBattleStartState()
		{
			bool flag = Enumerable.All<ISelectedObject<int>>(Enumerable.Where<ISelectedObject<int>>(this._listISelectSurface, (ISelectedObject<int> x) => x is UICommandLabelButton), (ISelectedObject<int> x) => ((UICommandLabelButton)x).battleCommand != BattleCommand.None);
			this._strFrame.battleStart.SetValid(flag);
			return flag;
		}

		private void OnActive(int nIndex)
		{
			if (this.index != nIndex)
			{
				this.index = nIndex;
				this.ChangeFocus(this.index);
			}
		}

		private void OnSelectSurface(UICommandLabelButton selectedSurface)
		{
			Dlg.Call<UICommandLabelButton>(ref this._actOnSelectedSurface, selectedSurface);
		}

		public void OnSelectSurface()
		{
			if (this.selectedSurface == null)
			{
				this.OnSelectBattleStart();
			}
			else
			{
				this.OnSelectSurface(this.selectedSurface);
			}
		}

		private void OnSelectBattleStart()
		{
			List<BattleCommand> list = Enumerable.ToList<BattleCommand>(Enumerable.Select<ISelectedObject<int>, BattleCommand>(Enumerable.Where<ISelectedObject<int>>(this._listISelectSurface, (ISelectedObject<int> x) => x is UICommandLabelButton), (ISelectedObject<int> x) => ((UICommandLabelButton)x).battleCommand));
			this._preOnDecideCommand.Invoke(list);
		}
	}
}
