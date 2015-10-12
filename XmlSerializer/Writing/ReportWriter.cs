using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

namespace SKBKontur.Catalogue.XmlSerializer.Writing
{
    public class ReportWriter : IReportWriter
    {
        public ReportWriter(IContentWriterCollection contentWriterCollection)
        {
            this.contentWriterCollection = contentWriterCollection;
        }

        public byte[] SerializeToBytes<T>(T data, bool omitXmlDeclaration, Encoding encoding)
        {
            var settings = new XmlWriterSettings
                {
                    NamespaceHandling = NamespaceHandling.OmitDuplicates,
                    OmitXmlDeclaration = omitXmlDeclaration,
                    Indent = true,
                    ConformanceLevel = ConformanceLevel.Document,
                    Encoding = encoding
                };
            var memoryStream = new MemoryStream();
            using(var xmlWriter = XmlWriter.Create(memoryStream, settings))
                Write(data, new CollapseWriter(new SimpleXmlWriter(xmlWriter)));
            return memoryStream.ToArray();
        }

        public string SerializeToString<T>(T data, bool omitXmlDeclaration, Encoding encoding)
        {
            return encoding.GetString(SerializeToBytes(data, omitXmlDeclaration, encoding));
        }

        public NameValueCollection SerializeToNameValueCollection<T>(T data, bool skipEmpty)
        {
            var innerWriter = new NameValueCollectionWriter();
            Write(data, skipEmpty ? new CollapseWriter(innerWriter) : (IWriter)innerWriter);
            return innerWriter.GetResult();
        }

        public void Serialize<T>(T data, IWriter writer)
        {
            Write(data, writer);
        }

        private void Write<T>(T data, IWriter writer)
        {
            var rootWriter = contentWriterCollection.GetRootWriter(typeof(T));
            rootWriter.Write(data, writer);
        }

        private readonly IContentWriterCollection contentWriterCollection;
    }
}