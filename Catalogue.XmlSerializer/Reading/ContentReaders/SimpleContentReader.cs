using SkbKontur.Catalogue.XmlSerializer.Attributes;

namespace SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class SimpleContentReader<T> : IContentReader<T>
    {
        public SimpleContentReader(Helpers.TryParseDelegate<T> parseDelegate)
        {
            this.parseDelegate = parseDelegate;
        }

        public T Read(IReader reader)
        {
            var readString = reader.ReadStringValue();
            T value;
            if (!parseDelegate(readString, out value))
                value = default(T);
            return value;
        }

        private readonly Helpers.TryParseDelegate<T> parseDelegate;
    }
}