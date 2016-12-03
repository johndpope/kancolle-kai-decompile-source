using Common.SaveManager;
using local.managers;
using Server_Common;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.SaveLoad
{
	public class SaveLoad : MonoBehaviour, ISaveDataOperator
	{
		public enum Execute_Mode
		{
			Load_Mode = 1,
			Save_Mode,
			Delete_Mode
		}

		public enum Now_State
		{
			Idle,
			Saving,
			Loading,
			Deleting,
			Exiting
		}

		private SoundManager _SM;

		private VitaSaveManager _instance;

		private string _SceneName;

		private UILabel _Label_status;

		private KeyControl ItemSelectController;

		private SaveLoad.Execute_Mode _Execute_Mode;

		private SaveLoad.Now_State _now_status;

		private Generics.Scene BackScene;

		private void Start()
		{
			this._SM = SingletonMonoBehaviour<SoundManager>.Instance;
			this._instance = VitaSaveManager.Instance;
			this._instance.Open(this);
			this._Label_status = GameObject.Find("Label_status").GetComponent<UILabel>();
			this.BackScene = Generics.Scene.PortTop;
			this._Set_Status(SaveLoad.Now_State.Idle);
			this._SceneName = Application.get_loadedLevelName();
			Debug.Log("Application.loadedLevelName: " + this._SceneName);
			Hashtable hashtable = null;
			if (RetentionData.GetData() != null)
			{
				hashtable = RetentionData.GetData();
			}
			if (hashtable == null || (int)hashtable.get_Item("rootType") != 1)
			{
				this._Set_Execute_Mode(SaveLoad.Execute_Mode.Save_Mode);
				if ((int)hashtable.get_Item("rootType") == 21)
				{
					this.BackScene = Generics.Scene.Strategy;
				}
			}
			else
			{
				this._Set_Execute_Mode(SaveLoad.Execute_Mode.Load_Mode);
			}
			if (hashtable != null)
			{
				RetentionData.Release();
			}
			if (this._Execute_Mode == SaveLoad.Execute_Mode.Load_Mode)
			{
				DebugUtils.SLog("ロードを実行します");
				this._DO_LOAD();
			}
			else if (this._Execute_Mode == SaveLoad.Execute_Mode.Save_Mode)
			{
				Debug.Log("セーブを実行します");
				this._DO_SAVE();
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null, true, true);
			}
		}

		private void _Set_Status(SaveLoad.Now_State stat)
		{
			this._now_status = stat;
		}

		public void _Set_Execute_Mode(SaveLoad.Execute_Mode mode)
		{
			this._Execute_Mode = mode;
		}

		private void _DO_SAVE()
		{
			Debug.Log("saveする");
			if (this._now_status != SaveLoad.Now_State.Idle)
			{
				return;
			}
			this._Set_Status(SaveLoad.Now_State.Saving);
			GameObject.Find("Label_status").GetComponent<UILabel>().text = string.Empty;
			this._instance.Save();
		}

		private void _DO_LOAD()
		{
			if (this._now_status != SaveLoad.Now_State.Idle)
			{
				return;
			}
			this._Set_Status(SaveLoad.Now_State.Loading);
			GameObject.Find("Label_status").GetComponent<UILabel>().text = string.Empty;
			DebugUtils.SLog("Loadする");
			this._instance.Load();
		}

		private void _DO_DELETE()
		{
			Debug.Log("Deleteする");
			if (this._now_status != SaveLoad.Now_State.Idle)
			{
				return;
			}
			this._Set_Status(SaveLoad.Now_State.Deleting);
			GameObject.Find("Label_status").GetComponent<UILabel>().text = "デリートdialog";
			this._instance.Delete();
		}

		private void back_to_port()
		{
			this.back_to_port(true);
		}

		private void back_to_port(bool saveloadSuccess)
		{
			if (this._now_status != SaveLoad.Now_State.Idle)
			{
				return;
			}
			this._Set_Status(SaveLoad.Now_State.Exiting);
			this._instance.Close();
			if (this._Execute_Mode == SaveLoad.Execute_Mode.Save_Mode)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(this.BackScene);
			}
			else if (saveloadSuccess)
			{
				DebugUtils.SLog("AppInitializeManager.IsInitialize = true;");
				AppInitializeManager.IsInitialize = true;
				DebugUtils.SLog("ManagerBase.initialize();");
				ManagerBase.initialize();
				DebugUtils.SLog("引継ぎ判定");
				if (Utils.IsValidNewGamePlus() && Utils.IsGameClear())
				{
					this.LoadedInheritData();
				}
				else
				{
					DebugUtils.SLog("通常データ");
					this.LoadedNormalData();
				}
			}
			else
			{
				Debug.Log("タイトルに戻ります。");
				Application.LoadLevel(Generics.Scene.Title.ToString());
			}
		}

		private void LoadedNormalData()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(Resources.Load("Sounds/Voice/kc9999/" + string.Format("{0:D2}", XorRandom.GetILim(206, 211))) as AudioClip, 0);
			DebugUtils.SLog("戦略マップへ進みます。");
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
			Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
		}

		private void LoadedInheritData()
		{
			Application.LoadLevel(Generics.Scene.InheritLoad.ToString());
		}

		public void Canceled()
		{
			Debug.Log("Save/Load/Delete dialog Cancelled.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			this._Label_status.text = string.Empty;
			this.back_to_port(false);
		}

		public void SaveError()
		{
			Debug.Log("Save Error.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			base.StartCoroutine(this.RightDown_Message("セーブ時にエラーが発生しました。"));
			this.back_to_port(false);
		}

		public void SaveComplete()
		{
			Debug.Log("Save Complete.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			this._Label_status.text = string.Empty;
			this._instance.Save();
		}

		public void LoadError()
		{
			DebugUtils.SLog("Load Error.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			base.StartCoroutine(this.RightDown_Message("ロード時に内部エラーが発生しました。"));
			this.back_to_port(false);
		}

		public void LoadComplete()
		{
			Debug.Log("Load Complete.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			this._Label_status.text = string.Empty;
			this.back_to_port();
		}

		public void LoadNothing()
		{
			Debug.Log("Data not found or empty.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			base.StartCoroutine(this.RightDown_Message("ロードデータがありません。"));
			this.back_to_port(false);
		}

		public void DeleteComplete()
		{
			Debug.Log("Delete Complete.");
			this._Set_Status(SaveLoad.Now_State.Idle);
			base.StartCoroutine(this.RightDown_Message("データを削除しました。"));
			this.back_to_port();
		}

		public void SaveManOpen()
		{
			Debug.Log("S/L SaveManOpen");
		}

		public void SaveManClose()
		{
			Debug.Log("S/L SaveManClose");
		}

		[DebuggerHidden]
		public IEnumerator RightDown_Message(string msg)
		{
			SaveLoad.<RightDown_Message>c__IteratorC9 <RightDown_Message>c__IteratorC = new SaveLoad.<RightDown_Message>c__IteratorC9();
			<RightDown_Message>c__IteratorC.msg = msg;
			<RightDown_Message>c__IteratorC.<$>msg = msg;
			<RightDown_Message>c__IteratorC.<>f__this = this;
			return <RightDown_Message>c__IteratorC;
		}
	}
}
