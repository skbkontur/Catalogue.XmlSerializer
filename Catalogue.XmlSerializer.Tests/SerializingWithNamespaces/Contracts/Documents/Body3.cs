using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents
{
    [XmlNamespace(Namespaces.ThreeNamespace)]
    public class Body3 : Body2
    {
        public string C { get; set; }
    }
}