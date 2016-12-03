using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdCloud : BaseAnimation
	{
		public enum AnimationList
		{
			ProdCloudIn,
			ProdCloudInNotFound,
			ProdCloudOut
		}

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private List<UITexture> _listClouds;

		public static ProdCloud Instantiate(ProdCloud prefab, Transform parent)
		{
			ProdCloud prodCloud = Object.Instantiate<ProdCloud>(prefab);
			prodCloud.get_transform().set_parent(parent);
			prodCloud.get_transform().localPositionZero();
			prodCloud.get_transform().localScaleZero();
			prodCloud.Ctor();
			return prodCloud;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelListSafe<UITexture>(ref this._listClouds);
		}

		private bool Ctor()
		{
			if (this._uiPanel == null)
			{
				this._uiPanel = base.GetComponent<UIPanel>();
			}
			if (this._listClouds == null)
			{
				this._listClouds.Add(base.get_transform().FindChild("CloudFront").GetComponent<UITexture>());
				this._listClouds.Add(base.get_transform().FindChild("CloudBack").GetComponent<UITexture>());
			}
			return true;
		}

		public void SetPanelDepth(int nDepth)
		{
			this._uiPanel.depth = nDepth;
		}

		public override void Play(Enum iEnum, Action forceCallback, Action callback)
		{
			base.get_transform().localScaleOne();
			base.Play(iEnum, forceCallback, callback);
		}

		protected override void onAnimationFinished()
		{
			base.onAnimationFinished();
		}
	}
}
