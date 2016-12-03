using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionParticipatingFleetSelector : MonoBehaviour
	{
		[Serializable]
		private class SortieStartBtn : IRebellionOrganizeSelectObject
		{
			[SerializeField]
			private UIButton _uiButton;

			[SerializeField]
			private UIToggle _uiToggle;

			public int index
			{
				get;
				private set;
			}

			public UIButton button
			{
				get
				{
					return this._uiButton;
				}
				private set
				{
					this._uiButton = value;
				}
			}

			public UIToggle toggle
			{
				get
				{
					return this._uiToggle;
				}
				private set
				{
					this._uiToggle = value;
				}
			}

			public DelDicideRebellionOrganizeSelectBtn delDicideRebellionOrganizeSelectBtn
			{
				get;
				private set;
			}

			public SortieStartBtn(Transform transform)
			{
				this._uiButton = transform.GetComponent<UIButton>();
			}

			public bool Init(int nIndex, Tuple<MonoBehaviour, string> delegateInfos, DelDicideRebellionOrganizeSelectBtn decideDelegate)
			{
				this.index = nIndex;
				this._uiButton.onClick = Util.CreateEventDelegateList(delegateInfos.get_Item1(), delegateInfos.get_Item2(), this);
				this._uiButton.isEnabled = false;
				this._uiButton.GetComponent<BoxCollider2D>().set_enabled(false);
				return true;
			}

			public bool UnInit()
			{
				Mem.Del<UIButton>(ref this._uiButton);
				Mem.Del<UIToggle>(ref this._uiToggle);
				this.delDicideRebellionOrganizeSelectBtn = null;
				return true;
			}

			public void Decide()
			{
				this._uiButton.state = UIButtonColor.State.Normal;
				if (this.delDicideRebellionOrganizeSelectBtn != null)
				{
					this.delDicideRebellionOrganizeSelectBtn(this);
				}
			}
		}

		[SerializeField]
		private Transform _prefabParticipatingFleetInfo;

		[SerializeField]
		private UIRebellionParticipatingFleetSelector.SortieStartBtn _uiSortieStartBtn;

		[SerializeField]
		private float _fStartOffs = -400f;

		[SerializeField]
		private Vector3 _vOriginPos = new Vector3(-304f, 0f, 0f);

		private DelDicideRebellionOrganizeSelectBtn _delDicideRebellionOrganizeSelectBtn;

		private Action _actDecideSortieStart;

		private int _nSelectedIndex;

		private List<Vector3> _listInfosPos;

		private List<UIRebellionParticipatingFleetInfo> _listFleetInfos;

		private List<IRebellionOrganizeSelectObject> _listSelectorObjects;

		private int selectedObjectMax
		{
			get
			{
				return (this._uiSortieStartBtn.button.state != UIButtonColor.State.Disabled) ? (this._listSelectorObjects.get_Count() - 1) : (this._listSelectorObjects.get_Count() - 2);
			}
		}

		public int nowIndex
		{
			get
			{
				return this._nSelectedIndex;
			}
		}

		public bool isPossibleSortieStart
		{
			get
			{
				return this._listFleetInfos.get_Item(2).isFlagShipExists;
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return base.GetComponentInChildren<BoxCollider2D>().get_enabled();
			}
			set
			{
				this._listSelectorObjects.ForEach(delegate(IRebellionOrganizeSelectObject x)
				{
					x.button.isEnabled = (x.button.state != UIButtonColor.State.Disabled);
				});
			}
		}

		public bool isSortieStartFocus
		{
			get
			{
				return this._uiSortieStartBtn.toggle.value;
			}
		}

		public UIPanel panel
		{
			get
			{
				return base.GetComponent<UIPanel>();
			}
		}

		public List<UIRebellionParticipatingFleetInfo> participatingFleetInfo
		{
			get
			{
				return this._listFleetInfos;
			}
		}

		public List<UIRebellionParticipatingFleetInfo> participatingFleetList
		{
			get
			{
				return new List<UIRebellionParticipatingFleetInfo>(this._listFleetInfos.FindAll((UIRebellionParticipatingFleetInfo x) => x.isFlagShipExists));
			}
		}

		public static UIRebellionParticipatingFleetSelector Instantiate(UIRebellionParticipatingFleetSelector prefab, Transform parent)
		{
			UIRebellionParticipatingFleetSelector uIRebellionParticipatingFleetSelector = Object.Instantiate<UIRebellionParticipatingFleetSelector>(prefab);
			uIRebellionParticipatingFleetSelector.get_transform().set_parent(parent);
			uIRebellionParticipatingFleetSelector.get_transform().localScaleOne();
			uIRebellionParticipatingFleetSelector.get_transform().set_localPosition(uIRebellionParticipatingFleetSelector._vOriginPos);
			uIRebellionParticipatingFleetSelector.Setup();
			return uIRebellionParticipatingFleetSelector;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabParticipatingFleetInfo);
			this._uiSortieStartBtn.UnInit();
			Mem.Del<UIRebellionParticipatingFleetSelector.SortieStartBtn>(ref this._uiSortieStartBtn);
			Mem.Del<float>(ref this._fStartOffs);
			Mem.Del<Vector3>(ref this._vOriginPos);
			Mem.Del<DelDicideRebellionOrganizeSelectBtn>(ref this._delDicideRebellionOrganizeSelectBtn);
			Mem.Del<Action>(ref this._actDecideSortieStart);
			Mem.Del<int>(ref this._nSelectedIndex);
			Mem.DelListSafe<Vector3>(ref this._listInfosPos);
			Mem.DelListSafe<UIRebellionParticipatingFleetInfo>(ref this._listFleetInfos);
			Mem.DelListSafe<IRebellionOrganizeSelectObject>(ref this._listSelectorObjects);
		}

		private bool Setup()
		{
			this._listInfosPos = new List<Vector3>();
			this._listInfosPos.Add(new Vector3(96f, 202f, 0f));
			this._listInfosPos.Add(new Vector3(100f, 91f, 0f));
			this._listInfosPos.Add(new Vector3(96f, -20f, 0f));
			this._listInfosPos.Add(new Vector3(100f, -131f, 0f));
			this._listInfosPos.Add(new Vector3(-5f, -230f, 0f));
			this._listFleetInfos = new List<UIRebellionParticipatingFleetInfo>();
			this._listSelectorObjects = new List<IRebellionOrganizeSelectObject>();
			this._nSelectedIndex = 0;
			return true;
		}

		[DebuggerHidden]
		public IEnumerator InstantiateObjects()
		{
			UIRebellionParticipatingFleetSelector.<InstantiateObjects>c__Iterator16D <InstantiateObjects>c__Iterator16D = new UIRebellionParticipatingFleetSelector.<InstantiateObjects>c__Iterator16D();
			<InstantiateObjects>c__Iterator16D.<>f__this = this;
			return <InstantiateObjects>c__Iterator16D;
		}

		public bool Init(DelDicideRebellionOrganizeSelectBtn decideDelegate, Action callback)
		{
			this._delDicideRebellionOrganizeSelectBtn = decideDelegate;
			this._actDecideSortieStart = callback;
			int cnt = 0;
			this._listFleetInfos.ForEach(delegate(UIRebellionParticipatingFleetInfo x)
			{
				x.Init((RebellionFleetType)cnt, new DelDicideRebellionOrganizeSelectBtn(this.DecideParticipatingFleetInfo));
				cnt++;
			});
			this._uiSortieStartBtn.Init(Enum.GetValues(typeof(RebellionFleetType)).get_Length(), new Tuple<MonoBehaviour, string>(this, "DecideParticipatingFleetInfo"), new DelDicideRebellionOrganizeSelectBtn(this.DecideParticipatingFleetInfo));
			this.ChangeBtnState(0);
			return true;
		}

		public void Show(Action callback)
		{
			this.panel.widgetsAreStatic = false;
			this._listSelectorObjects.ForEach(delegate(IRebellionOrganizeSelectObject x)
			{
				Action onComplete = null;
				if (x.index == this._uiSortieStartBtn.index)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate(long _)
						{
							Dlg.Call(ref callback);
						});
					};
				}
				if (x.button.get_transform().LTIsTweening())
				{
					x.button.get_transform().LTCancel();
				}
				x.button.get_transform().LTMoveLocal(this._listInfosPos.get_Item(x.index), 0.2f).setEase(LeanTweenType.easeInSine).setDelay((float)x.index * 0.03f).setOnComplete(onComplete);
			});
		}

		public void Hide(Action callback)
		{
			this._listSelectorObjects.ForEach(delegate(IRebellionOrganizeSelectObject x)
			{
				Action onComplete = null;
				if (x.index == this._uiSortieStartBtn.index)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate(long _)
						{
							Dlg.Call(ref callback);
							this.panel.widgetsAreStatic = true;
						});
					};
				}
				if (x.button.get_transform().LTIsTweening())
				{
					x.button.get_transform().LTCancel();
				}
				Vector3 to = this._listInfosPos.get_Item(x.index);
				to.x = this._fStartOffs;
				x.button.get_transform().LTMoveLocal(to, 0.2f).setEase(LeanTweenType.easeInSine).setDelay((float)x.index * 0.03f).setOnComplete(onComplete);
			});
		}

		public void MoveNext()
		{
			this.ChangeBtnState(true);
		}

		public void MovePrev()
		{
			this.ChangeBtnState(false);
		}

		public bool IsAlreadySetFleet(DeckModel model)
		{
			return this._listFleetInfos.FindAll((UIRebellionParticipatingFleetInfo x) => x.deckModel == model).get_Count() != 0;
		}

		public void SetFleetInfo(RebellionFleetType iType, DeckModel model)
		{
			DebugUtils.Log(string.Format("{0} - {1}", iType, (model == null) ? "Null" : model.ToString()));
			this._listFleetInfos.get_Item((int)iType).SetFleetInfo(model);
		}

		public void ChkSortieStartState()
		{
			this._uiSortieStartBtn.button.isEnabled = !this.isPossibleSortieStart;
			this._uiSortieStartBtn.button.isEnabled = this.isPossibleSortieStart;
		}

		private void ChangeBtnState(int nIndex)
		{
			this._nSelectedIndex = nIndex;
		}

		private void ChangeBtnState(bool isForward)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			int a = this._nSelectedIndex + ((!isForward) ? -1 : 1);
			this._nSelectedIndex = Mathe.MinMax2Rev(a, 0, this.selectedObjectMax);
			this._listSelectorObjects.get_Item(this._nSelectedIndex).toggle.value = true;
		}

		private void DecideParticipatingFleetInfo(IRebellionOrganizeSelectObject selectObj)
		{
			DebugUtils.Log("UIRebellionParticipatingFleetSelector", selectObj.button.get_name());
			this.ChangeBtnState(selectObj.index);
			if (selectObj.index == this._uiSortieStartBtn.index)
			{
				if (this._actDecideSortieStart != null)
				{
					this._actDecideSortieStart.Invoke();
					this._uiSortieStartBtn.button.set_enabled(false);
				}
				return;
			}
			if (this._delDicideRebellionOrganizeSelectBtn != null)
			{
				this._delDicideRebellionOrganizeSelectBtn(selectObj);
			}
		}
	}
}
