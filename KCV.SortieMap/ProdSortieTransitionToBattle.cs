using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.SortieMap
{
	[ExecuteInEditMode, RequireComponent(typeof(UIPanel))]
	public class ProdSortieTransitionToBattle : BaseAnimation
	{
		public enum AnimationName
		{
			ProdCloudOutToBattle,
			ProdSortieTransitionToBattleFadeIn
		}

		[SerializeField]
		private UIWidget _uiBattleStart;

		[SerializeField]
		private List<UISprite> _listLabels;

		[SerializeField]
		private List<UITexture> _listClouds;

		[SerializeField]
		private UISprite _uiLine;

		[SerializeField]
		private float _fLabelWidth = 22f;

		private UIPanel _uiPanel;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdSortieTransitionToBattle Instantiate(ProdSortieTransitionToBattle prefab, Transform parent)
		{
			ProdSortieTransitionToBattle prodSortieTransitionToBattle = Object.Instantiate<ProdSortieTransitionToBattle>(prefab);
			prodSortieTransitionToBattle.get_transform().set_parent(parent);
			prodSortieTransitionToBattle.get_transform().localPositionZero();
			prodSortieTransitionToBattle.get_transform().localScaleZero();
			prodSortieTransitionToBattle.Setup();
			return prodSortieTransitionToBattle;
		}

		private void LateUpdate()
		{
			if (base.animation.get_isPlaying())
			{
				this._uiLine.width = (int)this._fLabelWidth;
			}
		}

		protected override void OnDestroy()
		{
			Mem.Del<UIWidget>(ref this._uiBattleStart);
			if (this._listLabels != null)
			{
				this._listLabels.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe<UISprite>(ref this._listLabels);
			Mem.DelListSafe<UITexture>(ref this._listClouds);
			Mem.Del(ref this._uiLine);
			Mem.Del<float>(ref this._fLabelWidth);
		}

		private bool Setup()
		{
			this.panel.depth = 20;
			this._uiBattleStart.alpha = 0.01f;
			if (this._listClouds == null)
			{
				this._listClouds = new List<UITexture>();
				this._listClouds.Add(base.get_transform().FindChild("CloudBack").GetComponent<UITexture>());
				this._listClouds.Add(base.get_transform().FindChild("CloudFront").GetComponent<UITexture>());
			}
			this.panel.widgetsAreStatic = true;
			return true;
		}

		public ProdSortieTransitionToBattle QuickFadeInInit()
		{
			this.panel.alpha = 1f;
			this._uiLine.alpha = 0f;
			this._uiBattleStart.alpha = 0f;
			this._uiBattleStart.get_transform().localScaleZero();
			this._listClouds.get_Item(0).get_transform().localScaleOne();
			this._listClouds.get_Item(0).alpha = 1f;
			this._listClouds.get_Item(0).uvRect = new Rect(1f, 0f, 1f, 1f);
			this._listClouds.get_Item(1).get_transform().set_localScale(Vector3.get_one() * 1.25f);
			this._listClouds.get_Item(1).alpha = 1f;
			this._listClouds.get_Item(1).uvRect = new Rect(0.5f, 0f, 1f, 1f);
			base.get_transform().localScaleOne();
			return this;
		}

		public override void Play(Action callback)
		{
			this.Play(ProdSortieTransitionToBattle.AnimationName.ProdCloudOutToBattle, callback);
		}

		public void Play(ProdSortieTransitionToBattle.AnimationName iName, Action onFinished)
		{
			this.panel.widgetsAreStatic = false;
			base.get_transform().localScaleOne();
			base.Play(iName, onFinished);
		}
	}
}
