using Common.Enum;
using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV.Strategy
{
	public class TaskStrategyDebug : SceneTaskMono
	{
		private enum Mode
		{
			CategorySelect,
			Material,
			Ship,
			SlotItem,
			Deck,
			Area,
			Slog,
			Heal,
			Large,
			Rebellion
		}

		private KeyControl keyController;

		[SerializeField]
		private GameObject rootPrefab;

		[SerializeField]
		private GameObject cursol;

		[SerializeField]
		private UILabel[] materialsLabel;

		[SerializeField]
		private UILabel[] materialsNum;

		[SerializeField]
		private UILabel MstID;

		[SerializeField]
		private UILabel MemID;

		[SerializeField]
		private UILabel[] ShipName;

		[SerializeField]
		private UILabel ShipLevel;

		[SerializeField]
		private UILabel ItemMstID;

		[SerializeField]
		private UILabel ItemName;

		[SerializeField]
		private UILabel DeckNum;

		[SerializeField]
		private UILabel AreaOpenNo;

		[SerializeField]
		private UILabel MapOpenNo;

		[SerializeField]
		private UILabel ClearState;

		[SerializeField]
		private UILabel[] Category;

		[SerializeField]
		private UILabel SlogOnOff;

		[SerializeField]
		private UILabel LargeOnOff;

		[SerializeField]
		private UILabel RebellionForceOnOff;

		[SerializeField]
		private Transform DebugMode1;

		[SerializeField]
		private Transform DebugMode3;

		[SerializeField]
		private Transform DebugMenuNormal;

		[SerializeField]
		private UnloadAtlas unloadAtlas;

		private int mag;

		private Vector3 cursolOffset;

		private StrategyMapManager logicMng;

		public static bool isControl;

		public static bool ForceEnding;

		private TaskStrategyDebug.Mode nowMode;

		private Debug_Mod debugMod;

		private List<int> openAreaIDs;

		private List<int> AddMstIDs;

		private int materialPhase;

		private int nowMaterial;

		private bool AreaModeCursol = true;

		private int maxIndex;

		[SerializeField]
		private TaskStrategyDebug AnotherMode;

		private Coroutine turnend;

		protected override bool Init()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			this.maxIndex = this.Category.Length - 1;
			this.logicMng = new StrategyMapManager();
			this.rootPrefab.SetActive(true);
			this.keyController = new KeyControl(0, this.maxIndex, 0.4f, 0.1f);
			this.keyController.setChangeValue(-1f, 0f, 1f, 0f);
			this.keyController.HoldJudgeTime = 1f;
			this.nowMode = TaskStrategyDebug.Mode.CategorySelect;
			this.cursolOffset = new Vector3(-77f, -20f, 0f);
			this.MstID.text = "1";
			this.MemID.text = "1";
			this.debugMod = new Debug_Mod();
			this.materialsNum[0].text = StrategyTopTaskManager.GetLogicManager().Material.Fuel.ToString();
			this.materialsNum[1].text = StrategyTopTaskManager.GetLogicManager().Material.Ammo.ToString();
			this.materialsNum[2].text = StrategyTopTaskManager.GetLogicManager().Material.Steel.ToString();
			this.materialsNum[3].text = StrategyTopTaskManager.GetLogicManager().Material.Baux.ToString();
			this.materialsNum[4].text = StrategyTopTaskManager.GetLogicManager().Material.Devkit.ToString();
			this.materialsNum[5].text = StrategyTopTaskManager.GetLogicManager().Material.RepairKit.ToString();
			this.materialsNum[6].text = StrategyTopTaskManager.GetLogicManager().Material.BuildKit.ToString();
			this.materialsNum[7].text = StrategyTopTaskManager.GetLogicManager().Material.Revkit.ToString();
			this.materialsNum[8].text = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountNoMove().ToString();
			this.materialsNum[9].text = StrategyTopTaskManager.GetLogicManager().UserInfo.FCoin.ToString();
			this.materialsNum[10].text = StrategyTopTaskManager.GetLogicManager().UserInfo.SPoint.ToString();
			this.materialsNum[11].text = "0";
			int focusAreaID = StrategyAreaManager.FocusAreaID;
			this.AreaOpenNo.text = focusAreaID.ToString();
			if (StrategyTopTaskManager.GetLogicManager().SelectArea(focusAreaID).Maps[0].Cleared)
			{
			}
			this.openAreaIDs = new List<int>();
			this.AddMstIDs = new List<int>();
			this.ShipName[0].text = new ShipModelMst(1).Name;
			ShipModel ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(1);
			if (ship != null)
			{
				this.ShipName[1].text = ship.Name;
				this.ShipLevel.text = ship.Level.ToString();
			}
			this.ItemMstID.text = "1";
			this.SlogOnOff.text = SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw.ToString();
			this.DeckNum.text = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount.ToString();
			this.LargeOnOff.text = ((!new ArsenalManager().LargeEnabled) ? "OFF" : "ON");
			return true;
		}

		protected override bool UnInit()
		{
			if (StrategyTopTaskManager.Instance != null)
			{
				StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(true);
				StrategyTopTaskManager.Instance.GetInfoMng().updateUpperInfo();
			}
			return true;
		}

		protected override bool Run()
		{
			this.keyController.Update();
			if (!TaskStrategyDebug.isControl)
			{
				if (this.keyController.keyState.get_Item(0).down)
				{
					this.rootPrefab.SetActive(false);
					StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
					return false;
				}
				return true;
			}
			else
			{
				if (this.keyController.keyState.get_Item(1).down)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				}
				this.setMag();
				if (this.keyController.keyState.get_Item(6).down && this.keyController.keyState.get_Item(5).press)
				{
					Application.LoadLevel(Generics.Scene.Ending.ToString());
				}
				else if (this.keyController.keyState.get_Item(5).press && this.keyController.keyState.get_Item(2).down)
				{
					App.isInvincible = !App.isInvincible;
					CommonPopupDialog.Instance.StartPopup("無敵モード" + App.isInvincible);
				}
				else if (this.keyController.keyState.get_Item(4).press && this.keyController.keyState.get_Item(2).down)
				{
					for (int i = 1; i < 15; i++)
					{
						EscortDeckManager escortDeckManager = new EscortDeckManager(i);
						if (StrategyTopTaskManager.GetLogicManager().UserInfo.ShipCountData().NowCount > 300)
						{
							for (int j = 0; j < 300; j++)
							{
								if (StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(1 + j).IsInEscortDeck() == -1 && escortDeckManager.ChangeOrganize(6, 1 + j))
								{
									break;
								}
							}
						}
						StrategyTopTaskManager.GetLogicManager().Deploy(i, 10, escortDeckManager);
					}
					CommonPopupDialog.Instance.StartPopup("自動配備しました");
				}
				else if (this.keyController.keyState.get_Item(6).down && this.keyController.keyState.get_Item(4).press)
				{
					CommonPopupDialog.Instance.StartPopup("ゲームクリア！！");
					TaskStrategyDebug.ForceEnding = true;
				}
				else if (this.keyController.keyState.get_Item(6).down && this.keyController.keyState.get_Item(3).press)
				{
					if (this.turnend == null)
					{
						this.turnend = base.StartCoroutine(this.TurnEndSpeed(3495));
					}
				}
				else if (this.keyController.keyState.get_Item(6).press && this.keyController.keyState.get_Item(2).press)
				{
					StrategyTopTaskManager.GetTurnEnd().DebugTurnEnd();
					CommonPopupDialog.Instance.StartPopup(StrategyTopTaskManager.GetLogicManager().Turn.ToString());
				}
				else if (this.keyController.keyState.get_Item(6).down)
				{
					StrategyTopTaskManager.Instance.GameOver();
					this.keyController.firstUpdate = true;
				}
				else if (this.keyController.keyState.get_Item(3).down)
				{
					TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
					for (int k = 0; k < 20; k++)
					{
						tutorial.SetStepTutorialFlg(k);
					}
					for (int l = 0; l < 99; l++)
					{
						tutorial.SetKeyTutorialFlg(l);
					}
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
					}
				}
				if (this.keyController.keyState.get_Item(7).down && this.keyController.keyState.get_Item(4).press)
				{
					GameObject.Find("SingletonObject").AddComponent<TEST_Voyage>().StartVoyage();
				}
				if (this.keyController.keyState.get_Item(7).down && this.keyController.keyState.get_Item(5).press)
				{
					Object.Destroy(GameObject.Find("Live2DRender").get_gameObject());
					Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.get_gameObject());
					Object.Destroy(GameObject.Find("SingletonObject").get_gameObject());
					this.DelayActionFrame(3, delegate
					{
						Application.LoadLevel("TestEmptyScene");
						this.DelayAction(5f, delegate
						{
							Resources.UnloadUnusedAssets();
						});
					});
				}
				if (this.keyController.keyState.get_Item(4).hold && this.keyController.keyState.get_Item(5).hold)
				{
					if (base.get_gameObject().get_name() == "DebugMenuNormal")
					{
						this.DebugMenuNormal.SetActive(false);
						this.DebugMode1.SetActive(true);
						this.DebugMode3.SetActive(true);
					}
					else
					{
						this.DebugMenuNormal.SetActive(true);
						this.DebugMode1.SetActive(false);
						this.DebugMode3.SetActive(false);
					}
					StrategyTopTaskManager.SetDebug(this.AnotherMode);
					StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Debug);
					return false;
				}
				if (!Diagnostics.get_enableHUD() && this.keyController.keyState.get_Item(4).press && this.keyController.keyState.get_Item(5).press && this.keyController.keyState.get_Item(2).down)
				{
					Diagnostics.set_enableHUD(true);
				}
				return this.ModeRun();
			}
		}

		[DebuggerHidden]
		private IEnumerator TurnEndSpeed(int turn)
		{
			TaskStrategyDebug.<TurnEndSpeed>c__Iterator18F <TurnEndSpeed>c__Iterator18F = new TaskStrategyDebug.<TurnEndSpeed>c__Iterator18F();
			<TurnEndSpeed>c__Iterator18F.turn = turn;
			<TurnEndSpeed>c__Iterator18F.<$>turn = turn;
			return <TurnEndSpeed>c__Iterator18F;
		}

		private void setMag()
		{
			if (this.keyController.keyState.get_Item(3).press)
			{
				this.mag = 10;
			}
			else if (this.keyController.keyState.get_Item(2).press)
			{
				this.mag = 100;
			}
			else if (this.keyController.keyState.get_Item(5).press)
			{
				this.mag = 1000;
			}
			else if (this.keyController.keyState.get_Item(4).press)
			{
				this.mag = 10000;
			}
			else
			{
				this.mag = 1;
			}
		}

		private bool ModeRun()
		{
			switch (this.nowMode)
			{
			case TaskStrategyDebug.Mode.CategorySelect:
				return this.CategorySelectMode();
			case TaskStrategyDebug.Mode.Material:
				this.MaterialMode();
				break;
			case TaskStrategyDebug.Mode.Ship:
				this.ShipMode();
				break;
			case TaskStrategyDebug.Mode.SlotItem:
				this.SlotItemMode();
				break;
			case TaskStrategyDebug.Mode.Deck:
				this.DeckMode();
				break;
			case TaskStrategyDebug.Mode.Area:
				this.AreaMode();
				break;
			case TaskStrategyDebug.Mode.Slog:
				this.SlogMode();
				break;
			case TaskStrategyDebug.Mode.Heal:
				this.HealMode();
				break;
			case TaskStrategyDebug.Mode.Large:
				this.LargeMode();
				break;
			case TaskStrategyDebug.Mode.Rebellion:
				this.RebellionMode();
				break;
			}
			return true;
		}

		private void ChangeMode(int nextMode)
		{
			this.nowMode = (TaskStrategyDebug.Mode)nextMode;
			this.keyController.Index = 0;
			switch (this.nowMode)
			{
			case TaskStrategyDebug.Mode.CategorySelect:
				this.keyController.Index = (int)this.nowMode;
				this.keyController.maxIndex = this.maxIndex;
				this.keyController.setChangeValue(-1f, 0f, 1f, 0f);
				break;
			case TaskStrategyDebug.Mode.Material:
				this.keyController.maxIndex = 11;
				break;
			case TaskStrategyDebug.Mode.Ship:
				this.keyController.maxIndex = 1;
				break;
			case TaskStrategyDebug.Mode.SlotItem:
				this.keyController.maxIndex = 999;
				this.keyController.Index = 1;
				this.keyController.setChangeValue(0f, 1f, 0f, -1f);
				break;
			case TaskStrategyDebug.Mode.Deck:
				this.keyController.maxIndex = 0;
				break;
			case TaskStrategyDebug.Mode.Area:
				this.keyController.maxIndex = 0;
				break;
			}
		}

		private bool CategorySelectMode()
		{
			this.keyController.SilentChangeIndex(this.SeachActiveIndex(this.keyController.Index, this.Category, this.keyController.prevIndexChangeValue == 1));
			this.cursol.get_transform().set_position(this.Category[this.keyController.Index].get_transform().get_position());
			Transform expr_67 = this.cursol.get_transform();
			expr_67.set_localPosition(expr_67.get_localPosition() + this.cursolOffset);
			if (this.keyController.keyState.get_Item(1).down)
			{
				this.ChangeMode(this.keyController.Index + 1);
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.rootPrefab.SetActive(false);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
				return false;
			}
			if (this.keyController.keyState.get_Item(3).down)
			{
				Live2DModel.__DEBUG_MotionNAME_Draw = !Live2DModel.__DEBUG_MotionNAME_Draw;
				CommonPopupDialog.Instance.StartPopup("モーション名表示:" + Live2DModel.__DEBUG_MotionNAME_Draw);
			}
			return true;
		}

		private int SeachActiveIndex(int index, MonoBehaviour[] Array, bool isPositive)
		{
			int num = (!isPositive) ? -1 : 1;
			for (int i = 0; i < Array.Length; i++)
			{
				if (Array[index].get_isActiveAndEnabled())
				{
					break;
				}
				index = (int)Util.LoopValue(index + num, 0f, (float)(Array.Length - 1));
			}
			return index;
		}

		private void MaterialMode()
		{
			if (this.materialPhase == 0)
			{
				this.keyController.SilentChangeIndex(this.SeachActiveIndex(this.keyController.Index, this.materialsLabel, this.keyController.prevIndexChangeValue == 1));
				this.cursol.get_transform().set_position(this.materialsLabel[this.keyController.Index].get_transform().get_position());
				Transform expr_72 = this.cursol.get_transform();
				expr_72.set_localPosition(expr_72.get_localPosition() + this.cursolOffset);
			}
			else
			{
				this.cursol.get_transform().set_position(this.materialsNum[this.nowMaterial].get_transform().get_position());
				Transform expr_BF = this.cursol.get_transform();
				expr_BF.set_localPosition(expr_BF.get_localPosition() + new Vector3(-150f, -20f, 0f));
				if (this.nowMaterial != 8)
				{
					if (this.keyController.keyState.get_Item(8).down)
					{
						this.materialsNum[this.nowMaterial].text = (Convert.ToInt32(this.materialsNum[this.nowMaterial].text) + this.mag).ToString();
					}
					if (this.keyController.keyState.get_Item(12).down)
					{
						this.materialsNum[this.nowMaterial].text = (Convert.ToInt32(this.materialsNum[this.nowMaterial].text) - this.mag).ToString();
					}
				}
				else if (this.keyController.keyState.get_Item(1).down)
				{
					if (this.mag > 10)
					{
						this.mag = 10;
					}
					Debug_Mod.Add_Tunker(1 * this.mag);
					StrategyTopTaskManager.CreateLogicManager();
					this.materialsNum[8].text = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountNoMove().ToString();
				}
			}
			if (this.keyController.keyState.get_Item(1).down && this.materialPhase == 0)
			{
				this.materialPhase = 1;
				this.nowMaterial = this.keyController.Index;
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				if (this.materialPhase == 0)
				{
					this.ChangeMode(0);
					StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
					this.debugMod.Add_Materials(enumMaterialCategory.Fuel, Convert.ToInt32(this.materialsNum[0].text) - logicManager.Material.Fuel);
					this.debugMod.Add_Materials(enumMaterialCategory.Bull, Convert.ToInt32(this.materialsNum[1].text) - logicManager.Material.Ammo);
					this.debugMod.Add_Materials(enumMaterialCategory.Steel, Convert.ToInt32(this.materialsNum[2].text) - logicManager.Material.Steel);
					this.debugMod.Add_Materials(enumMaterialCategory.Bauxite, Convert.ToInt32(this.materialsNum[3].text) - logicManager.Material.Baux);
					this.debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, Convert.ToInt32(this.materialsNum[4].text) - logicManager.Material.Devkit);
					this.debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, Convert.ToInt32(this.materialsNum[5].text) - logicManager.Material.RepairKit);
					this.debugMod.Add_Materials(enumMaterialCategory.Build_Kit, Convert.ToInt32(this.materialsNum[6].text) - logicManager.Material.BuildKit);
					this.debugMod.Add_Materials(enumMaterialCategory.Revamp_Kit, Convert.ToInt32(this.materialsNum[7].text) - logicManager.Material.Revkit);
					this.debugMod.Add_Coin(Convert.ToInt32(this.materialsNum[9].text) - logicManager.UserInfo.FCoin);
					this.debugMod.Add_Spoint(Convert.ToInt32(this.materialsNum[10].text) - logicManager.UserInfo.SPoint);
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					for (int i = 0; i < 100; i++)
					{
						if ((1 <= i && i <= 3) || (10 <= i && i <= 12) || (49 <= i && i <= 59))
						{
							dictionary.set_Item(i, Convert.ToInt32(this.materialsNum[11].text));
						}
					}
					this.debugMod.Add_UseItem(dictionary);
				}
				else
				{
					this.materialPhase = 0;
					this.keyController.Index = this.nowMaterial;
				}
			}
		}

		private void ShipMode()
		{
			int num = Convert.ToInt32(this.MstID.text);
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.ChangeMode(0);
				this.debugMod.Add_Ship(this.AddMstIDs);
				this.AddMstIDs.Clear();
			}
			if (this.keyController.Index == 0)
			{
				this.cursol.get_transform().set_position(this.MstID.get_transform().get_position());
				Transform expr_8B = this.cursol.get_transform();
				expr_8B.set_localPosition(expr_8B.get_localPosition() + this.cursolOffset);
				if (this.keyController.keyState.get_Item(1).down && this.ShipName[0].text != string.Empty && this.ShipName[0].text != "なし")
				{
					this.AddMstIDs.Add(num);
					this.logicMng = new StrategyMapManager();
				}
				if (this.keyController.keyState.get_Item(10).down || this.keyController.keyState.get_Item(14).down)
				{
					int num2 = this.mag;
					if (this.keyController.keyState.get_Item(14).down)
					{
						num2 = -this.mag;
					}
					num += num2;
					this.MstID.text = num.ToString();
					if (Mst_DataManager.Instance.Mst_ship.ContainsKey(num))
					{
						ShipModelMst shipModelMst = new ShipModelMst(Convert.ToInt32(this.MstID.text));
						this.ShipName[0].text = shipModelMst.Name;
					}
					else
					{
						this.ShipName[0].text = string.Empty;
					}
				}
			}
			else if (this.keyController.Index == 1)
			{
				this.cursol.get_transform().set_position(this.MemID.get_transform().get_position());
				Transform expr_21A = this.cursol.get_transform();
				expr_21A.set_localPosition(expr_21A.get_localPosition() + this.cursolOffset);
				if (this.keyController.keyState.get_Item(1).down)
				{
					this.ShipLevelUp(1);
				}
				else if (this.keyController.keyState.get_Item(2).down)
				{
					this.ShipLevelUp(10);
				}
				else if (this.keyController.keyState.get_Item(3).down)
				{
					this.ShipLevelUp(100);
				}
				else if (this.keyController.keyState.get_Item(5).down)
				{
					this.ShipLevelUp(100);
					int ship_mem_id = Convert.ToInt32(this.MemID.text);
					ShipModel ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id);
					PortManager portManager = new PortManager(1);
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					dictionary.set_Item(55, 1);
					this.debugMod.Add_UseItem(dictionary);
					portManager.Marriage(ship.MemId);
					this.ShipLevel.text = ship.Level.ToString();
				}
				else if (this.keyController.keyState.get_Item(10).down || this.keyController.keyState.get_Item(14).down)
				{
					int num3 = this.mag;
					if (this.keyController.keyState.get_Item(14).down)
					{
						num3 = -this.mag;
					}
					int ship_mem_id2 = Convert.ToInt32(this.MemID.text) + num3;
					this.MemID.text = ship_mem_id2.ToString();
					if (StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id2) != null)
					{
						ShipModel ship2 = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id2);
						this.ShipName[1].text = ship2.Name;
						this.ShipLevel.text = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id2).Level.ToString();
					}
					else
					{
						this.ShipName[1].text = "NONE";
					}
				}
			}
		}

		private void ShipLevelUp(int AddLevel)
		{
			if (this.ShipName[1].text != "NONE")
			{
				int ship_mem_id = Convert.ToInt32(this.MemID.text);
				ShipModel ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(ship_mem_id);
				for (int i = 0; i < AddLevel; i++)
				{
					ship.AddExp(ship.Exp_Next);
				}
				this.ShipLevel.text = ship.Level.ToString();
			}
		}

		private void SlotItemMode()
		{
			this.cursol.get_transform().set_position(this.ItemMstID.get_transform().get_position());
			Transform expr_2B = this.cursol.get_transform();
			expr_2B.set_localPosition(expr_2B.get_localPosition() + this.cursolOffset);
			if (this.keyController.IsChangeIndex)
			{
				int num = 0;
				if (this.keyController.keyState.get_Item(10).down)
				{
					num = 1;
				}
				if (this.keyController.keyState.get_Item(14).down)
				{
					num = -1;
				}
				this.keyController.Index = (int)Util.RangeValue(this.keyController.Index + num * this.mag - num, 1f, 150f);
				this.ItemMstID.textInt = this.keyController.Index;
				SlotitemModel_Mst slotitemModel_Mst = new SlotitemModel_Mst(this.keyController.Index);
				if (slotitemModel_Mst != null)
				{
					this.ItemName.text = slotitemModel_Mst.Name;
				}
				else
				{
					this.ItemName.text = "NONE";
				}
			}
			if (this.keyController.keyState.get_Item(1).down)
			{
				this.AddMstIDs.Add(this.keyController.Index);
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.ChangeMode(0);
				this.debugMod.Add_SlotItem(this.AddMstIDs);
				this.AddMstIDs.Clear();
			}
		}

		private void DeckMode()
		{
			this.cursol.get_transform().set_position(this.DeckNum.get_transform().get_position());
			Transform expr_2B = this.cursol.get_transform();
			expr_2B.set_localPosition(expr_2B.get_localPosition() + this.cursolOffset);
			if (this.keyController.keyState.get_Item(1).down)
			{
				int deckCount = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount;
				if (deckCount < 8)
				{
					int rid = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount + 1;
					this.debugMod.Add_Deck(rid);
				}
				this.DeckNum.text = StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount.ToString();
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.ChangeMode(0);
			}
		}

		private void AreaMode()
		{
			int num = Convert.ToInt32(this.AreaOpenNo.text);
			int num2 = Convert.ToInt32(this.MapOpenNo.text);
			if (this.AreaModeCursol)
			{
				this.cursol.get_transform().set_position(this.AreaOpenNo.get_transform().get_position());
			}
			else
			{
				this.cursol.get_transform().set_position(this.MapOpenNo.get_transform().get_position());
			}
			Transform expr_7D = this.cursol.get_transform();
			expr_7D.set_localPosition(expr_7D.get_localPosition() + this.cursolOffset);
			if (this.keyController.keyState.get_Item(10).down || this.keyController.keyState.get_Item(14).down)
			{
				this.AreaModeCursol = !this.AreaModeCursol;
			}
			if (this.keyController.keyState.get_Item(12).down)
			{
				if (this.AreaModeCursol)
				{
					num--;
				}
				else
				{
					num2--;
				}
			}
			if (this.keyController.keyState.get_Item(8).down)
			{
				if (this.AreaModeCursol)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
			if (this.keyController.keyState.get_Item(1).down)
			{
				if (this.AreaModeCursol)
				{
					for (int i = 1; i < 7; i++)
					{
						Debug_Mod.OpenMapArea(num, i);
					}
				}
				else
				{
					Debug_Mod.OpenMapArea(num, num2);
				}
			}
			num = Util.FixRangeValue(num, 1, 17, 1);
			num2 = Util.FixRangeValue(num2, 1, 5, 1);
			this.AreaOpenNo.text = num.ToString();
			this.MapOpenNo.text = num2.ToString();
			if (StrategyTopTaskManager.GetLogicManager().SelectArea(num).Maps.Length > num2 - 1)
			{
				if (StrategyTopTaskManager.GetLogicManager().SelectArea(num).Maps[num2 - 1].Cleared)
				{
					this.ClearState.text = "状態：クリア済み";
				}
				else
				{
					this.ClearState.text = "状態：未クリア";
				}
			}
			else
			{
				this.ClearState.text = "マップが存在しません";
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("newOpenAreaIDs", this.openAreaIDs.ToArray());
				RetentionData.SetData(hashtable);
				this.ChangeMode(0);
			}
		}

		private void SlogMode()
		{
			this.cursol.get_transform().set_position(this.SlogOnOff.get_transform().get_position());
			Transform expr_2B = this.cursol.get_transform();
			expr_2B.set_localPosition(expr_2B.get_localPosition() + this.cursolOffset);
			if (this.keyController.keyState.get_Item(1).down)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw = !SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw;
				DebugUtils.ClearSLog();
				this.SlogOnOff.text = SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw.ToString();
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.ChangeMode(0);
			}
		}

		private void LargeMode()
		{
			this.cursol.get_transform().set_position(this.LargeOnOff.get_transform().get_position());
			Transform expr_2B = this.cursol.get_transform();
			expr_2B.set_localPosition(expr_2B.get_localPosition() + this.cursolOffset);
			if (this.keyController.keyState.get_Item(1).down)
			{
				Debug_Mod.OpenLargeDock();
				this.LargeOnOff.text = "ON";
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.ChangeMode(0);
			}
		}

		private void HealMode()
		{
			Debug_Mod.DeckRefresh(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
			this.ChangeMode(0);
		}

		private void RebellionMode()
		{
			this.cursol.get_transform().set_position(this.RebellionForceOnOff.get_transform().get_position());
			Transform expr_2B = this.cursol.get_transform();
			expr_2B.set_localPosition(expr_2B.get_localPosition() + this.cursolOffset);
			if (this.keyController.keyState.get_Item(1).down)
			{
				if (StrategyRebellionTaskManager.RebellionForceDebug)
				{
					this.RebellionForceOnOff.text = "OFF";
					StrategyRebellionTaskManager.RebellionForceDebug = false;
				}
				else
				{
					this.RebellionForceOnOff.text = "ON";
					StrategyRebellionTaskManager.RebellionForceDebug = true;
				}
			}
			if (this.keyController.keyState.get_Item(0).down)
			{
				this.ChangeMode(0);
			}
		}

		public void ChangeDebugMode()
		{
			this.DebugMode3.SetActive(true);
			this.DebugMode1.SetActive(false);
			TaskStrategyDebug.isControl = false;
		}

		private void OnDestroy()
		{
			this.keyController = null;
			this.rootPrefab = null;
			this.cursol = null;
			this.materialsLabel = null;
			this.materialsNum = null;
			this.MstID = null;
			this.MemID = null;
			this.ShipName = null;
			this.ShipLevel = null;
			this.ItemMstID = null;
			this.ItemName = null;
			this.DeckNum = null;
			this.AreaOpenNo = null;
			this.MapOpenNo = null;
			this.ClearState = null;
			this.Category = null;
			this.SlogOnOff = null;
			this.LargeOnOff = null;
			this.RebellionForceOnOff = null;
			this.DebugMode1 = null;
			this.DebugMode3 = null;
			this.AnotherMode = null;
		}
	}
}
