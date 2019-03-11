using System;

using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Reading.ContentReaders
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