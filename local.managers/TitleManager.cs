using Common.Enum;
using local.models;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class TitleManager
	{
		private SettingModel _setting_model;

		public SettingModel Settings
		{
			get
			{
				return this._setting_model;
			}
		}

		public string UserName
		{
			get
			{
				Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
				return (user_basic != null) ? user_basic.Nickname : string.Empty;
			}
		}

		public TitleManager()
		{
			this._setting_model = new SettingModel();
			new Api_req_Member().PurgeUserData();
		}

		public DifficultKind? GetOpenedDifficulty()
		{
			if (Server_Common.Utils.IsGameClear() && Server_Common.Utils.IsValidNewGamePlus())
			{
				DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
				int num = Comm_UserDatas.Instance.User_plus.ClearNum(DifficultKind.KOU);
				int num2 = Comm_UserDatas.Instance.User_plus.ClearNum(DifficultKind.OTU);
				if (difficult == DifficultKind.KOU && num == 1)
				{
					return new DifficultKind?(DifficultKind.SHI);
				}
				if (difficult == DifficultKind.OTU && num2 == 1)
				{
					return new DifficultKind?(DifficultKind.KOU);
				}
			}
			return default(DifficultKind?);
		}

		public void CreateSaveData(string user_name, int ship_mst_id, DifficultKind difficulty)
		{
			new Api_req_Member().CreateNewUser(1, user_name, ship_mst_id, difficulty);
			ManagerBase.initialize();
		}

		public bool CreateSaveDataPlus(string user_name, int ship_mst_id, DifficultKind difficulty)
		{
			if (Server_Common.Utils.IsGameClear() && Server_Common.Utils.IsValidNewGamePlus())
			{
				TrophyUtil.__tmp_start_album_ship_num__ = Server_Common.Utils.GetBookRegNum(1);
				TrophyUtil.__tmp_start_album_slot_num__ = Server_Common.Utils.GetBookRegNum(2);
				new Api_req_Member().NewGamePlus(user_name, difficulty, ship_mst_id);
				ManagerBase.initialize();
				return true;
			}
			return false;
		}

		public HashSet<DifficultKind> GetClearedDifficulty()
		{
			return new Api_req_Member().GetClearDifficult();
		}

		public HashSet<DifficultKind> GetSelectableDifficulty()
		{
			HashSet<DifficultKind> hashSet = new HashSet<DifficultKind>();
			hashSet.Add(DifficultKind.TEI);
			hashSet.Add(DifficultKind.HEI);
			hashSet.Add(DifficultKind.OTU);
			HashSet<DifficultKind> clearedDifficulty = this.GetClearedDifficulty();
			if (clearedDifficulty.Contains(DifficultKind.OTU))
			{
				hashSet.Add(DifficultKind.KOU);
			}
			if (clearedDifficulty.Contains(DifficultKind.KOU))
			{
				hashSet.Add(DifficultKind.SHI);
			}
			return hashSet;
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
