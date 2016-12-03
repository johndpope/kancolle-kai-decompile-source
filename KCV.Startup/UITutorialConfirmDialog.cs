using KCV.Utils;
using Librarys.Object;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Startup
{
	public class UITutorialConfirmDialog : AbsDialog<int, UIDialogButton>
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float showDialogOpenTime;

			public float showOverlayAlphaTime;

			public void Dispose()
			{
			}
		}

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private Transform _traDialogBackground;

		[SerializeField]
		private UITutorialConfirmDialog.Params _strParams;

		public static UITutorialConfirmDialog Instantiate(UITutorialConfirmDialog prefab, Transform parent)
		{
			UITutorialConfirmDialog uITutorialConfirmDialog = Object.Instantiate<UITutorialConfirmDialog>(prefab);
			uITutorialConfirmDialog.get_transform().set_parent(parent);
			uITutorialConfirmDialog.get_transform().localPositionZero();
			uITutorialConfirmDialog.get_transform().localScaleOne();
			uITutorialConfirmDialog.Setup();
			return uITutorialConfirmDialog;
		}

		private bool Setup()
		{
			this._uiOverlay.alpha = 0f;
			this._traDialogBackground.set_localScale(Vector3.get_one() * 0.5f);
			int cnt = 0;
			this._listButtons.ForEach(delegate(UIDialogButton x)
			{
				x.Init(cnt, true, true, 11, Util.CreateEventDelegateList(this, "OnActive", cnt), new Action(this.OnDecide));
				cnt++;
			});
			return true;
		}

		public override bool Init(Action onCancel, Action<int> onDecide)
		{
			this._actOnCancel = onCancel;
			this._actOnDecide = onDecide;
			return true;
		}

		protected override void OnUnInit()
		{
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.Del<Transform>(ref this._traDialogBackground);
		}

		protected override void PreparaNext(bool isFoward)
		{
			int currentIndex = base.currentIndex;
			base.currentIndex = Mathe.NextElement(base.currentIndex, 0, 1, isFoward);
			if (currentIndex != base.currentIndex)
			{
				this.ChangeFocus(base.currentIndex);
			}
		}

		protected override void OpenAnimation(Action onFinished)
		{
			this._traDialogBackground.LTScale(Vector3.get_one(), this._strParams.showDialogOpenTime).setEase(LeanTweenType.linear).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
			this._uiOverlay.get_transform().LTValue(this._uiOverlay.alpha, 0.5f, this._strParams.showOverlayAlphaTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiOverlay.alpha = x;
			});
		}

		protected override void CloseAimation(Action onFinished)
		{
		}

		protected override void OnChangeFocus()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		protected override void OnActive(int nIndex)
		{
			base.currentIndex = nIndex;
			this.OnDecide();
		}

		public override void OnCancel()
		{
			this._listButtons.ForEach(delegate(UIDialogButton x)
			{
				x.toggle.set_enabled(false);
			});
			base.OnCancel();
		}

		public override void OnDecide()
		{
			this._listButtons.ForEach(delegate(UIDialogButton x)
			{
				x.toggle.set_enabled(false);
			});
			Dlg.Call<int>(ref this._actOnDecide, base.currentIndex);
		}
	}
}
