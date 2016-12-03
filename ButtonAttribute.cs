using System;
using UnityEngine;

[AttributeUsage]
public sealed class ButtonAttribute : PropertyAttribute
{
	public string Function
	{
		get;
		private set;
	}

	public string Name
	{
		get;
		private set;
	}

	public object[] Parameters
	{
		get;
		private set;
	}

	public ButtonAttribute(string function, string name, params object[] parameters)
	{
		this.Function = function;
		this.Name = name;
		this.Parameters = parameters;
	}
}
