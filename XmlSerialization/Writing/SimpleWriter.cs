using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public class SimpleWriter : IWriter
    {
        public void Dispose()
        {
        }

        public void WriteStartAttribute(XmlElementInfo xmlElementInfo)
        {
        }

        public void WriteStartElement(XmlElementInfo xmlElementInfo)
        {
        }

        public void WriteStartArrayElement(XmlElementInfo xmlElementInfo, int index)
        {
        }

        public void WriteEndElement()
        {
        }

        public void WriteEndAttribute()
        {
        }

        public void WriteValue(object value)
        {
            Value = value == null || ReferenceEquals(value, "") ? null : value.ToString();
        }

        public void WriteRawData(string data)
        {
            Value = string.IsNullOrEmpty(data) ? null : data;
        }

        public string Value { get; private set; }
    }
}