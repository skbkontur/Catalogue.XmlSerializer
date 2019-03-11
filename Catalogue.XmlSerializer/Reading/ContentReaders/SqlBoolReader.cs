using Catalogue.XmlSerializer.Attributes;

namespace Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class SqlBoolReader : IContentReader<bool>
    {
        public bool Read(IReader reader)
        {
            var s = reader.ReadStringValue();
            return s == "1";
        }
    }
}