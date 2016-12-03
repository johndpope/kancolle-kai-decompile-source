using System;
using System.Collections.Generic;
using System.Threading;

namespace UniRx.InternalUtil
{
	internal class PriorityQueue<T> where T : IComparable<T>
	{
		private struct IndexedItem : IComparable<PriorityQueue<T>.IndexedItem>
		{
			public T Value;

			public long Id;

			public int CompareTo(PriorityQueue<T>.IndexedItem other)
			{
				int num = this.Value.CompareTo(other.Value);
				if (num == 0)
				{
					num = this.Id.CompareTo(other.Id);
				}
				return num;
			}
		}

		private static long _count = -9223372036854775808L;

		private PriorityQueue<T>.IndexedItem[] _items;

		private int _size;

		public int Count
		{
			get
			{
				return this._size;
			}
		}

		public PriorityQueue() : this(16)
		{
		}

		public PriorityQueue(int capacity)
		{
			this._items = new PriorityQueue<T>.IndexedItem[capacity];
			this._size = 0;
		}

		private bool IsHigherPriority(int left, int right)
		{
			return this._items[left].CompareTo(this._items[right]) < 0;
		}

		private void Percolate(int index)
		{
			if (index >= this._size || index < 0)
			{
				return;
			}
			int num = (index - 1) / 2;
			if (num < 0 || num == index)
			{
				return;
			}
			if (this.IsHigherPriority(index, num))
			{
				PriorityQueue<T>.IndexedItem indexedItem = this._items[index];
				this._items[index] = this._items[num];
				this._items[num] = indexedItem;
				this.Percolate(num);
			}
		}

		private void Heapify()
		{
			this.Heapify(0);
		}

		private void Heapify(int index)
		{
			if (index >= this._size || index < 0)
			{
				return;
			}
			int num = 2 * index + 1;
			int num2 = 2 * index + 2;
			int num3 = index;
			if (num < this._size && this.IsHigherPriority(num, num3))
			{
				num3 = num;
			}
			if (num2 < this._size && this.IsHigherPriority(num2, num3))
			{
				num3 = num2;
			}
			if (num3 != index)
			{
				PriorityQueue<T>.IndexedItem indexedItem = this._items[index];
				this._items[index] = this._items[num3];
				this._items[num3] = indexedItem;
				this.Heapify(num3);
			}
		}

		public T Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("HEAP is Empty");
			}
			return this._items[0].Value;
		}

		private void RemoveAt(int index)
		{
			this._items[index] = this._items[--this._size];
			this._items[this._size] = default(PriorityQueue<T>.IndexedItem);
			this.Heapify();
			if (this._size < this._items.Length / 4)
			{
				PriorityQueue<T>.IndexedItem[] items = this._items;
				this._items = new PriorityQueue<T>.IndexedItem[this._items.Length / 2];
				Array.Copy(items, 0, this._items, 0, this._size);
			}
		}

		public T Dequeue()
		{
			T result = this.Peek();
			this.RemoveAt(0);
			return result;
		}

		public void Enqueue(T item)
		{
			if (this._size >= this._items.Length)
			{
				PriorityQueue<T>.IndexedItem[] items = this._items;
				this._items = new PriorityQueue<T>.IndexedItem[this._items.Length * 2];
				Array.Copy(items, this._items, items.Length);
			}
			int num = this._size++;
			this._items[num] = new PriorityQueue<T>.IndexedItem
			{
				Value = item,
				Id = Interlocked.Increment(ref PriorityQueue<T>._count)
			};
			this.Percolate(num);
		}

		public bool Remove(T item)
		{
			for (int i = 0; i < this._size; i++)
			{
				if (EqualityComparer<T>.get_Default().Equals(this._items[i].Value, item))
				{
					this.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
	}
}
