using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents
{
    [XmlNamespace(Namespaces.TwoNamespace)]
    public class Body2 : Body1
    {
        public string B { get; set; }
    }
}