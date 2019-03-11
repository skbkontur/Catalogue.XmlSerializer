using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents
{
    [XmlNamespace(Namespaces.ThreeNamespace)]
    public class Body3 : Body2
    {
        public string C { get; set; }
    }
}