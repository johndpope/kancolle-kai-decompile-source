using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionNavigation : MonoBehaviour
	{
		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UIHowTo _uiHowTo;

		[Header("[Animation Properties]"), SerializeField]
		private Vector3 _vShowPos = new Vector3(480f, -238f, 0f);

		[SerializeField]
		private Vector3 _vHidePos = new Vector3(1000f, -238f, 0f);

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static UIRebellionNavigation Instantiate(UIRebellionNavigation prefab, Transform parent, CtrlRebellionOrganize.RebellionOrganizeMode iMode)
		{
			UIRebellionNavigation uIRebellionNavigation = Object.Instantiate<UIRebellionNavigation>(prefab);
			uIRebellionNavigation.get_transform().set_parent(parent);
			uIRebellionNavigation.get_transform().set_localPosition(uIRebellionNavigation._vHidePos);
			uIRebellionNavigation.get_transform().localScaleOne();
			uIRebellionNavigation.SetNavigation(iMode);
			return uIRebellionNavigation;
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<UIHowTo>(ref this._uiHowTo);
			Mem.Del<Vector3>(ref this._vShowPos);
			Mem.Del<Vector3>(ref this._vShowPos);
		}

		public void SetNavigation(CtrlRebellionOrganize.RebellionOrganizeMode iMode)
		{
			DebugUtils.Log("反抗編成ナビ設定:" + iMode);
			List<UIHowToDetail> list = new List<UIHowToDetail>();
			this.panel.widgetsAreStatic = false;
			if (iMode != CtrlRebellionOrganize.RebellionOrganizeMode.Main)
			{
				if (iMode == CtrlRebellionOrganize.RebellionOrganizeMode.Detail)
				{
					list.Add(new UIHowToDetail(HowToKey.btn_sankaku, "戻る"));
					list.Add(new UIHowToDetail(HowToKey.btn_maru, "決定"));
				}
			}
			else
			{
				list.Add(new UIHowToDetail(HowToKey.btn_sankaku, "詳細をみる"));
				list.Add(new UIHowToDetail(HowToKey.btn_batsu, "出撃中止"));
				list.Add(new UIHowToDetail(HowToKey.btn_shikaku, "はずす"));
				list.Add(new UIHowToDetail(HowToKey.btn_maru, "決定"));
			}
			this._uiHowTo.Refresh(list.ToArray());
			this.panel.widgetsAreStatic = true;
		}

		public void Show(Action onFinished)
		{
			this.panel.get_transform().LTCancel();
			this.panel.widgetsAreStatic = false;
			this.panel.get_transform().LTValue(this.panel.alpha, 1f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
			this.panel.get_transform().LTMoveLocal(this._vShowPos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			this.panel.get_transform().LTCancel();
			this.panel.get_transform().LTValue(this.panel.alpha, 0f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
			this.panel.get_transform().LTMoveLocal(this._vShowPos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete(delegate
			{
				this.panel.widgetsAreStatic = true;
				Dlg.Call(ref onFinished);
			});
		}
	}
}
