using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    [XmlNamespace(Namespaces.Namespace1, true)]
    public class BClass
    {
        public string C { get; set; }
    }
}