using KCV.Battle.Utils;
using Librarys.Cameras;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialRescueCutIn : MonoBehaviour
	{
		public enum RescueShipType
		{
			Defender,
			Protector
		}

		public List<UIBattleShip> _listBattleShip;

		private List<Vector3> _camTargetPos;

		private Action _actCallback;

		public Vector3 calcDefenderCamStartPos
		{
			get
			{
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				BattleFieldCamera friendFieldCamera = battleCameras.friendFieldCamera;
				return new Vector3(this._listBattleShip.get_Item(0).spPointOfGaze.x, this._listBattleShip.get_Item(0).spPointOfGaze.y, 0f);
			}
		}

		public void _init()
		{
			this._listBattleShip = new List<UIBattleShip>();
		}

		private void OnDestroy()
		{
			Mem.Del<Action>(ref this._actCallback);
			Mem.DelListSafe<UIBattleShip>(ref this._listBattleShip);
			Mem.DelListSafe<Vector3>(ref this._camTargetPos);
		}

		public void initShipList()
		{
			if (this._listBattleShip != null)
			{
				this._listBattleShip.Clear();
			}
			this._listBattleShip = null;
		}

		public void AddShipList(UIBattleShip defenderShip, UIBattleShip protecterShip)
		{
			if (this._listBattleShip != null)
			{
				this.initShipList();
				this._init();
			}
			this._listBattleShip.Add(defenderShip);
			this._listBattleShip.Add(protecterShip);
			this._listBattleShip.get_Item(0).standingPositionType = StandingPositionType.Advance;
		}

		public void setFieldCamera()
		{
			Vector3 calcDefenderCamStartPos = this.calcDefenderCamStartPos;
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			friendFieldCamera.motionBlur.set_enabled(true);
			friendFieldCamera.LookAt(this._listBattleShip.get_Item(0).spPointOfGaze);
			friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
		}

		public void Play(Action callBack)
		{
			this._actCallback = callBack;
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.friendFieldCamera;
			this.setFieldCamera();
			this._camTargetPos = this.calcCloseUpCamPos(fieldCam.get_transform().get_position());
			fieldCam.get_transform().MoveTo(this._camTargetPos.get_Item(0), 0.6f, iTween.EaseType.linear, delegate
			{
				fieldCam.motionBlur.set_enabled(false);
				this.PlayProtectDefender();
			});
		}

		public void PlayProtectDefender()
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera fieldCam = battleCameras.friendFieldCamera;
			fieldCam.get_transform().MoveTo(this._camTargetPos.get_Item(1), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1), iTween.EaseType.linear, null);
			Observable.Timer(TimeSpan.FromSeconds(0.42500001192092896)).Subscribe(delegate(long _)
			{
				fieldCam.get_transform().iTweenStop();
				Vector3 target = this.calcProtecterPos(this._camTargetPos.get_Item(3));
				this._listBattleShip.get_Item(1).get_transform().positionZ(target.z);
				this._listBattleShip.get_Item(1).get_transform().MoveTo(target, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0) * 1.2f, iTween.EaseType.easeOutSine, null);
				fieldCam.get_transform().MoveTo(this._camTargetPos.get_Item(2), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(0), iTween.EaseType.linear, delegate
				{
					fieldCam.get_transform().MoveTo(this._camTargetPos.get_Item(3), BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(1), iTween.EaseType.linear, delegate
					{
						this._listBattleShip.get_Item(0).standingPositionType = StandingPositionType.OneRow;
						this._listBattleShip.get_Item(1).standingPositionType = StandingPositionType.OneRow;
						this._actCallback.Invoke();
					});
				});
			});
		}

		protected void setProtecterLayer()
		{
			this._listBattleShip.get_Item(1).layer = Generics.Layers.FocusDim;
		}

		public Vector3 calcCamTargetPos(bool isPointOfGaze)
		{
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			Vector3 vector = Mathe.NormalizeDirection((!isPointOfGaze) ? this._listBattleShip.get_Item(0).spPointOfGaze : this._listBattleShip.get_Item(0).pointOfGaze, friendFieldCamera.eyePosition) * 10f;
			return (!isPointOfGaze) ? new Vector3(this._listBattleShip.get_Item(0).spPointOfGaze.x + vector.x, this._listBattleShip.get_Item(0).spPointOfGaze.y, this._listBattleShip.get_Item(0).spPointOfGaze.z + vector.z) : new Vector3(this._listBattleShip.get_Item(0).pointOfGaze.x + vector.x, this._listBattleShip.get_Item(0).pointOfGaze.y, this._listBattleShip.get_Item(0).pointOfGaze.z + vector.z);
		}

		protected virtual Vector3 calcProtecterPos(Vector3 close4)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			Vector3 vector = Vector3.Lerp(this._listBattleShip.get_Item(0).spPointOfGaze, close4, 0.58f);
			float num = this._listBattleShip.get_Item(0).get_transform().get_position().x - this._listBattleShip.get_Item(0).spPointOfGaze.x - (this._listBattleShip.get_Item(1).get_transform().get_position().x - this._listBattleShip.get_Item(1).spPointOfGaze.x);
			Vector3 position = this._listBattleShip.get_Item(0).get_transform().get_position();
			position.y = battleField.seaLevelPos.y;
			position.z = vector.z;
			return position;
		}

		public List<Vector3> calcCloseUpCamPos(Vector3 from)
		{
			Vector3 vector = this.calcCamTargetPos(false);
			Vector3 vector2 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(0));
			Vector3 vector3 = Vector3.Lerp(from, vector, BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE.get_Item(1));
			vector2.y = this._listBattleShip.get_Item(1).spPointOfGaze.y;
			vector3.y = this._listBattleShip.get_Item(1).spPointOfGaze.y;
			float num = this._listBattleShip.get_Item(0).get_transform().get_position().x - this._listBattleShip.get_Item(0).spPointOfGaze.x - (this._listBattleShip.get_Item(1).get_transform().get_position().x - this._listBattleShip.get_Item(1).spPointOfGaze.x);
			vector2.x += num;
			vector3.x += num;
			List<Vector3> list = new List<Vector3>();
			list.Add(Vector3.Lerp(from, vector, 0.98f));
			list.Add(vector);
			list.Add(vector2);
			list.Add(vector3);
			return list;
		}

		public static ProdAerialRescueCutIn Instantiate(ProdAerialRescueCutIn prefab, Transform parent)
		{
			ProdAerialRescueCutIn prodAerialRescueCutIn = Object.Instantiate<ProdAerialRescueCutIn>(prefab);
			prodAerialRescueCutIn.get_transform().set_parent(parent);
			prodAerialRescueCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodAerialRescueCutIn.get_transform().set_localScale(Vector3.get_one());
			prodAerialRescueCutIn._init();
			return prodAerialRescueCutIn;
		}
	}
}
