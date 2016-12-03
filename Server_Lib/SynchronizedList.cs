using System;

namespace Server_Lib
{
	public class SynchronizedList<T> : CustomCollectionBase<T>
	{
		public override T this[int index]
		{
			get
			{
				return (T)((object)base.get_List().get_Item(index));
			}
			set
			{
				object syncRoot = base.get_List().get_SyncRoot();
				lock (syncRoot)
				{
					base.get_List().set_Item(index, value);
				}
			}
		}

		public override int Add(T item)
		{
			object syncRoot = base.get_List().get_SyncRoot();
			int result;
			lock (syncRoot)
			{
				result = base.get_List().Add(item);
			}
			return result;
		}

		public override void AddRange(T[] items)
		{
			object syncRoot = base.get_List().get_SyncRoot();
			lock (syncRoot)
			{
				for (int i = 0; i < items.Length; i++)
				{
					T item = items[i];
					this.Add(item);
				}
			}
		}

		public override int IndexOf(T item)
		{
			object syncRoot = base.get_List().get_SyncRoot();
			int result;
			lock (syncRoot)
			{
				result = base.get_List().IndexOf(item);
			}
			return result;
		}

		public override void Insert(int index, T value)
		{
			object syncRoot = base.get_List().get_SyncRoot();
			lock (syncRoot)
			{
				base.get_List().Insert(index, value);
			}
		}

		public override void Remove(T value)
		{
			object syncRoot = base.get_List().get_SyncRoot();
			lock (syncRoot)
			{
				base.get_List().Remove(value);
			}
		}

		public override bool Contains(T value)
		{
			return base.get_List().Contains(value);
		}
	}
}
