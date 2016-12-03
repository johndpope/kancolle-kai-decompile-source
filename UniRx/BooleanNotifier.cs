using System;

namespace UniRx
{
	public class BooleanNotifier : IObservable<bool>
	{
		private readonly Subject<bool> boolTrigger = new Subject<bool>();

		private bool boolValue;

		public bool Value
		{
			get
			{
				return this.boolValue;
			}
			set
			{
				this.boolValue = value;
				this.boolTrigger.OnNext(value);
			}
		}

		public BooleanNotifier(bool initialValue = false)
		{
			this.Value = initialValue;
		}

		public void TurnOn()
		{
			if (!this.Value)
			{
				this.Value = true;
			}
		}

		public void TurnOff()
		{
			if (this.Value)
			{
				this.Value = false;
			}
		}

		public void SwitchValue()
		{
			this.Value = !this.Value;
		}

		public IDisposable Subscribe(IObserver<bool> observer)
		{
			return this.boolTrigger.Subscribe(observer);
		}
	}
}
