using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public abstract class KoukuuModelBase : BattlePhaseModel
	{
		protected List<ShipModel_BattleAll> _ships_f;

		protected List<ShipModel_BattleAll> _ships_e;

		protected AirBattle _data;

		protected List<PlaneModelBase> _planes_f;

		protected List<PlaneModelBase> _planes_e;

		private List<BakuRaiDamageModel> _bakurai_f;

		private List<BakuRaiDamageModel> _bakurai_e;

		private ShipModel_Attacker _taiku_ship_f;

		private ShipModel_Attacker _taiku_ship_e;

		private List<SlotitemModel_Battle> _taiku_slotitems_f;

		private List<SlotitemModel_Battle> _taiku_slotitems_e;

		public int Stage1_StartCount_f
		{
			get
			{
				return (this._data.Air1 != null) ? this._data.Air1.F_LostInfo.Count : 0;
			}
		}

		public int Stage1_LostCount_f
		{
			get
			{
				return (this._data.Air1 != null) ? this._data.Air1.F_LostInfo.LostCount : 0;
			}
		}

		public int Stage1_EndCount_f
		{
			get
			{
				return this.Stage1_StartCount_f - this.Stage1_LostCount_f;
			}
		}

		public int Stage1_StartCount_e
		{
			get
			{
				return (this._data.Air1 != null) ? this._data.Air1.E_LostInfo.Count : 0;
			}
		}

		public int Stage1_LostCount_e
		{
			get
			{
				return (this._data.Air1 != null) ? this._data.Air1.E_LostInfo.LostCount : 0;
			}
		}

		public int Stage1_EndCount_e
		{
			get
			{
				return this.Stage1_StartCount_e - this.Stage1_LostCount_e;
			}
		}

		public int Stage2_StartCount_f
		{
			get
			{
				return this.Stage1_EndCount_f;
			}
		}

		public int Stage2_LostCount_f
		{
			get
			{
				if (this._data.Air2 == null)
				{
					return 0;
				}
				if (this._data.Air2.F_LostInfo.Count == 0)
				{
					return 0;
				}
				return this._data.Air2.F_LostInfo.LostCount;
			}
		}

		public int Stage2_EndCount_f
		{
			get
			{
				return this.Stage2_StartCount_f - this.Stage2_LostCount_f;
			}
		}

		public int Stage2_StartCount_e
		{
			get
			{
				return this.Stage1_EndCount_e;
			}
		}

		public int Stage2_LostCount_e
		{
			get
			{
				if (this._data.Air2 == null)
				{
					return 0;
				}
				if (this._data.Air2.E_LostInfo.Count == 0)
				{
					return 0;
				}
				return this._data.Air2.E_LostInfo.LostCount;
			}
		}

		public int Stage2_EndCount_e
		{
			get
			{
				return this.Stage2_StartCount_e - this.Stage2_LostCount_e;
			}
		}

		public KoukuuModelBase(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, AirBattle data)
		{
			this._ships_f = ships_f;
			this._ships_e = ships_e;
			this._data = data;
		}

		public bool existStage2()
		{
			return this._data.StageFlag[1];
		}

		public ShipModel_Attacker GetTaikuShip(bool is_friend)
		{
			return (!is_friend) ? this._taiku_ship_e : this._taiku_ship_f;
		}

		public List<SlotitemModel_Battle> GetTaikuSlotitems(bool is_friend)
		{
			return (!is_friend) ? this._taiku_slotitems_e : this._taiku_slotitems_f;
		}

		public bool existStage3()
		{
			return this._data.StageFlag[2];
		}

		public PlaneModelBase[] GetPlanes(bool is_friend)
		{
			return ((!is_friend) ? this._planes_e : this._planes_f).ToArray();
		}

		public SlotitemModel_Battle[] GetBakugekiPlanes(bool is_friend)
		{
			if (this._data.Air3 == null)
			{
				return new SlotitemModel_Battle[0];
			}
			if (is_friend)
			{
				return this._data.Air3.F_BakugekiPlane.ConvertAll<SlotitemModel_Battle>((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
			}
			return this._data.Air3.E_BakugekiPlane.ConvertAll<SlotitemModel_Battle>((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
		}

		public SlotitemModel_Battle[] GetRaigekiPlanes(bool is_friend)
		{
			if (this._data.Air3 == null)
			{
				return new SlotitemModel_Battle[0];
			}
			if (is_friend)
			{
				return this._data.Air3.F_RaigekiPlane.ConvertAll<SlotitemModel_Battle>((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
			}
			return this._data.Air3.E_RaigekiPlane.ConvertAll<SlotitemModel_Battle>((int mstId) => new SlotitemModel_Battle(mstId)).ToArray();
		}

		public PlaneModelBase[] GetNoDamagePlane_f()
		{
			return this._planes_f.FindAll((PlaneModelBase plane) => plane.State_Stage2End == PlaneState.Normal).ToArray();
		}

		public PlaneModelBase[] GetNoDamagePlane_e()
		{
			return this._planes_e.FindAll((PlaneModelBase plane) => plane.State_Stage2End == PlaneState.Normal).ToArray();
		}

		public bool IsBakugeki_f()
		{
			if (this._data.Air3 == null)
			{
				return false;
			}
			if (this._data.Air3.F_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(this._data.Air3.F_Bakurai.IsBakugeki);
			return list.IndexOf(true) != -1;
		}

		public bool IsBakugeki_e()
		{
			if (this._data.Air3 == null)
			{
				return false;
			}
			if (this._data.Air3.E_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(this._data.Air3.E_Bakurai.IsBakugeki);
			return list.IndexOf(true) != -1;
		}

		public bool IsRaigeki_f()
		{
			if (this._data.Air3 == null)
			{
				return false;
			}
			if (this._data.Air3.F_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(this._data.Air3.F_Bakurai.IsRaigeki);
			return list.IndexOf(true) != -1;
		}

		public bool IsRaigeki_e()
		{
			if (this._data.Air3 == null)
			{
				return false;
			}
			if (this._data.Air3.E_Bakurai == null)
			{
				return false;
			}
			List<bool> list = new List<bool>(this._data.Air3.E_Bakurai.IsRaigeki);
			return list.IndexOf(true) != -1;
		}

		public BakuRaiDamageModel[] GetRaigekiData_f()
		{
			return this._bakurai_f.ToArray();
		}

		public BakuRaiDamageModel[] GetRaigekiData_e()
		{
			return this._bakurai_e.ToArray();
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return this.GetDefenders(is_friend, false);
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, bool all)
		{
			List<BakuRaiDamageModel> list = (!is_friend) ? this._bakurai_e : this._bakurai_f;
			if (!all)
			{
				list = list.FindAll((BakuRaiDamageModel item) => item != null && (item.IsRaigeki() || item.IsBakugeki()));
			}
			return list.ConvertAll<ShipModel_Defender>((BakuRaiDamageModel item) => (item != null) ? item.Defender : null);
		}

		public BakuRaiDamageModel GetAttackDamage(int defender_tmp_id)
		{
			BakuRaiDamageModel bakuRaiDamageModel = this._bakurai_f.Find((BakuRaiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (bakuRaiDamageModel != null)
			{
				return bakuRaiDamageModel;
			}
			bakuRaiDamageModel = this._bakurai_e.Find((BakuRaiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (bakuRaiDamageModel != null)
			{
				return bakuRaiDamageModel;
			}
			return null;
		}

		protected void _Initialize()
		{
			this._CreatePlanes();
			this._CalcStage1();
			this._CalcStage2();
			this._CalcStage3();
		}

		protected virtual void _CreatePlanes()
		{
		}

		protected List<PlaneModelBase> __CreatePlanes(List<ShipModel_Attacker> ships, List<int> plane_from)
		{
			List<PlaneModelBase> list = new List<PlaneModelBase>();
			for (int i = 0; i < plane_from.get_Count(); i++)
			{
				int tmp_id = plane_from.get_Item(i);
				ShipModel_Attacker shipModel_Attacker = ships.Find((ShipModel_Attacker item) => item.TmpId == tmp_id);
				if (shipModel_Attacker != null)
				{
					List<SlotitemModel_Battle> list2 = shipModel_Attacker.SlotitemList;
					list2 = list2.FindAll((SlotitemModel_Battle slot) => slot != null && slot.IsPlaneAtKouku());
					if (list2.get_Count() > 0)
					{
						for (int j = 0; j < list2.get_Count(); j++)
						{
							PlaneModel planeModel = new PlaneModel(shipModel_Attacker, list2.get_Item(j).MstId);
							list.Add(planeModel);
						}
					}
				}
			}
			return list;
		}

		protected void _CalcStage1()
		{
			if (this._data.Air1 == null)
			{
				return;
			}
			Random random = new Random();
			if (this._planes_f.get_Count() > 0)
			{
				int count = this._data.Air1.F_LostInfo.Count;
				int power = (int)Math.Floor((double)count / (double)this._planes_f.get_Count());
				int num = count % this._planes_f.get_Count();
				for (int i = 0; i < this._planes_f.get_Count(); i++)
				{
					this._planes_f.get_Item(i).SetStage1Power(power, ref num);
				}
				int j = this._data.Air1.F_LostInfo.LostCount;
				while (j > 0)
				{
					List<PlaneModelBase> list = this._planes_f.FindAll((PlaneModelBase plane) => plane.Power_Stage1End > 0);
					if (list.get_Count() > 0)
					{
						PlaneModelBase planeModelBase = list.get_Item(random.Next(list.get_Count()));
						planeModelBase.SetStage1Lost(ref j);
					}
				}
			}
			if (this._planes_e.get_Count() > 0)
			{
				int count2 = this._data.Air1.E_LostInfo.Count;
				int power2 = (int)Math.Floor((double)count2 / (double)this._planes_e.get_Count());
				int num2 = count2 % this._planes_e.get_Count();
				for (int k = 0; k < this._planes_e.get_Count(); k++)
				{
					this._planes_e.get_Item(k).SetStage1Power(power2, ref num2);
				}
				int l = this._data.Air1.E_LostInfo.LostCount;
				while (l > 0)
				{
					List<PlaneModelBase> list2 = this._planes_e.FindAll((PlaneModelBase plane) => plane.Power_Stage1End > 0);
					if (list2.get_Count() > 0)
					{
						PlaneModelBase planeModelBase2 = list2.get_Item(random.Next(list2.get_Count()));
						planeModelBase2.SetStage1Lost(ref l);
					}
				}
			}
		}

		protected void _CalcStage2()
		{
			if (this._data.Air2 == null)
			{
				return;
			}
			Random random = new Random();
			if (this._planes_f.get_Count() > 0)
			{
				int i = this.Stage2_LostCount_f;
				while (i > 0)
				{
					List<PlaneModelBase> list = this._planes_f.FindAll((PlaneModelBase plane) => plane.Power_Stage2End > 0);
					if (list.get_Count() > 0)
					{
						PlaneModelBase planeModelBase = list.get_Item(random.Next(list.get_Count()));
						planeModelBase.SetStage2Lost(ref i);
					}
				}
			}
			if (this._planes_e.get_Count() > 0)
			{
				int j = this.Stage2_LostCount_e;
				while (j > 0)
				{
					List<PlaneModelBase> list2 = this._planes_e.FindAll((PlaneModelBase plane) => plane.Power_Stage2End > 0);
					if (list2.get_Count() > 0)
					{
						PlaneModelBase planeModelBase2 = list2.get_Item(random.Next(list2.get_Count()));
						planeModelBase2.SetStage2Lost(ref j);
					}
				}
			}
			if (this._data.Air2.F_AntiFire != null)
			{
				AirFireInfo info = this._data.Air2.F_AntiFire;
				this._taiku_ship_f = this._ships_f.Find((ShipModel_BattleAll ship) => ship.TmpId == info.AttackerId).__CreateAttacker__();
				this._taiku_slotitems_f = new List<SlotitemModel_Battle>();
				for (int k = 0; k < info.UseItems.get_Count(); k++)
				{
					this._taiku_slotitems_f.Add(new SlotitemModel_Battle(info.UseItems.get_Item(k)));
				}
			}
			if (this._data.Air2.E_AntiFire != null)
			{
				AirFireInfo info = this._data.Air2.E_AntiFire;
				this._taiku_ship_e = this._ships_e.Find((ShipModel_BattleAll ship) => ship.TmpId == info.AttackerId).__CreateAttacker__();
				this._taiku_slotitems_e = new List<SlotitemModel_Battle>();
				for (int l = 0; l < info.UseItems.get_Count(); l++)
				{
					this._taiku_slotitems_e.Add(new SlotitemModel_Battle(info.UseItems.get_Item(l)));
				}
			}
		}

		protected void _CalcStage3()
		{
			if (this._data.Air3 == null)
			{
				this._data_f = this._CreateRaigekiData(null, this._ships_f);
				this._data_e = this._CreateRaigekiData(null, this._ships_e);
			}
			else
			{
				this._data_f = this._CreateRaigekiData(this._data.Air3.F_Bakurai, this._ships_f);
				this._data_e = this._CreateRaigekiData(this._data.Air3.E_Bakurai, this._ships_e);
			}
			this._bakurai_f = this._data_f.ConvertAll<BakuRaiDamageModel>((DamageModelBase item) => (BakuRaiDamageModel)item);
			this._bakurai_e = this._data_e.ConvertAll<BakuRaiDamageModel>((DamageModelBase item) => (BakuRaiDamageModel)item);
			for (int i = 0; i < this._bakurai_f.get_Count(); i++)
			{
				if (this._bakurai_f.get_Item(i) != null)
				{
					this._bakurai_f.get_Item(i).__CalcDamage__();
				}
			}
			for (int j = 0; j < this._bakurai_e.get_Count(); j++)
			{
				if (this._bakurai_e.get_Item(j) != null)
				{
					this._bakurai_e.get_Item(j).__CalcDamage__();
				}
			}
		}

		private List<DamageModelBase> _CreateRaigekiData(BakuRaiInfo Bakurai, List<ShipModel_BattleAll> ships)
		{
			List<DamageModelBase> list = new List<DamageModelBase>();
			for (int i = 0; i < ships.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships.get_Item(i);
				if (shipModel_BattleAll == null || Bakurai == null)
				{
					list.Add(null);
				}
				else
				{
					bool is_raigeki = Bakurai.IsRaigeki[i];
					bool is_bakugeki = Bakurai.IsBakugeki[i];
					BakuRaiDamageModel bakuRaiDamageModel = new BakuRaiDamageModel(shipModel_BattleAll, is_raigeki, is_bakugeki);
					int damage = Bakurai.Damage[i];
					BattleHitStatus hitstate = Bakurai.Clitical[i];
					BattleDamageKinds dmgkind = Bakurai.DamageType[i];
					bakuRaiDamageModel.__AddData__(damage, hitstate, dmgkind);
					list.Add(bakuRaiDamageModel);
				}
			}
			return list;
		}

		protected string ToString_Stage1()
		{
			string text = string.Empty;
			if (this._data.Air1 != null)
			{
				text += string.Format("--Stage1 ", new object[0]);
				text += string.Format("[味方側] Count:{0}-{1} Lost:{2} ", this.Stage1_StartCount_f, this.Stage1_EndCount_f, this.Stage1_LostCount_f);
				text += string.Format("[相手側] Count:{0}-{1} Lost:{2}\n", this.Stage1_StartCount_e, this.Stage1_EndCount_e, this.Stage1_LostCount_e);
			}
			return text;
		}

		protected string ToString_Stage2()
		{
			string text = string.Empty;
			if (this._data.Air2 != null)
			{
				text += string.Format("--Stage2 ", new object[0]);
				text += string.Format("[味方側] Count:{0}-{1} Lost:{2} ", this.Stage2_StartCount_f, this.Stage2_EndCount_f, this.Stage2_LostCount_f);
				text += string.Format("[相手側] Count:{0}-{1} Lost:{2}\n", this.Stage2_StartCount_e, this.Stage2_EndCount_e, this.Stage2_LostCount_e);
				ShipModel_Attacker taikuShip = this.GetTaikuShip(true);
				List<SlotitemModel_Battle> taikuSlotitems = this.GetTaikuSlotitems(true);
				if (taikuShip != null)
				{
					text += string.Format("[味方側 対空カットイン] {0}\n", taikuShip);
					text += "\t使用した装備: ";
					for (int i = 0; i < taikuSlotitems.get_Count(); i++)
					{
						if (taikuSlotitems.get_Item(i) == null)
						{
							text += " [-]";
						}
						else
						{
							text += string.Format(" [{0}]", taikuSlotitems.get_Item(i));
						}
					}
					text += "\n";
				}
				taikuShip = this.GetTaikuShip(false);
				taikuSlotitems = this.GetTaikuSlotitems(false);
				if (taikuShip != null)
				{
					text += string.Format("[相手側 対空カットイン] {0}\n", taikuShip);
					text += "\t使用した装備: ";
					for (int j = 0; j < taikuSlotitems.get_Count(); j++)
					{
						if (taikuSlotitems.get_Item(j) == null)
						{
							text += " [-]";
						}
						else
						{
							text += string.Format(" [{0}]", taikuSlotitems.get_Item(j));
						}
					}
					text += "\n";
				}
			}
			return text;
		}

		protected string ToString_Stage3()
		{
			string text = string.Empty;
			if (this._data.Air3 != null)
			{
				text += string.Format("--Stage3 ", new object[0]);
				text += string.Format("[味方側への爆撃] {0} ", (!this.IsBakugeki_f()) ? "無" : "有");
				text += string.Format("[相手側への爆撃] {0}\n", (!this.IsBakugeki_e()) ? "無" : "有");
				SlotitemModel_Battle[] bakugekiPlanes = this.GetBakugekiPlanes(true);
				if (bakugekiPlanes.Length > 0)
				{
					text += string.Format("--爆撃を行った艦載機(味方側)--\n", new object[0]);
				}
				for (int i = 0; i < bakugekiPlanes.Length; i++)
				{
					text += string.Format(" [{0}]\n", bakugekiPlanes[i]);
				}
				SlotitemModel_Battle[] bakugekiPlanes2 = this.GetBakugekiPlanes(false);
				if (bakugekiPlanes2.Length > 0)
				{
					text += string.Format("--爆撃を行った艦載機(相手側)--\n", new object[0]);
				}
				for (int j = 0; j < bakugekiPlanes2.Length; j++)
				{
					text += string.Format(" [{0}]\n", bakugekiPlanes2[j]);
				}
				SlotitemModel_Battle[] raigekiPlanes = this.GetRaigekiPlanes(true);
				if (raigekiPlanes.Length > 0)
				{
					text += string.Format("--雷撃を行った艦載機(味方側)--\n", new object[0]);
				}
				for (int k = 0; k < raigekiPlanes.Length; k++)
				{
					text += string.Format(" [{0}]\n", raigekiPlanes[k]);
				}
				SlotitemModel_Battle[] raigekiPlanes2 = this.GetRaigekiPlanes(false);
				if (raigekiPlanes2.Length > 0)
				{
					text += string.Format("--雷撃を行った艦載機(相手側)--\n", new object[0]);
				}
				for (int l = 0; l < raigekiPlanes2.Length; l++)
				{
					text += string.Format(" [{0}]\n", raigekiPlanes2[l]);
				}
				BakuRaiDamageModel[] array = this.GetRaigekiData_f();
				for (int m = 0; m < array.Length; m++)
				{
					BakuRaiDamageModel bakuRaiDamageModel = array[m];
					if (bakuRaiDamageModel != null)
					{
						if (bakuRaiDamageModel.IsRaigeki() || bakuRaiDamageModel.IsBakugeki())
						{
							text += string.Format("{0}({1}) へ雷撃 (ダメージ:{2} {3}{4}) {5}{6}\n", new object[]
							{
								bakuRaiDamageModel.Defender.Name,
								bakuRaiDamageModel.Defender.Index,
								bakuRaiDamageModel.GetDamage(),
								bakuRaiDamageModel.GetHitState(),
								(!bakuRaiDamageModel.GetProtectEffect()) ? string.Empty : "[かばう]",
								(!bakuRaiDamageModel.IsBakugeki()) ? string.Empty : "[爆]",
								(!bakuRaiDamageModel.IsRaigeki()) ? string.Empty : "[雷]"
							});
						}
					}
				}
				array = this.GetRaigekiData_e();
				for (int n = 0; n < array.Length; n++)
				{
					BakuRaiDamageModel bakuRaiDamageModel2 = array[n];
					if (bakuRaiDamageModel2 != null)
					{
						if (bakuRaiDamageModel2.IsRaigeki() || bakuRaiDamageModel2.IsBakugeki())
						{
							text += string.Format("{0}({1}) へ雷撃 (ダメージ:{2} {3}{4}) {5}{6}\n", new object[]
							{
								bakuRaiDamageModel2.Defender.Name,
								bakuRaiDamageModel2.Defender.Index,
								bakuRaiDamageModel2.GetDamage(),
								bakuRaiDamageModel2.GetHitState(),
								(!bakuRaiDamageModel2.GetProtectEffect()) ? string.Empty : "[かばう]",
								(!bakuRaiDamageModel2.IsBakugeki()) ? string.Empty : "[爆]",
								(!bakuRaiDamageModel2.IsRaigeki()) ? string.Empty : "[雷]"
							});
						}
					}
				}
			}
			return text;
		}
	}
}
