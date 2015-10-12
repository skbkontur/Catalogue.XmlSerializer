using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class StringContentReader : IContentReader<string>
    {
        public string Read(IReader reader)
        {
            return reader.ReadStringValue();
        }
    }
}