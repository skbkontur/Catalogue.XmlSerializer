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
            if (value == null)
                return;

            var array = (Array)value;
            for (var i = 0; i < array.Length; ++i)
            {
                var arrayItem = array.GetValue(i);
                var itemValueWriter = new ArrayItemValueWriter(xmlElementInfo, itemContentWriter, i);
                itemValueWriter.Write(arrayItem, writer);
            }
        }

        private readonly XmlElementInfo xmlElementInfo;
        private readonly IContentWriter itemContentWriter;
    }
}