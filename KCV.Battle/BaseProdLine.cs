using System;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseProdLine : MonoBehaviour
	{
		public enum AnimationName
		{
			ProdLine,
			ProdTripleLine,
			ProdSuccessiveLine,
			ProdNormalAttackLine,
			ProdAircraftAttackLine
		}

		protected Action _actCallback;

		protected bool _isFinished;

		protected UITexture _uiOverlay;

		protected virtual void OnDestroy()
		{
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<bool>(ref this._isFinished);
			Mem.Del<UITexture>(ref this._uiOverlay);
		}

		public virtual void Play(Action callback)
		{
			this._isFinished = false;
			this._actCallback = callback;
			base.GetComponent<Animation>().Play(BaseProdLine.AnimationName.ProdLine.ToString());
		}

		public virtual void Play(BaseProdLine.AnimationName iName, Action callback)
		{
			this._isFinished = false;
			this._actCallback = callback;
			base.GetComponent<Animation>().Play(iName.ToString());
		}

		protected virtual void onFinished()
		{
			this._isFinished = true;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
