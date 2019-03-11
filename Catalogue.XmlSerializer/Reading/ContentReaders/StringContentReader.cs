using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class StringContentReader : IContentReader<string>
    {
        public string Read(IReader reader)
        {
            return reader.ReadStringValue();
        }
    }
}