using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Server_Lib
{
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			bool isEmptyElement = reader.get_IsEmptyElement();
			reader.Read();
			if (isEmptyElement)
			{
				return;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			while (reader.get_NodeType() != 15)
			{
				reader.ReadStartElement("KeyValuePair");
				reader.ReadStartElement("Key");
				TKey tKey = (TKey)((object)xmlSerializer.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadStartElement("Value");
				TValue tValue = (TValue)((object)xmlSerializer2.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadEndElement();
				this.Add(tKey, tValue);
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			using (Dictionary<TKey, TValue>.KeyCollection.Enumerator enumerator = base.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TKey current = enumerator.get_Current();
					writer.WriteStartElement("KeyValuePair");
					writer.WriteStartElement("Key");
					xmlSerializer.Serialize(writer, current);
					writer.WriteEndElement();
					writer.WriteStartElement("Value");
					xmlSerializer2.Serialize(writer, this.get_Item(current));
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
			}
		}
	}
}
