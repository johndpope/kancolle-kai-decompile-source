using KCV.Battle.Utils;
using Librarys.Cameras;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleFleetAdvent : BaseBattleTask
	{
		private Dictionary<FleetType, ParticleSystem> _dicPSClouds;

		protected override bool Init()
		{
			if (!BattleTaskManager.GetBattleShips().isInitialize)
			{
				return false;
			}
			this._dicPSClouds = BattleTaskManager.GetBattleField().dicParticleClouds;
			Vector3 position = BattleTaskManager.GetBattleField().dicFleetAnchor.get_Item(FleetType.Friend).get_position();
			position.y = 20f;
			this._dicPSClouds.get_Item(FleetType.Friend).get_transform().set_position(position);
			position = BattleTaskManager.GetBattleField().dicFleetAnchor.get_Item(FleetType.Enemy).get_position();
			position.y = 20f;
			this._dicPSClouds.get_Item(FleetType.Enemy).get_transform().set_position(position);
			this._clsState = new StatementMachine();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitFriendFleetAdvent), new StatementMachine.StatementMachineUpdate(this.UpdateFriendFleetAdvent));
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			using (Dictionary<FleetType, ParticleSystem>.Enumerator enumerator = this._dicPSClouds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FleetType, ParticleSystem> current = enumerator.get_Current();
					current.get_Value().Stop();
					current.get_Value().SetActive(false);
				}
			}
			this._dicPSClouds = null;
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.FleetAdvent);
		}

		private bool InitFriendFleetAdvent(object data)
		{
			this._dicPSClouds.get_Item(FleetType.Friend).SetActive(true);
			this._dicPSClouds.get_Item(FleetType.Friend).Play();
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			UIBattleShip uIBattleShip = BattleTaskManager.GetBattleShips().dicFriendBattleShips.get_Item(0);
			Vector3 position = BattleTaskManager.GetBattleField().dicFleetAnchor.get_Item(FleetType.Friend).get_position();
			position.y = uIBattleShip.pointOfGaze.y;
			ShipUtils.PlayBattleStartVoice(uIBattleShip.shipModel);
			cam.ReqViewMode(CameraActor.ViewMode.RotateAroundObject);
			cam.SetRotateAroundObjectCamera(position, BattleDefines.FLEET_ADVENT_START_CAM_POS.get_Item(0), -10f);
			List<float> rotDst = this.CalcCloseUpCamDist(cam.rotateDistance, 30f);
			cam.get_transform().LTValue(cam.rotateDistance, rotDst.get_Item(0), 1f).setEase(BattleDefines.FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE).setOnUpdate(delegate(float x)
			{
				cam.rotateDistance = x;
			}).setOnComplete(delegate
			{
				cam.get_transform().LTValue(cam.rotateDistance, rotDst.get_Item(1), 1f).setEase(BattleDefines.FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE).setOnUpdate(delegate(float x)
				{
					cam.rotateDistance = x;
				}).setOnComplete(delegate
				{
					this.EndPhase(BattleUtils.NextPhase(BattlePhase.FleetAdvent));
				});
			});
			return false;
		}

		private bool UpdateFriendFleetAdvent(object data)
		{
			return true;
		}

		private List<float> CalcCloseUpCamDist(float from, float to)
		{
			List<float> list = new List<float>();
			list.Add(Mathe.Lerp(from, to, 0.95f));
			list.Add(to);
			return list;
		}
	}
}
