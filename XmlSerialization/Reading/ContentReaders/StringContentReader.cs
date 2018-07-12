using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class StringContentReader : IContentReader<string>
    {
        public string Read(IReader reader)
        {
            return reader.ReadStringValue();
        }
    }
}