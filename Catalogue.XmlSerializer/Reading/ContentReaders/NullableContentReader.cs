namespace SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders
{
    public class NullableContentReader<T> : IContentReader<T?> where T : struct
    {
        public NullableContentReader(IContentReaderCollection contentReaderCollection)
        {
            contentReader = contentReaderCollection.Get<T>();
        }

        public T? Read(IReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.Read();
                return null;
            }
            return contentReader.Read(reader);
        }

        private readonly IContentReader<T> contentReader;
    }
}