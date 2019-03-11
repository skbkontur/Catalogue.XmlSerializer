namespace Catalogue.XmlSerializer.Reading.ContentReaders
{
    public interface IContentReader<out T>
    {
        T Read(IReader reader);
    }
}