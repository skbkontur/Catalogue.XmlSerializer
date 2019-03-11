using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class ArrayItemValueWriter : ValueWriterBase
    {
        public ArrayItemValueWriter(XmlElementInfo itemNodeInfo, IContentWriter contentWriter, int index)
        {
            this.itemNodeInfo = itemNodeInfo;
            this.contentWriter = contentWriter;
            this.index = index;
        }

        protected override void WriteNonNullableValue(object value, IWriter writer)
        {
            writer.WriteStartArrayElement(itemNodeInfo, index);
            contentWriter.Write(value, writer);
            writer.WriteEndElement();
        }

        private readonly XmlElementInfo itemNodeInfo;
        private readonly IContentWriter contentWriter;
        private readonly int index;
    }
}