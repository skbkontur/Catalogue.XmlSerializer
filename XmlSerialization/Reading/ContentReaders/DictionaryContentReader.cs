using System.Collections.Generic;
using System.Linq;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class DictionaryContentReader<TKey, TValue> : IContentReader<Dictionary<TKey, TValue>>
    {
        public DictionaryContentReader(IContentReaderCollection contentReaderCollection)
        {
            arrayContentReader = new ArrayContentReader<DictionaryKeyValuePair<TKey, TValue>>(contentReaderCollection);
        }

        public Dictionary<TKey, TValue> Read(IReader reader)
        {
            var arr = arrayContentReader.Read(reader);
            return arr.ToDictionary(val => val.Key, val => val.Value);
        }

        private readonly ArrayContentReader<DictionaryKeyValuePair<TKey, TValue>> arrayContentReader;
    }
}