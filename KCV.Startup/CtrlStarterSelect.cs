using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlStarterSelect : MonoBehaviour
	{
		public enum StarterType
		{
			Ex,
			Normal
		}

		[SerializeField]
		private List<UIButton> _listStarterBtn;

		private Action<CtrlStarterSelect.StarterType> _actOnSelectStarter;

		private Action _actOnCancel;

		private CtrlStarterSelect.StarterType _iSelectType;

		private List<List<Texture2D>> _listStarterTexture;

		public CtrlStarterSelect.StarterType selectType
		{
			get
			{
				return this._iSelectType;
			}
		}

		public static CtrlStarterSelect Instantiate(CtrlStarterSelect prefab, Transform parent)
		{
			return Object.Instantiate<CtrlStarterSelect>(prefab);
		}

		private void Awake()
		{
			this._listStarterTexture = new List<List<Texture2D>>();
			List<List<Texture2D>> arg_34_0 = this._listStarterTexture;
			List<Texture2D> list = new List<Texture2D>(2);
			list.Add(null);
			list.Add(null);
			arg_34_0.Add(list);
			List<List<Texture2D>> arg_55_0 = this._listStarterTexture;
			list = new List<Texture2D>(2);
			list.Add(null);
			list.Add(null);
			arg_55_0.Add(list);
			CtrlStarterSelect.StarterType iType = CtrlStarterSelect.StarterType.Ex;
			this._listStarterBtn.ForEach(delegate(UIButton x)
			{
				x.onClick = Util.CreateEventDelegateList(this, "OnClickStarter", iType);
				iType++;
			});
			this._iSelectType = CtrlStarterSelect.StarterType.Ex;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<UIButton>(ref this._listStarterBtn);
			Mem.Del<Action<CtrlStarterSelect.StarterType>>(ref this._actOnSelectStarter);
			Mem.Del<CtrlStarterSelect.StarterType>(ref this._iSelectType);
			Mem.DelListSafe<List<Texture2D>>(ref this._listStarterTexture);
			base.get_transform().GetComponentsInChildren<UIWidget>().ForEach(delegate(UIWidget x)
			{
				if (x is UISprite)
				{
					((UISprite)x).Clear();
				}
				Mem.Del<UIWidget>(ref x);
			});
		}

		public bool Init(Action<CtrlStarterSelect.StarterType> onSelectStarter, Action onCancel)
		{
			UIStartupNavigation navigation = StartupTaskManager.GetNavigation();
			navigation.SetNavigationInStarterSelect();
			base.get_transform().localScaleOne();
			this._actOnSelectStarter = onSelectStarter;
			this._actOnCancel = onCancel;
			this._iSelectType = CtrlStarterSelect.StarterType.Ex;
			this.ChangeFocus(this._iSelectType);
			return true;
		}

		public void PreparaNext(bool isFoward)
		{
			CtrlStarterSelect.StarterType iSelectType = this._iSelectType;
			this._iSelectType = (CtrlStarterSelect.StarterType)Mathe.NextElement((int)this._iSelectType, 0, 1, isFoward);
			if (iSelectType != this._iSelectType)
			{
				this.ChangeFocus(this._iSelectType);
			}
		}

		private void ChangeFocus(CtrlStarterSelect.StarterType iType)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			bool flag = iType == CtrlStarterSelect.StarterType.Ex;
			this.ChangeStarterTexture(iType);
			this._listStarterBtn.get_Item((int)this._iSelectType).GetComponent<UITexture>().depth = 1;
			this._listStarterBtn.get_Item((int)(CtrlStarterSelect.StarterType.Normal - this._iSelectType)).GetComponent<UITexture>().depth = 0;
			UISelectedObject.SelectedOneBoardZoomUpDownStartup(this._listStarterBtn.get_Item(0).get_gameObject(), flag);
			UISelectedObject.SelectedOneBoardZoomUpDownStartup(this._listStarterBtn.get_Item(1).get_gameObject(), !flag);
		}

		private void ChangeStarterTexture(CtrlStarterSelect.StarterType iType)
		{
			bool flag = iType == CtrlStarterSelect.StarterType.Ex;
			if (flag)
			{
				if (this._listStarterTexture.get_Item(0).get_Item(1) == null)
				{
					this._listStarterTexture.get_Item(0).set_Item(1, Resources.Load<Texture2D>("Textures/Startup/Starter/starter1_on"));
				}
				if (this._listStarterTexture.get_Item(1).get_Item(0) == null)
				{
					this._listStarterTexture.get_Item(1).set_Item(0, Resources.Load<Texture2D>("Textures/Startup/Starter/starter2"));
				}
				this._listStarterBtn.get_Item(0).GetComponent<UITexture>().mainTexture = this._listStarterTexture.get_Item(0).get_Item(1);
				this._listStarterBtn.get_Item(1).GetComponent<UITexture>().mainTexture = this._listStarterTexture.get_Item(1).get_Item(0);
			}
			else
			{
				if (this._listStarterTexture.get_Item(0).get_Item(0) == null)
				{
					this._listStarterTexture.get_Item(0).set_Item(0, Resources.Load<Texture2D>("Textures/Startup/Starter/starter1"));
				}
				if (this._listStarterTexture.get_Item(1).get_Item(1) == null)
				{
					this._listStarterTexture.get_Item(1).set_Item(1, Resources.Load<Texture2D>("Textures/Startup/Starter/starter2_on"));
				}
				this._listStarterBtn.get_Item(0).GetComponent<UITexture>().mainTexture = this._listStarterTexture.get_Item(0).get_Item(0);
				this._listStarterBtn.get_Item(1).GetComponent<UITexture>().mainTexture = this._listStarterTexture.get_Item(1).get_Item(1);
			}
		}

		public void OnClickStarter(CtrlStarterSelect.StarterType iType)
		{
			if (this._iSelectType == iType)
			{
				this._listStarterBtn.ForEach(delegate(UIButton x)
				{
					UISelectedObject.SelectedOneBoardZoomUpDownStartup(x.get_gameObject(), false);
				});
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				base.get_transform().localScaleZero();
				Dlg.Call<CtrlStarterSelect.StarterType>(ref this._actOnSelectStarter, iType);
				return;
			}
			this._iSelectType = iType;
			this.ChangeFocus(iType);
		}

		public void OnCancel()
		{
			Dlg.Call(ref this._actOnCancel);
			base.get_transform().localScaleZero();
		}
	}
}
