using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    public class CClass
    {
        [XmlNamespace(Namespaces.Namespace3)]
        public string D { get; set; }
    }
}