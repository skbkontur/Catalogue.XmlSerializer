using System;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
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
                if (arrayItem != null)
                {
                    var itemValueWriter = new ArrayItemValueWriter(xmlElementInfo, itemContentWriter, i);
                    itemValueWriter.Write(arrayItem, writer);
                }
            }
        }

        private readonly XmlElementInfo xmlElementInfo;
        private readonly IContentWriter itemContentWriter;
    }
}