using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class BaseDialogPopup
	{
		private UITexture BG;

		public void setBG(UITexture bg)
		{
			this.BG = bg;
		}

		public virtual bool Init(ShipModel ship, UITexture _texture)
		{
			return true;
		}

		public virtual void Open(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localScale = new Vector3(fromX, fromY);
			obj.get_transform().set_localScale(localScale);
			iTween.ScaleTo(obj, iTween.Hash(new object[]
			{
				"islocal",
				true,
				"x",
				toX,
				"y",
				toY,
				"z",
				1f,
				"time",
				0.4f,
				"easetype",
				iTween.EaseType.easeOutBack
			}));
			obj.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, obj, string.Empty);
			if (this.BG != null)
			{
				TweenAlpha.Begin(this.BG.get_gameObject(), 0.2f, 0.8f);
			}
		}

		public static void Open2(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localScale = new Vector3(fromX, fromY);
			obj.get_transform().set_localScale(localScale);
			iTween.ScaleTo(obj, iTween.Hash(new object[]
			{
				"islocal",
				true,
				"x",
				toX,
				"y",
				toY,
				"z",
				1f,
				"time",
				0.4f,
				"easetype",
				iTween.EaseType.easeOutBack
			}));
			obj.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, obj, string.Empty);
		}

		public virtual void Close(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localScale = new Vector3(fromX, fromY);
			obj.get_transform().set_localScale(localScale);
			iTween.ScaleTo(obj, iTween.Hash(new object[]
			{
				"islocal",
				true,
				"x",
				toX,
				"y",
				toY,
				"z",
				1f,
				"time",
				0.4f,
				"easetype",
				iTween.EaseType.linear
			}));
			if (this.BG != null)
			{
				TweenAlpha.Begin(this.BG.get_gameObject(), 0.4f, 0f);
			}
		}

		public static void Close(GameObject obj, float duration, UITweener.Method _tween)
		{
			obj.SafeGetTweenAlpha(1f, 0f, duration, 0f, UITweener.Method.Linear, UITweener.Style.Once, obj, string.Empty);
		}

		public virtual void Open(GameObject obj, Vector3 from, Vector3 to)
		{
			this.Open(obj, from.x, from.y, to.x, to.y);
		}

		public virtual void Close(GameObject obj, Vector3 from, Vector3 to)
		{
			this.Close(obj, from.x, from.y, to.x, to.y);
		}
	}
}
