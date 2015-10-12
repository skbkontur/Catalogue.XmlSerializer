using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace SKBKontur.Catalogue.XmlSerializer.Reading
{
    public class ReportReader : IReportReader
    {
        public ReportReader(IContentReaderCollection contentReaderCollection)
        {
            this.contentReaderCollection = contentReaderCollection;
        }

        public T Read<T>(byte[] xmlContent, bool needTrimValues)
        {
            using(var reader = XmlReader.Create(new MemoryStream(xmlContent), new XmlReaderSettings {CloseInput = true,}))
                return Read<T>(reader, needTrimValues);
        }

        public T Read<T>(XmlReader reader, bool needTrimValues)
        {
            return Read<T>(new SimpleXmlReader(reader, needTrimValues));
        }

        public T Read<T>(NameValueCollection collection)
        {
            if(collection == null || collection.Count == 0)
                throw new EmptyNameValueCollectionNotSupportedException();
            return Read<T>(new NameValueCollectionReader(collection));
        }

        private T Read<T>(IReader reader)
        {
            var contentReader = contentReaderCollection.Get<T>();
            var result = contentReader.Read(reader);
            return result;
        }

        private readonly IContentReaderCollection contentReaderCollection;
    }
}