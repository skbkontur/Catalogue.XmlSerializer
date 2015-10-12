using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

using SKBKontur.Catalogue.XmlSerialization.Reading;
using SKBKontur.Catalogue.XmlSerialization.Writing;

namespace SKBKontur.Catalogue.XmlSerialization
{
    public class XmlSerializer : IXmlSerializer
    {
        public XmlSerializer()
        {
            Configuration = new XmlSerializerConfiguration();
            var xmlAttributeInterpretator = new XmlAttributeInterpretator();
            reportReader = new ReportReader(new ContentReaderCollection(xmlAttributeInterpretator, Configuration.OnDeserialize));
            reportWriter = new ReportWriter(new ContentWriterCollection(xmlAttributeInterpretator));
        }

        public byte[] SerializeToBytes<T>(T data, bool omitXmlDeclaration, Encoding encoding)
        {
            return reportWriter.SerializeToBytes(data, omitXmlDeclaration, encoding);
        }

        public NameValueCollection SerializeToNameValueCollection<T>(T data, bool skipEmpty = true)
        {
            return reportWriter.SerializeToNameValueCollection(data, skipEmpty);
        }

        public void Serialize<T>(T data, IWriter writer)
        {
            reportWriter.Serialize(data, writer);
        }

        public T Deserialize<T>(XmlReader reader, bool needTrimValues)
        {
            return reportReader.Read<T>(reader, needTrimValues);
        }

        public T Deserialize<T>(NameValueCollection collection)
        {
            return reportReader.Read<T>(collection);
        }

        public T Deserialize<T>(byte[] source, bool needTrimValues = true)
        {
            using(var xmlReader = XmlReader.Create(new MemoryStream(source)))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }

        public T Deserialize<T>(Stream stream, bool needTrimValues = true)
        {
            using(var xmlReader = XmlReader.Create(stream))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }

        public XmlSerializerConfiguration Configuration { get; private set; }

        public T Deserialize<T>(string source, bool needTrimValues = true)
        {
            using(var xmlReader = XmlReader.Create(new StringReader(source)))
                return reportReader.Read<T>(xmlReader, needTrimValues);
        }

        private readonly ReportReader reportReader;
        private readonly ReportWriter reportWriter;
    }
}