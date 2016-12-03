using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBattleCommandBuffer : InstantiateObject<ProdBattleCommandBuffer>
	{
		[SerializeField]
		private Transform _prefabUIBufferFleetCircle;

		[SerializeField]
		private Transform _prefabUIBufferShipCircle;

		[SerializeField]
		private BattleCommand _iCommand = BattleCommand.None;

		[Range(0f, 4f), SerializeField]
		private int _nBufferCnt;

		[Button("LoadPrefab", "コマンドプレハブ読み込み", new object[]
		{

		}), SerializeField]
		private int _nLoadPrefab;

		private bool _isBufferObjectDeployment;

		private EffectModel _clsEffectModel;

		private Action _actOnFinished;

		public static ProdBattleCommandBuffer Instantiate(ProdBattleCommandBuffer prefab, Transform parent, EffectModel buffer, int nBufferCnt)
		{
			ProdBattleCommandBuffer prodBattleCommandBuffer = InstantiateObject<ProdBattleCommandBuffer>.Instantiate(prefab, parent);
			prodBattleCommandBuffer.Init(buffer, nBufferCnt);
			return prodBattleCommandBuffer;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabUIBufferFleetCircle);
			Mem.Del<Transform>(ref this._prefabUIBufferShipCircle);
			Mem.Del<BattleCommand>(ref this._iCommand);
			Mem.Del<int>(ref this._nLoadPrefab);
			Mem.Del<bool>(ref this._isBufferObjectDeployment);
			Mem.Del<EffectModel>(ref this._clsEffectModel);
			Mem.Del<Action>(ref this._actOnFinished);
		}

		private bool Init(EffectModel model, int nBufferCnt)
		{
			this._nBufferCnt = nBufferCnt;
			ProdBufferEffect prodBufferEffect = BattleTaskManager.GetPrefabFile().prodBufferEffect;
			prodBufferEffect.Init();
			prodBufferEffect.SetEffectData(model);
			this._clsEffectModel = model;
			this.BufferObjectDeployment();
			return true;
		}

		public void Play(Action onFinished)
		{
			this._actOnFinished = onFinished;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetStandingPosition(StandingPositionType.CommandBuffer);
			this.PlayCommand(this._clsEffectModel.Command);
		}

		private void PlayCommand(BattleCommand iCommand)
		{
			switch (iCommand + 1)
			{
			case BattleCommand.Sekkin:
				this.OnFinished(null);
				break;
			case BattleCommand.Hougeki:
			case BattleCommand.Ridatu:
			{
				ProdBufferClose pbc = (!base.get_transform().GetComponentInChildren<ProdBufferClose>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Sekkin), base.get_gameObject(), false, false).GetComponent<ProdBufferClose>() : base.get_transform().GetComponentInChildren<ProdBufferClose>();
				pbc.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pbc.get_gameObject());
				});
				break;
			}
			case BattleCommand.Raigeki:
			{
				ProdBufferShelling pbh = (!base.get_transform().GetComponentInChildren<ProdBufferShelling>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Hougeki), base.get_gameObject(), false, false).GetComponent<ProdBufferShelling>() : base.get_transform().GetComponentInChildren<ProdBufferShelling>();
				pbh.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pbh.get_gameObject());
				});
				break;
			}
			case BattleCommand.Taisen:
			case BattleCommand.Kouku:
			{
				ProdBufferAvoidance pbad = (!base.get_transform().GetComponentInChildren<ProdBufferAvoidance>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Kaihi), base.get_gameObject(), false, false).GetComponent<ProdBufferAvoidance>() : base.get_transform().GetComponentInChildren<ProdBufferAvoidance>();
				pbad.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pbad.get_gameObject());
				});
				break;
			}
			case BattleCommand.Kaihi:
			{
				ProdBufferAntiSubmarine pbas = (!base.get_transform().GetComponentInChildren<ProdBufferAntiSubmarine>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Taisen), base.get_gameObject(), false, false).GetComponent<ProdBufferAntiSubmarine>() : base.get_transform().GetComponentInChildren<ProdBufferAntiSubmarine>();
				pbas.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pbas.get_gameObject());
				});
				break;
			}
			case BattleCommand.Totugeki:
			{
				ProdBufferAviation pba = (!base.get_transform().GetComponentInChildren<ProdBufferAviation>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Kouku), base.get_gameObject(), false, false).GetComponent<ProdBufferAviation>() : base.get_transform().GetComponentInChildren<ProdBufferAviation>();
				pba.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pba.get_gameObject());
				});
				break;
			}
			case BattleCommand.Tousha:
			{
				ProdBufferAssault pba = (!base.get_transform().GetComponentInChildren<ProdBufferAssault>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Totugeki), base.get_gameObject(), false, false).GetComponent<ProdBufferAssault>() : base.get_transform().GetComponentInChildren<ProdBufferAssault>();
				pba.Init(new Action(this.PlayLookAtLineAssult));
				pba.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pba.get_gameObject());
				});
				break;
			}
			case (BattleCommand)9:
			{
				ProdBufferUnifiedFire pbuf = (!base.get_transform().GetComponentInChildren<ProdBufferUnifiedFire>()) ? Util.Instantiate(this.LoadBufferPrefab(BattleCommand.Tousha), base.get_gameObject(), false, false).GetComponent<ProdBufferUnifiedFire>() : base.get_transform().GetComponentInChildren<ProdBufferUnifiedFire>();
				pbuf.Init(new Action(this.PlayLookAtLine));
				pbuf.Play(new Action(this.PlayBufferEffect), new Action(this.CalcInitLineRotation), new Action(this.PlayLineAnimation), new Action(this.PlayNextFocusShipCircleAnimation), this._nBufferCnt).Subscribe(delegate(bool _)
				{
					this.OnFinished(pbuf.get_gameObject());
				});
				break;
			}
			}
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			BattleTaskManager.GetTorpedoHpGauges().Hide();
		}

		private void CalcInitLineRotation()
		{
			BattleField field = BattleTaskManager.GetBattleField();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor.get_Item(FleetType.Enemy));
			});
			battleShips.bufferShipCirlce.get_Item(1).ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor.get_Item(FleetType.Friend));
			});
		}

		private void PlayLineAnimation()
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
			battleShips.bufferShipCirlce.get_Item(1).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
		}

		private void PlayLookAtLine()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_056);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferFleetCircle.get_Item(0).PlayRipple();
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLookAtLine();
			});
		}

		private void PlayLookAtLineAssult()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_056);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLookAtLine2Assult();
			});
		}

		private void PlayBufferEffect()
		{
			ProdBufferEffect prodBufferEffect = BattleTaskManager.GetPrefabFile().prodBufferEffect;
			prodBufferEffect.Play(null);
		}

		private void PlayNextFocusShipCircleAnimation()
		{
			if (this._clsEffectModel.NextActionShip == null)
			{
				return;
			}
			ShipModel_Battle focusShip = this._clsEffectModel.NextActionShip;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			int cnt = 0;
			battleShips.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayFocusCircleAnimation(focusShip.IsFriend() && focusShip.Index == cnt);
				cnt++;
			});
			cnt = 0;
			battleShips.bufferShipCirlce.get_Item(1).ForEach(delegate(UIBufferCircle x)
			{
				x.PlayFocusCircleAnimation(!focusShip.IsFriend() && focusShip.Index == cnt);
				cnt++;
			});
			List<UIBufferFleetCircle> bufferFleetCircle = battleShips.bufferFleetCircle;
			bufferFleetCircle.ForEach(delegate(UIBufferFleetCircle x)
			{
				x.PlayFocusCircleAnimation();
			});
		}

		private GameObject LoadBufferPrefab(BattleCommand Command)
		{
			string text = string.Empty;
			switch (Command + 1)
			{
			case BattleCommand.Sekkin:
				return null;
			case BattleCommand.Hougeki:
				text = "Close";
				break;
			case BattleCommand.Raigeki:
				text = "Shelling";
				break;
			case BattleCommand.Ridatu:
				text = "TorpedoSalvo";
				break;
			case BattleCommand.Taisen:
				text = "Withdrawal";
				break;
			case BattleCommand.Kaihi:
				text = "AntiSubmarine";
				break;
			case BattleCommand.Kouku:
				text = "Avoidance";
				break;
			case BattleCommand.Totugeki:
				text = "Aviation";
				break;
			case BattleCommand.Tousha:
				text = "Assault";
				break;
			case (BattleCommand)9:
				text = "UnifiedFire";
				break;
			}
			return Resources.Load(string.Format("Prefabs/Battle/Production/Command/ProdBuffer{0}", text)) as GameObject;
		}

		private void BufferObjectDeployment()
		{
			if (this._isBufferObjectDeployment)
			{
				return;
			}
			this._isBufferObjectDeployment = true;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(true);
		}

		public void BufferObjectConvergence()
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(false);
		}

		private void OnFinished(GameObject destroyObject)
		{
			if (destroyObject != null)
			{
				Object.Destroy(destroyObject);
			}
			this._isBufferObjectDeployment = false;
			Dlg.Call(ref this._actOnFinished);
		}
	}
}
