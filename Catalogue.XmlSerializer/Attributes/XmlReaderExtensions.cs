using Catalogue.XmlSerializer.Reading;

namespace Catalogue.XmlSerializer.Attributes
{
    public static class XmlReaderExtensions
    {
        public static string ReadStringValue(this IReader xmlReader)
        {
            return xmlReader.NodeType == NodeType.Attribute ? xmlReader.Value : xmlReader.ReadElementString();
        }
    }
}