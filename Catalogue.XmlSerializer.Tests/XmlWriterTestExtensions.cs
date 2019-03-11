using SkbKontur.Catalogue.XmlSerializer.Writing;

using XmlElementInfo = SkbKontur.Catalogue.XmlSerializer.Attributes.XmlElementInfo;

namespace Catalogue.XmlSerializer.Tests
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