using System;
using UnityEngine;

namespace KCV.Organize
{
	public class ShipBannerDragDrop : UIDragDropItem
	{
		public delegate bool OnDragEndCallBackDele(int index);

		[SerializeField]
		private UIPanel BannerPanel;

		private int OriginalDepth;

		private Vector3 OriginalPosition;

		[SerializeField]
		private OrganizeBannerManager BannerManager;

		private Action<int> OnDragStartCallBack;

		private ShipBannerDragDrop.OnDragEndCallBackDele OnDragEndCallBack;

		private Action<int> OnDragAnimationEndCallBack;

		public BoxCollider2D col
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.col = base.GetComponent<BoxCollider2D>();
			base.set_enabled(false);
			this.BannerPanel = base.get_transform().FindChild("ShipFrame").GetComponent<UIPanel>();
			this.BannerManager = base.GetComponent<OrganizeBannerManager>();
		}

		protected override void OnDragStart()
		{
		}

		protected override void OnDragEnd()
		{
			if (this.OnDragEndCallBack(this.BannerManager.number))
			{
				Util.MoveTo(base.get_gameObject(), 0.3f, this.OriginalPosition, iTween.EaseType.easeOutQuint);
				this.DelayAction(0.3f, delegate
				{
					this.BannerPanel.depth = this.OriginalDepth;
					base.OnDragEnd();
					this.OnDragAnimationEndCallBack.Invoke(this.BannerManager.number);
				});
				this.BannerManager.UpdateBanner(true);
			}
		}

		public void setDefaultPosition(Vector2 pos)
		{
			this.OriginalPosition = pos;
		}

		public void setOnDragStartCallBack(Action<int> CallBack)
		{
			this.OnDragStartCallBack = CallBack;
		}

		public void setOnDragEndCallBack(ShipBannerDragDrop.OnDragEndCallBackDele CallBack)
		{
			this.OnDragEndCallBack = CallBack;
		}

		public void setOnDragAnimationEndCallBack(Action<int> CallBack)
		{
			this.OnDragAnimationEndCallBack = CallBack;
		}

		public void setColliderEnable(bool isEnable)
		{
			this.col.set_enabled(isEnable);
		}

		private void OnDestroy()
		{
			this.BannerManager = null;
			this.BannerPanel = null;
			this.col = null;
			this.OnDragStartCallBack = null;
			this.OnDragEndCallBack = null;
		}
	}
}
