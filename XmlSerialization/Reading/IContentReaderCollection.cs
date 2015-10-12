using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public interface IContentReaderCollection
    {
        IContentReader<T> Get<T>();
    }
}