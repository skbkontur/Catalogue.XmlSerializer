using SKBKontur.Catalogue.XmlSerialization.Attributes;
using SKBKontur.Catalogue.XmlSerialization.Writing;

namespace SKBKontur.Catalogue.XmlSerializer.Tests
{
    public static class XmlWriterTestExtensions
    {
        public static void WriteStartElement(this IWriter writer, string name)
        {
            writer.WriteStartElement(new XmlElementInfo {Name = name});
        }

        public static void WriteStartAttribute(this IWriter writer, string name)
        {
            writer.WriteStartAttribute(new XmlElementInfo {Name = name});
        }
    }
}