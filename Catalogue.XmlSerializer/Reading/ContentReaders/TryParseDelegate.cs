using System;
using System.Globalization;

namespace SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public delegate bool TryParseDelegate<T>(string s, NumberStyles style, IFormatProvider provider, out T result);
}