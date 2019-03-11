using System.Collections.Specialized;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public interface IReader
    {
        bool Read();
        bool MoveToFirstAttribute();
        bool MoveToNextAttribute();
        void MoveToElement();
        string ReadElementString();
        string ReadRawData();
        NameValueCollection GetAttributes();
        string NamespaceURI { get; }
        int Depth { get; }
        NodeType NodeType { get; }
        string Value { get; }
        string LocalName { get; }
        bool HasAttributes { get; }
        bool IsEmptyElement { get; }
    }
}