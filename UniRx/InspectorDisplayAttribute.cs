using System;
using UnityEngine;

namespace UniRx
{
	[AttributeUsage]
	public class InspectorDisplayAttribute : PropertyAttribute
	{
		public string FieldName
		{
			get;
			private set;
		}

		public bool NotifyPropertyChanged
		{
			get;
			private set;
		}

		public InspectorDisplayAttribute(string fieldName = "value", bool notifyPropertyChanged = true)
		{
			this.FieldName = fieldName;
			this.NotifyPropertyChanged = notifyPropertyChanged;
		}
	}
}
