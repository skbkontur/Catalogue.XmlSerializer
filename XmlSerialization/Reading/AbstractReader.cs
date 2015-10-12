using System.Collections.Specialized;
using System.Xml;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public abstract class AbstractReader : IReader
    {
        public NameValueCollection GetAttributes()
        {
            if(NodeType != NodeType.Element) throw new XmlException(string.Format("Атрибуты можно вычитывать только у узла с типом Element. Тип '{0}' не годится.", NodeType));
            if(!HasAttributes) return new NameValueCollection();
            var collection = new NameValueCollection();
            MoveToFirstAttribute();
            collection.Add(LocalName, Value);
            while(MoveToNextAttribute())
                collection.Add(LocalName, Value);
            return collection;
        }

        public string ReadElementString()
        {
            string result = null;
            if(NodeType == NodeType.Attribute) MoveToElement();
            if(NodeType != NodeType.Element || (NodeType == NodeType.Element && !IsEmptyElement))
            {
                Read();
                while(NodeType != NodeType.EndElement)
                {
                    if(NodeType == NodeType.Text || NodeType == NodeType.CDATA) result = Value;
                    if(NodeType == NodeType.Element) throw new XmlException(string.Format("У элемента, который должен был быть простым, оказались дочерние элементы"));
                    if(!Read()) return null;
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