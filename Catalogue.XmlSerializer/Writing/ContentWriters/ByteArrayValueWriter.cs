using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ByteArrayValueWriter : ValueWriterBase
    {
        public ByteArrayValueWriter(XmlElementInfo xmlElementInfo)
        {
            this.xmlElementInfo = xmlElementInfo;
            contentWriter = new ByteArrayContentWriter();
        }

        protected override void WriteNonNullableValue(object value, IWriter writer)
        {
            writer.WriteStartElement(xmlElementInfo);
            contentWriter.Write(value, writer);
            writer.WriteEndElement();
        }

        private readonly XmlElementInfo xmlElementInfo;
        private readonly ByteArrayContentWriter contentWriter;
    }
}