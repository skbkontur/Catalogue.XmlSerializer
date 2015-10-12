using System.Collections;
using System.Linq;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ListValueWriter : ValueWriterBase
    {
        public ListValueWriter(XmlElementInfo xmlElementInfo, IContentWriter itemContentWriter)
        {
            arrayValueWriter = new ArrayValueWriter(xmlElementInfo, itemContentWriter);
        }

        protected override void WriteNonNullableValue(object value, IWriter writer)
        {
            arrayValueWriter.Write(((IList)value).Cast<object>().ToArray(), writer);
        }

        private readonly ArrayValueWriter arrayValueWriter;
    }
}