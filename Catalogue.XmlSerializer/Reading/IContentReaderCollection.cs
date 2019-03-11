using Catalogue.XmlSerializer.Reading.ContentReaders;

namespace Catalogue.XmlSerializer.Reading
{
    public interface IContentReaderCollection
    {
        IContentReader<T> Get<T>();
    }
}