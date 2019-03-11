using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    [XmlNamespace(Namespaces.Namespace1, true)]
    [DeclareXmlNamespace("ns3", Namespaces.Namespace3)]
    public class BClass2
    {
        public CClass C { get; set; }
    }
}