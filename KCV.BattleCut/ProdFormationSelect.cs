using Common.Enum;
using local.models;
using local.utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(BtlCut_FormationAnimation))]
	public class ProdFormationSelect : MonoBehaviour
	{
		[Serializable]
		private class BrightPoints
		{
			private delegate Vector3 GetFormationPos(int posNo, int spaceX, int spaceY);

			private ProdFormationSelect.BrightPoints.GetFormationPos[] GetFormations;

			[SerializeField]
			private Transform _tra;

			private List<Transform> _listBrightPoint;

			[SerializeField]
			private ProdFormationSelect.FormationIconPos[] _clsFormationPos;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public BrightPoints(Transform obj)
			{
				this._tra = obj;
			}

			public bool Init()
			{
				this._listBrightPoint = new List<Transform>(6);
				for (int i = 1; i <= this._listBrightPoint.get_Capacity(); i++)
				{
					this._listBrightPoint.Add(this.transform.FindChild(string.Format("BrightPoint{0}", i)));
				}
				for (int j = 0; j < this._clsFormationPos[0].IconPos.Length; j++)
				{
					this._listBrightPoint.get_Item(j).set_localPosition(this._clsFormationPos[0].IconPos[j]);
				}
				return true;
			}

			public bool UnInit()
			{
				Mem.DelAry<ProdFormationSelect.BrightPoints.GetFormationPos>(ref this.GetFormations);
				Mem.Del<Transform>(ref this._tra);
				Mem.DelList<Transform>(ref this._listBrightPoint);
				Mem.DelAry<ProdFormationSelect.FormationIconPos>(ref this._clsFormationPos);
				return true;
			}

			public void ChangeFormation(BattleFormationKinds1 iKind)
			{
				int cnt = 0;
				this._clsFormationPos[iKind - BattleFormationKinds1.TanJuu].IconPos.ForEach(delegate(Vector3 x)
				{
					this._listBrightPoint.get_Item(cnt).get_transform().LTMoveLocal(x, 0.3f).setEase(LeanTweenType.easeOutExpo);
					cnt++;
				});
			}

			private void CalcFormationBrightPointPos()
			{
				this.GetFormations = new ProdFormationSelect.BrightPoints.GetFormationPos[5];
				this.GetFormations[0] = new ProdFormationSelect.BrightPoints.GetFormationPos(this.GetTanjuPos);
				this.GetFormations[1] = new ProdFormationSelect.BrightPoints.GetFormationPos(this.GetHukujuPos);
				this.GetFormations[2] = new ProdFormationSelect.BrightPoints.GetFormationPos(this.GetRinkeiPos);
				this.GetFormations[3] = new ProdFormationSelect.BrightPoints.GetFormationPos(this.GetTeikeiPos);
				this.GetFormations[4] = new ProdFormationSelect.BrightPoints.GetFormationPos(this.GetTanouPos);
				for (int i = 0; i < 5; i++)
				{
					this._clsFormationPos[i] = new ProdFormationSelect.FormationIconPos();
				}
				int spaceX = 82;
				int spaceY = 86;
				for (int j = 0; j < 5; j++)
				{
					for (int k = 0; k < 6; k++)
					{
						this._clsFormationPos[j].IconPos[k] = this.GetFormations[j](k, spaceX, spaceY);
					}
				}
			}

			private Vector3 GetTanjuPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3((float)(spaceX / 2), (float)(0 + spaceY * posNo / 2 - spaceY / 4), 0f);
			}

			private Vector3 GetHukujuPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3((float)(posNo % 2 * spaceX), (float)(spaceY * (posNo / 2)), 0f);
			}

			private Vector3 GetRinkeiPos(int posNo, int spaceX, int spaceY)
			{
				if (posNo < 2)
				{
					return new Vector3((float)(spaceX / 2), (float)(spaceY * posNo / 2), 0f);
				}
				if (posNo < 4)
				{
					return new Vector3((float)(spaceX / 2 * (posNo % 3)), (float)spaceY, 0f);
				}
				if (posNo < 6)
				{
					return new Vector3((float)(spaceX / 2), (float)(spaceY * (posNo - 1) / 2), 0f);
				}
				return Vector3.get_zero();
			}

			private Vector3 GetTeikeiPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3((float)((posNo - 2) * spaceX / 2 + spaceX / 4), (float)(posNo * spaceY / 2 - spaceY / 4), 0f);
			}

			private Vector3 GetTanouPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3((float)(spaceX * (posNo - 2) / 2 + spaceX / 4), (float)spaceY, 0f);
			}
		}

		[Serializable]
		public class FormationIconPos
		{
			public Vector3[] IconPos;

			public FormationIconPos()
			{
				this.IconPos = new Vector3[6];
			}
		}

		private const int FORMATION_NUM = 5;

		[SerializeField]
		private ProdFormationSelect.BrightPoints _clsBrightPoints;

		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private DeckModel _clsDeckModel;

		private BtlCut_FormationAnimation _prodFormationAnim;

		private BattleFormationKinds1 _iSelectFormation;

		private Action<BattleFormationKinds1> _actCallback;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public BtlCut_FormationAnimation formationAnimation
		{
			get
			{
				return this.GetComponentThis(ref this._prodFormationAnim);
			}
		}

		public BattleFormationKinds1 selectFormation
		{
			get
			{
				return this._iSelectFormation;
			}
		}

		public static ProdFormationSelect Instantiate(ProdFormationSelect prefab, Transform parent, DeckModel model)
		{
			ProdFormationSelect prodFormationSelect = Object.Instantiate<ProdFormationSelect>(prefab);
			prodFormationSelect.get_transform().set_parent(parent);
			prodFormationSelect.get_transform().localScaleOne();
			prodFormationSelect.get_transform().localPositionZero();
			prodFormationSelect.Init(model);
			return prodFormationSelect;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<UILabelButton>(ref this._listLabelButton);
			this._clsBrightPoints.UnInit();
			Mem.Del<ProdFormationSelect.BrightPoints>(ref this._clsBrightPoints);
			Mem.Del<DeckModel>(ref this._clsDeckModel);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<BtlCut_FormationAnimation>(ref this._prodFormationAnim);
			Mem.Del<BattleFormationKinds1>(ref this._iSelectFormation);
			Mem.Del<Action<BattleFormationKinds1>>(ref this._actCallback);
		}

		private void Init(DeckModel model)
		{
			this._clsDeckModel = model;
			this._isInputPossible = false;
			this._iSelectFormation = BattleFormationKinds1.TanJuu;
			this.panel.alpha = 0f;
			this._clsBrightPoints.Init();
			HashSet<BattleFormationKinds1> hash = DeckUtil.GetSelectableFormations(this._clsDeckModel);
			BattleFormationKinds1 cnt = BattleFormationKinds1.TanJuu;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.Init((int)cnt, hash.Contains(cnt));
				x.isFocus = false;
				x.toggle.group = 1;
				x.toggle.set_enabled(false);
				x.toggle.onDecide = delegate
				{
					this.DecideFormation((BattleFormationKinds1)x.index);
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", cnt);
				if (x.index == 1)
				{
					x.isFocus = (x.toggle.startsActive = true);
				}
				cnt++;
			});
		}

		public void Play(Action<BattleFormationKinds1> callback)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInFormation();
			navigation.Show(Defines.PHASE_FADE_TIME, null);
			this._actCallback = callback;
			this.Show().setOnComplete(delegate
			{
				this._isInputPossible = true;
				this._listLabelButton.ForEach(delegate(UILabelButton x)
				{
					x.toggle.set_enabled(x.isValid);
				});
				this.ChangeFocus(this._iSelectFormation);
			});
		}

		public void Run()
		{
			if (!this._isInputPossible)
			{
				return;
			}
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.DOWN))
			{
				this.PreparaNext(true);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.UP))
			{
				this.PreparaNext(false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this.DecideFormation(this._iSelectFormation);
			}
		}

		private void PreparaNext(bool isForward)
		{
			BattleFormationKinds1 iSelectFormation = this._iSelectFormation;
			this._iSelectFormation = (BattleFormationKinds1)Mathe.NextElement((int)this._iSelectFormation, 1, 5, isForward, (int x) => this._listLabelButton.Find((UILabelButton y) => y.index == x).isValid);
			if (iSelectFormation != this._iSelectFormation)
			{
				this.ChangeFocus(this._iSelectFormation);
			}
		}

		private LTDescr Show()
		{
			return this.panel.get_transform().LTValue(0f, 1f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			return this.panel.get_transform().LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private void ChangeFocus(BattleFormationKinds1 iKind)
		{
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = (x.index == (int)iKind);
			});
			this._clsBrightPoints.ChangeFormation(iKind);
		}

		private void OnActive(BattleFormationKinds1 iKind)
		{
			if (this._iSelectFormation != iKind)
			{
				this._iSelectFormation = iKind;
				this.ChangeFocus(this._iSelectFormation);
			}
		}

		private void DecideFormation(BattleFormationKinds1 iKind)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			this._isInputPossible = false;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.set_enabled(false);
			});
			Observable.FromCoroutine(() => this.formationAnimation.StartAnimation(this._iSelectFormation), false).Subscribe(delegate(Unit _)
			{
				this.Hide().setOnComplete(delegate
				{
					Dlg.Call<BattleFormationKinds1>(this._actCallback, iKind);
				});
				BattleCutManager.GetLive2D().Hide(null);
			});
		}
	}
}
