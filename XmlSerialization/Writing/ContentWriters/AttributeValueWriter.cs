using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class AttributeValueWriter : IValueWriter
    {
        public AttributeValueWriter(XmlElementInfo xmlElementInfo, IContentWriter contentWriter)
        {
            this.xmlElementInfo = xmlElementInfo;
            this.contentWriter = contentWriter;
        }

        public void Write(object value, IWriter writer)
        {
            writer.WriteStartAttribute(xmlElementInfo);
            contentWriter.Write(value, writer);
            writer.WriteEndAttribute();
        }

        private readonly XmlElementInfo xmlElementInfo;
        private readonly IContentWriter contentWriter;
    }
}