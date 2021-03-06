using System.Collections.Specialized;
using System.Xml;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public abstract class AbstractReader : IReader
    {
        public NameValueCollection GetAttributes()
        {
            if (NodeType != NodeType.Element) throw new XmlException($"Атрибуты можно вычитывать только у узла с типом Element. Тип '{NodeType}' не годится.");
            if (!HasAttributes) return new NameValueCollection();
            var collection = new NameValueCollection();
            MoveToFirstAttribute();
            collection.Add(LocalName, Value);
            while (MoveToNextAttribute())
                collection.Add(LocalName, Value);
            return collection;
        }

        public string ReadElementString()
        {
            string result = null;
            if (NodeType == NodeType.Attribute) MoveToElement();
            if (NodeType != NodeType.Element || (NodeType == NodeType.Element && !IsEmptyElement))
            {
                Read();
                while (NodeType != NodeType.EndElement)
                {
                    if (NodeType == NodeType.Text || NodeType == NodeType.CDATA) result = Value;
                    if (NodeType == NodeType.Element) throw new XmlException($"У элемента, который должен был быть простым, оказались дочерние элементы ({LocalName})");
                    if (!Read()) return null;
                }
            }
            Read();
            return result;
        }

        public abstract string NamespaceURI { get; }
        public abstract bool IsEmptyElement { get; }
        public abstract int Depth { get; }
        public abstract bool Read();
        public abstract string ReadRawData();
        public abstract NodeType NodeType { get; }
        public abstract string Value { get; }
        public abstract string LocalName { get; }
        public abstract bool MoveToFirstAttribute();
        public abstract bool MoveToNextAttribute();
        public abstract void MoveToElement();
        public abstract bool HasAttributes { get; }
    }
}