﻿using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class ItemValueWriter : IValueWriter
    {
        public ItemValueWriter(XmlElementInfo itemNodeInfo, IContentWriter contentWriter)
        {
            this.itemNodeInfo = itemNodeInfo;
            this.contentWriter = contentWriter;
        }

        public void Write(object value, IWriter writer)
        {
            writer.WriteStartElement(itemNodeInfo);
            contentWriter.Write(value, writer);
            writer.WriteEndElement();
        }

        private readonly XmlElementInfo itemNodeInfo;
        private readonly IContentWriter contentWriter;
    }
}