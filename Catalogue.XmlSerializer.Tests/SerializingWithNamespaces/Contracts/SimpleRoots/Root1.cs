using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    [XmlNamespace(Namespaces.Namespace1, true)]
    public class Root1
    {
        public string A { get; set; }

        [XmlNamespace(Namespaces.Namespace2)]
        public BClass B { get; set; }
    }
}