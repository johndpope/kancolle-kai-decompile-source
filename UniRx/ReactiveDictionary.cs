using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UniRx
{
	[Serializable]
	public class ReactiveDictionary<TKey, TValue> : IEnumerable, ICollection, IDictionary, ISerializable, IDeserializationCallback, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> inner;

		[NonSerialized]
		private Subject<int> countChanged;

		[NonSerialized]
		private Subject<Unit> collectionReset;

		[NonSerialized]
		private Subject<DictionaryAddEvent<TKey, TValue>> dictionaryAdd;

		[NonSerialized]
		private Subject<DictionaryRemoveEvent<TKey, TValue>> dictionaryRemove;

		[NonSerialized]
		private Subject<DictionaryReplaceEvent<TKey, TValue>> dictionaryReplace;

		object IDictionary.Item
		{
			get
			{
				return this[(TKey)((object)key)];
			}
			set
			{
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.inner.get_IsFixedSize();
			}
		}

		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.inner.get_IsReadOnly();
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return this.inner.get_IsSynchronized();
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				return this.inner.get_Keys();
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this.inner.get_SyncRoot();
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				return this.inner.get_Values();
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return this.inner.get_IsReadOnly();
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.inner.get_Keys();
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.inner.get_Values();
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				return this.inner.get_Item(key);
			}
			set
			{
				TValue oldValue;
				if (this.TryGetValue(key, out oldValue))
				{
					this.inner.set_Item(key, value);
					if (this.dictionaryReplace != null)
					{
						this.dictionaryReplace.OnNext(new DictionaryReplaceEvent<TKey, TValue>(key, oldValue, value));
					}
				}
				else
				{
					this.inner.set_Item(key, value);
					if (this.dictionaryAdd != null)
					{
						this.dictionaryAdd.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
					}
					if (this.countChanged != null)
					{
						this.countChanged.OnNext(this.Count);
					}
				}
			}
		}

		public int Count
		{
			get
			{
				return this.inner.get_Count();
			}
		}

		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return this.inner.get_Keys();
			}
		}

		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return this.inner.get_Values();
			}
		}

		public ReactiveDictionary()
		{
			this.inner = new Dictionary<TKey, TValue>();
		}

		public ReactiveDictionary(IEqualityComparer<TKey> comparer)
		{
			this.inner = new Dictionary<TKey, TValue>(comparer);
		}

		public ReactiveDictionary(Dictionary<TKey, TValue> innerDictionary)
		{
			this.inner = innerDictionary;
		}

		void IDictionary.Add(object key, object value)
		{
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		bool IDictionary.Contains(object key)
		{
			return this.inner.Contains(key);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			this.inner.CopyTo(array, index);
		}

		void IDictionary.Remove(object key)
		{
			this.Remove((TKey)((object)key));
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.get_Key(), item.get_Value());
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.inner.Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.inner.CopyTo(array, arrayIndex);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			TValue tValue;
			if (this.TryGetValue(item.get_Key(), out tValue) && EqualityComparer<TValue>.get_Default().Equals(tValue, item.get_Value()))
			{
				this.Remove(item.get_Key());
				return true;
			}
			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		public void Add(TKey key, TValue value)
		{
			this.inner.Add(key, value);
			if (this.dictionaryAdd != null)
			{
				this.dictionaryAdd.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
			}
			if (this.countChanged != null)
			{
				this.countChanged.OnNext(this.Count);
			}
		}

		public void Clear()
		{
			int count = this.Count;
			this.inner.Clear();
			if (this.collectionReset != null)
			{
				this.collectionReset.OnNext(Unit.Default);
			}
			if (count > 0 && this.countChanged != null)
			{
				this.countChanged.OnNext(this.Count);
			}
		}

		public bool Remove(TKey key)
		{
			TValue value;
			if (this.inner.TryGetValue(key, ref value))
			{
				bool flag = this.inner.Remove(key);
				if (flag)
				{
					if (this.dictionaryRemove != null)
					{
						this.dictionaryRemove.OnNext(new DictionaryRemoveEvent<TKey, TValue>(key, value));
					}
					if (this.countChanged != null)
					{
						this.countChanged.OnNext(this.Count);
					}
				}
				return flag;
			}
			return false;
		}

		public bool ContainsKey(TKey key)
		{
			return this.inner.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.inner.TryGetValue(key, ref value);
		}

		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return this.inner.GetEnumerator();
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

		public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd()
		{
			Subject<DictionaryAddEvent<TKey, TValue>> arg_1B_0;
			if ((arg_1B_0 = this.dictionaryAdd) == null)
			{
				arg_1B_0 = (this.dictionaryAdd = new Subject<DictionaryAddEvent<TKey, TValue>>());
			}
			return arg_1B_0;
		}

		public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove()
		{
			Subject<DictionaryRemoveEvent<TKey, TValue>> arg_1B_0;
			if ((arg_1B_0 = this.dictionaryRemove) == null)
			{
				arg_1B_0 = (this.dictionaryRemove = new Subject<DictionaryRemoveEvent<TKey, TValue>>());
			}
			return arg_1B_0;
		}

		public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace()
		{
			Subject<DictionaryReplaceEvent<TKey, TValue>> arg_1B_0;
			if ((arg_1B_0 = this.dictionaryReplace) == null)
			{
				arg_1B_0 = (this.dictionaryReplace = new Subject<DictionaryReplaceEvent<TKey, TValue>>());
			}
			return arg_1B_0;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.inner.GetObjectData(info, context);
		}

		public void OnDeserialization(object sender)
		{
			this.inner.OnDeserialization(sender);
		}
	}
}
