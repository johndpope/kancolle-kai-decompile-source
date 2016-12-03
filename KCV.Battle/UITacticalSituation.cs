using KCV.Battle.Utils;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UITacticalSituation : MonoBehaviour
	{
		[Serializable]
		private struct AnimParams
		{
			public float showhideTime;

			public LeanTweenType showhideEase;
		}

		[Serializable]
		private class UIFrame : IDisposable
		{
			[SerializeField]
			private UIWidget _uiWidget;

			[SerializeField]
			private UILabel _uiLabel;

			[SerializeField]
			private UITexture _uiSeparator;

			public void Dispose()
			{
				Mem.Del<UIWidget>(ref this._uiWidget);
				Mem.Del<UILabel>(ref this._uiLabel);
				Mem.Del<UITexture>(ref this._uiSeparator);
			}
		}

		[SerializeField]
		private Transform _prefabUITacticalSituationShipBanner;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiBlur;

		[SerializeField]
		private UITacticalSituation.UIFrame _uiFrame;

		[SerializeField]
		private List<UITacticalSituationFleetInfos> _listFleetInfos;

		[Header("[Animation Parameter]"), SerializeField]
		private UITacticalSituation.AnimParams _strAnimParams;

		private UIPanel _uiPanel;

		private Action _actOnBack;

		private bool _isInputPossible;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static UITacticalSituation Instantiate(UITacticalSituation prefab, Transform parent, BattleManager manager)
		{
			UITacticalSituation uITacticalSituation = Object.Instantiate<UITacticalSituation>(prefab);
			uITacticalSituation.get_transform().set_parent(parent);
			uITacticalSituation.get_transform().localScaleOne();
			uITacticalSituation.get_transform().localPositionZero();
			uITacticalSituation.VirtualCtor(manager);
			return uITacticalSituation;
		}

		private void VirtualCtor(BattleManager manager)
		{
			this._isInputPossible = false;
			this.panel.alpha = 0f;
			this.InitFleetsInfos(manager);
			this.panel.widgetsAreStatic = true;
		}

		private void InitFleetsInfos(BattleManager manager)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						this._listFleetInfos.get_Item((int)fleetType).Init(fleetType, (fleetType != FleetType.Friend) ? "敵艦隊" : "味方艦隊", (fleetType != FleetType.Friend) ? Enumerable.ToList<ShipModel_BattleAll>(manager.Ships_e) : Enumerable.ToList<ShipModel_BattleAll>(manager.Ships_f), this._prefabUITacticalSituationShipBanner.GetComponent<UITacticalSituationShipBanner>());
					}
				}
			}
			Mem.Del<Transform>(ref this._prefabUITacticalSituationShipBanner);
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabUITacticalSituationShipBanner);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiBlur);
			Mem.DelIDisposableSafe<UITacticalSituation.UIFrame>(ref this._uiFrame);
			if (this._listFleetInfos != null)
			{
				this._listFleetInfos.ForEach(delegate(UITacticalSituationFleetInfos x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe<UITacticalSituationFleetInfos>(ref this._listFleetInfos);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Action>(ref this._actOnBack);
			Mem.Del<bool>(ref this._isInputPossible);
		}

		public bool Init(Action onBack)
		{
			this._actOnBack = onBack;
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (this._isInputPossible && keyControl.GetDown(KeyControl.KeyName.SANKAKU))
			{
				this.Hide();
				return true;
			}
			return false;
		}

		public void Show(Action onFinished)
		{
			this.panel.widgetsAreStatic = false;
			this.panel.get_transform().LTCancel();
			this.panel.get_transform().LTValue(this.panel.alpha, 1f, this._strAnimParams.showhideTime).setEase(this._strAnimParams.showhideEase).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				this._isInputPossible = true;
				Dlg.Call(ref onFinished);
			});
		}

		private void Hide()
		{
			this._isInputPossible = false;
			this.panel.get_transform().LTCancel();
			this.panel.get_transform().LTValue(this.panel.alpha, 0f, this._strAnimParams.showhideTime).setEase(this._strAnimParams.showhideEase).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				this.panel.widgetsAreStatic = true;
				Dlg.Call(ref this._actOnBack);
			});
		}
	}
}
