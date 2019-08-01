using System;

using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ArrayValueWriter : ValueWriterBase
    {
        public ArrayValueWriter(XmlElementInfo xmlElementInfo, IContentWriter itemContentWriter)
        {
            this.xmlElementInfo = xmlElementInfo;
            this.itemContentWriter = itemContentWriter;
        }

        protected override void WriteNonNullableValue(object value, IWriter writer)
        {
            var array = (Array)value;
            for (var i = 0; i < array.Length; ++i)
            {
                var arrayItem = array.GetValue(i);
                writer.WriteStartArrayElement(xmlElementInfo, i);
                itemContentWriter.Write(arrayItem, writer);
                writer.WriteEndElement();
            }
        }

        private readonly XmlElementInfo xmlElementInfo;
        private readonly IContentWriter itemContentWriter;
    }
}