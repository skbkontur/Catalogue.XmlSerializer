﻿namespace SKBKontur.Catalogue.XmlSerializer
{
    public class DictionaryKeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }

    public class DictionaryKeyValuePair : DictionaryKeyValuePair<object, object>
    {
    }
}