using System;

using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class EnumContentReader<T> : IContentReader<T> where T : struct
    {
        public T Read(IReader reader)
        {
            var value = reader.ReadStringValue();
            T result;
            return !Enum.TryParse(value, true, out result) ? default(T) : result;
        }
    }
}