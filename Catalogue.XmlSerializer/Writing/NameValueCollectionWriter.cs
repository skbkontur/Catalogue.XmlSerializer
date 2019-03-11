using System.Collections.Generic;
using System.Collections.Specialized;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing
{
    public class NameValueCollectionWriter : IWriter
    {
        public NameValueCollectionWriter()
        {
            currentPath = new List<KeyValuePair<string, bool>>();
            nameValueCollection = new NameValueCollection();
            namespacesStack = new Stack<string>();
        }

        public void WriteStartElement(XmlElementInfo xmlElementInfo)
        {
            Add(GetXmlElementString(xmlElementInfo));
            namespacesStack.Push(GetUrl(xmlElementInfo));
        }

        public void WriteStartArrayElement(XmlElementInfo xmlElementInfo, int index)
        {
            Add($"{GetXmlElementString(xmlElementInfo)}${index}");
            namespacesStack.Push(GetUrl(xmlElementInfo));
        }

        public void WriteStartAttribute(XmlElementInfo xmlElementInfo)
        {
            Add($"{GetXmlElementString(xmlElementInfo)}${"Attr"}");
            namespacesStack.Push(GetUrl(xmlElementInfo));
        }

        public void WriteEndElement()
        {
            namespacesStack.Pop();
            if (currentPath.Count == 1 && currentPath[0].Value)
                return; //к классе ничего не заполнено
            Remove();
        }

        public void WriteEndAttribute()
        {
            namespacesStack.Pop();
            Remove();
        }

        public void WriteValue(object obj)
        {
            if (obj != null)
            {
                currentPath[currentPath.Count - 1] = new KeyValuePair<string, bool>(currentPath[currentPath.Count - 1].Key, true);
                value = ValueToString(obj);
            }
        }

        public void WriteRawData(string data)
        {
            currentPath[currentPath.Count - 1] = new KeyValuePair<string, bool>(currentPath[currentPath.Count - 1].Key, true);
            value = data;
        }

        public void Dispose()
        {
        }

        public NameValueCollection GetResult()
        {
            return nameValueCollection;
        }

        private string GetUrl(XmlElementInfo xmlElementInfo)
        {
            var currentUrl = xmlElementInfo == null ? null : xmlElementInfo.NamespaceUri;
            var topUrl = namespacesStack.Count > 0 ? namespacesStack.Peek() : "";
            return string.IsNullOrEmpty(currentUrl) ? topUrl : currentUrl;
        }

        private string GetXmlElementString(XmlElementInfo xmlElementInfo)
        {
            var url = GetUrl(xmlElementInfo);
            var topUrl = namespacesStack.Count > 0 ? namespacesStack.Peek() : "";
            if (string.IsNullOrEmpty(url) || url == topUrl)
                return xmlElementInfo.Name;
            return "[" + url + "]" + xmlElementInfo.Name;
        }

        private void Add(string name)
        {
            if (currentPath.Count > 0)
                currentPath[currentPath.Count - 1] = new KeyValuePair<string, bool>(currentPath[currentPath.Count - 1].Key, false);
            currentPath.Add(new KeyValuePair<string, bool>(name, true));
        }

        private void Remove()
        {
            if (currentPath[currentPath.Count - 1].Value)
            {
                nameValueCollection.Add(GetCurrentName(), value);
                value = null;
            }
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        private string GetCurrentName()
        {
            var result = currentPath[1].Key;
            for (var i = 2; i < currentPath.Count; i++)
                result += "." + currentPath[i].Key;
            return result;
        }

        private static string ValueToString(object value)
        {
            return value.ToString();
        }

        private readonly List<KeyValuePair<string, bool>> currentPath;
        private readonly NameValueCollection nameValueCollection;
        private string value;
        private readonly Stack<string> namespacesStack;
    }
}