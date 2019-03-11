using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.SimpleRoots
{
    public class CClass
    {
        [XmlNamespace(Namespaces.Namespace3)]
        public string D { get; set; }
    }
}