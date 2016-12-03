using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract]
	public class Model_Base
	{
		public static void SetMaster<T>(out T instance, XElement element) where T : Model_Base, new()
		{
			instance = Activator.CreateInstance<T>();
			instance.setProperty(element);
			instance.setArrayItems();
		}

		public static T SetUserData<T>(XElement element) where T : Model_Base, new()
		{
			T result = Activator.CreateInstance<T>();
			result.setProperty(element);
			result.setArrayItems();
			return result;
		}

		protected virtual void setProperty(XElement element)
		{
		}

		protected virtual void setArrayItems()
		{
		}
	}
}
