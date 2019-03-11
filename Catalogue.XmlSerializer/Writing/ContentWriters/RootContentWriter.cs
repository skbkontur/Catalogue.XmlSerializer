using System;

using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Writing.ContentWriters
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