using System;

namespace KCV.Furniture
{
	public abstract class UIDynamicFurniture : UIFurniture
	{
		private Action<UIDynamicFurniture> mOnActionEvent;

		public void SetOnActionEvent(Action<UIDynamicFurniture> onActionEvent)
		{
			this.mOnActionEvent = onActionEvent;
		}

		[Obsolete("Inspectorで設定して使用します。")]
		public void OnTouchActionEvent()
		{
			this.OnActionEvent();
		}

		protected void OnActionEvent()
		{
			if (this.mOnActionEvent != null)
			{
				this.mOnActionEvent.Invoke(this);
			}
			this.OnCalledActionEvent();
		}

		protected virtual void OnCalledActionEvent()
		{
		}

		protected override void OnDestroyEvent()
		{
			this.mOnActionEvent = null;
		}
	}
}
