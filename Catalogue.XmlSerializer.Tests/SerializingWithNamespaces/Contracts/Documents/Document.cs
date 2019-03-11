using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Tests.SerializingWithNamespaces.Contracts.Documents
{
    [XmlNamespace(Namespaces.MainNamespace)]
    [DeclareXmlNamespace(Namespaces.MainNamespacePrefix, Namespaces.MainNamespace)]
    [DeclareXmlNamespace(Namespaces.OneNamespacePrefix, Namespaces.OneNamespace)]
    [DeclareXmlNamespace(Namespaces.TwoNamespacePrefix, Namespaces.TwoNamespace)]
    [DeclareXmlNamespace(Namespaces.ThreeNamespacePrefix, Namespaces.ThreeNamespace)]
    public class Document<T>
    {
        public T Body { get; set; }
    }
}