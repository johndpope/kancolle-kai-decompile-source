using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdShellingFormationJudge : BaseAnimation
	{
		[Serializable]
		private class Formation : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiOverlay;

			[SerializeField]
			private UITexture _uiLabel;

			[SerializeField]
			private UITexture _uiGrow;

			public Formation(Transform obj)
			{
				this._tra = obj;
				Util.FindParentToChild<UITexture>(ref this._uiOverlay, this._tra, "Overlay");
				Util.FindParentToChild<UITexture>(ref this._uiLabel, this._tra, "Label");
			}

			public bool Init()
			{
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UITexture>(ref this._uiOverlay);
				if (this._uiLabel.mainTexture != null)
				{
					this._uiLabel = null;
				}
				Mem.Del<UITexture>(ref this._uiLabel);
				if (this._uiGrow.mainTexture != null)
				{
					this._uiGrow = null;
				}
				Mem.Del<UITexture>(ref this._uiGrow);
			}

			public void SetFormation(BattleFormationKinds1 iKind, bool isFriend)
			{
				this._uiLabel.mainTexture = Resources.Load<Texture2D>(string.Format("Textures/Battle/Shelling/FormationJudge/txt_{0}", iKind.ToString()));
				this._uiLabel.MakePixelPerfect();
				this._uiGrow.mainTexture = Resources.Load<Texture2D>(string.Format("Textures/Battle/Shelling/FormationJudge/txt_{0}_{1}", iKind.ToString(), (!isFriend) ? "r" : "g"));
				this._uiGrow.MakePixelPerfect();
			}
		}

		[Serializable]
		private class FormationResult : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiMainResult;

			[SerializeField]
			private UISprite _uiMainResultFlash;

			[SerializeField]
			private UISprite _uiSubResult;

			public FormationResult(Transform obj)
			{
				this._tra = obj;
				Util.FindParentToChild<UISprite>(ref this._uiMainResult, this._tra, "MainLabel");
				Util.FindParentToChild<UISprite>(ref this._uiSubResult, this._tra, "SubLabel");
			}

			public bool Init()
			{
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del(ref this._uiMainResult);
				Mem.Del(ref this._uiMainResultFlash);
				Mem.Del(ref this._uiSubResult);
			}

			public void SetResult(BattleFormationKinds2 iKind)
			{
				switch (iKind)
				{
				case BattleFormationKinds2.Doukou:
				{
					UISprite arg_38_0 = this._uiMainResult;
					string spriteName = "Doukou";
					this._uiMainResultFlash.spriteName = spriteName;
					arg_38_0.spriteName = spriteName;
					Transform arg_60_0 = this._uiMainResult.get_transform();
					Vector3 vector = Vector3.get_zero();
					this._uiMainResultFlash.get_transform().set_localPosition(vector);
					arg_60_0.set_localPosition(vector);
					this._uiMainResult.get_transform().set_localScale(Vector3.get_one());
					this._uiMainResultFlash.get_transform().set_localScale(Vector3.get_one());
					this._uiSubResult.get_transform().set_localScale(Vector3.get_zero());
					break;
				}
				case BattleFormationKinds2.Hankou:
				{
					this._uiMainResult.spriteName = "Hankou";
					this._uiMainResultFlash.spriteName = "Hankou";
					Transform arg_EC_0 = this._uiMainResult.get_transform();
					Vector3 vector = Vector3.get_zero();
					this._uiMainResultFlash.get_transform().set_localPosition(vector);
					arg_EC_0.set_localPosition(vector);
					Transform arg_114_0 = this._uiMainResult.get_transform();
					vector = Vector3.get_one();
					this._uiMainResultFlash.get_transform().set_localScale(vector);
					arg_114_0.set_localScale(vector);
					this._uiSubResult.get_transform().set_localScale(Vector3.get_zero());
					break;
				}
				case BattleFormationKinds2.T_Own:
				{
					UISprite arg_14C_0 = this._uiMainResult;
					string spriteName = "T";
					this._uiMainResultFlash.spriteName = spriteName;
					arg_14C_0.spriteName = spriteName;
					this._uiMainResult.get_transform().set_localPosition(BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS.get_Item(0));
					this._uiMainResultFlash.get_transform().set_localPosition(Vector3.get_zero());
					Transform arg_1A4_0 = this._uiMainResult.get_transform();
					Vector3 vector = Vector3.get_one();
					this._uiMainResultFlash.get_transform().set_localScale(vector);
					arg_1A4_0.set_localScale(vector);
					this._uiSubResult.spriteName = "fav";
					this._uiSubResult.get_transform().set_localPosition(BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS.get_Item(1));
					this._uiSubResult.get_transform().set_localScale(Vector3.get_one());
					break;
				}
				case BattleFormationKinds2.T_Enemy:
				{
					UISprite arg_207_0 = this._uiMainResult;
					string spriteName = "T";
					this._uiMainResultFlash.spriteName = spriteName;
					arg_207_0.spriteName = spriteName;
					this._uiMainResult.get_transform().set_localPosition(BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS.get_Item(0));
					this._uiMainResultFlash.get_transform().set_localPosition(Vector3.get_zero());
					Transform arg_25F_0 = this._uiMainResult.get_transform();
					Vector3 vector = Vector3.get_one();
					this._uiMainResultFlash.get_transform().set_localScale(vector);
					arg_25F_0.set_localScale(vector);
					this._uiSubResult.spriteName = "unfav";
					this._uiSubResult.get_transform().set_localPosition(BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS.get_Item(1));
					this._uiSubResult.get_transform().set_localScale(Vector3.get_one());
					break;
				}
				}
			}
		}

		[SerializeField]
		private UIAtlas _uiAtlas;

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private ProdShellingFormationJudge.FormationResult _clsFormationResult;

		[SerializeField]
		private List<ProdShellingFormationJudge.Formation> _listFormation;

		public static ProdShellingFormationJudge Instantiate(ProdShellingFormationJudge prefab, BattleManager manager, Transform parent)
		{
			ProdShellingFormationJudge prodShellingFormationJudge = Object.Instantiate<ProdShellingFormationJudge>(prefab);
			prodShellingFormationJudge.get_transform().set_parent(parent);
			prodShellingFormationJudge.get_transform().set_localPosition(Vector3.get_zero());
			prodShellingFormationJudge.get_transform().set_localScale(Vector3.get_zero());
			prodShellingFormationJudge.SetFormationData(manager);
			return prodShellingFormationJudge;
		}

		protected override void Awake()
		{
			base.Awake();
			this.GetComponentThis(ref this._uiPanel);
			if (this._uiOverlay == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiOverlay, base.get_transform(), "Overlay");
			}
			if (this._clsFormationResult == null)
			{
				this._clsFormationResult = new ProdShellingFormationJudge.FormationResult(base.get_transform().FindChild("FormationResult"));
			}
			if (this._listFormation == null)
			{
				using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FleetType fleetType = (FleetType)((int)enumerator.get_Current());
						if (fleetType != FleetType.CombinedFleet)
						{
							this._listFormation.Add(new ProdShellingFormationJudge.Formation(base.get_transform().FindChild(string.Format("{0}Formation", fleetType.ToString()))));
						}
					}
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UIAtlas>(ref this._uiAtlas);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.DelIDisposableSafe<ProdShellingFormationJudge.FormationResult>(ref this._clsFormationResult);
			if (this._listFormation != null)
			{
				this._listFormation.ForEach(delegate(ProdShellingFormationJudge.Formation x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe<ProdShellingFormationJudge.Formation>(ref this._listFormation);
		}

		private void SetFormationData(BattleManager manager)
		{
			this._clsFormationResult.SetResult(manager.CrossFormationId);
			this._listFormation.get_Item(0).SetFormation(manager.FormationId_f, true);
			this._listFormation.get_Item(1).SetFormation(manager.FormationId_e, false);
		}

		public override void Play(Action forceCallback, Action callback)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			base.Play(forceCallback, callback);
		}

		private void PlaySlideSE()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_940);
		}

		private void PlayMessageSE()
		{
		}

		protected override void OnForceAnimationFinished()
		{
			base.OnForceAnimationFinished();
		}
	}
}
