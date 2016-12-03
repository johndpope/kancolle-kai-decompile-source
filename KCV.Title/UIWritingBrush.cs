using System;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[ExecuteInEditMode, RequireComponent(typeof(UISprite))]
	public class UIWritingBrush : MonoBehaviour
	{
		private int _nIndex;

		private UISprite _uiSprite;

		public UISprite sprite
		{
			get
			{
				return this.GetComponentThis(ref this._uiSprite);
			}
		}

		private int index
		{
			get
			{
				return this._nIndex;
			}
			set
			{
				this._nIndex = Mathe.MinMax2(value, 0, 12);
				this.sprite.spriteName = string.Format("btn_on_{0:D5}", this._nIndex + 2);
			}
		}

		public static UIWritingBrush Instantiate(UIWritingBrush prefab, Transform parent)
		{
			UIWritingBrush uIWritingBrush = Object.Instantiate<UIWritingBrush>(prefab);
			uIWritingBrush.get_transform().set_parent(parent);
			uIWritingBrush.get_transform().localScaleZero();
			uIWritingBrush.get_transform().localPositionZero();
			uIWritingBrush.Init();
			return uIWritingBrush;
		}

		private void OnDestroy()
		{
			Mem.Del<int>(ref this._nIndex);
			Mem.Del(ref this._uiSprite);
		}

		private bool Init()
		{
			this.index = 0;
			return true;
		}

		public void Play(Action onFinished)
		{
			base.get_transform().localScaleOne();
			Observable.IntervalFrame(1, FrameCountType.Update).Select(delegate(long x)
			{
				long expr_01 = x;
				x = expr_01 + 1L;
				return expr_01;
			}).TakeWhile((long x) => x <= 12L).Subscribe(delegate(long x)
			{
				this.index = (int)x;
			}, delegate
			{
				Dlg.Call(ref onFinished);
			}).AddTo(base.get_gameObject());
		}
	}
}
