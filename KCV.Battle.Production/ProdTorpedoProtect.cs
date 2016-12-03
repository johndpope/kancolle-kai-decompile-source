using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoProtect : MonoBehaviour
	{
		public enum RescueShipType
		{
			Defender,
			Protector
		}

		public List<UIBattleShip> _listBattleShipF;

		public List<UIBattleShip> _listBattleShipE;

		private bool[] _isProtect;

		private List<Vector3> _camTargetPosF;

		private List<Vector3> _camTargetPosE;

		private Action _actCallback;

		public Vector3 calcDefenderCamStartPosF
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
				return new Vector3(this._listBattleShipF.get_Item(0).spPointOfGaze.x, this._listBattleShipF.get_Item(0).spPointOfGaze.y, 0f);
			}
		}

		public Vector3 calcDefenderCamStartPosE
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
				return new Vector3(this._listBattleShipE.get_Item(0).spPointOfGaze.x, this._listBattleShipE.get_Item(0).spPointOfGaze.y, 0f);
			}
		}

		public void _init()
		{
			this._isProtect = new bool[2];
			this._listBattleShipF = new List<UIBattleShip>();
			this._listBattleShipE = new List<UIBattleShip>();
		}

		private void OnDestroy()
		{
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<bool[]>(ref this._isProtect);
			Mem.DelListSafe<UIBattleShip>(ref this._listBattleShipF);
			Mem.DelListSafe<UIBattleShip>(ref this._listBattleShipE);
			Mem.DelListSafe<Vector3>(ref this._camTargetPosF);
			Mem.DelListSafe<Vector3>(ref this._camTargetPosE);
		}

		public void initShipList(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				if (this._listBattleShipF != null)
				{
					this._listBattleShipF.Clear();
					this._listBattleShipF = new List<UIBattleShip>();
					this._isProtect[0] = false;
				}
			}
			else if (this._listBattleShipE != null)
			{
				this._listBattleShipE.Clear();
				this._listBattleShipE = new List<UIBattleShip>();
				this._isProtect[1] = false;
			}
		}

		public void AddShipList(UIBattleShip defenderShip, UIBattleShip protecterShip, FleetType type)
		{
			this.initShipList(type);
			if (type == FleetType.Friend)
			{
				this._isProtect[0] = true;
				this._listBattleShipF.Add(defenderShip);
				this._listBattleShipF.Add(protecterShip);
				this._listBattleShipF.get_Item(0).standingPositionType = StandingPositionType.Advance;
			}
			else
			{
				this._isProtect[1] = true;
				this._listBattleShipE.Add(defenderShip);
				this._listBattleShipE.Add(protecterShip);
				this._listBattleShipE.get_Item(0).standingPositionType = StandingPositionType.Advance;
			}
		}

		public void setFieldCamera(BattleFieldCamera camF, BattleFieldCamera camE)
		{
			if (this._isProtect[0])
			{
				Vector3 calcDefenderCamStartPosF = this.calcDefenderCamStartPosF;
				camF.motionBlur.set_enabled(true);
				camF.LookAt(this._listBattleShipF.get_Item(0).spPointOfGaze);
				camF.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			}
			if (this._isProtect[1])
			{
				Vector3 calcDefenderCamStartPosE = this.calcDefenderCamStartPosE;
				camE.motionBlur.set_enabled(true);
				camE.LookAt(this._listBattleShipE.get_Item(0).spPointOfGaze);
				camE.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			}
		}

		public void Play(Action callBack)
		{
			this._actCallback = callBack;
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCamF = battleCameras.friendFieldCamera;
			BattleFieldCamera fieldCamE = battleCameras.enemyFieldCamera;
			this.setFieldCamera(fieldCamF, fieldCamE);
			if (this._isProtect[0])
			{
				this._camTargetPosF = this.calcCloseUpCamPos(fieldCamF.get_transform().get_position(), FleetType.Friend);
				fieldCamF.get_transform().MoveTo(this._camTargetPosF.get_Item(0), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0), iTween.EaseType.linear, delegate
				{
					fieldCamF.motionBlur.set_enabled(false);
					this.PlayProtectDefender(fieldCamF, FleetType.Friend);
				});
			}
			if (this._isProtect[1])
			{
				this._camTargetPosE = this.calcCloseUpCamPos(fieldCamE.get_transform().get_position(), FleetType.Enemy);
				fieldCamE.get_transform().MoveTo(this._camTargetPosE.get_Item(0), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0), iTween.EaseType.linear, delegate
				{
					fieldCamE.motionBlur.set_enabled(false);
					this.PlayProtectDefender(fieldCamE, FleetType.Enemy);
				});
			}
		}

		public void PlayProtectDefender(BattleFieldCamera cam, FleetType type)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			if (type == FleetType.Friend)
			{
				cam.get_transform().MoveTo(this._camTargetPosF.get_Item(1), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1), iTween.EaseType.linear, null);
				Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate(long _)
				{
					cam.get_transform().iTweenStop();
					Vector3 target = this.calcProtecterPos(this._camTargetPosF.get_Item(3), FleetType.Friend);
					this._listBattleShipF.get_Item(1).get_transform().positionZ(target.z);
					this._listBattleShipF.get_Item(1).get_transform().MoveTo(target, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0) * 1.2f, iTween.EaseType.easeOutSine, null);
					cam.get_transform().MoveTo(this._camTargetPosF.get_Item(2), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0), iTween.EaseType.linear, delegate
					{
						cam.get_transform().MoveTo(this._camTargetPosF.get_Item(3), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1), iTween.EaseType.linear, delegate
						{
							this._listBattleShipF.ForEach(delegate(UIBattleShip x)
							{
								x.standingPositionType = StandingPositionType.OneRow;
							});
							this._actCallback.Invoke();
						});
					});
				});
			}
			else
			{
				cam.get_transform().MoveTo(this._camTargetPosE.get_Item(1), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1), iTween.EaseType.linear, null);
				Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate(long _)
				{
					cam.get_transform().iTweenStop();
					Vector3 target = this.calcProtecterPos(this._camTargetPosE.get_Item(3), FleetType.Enemy);
					this._listBattleShipE.get_Item(1).get_transform().positionZ(target.z);
					this._listBattleShipE.get_Item(1).get_transform().MoveTo(target, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0) * 1.2f, iTween.EaseType.easeOutSine, null);
					cam.get_transform().MoveTo(this._camTargetPosE.get_Item(2), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0), iTween.EaseType.linear, delegate
					{
						cam.get_transform().MoveTo(this._camTargetPosE.get_Item(3), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1), iTween.EaseType.linear, delegate
						{
							this._listBattleShipE.ForEach(delegate(UIBattleShip x)
							{
								x.standingPositionType = StandingPositionType.OneRow;
							});
							this._actCallback.Invoke();
						});
					});
				});
			}
		}

		protected void setProtecterLayer()
		{
			if (this._isProtect[0])
			{
				this._listBattleShipF.get_Item(1).layer = Generics.Layers.FocusDim;
			}
			if (this._isProtect[1])
			{
				this._listBattleShipE.get_Item(1).layer = Generics.Layers.FocusDim;
			}
		}

		public Vector3 calcCamTargetPos(bool isPointOfGaze, FleetType type)
		{
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			BattleFieldCamera friendFieldCamera2 = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			if (type == FleetType.Friend)
			{
				Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? this._listBattleShipF.get_Item(0).spPointOfGaze : this._listBattleShipF.get_Item(0).pointOfGaze, friendFieldCamera2.eyePosition) * 10f;
				return (!isPointOfGaze) ? new Vector3(this._listBattleShipF.get_Item(0).spPointOfGaze.x + vector.x, this._listBattleShipF.get_Item(0).spPointOfGaze.y, this._listBattleShipF.get_Item(0).spPointOfGaze.z + vector.z) : new Vector3(this._listBattleShipF.get_Item(0).pointOfGaze.x + vector.x, this._listBattleShipF.get_Item(0).pointOfGaze.y, this._listBattleShipF.get_Item(0).pointOfGaze.z + vector.z);
			}
			Vector3 vector2 = Mathe.NormalizeDirection((!isPointOfGaze) ? this._listBattleShipE.get_Item(0).spPointOfGaze : this._listBattleShipE.get_Item(0).pointOfGaze, friendFieldCamera.eyePosition) * 10f;
			return (!isPointOfGaze) ? new Vector3(this._listBattleShipE.get_Item(0).spPointOfGaze.x + vector2.x, this._listBattleShipE.get_Item(0).spPointOfGaze.y, this._listBattleShipE.get_Item(0).spPointOfGaze.z + vector2.z) : new Vector3(this._listBattleShipE.get_Item(0).pointOfGaze.x + vector2.x, this._listBattleShipE.get_Item(0).pointOfGaze.y, this._listBattleShipE.get_Item(0).pointOfGaze.z + vector2.z);
		}

		protected virtual Vector3 calcProtecterPos(Vector3 close4, FleetType type)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			if (type == FleetType.Friend)
			{
				Vector3 vector = Vector3.Lerp(this._listBattleShipF.get_Item(0).spPointOfGaze, close4, 0.58f);
				float num = this._listBattleShipF.get_Item(0).get_transform().get_position().x - this._listBattleShipF.get_Item(0).spPointOfGaze.x - (this._listBattleShipF.get_Item(1).get_transform().get_position().x - this._listBattleShipF.get_Item(1).spPointOfGaze.x);
				Vector3 position = this._listBattleShipF.get_Item(0).get_transform().get_position();
				position.y = battleField.seaLevelPos.y;
				position.z = vector.z + 1f;
				return position;
			}
			Vector3 vector2 = Vector3.Lerp(this._listBattleShipE.get_Item(0).spPointOfGaze, close4, 0.58f);
			float num2 = this._listBattleShipE.get_Item(0).get_transform().get_position().x - this._listBattleShipE.get_Item(0).spPointOfGaze.x - (this._listBattleShipE.get_Item(1).get_transform().get_position().x - this._listBattleShipE.get_Item(1).spPointOfGaze.x);
			Vector3 position2 = this._listBattleShipE.get_Item(0).get_transform().get_position();
			position2.y = battleField.seaLevelPos.y;
			position2.z = vector2.z - 1f;
			return position2;
		}

		public List<Vector3> calcCloseUpCamPos(Vector3 from, FleetType type)
		{
			Vector3 vector = this.calcCamTargetPos(false, type);
			List<Vector3> list;
			List<Vector3> list2;
			if (type == FleetType.Friend)
			{
				Vector3 vector2 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(0));
				Vector3 vector3 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(1));
				vector2.y = this._listBattleShipF.get_Item(1).spPointOfGaze.y;
				vector3.y = this._listBattleShipF.get_Item(1).spPointOfGaze.y;
				float num = this._listBattleShipF.get_Item(0).get_transform().get_position().x - this._listBattleShipF.get_Item(0).spPointOfGaze.x - (this._listBattleShipF.get_Item(1).get_transform().get_position().x - this._listBattleShipF.get_Item(1).spPointOfGaze.x);
				vector2.x += num;
				vector3.x += num;
				list = new List<Vector3>();
				list.Add(Vector3.Lerp(from, vector, 0.98f));
				list.Add(vector);
				list.Add(vector2);
				list.Add(vector3);
				list2 = list;
				for (int i = 0; i < list2.get_Count(); i++)
				{
					list2.set_Item(i, new Vector3(list2.get_Item(i).x - 2f, list2.get_Item(i).y, list2.get_Item(i).z - 8f));
				}
				return list2;
			}
			Vector3 vector4 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(0));
			Vector3 vector5 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(1));
			vector4.y = this._listBattleShipE.get_Item(1).spPointOfGaze.y;
			vector5.y = this._listBattleShipE.get_Item(1).spPointOfGaze.y;
			float num2 = this._listBattleShipE.get_Item(0).get_transform().get_position().x - this._listBattleShipE.get_Item(0).spPointOfGaze.x - (this._listBattleShipE.get_Item(1).get_transform().get_position().x - this._listBattleShipE.get_Item(1).spPointOfGaze.x);
			vector4.x += num2;
			vector5.x += num2;
			list = new List<Vector3>();
			list.Add(Vector3.Lerp(from, vector, 0.98f));
			list.Add(vector);
			list.Add(vector4);
			list.Add(vector5);
			list2 = list;
			for (int j = 0; j < list2.get_Count(); j++)
			{
				list2.set_Item(j, new Vector3(list2.get_Item(j).x, list2.get_Item(j).y, list2.get_Item(j).z + 8f));
			}
			return list2;
		}

		public static ProdTorpedoProtect Instantiate(ProdTorpedoProtect prefab, Transform parent)
		{
			ProdTorpedoProtect prodTorpedoProtect = Object.Instantiate<ProdTorpedoProtect>(prefab);
			prodTorpedoProtect.get_transform().set_parent(parent);
			prodTorpedoProtect.get_transform().set_localPosition(Vector3.get_zero());
			prodTorpedoProtect.get_transform().set_localScale(Vector3.get_one());
			prodTorpedoProtect._init();
			return prodTorpedoProtect;
		}
	}
}
