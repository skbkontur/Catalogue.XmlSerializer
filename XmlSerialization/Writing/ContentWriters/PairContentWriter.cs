using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class PairContentWriter : ContentWriterBase
    {
        public PairContentWriter(IContentWriter keyContentWriter, IContentWriter valueContentWriter, IXmlAttributeInterpretator xmlAttributeInterpretator)
        {
            this.keyContentWriter = keyContentWriter;
            this.valueContentWriter = valueContentWriter;
            keyNodeInfo = xmlAttributeInterpretator.GetPropertyNodeInfo(typeof(DictionaryKeyValuePair).GetProperty("Key"));
            valueNodeInfo = xmlAttributeInterpretator.GetPropertyNodeInfo(typeof(DictionaryKeyValuePair).GetProperty("Value"));
        }

        protected override void WriteNonNullableObject(object obj, IWriter writer)
        {
            var pair = (DictionaryKeyValuePair)obj;
            writer.WriteStartElement(keyNodeInfo);
            keyContentWriter.Write(pair.Key, writer);
            writer.WriteEndElement();
            writer.WriteStartElement(valueNodeInfo);
            valueContentWriter.Write(pair.Value, writer);
            writer.WriteEndElement();
        }

        private readonly IContentWriter keyContentWriter;
        private readonly IContentWriter valueContentWriter;
        private readonly XmlElementInfo keyNodeInfo;
        private readonly XmlElementInfo valueNodeInfo;
    }
}