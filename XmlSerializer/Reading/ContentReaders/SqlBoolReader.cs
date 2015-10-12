using SKBKontur.Catalogue.XmlSerializer.Attributes;

namespace SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders
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