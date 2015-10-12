using System;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Writing
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