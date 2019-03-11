using System;

using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class RootContentWriter : IContentWriter
    {
        public RootContentWriter(Type type, IContentWriterCollection contentWriterCollection, IXmlAttributeInterpreter xmlAttributeInterpreter)
        {
            contentWriter = contentWriterCollection.Get(type);
            rootElementInfo = xmlAttributeInterpreter.GetRootNodeInfo(type);
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