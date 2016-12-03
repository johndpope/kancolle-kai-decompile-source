using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UICommandBox : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			[Header("[Backgrounds Properties]")]
			public float backgroundsShowTime;

			public LeanTweenType backgroundsShowEase;

			[Header("[Surface Properties]")]
			public Vector3 surfaceInitPosOffs;

			public float surfaceShowMoveTime;

			public LeanTweenType surfaceShowEase;

			public float surfaceShowInterval;

			public void Dispose()
			{
				Mem.Del<float>(ref this.backgroundsShowTime);
				Mem.Del<LeanTweenType>(ref this.backgroundsShowEase);
				Mem.Del<Vector3>(ref this.surfaceInitPosOffs);
				Mem.Del<float>(ref this.surfaceShowMoveTime);
				Mem.Del<LeanTweenType>(ref this.surfaceShowEase);
				Mem.Del<float>(ref this.surfaceShowInterval);
			}
		}

		[Serializable]
		private class UIBattleStartBtn : IDisposable, IUICommandSurface
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiGrow;

			[SerializeField]
			private UISprite _uiSprite;

			private UIWidget _uiWidget;

			private int _nIndex;

			private Animation _anim;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public UIButton button
			{
				get
				{
					return this.transform.GetComponent<UIButton>();
				}
			}

			public BoxCollider2D colliderBox2D
			{
				get
				{
					return this.transform.GetComponent<BoxCollider2D>();
				}
			}

			public bool isEnabled
			{
				get
				{
					return this.button.isEnabled;
				}
				set
				{
					if (value)
					{
						this.button.normalSprite = "select_btn_on";
						this._uiGrow.get_transform().LTValue(0f, 1f, 1f).setEase(LeanTweenType.linear).setLoopPingPong().setOnUpdate(delegate(float x)
						{
							this._uiGrow.alpha = x;
						});
						this.button.isEnabled = true;
					}
					else
					{
						this._uiGrow.get_transform().LTCancel();
						this._uiGrow.get_transform().LTValue(this._uiGrow.alpha, 0f, 1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
						{
							this._uiGrow.alpha = x;
						});
						this.button.normalSprite = "select_btn";
						this.button.isEnabled = false;
					}
				}
			}

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

			public UIWidget widget
			{
				get
				{
					return this.transform.GetComponent<UIWidget>();
				}
			}

			public UIBattleStartBtn(Transform obj)
			{
			}

			public bool Init(int nIndex, Func<bool> onDecideBattleStart)
			{
				this._nIndex = nIndex;
				this.button.onClick.Add(new EventDelegate(delegate
				{
					onDecideBattleStart.Invoke();
				}));
				this.button.normalSprite = "select_btn";
				this.button.isEnabled = false;
				this._uiGrow.alpha = 0f;
				this._anim = this.transform.GetComponent<Animation>();
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del(ref this._uiGrow);
				Mem.Del(ref this._uiSprite);
				Mem.Del<UIWidget>(ref this._uiWidget);
				Mem.Del<int>(ref this._nIndex);
				Mem.Del<Animation>(ref this._anim);
			}

			public LTDescr Magnify()
			{
				return this.transform.LTScale(Vector3.get_one() * 1.2f, 0.2f).setEase(LeanTweenType.easeOutSine);
			}

			public LTDescr Reduction()
			{
				return this.transform.LTScale(Vector3.get_one(), 0.2f).setEase(LeanTweenType.easeOutSine);
			}

			public void Show()
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_943);
				this._anim.Play();
			}

			public void PlayDecide()
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_944);
				this._uiGrow.get_transform().LTCancel();
				this._uiGrow.get_transform().LTScale(Vector3.get_one() * 1.5f, 0.5f);
				this._uiGrow.get_transform().LTValue(this._uiGrow.alpha, 0f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
				{
					this._uiGrow.alpha = x;
				});
			}
		}

		[Serializable]
		private class Backgrounds : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiBackground;

			[SerializeField]
			private UISprite _uiBGLight;

			[SerializeField]
			private UITexture _uiGlowLight;

			[SerializeField]
			private UISprite _uiCommandLabel;

			public bool Init()
			{
				this._uiBackground.height = 0;
				this._uiBGLight.height = 0;
				this._uiGlowLight.width = 0;
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				if (this._uiBackground != null)
				{
					this._uiBackground.Clear();
				}
				Mem.Del(ref this._uiBackground);
				if (this._uiBGLight != null)
				{
					this._uiBGLight.Clear();
				}
				Mem.Del(ref this._uiBGLight);
				if (this._uiCommandLabel != null)
				{
					this._uiCommandLabel.Clear();
				}
				Mem.Del(ref this._uiCommandLabel);
				Mem.Del<UITexture>(ref this._uiGlowLight);
			}

			public LTDescr Show(float time, LeanTweenType iType)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_941);
				this._uiCommandLabel.get_transform().LTMoveLocalY(-125f, time).setEase(iType);
				this._uiGlowLight.get_transform().LTValue(0f, 1f, 5f).setDelay(time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					Rect uvRect = this._uiGlowLight.uvRect;
					uvRect.set_x(x);
					this._uiGlowLight.uvRect = uvRect;
				}).setLoopClamp();
				this._uiGlowLight.get_transform().LTValue(1f, 0.3f, 1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiGlowLight.alpha = x;
				}).setLoopPingPong();
				return this._uiBackground.get_transform().LTValue((float)this._uiBackground.width, 1150f, time).setEase(iType).setOnUpdate(delegate(float x)
				{
					int num = Convert.ToInt32(x);
					this._uiBackground.height = num;
					this._uiBGLight.height = num;
					this._uiGlowLight.width = num;
				});
			}

			public LTDescr Hide()
			{
				return null;
			}
		}

		[SerializeField]
		private Transform _prefabCommandSurface;

		[SerializeField]
		private UICommandBox.Backgrounds _clsBackgrounds;

		[SerializeField]
		private UICommandBox.UIBattleStartBtn _uiBattleStartBtn;

		[SerializeField]
		private List<Vector3> _listCommandSurfacePos = new List<Vector3>();

		[Header("[Animation Properties]"), SerializeField]
		private UICommandBox.Params _strParams;

		[SerializeField]
		private Vector3 _vCommandBoxPos = Vector3.get_zero();

		[SerializeField]
		private Vector3 _vBattleStartBtnPos = Vector3.get_zero();

		private List<UICommandSurface> _listCommandSurface;

		private List<IUICommandSurface> _listICommandSurface;

		private Predicate<List<BattleCommand>> _actDecideBattleStart;

		private UIPanel _uiPanel;

		private int _nSelectIndex;

		public List<UICommandSurface> listCommandSurfaces
		{
			get
			{
				return this._listCommandSurface;
			}
			private set
			{
				this._listCommandSurface = value;
			}
		}

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public int selectIndex
		{
			get
			{
				return this._nSelectIndex;
			}
			private set
			{
				int mx = (!this._uiBattleStartBtn.isEnabled) ? (this._listCommandSurface.get_Count() - 1) : this._listCommandSurface.get_Count();
				this._nSelectIndex = Mathe.MinMax2Rev(value, 0, mx);
			}
		}

		public bool isSelectBattleStart
		{
			get
			{
				return this.selectIndex == this._listCommandSurface.get_Count();
			}
		}

		public UICommandSurface focusSurface
		{
			get
			{
				return this._listCommandSurface.get_Item(this.selectIndex);
			}
		}

		public bool isColliderEnabled
		{
			set
			{
				this._listCommandSurface.ForEach(delegate(UICommandSurface x)
				{
					x.boxCollider2D.set_enabled(value);
				});
				this._uiBattleStartBtn.colliderBox2D.set_enabled(value);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabCommandSurface);
			Mem.DelIDisposableSafe<UICommandBox.Backgrounds>(ref this._clsBackgrounds);
			Mem.DelIDisposableSafe<UICommandBox.UIBattleStartBtn>(ref this._uiBattleStartBtn);
			Mem.DelListSafe<Vector3>(ref this._listCommandSurfacePos);
			Mem.DelIDisposableSafe<UICommandBox.Params>(ref this._strParams);
			Mem.Del<Vector3>(ref this._vCommandBoxPos);
			Mem.Del<Vector3>(ref this._vBattleStartBtnPos);
			Mem.DelListSafe<UICommandSurface>(ref this._listCommandSurface);
			Mem.DelListSafe<IUICommandSurface>(ref this._listICommandSurface);
			Mem.Del<Predicate<List<BattleCommand>>>(ref this._actDecideBattleStart);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<int>(ref this._nSelectIndex);
		}

		public bool Init(CommandPhaseModel model, Predicate<List<BattleCommand>> onDeideBattleStart)
		{
			this._nSelectIndex = 0;
			base.get_transform().set_localPosition(this._vCommandBoxPos);
			this._actDecideBattleStart = onDeideBattleStart;
			this.CreateSurface(model.GetPresetCommand());
			this._uiBattleStartBtn.Init(this._listCommandSurface.get_Count(), new Func<bool>(this.DecideStartBattle));
			this._uiBattleStartBtn.transform.set_localPosition(this._vBattleStartBtnPos);
			this._uiBattleStartBtn.transform.localScaleZero();
			this._listICommandSurface.Add(this._uiBattleStartBtn);
			this.panel.alpha = 1f;
			this.ChkAllSurfaceSet();
			this._clsBackgrounds.Init();
			return true;
		}

		private void CreateSurface(List<BattleCommand> presetList)
		{
			this._listCommandSurface = new List<UICommandSurface>();
			this._listICommandSurface = new List<IUICommandSurface>();
			int num = 0;
			using (List<BattleCommand>.Enumerator enumerator = presetList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleCommand current = enumerator.get_Current();
					this._listCommandSurface.Add(UICommandSurface.Instantiate(this._prefabCommandSurface.GetComponent<UICommandSurface>(), base.get_transform(), this._listCommandSurfacePos.get_Item(num) + this._strParams.surfaceInitPosOffs, num, current, new Action(this.ChkAllSurfaceSet)));
					this._listICommandSurface.Add(this._listCommandSurface.get_Item(num));
					num++;
				}
			}
		}

		public void SetBattleStartButtonLayer()
		{
			this._uiBattleStartBtn.transform.SetLayer(Generics.Layers.CutIn.IntLayer(), true);
		}

		public void FocusSurfaceMagnify()
		{
			UICommandSurface uICommandSurface = Enumerable.FirstOrDefault<UICommandSurface>(this._listCommandSurface, (UICommandSurface x) => !x.isSetUnit);
			if (this._uiBattleStartBtn.isEnabled)
			{
				this.selectIndex = this._uiBattleStartBtn.index;
			}
			else if (uICommandSurface != null)
			{
				this.selectIndex = uICommandSurface.index;
			}
			else
			{
				this.selectIndex = 0;
			}
			this._listICommandSurface.get_Item(this.selectIndex).Magnify();
		}

		public void Prev()
		{
			this.selectIndex--;
			this._listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				if (x.index == this.selectIndex)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
					x.Magnify();
				}
				else
				{
					x.Reduction();
				}
			});
		}

		public void Next()
		{
			this.selectIndex++;
			this._listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				if (x.index == this.selectIndex)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
					x.Magnify();
				}
				else
				{
					x.Reduction();
				}
			});
		}

		public void RemoveCommandUnit2FocusSurface()
		{
			if (this.selectIndex == this._uiBattleStartBtn.index)
			{
				return;
			}
			this._listCommandSurface.get_Item(this.selectIndex).RemoveCommandUnit();
		}

		public void RemoveCommandUnitAll()
		{
			this._listCommandSurface.ForEach(delegate(UICommandSurface x)
			{
				x.RemoveCommandUnit();
			});
			this.selectIndex = 0;
			this._listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				if (x.index == this.selectIndex)
				{
					x.Magnify();
				}
				else
				{
					x.Reduction();
				}
			});
		}

		public void AbsodedUnitIcon2FocusSurface()
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			UICommandUnitIcon focusUnitIcon = prodBattleCommandSelect.commandUnitList.focusUnitIcon;
			this._listCommandSurface.get_Item(this.selectIndex).Absorded(focusUnitIcon);
		}

		public void SetFocusUnitIcon2FocusSurface()
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			UICommandUnitIcon focusUnitIcon = prodBattleCommandSelect.commandUnitList.focusUnitIcon;
			this._listCommandSurface.get_Item(this.selectIndex).SetCommandUnit(focusUnitIcon);
		}

		public void ReductionAll()
		{
			this._listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				x.Reduction();
			});
		}

		public void ChkAllSurfaceSet()
		{
			if (Enumerable.All<UICommandSurface>(this.listCommandSurfaces, (UICommandSurface x) => x.isSetUnit))
			{
				this._uiBattleStartBtn.isEnabled = true;
			}
			else
			{
				this._uiBattleStartBtn.isEnabled = false;
			}
		}

		[DebuggerHidden]
		public IEnumerator PlayShowAnimation()
		{
			UICommandBox.<PlayShowAnimation>c__Iterator10A <PlayShowAnimation>c__Iterator10A = new UICommandBox.<PlayShowAnimation>c__Iterator10A();
			<PlayShowAnimation>c__Iterator10A.<>f__this = this;
			return <PlayShowAnimation>c__Iterator10A;
		}

		public bool DecideStartBattle()
		{
			List<BattleCommand> val = Enumerable.ToList<BattleCommand>(Enumerable.Select<UICommandSurface, BattleCommand>(this._listCommandSurface, (UICommandSurface x) => x.commandType));
			bool flag = Dlg.Call<List<BattleCommand>>(ref this._actDecideBattleStart, val);
			if (flag)
			{
				this._uiBattleStartBtn.PlayDecide();
				this._uiBattleStartBtn.button.isEnabled = false;
				this._listCommandSurface.ForEach(delegate(UICommandSurface x)
				{
					x.boxCollider2D.set_enabled(false);
				});
			}
			return flag;
		}
	}
}
