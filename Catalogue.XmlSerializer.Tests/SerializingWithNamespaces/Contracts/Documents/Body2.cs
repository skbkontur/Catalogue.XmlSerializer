using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents
{
    [XmlNamespace(Namespaces.TwoNamespace)]
    public class Body2 : Body1
    {
        public string B { get; set; }
    }
}