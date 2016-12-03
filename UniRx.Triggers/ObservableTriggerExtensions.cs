using System;
using UnityEngine;

namespace UniRx.Triggers
{
	public static class ObservableTriggerExtensions
	{
		public static IObservable<int> OnAnimatorIKAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<int>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(component.get_gameObject()).OnAnimatorIKAsObservable();
		}

		public static IObservable<Unit> OnAnimatorMoveAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(component.get_gameObject()).OnAnimatorMoveAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionEnter2DAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(component.get_gameObject()).OnCollisionEnter2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionExit2DAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(component.get_gameObject()).OnCollisionExit2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionStay2DAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(component.get_gameObject()).OnCollisionStay2DAsObservable();
		}

		public static IObservable<Collision> OnCollisionEnterAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(component.get_gameObject()).OnCollisionEnterAsObservable();
		}

		public static IObservable<Collision> OnCollisionExitAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(component.get_gameObject()).OnCollisionExitAsObservable();
		}

		public static IObservable<Collision> OnCollisionStayAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(component.get_gameObject()).OnCollisionStayAsObservable();
		}

		public static IObservable<Unit> OnDestroyAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDestroyTrigger>(component.get_gameObject()).OnDestroyAsObservable();
		}

		public static IObservable<Unit> OnEnableAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(component.get_gameObject()).OnEnableAsObservable();
		}

		public static IObservable<Unit> OnDisableAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(component.get_gameObject()).OnDisableAsObservable();
		}

		public static IObservable<Unit> FixedUpdateAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableFixedUpdateTrigger>(component.get_gameObject()).FixedUpdateAsObservable();
		}

		public static IObservable<Unit> LateUpdateAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableLateUpdateTrigger>(component.get_gameObject()).LateUpdateAsObservable();
		}

		public static IObservable<Unit> OnMouseDownAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseDownAsObservable();
		}

		public static IObservable<Unit> OnMouseDragAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseDragAsObservable();
		}

		public static IObservable<Unit> OnMouseEnterAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseEnterAsObservable();
		}

		public static IObservable<Unit> OnMouseExitAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseExitAsObservable();
		}

		public static IObservable<Unit> OnMouseOverAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseOverAsObservable();
		}

		public static IObservable<Unit> OnMouseUpAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseUpAsObservable();
		}

		public static IObservable<Unit> OnMouseUpAsButtonAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(component.get_gameObject()).OnMouseUpAsButtonAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerEnter2DAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(component.get_gameObject()).OnTriggerEnter2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerExit2DAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(component.get_gameObject()).OnTriggerExit2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerStay2DAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(component.get_gameObject()).OnTriggerStay2DAsObservable();
		}

		public static IObservable<Collider> OnTriggerEnterAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(component.get_gameObject()).OnTriggerEnterAsObservable();
		}

		public static IObservable<Collider> OnTriggerExitAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(component.get_gameObject()).OnTriggerExitAsObservable();
		}

		public static IObservable<Collider> OnTriggerStayAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(component.get_gameObject()).OnTriggerStayAsObservable();
		}

		public static IObservable<Unit> UpdateAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableUpdateTrigger>(component.get_gameObject()).UpdateAsObservable();
		}

		public static IObservable<Unit> OnBecameInvisibleAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(component.get_gameObject()).OnBecameInvisibleAsObservable();
		}

		public static IObservable<Unit> OnBecameVisibleAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(component.get_gameObject()).OnBecameVisibleAsObservable();
		}

		public static IObservable<Unit> OnBeforeTransformParentChangedAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(component.get_gameObject()).OnBeforeTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformParentChangedAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(component.get_gameObject()).OnTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformChildrenChangedAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(component.get_gameObject()).OnTransformChildrenChangedAsObservable();
		}

		public static IObservable<Unit> OnCanvasGroupChangedAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCanvasGroupChangedTrigger>(component.get_gameObject()).OnCanvasGroupChangedAsObservable();
		}

		public static IObservable<Unit> OnRectTransformDimensionsChangeAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(component.get_gameObject()).OnRectTransformDimensionsChangeAsObservable();
		}

		public static IObservable<Unit> OnRectTransformRemovedAsObservable(this Component component)
		{
			if (component == null || component.get_gameObject() == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(component.get_gameObject()).OnRectTransformRemovedAsObservable();
		}

		public static IObservable<int> OnAnimatorIKAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<int>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(gameObject).OnAnimatorIKAsObservable();
		}

		public static IObservable<Unit> OnAnimatorMoveAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(gameObject).OnAnimatorMoveAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionEnter2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(gameObject).OnCollisionEnter2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionExit2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(gameObject).OnCollisionExit2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionStay2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(gameObject).OnCollisionStay2DAsObservable();
		}

		public static IObservable<Collision> OnCollisionEnterAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(gameObject).OnCollisionEnterAsObservable();
		}

		public static IObservable<Collision> OnCollisionExitAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(gameObject).OnCollisionExitAsObservable();
		}

		public static IObservable<Collision> OnCollisionStayAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(gameObject).OnCollisionStayAsObservable();
		}

		public static IObservable<Unit> OnDestroyAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDestroyTrigger>(gameObject).OnDestroyAsObservable();
		}

		public static IObservable<Unit> OnEnableAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(gameObject).OnEnableAsObservable();
		}

		public static IObservable<Unit> OnDisableAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(gameObject).OnDisableAsObservable();
		}

		public static IObservable<Unit> FixedUpdateAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableFixedUpdateTrigger>(gameObject).FixedUpdateAsObservable();
		}

		public static IObservable<Unit> LateUpdateAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableLateUpdateTrigger>(gameObject).LateUpdateAsObservable();
		}

		public static IObservable<Unit> OnMouseDownAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseDownAsObservable();
		}

		public static IObservable<Unit> OnMouseDragAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseDragAsObservable();
		}

		public static IObservable<Unit> OnMouseEnterAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseEnterAsObservable();
		}

		public static IObservable<Unit> OnMouseExitAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseExitAsObservable();
		}

		public static IObservable<Unit> OnMouseOverAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseOverAsObservable();
		}

		public static IObservable<Unit> OnMouseUpAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseUpAsObservable();
		}

		public static IObservable<Unit> OnMouseUpAsButtonAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMouseTrigger>(gameObject).OnMouseUpAsButtonAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerEnter2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(gameObject).OnTriggerEnter2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerExit2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(gameObject).OnTriggerExit2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerStay2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(gameObject).OnTriggerStay2DAsObservable();
		}

		public static IObservable<Collider> OnTriggerEnterAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(gameObject).OnTriggerEnterAsObservable();
		}

		public static IObservable<Collider> OnTriggerExitAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(gameObject).OnTriggerExitAsObservable();
		}

		public static IObservable<Collider> OnTriggerStayAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(gameObject).OnTriggerStayAsObservable();
		}

		public static IObservable<Unit> UpdateAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableUpdateTrigger>(gameObject).UpdateAsObservable();
		}

		public static IObservable<Unit> OnBecameInvisibleAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(gameObject).OnBecameInvisibleAsObservable();
		}

		public static IObservable<Unit> OnBecameVisibleAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(gameObject).OnBecameVisibleAsObservable();
		}

		public static IObservable<Unit> OnBeforeTransformParentChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(gameObject).OnBeforeTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformParentChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(gameObject).OnTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformChildrenChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(gameObject).OnTransformChildrenChangedAsObservable();
		}

		public static IObservable<Unit> OnCanvasGroupChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCanvasGroupChangedTrigger>(gameObject).OnCanvasGroupChangedAsObservable();
		}

		public static IObservable<Unit> OnRectTransformDimensionsChangeAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(gameObject).OnRectTransformDimensionsChangeAsObservable();
		}

		public static IObservable<Unit> OnRectTransformRemovedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(gameObject).OnRectTransformRemovedAsObservable();
		}

		private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
		{
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}
