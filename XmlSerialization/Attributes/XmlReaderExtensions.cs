using SKBKontur.Catalogue.XmlSerialization.Reading;

namespace SKBKontur.Catalogue.XmlSerialization.Attributes
{
    public static class XmlReaderExtensions
    {
        public static string ReadStringValue(this IReader xmlReader)
        {
            return xmlReader.NodeType == NodeType.Attribute ? xmlReader.Value : xmlReader.ReadElementString();
        }
    }
}