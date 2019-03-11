using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Catalogue.XmlSerializer.Reading
{
    public class NameValueCollectionReader : AbstractReader
    {
        public NameValueCollectionReader(NameValueCollection collection)
        {
            root = new TreeVertex("GlobalRoot");
            var allKeys = collection.AllKeys.OrderBy(key => key, new NameValueCollectionKeyComparer());
            foreach (var key in allKeys)
                AddPath("Root." + key, collection[key]);
            cur = root.LeftSon;
        }

        public override bool Read()
        {
            if (root.NodeType == NodeType.EndElement) return false;
            switch (cur.NodeType)
            {
            case NodeType.Attribute:
                return false;
            case NodeType.None:
                return false;
            case NodeType.EndElement:
                if (cur.RightNeighbour != null) cur = cur.RightNeighbour;
                else if (cur.Parent != null)
                {
                    cur = cur.Parent;
                    cur.NodeType = NodeType.EndElement;
                }
                else return false;
                return true;
            case NodeType.Text:
                cur.NodeType = NodeType.EndElement;
                return true;
            case NodeType.Element:
                if (!cur.IsEmptyElement)
                {
                    if (cur.LeftSon == null) cur.NodeType = NodeType.Text;
                    else cur = cur.LeftSon;
                    return true;
                }
                cur.NodeType = NodeType.EndElement;
                return Read();
            default:
                return false;
            }
        }

        public override string ReadRawData()
        {
            if (root.NodeType == NodeType.EndElement) return null;
            string result = null;
            switch (cur.NodeType)
            {
            case NodeType.Text:
                result = cur.Value;
                Read();
                break;
            case NodeType.Attribute:
                result = cur.Value;
                break;
            case NodeType.EndElement:
                Read();
                break;
            case NodeType.Element:
                Read();
                if (NodeType != NodeType.Text)
                    throw new InvalidOperationException("Unable to read raw data");
                result = cur.Value;
                Read();
                Read();
                break;
            }
            return result;
        }

        public override bool MoveToFirstAttribute()
        {
            return cur.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return cur.MoveToNextAttribute();
        }

        public override void MoveToElement()
        {
            cur.MoveToElement();
        }

        public override string NamespaceURI => cur.Url;

        public override bool IsEmptyElement => cur.IsEmptyElement;
        public override bool HasAttributes => cur.Attributes != null;

        public override NodeType NodeType
        {
            get
            {
                if (root.NodeType == NodeType.EndElement) return NodeType.None;
                return cur.NodeType;
            }
        }

        public override string Value => ValueTrimmer.Trim(cur.Value);
        public override string LocalName => cur.Name;
        public override int Depth => cur.Depth;

        private static bool IsAttribute(string name)
        {
            return name.EndsWith("$Attr");
        }

        private string[] SplitPath(string path)
        {
            var result = new List<string>();
            for (var i = 0; i < path.Length; i++)
            {
                var list = new List<char>();
                if (path[i] == '[')
                {
                    while (path[i] != ']')
                        list.Add(path[i++]);
                    while (i < path.Length && path[i] != '.')
                        list.Add(path[i++]);
                }
                else
                {
                    while (i < path.Length && path[i] != '.')
                        list.Add(path[i++]);
                }
                result.Add(new string(list.ToArray()));
            }
            return result.ToArray();
        }

        private void AddPath(string path, string value)
        {
            var name = SplitPath(path);
            var it = root;
            for (var i = 0; i < name.Length; i++)
            {
                if (!IsAttribute(name[i]))
                {
                    var treeVertex = new TreeVertex(name[i]);
                    if (it.RightSon == null || it.RightSon.FullName != treeVertex.FullName)
                        it.AddSon(treeVertex);
                    it = it.RightSon;
                }
                else
                {
                    if (i != name.Length - 1)
                        throw new InvalidOperationException($"Атрибут {name[i]} не последний в цепочке '{path}'");
                }
            }
            var lastName = name[name.Length - 1];
            if (IsAttribute(lastName))
                it.AddAttribute(lastName, value);
            else it.AddText(value);
        }

        private readonly TreeVertex root;
        private TreeVertex cur;

        private class XmlAttribute
        {
            public XmlAttribute(string namespaceURI, string name, string value)
            {
                NamespaceURI = namespaceURI;
                Name = name;
                Value = value;
            }

            public string NamespaceURI { get; private set; }
            public string Name { get; private set; }
            public string Value { get; private set; }
        }

        private class TreeVertex
        {
            public TreeVertex(string fullName)
            {
                if (fullName[0] == '[')
                {
                    var pos = fullName.IndexOf(']');
                    url = fullName.Substring(1, pos - 1);
                    FullName = fullName.Substring(pos + 1);
                }
                else
                    FullName = fullName;
                NodeType = NodeType.Element;
                depth = 0;
            }

            public override string ToString()
            {
                var attrs = "";
                if (Attributes != null)
                {
                    for (var i = 0; i < Attributes.Count; i++)
                    {
                        if (i == 0) attrs += "[";
                        if (i != 0) attrs += "; ";
                        attrs += $"{Attributes[i].NamespaceURI}:{Attributes[i].Name}={Attributes[i].Value}";
                        if (i == Attributes.Count - 1) attrs += "]";
                    }
                }
                return string.Format(FullName + attrs + ": " + value);
            }

            public void AddAttribute(string fullName, string attrValue)
            {
                string name, attrUrl = null;
                if (fullName[0] == '[')
                {
                    var pos = fullName.IndexOf(']');
                    attrUrl = fullName.Substring(1, pos - 1);
                    name = fullName.Substring(pos + 1);
                }
                else
                    name = fullName;
                if (Attributes == null)
                    Attributes = new List<XmlAttribute>();
                var attribute = new XmlAttribute(attrUrl, GetShortName(name), attrValue);
                Attributes.Add(attribute);
                return;
            }

            public void AddText(string value)
            {
                this.value = value;
            }

            public void AddSon(TreeVertex son)
            {
                son.depth = depth + 1;
                son.Parent = this;
                if (LeftSon == null)
                    LeftSon = RightSon = son;
                else
                {
                    RightSon.RightNeighbour = son;
                    RightSon = son;
                }
            }

            public bool MoveToFirstAttribute()
            {
                if (Attributes == null) return false;
                attributeIndex = 0;
                NodeType = NodeType.Attribute;
                return true;
            }

            public bool MoveToNextAttribute()
            {
                if (Attributes == null) return false;
                if (NodeType != NodeType.Attribute) return false;
                if (attributeIndex + 1 >= Attributes.Count) return false;
                attributeIndex++;
                return true;
            }

            public bool MoveToElement()
            {
                if (NodeType != NodeType.Attribute) return false;
                NodeType = NodeType.Element;
                return true;
            }

            public TreeVertex Parent { get; private set; }
            public TreeVertex LeftSon { get; private set; }
            public TreeVertex RightSon { get; private set; }
            public TreeVertex RightNeighbour { get; private set; }
            public List<XmlAttribute> Attributes { get; private set; }
            public string FullName { get; private set; }
            public string Url => NodeType == NodeType.Attribute ? Attributes[attributeIndex].NamespaceURI : GetUrl();
            public string Name => NodeType == NodeType.Attribute ? Attributes[attributeIndex].Name : GetShortName(FullName);
            public string Value => NodeType == NodeType.Attribute ? Attributes[attributeIndex].Value : value;
            public NodeType NodeType { get; set; }
            public int Depth => NodeType == NodeType.Text ? depth + 1 : depth;
            public bool IsEmptyElement => LeftSon == null && Value == null;

            private string GetUrl()
            {
                return url ?? (url = (Parent == null ? "" : Parent.GetUrl()));
            }

            private static string GetShortName(string fullName)
            {
                var pos = fullName.IndexOf('$');
                if (pos == -1) return fullName;
                return fullName.Substring(0, pos);
            }

            private string url;

            private int attributeIndex;

            private int depth;
            private string value;
        }
    }
}