using System;

namespace SKBKontur.Catalogue.XmlSerialization.Attributes
{
    public class XmlNamespaceAttribute : Attribute
    {
        public XmlNamespaceAttribute(string namespaceUri)
        {
            NamespaceUri = namespaceUri;
            UseForChildProperties = false;
        }

        public XmlNamespaceAttribute(string namespaceUri, bool useForChildProperties)
        {
            NamespaceUri = namespaceUri;
            UseForChildProperties = useForChildProperties;
        }

        public string NamespaceUri { get; set; }
        public bool UseForChildProperties { get; set; }
    }
}