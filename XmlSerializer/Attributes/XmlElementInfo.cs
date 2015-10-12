namespace SKBKontur.Catalogue.XmlSerializer.Attributes
{
    public class XmlElementInfo
    {
        public string Name { get; set; }
        public bool Optional { get; set; }
        public string NamespaceUri { get; set; }
        public XmlNamespaceDescription[] NamespaceDescriptions { get; set; }
    }

    public class XmlNamespaceDescription
    {
        public XmlNamespaceDescription(string prefix, string uri)
        {
            Prefix = prefix;
            Uri = uri;
        }

        public string Prefix { get; private set; }
        public string Uri { get; private set; }
    }
}