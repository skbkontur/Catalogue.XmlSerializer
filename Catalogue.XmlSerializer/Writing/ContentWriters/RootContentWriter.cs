using System;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class RootContentWriter : IContentWriter
    {
        public RootContentWriter(Type type, IContentWriterCollection contentWriterCollection, IXmlAttributeInterpretator xmlAttributeInterpretator)
        {
            contentWriter = contentWriterCollection.Get(type);
            rootElementInfo = xmlAttributeInterpretator.GetRootNodeInfo(type);
        }

        public void Write(object obj, IWriter writer)
        {
            writer.WriteStartElement(rootElementInfo);
            contentWriter.Write(obj, writer);
            writer.WriteEndElement();
        }

        private readonly IContentWriter contentWriter;
        private readonly XmlElementInfo rootElementInfo;
    }
}