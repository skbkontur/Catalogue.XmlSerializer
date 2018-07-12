using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents
{
    [XmlNamespace(Namespaces.OneNamespace)]
    public class Body1
    {
        public string A { get; set; }
    }
}