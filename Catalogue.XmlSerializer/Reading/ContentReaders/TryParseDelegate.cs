using System;
using System.Globalization;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public delegate bool TryParseDelegate<T>(string s, NumberStyles style, IFormatProvider provider, out T result);
}