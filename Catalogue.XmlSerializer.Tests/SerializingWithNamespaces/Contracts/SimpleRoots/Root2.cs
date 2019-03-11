using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    [XmlNamespace(Namespaces.Namespace1, true)]
    [DeclareXmlNamespace("ns1", Namespaces.Namespace1)]
    public class Root2
    {
        public string A { get; set; }

        [XmlNamespace(Namespaces.Namespace2)]
        public BClass2 B { get; set; }
    }
}