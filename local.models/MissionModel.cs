using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class MissionModel
	{
		private int _id;

		private MissionClearKinds _state;

		private Mst_mission2 _mst;

		private DeckModel _deck;

		public int Id
		{
			get
			{
				return this._id;
			}
		}

		public DeckModel Deck
		{
			get
			{
				return (this._deck == null || this._deck.MissionState == MissionStates.NONE) ? null : this._deck;
			}
		}

		public MissionClearKinds State
		{
			get
			{
				return this._state;
			}
		}

		public int AreaId
		{
			get
			{
				return this._mst.Maparea_id;
			}
		}

		public string Name
		{
			get
			{
				return this._mst.Name;
			}
		}

		public string Description
		{
			get
			{
				return this._mst.Details;
			}
		}

		public int Turn
		{
			get
			{
				return this._mst.Time;
			}
		}

		public int Difficulty
		{
			get
			{
				return this._mst.Difficulty;
			}
		}

		public double UseFuel
		{
			get
			{
				return this._mst.Use_fuel;
			}
		}

		public double UseAmmo
		{
			get
			{
				return this._mst.Use_bull;
			}
		}

		public int TankerMinCount
		{
			get
			{
				return 0;
			}
		}

		public int TankerMaxCount
		{
			get
			{
				return this._mst.Tanker_num_max;
			}
		}

		public int TankerCount
		{
			get
			{
				return this._mst.Tanker_num;
			}
		}

		public int CompleteTurn
		{
			get
			{
				return (this._deck != null) ? this._deck.MissionCompleteTurn : 0;
			}
		}

		public MissionModel(User_MissionFmt fmt)
		{
			this._id = fmt.MissionId;
			this._state = fmt.State;
			this._mst = Mst_DataManager.Instance.Mst_mission.get_Item(this._id);
		}

		public MissionModel(User_MissionFmt fmt, DeckModel deck)
		{
			this._id = fmt.MissionId;
			this._state = fmt.State;
			this._mst = Mst_DataManager.Instance.Mst_mission.get_Item(this._id);
			this._deck = deck;
		}

		public MaterialInfo GetRewardMaterials()
		{
			return new MaterialInfo
			{
				Fuel = this._mst.Win_mat1,
				Ammo = this._mst.Win_mat2,
				Steel = this._mst.Win_mat3,
				Baux = this._mst.Win_mat4
			};
		}

		public List<Reward_Useitem> GetRewardUseitems()
		{
			List<Reward_Useitem> list = new List<Reward_Useitem>();
			if (this._mst.Win_item1 > 0)
			{
				list.Add(new Reward_Useitem(this._mst.Win_item1, this._mst.Win_item1_num));
			}
			if (this._mst.Win_item2 > 0)
			{
				list.Add(new Reward_Useitem(this._mst.Win_item2, this._mst.Win_item2_num));
			}
			return list;
		}

		public override string ToString()
		{
			string text = string.Format("{0}(ID:{1}) 状態:{2} 海域:{3} {4}ターン", new object[]
			{
				this.Name,
				this.Id,
				this.State,
				this.AreaId,
				this.Turn
			});
			if (this.TankerMaxCount - this.TankerMinCount > 0)
			{
				text += string.Format(" 必要輸送船数:{0}-{1}", this.TankerMinCount, this.TankerMaxCount);
			}
			if (this.Deck != null)
			{
				string text2 = "?";
				if (this.Deck.MissionState == MissionStates.RUNNING)
				{
					text2 = "遠征中";
				}
				else if (this.Deck.MissionState == MissionStates.END)
				{
					text2 = "遠征完了";
				}
				else if (this.Deck.MissionState == MissionStates.STOP)
				{
					text2 = "遠征中止";
				}
				text += string.Format(" [[艦隊{0} {1} 終了ターン:{2}]]\n", this.Deck.Id, text2, this.Deck.MissionCompleteTurn);
			}
			else
			{
				text += string.Format("\n", new object[0]);
			}
			text += string.Format("\t{0}\n", this.Description);
			text += string.Format("\t難易度:{0} 消費資材:{1}/{2}  ", this.Difficulty, this.UseFuel, this.UseAmmo);
			MaterialInfo rewardMaterials = this.GetRewardMaterials();
			text += string.Format("報酬４資材 {0}/{1}/{2}/{3}  ", new object[]
			{
				rewardMaterials.Fuel,
				rewardMaterials.Ammo,
				rewardMaterials.Steel,
				rewardMaterials.Baux
			});
			List<Reward_Useitem> rewardUseitems = this.GetRewardUseitems();
			for (int i = 0; i < rewardUseitems.get_Count(); i++)
			{
				text += string.Format("報酬アイテム{0}:{1}  ", i + 1, rewardUseitems.get_Item(i));
			}
			return text + "\n";
		}
	}
}
