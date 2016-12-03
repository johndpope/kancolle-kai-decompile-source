using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public sealed class CompositeDisposable : IDisposable, ICollection<IDisposable>, ICancelable, IEnumerable, IEnumerable<IDisposable>
	{
		private const int SHRINK_THRESHOLD = 64;

		private readonly object _gate = new object();

		private bool _disposed;

		private List<IDisposable> _disposables;

		private int _count;

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		public CompositeDisposable()
		{
			this._disposables = new List<IDisposable>();
		}

		public CompositeDisposable(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this._disposables = new List<IDisposable>(capacity);
		}

		public CompositeDisposable(params IDisposable[] disposables)
		{
			if (disposables == null)
			{
				throw new ArgumentNullException("disposables");
			}
			this._disposables = new List<IDisposable>(disposables);
			this._count = this._disposables.get_Count();
		}

		public CompositeDisposable(IEnumerable<IDisposable> disposables)
		{
			if (disposables == null)
			{
				throw new ArgumentNullException("disposables");
			}
			this._disposables = new List<IDisposable>(disposables);
			this._count = this._disposables.get_Count();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(IDisposable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			bool flag = false;
			object gate = this._gate;
			lock (gate)
			{
				flag = this._disposed;
				if (!this._disposed)
				{
					this._disposables.Add(item);
					this._count++;
				}
			}
			if (flag)
			{
				item.Dispose();
			}
		}

		public bool Remove(IDisposable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			bool flag = false;
			object gate = this._gate;
			lock (gate)
			{
				if (!this._disposed)
				{
					int num = this._disposables.IndexOf(item);
					if (num >= 0)
					{
						flag = true;
						this._disposables.set_Item(num, null);
						this._count--;
						if (this._disposables.get_Capacity() > 64 && this._count < this._disposables.get_Capacity() / 2)
						{
							List<IDisposable> disposables = this._disposables;
							this._disposables = new List<IDisposable>(this._disposables.get_Capacity() / 2);
							using (List<IDisposable>.Enumerator enumerator = disposables.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									IDisposable current = enumerator.get_Current();
									if (current != null)
									{
										this._disposables.Add(current);
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				item.Dispose();
			}
			return flag;
		}

		public void Dispose()
		{
			IDisposable[] array = null;
			object gate = this._gate;
			lock (gate)
			{
				if (!this._disposed)
				{
					this._disposed = true;
					array = this._disposables.ToArray();
					this._disposables.Clear();
					this._count = 0;
				}
			}
			if (array != null)
			{
				IDisposable[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					IDisposable disposable = array2[i];
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		public void Clear()
		{
			IDisposable[] array = null;
			object gate = this._gate;
			lock (gate)
			{
				array = this._disposables.ToArray();
				this._disposables.Clear();
				this._count = 0;
			}
			IDisposable[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				IDisposable disposable = array2[i];
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public bool Contains(IDisposable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			object gate = this._gate;
			bool result;
			lock (gate)
			{
				result = this._disposables.Contains(item);
			}
			return result;
		}

		public void CopyTo(IDisposable[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0 || arrayIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			object gate = this._gate;
			lock (gate)
			{
				List<IDisposable> list = new List<IDisposable>();
				using (List<IDisposable>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDisposable current = enumerator.get_Current();
						if (current != null)
						{
							list.Add(current);
						}
					}
				}
				Array.Copy(list.ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
			}
		}

		public IEnumerator<IDisposable> GetEnumerator()
		{
			List<IDisposable> list = new List<IDisposable>();
			object gate = this._gate;
			lock (gate)
			{
				using (List<IDisposable>.Enumerator enumerator = this._disposables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDisposable current = enumerator.get_Current();
						if (current != null)
						{
							list.Add(current);
						}
					}
				}
			}
			return list.GetEnumerator();
		}
	}
}
