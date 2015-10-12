using System.Collections.Generic;
using System.Linq;

namespace SKBKontur.Catalogue.XmlSerializer.Reading.ContentReaders
{
    public class ListContentReader<T> : IContentReader<List<T>>
    {
        public ListContentReader(IContentReaderCollection contentReaderCollection)
        {
            arrayContentReader = new ArrayContentReader<T>(contentReaderCollection);
        }

        public List<T> Read(IReader reader)
        {
            return arrayContentReader.Read(reader).ToList();
        }

        private readonly ArrayContentReader<T> arrayContentReader;
    }
}