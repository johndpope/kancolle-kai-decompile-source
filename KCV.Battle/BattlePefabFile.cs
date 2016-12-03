using KCV.Battle.Production;
using KCV.SortieBattle;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public class BattlePefabFile : BasePrefabFile
	{
		[Header("[Common]"), SerializeField]
		private Transform _prefabBattleField;

		[SerializeField]
		private Transform _prefabBattleShip;

		[SerializeField]
		private Transform _prefabFieldDimCamera;

		[SerializeField]
		private Transform _prefabProdCloud;

		[SerializeField]
		private Transform _prefabUICircleHPGauge;

		[SerializeField]
		private Transform _prefabUIBattleNavigation;

		[Header("[BossInsert]"), SerializeField]
		private Transform _prefabProdBossInsert;

		[Header("[Detection]"), SerializeField]
		private Transform _prefabProdDetectionStartCutIn;

		[SerializeField]
		private Transform _prefabProdDetectionCutIn;

		[SerializeField]
		private Transform _prefabProdDetectionResultCutIn;

		[Header("[Command]"), SerializeField]
		private Transform _prefabProdBattleCommandSelect;

		[SerializeField]
		private Transform _prefabProdBattleCommandBuffer;

		[SerializeField]
		private Transform _prefabProdBufferEffect;

		[SerializeField]
		private Transform _prefabUIBufferFleetCircle;

		[SerializeField]
		private Transform _prefabUIBufferShipCircle;

		[Header("[AerialCombat]"), SerializeField]
		private Transform _prefabProdAerialCombatCutinP;

		[SerializeField]
		private Transform _prefabProdAerialCombatP1;

		[Header("[SupportingFire]"), SerializeField]
		private Transform _prefabProdSupportCutIn;

		[Header("[OpeningTorpedoSalvo]"), SerializeField]
		private Transform _prefabProdOpeningTorpedoCutIn;

		[Header("[Shelling]"), SerializeField]
		private Transform _prefabProdShellingFormationJudge;

		[SerializeField]
		private Transform _prefabProdShellingSlotLine;

		[Header("[TorpedoSalvo]"), SerializeField]
		private Transform _prefabProdTorpedoCutIn;

		[SerializeField]
		private Transform _prefabTorpedoStraightController;

		[SerializeField]
		private Transform _prefabProdTorpedoResucueCutIn;

		[Header("[WithdrawalDecision]"), SerializeField]
		private Transform _prefabProdWithdrawalDecisionSelection;

		[Header("[NightCombat]"), SerializeField]
		private Transform _prefabProdNightRadarDeployment;

		[SerializeField]
		private Transform _prefabSearchLightSceneController;

		[SerializeField]
		private Transform _prefabFlareBulletSceneController;

		[SerializeField]
		private Transform _prefabProdDeathCry;

		[Header("[Result]"), SerializeField]
		private Transform _prefabProdVeteransReport;

		[Header("[AdvancingWithDrawal]"), SerializeField]
		private Transform _prefabProdAdvancingWithDrawalSelect;

		[Header("[AdvancingWithDrawalDC]"), SerializeField]
		private Transform _prefabProdAdvancingWithDrawalDC;

		[Header("[FlagshipWreck]"), SerializeField]
		private Transform _prefabProdFlagshipWreck;

		[Header("Others"), SerializeField]
		private Transform _prefabProdCombatRation;

		[Header("[Damage]"), SerializeField]
		private Transform _prefabDamageCutIn;

		[SerializeField]
		private Transform _prefabProdSinking;

		private UIBattleNavigation _uiBattleNavigation;

		private ProdCloud _prodCloud;

		private UICircleHPGauge _uiCircleHPGauge;

		private ProdShellingSlotLine _prodShellingSlotLine;

		private ProdBattleCommandBuffer _prodBattleCommandBuffer;

		private ProdDamageCutIn _prodDamageCutIn;

		private ProdSinking _prodSinking;

		private BattleShutter _clsBattleShutter;

		private ProdBufferEffect _prodBufferEffect;

		public Transform prefabBattleField
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabBattleField);
			}
		}

		public Transform prefabBattleShip
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabBattleShip);
			}
		}

		public Transform prefabFieldDimCamera
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabFieldDimCamera);
			}
		}

		public Transform prefabProdCloud
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdCloud);
			}
		}

		public Transform prefabProdBossInsert
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdBossInsert);
			}
		}

		public Transform prefabProdDetectionStartCutIn
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdDetectionStartCutIn);
			}
		}

		public Transform prefabProdDetectionCutIn
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdDetectionCutIn);
			}
		}

		public Transform prefabProdDetectionResultCutIn
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdDetectionResultCutIn);
			}
		}

		public Transform prefabProdBattleCommandSelect
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdBattleCommandSelect);
			}
		}

		public Transform prefabProdBattleCommandBuffer
		{
			get
			{
				return this._prefabProdBattleCommandBuffer;
			}
		}

		public Transform prefabUIBufferFleetCircle
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabUIBufferFleetCircle);
			}
		}

		public Transform prefabUIBufferShipCircle
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabUIBufferShipCircle);
			}
		}

		public Transform prefabProdAerialCombatCutinP
		{
			get
			{
				return this._prefabProdAerialCombatCutinP;
			}
		}

		public Transform prefabProdAerialCombatP1
		{
			get
			{
				return this._prefabProdAerialCombatP1;
			}
		}

		public Transform prefabProdSupportCutIn
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdSupportCutIn);
			}
		}

		public Transform prefabProdOpeningTorpedoCutIn
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdOpeningTorpedoCutIn);
			}
		}

		public Transform prefabProdShellingFormationJudge
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdShellingFormationJudge);
			}
		}

		public Transform prefabProdShellingSlotLine
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdShellingSlotLine);
			}
		}

		public Transform prefabProdTorpedoCutIn
		{
			get
			{
				return this._prefabProdTorpedoCutIn;
			}
		}

		public Transform prefabTorpedoStraightController
		{
			get
			{
				return this._prefabTorpedoStraightController;
			}
		}

		public Transform prefabProdTorpedoResucueCutIn
		{
			get
			{
				return this._prefabProdTorpedoResucueCutIn;
			}
		}

		public Transform prefabProdWithdrawalDecisionSelection
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdWithdrawalDecisionSelection);
			}
		}

		public Transform prefabProdNightRadarDeployment
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdNightRadarDeployment);
			}
		}

		public Transform prefabSearchLightSceneController
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabSearchLightSceneController);
			}
		}

		public Transform prefabFlareBulletSceneController
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabFlareBulletSceneController);
			}
		}

		public Transform prefabProdDeathCry
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdDeathCry);
			}
		}

		public Transform prefabProdVeteransReport
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdVeteransReport);
			}
		}

		public Transform prefabProdAdvancingWithDrawalSelect
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdAdvancingWithDrawalSelect);
			}
		}

		public Transform prefabProdAdvancingWithDrawalDC
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdAdvancingWithDrawalDC);
			}
		}

		public Transform prefabProdFlagshipWreck
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdFlagshipWreck);
			}
		}

		public Transform prefabProdCombatRation
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdCombatRation);
			}
		}

		public UIBattleNavigation battleNavigation
		{
			get
			{
				if (this._uiBattleNavigation == null)
				{
					this._uiBattleNavigation = UIBattleNavigation.Instantiate(BasePrefabFile.PassesPrefab(ref this._prefabUIBattleNavigation).GetComponent<UIBattleNavigation>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), BattleTaskManager.GetSettingModel());
				}
				return this._uiBattleNavigation;
			}
		}

		public ProdCloud prodCloud
		{
			get
			{
				if (this._prodCloud == null)
				{
					this._prodCloud = ProdCloud.Instantiate(BasePrefabFile.PassesPrefab(ref this._prefabProdCloud).GetComponent<ProdCloud>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
				}
				return this._prodCloud;
			}
		}

		public UICircleHPGauge circleHPGauge
		{
			get
			{
				if (this._uiCircleHPGauge == null)
				{
					BasePrefabFile.InstantiatePrefab<UICircleHPGauge>(ref this._uiCircleHPGauge, ref this._prefabUICircleHPGauge, BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
				}
				return this._uiCircleHPGauge;
			}
		}

		public ProdShellingSlotLine prodShellingSlotLine
		{
			get
			{
				if (this._prodShellingSlotLine == null)
				{
					BasePrefabFile.InstantiatePrefab<ProdShellingSlotLine>(ref this._prodShellingSlotLine, ref this._prefabProdShellingSlotLine, BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
				}
				return this._prodShellingSlotLine;
			}
			set
			{
				this._prodShellingSlotLine = value;
			}
		}

		public BattleShutter battleShutter
		{
			get
			{
				if (this._clsBattleShutter == null)
				{
					this._clsBattleShutter = BattleShutter.Instantiate((SortieBattleTaskManager.GetSortieBattlePrefabFile() == null) ? Resources.Load<BattleShutter>("Prefabs/Battle/UI/BattleShutter") : SortieBattleTaskManager.GetSortieBattlePrefabFile().prefabUIBattleShutter.GetComponent<BattleShutter>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), 20);
				}
				return this._clsBattleShutter;
			}
		}

		public ProdDamageCutIn prodDamageCutIn
		{
			get
			{
				if (this._prodDamageCutIn == null)
				{
					BasePrefabFile.InstantiatePrefab<ProdDamageCutIn>(ref this._prodDamageCutIn, ref this._prefabDamageCutIn, BattleTaskManager.GetBattleCameras().cutInEffectCamera.get_transform());
				}
				return this._prodDamageCutIn;
			}
		}

		public ProdBattleCommandBuffer prodBattleCommandBuffer
		{
			get
			{
				return this._prodBattleCommandBuffer;
			}
			set
			{
				this._prodBattleCommandBuffer = value;
			}
		}

		public ProdSinking prodSinking
		{
			get
			{
				if (this._prodSinking == null)
				{
					BasePrefabFile.InstantiatePrefab<ProdSinking>(ref this._prodSinking, ref this._prefabProdSinking, BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
				}
				return this._prodSinking;
			}
		}

		public ProdBufferEffect prodBufferEffect
		{
			get
			{
				if (this._prodBufferEffect == null)
				{
					this._prodBufferEffect = ProdBufferEffect.Instantiate(this._prefabProdBufferEffect.GetComponent<ProdBufferEffect>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
					Mem.Del<Transform>(ref this._prefabProdBufferEffect);
				}
				return this._prodBufferEffect;
			}
		}

		public void DisposeProdCloud()
		{
			if (this._prodCloud != null)
			{
				Mem.DelComponentSafe<ProdCloud>(ref this._prodCloud);
			}
		}

		public void DisposeProdCommandBuffer()
		{
			if (this.prodBattleCommandBuffer != null)
			{
				this.prodBattleCommandBuffer.BufferObjectConvergence();
				Mem.DelComponentSafe<ProdBattleCommandBuffer>(ref this._prodBattleCommandBuffer);
			}
		}

		public void DisposeUnneccessaryObject2Result()
		{
			Mem.Del<Transform>(ref this._prefabBattleField);
			Mem.Del<Transform>(ref this._prefabUICircleHPGauge);
			Mem.Del<Transform>(ref this._prefabProdBossInsert);
			Mem.Del<Transform>(ref this._prefabProdDetectionStartCutIn);
			Mem.Del<Transform>(ref this._prefabProdDetectionCutIn);
			Mem.Del<Transform>(ref this._prefabProdDetectionResultCutIn);
			Mem.Del<Transform>(ref this._prefabProdBattleCommandSelect);
			Mem.Del<Transform>(ref this._prefabProdBattleCommandBuffer);
			Mem.Del<Transform>(ref this._prefabProdBufferEffect);
			Mem.Del<Transform>(ref this._prefabUIBufferFleetCircle);
			Mem.Del<Transform>(ref this._prefabUIBufferShipCircle);
			Mem.Del<Transform>(ref this._prefabProdAerialCombatCutinP);
			Mem.Del<Transform>(ref this._prefabProdAerialCombatP1);
			Mem.Del<Transform>(ref this._prefabProdSupportCutIn);
			Mem.Del<Transform>(ref this._prefabProdOpeningTorpedoCutIn);
			Mem.Del<Transform>(ref this._prefabProdShellingFormationJudge);
			Mem.Del<Transform>(ref this._prefabProdShellingSlotLine);
			Mem.Del<Transform>(ref this._prefabProdTorpedoCutIn);
			Mem.Del<Transform>(ref this._prefabTorpedoStraightController);
			Mem.Del<Transform>(ref this._prefabProdTorpedoResucueCutIn);
			Mem.Del<Transform>(ref this._prefabProdWithdrawalDecisionSelection);
			Mem.Del<Transform>(ref this._prefabProdNightRadarDeployment);
			Mem.Del<Transform>(ref this._prefabSearchLightSceneController);
			Mem.Del<Transform>(ref this._prefabFlareBulletSceneController);
			Mem.Del<Transform>(ref this._prefabProdDeathCry);
			Mem.Del<ProdCloud>(ref this._prodCloud);
			Mem.DelComponentSafe<UICircleHPGauge>(ref this._uiCircleHPGauge);
			Mem.DelComponentSafe<ProdShellingSlotLine>(ref this._prodShellingSlotLine);
			Mem.DelComponentSafe<ProdDamageCutIn>(ref this._prodDamageCutIn);
			Mem.DelComponentSafe<ProdBattleCommandBuffer>(ref this._prodBattleCommandBuffer);
			Mem.DelComponentSafe<ProdSinking>(ref this._prodSinking);
			Mem.DelComponentSafe<ProdBufferEffect>(ref this._prodBufferEffect);
			Mem.Del<Transform>(ref this._prefabDamageCutIn);
			Mem.Del<Transform>(ref this._prefabProdSinking);
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<Transform>(ref this._prefabBattleShip);
			Mem.Del<Transform>(ref this._prefabFieldDimCamera);
			Mem.Del<Transform>(ref this._prefabProdCloud);
			Mem.Del<Transform>(ref this._prefabProdVeteransReport);
			Mem.Del<Transform>(ref this._prefabProdAdvancingWithDrawalSelect);
			Mem.Del<Transform>(ref this._prefabProdAdvancingWithDrawalDC);
			Mem.Del<Transform>(ref this._prefabProdFlagshipWreck);
			Mem.Del<Transform>(ref this._prefabProdCombatRation);
			Mem.Del<ProdShellingSlotLine>(ref this._prodShellingSlotLine);
			Mem.Del<ProdBattleCommandBuffer>(ref this._prodBattleCommandBuffer);
			Mem.Del<ProdDamageCutIn>(ref this._prodDamageCutIn);
			Mem.Del<ProdSinking>(ref this._prodSinking);
			Mem.Del<BattleShutter>(ref this._clsBattleShutter);
			Mem.Del<ProdBufferEffect>(ref this._prodBufferEffect);
			Mem.Del<UIBattleNavigation>(ref this._uiBattleNavigation);
			base.Dispose(disposing);
		}
	}
}
