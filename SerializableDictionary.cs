using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : IEnumerable, IEnumerable<KeyValuePair<TKey, TValue>>
{
	private class Enumerator : IEnumerator, IDisposable, IEnumerator<KeyValuePair<TKey, TValue>>
	{
		private readonly SerializableDictionary<TKey, TValue> Dictionary;

		private int current = -1;

		object IEnumerator.Current
		{
			get
			{
				return this.Dictionary.GetAt(this.current);
			}
		}

		public KeyValuePair<TKey, TValue> Current
		{
			get
			{
				return this.Dictionary.GetAt(this.current);
			}
		}

		public Enumerator(SerializableDictionary<TKey, TValue> dictionary)
		{
			this.Dictionary = dictionary;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			this.current++;
			return this.current < this.Dictionary.Count;
		}

		public void Reset()
		{
			this.current = -1;
		}
	}

	private Dictionary<TKey, int> Dictionary = new Dictionary<TKey, int>();

	[SerializeField]
	private List<TKey> KeysList = new List<TKey>();

	[SerializeField]
	private List<TValue> ValuesList = new List<TValue>();

	[NonSerialized]
	private bool dictionaryRestored;

	public TValue this[TKey key]
	{
		get
		{
			if (!this.dictionaryRestored)
			{
				this.RestoreDictionary();
			}
			return this.ValuesList.get_Item(this.Dictionary.get_Item(key));
		}
		set
		{
			if (!this.dictionaryRestored)
			{
				this.RestoreDictionary();
			}
			int num;
			if (this.Dictionary.TryGetValue(key, ref num))
			{
				this.ValuesList.set_Item(num, value);
			}
			else
			{
				this.Add(key, value);
			}
		}
	}

	public int Count
	{
		get
		{
			return this.ValuesList.get_Count();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new SerializableDictionary<TKey, TValue>.Enumerator(this);
	}

	public void Add(TKey key, TValue value)
	{
		this.Dictionary.Add(key, this.ValuesList.get_Count());
		this.KeysList.Add(key);
		this.ValuesList.Add(value);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return new SerializableDictionary<TKey, TValue>.Enumerator(this);
	}

	public TValue Get(TKey key, TValue defaultValue)
	{
		if (!this.dictionaryRestored)
		{
			this.RestoreDictionary();
		}
		int num;
		if (this.Dictionary.TryGetValue(key, ref num))
		{
			return this.ValuesList.get_Item(num);
		}
		return defaultValue;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		if (!this.dictionaryRestored)
		{
			this.RestoreDictionary();
		}
		int num;
		if (this.Dictionary.TryGetValue(key, ref num))
		{
			value = this.ValuesList.get_Item(num);
			return true;
		}
		value = default(TValue);
		return false;
	}

	public bool Remove(TKey key)
	{
		if (!this.dictionaryRestored)
		{
			this.RestoreDictionary();
		}
		int index;
		if (this.Dictionary.TryGetValue(key, ref index))
		{
			this.RemoveAt(index);
			return true;
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (!this.dictionaryRestored)
		{
			this.RestoreDictionary();
		}
		TKey tKey = this.KeysList.get_Item(index);
		this.Dictionary.Remove(tKey);
		this.KeysList.RemoveAt(index);
		this.ValuesList.RemoveAt(index);
		for (int i = index; i < this.KeysList.get_Count(); i++)
		{
			Dictionary<TKey, int> dictionary;
			Dictionary<TKey, int> expr_50 = dictionary = this.Dictionary;
			TKey tKey2;
			TKey expr_5E = tKey2 = this.KeysList.get_Item(i);
			int num = dictionary.get_Item(tKey2);
			expr_50.set_Item(expr_5E, num - 1);
		}
	}

	public KeyValuePair<TKey, TValue> GetAt(int index)
	{
		return new KeyValuePair<TKey, TValue>(this.KeysList.get_Item(index), this.ValuesList.get_Item(index));
	}

	public TValue GetValueAt(int index)
	{
		return this.ValuesList.get_Item(index);
	}

	public bool ContainsKey(TKey key)
	{
		if (!this.dictionaryRestored)
		{
			this.RestoreDictionary();
		}
		return this.Dictionary.ContainsKey(key);
	}

	public void Clear()
	{
		this.Dictionary.Clear();
		this.KeysList.Clear();
		this.ValuesList.Clear();
	}

	private void RestoreDictionary()
	{
		for (int i = 0; i < this.KeysList.get_Count(); i++)
		{
			this.Dictionary.set_Item(this.KeysList.get_Item(i), i);
		}
		this.dictionaryRestored = true;
	}
}
