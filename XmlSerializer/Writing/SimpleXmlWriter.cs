using System;
using System.Xml;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Writing
{
    public class SimpleXmlWriter : IWriter
    {
        public SimpleXmlWriter(XmlWriter writer)
        {
            xmlWriter = writer;
        }

        public void Dispose()
        {
            ((IDisposable)xmlWriter).Dispose();
        }

        public void WriteStartAttribute(XmlElementInfo xmlElementInfo)
        {
            xmlWriter.WriteStartAttribute(xmlElementInfo);
        }

        public void WriteEndAttribute()
        {
            xmlWriter.WriteEndAttribute();
        }

        public void WriteStartElement(XmlElementInfo xmlElementInfo)
        {
            xmlWriter.WriteStartElement(xmlElementInfo);
        }

        public void WriteStartArrayElement(XmlElementInfo xmlElementInfo, int index)
        {
            xmlWriter.WriteStartElement(xmlElementInfo);
        }

        public void WriteEndElement()
        {
            xmlWriter.WriteEndElement();
        }

        public void WriteValue(object value)
        {
            if(value != null)
                xmlWriter.WriteValue(value);
        }

        public void WriteRawData(string data)
        {
            xmlWriter.WriteRaw(data);
        }

        private readonly XmlWriter xmlWriter;
    }
}