using System;

namespace SKBKontur.Catalogue.XmlSerializer.Attributes
{
    public class XmlNamespaceAttribute : Attribute
    {
        public XmlNamespaceAttribute(string namespaceUri)
        {
            NamespaceUri = namespaceUri;
        }

        public string NamespaceUri { get; set; }
    }
}