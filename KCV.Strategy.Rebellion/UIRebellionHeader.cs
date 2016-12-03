using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionHeader : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private List<Animation> _listGearAnimation;

		[SerializeField]
		private UILabel _uiLabel;

		[Header("[Animation Properties]"), SerializeField]
		private Vector3 _vShowPos = new Vector3(171f, 233f, 0f);

		[SerializeField]
		private Vector3 _vHidePos = new Vector3(171f, 315f, 0f);

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static UIRebellionHeader Instantiate(UIRebellionHeader prefab, Transform parent)
		{
			UIRebellionHeader uIRebellionHeader = Object.Instantiate<UIRebellionHeader>(prefab);
			uIRebellionHeader.get_transform().set_parent(parent);
			uIRebellionHeader.get_transform().localPositionZero();
			uIRebellionHeader.get_transform().localScaleOne();
			uIRebellionHeader.Init();
			return uIRebellionHeader;
		}

		private bool Init()
		{
			this._listGearAnimation.ForEach(delegate(Animation x)
			{
				x.Stop();
			});
			base.get_transform().set_localPosition(this._vHidePos);
			this.panel.alpha = 0f;
			this.panel.widgetsAreStatic = true;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiBackground);
			Mem.DelListSafe<Animation>(ref this._listGearAnimation);
			Mem.Del<UILabel>(ref this._uiLabel);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Vector3>(ref this._vShowPos);
			Mem.Del<Vector3>(ref this._vHidePos);
		}

		public void Show(Action onFinished)
		{
			this.panel.widgetsAreStatic = false;
			this._listGearAnimation.ForEach(delegate(Animation x)
			{
				x.Play();
			});
			this.panel.get_transform().LTValue(this.panel.alpha, 1f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
			base.get_transform().LTMoveLocal(this._vShowPos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			this.panel.get_transform().LTValue(this.panel.alpha, 0f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
			base.get_transform().LTMoveLocal(this._vHidePos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				this._listGearAnimation.ForEach(delegate(Animation x)
				{
					x.Stop();
				});
				this.panel.widgetsAreStatic = true;
			});
		}
	}
}
