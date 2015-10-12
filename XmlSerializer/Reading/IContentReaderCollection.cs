using SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerializer.Reading
{
    public interface IContentReaderCollection
    {
        IContentReader<T> Get<T>();
    }
}