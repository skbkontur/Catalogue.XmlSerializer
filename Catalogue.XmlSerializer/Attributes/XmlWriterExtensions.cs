using System.Linq;
using System.Xml;

namespace SkbKontur.Catalogue.XmlSerializer.Attributes
{
    public static class XmlWriterExtensions
    {
        public static void WriteStartElement(this XmlWriter writer, XmlElementInfo xmlElementInfo)
        {
            if (string.IsNullOrEmpty(xmlElementInfo.NamespaceUri))
                writer.WriteStartElement(xmlElementInfo.Name);
            else
            {
                var xmlNamespaceDescription = xmlElementInfo.NamespaceDescriptions?.FirstOrDefault(x => x.Uri == xmlElementInfo.NamespaceUri);
                if (xmlNamespaceDescription != null)
                    writer.WriteStartElement(xmlNamespaceDescription.Prefix, xmlElementInfo.Name, xmlElementInfo.NamespaceUri);
                else
                    writer.WriteStartElement(xmlElementInfo.Name, xmlElementInfo.NamespaceUri);
            }

            if (xmlElementInfo.NamespaceDescriptions != null)
            {
                foreach (var namespaceDescription in xmlElementInfo.NamespaceDescriptions)
                    writer.WriteAttributeString("xmlns", namespaceDescription.Prefix, null, namespaceDescription.Uri);
            }
        }

        public static void WriteStartAttribute(this XmlWriter writer, XmlElementInfo xmlElementInfo)
        {
            if (string.IsNullOrEmpty(xmlElementInfo.NamespaceUri))
                writer.WriteStartAttribute(xmlElementInfo.Name);
            else
                writer.WriteStartAttribute(xmlElementInfo.Name, xmlElementInfo.NamespaceUri);
        }
    }
}