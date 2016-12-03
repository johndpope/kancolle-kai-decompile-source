using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			None,
			TorpedoCutInFriend,
			TorpedoCutInEnemy,
			ProdTorpedoCutIn
		}

		[SerializeField]
		private List<UITexture> _listShipTex;

		private ProdTorpedoCutIn.AnimationList _iList;

		private RaigekiModel _clsRaigeki;

		public static ProdTorpedoCutIn Instantiate(ProdTorpedoCutIn prefab, RaigekiModel model, Transform parent)
		{
			ProdTorpedoCutIn prodTorpedoCutIn = Object.Instantiate<ProdTorpedoCutIn>(prefab);
			prodTorpedoCutIn.get_transform().set_parent(parent);
			prodTorpedoCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodTorpedoCutIn.get_transform().set_localScale(Vector3.get_one());
			prodTorpedoCutIn._clsRaigeki = model;
			prodTorpedoCutIn.init();
			prodTorpedoCutIn.setShipInfo();
			return prodTorpedoCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelListSafe<UITexture>(ref this._listShipTex);
			Mem.Del<ProdTorpedoCutIn.AnimationList>(ref this._iList);
			this._clsRaigeki = null;
		}

		private void init()
		{
			this._iList = ProdTorpedoCutIn.AnimationList.None;
			if (this._listShipTex == null)
			{
				this._listShipTex = new List<UITexture>();
				using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FleetType fleetType = (FleetType)((int)enumerator.get_Current());
						if (fleetType != FleetType.CombinedFleet)
						{
							this._listShipTex.Add(base.get_transform().FindChild(string.Format("{0}Ship/Anchor/Object2D", fleetType.ToString())).GetComponent<UITexture>());
						}
					}
				}
			}
		}

		public bool Run()
		{
			return false;
		}

		private void setShipInfo()
		{
			ShipModel_Attacker torpedoCutInShip = this.getTorpedoCutInShip(this._clsRaigeki, true);
			if (torpedoCutInShip != null)
			{
				this._listShipTex.get_Item(0).mainTexture = ShipUtils.LoadTexture(torpedoCutInShip);
				this._listShipTex.get_Item(0).MakePixelPerfect();
				this._listShipTex.get_Item(0).get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(torpedoCutInShip.GetGraphicsMstId()).GetShipDisplayCenter(torpedoCutInShip.DamagedFlg)));
			}
			ShipModel_Attacker torpedoCutInShip2 = this.getTorpedoCutInShip(this._clsRaigeki, false);
			if (torpedoCutInShip2 != null)
			{
				this._listShipTex.get_Item(1).mainTexture = ShipUtils.LoadTexture(torpedoCutInShip2);
				this._listShipTex.get_Item(1).MakePixelPerfect();
				this._listShipTex.get_Item(1).get_transform().set_localPosition(ShipUtils.GetShipOffsPos(torpedoCutInShip2, MstShipGraphColumn.CutIn));
			}
			this._iList = this.getAnimationList(torpedoCutInShip, torpedoCutInShip2);
		}

		private void debugShipInfo()
		{
		}

		public override void Play(Action callback)
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			base.get_transform().set_localScale(Vector3.get_one());
			this._actCallback = callback;
			base.GetComponent<UIPanel>().widgetsAreStatic = false;
			if (this._iList == ProdTorpedoCutIn.AnimationList.None)
			{
				this.onAnimationFinishedAfterDiscard();
				return;
			}
			base.Play(this._iList, callback);
			if (this._iList == ProdTorpedoCutIn.AnimationList.ProdTorpedoCutIn || this._iList == ProdTorpedoCutIn.AnimationList.TorpedoCutInFriend)
			{
				ShipUtils.PlayTorpedoVoice(this.getTorpedoCutInShip(this._clsRaigeki, true));
			}
			else if (this._iList == ProdTorpedoCutIn.AnimationList.TorpedoCutInEnemy)
			{
			}
		}

		private void DebugPlay()
		{
			this._iList = ProdTorpedoCutIn.AnimationList.ProdTorpedoCutIn;
			this._actCallback = null;
			base.GetComponent<Animation>().Stop();
			base.Play(this._iList, null);
			this.debugShipInfo();
			if (this._iList == ProdTorpedoCutIn.AnimationList.ProdTorpedoCutIn || this._iList == ProdTorpedoCutIn.AnimationList.TorpedoCutInFriend)
			{
				ShipUtils.PlayTorpedoVoice(this.getTorpedoCutInShip(this._clsRaigeki, true));
			}
			else if (this._iList == ProdTorpedoCutIn.AnimationList.TorpedoCutInEnemy)
			{
			}
		}

		private ProdTorpedoCutIn.AnimationList getAnimationList(ShipModel_Battle friendShip, ShipModel_Battle enemyShip)
		{
			if (friendShip == null && enemyShip == null)
			{
				return ProdTorpedoCutIn.AnimationList.None;
			}
			if (friendShip != null && enemyShip == null)
			{
				return ProdTorpedoCutIn.AnimationList.TorpedoCutInFriend;
			}
			if (friendShip == null && enemyShip != null)
			{
				return ProdTorpedoCutIn.AnimationList.TorpedoCutInEnemy;
			}
			return ProdTorpedoCutIn.AnimationList.ProdTorpedoCutIn;
		}

		private ShipModel_Attacker getTorpedoCutInShip(RaigekiModel model, bool isFriend)
		{
			List<ShipModel_Attacker> attackers = model.GetAttackers(isFriend);
			if (attackers == null)
			{
				return null;
			}
			using (List<ShipModel_Attacker>.Enumerator enumerator = attackers.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.get_Current();
				}
			}
			return null;
		}

		private void startMotionBlur()
		{
		}

		private void endMotionBlur()
		{
		}

		private void onPlaySeAnime(int seNo)
		{
			if (seNo == 0)
			{
				SEFIleInfos info = SEFIleInfos.BattleAdmission;
				base._playSE(info);
			}
			else if (seNo == 1)
			{
				SEFIleInfos info = SEFIleInfos.BattleNightMessage;
				base._playSE(info);
			}
		}

		private void onInitBackground()
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.SetLayer(Generics.Layers.ShipGirl);
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.ResetFleetAnchorPosition();
			BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SwitchMainCamera(FleetType.Friend);
			battleCameras.InitEnemyFieldCameraDefault();
			BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
			BattleFieldCamera enemyFieldCamera = battleCameras.enemyFieldCamera;
			battleCameras.isFieldDimCameraEnabled = false;
			friendFieldCamera.ResetMotionBlur();
			friendFieldCamera.clearFlags = 1;
			friendFieldCamera.cullingMask = battleCameras.GetDefaultLayers();
			enemyFieldCamera.cullingMask = battleCameras.GetEnemyCamSplitLayers();
			battleCameras.SetVerticalSplitCameras(true);
			friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			enemyFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Friend).get_position();
			friendFieldCamera.get_transform().set_position(new Vector3(-51f, 8f, 90f));
			friendFieldCamera.get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 70f, 0f)));
			Vector3 position2 = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Enemy).get_position();
			enemyFieldCamera.get_transform().set_position(new Vector3(-51f, 8f, -90f));
			enemyFieldCamera.get_transform().set_rotation(Quaternion.Euler(new Vector3(10.5f, 111f, 0f)));
			battleField.isEnemySeaLevelActive = true;
			battleField.AlterWaveDirection(FleetType.Friend, FleetType.Friend);
			battleField.AlterWaveDirection(FleetType.Enemy, FleetType.Enemy);
			BattleShips battleShips2 = BattleTaskManager.GetBattleShips();
			battleShips2.RadarDeployment(false);
			battleShips2.SetBollboardTarget(false, enemyFieldCamera.get_transform());
			battleShips2.SetTorpedoSalvoWakeAngle(true);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UITexture component = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
			if (component != null)
			{
				component.alpha = 1f;
			}
			BattleTaskManager.GetBattleCameras().fieldDimCamera.maskAlpha = 0f;
			using (Dictionary<int, UIBattleShip>.ValueCollection.Enumerator enumerator = BattleTaskManager.GetBattleShips().dicFriendBattleShips.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIBattleShip current = enumerator.get_Current();
					current.billboard.billboardTarget = BattleTaskManager.GetBattleCameras().friendFieldCamera.get_transform();
				}
			}
			using (Dictionary<int, UIBattleShip>.ValueCollection.Enumerator enumerator2 = BattleTaskManager.GetBattleShips().dicEnemyBattleShips.get_Values().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIBattleShip current2 = enumerator2.get_Current();
					current2.billboard.billboardTarget = BattleTaskManager.GetBattleCameras().enemyFieldCamera.get_transform();
				}
			}
		}
	}
}
