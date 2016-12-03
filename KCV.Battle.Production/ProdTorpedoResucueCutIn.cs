using Common.Enum;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoResucueCutIn : MonoBehaviour
	{
		private enum ResucueAnimationList
		{
			None,
			RescueCutIn1,
			RescueCutInFriend,
			RescueCutInEnemy
		}

		private Dictionary<FleetType, bool> _dicIsCutIn;

		private Dictionary<FleetType, UITexture> _listFlagShipTex;

		[SerializeField]
		private List<UITexture> _listShipFriend;

		[SerializeField]
		private List<UITexture> _listShipEnemy;

		[SerializeField]
		private List<ShipModel_Defender> _listShipDefenderF;

		[SerializeField]
		private List<ShipModel_Defender> _listShipDefenderE;

		private Dictionary<FleetType, UITexture> _dicOverlay;

		[SerializeField]
		private Animation _anime;

		private Action _actCallback;

		private ProdTorpedoResucueCutIn.ResucueAnimationList _iList;

		private int _count;

		private int _countF;

		private int _countE;

		private bool _isShake;

		private float _fPower = 0.1f;

		private Vector3 _vBasePosition;

		private Quaternion _qBaseRotation;

		private float _fShakeDecay = 0.002f;

		private void Awake()
		{
			this._init();
		}

		public void _init()
		{
			this._iList = ProdTorpedoResucueCutIn.ResucueAnimationList.None;
			this._dicIsCutIn = new Dictionary<FleetType, bool>();
			this._dicIsCutIn.Add(FleetType.Friend, false);
			this._dicIsCutIn.Add(FleetType.Enemy, false);
			int num = 5;
			this._listShipFriend = new List<UITexture>();
			this._listShipFriend.set_Capacity(num);
			Transform transform = base.get_transform().FindChild("FriendShip");
			for (int i = 0; i < num; i++)
			{
				this._listShipFriend.Add(transform.FindChild("Anchor" + (i + 1) + "/Ship").GetComponent<UITexture>());
			}
			this._listShipEnemy = new List<UITexture>();
			Transform transform2 = base.get_transform().FindChild("EnemyShip");
			for (int j = 0; j < num; j++)
			{
				this._listShipEnemy.Add(transform2.FindChild("Anchor" + (j + 1) + "/Ship").GetComponent<UITexture>());
			}
			this._listShipDefenderF = new List<ShipModel_Defender>();
			this._listShipDefenderE = new List<ShipModel_Defender>();
			if (this._listFlagShipTex == null)
			{
				this._listFlagShipTex = new Dictionary<FleetType, UITexture>();
				this._listFlagShipTex.Add(FleetType.Friend, base.get_transform().FindChild("FriendShip/AnchorFlag/Ship").GetComponent<UITexture>());
				this._listFlagShipTex.Add(FleetType.Enemy, base.get_transform().FindChild("EnemyShip/AnchorFlag/Ship").GetComponent<UITexture>());
			}
			if (this._dicOverlay == null)
			{
				this._dicOverlay = new Dictionary<FleetType, UITexture>();
				this._dicOverlay.Add(FleetType.Friend, base.get_transform().FindChild("FriendShip/Overlay").GetComponent<UITexture>());
				this._dicOverlay.Add(FleetType.Enemy, base.get_transform().FindChild("EnemyShip/Overlay").GetComponent<UITexture>());
			}
			if (this._anime == null)
			{
				this._anime = base.get_transform().GetComponent<Animation>();
			}
		}

		private void OnDestroy()
		{
			if (this._listFlagShipTex != null)
			{
				this._listFlagShipTex.Clear();
			}
			this._listFlagShipTex = null;
			this._actCallback = null;
		}

		private void Update()
		{
			if (this._isShake)
			{
			}
		}

		public void SetFlagShipInfo(FleetType type, ShipModel_BattleAll flagShip)
		{
			bool flag = false;
			if (flagShip.DmgStateEnd == DamageState_Battle.Tyuuha || flagShip.DmgStateEnd == DamageState_Battle.Taiha || flagShip.DmgStateEnd == DamageState_Battle.Gekichin)
			{
				flag = true;
			}
			this._listFlagShipTex.get_Item(type).mainTexture = ShipUtils.LoadTexture(flagShip, flag);
			this._listFlagShipTex.get_Item(type).MakePixelPerfect();
			this._listFlagShipTex.get_Item(type).get_transform().set_localPosition(ShipUtils.GetShipOffsPos(flagShip, flag, MstShipGraphColumn.CutIn));
			this._dicIsCutIn.set_Item(type, true);
		}

		public void AddShipInfo(FleetType type, ShipModel_Defender ship)
		{
			bool isAfter = false;
			if (ship.DmgStateBefore == DamageState_Battle.Tyuuha || ship.DmgStateBefore == DamageState_Battle.Taiha || ship.DmgStateBefore == DamageState_Battle.Gekichin)
			{
				isAfter = true;
			}
			if (type == FleetType.Friend)
			{
				this._listShipDefenderF.Add(ship);
				this._countF = this._listShipDefenderF.get_Count();
				this._listShipFriend.get_Item(this._countF - 1).mainTexture = ShipUtils.LoadTexture(ship, isAfter);
				this._listShipFriend.get_Item(this._countF - 1).MakePixelPerfect();
				this._listShipFriend.get_Item(this._countF - 1).get_transform().set_localPosition(ShipUtils.GetShipOffsPos(ship, ship.DmgStateBefore, MstShipGraphColumn.CutIn));
			}
			else
			{
				this._listShipDefenderE.Add(ship);
				this._countE = this._listShipDefenderE.get_Count();
				this._listShipEnemy.get_Item(this._countE - 1).mainTexture = ShipUtils.LoadTexture(ship, isAfter);
				this._listShipEnemy.get_Item(this._countE - 1).MakePixelPerfect();
				this._listShipEnemy.get_Item(this._countE - 1).get_transform().set_localPosition(ShipUtils.GetShipOffsPos(ship, ship.DmgStateBefore, MstShipGraphColumn.CutIn));
			}
			this._dicIsCutIn.set_Item(type, true);
		}

		public void Play(Action callback)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			this._actCallback = callback;
			if (this._countF >= this._countE)
			{
				this._count = this._countF;
			}
			else
			{
				this._count = this._countE;
			}
			if (this._count == 0)
			{
				this._cutinFinished();
				return;
			}
			if (this._countF > 0)
			{
				this._dicOverlay.get_Item(FleetType.Friend).SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
				this._listFlagShipTex.get_Item(FleetType.Friend).SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
			}
			else
			{
				this._listFlagShipTex.get_Item(FleetType.Friend).mainTexture = null;
			}
			if (this._countE > 0)
			{
				this._dicOverlay.get_Item(FleetType.Enemy).SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
				this._listFlagShipTex.get_Item(FleetType.Enemy).SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
			}
			else
			{
				this._listFlagShipTex.get_Item(FleetType.Enemy).mainTexture = null;
			}
			this._anime.Play(ProdTorpedoResucueCutIn.ResucueAnimationList.RescueCutIn1.ToString());
		}

		private void _onAnimetionFinished(int index)
		{
			if (this._count == index)
			{
				if (this._countF > 0)
				{
					this._dicOverlay.get_Item(FleetType.Friend).SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
					this._listFlagShipTex.get_Item(FleetType.Friend).SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
				}
				if (this._countE > 0)
				{
					this._dicOverlay.get_Item(FleetType.Enemy).SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
					this._listFlagShipTex.get_Item(FleetType.Enemy).SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
				}
				this._cutinFinished();
			}
		}

		private void onPlaySeAnime(int seNo)
		{
			if (seNo != 0)
			{
				if (seNo == 1)
				{
				}
			}
		}

		private void startSplashParticle()
		{
			this._isShake = true;
			this._vBasePosition = base.get_transform().get_localPosition();
			this._qBaseRotation = base.get_transform().get_localRotation();
		}

		private void _cutinFinished()
		{
			this._actCallback.Invoke();
		}

		public static ProdTorpedoResucueCutIn Instantiate(ProdTorpedoResucueCutIn prefab, RaigekiModel model, Transform parent)
		{
			ProdTorpedoResucueCutIn prodTorpedoResucueCutIn = Object.Instantiate<ProdTorpedoResucueCutIn>(prefab);
			prodTorpedoResucueCutIn.get_transform().set_parent(parent);
			prodTorpedoResucueCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodTorpedoResucueCutIn.get_transform().set_localScale(Vector3.get_one());
			return prodTorpedoResucueCutIn;
		}
	}
}
