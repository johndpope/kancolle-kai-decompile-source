using KCV.Battle;
using local.models;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Checker
{
	public class BattleDrawCheck : MonoBehaviour
	{
		public enum POG
		{
			注視点,
			特殊注視点,
			演習注視点
		}

		public enum Mode
		{
			ビュワー,
			編集
		}

		private ShipModelMst _clsCurrentShipMst;

		public UIBattleShip uiBattleShip;

		public BattleFieldCamera fieldCamera;

		public GameObject latticePattern;

		public Transform pointOfGazeObj;

		public int shipID;

		public bool isDamaged;

		public bool isInformationOpen = true;

		public BattleDrawCheck.POG pog;

		public BattleDrawCheck.Mode mode;

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

		private void Awake()
		{
			BattleField componentInChildren = base.get_transform().GetComponentInChildren<BattleField>();
			if (this.uiBattleShip != null)
			{
				this.uiBattleShip = Util.Instantiate(this.uiBattleShip.get_gameObject(), componentInChildren.get_gameObject(), false, false).GetComponent<UIBattleShip>();
				this.uiBattleShip.get_transform().set_position(Vector3.get_zero());
				this.uiBattleShip.billboard.billboardTarget = this.fieldCamera.get_transform();
			}
			if (this.fieldCamera != null)
			{
				this.fieldCamera.cullingMask = this.GetDefaultLayers();
			}
		}

		private void Start()
		{
			this.shipID = this.MIN_SHIP_ID;
			this._clsCurrentShipMst = new ShipModelMst(this.shipID);
			Debug.Log(string.Concat(new object[]
			{
				string.Empty,
				this.MIN_SHIP_ID,
				"|",
				this.MAX_SHIP_ID
			}));
			this.setShipID(this.shipID);
		}

		private void Update()
		{
			BattleDrawCheck.Mode mode = this.mode;
			if (mode != BattleDrawCheck.Mode.ビュワー)
			{
				if (mode == BattleDrawCheck.Mode.編集)
				{
					if (Input.GetKeyDown(114))
					{
						this.UpdateShip();
						this.focusCamera();
					}
					switch (this.pog)
					{
					case BattleDrawCheck.POG.注視点:
						if (Input.GetKeyDown(276))
						{
							this.setShipLocalPointOfGaze(Vector3.get_left());
						}
						else if (Input.GetKeyDown(275))
						{
							this.setShipLocalPointOfGaze(Vector3.get_right());
						}
						else if (Input.GetKeyDown(273))
						{
							this.setShipLocalPointOfGaze(Vector3.get_up());
						}
						else if (Input.GetKeyDown(274))
						{
							this.setShipLocalPointOfGaze(Vector3.get_down());
						}
						if (Input.GetKey(304) && Input.GetKey(276))
						{
							this.setShipLocalPointOfGaze(Vector3.get_left() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(275))
						{
							this.setShipLocalPointOfGaze(Vector3.get_right() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(273))
						{
							this.setShipLocalPointOfGaze(Vector3.get_up() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(274))
						{
							this.setShipLocalPointOfGaze(Vector3.get_down() * 10f);
						}
						break;
					case BattleDrawCheck.POG.特殊注視点:
						if (Input.GetKeyDown(276))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_left());
						}
						else if (Input.GetKeyDown(275))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_right());
						}
						else if (Input.GetKeyDown(273))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_up());
						}
						else if (Input.GetKeyDown(274))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_down());
						}
						if (Input.GetKey(304) && Input.GetKey(276))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_left() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(275))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_right() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(273))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_up() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(274))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_down() * 10f);
						}
						break;
					case BattleDrawCheck.POG.演習注視点:
						if (Input.GetKeyDown(276))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_left());
						}
						else if (Input.GetKeyDown(275))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_right());
						}
						else if (Input.GetKeyDown(273))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_up());
						}
						else if (Input.GetKeyDown(274))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_down());
						}
						if (Input.GetKey(304) && Input.GetKey(276))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_left() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(275))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_right() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(273))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_up() * 10f);
						}
						if (Input.GetKey(304) && Input.GetKey(274))
						{
							this.setShipLocalSPPointOfGaze(Vector3.get_down() * 10f);
						}
						break;
					}
				}
			}
			else if (Input.GetKeyDown(276))
			{
				this.setShipID(this.shipID - 1);
			}
			else if (Input.GetKeyDown(275))
			{
				this.setShipID(this.shipID + 1);
			}
			else if (Input.GetKeyDown(273))
			{
				this.setShipID(this.shipID + 10);
			}
			else if (Input.GetKeyDown(274))
			{
				this.setShipID(this.shipID - 10);
			}
			if (Input.GetKeyDown(100))
			{
				this.changeDamage();
			}
			if (Input.GetKeyDown(102))
			{
				this.focusCamera();
			}
			if (Input.GetKeyDown(112))
			{
				this.changePog();
			}
			if (Input.GetKeyDown(101))
			{
				this.changeMode();
			}
			if (Input.GetKeyDown(49))
			{
				this.setShipID(1);
			}
			if (Input.GetKeyDown(50))
			{
				this.setShipID(501);
			}
			if (Input.GetKeyDown(111))
			{
				this.isInformationOpen = !this.isInformationOpen;
			}
			if (Input.GetKeyDown(97))
			{
				this.latticePatternActive();
			}
		}

		private void FixedUpdate()
		{
			if (this.pointOfGazeObj.get_position() != this.fieldCamera.pointOfGaze)
			{
				this.pointOfGazeObj.set_position(this.fieldCamera.pointOfGaze);
			}
		}

		private void UpdateShip()
		{
			if (this._clsCurrentShipMst != null)
			{
				this.UpdateShipTexture();
				this.UpdateShipFootOffs();
				this.UpdateShipPog();
				this.UpdateShipSPPog();
				this.UpdateShipScaleMag();
			}
			else
			{
				this.uiBattleShip.object3D.mainTexture = null;
			}
		}

		private void UpdateShipTexture()
		{
			int texNum = (!this.isDamaged) ? 9 : 10;
			this.uiBattleShip.object3D.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this._clsCurrentShipMst.GetGraphicsMstId(), texNum);
			this.uiBattleShip.object3D.MakePixelPerfect();
		}

		private void UpdateShipFootOffs()
		{
			Vector3 vector;
			try
			{
				vector = Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetFoot_InBattle(this.isDamaged));
				Debug.Log(string.Format("足元位置:{0}", vector));
			}
			catch
			{
				vector = Vector3.get_zero();
			}
			this.uiBattleShip.object3D.get_transform().set_localPosition(vector);
		}

		private void UpdateShipPog()
		{
			Vector3 vector;
			try
			{
				vector = Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetPog_InBattle(this.isDamaged));
				Debug.Log(string.Format("注視点:{0}", vector));
			}
			catch
			{
				vector = Vector3.get_zero();
			}
			Transform transform = this.uiBattleShip.get_transform().FindChild("POG");
			transform.get_transform().set_localPosition(vector);
		}

		private void UpdateShipSPPog()
		{
			Vector3 vector;
			try
			{
				vector = ((this.pog != BattleDrawCheck.POG.特殊注視点) ? Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetPogSpEnsyu_InBattle(this.isDamaged)) : Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetPogSp_InBattle(this.isDamaged)));
				Debug.Log(string.Format("特殊注視点:{0} - {1}", vector, this.pog));
			}
			catch
			{
				vector = Vector3.get_zero();
			}
			Transform transform = this.uiBattleShip.get_transform().FindChild("SPPog");
			transform.get_transform().set_localPosition(vector);
		}

		private void UpdateShipScaleMag()
		{
			float num = 1f;
			try
			{
				num = (float)Mst_DataManager.Instance.Mst_shipgraphbattle.get_Item(this._clsCurrentShipMst.GetGraphicsMstId()).Scale_mag;
			}
			catch
			{
				num = 1f;
			}
			Transform transform = this.uiBattleShip.get_transform().Find("ShipAnchor");
			transform.get_transform().set_localScale(Vector3.get_one() * num);
		}

		private void changePog()
		{
			if (this.pog == BattleDrawCheck.POG.注視点)
			{
				this.pog = BattleDrawCheck.POG.特殊注視点;
			}
			else if (this.pog == BattleDrawCheck.POG.特殊注視点)
			{
				this.pog = BattleDrawCheck.POG.演習注視点;
			}
			else
			{
				this.pog = BattleDrawCheck.POG.注視点;
			}
			this.UpdateShip();
			this.focusCamera();
		}

		private void changeMode()
		{
			if (this.mode == BattleDrawCheck.Mode.ビュワー)
			{
				this.mode = BattleDrawCheck.Mode.編集;
			}
			else
			{
				this.mode = BattleDrawCheck.Mode.ビュワー;
			}
		}

		private void changeDamage()
		{
			this.isDamaged = !this.isDamaged;
			this.UpdateShip();
			this.focusCamera();
		}

		private void focusCamera()
		{
			if (this.fieldCamera != null)
			{
				for (int i = 0; i < 3; i++)
				{
					switch (this.pog)
					{
					case BattleDrawCheck.POG.注視点:
						this.fieldCamera.get_transform().set_position(this.calcCamTargetPosToPog());
						this.fieldCamera.LookAt(this.uiBattleShip.pointOfGaze);
						break;
					case BattleDrawCheck.POG.特殊注視点:
						this.fieldCamera.get_transform().set_position(this.calcCamTargetPosToSPPog());
						this.fieldCamera.LookAt(this.uiBattleShip.spPointOfGaze);
						break;
					case BattleDrawCheck.POG.演習注視点:
						this.fieldCamera.get_transform().set_position(this.calcCamTargetPosToSPPog());
						this.fieldCamera.LookAt(this.uiBattleShip.spPointOfGaze);
						break;
					}
				}
			}
		}

		public Generics.Layers GetDefaultLayers()
		{
			return Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects;
		}

		private Vector3 calcCamTargetPosToPog()
		{
			Vector3 vector = Mathe.NormalizeDirection(this.uiBattleShip.pointOfGaze, this.fieldCamera.eyePosition) * 10f;
			return new Vector3(this.uiBattleShip.pointOfGaze.x + vector.x, this.uiBattleShip.pointOfGaze.y, this.uiBattleShip.pointOfGaze.z + vector.z);
		}

		private Vector3 calcCamTargetPosToSPPog()
		{
			Vector3 vector = Mathe.NormalizeDirection(this.uiBattleShip.spPointOfGaze, this.fieldCamera.eyePosition) * 10f;
			return new Vector3(this.uiBattleShip.spPointOfGaze.x + vector.x, this.uiBattleShip.spPointOfGaze.y, this.uiBattleShip.spPointOfGaze.z + vector.z);
		}

		private void setShipID(int id)
		{
			this.shipID = Mathe.MinMax2(id, this.MIN_SHIP_ID, this.MAX_SHIP_ID);
			try
			{
				this._clsCurrentShipMst = new ShipModelMst(this.shipID);
			}
			catch
			{
			}
			this.UpdateShip();
			this.focusCamera();
		}

		private void setShipLocalPointOfGaze(Vector3 dir)
		{
			this.uiBattleShip.localPointOfGaze += dir;
			this.focusCamera();
		}

		private void setShipLocalSPPointOfGaze(Vector3 dir)
		{
			this.uiBattleShip.localSPPointOfGaze += dir;
			this.focusCamera();
		}

		private void positionCopy()
		{
			switch (this.pog)
			{
			case BattleDrawCheck.POG.注視点:
				Debug.Log(string.Format("注視点[{1}]:{0}", this.uiBattleShip.localPointOfGaze, (!this.isDamaged) ? "通常" : "ダメージ"));
				break;
			case BattleDrawCheck.POG.特殊注視点:
				Debug.Log(string.Format("特殊注視点注視点[{1}]:{0}", this.uiBattleShip.localSPPointOfGaze, (!this.isDamaged) ? "通常" : "ダメージ"));
				break;
			case BattleDrawCheck.POG.演習注視点:
				Debug.Log(string.Format("演習特殊注視点[{1}]:{0}", this.uiBattleShip.localSPPointOfGaze, (!this.isDamaged) ? "通常" : "ダメージ"));
				break;
			}
		}

		private void footPositionCopy()
		{
			Debug.Log(string.Format("足元座標[{1}]:{0}", this.uiBattleShip.object3D.get_transform().get_localPosition(), (!this.isDamaged) ? "通常" : "ダメージ"));
		}

		private Mst_ship getMstShip(int mstId)
		{
			if (Mst_DataManager.Instance.Mst_ship.ContainsKey(mstId))
			{
				return Mst_DataManager.Instance.Mst_ship.get_Item(mstId);
			}
			return null;
		}

		private void latticePatternActive()
		{
			this.latticePattern.SetActive(!this.latticePattern.get_activeSelf());
		}

		private void OnGUI()
		{
			GUILayout.BeginVertical("Box", new GUILayoutOption[0]);
			GUILayout.Label("[BattleDrawCheck Information]", new GUILayoutOption[0]);
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label(string.Format("モード:{0}", this.mode), new GUILayoutOption[0]);
			if (GUILayout.Button("モード変更(E)", new GUILayoutOption[0]))
			{
				this.changeMode();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label("注視点状態:" + this.pog.ToString(), new GUILayoutOption[0]);
			if (GUILayout.Button("注視点変更(P)", new GUILayoutOption[0]))
			{
				this.changePog();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.Label(string.Format("ダメージ状態:{0}", (!this.isDamaged) ? "通常" : "ダメージ"), new GUILayoutOption[0]);
			if (GUILayout.Button("ダメージ状態変更(D)", new GUILayoutOption[0]))
			{
				this.changeDamage();
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("艦ID:" + this.shipID, new GUILayoutOption[0]);
			this.DrawCurrentInfo();
			this.DrawCurrentPog();
			this.isInformationOpen = GUILayout.Toggle(this.isInformationOpen, "Open Settings.(O)", new GUILayoutOption[0]);
			if (this.isInformationOpen)
			{
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				BattleDrawCheck.Mode mode = this.mode;
				if (mode != BattleDrawCheck.Mode.ビュワー)
				{
					if (mode == BattleDrawCheck.Mode.編集)
					{
						BattleDrawCheck.POG pOG = this.pog;
						if (pOG != BattleDrawCheck.POG.注視点)
						{
							if (pOG == BattleDrawCheck.POG.特殊注視点)
							{
								GUILayout.BeginVertical(new GUILayoutOption[0]);
								GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
								if (GUILayout.Button("-1(←)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_left());
								}
								else if (GUILayout.Button("+1(→)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_right());
								}
								else if (GUILayout.Button("-1(↓)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_up());
								}
								else if (GUILayout.Button("+1(↑)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_down());
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
								if (GUILayout.Button("-10(Sf+←)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_left() * 10f);
								}
								if (GUILayout.Button("+10(Sf+→)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_right() * 10f);
								}
								if (GUILayout.Button("-10(Sf+↓)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_down() * 10f);
								}
								if (GUILayout.Button("+10(Sf+↑)", new GUILayoutOption[0]))
								{
									this.setShipLocalSPPointOfGaze(Vector3.get_up() * 10f);
								}
								GUILayout.EndHorizontal();
								GUILayout.EndVertical();
							}
						}
						else
						{
							GUILayout.BeginVertical(new GUILayoutOption[0]);
							GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
							if (GUILayout.Button("-1(←)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_left());
							}
							else if (GUILayout.Button("+1(→)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_right());
							}
							else if (GUILayout.Button("-1(↓)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_up());
							}
							else if (GUILayout.Button("+1(↑)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_down());
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
							if (GUILayout.Button("-10(Sf+←)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_left() * 10f);
							}
							if (GUILayout.Button("+10(Sf+→)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_right() * 10f);
							}
							if (GUILayout.Button("-10(Sf+↓)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_down() * 10f);
							}
							if (GUILayout.Button("+10(Sf+↑)", new GUILayoutOption[0]))
							{
								this.setShipLocalPointOfGaze(Vector3.get_up() * 10f);
							}
							GUILayout.EndHorizontal();
							GUILayout.EndVertical();
						}
					}
				}
				else
				{
					GUILayout.Label(string.Format("艦ID", new object[0]), new GUILayoutOption[0]);
					if (GUILayout.Button("-1(←)", new GUILayoutOption[0]))
					{
						this.setShipID(this.shipID - 1);
					}
					else if (GUILayout.Button("+1(→)", new GUILayoutOption[0]))
					{
						this.setShipID(this.shipID + 1);
					}
					else if (GUILayout.Button("-10(↓)", new GUILayoutOption[0]))
					{
						this.setShipID(this.shipID - 10);
					}
					else if (GUILayout.Button("+10(↑)", new GUILayoutOption[0]))
					{
						this.setShipID(this.shipID + 10);
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("味方/敵切り替え", new object[0]), new GUILayoutOption[0]);
				if (GUILayout.Button("味方艦(1)", new GUILayoutOption[0]))
				{
					this.setShipID(1);
				}
				else if (GUILayout.Button("敵艦(2)", new GUILayoutOption[0]))
				{
					this.setShipID(501);
				}
				GUILayout.EndHorizontal();
				if (GUILayout.Button(string.Format("フォーカス[{0}](F)", this.pog.ToString()), new GUILayoutOption[0]))
				{
					this.focusCamera();
				}
				if (GUILayout.Button(string.Format("格子表示切り替え(A)", new object[0]), new GUILayoutOption[0]))
				{
					this.latticePatternActive();
				}
				if (GUILayout.Button(string.Format("座標コピー[{0}](C)", this.pog.ToString()), new GUILayoutOption[0]))
				{
					this.positionCopy();
				}
			}
			GUILayout.EndVertical();
		}

		private void DrawCurrentInfo()
		{
			try
			{
				this._clsCurrentShipMst = new ShipModelMst(this.shipID);
				GUILayout.Label(string.Format("[{0}({1})]艦ID:{2} GraphicID:{3}", new object[]
				{
					this._clsCurrentShipMst.Name,
					this._clsCurrentShipMst.Yomi,
					this._clsCurrentShipMst.MstId,
					this._clsCurrentShipMst.GetGraphicsMstId()
				}), new GUILayoutOption[0]);
			}
			catch
			{
			}
		}

		private void DrawCurrentPog()
		{
			try
			{
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("[足元座標({0})]\n{1}", (!this.isDamaged) ? "通常" : "大破", Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetFoot_InBattle(this.isDamaged))), new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
				GUILayout.BeginVertical("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("[注視点{0}]\nMst:{1}\n---Edit---\nW:{2}\nL:{3}", new object[]
				{
					(!this.isDamaged) ? "通常" : "大破",
					Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetPog_InBattle(this.isDamaged)),
					this.uiBattleShip.pointOfGaze,
					this.uiBattleShip.localPointOfGaze
				}), new GUILayoutOption[0]);
				GUILayout.EndVertical();
				GUILayout.BeginVertical("box", new GUILayoutOption[0]);
				GUILayout.Label(string.Format("[特殊注視点{0}]\nMst:{1}\n---Edit---\nW:{2}\nL:{3}", new object[]
				{
					(!this.isDamaged) ? "通常" : "大破",
					Util.Poi2Vec(this._clsCurrentShipMst.Offsets.GetPogSp_InBattle(this.isDamaged)),
					this.uiBattleShip.spPointOfGaze,
					this.uiBattleShip.localSPPointOfGaze
				}), new GUILayoutOption[0]);
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			catch
			{
			}
		}
	}
}
