using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleClassWithManyNamespaces
{
    [DeclareXmlNamespace("a", "urn:d")]
    [DeclareXmlNamespace("b", "urn:c")]
    [DeclareXmlNamespace("c", "urn:b")]
    [DeclareXmlNamespace("d", "urn:a")]
    public class RootWithManyNamespaces
    {
        [XmlNamespace("urn:a")]
        public string A { get; set; }

        [XmlNamespace("urn:b")]
        public string B { get; set; }

        [XmlNamespace("urn:c")]
        public string C { get; set; }

        [XmlNamespace("urn:d")]
        public string D { get; set; }
    }
}