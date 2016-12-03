using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using local.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class ProdBCResult : BtlCut_Base
	{
		[Serializable]
		private class MVPShip
		{
			[SerializeField]
			private UIPanel _uiPanel;

			[SerializeField]
			private UITexture _uiShipTexture;

			[SerializeField]
			private UITexture _uiShipShadow;

			private ShipModel_BattleResult _clsShipModel;

			private UIPlayTween _uiPlayTween;

			[Header("[Animation Properties]"), SerializeField]
			private float _fDuration = 0.2f;

			[SerializeField]
			private iTween.EaseType _iEaseType = iTween.EaseType.easeOutSine;

			public int index
			{
				get;
				private set;
			}

			public float duration
			{
				get
				{
					return this._fDuration;
				}
				set
				{
					this._fDuration = value;
				}
			}

			public iTween.EaseType easeType
			{
				get
				{
					return this._iEaseType;
				}
				set
				{
					this._iEaseType = value;
				}
			}

			public UIPanel panel
			{
				get
				{
					return this._uiPanel;
				}
			}

			public Transform transform
			{
				get
				{
					return this._uiPanel.get_transform();
				}
			}

			public ShipModel_BattleResult shipModel
			{
				get
				{
					return this._clsShipModel;
				}
			}

			private UIPlayTween playTween
			{
				get
				{
					if (this._uiPlayTween == null)
					{
						this._uiPlayTween = this.transform.GetComponent<UIPlayTween>();
					}
					return this._uiPlayTween;
				}
			}

			public MVPShip(Transform transform)
			{
			}

			public bool Init(ShipModel_BattleResult model)
			{
				this._clsShipModel = model;
				this.panel.alpha = 0f;
				if (model == null)
				{
					return false;
				}
				this.index = model.Index;
				Texture2D texture2D = ShipUtils.LoadTexture(model);
				UITexture arg_47_0 = this._uiShipTexture;
				Texture mainTexture = texture2D;
				this._uiShipShadow.mainTexture = mainTexture;
				arg_47_0.mainTexture = mainTexture;
				this._uiShipTexture.MakePixelPerfect();
				this._uiShipShadow.MakePixelPerfect();
				Vector3 vpos = Util.Poi2Vec(model.Offsets.GetShipDisplayCenter(model.IsDamaged())) + Vector3.get_down() * 10f;
				this._uiShipTexture.get_transform().set_localScale(Vector3.get_one() * 1.1f);
				this._uiShipTexture.get_transform().AddLocalPosition(vpos);
				this._uiShipShadow.get_transform().AddLocalPosition(vpos);
				return true;
			}

			public bool UnInit()
			{
				this._uiPanel = null;
				this._uiShipTexture = null;
				this._uiShipShadow = null;
				return true;
			}

			public void PlayMVPVoice(bool isPlayVoice)
			{
				if (this._clsShipModel == null)
				{
					return;
				}
				if (isPlayVoice)
				{
					ShipUtils.PlayMVPVoice(this.shipModel);
				}
				this.playTween.Play(true);
				this.transform.ValueTo(0f, 1f, this.duration, this._iEaseType, delegate(object x)
				{
					this.panel.alpha = Convert.ToSingle(x);
				}, null);
			}
		}

		[Serializable]
		private class ResultFrame
		{
			[SerializeField]
			private UIPanel _uiPanel;

			[Header("[Animation Properties]"), SerializeField]
			private float _fDuration = 0.7f;

			[SerializeField]
			private Vector3 _vFromPos = Vector3.get_left() * 36.1f;

			[SerializeField]
			private Vector3 _vToPos = Vector3.get_zero();

			[SerializeField]
			private iTween.EaseType _iEaseType = iTween.EaseType.easeOutSine;

			public UIPanel panel
			{
				get
				{
					return this._uiPanel;
				}
			}

			public Transform transform
			{
				get
				{
					return this._uiPanel.get_transform();
				}
			}

			public float duration
			{
				get
				{
					return this._fDuration;
				}
				set
				{
					this._fDuration = value;
				}
			}

			public ResultFrame(Transform transform)
			{
			}

			public bool Init()
			{
				return true;
			}

			public bool UnInit()
			{
				return true;
			}

			public void Show()
			{
				this.panel.widgetsAreStatic = false;
				this.transform.set_localPosition(this._vFromPos);
				this.transform.LocalMoveTo(this._vToPos, this.duration, this._iEaseType, null);
				this.transform.ValueTo(0f, 1f, this.duration, this._iEaseType, delegate(object x)
				{
					this.panel.alpha = Convert.ToSingle(x);
				}, null);
			}

			public void Hide()
			{
				this.transform.set_localPosition(this._vToPos);
				this.transform.LocalMoveTo(this._vFromPos, this.duration, this._iEaseType, null);
				this.transform.ValueTo(1f, 0f, this.duration, this._iEaseType, delegate(object x)
				{
					this.panel.alpha = Convert.ToSingle(x);
				}, delegate
				{
					this.panel.widgetsAreStatic = true;
				});
			}
		}

		[Serializable]
		private class Ships
		{
			[SerializeField]
			private UIPanel _uiPanel;

			[Header("[Animation Properties]"), SerializeField]
			private float _fDuration = 0.7f;

			[SerializeField]
			private Vector3 _vFromPos = new Vector3(-385.72f, 12.06f, 0f);

			[SerializeField]
			private Vector3 _vToPos = new Vector3(-315f, -12.06f, 0f);

			[SerializeField]
			private iTween.EaseType _iEaseType = iTween.EaseType.easeOutSine;

			private List<BtlCut_ResultShip> _listResultShips;

			public Transform transform
			{
				get
				{
					return this._uiPanel.get_transform();
				}
			}

			public UIPanel panel
			{
				get
				{
					return this._uiPanel;
				}
			}

			public float duration
			{
				get
				{
					return this._fDuration;
				}
				set
				{
					this._fDuration = value;
				}
			}

			public List<BtlCut_ResultShip> resultShips
			{
				get
				{
					return this._listResultShips;
				}
			}

			public Ships(Transform transform)
			{
			}

			public bool Init(Tuple<BattleResultModel, Transform> infos)
			{
				this._listResultShips = new List<BtlCut_ResultShip>(infos.get_Item1().Ships_f.Length);
				for (int i = 0; i < this._listResultShips.get_Capacity(); i++)
				{
					if (infos.get_Item1().Ships_f[i] != null)
					{
						this._listResultShips.Add(BtlCut_ResultShip.Instantiate(infos.get_Item2().GetComponent<BtlCut_ResultShip>(), this.transform, new Vector3(0f, 150f - (float)(i * 60), 0f), infos.get_Item1().Ships_f[i]));
					}
				}
				return true;
			}

			public bool UnInit()
			{
				return true;
			}

			public void Show()
			{
				this._uiPanel.widgetsAreStatic = false;
				this.transform.set_localPosition(this._vFromPos);
				this.transform.LocalMoveTo(this._vToPos, this.duration, this._iEaseType, null);
				this.transform.ValueTo(0f, 1f, this.duration, this._iEaseType, delegate(object x)
				{
					this.panel.alpha = Convert.ToSingle(x);
				}, null);
			}

			public void Hide()
			{
				this.transform.set_localPosition(this._vToPos);
				this.transform.LocalMoveTo(this._vFromPos, this.duration, this._iEaseType, null);
				this.transform.ValueTo(1f, 0f, this.duration, this._iEaseType, delegate(object x)
				{
					this.panel.alpha = Convert.ToSingle(x);
				}, delegate
				{
					this.panel.widgetsAreStatic = true;
				});
			}
		}

		[SerializeField]
		private Transform _prefabResultShipOrigin;

		[SerializeField]
		private ProdBCResult.MVPShip _clsMVPShip;

		[SerializeField]
		private ProdBCResult.ResultFrame _clsResultFrame;

		[SerializeField]
		private ProdBCResult.Ships _clsShips;

		private bool isAnimEnd;

		private int[] startHP;

		private BattleResultModel _clsBattleResult;

		private KeyControl key;

		private Action _actCallback;

		public static ProdBCResult Instantiate(ProdBCResult prefab, Transform parent)
		{
			ProdBCResult prodBCResult = Object.Instantiate<ProdBCResult>(prefab);
			prodBCResult.get_transform().set_parent(parent);
			prodBCResult.get_transform().localScaleOne();
			prodBCResult.get_transform().localPositionZero();
			prodBCResult.Init();
			return prodBCResult;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabResultShipOrigin);
			if (this._clsMVPShip != null)
			{
				this._clsMVPShip.UnInit();
			}
			Mem.Del<ProdBCResult.MVPShip>(ref this._clsMVPShip);
			if (this._clsResultFrame != null)
			{
				this._clsResultFrame.UnInit();
			}
			Mem.Del<ProdBCResult.ResultFrame>(ref this._clsResultFrame);
			if (this._clsShips != null)
			{
				this._clsShips.UnInit();
			}
			Mem.Del<ProdBCResult.Ships>(ref this._clsShips);
			Mem.Del<bool>(ref this.isAnimEnd);
			Mem.DelAry<int>(ref this.startHP);
			Mem.Del<BattleResultModel>(ref this._clsBattleResult);
			Mem.Del<KeyControl>(ref this.key);
			Mem.Del<Action>(ref this._actCallback);
		}

		public override void Init()
		{
			this._clsBattleResult = BattleCutManager.GetBattleManager().GetBattleResult();
			this.startHP = this.GetStartHP(this._clsBattleResult);
			this.isAnimEnd = false;
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
			this.SetResultShips();
			this._clsMVPShip.Init(this._clsBattleResult.MvpShip);
			this._clsMVPShip.PlayMVPVoice(BattleUtils.IsPlayMVPVoice(this._clsBattleResult.WinRank));
		}

		private int[] GetStartHP(BattleResultModel model)
		{
			int[] array = new int[model.Ships_f.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (model.Ships_f[i] == null)
				{
					array[i] = 0;
				}
				else
				{
					array[i] = model.Ships_f[i].HpStart;
				}
			}
			return array;
		}

		public override void Run()
		{
			if (this.isAnimEnd)
			{
				this.key.Update();
				if (this.key.keyState.get_Item(1).down || Input.GetMouseButton(0) || Input.get_touchCount() > 0)
				{
					this.SceneFinish();
				}
			}
		}

		private void SetResultShips()
		{
			this._clsShips.Init(new Tuple<BattleResultModel, Transform>(this._clsBattleResult, this._prefabResultShipOrigin));
		}

		public void StartAnimation(Action callback)
		{
			TrophyUtil.Unlock_UserLevel();
			this._actCallback = callback;
			Observable.Timer(TimeSpan.FromSeconds(0.800000011920929)).Subscribe(delegate(long _)
			{
				this._clsResultFrame.Show();
				this._clsShips.Show();
			}).AddTo(base.get_gameObject());
			Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(delegate(long _)
			{
				this.StartHPBarAnim();
				this._clsShips.resultShips.get_Item(0).act = delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long __)
					{
						this._clsShips.resultShips.get_Item(this._clsMVPShip.index).ShowMVPIcon();
						this.StartEXPBarAnim();
						this.isAnimEnd = true;
						UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
						navigation.SetNavigationInResult();
						navigation.Show(0.2f, null);
					});
				};
			}).AddTo(base.get_gameObject());
		}

		public void StartHPBarAnim()
		{
			Enumerable.Where<BtlCut_ResultShip>(this._clsShips.resultShips, (BtlCut_ResultShip order) => order.get_isActiveAndEnabled()).ForEach(delegate(BtlCut_ResultShip x)
			{
				x.UpdateHP();
			});
		}

		public void StartEXPBarAnim()
		{
			Enumerable.Where<BtlCut_ResultShip>(this._clsShips.resultShips, (BtlCut_ResultShip order) => order.get_isActiveAndEnabled()).ForEach(delegate(BtlCut_ResultShip x)
			{
				x.UpdateEXPGauge();
			});
		}

		private void SceneFinish()
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(0.2f, null);
			this.isAnimEnd = false;
			this.hideAll(delegate
			{
				Dlg.Call(ref this._actCallback);
			});
		}

		private void hideAll(Action callback)
		{
			base.get_transform().ValueTo(1f, 0f, 0.2f, iTween.EaseType.easeOutSine, delegate(object x)
			{
				float num = Convert.ToSingle(x);
				UIPanel arg_39_0 = this._clsMVPShip.panel;
				float num2 = num;
				this._clsShips.panel.alpha = num2;
				num2 = num2;
				this._clsResultFrame.panel.alpha = num2;
				arg_39_0.alpha = num2;
			}, callback);
		}
	}
}
