using System.Xml;

namespace SKBKontur.Catalogue.XmlSerializer.Attributes
{
    public static class XmlWriterExtensions
    {
        public static void WriteStartElement(this XmlWriter writer, XmlElementInfo xmlElementInfo)
        {
            if(string.IsNullOrEmpty(xmlElementInfo.NamespaceUri))
                writer.WriteStartElement(xmlElementInfo.Name);
            else
                writer.WriteStartElement(xmlElementInfo.Name, xmlElementInfo.NamespaceUri);
        }

        public static void WriteStartAttribute(this XmlWriter writer, XmlElementInfo xmlElementInfo)
        {
            if(string.IsNullOrEmpty(xmlElementInfo.NamespaceUri))
                writer.WriteStartAttribute(xmlElementInfo.Name);
            else
                writer.WriteStartAttribute(xmlElementInfo.Name, xmlElementInfo.NamespaceUri);
        }
    }
}