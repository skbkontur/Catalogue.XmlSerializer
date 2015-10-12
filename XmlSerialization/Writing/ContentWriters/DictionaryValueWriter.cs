using System.Collections;
using System.Collections.Generic;

using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Writing.ContentWriters
{
    public class DictionaryValueWriter : ValueWriterBase
    {
        public DictionaryValueWriter(XmlElementInfo xmlElementInfo, IContentWriter keyWriter, IContentWriter valueWriter, IXmlAttributeInterpretator xmlAttributeInterpretator)
        {
            itemContentWriter = new PairContentWriter(keyWriter, valueWriter, xmlAttributeInterpretator);
            arrayValueWriter = new ArrayValueWriter(xmlElementInfo, itemContentWriter);
        }

        protected override void WriteNonNullableValue(object value, IWriter writer)
        {
            var dictionary = (IDictionary)value;
            var list = new List<DictionaryKeyValuePair>();
            foreach(var key in dictionary.Keys)
            {
                var pair = new DictionaryKeyValuePair {Key = key, Value = dictionary[key]};
                list.Add(pair);
            }
            arrayValueWriter.Write(list.ToArray(), writer);
        }

        private readonly IContentWriter itemContentWriter;
        private readonly ArrayValueWriter arrayValueWriter;
    }
}