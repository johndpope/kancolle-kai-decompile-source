using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UniRx
{
	[Serializable]
	public class ReactiveCollection<T> : Collection<T>
	{
		[NonSerialized]
		private Subject<int> countChanged;

		[NonSerialized]
		private Subject<Unit> collectionReset;

		[NonSerialized]
		private Subject<CollectionAddEvent<T>> collectionAdd;

		[NonSerialized]
		private Subject<CollectionMoveEvent<T>> collectionMove;

		[NonSerialized]
		private Subject<CollectionRemoveEvent<T>> collectionRemove;

		[NonSerialized]
		private Subject<CollectionReplaceEvent<T>> collectionReplace;

		public ReactiveCollection()
		{
		}

		public ReactiveCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			using (IEnumerator<T> enumerator = collection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T current = enumerator.get_Current();
					this.Add(current);
				}
			}
		}

		public ReactiveCollection(List<T> list) : base((list == null) ? null : new List<T>(list))
		{
		}

		protected override void ClearItems()
		{
			int count = this.get_Count();
			base.ClearItems();
			if (this.collectionReset != null)
			{
				this.collectionReset.OnNext(Unit.Default);
			}
			if (count > 0 && this.countChanged != null)
			{
				this.countChanged.OnNext(this.get_Count());
			}
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			if (this.collectionAdd != null)
			{
				this.collectionAdd.OnNext(new CollectionAddEvent<T>(index, item));
			}
			if (this.countChanged != null)
			{
				this.countChanged.OnNext(this.get_Count());
			}
		}

		public void Move(int oldIndex, int newIndex)
		{
			this.MoveItem(oldIndex, newIndex);
		}

		protected virtual void MoveItem(int oldIndex, int newIndex)
		{
			T t = this.get_Item(oldIndex);
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, t);
			if (this.collectionMove != null)
			{
				this.collectionMove.OnNext(new CollectionMoveEvent<T>(oldIndex, newIndex, t));
			}
		}

		protected override void RemoveItem(int index)
		{
			T value = this.get_Item(index);
			base.RemoveItem(index);
			if (this.collectionRemove != null)
			{
				this.collectionRemove.OnNext(new CollectionRemoveEvent<T>(index, value));
			}
			if (this.countChanged != null)
			{
				this.countChanged.OnNext(this.get_Count());
			}
		}

		protected override void SetItem(int index, T item)
		{
			T oldValue = this.get_Item(index);
			base.SetItem(index, item);
			if (this.collectionReplace != null)
			{
				this.collectionReplace.OnNext(new CollectionReplaceEvent<T>(index, oldValue, item));
			}
		}

		public IObservable<int> ObserveCountChanged()
		{
			Subject<int> arg_1B_0;
			if ((arg_1B_0 = this.countChanged) == null)
			{
				arg_1B_0 = (this.countChanged = new Subject<int>());
			}
			return arg_1B_0;
		}

		public IObservable<Unit> ObserveReset()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.collectionReset) == null)
			{
				arg_1B_0 = (this.collectionReset = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public IObservable<CollectionAddEvent<T>> ObserveAdd()
		{
			Subject<CollectionAddEvent<T>> arg_1B_0;
			if ((arg_1B_0 = this.collectionAdd) == null)
			{
				arg_1B_0 = (this.collectionAdd = new Subject<CollectionAddEvent<T>>());
			}
			return arg_1B_0;
		}

		public IObservable<CollectionMoveEvent<T>> ObserveMove()
		{
			Subject<CollectionMoveEvent<T>> arg_1B_0;
			if ((arg_1B_0 = this.collectionMove) == null)
			{
				arg_1B_0 = (this.collectionMove = new Subject<CollectionMoveEvent<T>>());
			}
			return arg_1B_0;
		}

		public IObservable<CollectionRemoveEvent<T>> ObserveRemove()
		{
			Subject<CollectionRemoveEvent<T>> arg_1B_0;
			if ((arg_1B_0 = this.collectionRemove) == null)
			{
				arg_1B_0 = (this.collectionRemove = new Subject<CollectionRemoveEvent<T>>());
			}
			return arg_1B_0;
		}

		public IObservable<CollectionReplaceEvent<T>> ObserveReplace()
		{
			Subject<CollectionReplaceEvent<T>> arg_1B_0;
			if ((arg_1B_0 = this.collectionReplace) == null)
			{
				arg_1B_0 = (this.collectionReplace = new Subject<CollectionReplaceEvent<T>>());
			}
			return arg_1B_0;
		}
	}
}
