using Sony.NP;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class TrophyManager : SingletonMonoBehaviour<TrophyManager>
	{
		private class TrophyData
		{
			public int Id;

			public bool unlocked;

			public TrophyData(Trophies.TrophyData data)
			{
				this.Id = data.trophyId;
				this.unlocked = data.unlocked;
			}

			public override string ToString()
			{
				string empty = string.Empty;
				return string.Format("[Trophy{0:D2}{1} {2}]{3}", new object[]
				{
					this.Id,
					(!this.unlocked) ? "(未)" : "(済)",
					empty,
					(this.Id % 2 != 1) ? "  " : "\n"
				});
			}
		}

		private List<TrophyManager.TrophyData> _data;

		private Action<bool> _callback_initialize;

		private Action<bool> _callback_update;

		public Action<string> LogMethod;

		public bool Available
		{
			get
			{
				return Trophies.get_TrophiesAreAvailable();
			}
		}

		public int Count
		{
			get
			{
				return (this._data == null) ? 0 : this._data.get_Count();
			}
		}

		public void Initialize(Action<bool> callback)
		{
			if (this._callback_initialize != null)
			{
				return;
			}
			if (this.Available)
			{
				callback.Invoke(true);
				return;
			}
			this._callback_initialize = callback;
			Main.set_enableInternalLogging(true);
			Main.add_OnLog(new Messages.EventHandler(this.__EventHandler_Log__));
			Main.add_OnLogWarning(new Messages.EventHandler(this.__EventHandler_Log__));
			Main.add_OnLogError(new Messages.EventHandler(this.__EventHandler_Log__));
			Main.add_OnNPInitialized(new Messages.EventHandler(this.__EventHandler_Initialized__));
			int num = Main.kNpToolkitCreate_CacheTrophyIcons | Main.kNpToolkitCreate_NoRanking;
			Main.Initialize(num);
		}

		public void UpdateTrophyInfo(Action<bool> callback)
		{
			if (!this.Available)
			{
				callback.Invoke(false);
				return;
			}
			if (Trophies.RequestTrophyInfoIsBusy())
			{
				callback.Invoke(false);
				return;
			}
			if (this._callback_update != null)
			{
				callback.Invoke(false);
				return;
			}
			this._callback_update = callback;
			ErrorCode errorCode = Trophies.RequestTrophyInfo();
			if (errorCode != null)
			{
				this._callback_update = null;
				callback.Invoke(false);
			}
		}

		public void UnlockTrophies(List<int> trophy_ids)
		{
			if (!this.Available || this._data == null || this._callback_update != null)
			{
				return;
			}
			if (trophy_ids == null)
			{
				return;
			}
			for (int i = 0; i < trophy_ids.get_Count(); i++)
			{
				int num = trophy_ids.get_Item(i);
				this._Log(string.Format("ID:{0} 解除開始", num));
				TrophyManager.TrophyData trophyData = this._GetTrophyData(num);
				trophyData.unlocked = true;
				int num2 = this._IDtoIndex(trophyData);
				if (num2 > 0)
				{
					ErrorCode errorCode = Trophies.AwardTrophy(num2);
					if (errorCode != null)
					{
						trophyData.unlocked = false;
					}
				}
			}
		}

		public bool IsUnlocked(int trophy_id)
		{
			TrophyManager.TrophyData trophyData = this._GetTrophyData(trophy_id);
			return trophyData == null || trophyData.unlocked;
		}

		private int _IDtoIndex(int trophy_id)
		{
			TrophyManager.TrophyData trophyData = this._GetTrophyData(trophy_id);
			return this._IDtoIndex(trophyData);
		}

		private int _IDtoIndex(TrophyManager.TrophyData trophyData)
		{
			if (this._data == null)
			{
				return -1;
			}
			if (trophyData == null)
			{
				return -1;
			}
			return this._data.IndexOf(trophyData);
		}

		private TrophyManager.TrophyData _GetTrophyData(int id)
		{
			if (this._data == null)
			{
				return null;
			}
			return this._data.Find((TrophyManager.TrophyData t) => t.Id == id);
		}

		private void __EventHandler_Initialized__(Messages.PluginMessage msg)
		{
			Trophies.add_OnGotTrophyInfo(new Messages.EventHandler(this.__EventHandler_UpdateTrophyInfo__));
			Action<bool> callback_initialize = this._callback_initialize;
			this._callback_initialize = null;
			this.UpdateTrophyInfo(callback_initialize);
		}

		private void __EventHandler_UpdateTrophyInfo__(Messages.PluginMessage msg)
		{
			Trophies.TrophyData[] cachedTrophyData = Trophies.GetCachedTrophyData();
			List<Trophies.TrophyData> list = new List<Trophies.TrophyData>(cachedTrophyData);
			this._data = list.ConvertAll<TrophyManager.TrophyData>((Trophies.TrophyData item) => new TrophyManager.TrophyData(item));
			Action<bool> callback_update = this._callback_update;
			this._callback_update = null;
			if (callback_update != null)
			{
				callback_update.Invoke(true);
			}
		}

		private void __EventHandler_UnlockSuccess__(Messages.PluginMessage msg)
		{
		}

		private void __EventHandler_Log__(Messages.PluginMessage msg)
		{
			this._Log("[NP Log] " + msg.get_Text());
		}

		private void Update()
		{
			Main.Update();
		}

		private void OnDestroy()
		{
			this._callback_initialize = null;
			this._callback_update = null;
			this.LogMethod = null;
			if (this._data != null)
			{
				this._data.Clear();
			}
			this._data = null;
		}

		private void _Log(string message)
		{
			if (this.LogMethod != null)
			{
				this.LogMethod.Invoke(message);
			}
		}

		public override string ToString()
		{
			string text = "== トロフィーデータ ==\n";
			if (this._data != null)
			{
				for (int i = 0; i < this._data.get_Count(); i++)
				{
					text += this._data.get_Item(i).ToString();
				}
			}
			else
			{
				text += "なし";
			}
			return text;
		}
	}
}
