using System;

namespace SKBKontur.Catalogue.XmlSerialization.Attributes
{
    public class XmlNamespaceAttribute : Attribute
    {
        public XmlNamespaceAttribute(string namespaceUri)
        {
            NamespaceUri = namespaceUri;
            IncludingAttributes = false;
        }

        public XmlNamespaceAttribute(string namespaceUri, bool includingAttributes)
        {
            NamespaceUri = namespaceUri;
            IncludingAttributes = includingAttributes;
        }

        public string NamespaceUri { get; set; }
        public bool IncludingAttributes { get; set; }
    }
}