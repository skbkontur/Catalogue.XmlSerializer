using System;
using System.Collections.Generic;
using System.Xml;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public class SimpleXmlReader : AbstractReader
    {
        static SimpleXmlReader()
        {
            var xmlNodeType = typeof(XmlNodeType);
            var xmlNodeTypeNames = Enum.GetNames(xmlNodeType);
            var xmlNodeTypeValues = Enum.GetValues(xmlNodeType);
            var map = new Dictionary<string, int>();
            var max = 0;
            for (var i = 0; i < xmlNodeTypeNames.Length; ++i)
            {
                var value = (int)xmlNodeTypeValues.GetValue(i);
                map.Add(xmlNodeTypeNames[i], value);
                if (value > max) max = value;
            }
            if (max > (1 << 20)) throw new NotSupportedException();
            transformTable = new int[max + 1];
            for (var i = 0; i < transformTable.Length; ++i)
                transformTable[i] = -1;
            var nodeType = typeof(NodeType);
            var nodeTypeNames = Enum.GetNames(nodeType);
            var nodeTypeValues = Enum.GetValues(nodeType);
            for (var i = 0; i < nodeTypeNames.Length; ++i)
                transformTable[map[nodeTypeNames[i]]] = (int)nodeTypeValues.GetValue(i);
            goodNodeTypes = new bool[max + 1];
            goodNodeTypes[(int)XmlNodeType.Element] = true;
            goodNodeTypes[(int)XmlNodeType.CDATA] = true;
            goodNodeTypes[(int)XmlNodeType.EndElement] = true;
            goodNodeTypes[(int)XmlNodeType.Text] = true;
            goodNodeTypes[(int)XmlNodeType.Attribute] = true;
        }

        public SimpleXmlReader(XmlReader xmlReader, bool needTrimValues)
        {
            this.needTrimValues = needTrimValues;
            this.xmlReader = xmlReader;
            ReadWhileBadNodeType();
        }

        public override bool Read()
        {
            if (!xmlReader.Read()) return false;
            return ReadWhileBadNodeType();
        }

        public override string ReadRawData()
        {
            var result = xmlReader.ReadInnerXml().Replace("\n", "\r\n").Replace("\r\r", "\r");
            ReadWhileBadNodeType();
            return result;
        }

        public override bool MoveToFirstAttribute()
        {
            if (xmlReader.MoveToFirstAttribute()) return true;
            ReadWhileBadNodeType();
            return false;
        }

        public override bool MoveToNextAttribute()
        {
            if (xmlReader.MoveToNextAttribute()) return true;
            ReadWhileBadNodeType();
            return false;
        }

        public override void MoveToElement()
        {
            xmlReader.MoveToElement();
        }

        public override NodeType NodeType
        {
            get
            {
                /*NodeType result;
                if(Enum.TryParse(xmlReader.NodeType.ToString(), true, out result)) return result;*/
                var x = transformTable[(int)xmlReader.NodeType];
                if (x >= 0) return (NodeType)x;
                throw new XmlException("BUG");
            }
        }

        public override string Value => needTrimValues ? ValueTrimmer.Trim(xmlReader.Value) : xmlReader.Value;
        public override string LocalName => xmlReader.LocalName;
        public override bool HasAttributes => xmlReader.HasAttributes;
        public override string NamespaceURI => xmlReader.NamespaceURI;

        public override bool IsEmptyElement => xmlReader.IsEmptyElement;
        public override int Depth => xmlReader.Depth;

        private bool ReadWhileBadNodeType()
        {
            while (true)
            {
                if (goodNodeTypes[(int)xmlReader.NodeType])
                    return true;
                if (!xmlReader.Read())
                    return false;
            }
        }

        private static readonly int[] transformTable;
        private static readonly bool[] goodNodeTypes;

        private readonly XmlReader xmlReader;
        private readonly bool needTrimValues;
    }
}