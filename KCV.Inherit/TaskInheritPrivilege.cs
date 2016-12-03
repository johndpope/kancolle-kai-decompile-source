using Common.Enum;
using local.managers;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritPrivilege : SceneTaskMono
	{
		private enum RewardType
		{
			Difficulty,
			PortExtend,
			Syorui,
			SPoint,
			_num
		}

		private UIWidget myUIWidget;

		[SerializeField]
		private InheritRewardMessage[] rewardMessages;

		private InheritRewardMessage NowRewardMessage;

		private KeyControl key;

		[SerializeField]
		private UISprite DifficltyTex;

		protected override void Start()
		{
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
			this.myUIWidget = base.GetComponent<UIWidget>();
			this.ClearCheck();
		}

		protected override bool Run()
		{
			this.key.Update();
			return true;
		}

		protected override bool Init()
		{
			base.StartCoroutine(this.DialogControl());
			return true;
		}

		[DebuggerHidden]
		private IEnumerator DialogControl()
		{
			TaskInheritPrivilege.<DialogControl>c__Iterator67 <DialogControl>c__Iterator = new TaskInheritPrivilege.<DialogControl>c__Iterator67();
			<DialogControl>c__Iterator.<>f__this = this;
			return <DialogControl>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator WaitForKey(params KeyControl.KeyName[] keyNames)
		{
			TaskInheritPrivilege.<WaitForKey>c__Iterator68 <WaitForKey>c__Iterator = new TaskInheritPrivilege.<WaitForKey>c__Iterator68();
			<WaitForKey>c__Iterator.keyNames = keyNames;
			<WaitForKey>c__Iterator.<$>keyNames = keyNames;
			<WaitForKey>c__Iterator.<>f__this = this;
			return <WaitForKey>c__Iterator;
		}

		private void ClearCheck()
		{
			if (App.GetTitleManager() == null)
			{
				App.SetTitleManager(new TitleManager());
			}
			DifficultKind? openedDifficulty = App.GetTitleManager().GetOpenedDifficulty();
			Debug.Log(openedDifficulty);
			if (openedDifficulty.get_HasValue())
			{
				int value = (int)openedDifficulty.get_Value();
				this.rewardMessages[0].isNeedShow = true;
				this.DifficltyTex.spriteName = "txt_diff" + value;
			}
			this.rewardMessages[1].isNeedShow = true;
		}

		private void OnDestroy()
		{
			this.myUIWidget = null;
			this.rewardMessages = null;
			this.NowRewardMessage = null;
			this.key = null;
			this.DifficltyTex = null;
		}
	}
}
