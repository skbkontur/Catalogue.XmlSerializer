namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public interface IContentReader<out T>
    {
        T Read(IReader reader);
    }
}