using System;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public interface IWriter : IDisposable
    {
        void WriteStartAttribute(XmlElementInfo xmlElementInfo);
        void WriteStartElement(XmlElementInfo xmlElementInfo);
        void WriteStartArrayElement(XmlElementInfo xmlElementInfo, int index);
        void WriteEndElement();
        void WriteEndAttribute();
        void WriteValue(object value);
        void WriteRawData(string data);
    }
}