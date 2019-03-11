using System.Collections;
using System.Collections.Generic;

using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace SkbKontur.Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class DictionaryValueWriter : ValueWriterBase
    {
        public DictionaryValueWriter(XmlElementInfo xmlElementInfo, IContentWriter keyWriter, IContentWriter valueWriter, IXmlAttributeInterpreter xmlAttributeInterpreter)
        {
            itemContentWriter = new PairContentWriter(keyWriter, valueWriter, xmlAttributeInterpreter);
            arrayValueWriter = new ArrayValueWriter(xmlElementInfo, itemContentWriter);
        }

        protected override void WriteNonNullableValue(object value, IWriter writer)
        {
            var dictionary = (IDictionary)value;
            var list = new List<DictionaryKeyValuePair>();
            foreach (var key in dictionary.Keys)
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