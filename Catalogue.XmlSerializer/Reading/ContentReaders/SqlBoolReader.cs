using SKBKontur.Catalogue.XmlSerialization.Attributes;

namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
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