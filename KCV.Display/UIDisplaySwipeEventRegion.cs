using System;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Display
{
	[RequireComponent(typeof(BoxCollider))]
	public class UIDisplaySwipeEventRegion : MonoBehaviour
	{
		public enum ActionType
		{
			None,
			Start,
			Moving,
			FingerUp,
			FingerUpWithVerticalFlick
		}

		public delegate void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime);

		[SerializeField]
		private Camera mCamera;

		private UIDisplaySwipeEventRegion.SwipeJudgeDelegate mSwipeActionJudgeCallBack;

		private BoxCollider mBoxCollider;

		private Transform mTransformCache;

		private Vector3 mVector3SwipeMoved = Vector3.get_zero();

		private int mCurrentDragIndex = -1;

		private Vector3 mLastSwipeStartWorldPosition = Vector3.get_zero();

		private Stopwatch mStopWatch;

		private bool mNeedFlickCheck;

		private UIDisplaySwipeEventRegion.SwipeJudgeDelegate mOnSwipeListener;

		private Vector3 mOnDraggingScreenToWorldPointCache = Vector3.get_zero();

		private Vector3 mMovedPercentage = Vector3.get_zero();

		public bool isDraging
		{
			get;
			private set;
		}

		public void ResetTouch()
		{
			this.isDraging = false;
		}

		private void Awake()
		{
			this.mBoxCollider = base.GetComponent<BoxCollider>();
			this.mTransformCache = base.get_transform();
			this.mStopWatch = new Stopwatch();
			if (this.mCamera != null)
			{
				UICamera component = this.mCamera.GetComponent<UICamera>();
				if (component != null)
				{
					component.allowMultiTouch = false;
				}
			}
		}

		private void OnEnable()
		{
			this.isDraging = false;
			IT_Gesture.add_onDraggingStartE(new IT_Gesture.DraggingStartHandler(this.OnDraggingStart));
			IT_Gesture.add_onDraggingE(new IT_Gesture.DraggingHandler(this.OnDragging));
			IT_Gesture.add_onDraggingEndE(new IT_Gesture.DraggingEndHandler(this.OnDraggingEnd));
		}

		private void OnDisable()
		{
			this.isDraging = false;
			IT_Gesture.remove_onDraggingStartE(new IT_Gesture.DraggingStartHandler(this.OnDraggingStart));
			IT_Gesture.remove_onDraggingE(new IT_Gesture.DraggingHandler(this.OnDragging));
			IT_Gesture.remove_onDraggingEndE(new IT_Gesture.DraggingEndHandler(this.OnDraggingEnd));
		}

		public void SetEventCatchCamera(Camera camera)
		{
			this.mCamera = camera;
			if (this.mCamera != null)
			{
				UICamera component = this.mCamera.GetComponent<UICamera>();
				if (component != null)
				{
					component.allowMultiTouch = false;
				}
			}
		}

		public void SetOnSwipeActionJudgeCallBack(UIDisplaySwipeEventRegion.SwipeJudgeDelegate swipeActionJudgeCallBack)
		{
			this.SetOnSwipeActionJudgeCallBack(swipeActionJudgeCallBack, false);
		}

		public void SetOnSwipeActionJudgeCallBack(UIDisplaySwipeEventRegion.SwipeJudgeDelegate swipeActionJudgeCallBack, bool needFlickCheck)
		{
			this.mNeedFlickCheck = needFlickCheck;
			this.mSwipeActionJudgeCallBack = swipeActionJudgeCallBack;
		}

		public void SetOnSwipeListener(UIDisplaySwipeEventRegion.SwipeJudgeDelegate onSwipeListener)
		{
			this.mOnSwipeListener = onSwipeListener;
		}

		private void OnDraggingStart(DragInfo dragInfo)
		{
			if (SingletonMonoBehaviour<UIShortCutMenu>.exist() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
			{
				return;
			}
			if (this.mCamera != null)
			{
				Ray ray = this.mCamera.ScreenPointToRay(dragInfo.pos);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity))
				{
					if (!raycastHit.get_collider().get_gameObject().get_transform().Equals(base.get_transform()))
					{
						return;
					}
					if (raycastHit.get_collider().get_transform() == this.mTransformCache)
					{
						this.mVector3SwipeMoved.Set(0f, 0f, 0f);
						this.mStopWatch.Reset();
						this.mStopWatch.Start();
						this.isDraging = true;
						this.OnSwipeEventAction(UIDisplaySwipeEventRegion.ActionType.Start, dragInfo.delta.x, dragInfo.delta.y, 0f, 0f, 0f);
						this.mLastSwipeStartWorldPosition.Set(dragInfo.pos.x, dragInfo.pos.y, 0f);
						this.mCurrentDragIndex = dragInfo.index;
					}
				}
			}
		}

		private void OnDragging(DragInfo dragInfo)
		{
			if (this.isDraging && this.mCamera != null && dragInfo.index == this.mCurrentDragIndex)
			{
				this.mOnDraggingScreenToWorldPointCache.Set(dragInfo.pos.x, dragInfo.pos.y, 0f);
				Vector3 vector = this.mCamera.ScreenToWorldPoint(this.mOnDraggingScreenToWorldPointCache);
				this.mVector3SwipeMoved.Set(this.mVector3SwipeMoved.x + dragInfo.delta.x, this.mVector3SwipeMoved.y - dragInfo.delta.y, 0f);
				Vector3 vector2 = this.GenerateMovedPercentage(this.mVector3SwipeMoved.x, this.mVector3SwipeMoved.y);
				this.OnSwipeEventAction(UIDisplaySwipeEventRegion.ActionType.Moving, dragInfo.delta.x, dragInfo.delta.y, vector2.x, vector2.y, (float)this.mStopWatch.get_ElapsedMilliseconds());
			}
		}

		private void OnDraggingEnd(DragInfo dragInfo)
		{
			if (this.isDraging && this.mCamera != null)
			{
				this.mStopWatch.Stop();
				Vector3 vector = this.GenerateMovedPercentage(this.mVector3SwipeMoved.x, this.mVector3SwipeMoved.y);
				this.OnSwipe(UIDisplaySwipeEventRegion.ActionType.FingerUp, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, (float)this.mStopWatch.get_ElapsedMilliseconds());
				if (this.mNeedFlickCheck && dragInfo.isFlick)
				{
					if (Math.Abs(vector.x) < Math.Abs(vector.y))
					{
						this.OnSwipeEventAction(UIDisplaySwipeEventRegion.ActionType.FingerUpWithVerticalFlick, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, (float)this.mStopWatch.get_ElapsedMilliseconds());
					}
					else
					{
						this.OnSwipeEventAction(UIDisplaySwipeEventRegion.ActionType.FingerUp, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, (float)this.mStopWatch.get_ElapsedMilliseconds());
					}
				}
				else
				{
					this.OnSwipeEventAction(UIDisplaySwipeEventRegion.ActionType.FingerUp, dragInfo.delta.x, dragInfo.delta.y, vector.x, vector.y, (float)this.mStopWatch.get_ElapsedMilliseconds());
				}
				if (dragInfo.index == this.mCurrentDragIndex)
				{
					this.mCurrentDragIndex = -1;
				}
				this.mVector3SwipeMoved.Set(0f, 0f, 0f);
			}
			this.isDraging = false;
		}

		private void OnSwipeEventAction(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (this.mSwipeActionJudgeCallBack != null)
			{
				this.mSwipeActionJudgeCallBack(actionType, deltaX, deltaY, movedPercentageX, movedPercentageY, elapsedTime);
			}
		}

		private void OnSwipe(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (this.mOnSwipeListener != null)
			{
				this.mOnSwipeListener(actionType, deltaX, deltaY, movedPercentageX, movedPercentageY, elapsedTime);
			}
		}

		private Vector3 GenerateMovedPercentage(float toX, float toY)
		{
			float x = this.mBoxCollider.get_size().x;
			float y = this.mBoxCollider.get_size().y;
			float num = 0f;
			if (toX != 0f)
			{
				num = Math.Abs(toX) / x;
				if (toX < 0f)
				{
					num = -num;
				}
			}
			float num2 = 0f;
			if (toY != 0f)
			{
				num2 = Math.Abs(toY) / y;
				if (toY < 0f)
				{
					num2 = -num2;
				}
			}
			this.mMovedPercentage.x = num;
			this.mMovedPercentage.y = num2;
			return this.mMovedPercentage;
		}

		internal void Release()
		{
			this.mCamera = null;
			this.mSwipeActionJudgeCallBack = null;
			this.mBoxCollider = null;
			this.mTransformCache = null;
			if (this.mStopWatch != null)
			{
				this.mStopWatch.Stop();
			}
			this.mStopWatch = null;
		}

		private void OnDestroy()
		{
			this.mCamera = null;
			this.mSwipeActionJudgeCallBack = null;
			this.mBoxCollider = null;
			this.mTransformCache = null;
			if (this.mStopWatch != null)
			{
				this.mStopWatch.Stop();
			}
			this.mStopWatch = null;
		}
	}
}
