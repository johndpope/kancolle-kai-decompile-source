using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode, RequireComponent(typeof(UIPanel))]
	public class ProdShellingLine : BaseAnimation
	{
		public enum AnimationList
		{
			ProdNormalShellingFriendLine,
			ProdNormalShellingEnemyLine
		}

		[SerializeField]
		private List<UITexture> _uiOverlayLines;

		[Range(0f, 1f), SerializeField]
		private float _fFillAmount = 1f;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
		}

		public float fillAmount
		{
			get
			{
				return this._fFillAmount;
			}
			set
			{
				if (this._fFillAmount != value)
				{
					this._fFillAmount = Mathe.MinMax2F01(value);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this.panel.widgetsAreStatic = true;
		}

		private void LateUpdate()
		{
			if (!base.animation.get_isPlaying())
			{
				return;
			}
			this._uiOverlayLines.ForEach(delegate(UITexture x)
			{
				x.fillAmount = this.fillAmount;
			});
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelList<UITexture>(ref this._uiOverlayLines);
			Mem.Del<float>(ref this._fFillAmount);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		public void Play(ProdShellingLine.AnimationList iList)
		{
			this.panel.widgetsAreStatic = false;
			base.get_transform().localScaleOne();
			base.Play(iList, null);
		}

		public void Play(bool isFriend)
		{
			UITexture component = base.get_transform().FindChild("Anchor/OverlayA").GetComponent<UITexture>();
			UITexture component2 = base.get_transform().FindChild("Anchor/OverlayC").GetComponent<UITexture>();
			Color color = (!isFriend) ? new Color(1f, 0f, 0f, component.alpha) : new Color(0f, 0.31875f, 1f, component.alpha);
			component.color = color;
			component2.color = color;
			this.Play((!isFriend) ? ProdShellingLine.AnimationList.ProdNormalShellingEnemyLine : ProdShellingLine.AnimationList.ProdNormalShellingFriendLine);
		}

		protected override void onAnimationFinished()
		{
			this.panel.widgetsAreStatic = true;
			base.get_transform().localScaleZero();
		}
	}
}
