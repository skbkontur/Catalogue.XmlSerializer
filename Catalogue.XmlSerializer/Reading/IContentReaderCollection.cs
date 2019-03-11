using SkbKontur.Catalogue.XmlSerializer.Reading.ContentReaders;

namespace SkbKontur.Catalogue.XmlSerializer.Reading
{
    public interface IContentReaderCollection
    {
        IContentReader<T> Get<T>();
    }
}