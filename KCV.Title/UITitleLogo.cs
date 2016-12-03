using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class UITitleLogo : MonoBehaviour
	{
		[Serializable]
		private struct Params
		{
			[SerializeField]
			private float _fShowTime;

			[SerializeField]
			private float _fHideTime;

			[SerializeField]
			private float _fMinAlpha;

			[SerializeField]
			private float _fMaxAlpha;

			[SerializeField]
			private float _fAddTime;

			[SerializeField]
			private float _fSubTime;

			[SerializeField]
			private LeanTweenType _iAddEase;

			[SerializeField]
			private LeanTweenType _iSubEase;

			public float showTime
			{
				get
				{
					return this._fShowTime;
				}
			}

			public float hideTime
			{
				get
				{
					return this._fHideTime;
				}
			}

			public float minAlpha
			{
				get
				{
					return Mathe.Rate(0f, 255f, this._fMinAlpha);
				}
			}

			public float maxAlpha
			{
				get
				{
					return Mathe.Rate(0f, 255f, this._fMaxAlpha);
				}
			}

			public float addTime
			{
				get
				{
					return this._fAddTime;
				}
			}

			public float subTime
			{
				get
				{
					return this._fSubTime;
				}
			}

			public LeanTweenType addEase
			{
				get
				{
					return this._iAddEase;
				}
			}

			public LeanTweenType subEase
			{
				get
				{
					return this._iSubEase;
				}
			}

			public Params(float showTime, float hideTime, float minAlpha, float maxAlpha, float addTime, float subTime, LeanTweenType addEase, LeanTweenType subEase)
			{
				this._fShowTime = showTime;
				this._fHideTime = hideTime;
				this._fMinAlpha = minAlpha;
				this._fMaxAlpha = maxAlpha;
				this._fAddTime = addTime;
				this._fSubTime = subTime;
				this._iAddEase = addEase;
				this._iSubEase = subEase;
			}
		}

		[SerializeField]
		private UITexture _uiLogo;

		[Header("[Logo Animation Param]"), SerializeField]
		private UITitleLogo.Params _strParams;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private void Awake()
		{
			this.panel.alpha = 0f;
		}

		private void OnDestroy()
		{
			this._uiLogo.get_transform().LTCancel();
			this.panel.get_transform().LTCancel();
			Mem.Del<UITexture>(ref this._uiLogo);
			Mem.Del<UITitleLogo.Params>(ref this._strParams);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		public void StartLogoAnim()
		{
			this._uiLogo.get_transform().LTValue(this._strParams.maxAlpha, this._strParams.minAlpha, this._strParams.subTime).setEase(this._strParams.subEase).setOnUpdate(delegate(float x)
			{
				this._uiLogo.alpha = x;
			}).setOnComplete(delegate
			{
				this._uiLogo.get_transform().LTValue(this._strParams.minAlpha, this._strParams.maxAlpha, this._strParams.addTime).setOnUpdate(delegate(float x)
				{
					this._uiLogo.alpha = x;
				}).setEase(this._strParams.addEase).setOnComplete(new Action(this.StartLogoAnim));
			});
		}

		public LTDescr Show()
		{
			this.panel.alpha = 0f;
			return this.panel.get_transform().LTValue(this.panel.alpha, 1f, this._strParams.showTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		public LTDescr Hide()
		{
			return this.panel.get_transform().LTValue(this.panel.alpha, 0f, this._strParams.hideTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				this._uiLogo.get_transform().LTCancel();
				this._uiLogo.alpha = this._strParams.maxAlpha;
			});
		}
	}
}
