using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    [XmlNamespace(Namespaces.Namespace1)]
    [DeclareXmlNamespace("ns1", Namespaces.Namespace1)]
    public class Root3
    {
        [XmlElement("zzz")]
        public string A { get; set; }

        [XmlAttribute("qxx")]
        public string B { get; set; }

        [XmlAttribute("qxz")]
        [XmlNamespace(Namespaces.Namespace1)]
        public string C { get; set; }
    }
}