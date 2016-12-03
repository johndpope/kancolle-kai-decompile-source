using Server_Models;
using System;

namespace local.models.battle
{
	public abstract class PlaneModelBase
	{
		private Mst_slotitem _mst;

		private int _power_stage1_start;

		private int _power_stage1_end;

		private int _power_stage2_end = -1;

		public int MstId
		{
			get
			{
				return this._mst.Id;
			}
		}

		public string Name
		{
			get
			{
				return this._mst.Name;
			}
		}

		public int Power_Stage1Start
		{
			get
			{
				return this._power_stage1_start;
			}
		}

		public int Power_Stage1End
		{
			get
			{
				return this._power_stage1_end;
			}
		}

		public PlaneState State_Stage1End
		{
			get
			{
				return this._GetState(this._power_stage1_end);
			}
		}

		public int Power_Stage2End
		{
			get
			{
				return (this._power_stage2_end != -1) ? this._power_stage2_end : this._power_stage1_end;
			}
		}

		public PlaneState State_Stage2End
		{
			get
			{
				return this._GetState(this.Power_Stage2End);
			}
		}

		public PlaneModelBase(int slotitem_mst_id)
		{
			Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(slotitem_mst_id, ref this._mst);
		}

		public void SetStage1Power(int power, ref int extra)
		{
			this._power_stage1_start = power;
			if (extra > 0)
			{
				this._power_stage1_start++;
				extra--;
			}
			this._power_stage1_end = this._power_stage1_start;
		}

		public void SetStage1Lost(ref int lost)
		{
			int num = Math.Min(lost, this._power_stage1_end);
			this._power_stage1_end = this._power_stage1_start - num;
			lost -= num;
			this._power_stage2_end = this._power_stage1_end;
		}

		public void SetStage2Lost(ref int lost)
		{
			int num = Math.Min(lost, this.Power_Stage2End);
			this._power_stage2_end = this.Power_Stage2End - num;
			lost -= num;
		}

		public bool IsAttackPlane()
		{
			return this._mst.Type3 == 6;
		}

		private PlaneState _GetState(int power)
		{
			if (power <= 0)
			{
				return PlaneState.Crush;
			}
			if (power < this._power_stage1_start)
			{
				return PlaneState.Damage;
			}
			return PlaneState.Normal;
		}

		protected string ToString_PlaneState()
		{
			return string.Format("stg1:{0}->{1}({2}) stg2:{3}->{4}({5})", new object[]
			{
				this.Power_Stage1Start,
				this.Power_Stage1End,
				this.State_Stage1End,
				this.Power_Stage1End,
				this.Power_Stage2End,
				this.State_Stage2End
			});
		}

		public override string ToString()
		{
			return string.Format("[{0}(mst:{1}) {2}]", this.Name, this.MstId, this.ToString_PlaneState());
		}
	}
}
