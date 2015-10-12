using System;

namespace SKBKontur.Catalogue.XmlSerialization.Attributes
{
    public class DeclareXmlNamespaceAttribute : Attribute
    {
        public DeclareXmlNamespaceAttribute(string prefix, string uri)
        {
            Prefix = prefix;
            Uri = uri;
        }

        public string Prefix { get; set; }
        public string Uri { get; set; }
    }
}