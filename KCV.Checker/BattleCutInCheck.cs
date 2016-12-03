using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Checker
{
	public class BattleCutInCheck : MonoBehaviour
	{
		public ProdTranscendenceCutIn prodTranscendenceCutIn;

		public int mstId = 1;

		private ShipModelMst _clsCurrentShipMst;

		public bool isDamaged;

		public BattleAttackKind battleAttackKind;

		public Vector3 offset = Vector3.get_zero();

		[Button("SwitchIsDamaged", "SwitchIsDamaged", new object[]
		{

		})]
		public int Damaged;

		private Dictionary<int, Mst_ship> _dicMstShip;

		private int MIN_SHIP_ID
		{
			get
			{
				return Enumerable.First<KeyValuePair<int, Mst_ship>>(Enumerable.OrderBy<KeyValuePair<int, Mst_ship>, int>(Mst_DataManager.Instance.Mst_ship, (KeyValuePair<int, Mst_ship> order) => order.get_Value().Id)).get_Value().Id;
			}
		}

		private int MAX_SHIP_ID
		{
			get
			{
				return Enumerable.Max<KeyValuePair<int, Mst_ship>>(Mst_DataManager.Instance.Mst_ship, (KeyValuePair<int, Mst_ship> order) => order.get_Value().Id);
			}
		}

		private void Start()
		{
			this._dicMstShip = Mst_DataManager.Instance.Mst_ship;
			this.mstId = this.MIN_SHIP_ID;
			this._clsCurrentShipMst = new ShipModelMst(this.mstId);
		}

		private void Update()
		{
			if (Input.GetKeyDown(13))
			{
				this.PlayTranscendenceCutIn();
			}
			if (Input.GetKeyDown(32))
			{
				this.PlayOffsetPlayTranscendenceCutIn();
			}
			if (Input.GetKeyDown(102))
			{
				this.SwitchIsDamaged();
			}
			if (Input.GetKeyDown(276))
			{
				this.SubMstID();
			}
			if (Input.GetKeyDown(275))
			{
				this.AddMstID();
			}
			if (Input.GetKeyDown(274))
			{
				this.Sub10MstID();
			}
			if (Input.GetKeyDown(273))
			{
				this.Add10MstID();
			}
			if (Input.GetKeyDown(49))
			{
				this.SubBattleAttackKind();
			}
			if (Input.GetKeyDown(50))
			{
				this.AddBattleAttackKind();
			}
			if (Input.GetKeyDown(97))
			{
				this.SubOffSetX();
			}
			if (Input.GetKeyDown(100))
			{
				this.AddOffSetX();
			}
			if (Input.GetKeyDown(115))
			{
				this.SubOffSetY();
			}
			if (Input.GetKeyDown(119))
			{
				this.AddOffSetY();
			}
			if (Input.GetKey(106))
			{
				this.SubOffSetX();
			}
			if (Input.GetKey(108))
			{
				this.AddOffSetX();
			}
			if (Input.GetKey(107))
			{
				this.SubOffSetY();
			}
			if (Input.GetKey(105))
			{
				this.AddOffSetY();
			}
			if (Input.GetKeyDown(122))
			{
				this.SubOffSetZ();
			}
			if (Input.GetKeyDown(120))
			{
				this.AddOffSetZ();
			}
			if (Input.GetKeyDown(114))
			{
				this.offset = Vector3.get_zero();
			}
			if (Input.GetKeyDown(116))
			{
				this.GetOffs();
			}
			if (Input.GetKeyDown(118))
			{
				this.OffsetCopy();
			}
		}

		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect((float)(Screen.get_width() / 2), (float)(Screen.get_height() / 2), (float)(Screen.get_width() / 2), (float)(Screen.get_height() / 2)));
			GUILayout.BeginVertical("box", new GUILayoutOption[0]);
			if (GUILayout.Button("Play(Enter)", new GUILayoutOption[]
			{
				GUILayout.MinWidth(200f)
			}))
			{
				this.PlayTranscendenceCutIn();
			}
			if (GUILayout.Button("PlayOffs(Space)", new GUILayoutOption[]
			{
				GUILayout.MinWidth(200f)
			}))
			{
				this.PlayOffsetPlayTranscendenceCutIn();
			}
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			if (GUILayout.Button("-(1)", new GUILayoutOption[]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				this.SubBattleAttackKind();
			}
			if (GUILayout.Button("+(2)", new GUILayoutOption[]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				this.AddBattleAttackKind();
			}
			GUILayout.Label(string.Format("{0}", this.battleAttackKind), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			if (GUILayout.Button("-(←)", new GUILayoutOption[]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				this.SubMstID();
			}
			if (GUILayout.Button("+(→)", new GUILayoutOption[]
			{
				GUILayout.MinWidth(20f)
			}))
			{
				this.AddMstID();
			}
			GUILayout.Label(string.Format("[{1}]{0}", this.mstId, (!this._dicMstShip.ContainsKey(this.mstId)) ? string.Empty : this._dicMstShip.get_Item(this.mstId).Name), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			int num = (!Mst_DataManager.Instance.Mst_ship_resources.ContainsKey(this.mstId)) ? 0 : Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.mstId).Standing_id;
			int arg_23B_0 = (num <= 500) ? 1 : 0;
			GUILayout.Label(string.Format("ID:{0} StandingID:{1}", this.mstId, num), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			int num2 = (!Mst_DataManager.Instance.Mst_ship_resources.ContainsKey(this.mstId)) ? 0 : Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.mstId).Standing_id;
			bool isFriend = num2 <= 500;
			try
			{
				Vector3 shipOffsPos = ShipUtils.GetShipOffsPos(num2, isFriend, false, MstShipGraphColumn.CutIn);
				Vector3 shipOffsPos2 = ShipUtils.GetShipOffsPos(num2, isFriend, true, MstShipGraphColumn.CutIn);
				GUILayout.Label(string.Format("[主主主左上]\n通常:{0}\nダメージ:{1}", shipOffsPos, shipOffsPos2), new GUILayoutOption[0]);
				Vector3 shipOffsPos3 = ShipUtils.GetShipOffsPos(num2, isFriend, false, MstShipGraphColumn.CutInSp1);
				Vector3 shipOffsPos4 = ShipUtils.GetShipOffsPos(num2, isFriend, true, MstShipGraphColumn.CutInSp1);
				GUILayout.Label(string.Format("[雷雷中心]\n通常:{0}\nダメージ:{1}", shipOffsPos3, shipOffsPos4), new GUILayoutOption[0]);
			}
			catch
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label(string.Format("可変オフセット:{0}", this.offset), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			if (GUILayout.Button(string.Format("ダメージ状態:{0}(D)", (!this.isDamaged) ? "通常" : "ダメージ"), new GUILayoutOption[0]))
			{
				this.SwitchIsDamaged();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private void GetOffs()
		{
			this.offset = ShipUtils.GetShipOffsPos(this.mstId, this.mstId < 500, this.isDamaged, MstShipGraphColumn.CutIn);
		}

		private void PlayTranscendenceCutIn()
		{
		}

		private void PlayOffsetPlayTranscendenceCutIn()
		{
		}

		private void SwitchIsDamaged()
		{
			this.isDamaged = !this.isDamaged;
		}

		private void AddMstID()
		{
			this.mstId++;
			this.mstId = Mathe.MinMax2(this.mstId, this.MIN_SHIP_ID, this.MAX_SHIP_ID);
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void Add10MstID()
		{
			this.mstId += 10;
			this.mstId = Mathe.MinMax2(this.mstId, this.MIN_SHIP_ID, this.MAX_SHIP_ID);
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void SubMstID()
		{
			this.mstId--;
			this.mstId = Mathe.MinMax2(this.mstId, this.MIN_SHIP_ID, this.MAX_SHIP_ID);
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void Sub10MstID()
		{
			this.mstId -= 10;
			this.mstId = Mathe.MinMax2(this.mstId, this.MIN_SHIP_ID, this.MAX_SHIP_ID);
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void AddBattleAttackKind()
		{
			this.battleAttackKind++;
		}

		private void SubBattleAttackKind()
		{
			this.battleAttackKind--;
		}

		private void AddOffSetX()
		{
			this.offset.x = this.offset.x + 1f;
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void SubOffSetX()
		{
			this.offset.x = this.offset.x - 1f;
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void AddOffSetY()
		{
			this.offset.y = this.offset.y + 1f;
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void SubOffSetY()
		{
			this.offset.y = this.offset.y - 1f;
			this.PlayOffsetPlayTranscendenceCutIn();
		}

		private void AddOffSetZ()
		{
			this.offset.z = this.offset.z + 1f;
		}

		private void SubOffSetZ()
		{
			this.offset.z = this.offset.z - 1f;
		}

		private void OffsetCopy()
		{
		}
	}
}
